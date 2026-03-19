# POC Technical Specification
## Triage Agent + Webhook Integration
**Last Updated:** 2026-03-07 (rev 2 — PubMed API added, architecture options expanded)
**Status:** Ready to build
**Author:** Mark McArthey / Learn Geek LLC

---

## Objective

Validate the core agent + WhatsApp + webhook architecture before committing to full
project scope. This POC answers the critical unknowns:

1. Does the WhatsApp → Agent → Webhook flow work end-to-end?
2. What does the post-conversation webhook payload look like — is it structured and complete?
3. Can we inject custom PubMed RAG context mid-conversation?
4. Is the conditional triage logic (emergency vs. non-emergency) flexible enough?
5. **New:** Does ElevenLabs constrain us enough that Twilio is the better orchestration
   layer, with ElevenLabs used only for voice/TTS?

---

## Architecture Decision — CONFIRMED

**Decision: Option B — Twilio as Orchestrator + Claude API + ElevenLabs TTS Only**

```
WhatsApp → Twilio → Our Custom Agent Logic (Node.js)
                         ├── LLM (Claude API) — conversation + triage reasoning
                         ├── PubMed RAG — clinical context retrieval
                         ├── ElevenLabs TTS — voice synthesis only (response text, no PII)
                         └── Our DB — data capture + physician dashboard
```

**Why this was chosen over ElevenLabs as full orchestrator:**
- We own all conversation logic — fully productizable and white-labelable
- ElevenLabs never touches patient data — only receives AI response text for voice synthesis
- Critical for Ley 29733 compliance: we control exactly what data leaves Peru and where it goes
- Twilio has HIPAA-eligible services, established DPAs, Latin American presence
- Full control over triage branching — no workflow builder constraints
- Claude API is swappable for AWS Bedrock (São Paulo) if data residency becomes a hard requirement
- Existing Twilio framework reduces build effort — extension, not greenfield

**Data residency advantage:** In this architecture, patient health data flows through
Twilio (channel) and Anthropic (reasoning) only. ElevenLabs and PubMed never see PII.
If regional processing is required, Claude API can move to Bedrock (sa-east-1) and
the database can host in AWS São Paulo — no architecture change needed.

**Note on Workstream A (product chatbot):** The same Twilio + Claude architecture
applies. ElevenLabs full agent was considered for the simpler chatbot, but using
the same stack for both workstreams reduces maintenance burden and keeps the
data flow consistent for compliance.

---

## POC Scope

One conversation flow, one webhook endpoint, one live PubMed query, one ElevenLabs
voice response.

**In scope:**
- Twilio WhatsApp sandbox receiving parent message
- Claude API handling conversation logic + triage reasoning
- PubMed E-utilities API queried with extracted symptom keywords (real API call)
- ElevenLabs TTS generating Spanish voice response (text-first, voice second)
- ngrok tunnel exposing local webhook endpoint
- Webhook receiver logging full structured payload to JSON file
- Simulate what physician-facing data looks like

**Out of scope for POC:**
- Production WhatsApp Business account (sandbox is fine)
- Database storage (flat file logging is sufficient)
- Authentication
- Physician dashboard UI
- Multi-tenant support
- ElevenLabs full agent platform

---

## Stack

| Component | Technology | Purpose |
|---|---|---|
| WhatsApp channel | Twilio WhatsApp Sandbox | Message in/out |
| Local tunnel | ngrok | Expose local webhook |
| Conversation logic | Claude API (claude-sonnet-4-6) | Triage reasoning + NLU |
| Medical RAG | PubMed E-utilities API | Clinical evidence retrieval |
| Voice synthesis | ElevenLabs TTS API | Spanish voice output only |
| Webhook receiver | Node.js (Express) — reuse Twilio framework | Data capture |
| Data logging | JSON flat file → ./captured_conversations/ | Physician review sim |

---

## Conversation Logic — Claude API System Prompt

