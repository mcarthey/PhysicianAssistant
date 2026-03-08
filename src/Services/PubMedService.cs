using System.Text.Json;
using Microsoft.Extensions.Options;
using PhysicianAssistant.Configuration;
using PhysicianAssistant.Models;

namespace PhysicianAssistant.Services;

public class PubMedService : IPubMedService
{
    private readonly HttpClient _httpClient;
    private readonly PubMedSettings _settings;
    private readonly ILogger<PubMedService> _logger;

    public PubMedService(
        HttpClient httpClient,
        IOptions<PubMedSettings> settings,
        ILogger<PubMedService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<ClinicalContext> GetClinicalContextAsync(string[] symptoms, CancellationToken cancellationToken = default)
    {
        if (symptoms.Length == 0)
        {
            return ClinicalContext.Empty;
        }

        try
        {
            // Build pediatric-focused query — OR between symptoms for broader matching.
            // Use spaces (not +) so Uri.EscapeDataString handles encoding correctly.
            var symptomTerms = symptoms.Length == 1
                ? $"{symptoms[0]}[tiab]"
                : $"({string.Join(" OR ", symptoms.Select(s => $"{s}[tiab]"))})";
            var query = $"{symptomTerms} AND (pediatric[tiab] OR child[tiab] OR infant[tiab]) AND 2015:2026[dp]";

            _logger.LogInformation("PubMed search query: {Query}", query);

            // Step 1: ESearch — get relevant PMIDs
            var searchParams = new Dictionary<string, string?>
            {
                ["db"] = "pubmed",
                ["term"] = query,
                ["retmax"] = _settings.MaxResults.ToString(),
                ["sort"] = "relevance",
                ["retmode"] = "json"
            };

            if (!string.IsNullOrEmpty(_settings.ApiKey))
                searchParams["api_key"] = _settings.ApiKey;
            if (!string.IsNullOrEmpty(_settings.Email))
                searchParams["email"] = _settings.Email;
            if (!string.IsNullOrEmpty(_settings.ToolName))
                searchParams["tool"] = _settings.ToolName;

            var searchUrl = $"{_settings.BaseUrl}esearch.fcgi?{BuildQueryString(searchParams)}";
            var searchResponse = await _httpClient.GetStringAsync(searchUrl, cancellationToken);
            var searchResult = JsonDocument.Parse(searchResponse);

            var idList = searchResult.RootElement
                .GetProperty("esearchresult")
                .GetProperty("idlist")
                .EnumerateArray()
                .Select(e => e.GetString()!)
                .ToArray();

            if (idList.Length == 0)
            {
                _logger.LogInformation("PubMed returned no results for query: {Query}", query);
                return ClinicalContext.Empty with { QueryUsed = query, RetrievedAt = DateTime.UtcNow };
            }

            _logger.LogInformation("PubMed returned {Count} PMIDs: {Pmids}", idList.Length, string.Join(", ", idList));

            // Step 2: EFetch — retrieve abstracts
            var fetchParams = new Dictionary<string, string?>
            {
                ["db"] = "pubmed",
                ["id"] = string.Join(",", idList),
                ["rettype"] = "abstract",
                ["retmode"] = "text"
            };

            if (!string.IsNullOrEmpty(_settings.ApiKey))
                fetchParams["api_key"] = _settings.ApiKey;
            if (!string.IsNullOrEmpty(_settings.Email))
                fetchParams["email"] = _settings.Email;
            if (!string.IsNullOrEmpty(_settings.ToolName))
                fetchParams["tool"] = _settings.ToolName;

            var fetchUrl = $"{_settings.BaseUrl}efetch.fcgi?{BuildQueryString(fetchParams)}";
            var abstracts = await _httpClient.GetStringAsync(fetchUrl, cancellationToken);

            _logger.LogInformation("PubMed abstracts retrieved, {Length} chars", abstracts.Length);

            return new ClinicalContext(
                Source: "PubMed E-utilities (NCBI/NLM)",
                Pmids: idList,
                Abstracts: abstracts,
                QueryUsed: query,
                RetrievedAt: DateTime.UtcNow
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PubMed API error");
            return ClinicalContext.Empty;
        }
    }

    private static string BuildQueryString(Dictionary<string, string?> parameters)
    {
        return string.Join("&", parameters
            .Where(p => p.Value is not null)
            .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value!)}"));
    }
}
