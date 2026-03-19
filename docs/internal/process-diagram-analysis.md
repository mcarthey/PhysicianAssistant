# Process Diagram Analysis
## Infanzia® — Physician AI Assistant System

**Received:** March 16, 2026 (via WhatsApp)  
**Source:** Dr. Martin Núñez — hand-drawn/designed process diagram  
**Context:** Martin was at Expo West (Anaheim) — delayed sending. He noted the graphic shows the process well and wants to discuss budget, timeline, and scope next.  
**Status:** Internal reference — update implementation plan and proposal accordingly

---

## Martin's Message (translated)

> "Hi Mark. Sorry for the delay. I had work from what I brought from Expo West in Anaheim. I think the graphic shows well how the process would be. Let me know when you can see it, and with that we can talk about budget, time, and scope. Talk soon. Thanks."

**Note:** His mention of Expo West is significant — that is a major natural/organic products trade show in the US. Suggests Infanzia has active US market interest, not just aspirational. Worth following up on.

---

## Diagram Description

The diagram shows three nodes arranged in a circular flow:

**Left node — Padres / Tutores (Parents / Guardians)**
Input they provide:
- Historia de la enfermedad / consulta (medical history / reason for visit)
- Signos / Síntomas (signs / symptoms)
- Resultados * (results — likely lab results or prior test data)

Input channel: WhatsApp — Texto, mensaje hablado, imágenes (text, voice message, images)

**Center node — IA (AI)**
Functions listed:
- Data warehouse (por paciente / ID) — per-patient data store
- Data mining (PubMed, Open Evidence, etc.)

Control: On / Off toggle at the bottom (physician activates/deactivates)

Audit: "VoBo del proceso (auditoría)" — sign-off / audit trail of the process

**Right node — Médico (Physician)**
What they receive:
- Fecha / hora (date / time)
- Nombre / ID (patient name / ID)
- Sg / Sm (resumen) — signs / symptoms summary
- Adjuntos / resultados (attachments / results)
- Impresión Dx. ? (diagnostic impression — note the question mark, meaning AI provides a suggested impression for the physician to confirm or override)
- Desenlace (EMG / OBS) — outcome categorization (Emergency / Observation)

**Flow directions:**
- Green arc (top): Observación / Sugerencias (Observations / Suggestions) — from AI back to parents
- Red arc (top): Emergencia — emergency redirect path
- Blue arc (right): WhatsApp — physician receives data via WhatsApp
- Bottom: On/Off toggle controls when the system is active

---

## Key Findings — What the Diagram Adds

### 1. Images are an input channel
Martin explicitly includes images alongside text and voice. Parents may send photos of rashes, medications, lab results, or other visual information. This is a meaningful addition to the scope — the AI needs to handle image inputs, not just text and audio.

### 2. "Impresión Dx. ?" — Diagnostic impression with a question mark
This is the most significant detail in the diagram. Martin envisions the AI providing a *suggested* diagnostic impression for the physician to review — not a definitive diagnosis. The question mark is intentional: it's a prompt for the physician, not a conclusion. This needs careful handling in the system design and liability framework. The AI can surface a structured impression (e.g., "possible febrile illness, non-emergency") but must always frame it as a hypothesis for physician review, never a diagnosis.

### 3. "Desenlace (EMG / OBS)" — Outcome classification
The physician-facing output includes an outcome field: Emergency or Observation. The AI pre-classifies each interaction. This confirms the triage logic is a core output, not just a routing decision.

### 4. Data warehouse per patient / ID
Martin wants patient-level data persistence — not just conversation logs. Each patient gets their own record built over time. This is a more substantive data layer than initially scoped, and has implications for the Ley 29733 compliance approach.

### 5. PubMed AND OpenEvidence listed together
The diagram lists both PubMed and OpenEvidence (with "etc.") as data mining sources. This confirms our decision to use PubMed as the primary knowledge backbone is aligned with his thinking — he's not locked to OpenEvidence specifically, he just wants grounded medical evidence.

### 6. "VoBo del proceso (auditoría)" — Audit sign-off
Martin wants a formal audit trail where the physician signs off on each interaction. "VoBo" (Visto Bueno) is a Latin American business term for an approval/sign-off. This is not just logging — it's a structured physician acknowledgment that they reviewed the AI's output. Important for regulatory compliance and liability.

### 7. Adjuntos / resultados — Attachments/results going TO the physician
The physician receives attachments and results as part of the queue. This means the system needs to store and forward any images or documents the parent submitted, not just the text summary.

---

## Scope Implications

| Finding | Implication | Impact |
|---|---|---|
| Image inputs | Multi-modal input handling (text + voice + images) | Adds complexity — image analysis / forwarding needed |
| Impresión Dx. ? | Structured triage output with suggested impression | Must be clearly framed as hypothesis, not diagnosis — legal review needed |
| Per-patient data warehouse | Persistent patient records, not just conversation logs | Larger data model, more Ley 29733 surface area |
| PubMed + OpenEvidence | Confirms PubMed fallback is acceptable | Validates current POC direction |
| VoBo / audit sign-off | Physician must explicitly acknowledge each review | Adds a workflow step to the dashboard |
| Attachments forwarded to physician | File storage and forwarding required | S3 or equivalent blob storage needed |

---

## Questions This Diagram Raises

- **Images:** Does Martin expect the AI to analyze images (e.g., "this looks like a rash"), or just forward them to the physician for review? AI image analysis in a medical context is a significant liability question.
- **Impresión Dx.:** Who is comfortable with the AI generating a diagnostic impression, even tentative? This needs explicit language in the liability framework and the engagement letter.
- **Data warehouse:** Is this per-patient across multiple interactions (longitudinal record), or just per-conversation? Longitudinal records significantly increase data governance complexity.
- **VoBo:** Does the physician sign off within the dashboard UI, or via WhatsApp reply? The diagram shows physician interaction via WhatsApp — clarify.
- **"Resultados *":** The asterisk on "Resultados" in the parent input section suggests a footnote or condition — what is it? Lab results? Prior physician notes? Needs clarification.

---

## Updated Open Items

- [ ] Reply to Martin — confirm receipt, request a call to discuss budget, timeline, scope
- [ ] Clarify image handling expectation (forward only vs. AI analysis)
- [ ] Clarify "Impresión Dx. ?" — how tentative/structured does he want this
- [ ] Clarify "Resultados *" asterisk on parent input side
- [ ] Clarify VoBo mechanism — dashboard button vs. WhatsApp reply
- [ ] Clarify longitudinal vs. per-conversation data warehouse
- [ ] Update implementation plan scope with image input and file storage
- [ ] Update proposal if scope changes materially
- [ ] Add Expo West / US market thread to discovery phase questions

---

*Internal document — Learn Geek LLC. Not for distribution.*
