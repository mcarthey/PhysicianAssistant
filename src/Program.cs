using Polly;
using Polly.Extensions.Http;
using Twilio.TwiML;
using PhysicianAssistant.Configuration;
using PhysicianAssistant.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
builder.Services.Configure<ClaudeSettings>(builder.Configuration.GetSection(ClaudeSettings.SectionName));
builder.Services.Configure<PubMedSettings>(builder.Configuration.GetSection(PubMedSettings.SectionName));
builder.Services.Configure<ConversationSettings>(builder.Configuration.GetSection(ConversationSettings.SectionName));

// Polly policies for Claude API
var claudeSettings = builder.Configuration.GetSection(ClaudeSettings.SectionName).Get<ClaudeSettings>() ?? new ClaudeSettings();

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        onRetry: (outcome, timespan, retryAttempt, context) =>
        {
            Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
        });

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30),
        onBreak: (outcome, timespan) => Console.WriteLine($"Circuit breaker opened for {timespan.TotalSeconds}s"),
        onReset: () => Console.WriteLine("Circuit breaker reset"));

var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(claudeSettings.TimeoutSeconds));

// Register HttpClient for Claude API with Polly
builder.Services.AddHttpClient<IClaudeService, ClaudeService>()
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy)
    .AddPolicyHandler(timeoutPolicy);

// Register HttpClient for PubMed (simpler policies — it's fast and free)
builder.Services.AddHttpClient<IPubMedService, PubMedService>()
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(2, _ => TimeSpan.FromMilliseconds(500)));

// Register services
builder.Services.AddSingleton<IConversationService, ConversationService>();
builder.Services.AddScoped<ITriageService, TriageService>();

var app = builder.Build();

// Health check
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "PhysicianAssistant Triage POC",
    timestamp = DateTime.UtcNow
}));

// Triage webhook — Twilio WhatsApp messages arrive here
app.MapPost("/sms", async (HttpRequest request, ITriageService triageService, ILogger<Program> logger) =>
{
    var form = await request.ReadFormAsync();
    var incomingMessage = form["Body"].ToString();
    var fromNumber = form["From"].ToString();

    logger.LogInformation("WhatsApp message from {Number}: {Message}", fromNumber, incomingMessage);

    var responseText = await triageService.ProcessMessageAsync(fromNumber, incomingMessage);

    // Build TwiML response (same format for SMS and WhatsApp)
    var response = new MessagingResponse();
    response.Message(responseText);

    logger.LogInformation("Triage response to {Number}: {Response}", fromNumber, responseText);

    return Results.Content(response.ToString(), "application/xml");
});

app.Run();