```
You are a compassionate pediatric health assistant helping parents when their
doctor is unavailable. You communicate in Spanish.

Your ONLY functions are:
1. Greet the parent warmly and ask about the child (name, age)
2. Ask them to describe the symptoms clearly
3. Assess urgency based ONLY on what they describe
4. If you detect ANY emergency indicators, immediately direct them to emergency
   services — do NOT continue the conversation:
   - Difficulty breathing / not breathing
   - Seizures / convulsions
   - Unconscious / unresponsive
   - Severe bleeding
   - Blue lips or skin discoloration
   - Not waking up
   - High fever in infant under 3 months
5. If NOT an emergency: reassure the parent calmly, confirm the doctor will
   review everything and contact them
6. Summarize what was captured before closing

YOU ARE NOT A DOCTOR. You do not diagnose. You do not prescribe.
You only capture information and keep parents calm.
Always remind parents that the doctor will make all clinical decisions.

At conversation end, return a structured JSON triage assessment:
{
  "triage_level": "emergency | urgent | non_urgent",
  "child_name": "",
  "child_age": "",
  "symptoms": [],
  "summary": "",
  "recommended_action": ""
}
```

---

## PubMed API Integration

### Overview
PubMed E-utilities is a free, open, public API from the US National Library of
Medicine. No signup wall. No geographic restrictions. No NPI. No licensing fee.
Covers 35+ million peer-reviewed biomedical citations — the same literature base
OpenEvidence draws from.

### Base URL
```
https://eutils.ncbi.nlm.nih.gov/entrez/eutils/
```

### API Key (free — register at ncbi.nlm.nih.gov/account)
- Without key: 3 requests/second
- With key: 10 requests/second
- One key per NCBI account, free, takes 2 minutes

### Core Endpoints

#### 1. ESearch — Search by symptom keywords → returns PMIDs
```
GET https://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi
  ?db=pubmed
  &term=fever+infant+management[tiab]+AND+pediatric[tiab]
  &retmax=5
  &sort=relevance
  &retmode=json
  &api_key=YOUR_KEY
```

**Search field modifiers for precision:**
- `[tiab]` — searches title and abstract (most useful for our queries)
- `[mh]`  — MeSH (Medical Subject Headings) — controlled vocabulary
- `[la]`  — language filter e.g. `english[la]` or `spanish[la]`
- `[dp]`  — date published e.g. `2020:2026[dp]` for recent guidelines only

**Example queries for triage use cases:**
```
# Fever in infant
fever+infant+management[tiab]+AND+pediatric[tiab]+AND+2018:2026[dp]

# Colic / abdominal pain
infant+colic+crying[tiab]+AND+pediatric[tiab]

# Respiratory distress (emergency)
respiratory+distress+infant[tiab]+AND+emergency[tiab]

# Diarrhea / dehydration
diarrhea+dehydration+child[tiab]+AND+pediatric[tiab]
```

#### 2. EFetch — Retrieve abstracts by PMID → returns text
```
GET https://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi
  ?db=pubmed
  &id=38291045,37845123,36901234
  &rettype=abstract
  &retmode=text
  &api_key=YOUR_KEY
```

#### 3. ESummary — Get metadata (title, journal, date) → for citations
```
GET https://eutils.ncbi.nlm.nih.gov/entrez/eutils/esummary.fcgi
  ?db=pubmed
  &id=38291045,37845123
  &retmode=json
  &api_key=YOUR_KEY
```

### POC Implementation

