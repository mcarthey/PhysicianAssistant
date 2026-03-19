# POC Quick Start

## Prerequisites
- .NET 10 SDK
- ngrok account (free)
- Twilio account with WhatsApp sandbox configured
- Anthropic API key
- PubMed NCBI API key (optional but recommended — free, 2 min at ncbi.nlm.nih.gov/account)

## Setup

1. Fill in `src/appsettings.Development.json`:
   - `Claude.ApiKey` — your Anthropic API key
   - `PubMed.ApiKey` — your free NCBI key (or leave blank, works without it at 3 req/sec)
   - `Twilio` section — your account SID and auth token

2. Start the app:
   ```
   cd src
   dotnet run
   ```

3. Start ngrok tunnel:
   ```
   ngrok http 5000
   ```
   (Check the port — it prints on startup)

4. Copy the ngrok forwarding URL and paste into Twilio WhatsApp sandbox webhook settings:
   ```
   https://YOUR_NGROK_URL/webhook/triage
   ```

5. Send a WhatsApp message to your Twilio sandbox number. Try something like:
   ```
   Hola, mi bebé tiene fiebre y llora mucho
   ```

## What to Look For
- Does Claude respond naturally in Spanish?
- Is the tone calm and appropriate for a worried parent at 2am?
- Does the emergency detection trigger correctly? (Try: "mi bebé no puede respirar")
- Is the end-to-end latency acceptable?
- Does the PubMed context improve the quality of responses?

## Health Check
```
GET http://localhost:5000/health
```

## Project Structure
Built on the SmsLlm pattern (E:\Documents\dev\SmsLlm). Same Twilio + Polly + DI architecture.
ClaudeService replaces OllamaService. PubMedService replaces ChromaDB vector search.
ConversationService is identical — in-memory, per-phone-number sessions with timeout.
