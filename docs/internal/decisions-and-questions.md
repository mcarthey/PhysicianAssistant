# Infanzia — Decisions & Open Questions Log
**Last Updated:** 2026-03-07  
**Author:** Mark McArthey / Learn Geek LLC

---

## Architecture Decisions

| # | Decision | Rationale | Date |
|---|---|---|---|
| 001 | ~~ElevenLabs as primary agent platform~~ **SUPERSEDED by 007** | ~~Native WhatsApp, RAG, voice/text, multilingual, post-call webhooks — covers ~80% of needs out of the box~~ | 2026-03-07 |
| 002 | ngrok POC before any client commitment | Validates critical unknowns before scoping is finalized — same approach used successfully with Twilio | 2026-03-07 |
| 003 | Split into two workstreams (chatbot + physician system) | Different risk profiles, different timelines — chatbot is fast revenue, physician system needs proper discovery | 2026-03-07 |
| 004 | Paid discovery phase before physician system build | Too complex and too much regulatory/liability risk to quote blind | 2026-03-07 |
| 005 | AI captures and calms only — never diagnoses | Core liability protection for client and builder — non-negotiable, hardcoded into system prompt | 2026-03-07 |
| 006 | Custom webhook backend for data persistence | Our backend owns all conversation logic, storage, and physician dashboard — no vendor dependency on critical path | 2026-03-07 |
| 007 | Twilio + Claude API as orchestrator, ElevenLabs for TTS only | We own conversation logic. ElevenLabs never sees patient data (response text only). Twilio has HIPAA/DPA compliance story. Claude API swappable for Bedrock (São Paulo) if data residency required. Existing Twilio framework reduces build effort. | 2026-03-07 |
| 008 | PubMed + PAHO + physician-curated RAG (OpenEvidence ruled out) | OpenEvidence requires US NPI, enterprise API only, US-centric guidelines. PubMed is free/open, no geographic restrictions. Physician controls knowledge base. Geographically appropriate for Peru. | 2026-03-07 |
| 009 | Same stack (Twilio + Claude) for both workstreams | Consistent data flow for compliance, single codebase to maintain, same architecture validates for both use cases | 2026-03-07 |

---

## Open Questions

### Technical

| # | Question | Priority | Status |
|---|---|---|---|
| T001 | Does OpenEvidence have a public API accessible outside institutional networks? | High | **Closed** — No. Requires US NPI, enterprise-only. See OpenEvidence section below |
| T002 | ~~What does the actual ElevenLabs post-call webhook payload contain?~~ | ~~High~~ | **Closed** — N/A. Architecture changed to Twilio + Claude (decision 007). ElevenLabs used for TTS only. |
| T003 | ~~Does ElevenLabs support mid-conversation tool calls?~~ | ~~High~~ | **Closed** — N/A. We own conversation logic via Claude API. |
| T004 | ~~How flexible is ElevenLabs conditional conversation flow for triage branching?~~ | ~~High~~ | **Closed** — N/A. Full control via Claude API system prompt + our code. |
| T005 | Does WhatsApp integration require a separate WhatsApp Business account from Meta? | Medium | Open — Twilio WhatsApp sandbox for POC, production requires WhatsApp Business API approval |
| T006 | What are ElevenLabs TTS pricing tiers at expected voice message volume? | Low | Open — lower priority since TTS is optional for MVP (text-only WhatsApp sufficient) |
| T007 | ~~Can physician toggle system on/off via ElevenLabs API programmatically?~~ | ~~Medium~~ | **Closed** — N/A. Toggle implemented in our backend, no vendor dependency. |
| T008 | What is ElevenLabs TTS voice generation latency for Spanish WhatsApp voice messages? | Medium | Open — POC will answer |
| T009 | Can ElevenLabs voice be cloned/customized for a consistent brand voice? | Low | Open |
| T010 | What is end-to-end latency: parent WhatsApp message → Twilio → Claude API → response? | High | Open — POC will answer |
| T011 | Does Ley 29733 require patient data to remain within Peru, or is cross-border transfer with explicit consent sufficient? | High | Open — needs legal review. Architecture supports Bedrock (São Paulo) as fallback if residency required. |
| T012 | What DPAs does Twilio offer? What DPAs does Anthropic offer? Must be executed before production. | High | Open — research during discovery |