```javascript
const axios = require('axios');

const PUBMED_BASE = 'https://eutils.ncbi.nlm.nih.gov/entrez/eutils/';
const NCBI_API_KEY = process.env.NCBI_API_KEY;
const NCBI_EMAIL = process.env.NCBI_EMAIL; // required by NCBI policy

/**
 * Search PubMed for clinical context based on extracted symptoms
 * Called mid-conversation once symptoms are identified
 */
async function getClinicalContext(symptoms) {
  try {
    // Build focused pediatric query from symptom keywords
    const query = symptoms
      .map(s => `${s}[tiab]`)
      .join('+AND+')
      + '+AND+pediatric[tiab]'
      + '+AND+2018:2026[dp]'; // recent guidelines only

    // Step 1: ESearch — get top 3 relevant PMIDs
    const searchRes = await axios.get(`${PUBMED_BASE}esearch.fcgi`, {
      params: {
        db: 'pubmed',
        term: query,
        retmax: 3,
        sort: 'relevance',
        retmode: 'json',
        api_key: NCBI_API_KEY,
        email: NCBI_EMAIL,
        tool: 'infanzia-triage'
      }
    });

    const pmids = searchRes.data.esearchresult.idlist;
    if (!pmids.length) {
      return { source: 'PubMed', guidance: null, references: [] };
    }

    // Step 2: EFetch — retrieve abstracts
    const fetchRes = await axios.get(`${PUBMED_BASE}efetch.fcgi`, {
      params: {
        db: 'pubmed',
        id: pmids.join(','),
        rettype: 'abstract',
        retmode: 'text',
        api_key: NCBI_API_KEY,
        email: NCBI_EMAIL,
        tool: 'infanzia-triage'
      }
    });

    return {
      source: 'PubMed E-utilities (NCBI/NLM)',
      pmids,
      abstracts: fetchRes.data,       // injected into Claude context
      query_used: query,
      retrieved_at: new Date().toISOString()
    };

  } catch (err) {
    console.error('PubMed API error:', err.message);
    return { source: 'PubMed', error: err.message, abstracts: null };
  }
}
```

### How RAG Context Gets Injected Into Claude

```javascript
// After symptoms extracted from conversation, fetch clinical context
const clinicalContext = await getClinicalContext(extractedSymptoms);

// Inject as additional context in next Claude API call
const claudeResponse = await anthropic.messages.create({
  model: 'claude-sonnet-4-6',
  max_tokens: 1024,
  system: SYSTEM_PROMPT,
  messages: [
    ...conversationHistory,
    {
      role: 'user',
      content: `
        Parent message: ${parentMessage}

        [CLINICAL CONTEXT — internal use only, do not quote directly to parent]
        Source: ${clinicalContext.source}
        PMIDs: ${clinicalContext.pmids?.join(', ')}
        Evidence summary: ${clinicalContext.abstracts}
        
        Use this evidence to inform your response but communicate in plain,
        calm language appropriate for a worried parent.
      `
    }
  ]
});
```

---

## Webhook Receiver

### Twilio WhatsApp Incoming Message Handler

```javascript
const twilio = require('twilio');
const express = require('express');
const app = express();
app.use(express.urlencoded({ extended: false }));

// In-memory session store (Redis for production)
const sessions = new Map();

app.post('/webhook/triage', async (req, res) => {
  const {
    From,         // e.g. whatsapp:+51976714745
    Body,         // parent's text message
    MessageSid,
    ProfileName
  } = req.body;

  // Get or create session for this WhatsApp number
  const sessionKey = From;
  if (!sessions.has(sessionKey)) {
    sessions.set(sessionKey, {
      history: [],
      startedAt: new Date().toISOString(),
      physicianId: 'dr_nunez_001'
    });
  }

  const session = sessions.get(sessionKey);
  session.history.push({ role: 'user', content: Body });

  // Get Claude response with PubMed context if symptoms identified
  const { responseText, triageData, symptoms } = await processMessage(session);
  session.history.push({ role: 'assistant', content: responseText });

  // If conversation complete, save structured capture and clear session
  if (triageData) {
    await saveConversation(From, session, triageData, symptoms);
    sessions.delete(sessionKey);
  }

  // Reply via Twilio
  const twiml = new twilio.twiml.MessagingResponse();
  twiml.message(responseText);
  res.type('text/xml').send(twiml.toString());
});

app.listen(3000, () => console.log('Triage webhook listening on port 3000'));
```

### Structured Physician Review Output

