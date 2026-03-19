# Client Interview — Meeting Notes
## Infanzia® / Kezer-Lab — Initial Discovery Call

**Date:** March 7, 2026  
**Interviewer:** Mark McArthey — Learned Geek LLC  
**Client:** Dr. Martin Núñez — Infanzia® / Kezer-Lab  
**Also present:** Karen McArthey (interpreter as needed)  
**Format:** Audio recorded, transcribed  
**Status:** Internal reference document

---

## Context

Dr. Núñez was introduced through a personal connection (Karen's high school friend). The meeting was the first substantive conversation following initial outreach, during which Martin had shared a company PDF and mentioned interest in "cosas con AI" and an "automatic dashboard." The purpose of this call was discovery — understanding what Martin actually needs before any scoping or proposal.

---

## Key Themes

### 1. The Real Problem He's Solving

Martin opened not with a product request, but with a problem statement about the pharmaceutical industry. Other laboratories compete for physician loyalty by paying for conferences, trips, and gifts. Martin cannot and does not want to compete that way. Instead, his strategy is to give physicians something genuinely valuable: a technology tool that helps them serve their patients better.

> *"All the laboratories pay the physicians for Congress, pay for trips, pay for a lot of things. I don't have the money. I don't want to pay the physicians in that way. I would like to give them an IT solution."*

This framing is important. The physician assistant system is not just a feature — it's his go-to-market strategy for physician relationships.

---

### 2. Primary Vision — After-Hours Physician AI Assistant

Martin's core idea is an AI system that acts as a virtual assistant for pediatricians during their off-hours. He described it as a "night system" or "secretary" that operates when the physician cannot take calls or respond to WhatsApp.

**How he described the flow:**
- Physician activates the system when going off-shift (nights, weekends, vacations)
- Parents contact via WhatsApp — text or voice
- AI captures the child's name, age, and symptoms
- AI cross-references the information with a trusted medical evidence source
- AI triages urgency: emergency cases are redirected immediately; non-emergency cases are reassured and queued
- All captured information is stored and presented to the physician when they return
- Physician reviews the queue, calls the patient back, and closes the loop

He specifically named **OpenEvidence** as the medical evidence platform he had in mind, noting it is used by Cleveland Clinic and Johns Hopkins. His concern was that AI without a grounded evidence source might give incorrect or unsupported medical information — he wanted the system's responses to be traceable to real clinical literature.

> *"The AI... cross-information with OpenEvidence, which is allowed for Cleveland Clinic and Johns Hopkins. And with that information, the parents can do something. Can go to the emergency, can follow the rules of the physicians, can do something."*

---

### 3. What the System Must NOT Do

Martin was emphatic on several constraints:

- **Not a patient capture tool.** He does not want to collect patient data for commercial purposes. The system exists to help the physician, not to build a marketing database.
- **Not a diagnostic tool.** The AI must not make clinical decisions. He agreed immediately when Mark explained the liability boundary: AI captures and calms; physician makes all decisions.
- **Must comply with Peruvian government health policies.** He flagged regulatory compliance as a key concern before the system could launch.
- **Physician must control when it's active.** The system should be switchable — on when the doctor is unavailable, off otherwise.

> *"I don't want to capture the patient. I don't want to track the patient — just help the physician in a holistic way."*

---

### 4. The Liability Conversation

Mark raised the liability concern around AI giving incorrect medical information. Martin and his colleague (present but not named in transcript) were initially focused on ensuring the AI's responses matched accepted medical evidence — their concern was accuracy, not liability per se.

Mark explained that the practical solution is to constrain what the AI does: it captures information and keeps parents calm, but explicitly tells parents that the physician will make all clinical decisions and will follow up. Martin agreed with this framing immediately.

> *"So it looks like an assistant... to keep the parents calm... but also to run one pathway for that child, because if they send an emergency, the child needs to go to emergency. If not, they can wait for the decision."*

This "captures and calms" framing became the agreed architectural boundary for the system.

---

### 5. Voice AND Text

When asked whether the system should handle text or voice, Martin said both. His reasoning: parents in stressful situations may prefer to speak rather than type.

> *"I think both channels would be very good, because sometimes [parents are in] crazy moments... send voice."*

---

### 6. The Physician Interface

Mark walked Martin through the distinction between the patient-facing WhatsApp interaction and the physician-facing review interface. Martin confirmed that:

- Physicians would not be interacting with the system in real-time — they would review captured data afterward
- A dashboard or review interface for the physician would be needed, but the priority is the parent-facing interaction
- Physicians in Peru use WhatsApp extensively

Mark also explained the knowledge base / RAG concept — that the AI model needs to be trained on documents and data, and that this is best done through a proper admin interface rather than via text message. Martin understood and agreed.

> *"I would like to have the physicians in that way to have an assistant during the night."*

---

### 7. Secondary Goal — Infanzia Product Chatbot

Toward the end of the meeting, Martin mentioned a simpler, parallel use case: a chatbot for the Infanzia product line to handle claims and customer surveys. He acknowledged this would be easier to build and suggested running both projects in parallel.

> *"Another specific chatbot for claims or for surveys — I think it's more easy. Maybe we can follow both processes in parallel."*

This is Workstream A in the project plan — lower complexity, faster delivery, good for early revenue while the triage system is scoped properly.

---

### 8. No Website Needed

Martin confirmed he does not need a patient-facing website. The parent company (Kezer-Lab) has an existing website. The focus is entirely on the WhatsApp AI system and the physician tooling.

> *"My company has a website, so I think it's enough for that purpose."*

---

### 9. Multilingual Potential

Mark raised the multilingual capability of AI voice systems — the ability to operate in Spanish, English, Portuguese, and other languages, even with regional accent matching. Martin responded positively. Given the international market aspirations visible in the company PDF (Japan, China, Australia, Korea, EU, US all referenced), this is a meaningful feature for future phases.

---

### 10. Next Steps Agreed in the Meeting

Martin committed to diagramming his full envisioned process and sending it via email in Spanish. Both parties agreed to continue the conversation after reviewing the diagram.

> *"I can diagram all the process with my idea, send it to you, and we can continue talking about this and another points, and we can work together in the future, I hope so."*

---

## Mark's Observations from the Call

- Martin is thoughtful and has clearly been developing this idea for some time. It is not a vague AI wish — it has a specific problem, a specific workflow, and specific constraints.
- He is a solo operator with limited time. He needs the system to work reliably with minimal ongoing maintenance burden on his part.
- His medical background is his credibility asset. The physician triage system directly leverages that credibility by giving his physician network something of real value.
- The personal connection (Karen) provided immediate trust. The tone of the meeting was collaborative and open.
- Budget was not discussed. The scope as described warrants a significant investment — the discovery phase will be the appropriate moment to introduce pricing.
- The regulatory concern (Ley 29733) is real and was raised by Martin himself — it needs proper legal review before launch.

---

## Open Items from This Meeting

- [ ] Await Martin's process diagram — arriving via email in Spanish
- [ ] Investigate OpenEvidence API accessibility — requires US NPI, not viable as-is (see decisions log)
- [ ] Research Peruvian Ley 29733 compliance requirements for health data via AI
- [ ] Determine whether physician network beyond Martin is identified or aspirational
- [ ] Confirm international expansion is active vs. aspirational
- [ ] Prepare formal proposal for Martin's review
- [ ] Execute engagement letter before any billable work begins

---

*Internal document — Learned Geek LLC. Not for distribution.*