### Regulatory & Legal

| # | Question | Priority | Status |
|---|---|---|---|
| R001 | What does Peruvian Ley 29733 specifically require for health data capture via AI? | High | Open — needs legal review |
| R002 | Is parental consent via WhatsApp message legally sufficient in Peru? | High | Open — needs legal review |
| R003 | Does Martin need to register as a data processor under Peruvian law? | High | Open — needs legal review |
| R004 | Are there specific MINSA (Ministerio de Salud Peru) regulations for AI-assisted triage? | High | Open — needs research |
| R005 | What liability does Learn Geek LLC carry as the system builder? | High | Open — needs legal advice |
| R006 | Who pays for the Peruvian legal advisor review — Martin or Learn Geek? Must be clarified before engagement letter. If client expense, state it explicitly in the proposal. If bundled into discovery, the $2,500–$3,500 range may be too low. | High | Open |
| R007 | Engagement letter must include: explicit limitation of liability for Learn Geek LLC, statement that physician remains solely responsible for all clinical decisions, requirement that Martin carry professional liability insurance covering AI-assisted triage. Do not sign without these. | High | Open — draft before engagement |

### Business / Client

| # | Question | Priority | Status |
|---|---|---|---|
| B001 | What is Martin's budget range? | High | Open — not discussed in meeting |
| B002 | Is the physician network beyond Martin already identified, or aspirational? | High | Open |
| B003 | Is international expansion active (existing distributor contacts) or aspirational? | Medium | Open |
| B004 | Does Martin's Infanzia franchise agreement restrict how he can market/sell? | Medium | Open |
| B005 | Who is the domain/hosting for the physician dashboard — Martin or Learn Geek? | Medium | Open |
| B006 | What is Martin's timeline expectation? | Medium | Open |
| B007 | Does Martin have existing patient data we need to consider for migration? | Low | Open |

---

## Risks

| # | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R001 | OpenEvidence API not publicly accessible | Medium | High | Build custom RAG fallback — design for this from day one |
| R002 | Peruvian health data law creates compliance blockers | Medium | High | Paid discovery phase includes legal review before build |
| R003 | Scope creep due to personal relationship | High | Medium | Formal contract, change order process, clear phase boundaries |
| R004 | ElevenLabs conditional flow not flexible enough for triage | Low | High | POC answers this — alternative: custom LLM layer on top |
| R005 | Martin cannot maintain knowledge base without support | Medium | Medium | Admin interface designed for non-technical user |
| R006 | AI gives incorrect medical information, liability exposure | Low | Very High | Hard guardrails in system prompt, liability language in contract |
| R007 | WhatsApp Business API approval delays | Medium | Medium | Start approval process early — can take weeks |
| R008 | Sticker shock — full $34.5K–$43.5K shown upfront before budget discussion | Medium | High | Consider leading proposal with Discovery price only ($2,500–$3,500); present full build pricing after discovery when client is invested and spec is concrete |
| R009 | POC (Phase 0) is 1–2 weeks of unbillable work — sunk cost if Martin's budget doesn't support full build | Medium | Medium | Treat POC as platform investment (reusable for other clients), not client-specific cost. Keep POC scope tight. |

---

## Assumptions