```json
{
  "session_id": "whatsapp:+51976714745_2026-03-07T20:30:00Z",
  "physician_id": "dr_nunez_001",
  "captured_at": "2026-03-07T20:30:00Z",
  "channel": "whatsapp",
  "parent": {
    "name": "María García",
    "whatsapp": "+51976714745"
  },
  "child": {
    "name": "Lucas",
    "age": "8 meses"
  },
  "triage": {
    "level": "non_urgent",
    "symptoms": ["fiebre", "llanto persistente", "no come bien"],
    "summary": "Lactante de 8 meses con fiebre de 38.2°C y llanto desde hace 6 horas. Sin signos de alarma.",
    "recommended_action": "Monitorear temperatura. Médico revisará mañana 8am."
  },
  "clinical_context": {
    "source": "PubMed E-utilities (NCBI/NLM)",
    "pmids": ["38291045", "37845123"],
    "query_used": "fever[tiab]+AND+infant[tiab]+AND+pediatric[tiab]",
    "retrieved_at": "2026-03-07T20:28:30Z"
  },
  "full_transcript": [
    { "role": "assistant", "content": "Hola, soy el asistente del Dr. Núñez...", "ts": "20:25:00" },
    { "role": "user", "content": "Mi bebé tiene fiebre y llora mucho", "ts": "20:25:45" },
    { "role": "assistant", "content": "Entiendo, cuánto tiempo lleva con fiebre...", "ts": "20:25:46" }
  ],
  "status": "pending_physician_review",
  "physician_action": null,
  "physician_reviewed_at": null
}
```

---

## ElevenLabs TTS — Voice Only

In this architecture ElevenLabs handles voice synthesis only — not agent orchestration.
Best-in-class voice quality, fully swappable, no lock-in on the critical path.

```javascript
const { ElevenLabsClient } = require('elevenlabs');
const client = new ElevenLabsClient({ apiKey: process.env.ELEVENLABS_API_KEY });

/**
 * Convert Spanish text response to audio
 * Returns audio buffer for sending as WhatsApp voice message via Twilio
 */
async function synthesizeSpanishVoice(text) {
  const audio = await client.textToSpeech.convert(
    process.env.ELEVENLABS_VOICE_ID,
    {
      text,
      model_id: 'eleven_multilingual_v2', // best for Spanish
      voice_settings: {
        stability: 0.75,
        similarity_boost: 0.85
      }
    }
  );
  return audio;
}
```

**Voice selection notes for POC:**
- Test several Spanish/Latin American voices from ElevenLabs library
- Target: warm, calm, professional — appropriate for worried parents at 2am
- Voice cloning option (future): create a consistent branded "assistant" voice
- For text-only WhatsApp messages: ElevenLabs not required for POC
- For voice calls (future Twilio Voice integration): full TTS pipeline applies

**Recommended voices to test:**
- Search ElevenLabs voice library for `Spanish`, filter by `Latin American`
- Prefer female voices — research suggests more calming for health contexts
- Test with the actual triage script for naturalness assessment

---

## Environment Variables

```bash
# .env
# Twilio
TWILIO_ACCOUNT_SID=your_sid
TWILIO_AUTH_TOKEN=your_token
TWILIO_WHATSAPP_NUMBER=whatsapp:+14155238886  # sandbox number

# Anthropic
ANTHROPIC_API_KEY=your_claude_key

# PubMed (free from ncbi.nlm.nih.gov/account — do this now)
NCBI_API_KEY=your_free_pubmed_key
NCBI_EMAIL=mark@learnedgeek.com               # required by NCBI usage policy

# ElevenLabs (TTS only)
ELEVENLABS_API_KEY=your_key
ELEVENLABS_VOICE_ID=your_chosen_spanish_voice_id

# Runtime
PORT=3000
NODE_ENV=development
```

---

## ngrok Setup

```bash
# Install
brew install ngrok

# Authenticate (free account at ngrok.com)
ngrok config add-authtoken YOUR_TOKEN

# Start tunnel
ngrok http 3000

# Copy forwarding URL e.g. https://abc123.ngrok.io
# → Paste into Twilio WhatsApp sandbox webhook settings:
#   https://abc123.ngrok.io/webhook/triage
```

