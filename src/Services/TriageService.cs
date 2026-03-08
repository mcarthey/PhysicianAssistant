using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PhysicianAssistant.Services;

public class TriageService : ITriageService
{
    private readonly IClaudeService _claudeService;
    private readonly IPubMedService _pubMedService;
    private readonly IConversationService _conversationService;
    private readonly ILogger<TriageService> _logger;

    private const string SystemPrompt = """
        You are a compassionate pediatric assistant helping parents when their doctor is unavailable.
        DETECT the language of the parent's message and ALWAYS reply in that same language.
        If the parent writes in English, reply in English. If in Spanish, reply in Spanish.

        CRITICAL — SMS LENGTH LIMIT: Your responses MUST be under 450 characters total.
        Be warm but concise. No bullet lists longer than 3 items. No emojis. No markdown formatting.

        Your ONLY functions are:
        1. Greet the parent warmly and ask about the child (name, age)
        2. Ask them to describe symptoms clearly
        3. Assess urgency based ONLY on what they describe
        4. If you detect ANY emergency indicator, direct them immediately to emergency services
           — DO NOT continue the conversation:
           - Difficulty breathing / not breathing
           - Seizures
           - Unconscious / unresponsive
           - Severe bleeding
           - Blue lips or skin discoloration
           - Won't wake up
           - High fever in infant under 3 months
        5. If NOT an emergency: calmly reassure the parent, confirm the doctor will review and contact them
        6. Summarize what was captured before closing

        You are NOT a doctor. You do not diagnose. You do not prescribe.
        You only capture information and keep parents calm.
        Always remind parents that the doctor will make all clinical decisions.

        ALWAYS end your response with a symptoms block (the parent will NOT see this).
        List any medical symptoms mentioned so far in English, comma-separated:
        ---SYMPTOMS---
        fever, cough
        ---END_SYMPTOMS---

        When you have enough information to close the conversation, include a triage JSON block
        (the parent will not see this either):
        ---TRIAGE_JSON---
        {
          "triage_level": "emergency | urgent | non_urgent",
          "child_name": "",
          "child_age": "",
          "symptoms": [],
          "summary": "",
          "recommended_action": ""
        }
        ---END_TRIAGE_JSON---
        """;

    private const string SystemPromptWithContext = """
        You are a compassionate pediatric assistant helping parents when their doctor is unavailable.
        DETECT the language of the parent's message and ALWAYS reply in that same language.
        If the parent writes in English, reply in English. If in Spanish, reply in Spanish.

        CRITICAL — SMS LENGTH LIMIT: Your responses MUST be under 450 characters total.
        Be warm but concise. No bullet lists longer than 3 items. No emojis. No markdown formatting.

        Your ONLY functions are:
        1. Greet the parent warmly and ask about the child (name, age)
        2. Ask them to describe symptoms clearly
        3. Assess urgency based ONLY on what they describe
        4. If you detect ANY emergency indicator, direct them immediately to emergency services
           — DO NOT continue the conversation
        5. If NOT an emergency: calmly reassure the parent, confirm the doctor will review and contact them
        6. Summarize what was captured before closing

        You are NOT a doctor. You do not diagnose. You do not prescribe.
        You only capture information and keep parents calm.
        Always remind parents that the doctor will make all clinical decisions.

        You have access to clinical context from PubMed. Use it to inform your understanding
        but NEVER cite articles or use technical language with parents.
        Communicate in simple, reassuring language appropriate for a worried parent.

        ALWAYS end your response with a symptoms block (the parent will NOT see this).
        List any medical symptoms mentioned so far in English, comma-separated:
        ---SYMPTOMS---
        fever, cough
        ---END_SYMPTOMS---

        When you have enough information to close the conversation, include a triage JSON block
        (the parent will not see this either):
        ---TRIAGE_JSON---
        {
          "triage_level": "emergency | urgent | non_urgent",
          "child_name": "",
          "child_age": "",
          "symptoms": [],
          "summary": "",
          "recommended_action": ""
        }
        ---END_TRIAGE_JSON---
        """;

    public TriageService(
        IClaudeService claudeService,
        IPubMedService pubMedService,
        IConversationService conversationService,
        ILogger<TriageService> logger)
    {
        _claudeService = claudeService;
        _pubMedService = pubMedService;
        _conversationService = conversationService;
        _logger = logger;
    }

    public async Task<string> ProcessMessageAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Triage message from {PhoneNumber}: {Message}", phoneNumber, message);