1. Martin is the sole decision-maker — no board or parent company approval needed
2. The Infanzia parent company website is separate and not in scope
3. All patient/parent interactions will be in Spanish initially
4. Martin will provide his own WhatsApp Business number for the system
5. The physician audience initially is Martin himself, potentially expanding to his network
6. Learn Geek LLC is contracting directly with Martin, not with Kezer-Lab corporate
7. Hosting will be on infrastructure Learn Geek controls (not Martin's local machine)

---

## Contacts

| Name | Role | Contact | Notes |
|---|---|---|---|
| Martin Núñez | Client / Pediatrician | WhatsApp 976714745 | Spanish preferred |
| Karen McArthey | Interpreter / Introducer | — | Wife — keep professional boundary |

---

## OpenEvidence — Research Findings
**Researched:** 2026-03-07

### What OpenEvidence Is
OpenEvidence is the fastest-growing clinical decision support platform in the US, currently used by over 40% of US physicians daily across 10,000+ hospitals. It is a medically-tuned LLM trained on 35 million peer-reviewed publications, NEJM, JAMA, Mayo Clinic, and major clinical guidelines. It scored above 90% on the USMLE — higher than general-purpose LLMs including GPT-4 and Claude. In January 2026, the company closed a $250 million Series D led by Thrive Capital and DST Global, doubling its valuation to $12 billion.

### API Status — The Critical Finding
OpenEvidence offers its capabilities through an API, making it accessible for integration into various medical applications — it currently powers Elsevier's ClinicalKey AI. However this is **not a public open API**. The picture is more nuanced:

- API licensing for clinical decision support integration is one of OpenEvidence's revenue streams, primarily targeting health systems and enterprise partners.
- Their Microsoft collaboration integrates OpenEvidence technology directly into Dragon Copilot for enterprise healthcare organizations.
- EHR integrations via FHIR-based pilots with Epic are already underway, with Sutter Health announcing a collaboration in February 2026.
- The free app requires a US NPI number — **Martin as a Peruvian physician would not qualify for free access.**
- Account creation requires an NPI number — a US-specific physician identifier. Peruvian physicians have no NPI.

### Implications for This Project

**The NPI problem is significant.** OpenEvidence is architected entirely around the US healthcare system. Their verified user base, their free access model, their regulatory compliance — all US-centric. A Peruvian pediatrician operating in Lima falls entirely outside their target market and access model.

**API access is enterprise/institutional only.** Even if we could obtain API access, it would require a formal partnership agreement and likely significant licensing cost — not appropriate for a single-operator franchise at this stage.

**The geographic gap is the real issue.** Even if OpenEvidence opened API access tomorrow, their clinical evidence database skews heavily toward US clinical guidelines, US drug formularies, and US standard-of-care protocols. Peruvian pediatric practice may differ meaningfully — different drug availability, different endemic diseases, different MINSA guidelines.

### Recommended Alternatives

#### Option A — PubMed / NCBI API (Recommended Starting Point)
- Completely free and open public API
- Access to 35+ million biomedical citations and abstracts
- No geographic or NPI restrictions
- Covers the same peer-reviewed literature OpenEvidence draws from
- We build our own RAG layer on top — curated, pediatric-focused, Spanish-friendly
- Full control over what goes into the knowledge base

#### Option B — WHO / PAHO Guidelines
- Pan American Health Organization (PAHO) is the WHO regional office for the Americas
- Publishes clinical guidelines specifically for Latin American context
- Free, authoritative, and geographically appropriate
- Excellent complement to PubMed for this use case

#### Option C — Curated Pediatric RAG (Custom)
- Martin as a pediatrician curates the knowledge base himself
- Uploads Peruvian MINSA pediatric guidelines, AAP guidelines he trusts, clinical references from the Infanzia product documentation
- We build the RAG layer — he owns and controls the content
- Most appropriate for the triage use case because the content is physician-vetted
- This is actually what Martin described in the meeting — he wants physician-controlled context

#### Option D — Approach OpenEvidence Directly (Long Shot)
- Their mission statement includes making medical knowledge "universally available"
- A formal partnership pitch — "we're bringing your platform to Latin American physicians" — could be interesting to them as a market expansion play
- Their Veeva partnership suggests appetite for pharmaceutical/medical adjacent relationships
- Worth a cold outreach but cannot be relied upon for project timeline

### Recommendation
**Build the custom RAG layer from day one using PubMed API + PAHO guidelines + physician-curated content.** This is actually *better* than OpenEvidence for this use case because:
1. No geographic restrictions
2. No NPI requirement
3. Content is appropriate for Peruvian clinical context
4. Physician controls what the AI knows — Martin's concern about AI accuracy is addressed
5. No dependency on a third-party platform that could change access terms
6. The abstraction layer we're already building for ElevenLabs applies here too

**Update the POC spec** to mock a PubMed API call instead of an OpenEvidence call — same architecture, better long-term path.

---

## Document History

| Date | Change | Author |
|---|---|---|
| 2026-03-07 | Initial creation — post first meeting | Mark McArthey |
| 2026-03-07 | OpenEvidence research findings added — T001 closed | Mark McArthey |