---

## POC Build Steps

1. **Pre-requisites (do these first — all free)**
   - [ ] Register free NCBI API key → ncbi.nlm.nih.gov/account (2 min)
   - [ ] Configure Twilio WhatsApp sandbox
   - [ ] Confirm ElevenLabs API key
   - [ ] Copy .env template, fill in values

2. **Webhook receiver (extend existing Twilio framework)**
   - [ ] Add POST /webhook/triage route
   - [ ] Wire up in-memory session store
   - [ ] Log raw Twilio payload to confirm structure

3. **Conversation manager**
   - [ ] Integrate Claude API with system prompt above
   - [ ] Maintain conversation history per session
   - [ ] Parse structured triage JSON from Claude at conversation end
   - [ ] Implement emergency detection path

4. **PubMed RAG integration**
   - [ ] Implement getClinicalContext() with real API calls
   - [ ] Test with sample symptom queries (fever, colic, diarrhea)
   - [ ] Measure latency (ESearch + EFetch combined)
   - [ ] Inject context into Claude prompt and verify quality improvement

5. **ElevenLabs TTS (optional for POC — text first)**
   - [ ] Implement synthesizeSpanishVoice()
   - [ ] Test 3-5 voice options with triage script
   - [ ] Assess latency for WhatsApp voice message delivery

6. **End-to-end test**
   - [ ] Send WhatsApp message to Twilio sandbox number
   - [ ] Walk through non-emergency conversation in Spanish
   - [ ] Walk through emergency conversation — verify immediate redirect
   - [ ] Confirm JSON saved to ./captured_conversations/
   - [ ] Read the output as if you were the physician reviewing overnight

7. **Document findings in 05-poc-results.md**

---

## Success Criteria

| Criteria | Pass Condition |
|---|---|
| WhatsApp message received via Twilio | ✓ Webhook fires, body parsed correctly |
| Claude responds naturally in Spanish | ✓ Appropriate tone, medically careful |
| Emergency path triggers correctly | ✓ Immediate redirect, no further questions |
| PubMed ESearch returns relevant PMIDs | ✓ Results match symptom query |
| EFetch retrieves readable abstracts | ✓ Text parseable, medically relevant |
| RAG context improves Claude response | ✓ Evidence-informed, grounded guidance |
| Structured triage JSON captured | ✓ All required fields populated |
| JSON saved and physician-readable | ✓ File written, makes sense on review |
| ElevenLabs voice sounds natural | ✓ Spanish, calm, appropriate accent |

---

## Questions to Answer During POC

**Architecture (Option B confirmed — validate in practice):**
- Does the Twilio → Claude API → response flow feel responsive enough for WhatsApp?
- Is in-memory session management sufficient or do we need Redis from day one?
- What is end-to-end latency: parent message → Claude reasoning → Twilio response?

**PubMed:**
- How relevant are returned abstracts for pediatric triage queries?
- What is combined ESearch + EFetch latency? Acceptable mid-conversation?
- Should we pre-index a curated subset of pediatric articles vs. querying live?
- Do abstracts alone provide enough context, or do we need full-text via PMC?

**ElevenLabs TTS:**
- Which voice ID gives the most natural Spanish/Latin American result?
- What is audio generation latency? Acceptable for WhatsApp voice messages?
- Is voice necessary for MVP or is text-only sufficient to validate the concept?

**Twilio:**
- Any WhatsApp sandbox limitations that differ from production?
- Message length limits for WhatsApp via Twilio?

---

## Next Steps After POC

- [ ] Document all findings in 05-poc-results.md
- [ ] Validate architecture decision (Option B confirmed) — note any friction points
- [ ] Review Martin's process diagram when received
- [ ] Update implementation plan with confirmed stack
- [ ] Prepare phased proposal document for Martin
- [ ] **Register NCBI API key now — ncbi.nlm.nih.gov/account (2 min, free)**