        var lowerMessage = message.Trim().ToLowerInvariant();
        if (lowerMessage is "reset" or "clear" or "nuevo")
        {
            _conversationService.ClearConversation(phoneNumber);
            return "Conversation reset. How can I help you? / Conversacion reiniciada. En que puedo ayudarle?";
        }

        try
        {
            _conversationService.AddMessage(phoneNumber, "Padre/Madre", message);

            var prompt = await BuildPromptAsync(phoneNumber, message, cancellationToken);

            var response = await _claudeService.GenerateAsync(prompt, prompt.Contains("[CLINICAL CONTEXT")
                ? SystemPromptWithContext
                : SystemPrompt, cancellationToken);

            // Strip system blocks from the response before sending to parent
            var parentResponse = StripSystemBlocks(response);

            _conversationService.AddMessage(phoneNumber, "Asistente", response);

            _logger.LogInformation("Triage response for {PhoneNumber}: {Response}", phoneNumber, parentResponse);

            return parentResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing triage message from {PhoneNumber}", phoneNumber);
            return "Sorry, I'm having difficulties right now. If this is an emergency, please call emergency services immediately. Otherwise, please try again shortly.";
        }
    }

    private async Task<string> BuildPromptAsync(string phoneNumber, string message, CancellationToken cancellationToken)
    {
        var promptBuilder = new StringBuilder();

        // Check conversation history for Claude-extracted symptoms from previous turns
        var conversationContext = _conversationService.GetConversationContext(phoneNumber);
        var previousSymptoms = ExtractSymptomsFromHistory(conversationContext);

        if (previousSymptoms.Length > 0)
        {
            _logger.LogInformation("Found symptoms from previous turns: {Symptoms}", string.Join(", ", previousSymptoms));
            try
            {
                var context = await _pubMedService.GetClinicalContextAsync(previousSymptoms, cancellationToken);
                if (context.HasResults)
                {
                    promptBuilder.AppendLine("[CLINICAL CONTEXT — internal use only, do not cite to parent]");
                    promptBuilder.AppendLine($"Source: {context.Source}");
                    promptBuilder.AppendLine($"PMIDs: {string.Join(", ", context.Pmids)}");
                    promptBuilder.AppendLine($"Evidence summary: {context.Abstracts}");
                    promptBuilder.AppendLine();
                    promptBuilder.AppendLine("Use this evidence to inform your response but communicate in simple,");
                    promptBuilder.AppendLine("reassuring language appropriate for a worried parent.");
                    promptBuilder.AppendLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "PubMed lookup failed, continuing without clinical context");
            }
        }

        // Add conversation history
        if (!string.IsNullOrEmpty(conversationContext))
        {
            promptBuilder.AppendLine("Previous conversation:");
            promptBuilder.AppendLine(conversationContext);
            promptBuilder.AppendLine();
        }

        promptBuilder.AppendLine($"Parent: {message}");
        promptBuilder.AppendLine();
        promptBuilder.AppendLine("Assistant:");

        return promptBuilder.ToString();
    }

    private static string[] ExtractSymptomsFromHistory(string conversationContext)
    {
        if (string.IsNullOrEmpty(conversationContext))
            return [];

        var symptoms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var matches = Regex.Matches(conversationContext, @"---SYMPTOMS---\s*(.+?)\s*---END_SYMPTOMS---", RegexOptions.Singleline);

        foreach (Match match in matches)
        {
            var symptomList = match.Groups[1].Value.Trim();
            foreach (var symptom in symptomList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                symptoms.Add(symptom);
            }
        }

        return [.. symptoms];
    }

    private static string StripSystemBlocks(string response)
    {
        // Strip ---SYMPTOMS--- block
        var symptomsStart = response.IndexOf("---SYMPTOMS---", StringComparison.Ordinal);
        if (symptomsStart >= 0)
        {
            var symptomsEnd = response.IndexOf("---END_SYMPTOMS---", symptomsStart, StringComparison.Ordinal);
            if (symptomsEnd >= 0)
            {
                response = string.Concat(
                    response.AsSpan(0, symptomsStart),
                    response.AsSpan(symptomsEnd + "---END_SYMPTOMS---".Length)
                ).Trim();
            }
        }

        // Strip ---TRIAGE_JSON--- block
        var triageStart = response.IndexOf("---TRIAGE_JSON---", StringComparison.Ordinal);
        if (triageStart >= 0)
        {
            response = response[..triageStart].Trim();
        }

        return response;
    }
}
