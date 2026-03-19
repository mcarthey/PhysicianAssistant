# Infanzia / Kezer-Lab — Internal Technical Proposal
## Learned Geek — Working Document

**Prepared by:** Mark McArthey  
**Date:** March 17, 2026 (rev 2 — Martin's Q&A responses incorporated)  
**Status:** Internal only — do not share with client  
**Purpose:** Full technical reference, architecture decisions, resolved questions, risk assessment, and pricing framework for the Infanzia engagement

---

## Project Summary

Dr. Martin Núñez is a Peruvian pediatrician and sole operator of an Infanzia® / Kezer-Lab franchise — a precision infant nutrition brand with EU-certified products manufactured in Germany. He was introduced through Karen's personal network and is a fully commercial engagement.

The project has two workstreams of very different complexity:

- **Workstream A — Infanzia Product Chatbot:** A WhatsApp AI assistant for the product line. Answers parent questions about Biomilk, Infabiotix, Infavit, and other products. Lower complexity, faster delivery, earlier revenue.
- **Workstream B — Physician After-Hours Patient Communication System:** An AI assistant that operates on behalf of pediatricians during off-hours. Parents contact via WhatsApp, the AI captures symptoms, assesses urgency, keeps parents calm, and queues everything for physician review. Complex, medically sensitive, potentially white-labelable across Latin America.

Martin was at Expo West (Anaheim) when he sent the process diagram — he has active US market interest, not just aspirational.

**Status as of March 17:** Martin has responded to all six scope questions in writing. Answers are detailed, clinically precise, and reveal a more sophisticated vision than initially scoped. Key implications are documented below.

---

## Martin's Responses — Full Analysis

Martin submitted written answers to all six questions on March 17. His responses are summarized and analyzed here. The original document is on file.

---

### Q1 — Images: Analyze or Forward?

**Martin's answer:** He distinguishes three distinct image categories, each with different handling:

**Category A — Radiology (X-ray, CT, MRI, ultrasound)**
- AI analysis depends on more than one image; requires direct integration with HIS/RIS/PACS systems
- Notes that today's AI is often more accurate than specialist physicians in radiology interpretation
- His recommendation: defer to a future phase
- **Decision: OUT of current scope. Flag for Phase 2 expansion.**

**Category B — Camera/photo (dermatology, trauma, secretions, fluids)**
- Parents frequently send these types of images
- AI interpretation is acceptable and generally accurate
- Mandatory disclaimer required: *"Debe usted tener necesariamente la opinión de su médico tratante o especialista"*
- **Decision: IN scope for MVP. AI analyzes with hardcoded disclaimer. Price separately.**

**Category C — Lab results (numeric values)**
- Lab result numbers are a meaningful, analyzable input
- AI can use these to provide more specific guidance
- **Decision: IN scope for MVP. Parse and interpret numeric lab values. Factor into urgency assessment.**

**Scope implication:** Image handling is now three sub-problems, not one. Category B adds vision model integration and liability language. Category C requires structured parsing of numeric data. Budget both separately from the base system.

---

### Q2 — Impresión Dx.: How Should It Be Framed?

**Martin's answer:** He makes a precise clinical distinction that matters for both implementation and liability:

- "Impresión diagnóstica" in medicine implies a *substantiated and verifiable analysis* — it is not a rough guess or symptom summary
- It is categorically different from the "diagnóstico definitivo," which belongs exclusively to the treating physician
- The physician audit/validation process that follows is *indispensable*

**What this means:** Martin is comfortable with — and expects — the AI to produce a proper clinical impression. He is using the term correctly and deliberately. The key flow is: AI produces the impression → physician validates or overrides → validation is mandatory, not optional.

**Implementation guidance:**
- The `impresion_dx` field should be presented as a substantiated AI analysis, not a vague hypothesis
- Suggested label: *"Impresión diagnóstica IA: [output] — Requiere validación del médico tratante"*
- Physician cannot close/archive an urgent or emergency case without completing the VoBo
- This term and its precise meaning must be explicitly defined in the engagement letter

---

### Q3 — VoBo: How Does It Work in Practice?

**Martin's answer:** He provides an important nuance on validation scope:

- Physician must validate what the AI worked and suggested — this validation improves AI training over time
- **Not required on every single interaction**
- **Mandatory for urgent / emergency cases** — no exceptions
- For routine (non-urgent) cases: statistically significant sampling is sufficient — he cites Pareto 80/20

**Implementation guidance:**
- Dashboard: urgent/emergency cases are locked until VoBo is completed
- Routine cases: system presents a sampling prompt periodically (e.g., "You have 12 unreviewed routine cases — review a sample?")
- VoBo data feeds back into the AI training loop — this is a feature, not just compliance
- Mechanism: dashboard web interface (not WhatsApp)
- Sampling rate (e.g., 20% of routine cases) to be confirmed with Martin in Discovery

---

### Q4 — Patient Records: Single Conversation or Ongoing History?

**Martin's answer:** Longitudinal records, with significant monetization implications he has already thought through:

- System must recognize patients by their ID
- Must connect with conversation history to provide more specific responses
- **Monetization thinking:** physician pays for the system; but storing patient history opens the possibility of charging parents/guardians monthly for data storage and historical data download
- Further vision: linking with hospital and health center records (databases, labs, imaging, rehabilitation centers)

**What this means:** Martin is thinking about a longitudinal health record platform with a B2C layer on top — a significantly larger vision than the initial scope. Build the data model correctly now so these paths are not blocked. Do not build B2C or EHR integration in Phase 1, but do not architect something that makes them impossible.

**Data model note:** The `patients` table needs a stable patient ID persisting across contacts from the same WhatsApp number. Edge case: multiple guardians contacting from different numbers for the same child — flag for Discovery.

---

### Q5 — Voice Responses: Required from the Start?

**Martin's answer:** Text is sufficient to begin.

**Decision: Voice output (ElevenLabs TTS) deferred to Phase 5.** System receives voice messages (STT transcription still required for incoming audio) but responds in text for MVP.

---

### Q6 — "Resultados *": What Does It Mean?

**Martin's answer:** Two-part clarification:

- Refers to results from evaluations or referrals ordered by the treating physician (lab work, specialist consultations, imaging)
- **Critical results** are a distinct subcategory — these cannot wait. Parent reporting a critical result must trigger the emergency pathway: contact physician immediately and go to ER

**Implementation guidance:**
- Add "critical lab result" to emergency detection logic — not just symptoms
- Parents should be able to describe or upload a result; system assesses whether it is within normal range or flagged critical
- Intersects with Q1 Category C (lab value parsing) — consistent design needed
- Critical numeric thresholds (hemoglobin, glucose, etc.) to be defined with Martin in Discovery

---

## Updated Scope Implications Summary

| Item | Previous Assumption | Confirmed Reality | Impact |
|---|---|---|---|
| Image handling | TBD — one question | Three categories; B and C in MVP, A deferred | Vision model + lab parser; price separately |
| Impresión Dx. | Cautious hypothesis | Full substantiated clinical impression | More capable output; stronger liability language |
| VoBo | All cases sign-off | Mandatory urgent/emergency; sampled routine | Dashboard workflow redesign; reduces physician burden |
| Patient records | TBD | Longitudinal by patient ID; B2C + EHR on roadmap | Larger data model; architecture must not block future |
| Voice output | TBD | Text-only for MVP | Removes ElevenLabs TTS from Phase 3; reduces cost |
| Resultados * | Lab results | Physician-ordered results + critical result emergency path | Critical results added to emergency detection |

---

## Architecture — Recommended Stack

### Core: Twilio + Claude API + ElevenLabs TTS (deferred)

```
Parent (WhatsApp)
      │
      ▼
Twilio WhatsApp
      │
      ▼
Learned Geek Webhook Backend (C# / ASP.NET Core)
      ├── Session manager (PostgreSQL-backed — per-number conversation state)
      ├── STT transcription (incoming voice → text)
      ├── Image router:
      │     ├── Category A (radiology) → store + forward, flag for future
      │     ├── Category B (photos) → vision model + mandatory disclaimer
      │     └── Category C (lab results) → numeric parser + urgency assessment
      ├── Critical result detector (runs alongside emergency keyword check)
      ├── Claude API — conversation logic + clinical impression generation
      │     └── System prompt (guardrails hardcoded)
      ├── PubMed E-utilities API — RAG context injection
      └── Structured JSON output parser
            │
            ▼
      Patient Data Warehouse (PostgreSQL)
            ├── Per-patient longitudinal records (stable patient ID)
            ├── Conversation history (threaded per patient)
            ├── Attachments (S3 — categorized by image type)
            ├── Lab values (structured numeric storage)
            ├── Clinical impressions (AI-generated, physician-validated)
            └── VoBo audit log (mandatory urgent; sampled routine)
                  │
                  ▼
      Physician Dashboard (Blazor)
            ├── Prioritized queue (EMG → urgent → routine)
            ├── Full transcript + image viewer + lab values
            ├── Impresión Dx. (AI clinical impression — validated/overridden)
            ├── VoBo workflow (locked urgent; sampling prompt routine)
            ├── Action tracking (called back / referred / no action)
            ├── System on/off toggle
            └── AI training feedback loop (VoBo outcomes → model improvement)
```

### Updated System Prompt

```
You are a compassionate pediatric health assistant operating on behalf of
[Physician Name]. You communicate in Spanish.

YOUR ONLY FUNCTIONS:
1. Greet the parent warmly and gather: child name, age, symptoms,
   medical history, and any results they mention
2. If an image is shared:
   - Photo/camera image: describe findings, always add disclaimer
   - Lab result values: assess against normal ranges, flag if critical
   - Radiology image: acknowledge receipt, tell parent doctor will review
3. Assess urgency — emergency indicators trigger IMMEDIATE redirect:
   [hardcoded symptom + critical result keyword list]
4. Generate a substantiated clinical impression based on symptoms,
   history, and lab values (for physician review only — never for parent)
5. Non-emergency → reassure, confirm physician follow-up
6. Close with structured summary

YOU ARE NOT THE TREATING PHYSICIAN. The impresión diagnóstica you
generate is for the physician's review and validation only.
Every interaction must include: "El médico tratante revisará y
validará esta información. Todas las decisiones clínicas son
exclusivamente suyas."

MANDATORY IMAGE DISCLAIMER (Category B):
"La interpretación de imágenes por IA es orientativa. Debe usted
tener necesariamente la opinión de su médico tratante o especialista."
```

### Updated Emergency Trigger List

**Symptom-based:**
- dificultad para respirar / no puede respirar / no respira
- convulsiones / convulsionando / temblores
- inconsciente / no responde / no despierta
- sangrado severo / mucha sangre
- labios azules / morado / cianosis
- fiebre en bebé menor de 3 meses
- golpe en la cabeza / traumatismo craneal

**Result-based (new — from Q6):**
- resultado crítico / valores críticos
- médico pidió llamar urgente / resultado urgente
- [specific numeric thresholds — to be defined with Martin in Discovery]

---

## Data Architecture — Updated Schema

```sql
patients (
  id UUID PRIMARY KEY,
  physician_id UUID REFERENCES physicians(id),
  whatsapp_numbers JSONB,          -- array: multiple guardians may contact
  full_name VARCHAR,
  date_of_birth DATE,
  first_seen TIMESTAMPTZ,
  last_seen TIMESTAMPTZ,
  ehr_patient_id VARCHAR           -- null until future EHR integration
)

conversations (
  id UUID PRIMARY KEY,
  patient_id UUID REFERENCES patients(id),
  started_at TIMESTAMPTZ,
  ended_at TIMESTAMPTZ,
  urgency_level VARCHAR,           -- emergency / urgent / non_urgent
  desenlace VARCHAR,               -- EMG / OBS
  impresion_dx TEXT,               -- AI clinical impression — physician eyes only
  impresion_dx_validated BOOLEAN,
  impresion_dx_override TEXT,      -- physician correction if overridden
  symptoms JSONB,
  lab_values JSONB,                -- structured numeric lab data
  summary TEXT,
  pubmed_refs JSONB,
  vobo_required BOOLEAN,           -- true for urgent/emergency; sampled for routine
  vobo_signed_at TIMESTAMPTZ,
  vobo_notes TEXT,
  physician_action VARCHAR         -- called_back / referred / no_action
)

messages (
  id UUID PRIMARY KEY,
  conversation_id UUID REFERENCES conversations(id),
  role VARCHAR,                    -- user / assistant
  content TEXT,
  sent_at TIMESTAMPTZ,
  has_attachment BOOLEAN
)

attachments (
  id UUID PRIMARY KEY,
  message_id UUID REFERENCES messages(id),
  s3_key VARCHAR,
  content_type VARCHAR,
  image_category VARCHAR,          -- radiology / camera / lab_result
  ai_analyzed BOOLEAN,
  ai_description TEXT,             -- Category B: vision model output
  lab_values_parsed JSONB,         -- Category C: structured numeric extraction
  disclaimer_shown BOOLEAN         -- Category B: mandatory disclaimer confirmed
)

physicians (
  id UUID PRIMARY KEY,
  name VARCHAR,
  whatsapp_number VARCHAR,
  system_active BOOLEAN DEFAULT false,
  enrolled_at TIMESTAMPTZ,
  vobo_sampling_rate FLOAT         -- e.g. 0.2 = 20% of routine cases
)
```

---

## Open Questions

### Resolved ✓

| # | Question | Answer |
|---|---|---|
| S001 | Image handling | Three categories: radiology deferred; photos analyzed with disclaimer; lab values parsed |
| S002 | Voice output for MVP? | Text only — voice deferred to Phase 5 |
| S003 | Impresión Dx. framing | Full substantiated clinical impression; mandatory physician validation |
| S004 | VoBo mechanism | Dashboard web interface; mandatory urgent/emergency; sampled routine |
| S005 | Resultados * | Physician-ordered results; critical results trigger emergency pathway |
| S006 | Longitudinal records | Yes — by patient ID; B2C and EHR integration on future roadmap |

### Still Open — Discovery Phase

| # | Question | Priority |
|---|---|---|
| D001 | What is Martin's budget range? | High — not yet discussed |
| D002 | Is physician network beyond Martin identified or aspirational? | High |
| D003 | Expo West — is US market active now? | High |
| D004 | Does franchise agreement restrict AI tool deployment? | Medium |
| D005 | Physician notification (SMS/push) when new conversation queued? | Medium |
| D006 | Languages beyond Spanish for MVP? | Medium |
| D007 | What is his timeline expectation? | High |
| D008 | Critical lab value thresholds — specific numeric triggers? | High — needed before emergency logic can be written |
| D009 | Multiple guardians per patient — identity resolution approach? | Medium |
| D010 | B2C parent subscription — Phase 1 or later? | Medium |
| D011 | VoBo sampling rate for routine cases? | Medium |

### Technical — Answer During POC

| # | Question |
|---|---|
| T001 | ~~Does Twilio WhatsApp sandbox behave consistently for testing?~~ **Answered — Yes, POC validated.** |
| T002 | ~~What is PubMed ESearch + EFetch combined latency mid-conversation?~~ **Answered — acceptable; POC validated.** |
| T003 | Which vision model gives best results for dermatology/trauma photos at acceptable cost? |
| T004 | Can Claude reliably parse lab result images and extract numeric values? |
| T005 | ~~Redis for session management or PostgreSQL-backed sessions?~~ **Decision: PostgreSQL-backed. Stack is C#/.NET — Redis adds unnecessary infrastructure.** |

---

## Risk Register — Updated

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Impresión Dx. misinterpreted as a diagnosis by parent or physician | Medium | Very High | Precise label, engagement letter language, physician onboarding |
| Category B image analysis inaccurate — AI misreads a photo | Medium | High | Mandatory disclaimer on every analysis; physician always validates |
| Critical lab thresholds not defined before build | Medium | High | Resolve D008 in Discovery before writing emergency logic |
| Ley 29733 compliance with longitudinal records + potential B2C layer | Medium | High | Legal review before launch; B2C layer needs separate sign-off |
| Multiple guardians per patient — identity confusion | Medium | Medium | Design deduplication in Discovery; MVP can default to one number = one patient |
| Scope creep into EHR integration before foundation is solid | Medium | High | Explicitly defer in engagement letter |
| WhatsApp Business API approval delays | Medium | Medium | Start at Phase 1 kickoff |
| Martin's budget below project scope | Unknown | High | Discover in next call; prepare tiered scope options |

---

## AI Accuracy and Hallucination Risk

This is one of the most important architectural considerations in the project and must be designed for explicitly — not treated as an edge case. Martin raised accuracy concerns in the initial meeting (his OpenEvidence reference was fundamentally about this), and the problem is real and well understood in the field. The technical term is **hallucination under retrieval failure**: the AI generates a plausible-sounding answer even when it has insufficient or no retrieved evidence to support it.

Both systems face this risk differently.

---

### Workstream A — Infanzia Product Chatbot

**The failure mode:** A parent asks about a dosage, ingredient, or product detail that isn't in the knowledge base. Rather than acknowledging the gap, the AI confabulates a plausible-sounding answer from its general training data.

**Mitigations:**

**Retrieval confidence thresholding.** Before Claude answers, the RAG layer checks whether retrieved documents match the query with sufficient semantic similarity. If the confidence score falls below a defined threshold, the system routes to a safe fallback response rather than allowing Claude to answer from general knowledge.

**Constrained system prompt.** Claude is explicitly instructed: *"Answer only from the provided product documentation. If the answer is not clearly supported by the documents, say so and offer to connect the parent with a representative."* This reduces but does not eliminate the problem — Claude can still rationalize that something is implied by the docs.

**Citation enforcement.** For factual claims (dosage amounts, ingredients, certifications), Claude must cite the specific document and passage. If it cannot cite, it cannot claim. Detectable and enforceable in output validation.

**Escalation path.** Any query the system cannot answer with sufficient confidence routes to: "I don't have that information — a representative will follow up with you." This is a feature, not a failure.

---

### Workstream B — Physician Communication System

**The risk is higher** and splits into two distinct failure modes:

**Failure mode 1 — No relevant PubMed results.** ESearch returns nothing or low-relevance results. Claude generates a clinical impression from its general medical training, which may be outdated, wrong for the Peruvian clinical context, or simply hallucinated.

**Failure mode 2 — Results retrieved but misapplied.** PubMed returns tangentially related abstracts. Claude over-generalizes and produces a confident-sounding clinical impression that the retrieved evidence doesn't actually support.

**Mitigations:**

**Explicit RAG fallback handling.** If PubMed returns zero results or all results fall below the confidence threshold, Claude is explicitly instructed: *"No relevant clinical evidence was retrieved. Do not generate a clinical impression. Capture symptoms and flag for physician review without an evidence-based assessment."* The `impresion_dx` field is left null or populated with "Insufficient evidence — physician review required."

**Source attribution enforcement.** Claude's clinical impression must cite specific PMIDs from the retrieved set. Output is parsed to verify every claim maps to a retrieved abstract. Responses that cannot be attributed are rejected and replaced with the null fallback.

**Temperature control.** Clinical impression generation runs at low temperature (0.2–0.3), trading fluency for factual conservatism. The right tradeoff in a medical context.

**Physician VoBo as the primary safeguard.** The VoBo validation loop is not just a compliance step — it is the most important accuracy safeguard in the system. The AI may confabulate; the physician catches it. The audit trail records when it happened. Over time the feedback loop improves the system's performance in specific query categories. Martin's insistence on physician validation is clinically sound for exactly this reason.

**Physician-curated fallback layer.** Over time, categories of queries where the AI consistently underperforms (low PMID relevance, high physician override rate) can be identified and routed to a "physician review only" pathway from the start — bypassing AI assessment entirely for those categories.

---

### Testing and Feedback Requirements

AI accuracy in this context cannot be validated in a lab environment alone. **Martin's active participation in testing is a project requirement, not a nice-to-have.** Specifically:

- During UAT, Martin and ideally one or two other physicians must run realistic parent scenarios through the system and review every AI-generated clinical impression
- Override rate tracking must be active from day one of testing — if the physician overrides more than ~20% of impressions, the system is not ready for production
- The emergency detection logic (both symptom-based and result-based) must be stress-tested with ambiguous inputs — cases that are close to but not clearly emergencies
- Image analysis (Category B) accuracy must be reviewed against real dermatology and trauma photos, not stock images
- A formal sign-off from Martin on accuracy acceptability is required before go-live — this should be documented and kept on file

This is a shared responsibility. We build the safeguards; Martin validates that they work in the clinical context he understands and we do not.

---

### Risk Register Additions

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| AI confabulates product information not in knowledge base | Medium | High | Confidence thresholding + citation enforcement + escalation path |
| AI generates clinical impression with no supporting PubMed evidence | Medium | Very High | RAG fallback + null impresion_dx + physician VoBo mandatory |
| AI misapplies retrieved PubMed abstracts to wrong clinical context | Medium | High | PMID attribution enforcement + low temperature + physician override tracking |
| Category B image analysis inaccurate in real clinical photos | Medium | High | Mandatory disclaimer + physician always reviews + UAT with real images |
| Accuracy not validated before go-live | Low | Very High | Formal physician sign-off required as go-live gate |

---

## Phasing Plan — Updated

### Phase 0 — POC (Internal, 1–2 weeks, not billable) ✓ COMPLETE
- ✅ Twilio → Claude API → PubMed RAG → structured JSON output — validated
- ✅ Bilingual (Spanish/English) triage conversation — validated
- ✅ File-backed conversation persistence per phone number — validated
- ✅ PubMed latency acceptable mid-conversation — validated
- Add: vision model test with sample Category B photo
- Add: lab value parsing test with sample numeric result

### Phase 1 — Discovery & Specification (2–3 weeks, billable)
- Resolve D001–D011
- Define critical lab value thresholds with Martin (D008)
- Map full physician workflow including VoBo sampling design
- Legal review — scope now includes longitudinal records + potential B2C
- Define liability boundaries for clinical impression and image analysis
- Detailed technical specification for client sign-off

### Phase 2 — Infanzia Product Chatbot (4–6 weeks)
- WhatsApp AI assistant for product line
- Knowledge base: all Infanzia products
- Admin interface (Blazor): document upload, conversation log, on/off toggle

### Phase 3 — Physician System Backend (7–9 weeks)
- Twilio + Claude API + PubMed RAG pipeline
- Image routing: Category A store/forward; Category B vision + disclaimer; Category C lab parser
- Critical result emergency detection
- Longitudinal patient data warehouse (PostgreSQL)
- Session management (PostgreSQL-backed)
- VoBo audit log with mandatory/sampling logic
- Clinical impression generation

### Phase 4 — Physician Dashboard (4–6 weeks, concurrent with Phase 3)
- Prioritized queue (EMG → urgent → routine)
- Transcript + image viewer + lab values panel
- Impresión Dx. with physician override
- VoBo workflow (locked urgent; sampling prompt routine)
- AI feedback loop — physician validations feed training data

### Phase 5 — Voice Integration (2–3 weeks, deferred)
- ElevenLabs TTS for Spanish voice output
- Activate after text-only MVP is validated in production

### Phase 6 — Future Platform (separate scoping)
- B2C parent subscription layer
- Hospital / EHR integration
- Radiology image analysis (HIS/RIS/PACS)
- Multi-tenant productization layer

### Phase 7 — Ongoing Retainer
- Hosting and infrastructure
- Knowledge base updates
- PubMed query tuning
- VoBo sampling rate adjustment

---

## Pricing Framework (Internal — Do Not Share)

| Deliverable | Est. Range | Notes |
|---|---|---|
| Discovery & Specification | $2,500 – $3,500 | Legal coordination, full spec doc |
| Infanzia Product Chatbot | $8,000 – $10,000 | Full build including admin interface |
| Physician System Backend | $16,000 – $20,000 | Twilio + Claude + PubMed + image analysis + lab parser + data warehouse |
| Physician Dashboard | $6,000 – $8,000 | VoBo workflow, sampling logic, queue management |
| Voice Integration (deferred) | $3,000 – $4,000 | Phase 5 — not in MVP |
| **Total MVP Build Estimate** | **$32,500 – $41,500** | Excluding voice and retainer |
| Monthly Retainer | $500 – $1,200/mo | Hosting + maintenance + KB updates + support |

**Separate future line items (do not include in current proposal):**
- Radiology integration (HIS/RIS/PACS) — significant complexity, separate scoping
- B2C parent subscription layer — separate scoping
- Hospital/EHR integration — separate scoping
- Multi-tenant productization layer — $8,000–12,000 when ready

**Internal note on budget:** Martin's budget has not been discussed. His sophisticated responses suggest he understands this is a serious investment — but do not assume. Prepare a tiered scope option: a leaner MVP (no image analysis, no lab parsing) for a lower entry point if needed.

---

## Productization Potential — Updated

Martin's Q4 response reveals he is already thinking about this as a platform: B2C subscriptions, EHR linking, multi-institutional data. He may be a co-visionary, not just a first client. The correct posture is to build the foundation correctly now (patient ID system, longitudinal records, clean data model) and keep all future paths open. A separate conversation about the platform vision is worth having once the initial engagement is underway.

---

## Immediate Next Steps

- [x] Receive and analyze Martin's written Q&A responses ✓
- [x] Build POC (Phase 0) — core pipeline validated (Twilio + Claude + PubMed RAG + bilingual + persistence) ✓
- [ ] Send Martin two meeting time options for Discovery call
- [ ] Register free NCBI API key: ncbi.nlm.nih.gov/account (2 min)
- [ ] Update POC to include Category B vision model test + lab value parsing
- [ ] Engage Peruvian legal contact — scope now broader (longitudinal + B2C potential)
- [ ] Prepare tiered scope options in case budget conversation is needed
- [ ] Execute engagement letter before any Phase 1 billable work begins
- [ ] Begin WhatsApp Business API application at Phase 1 kickoff

---

*Learned Geek — Internal document. Not for client distribution.*
