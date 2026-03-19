# Physician AI Assistant System
## Project Alignment & Phased Approach

**Prepared by:** Mark McArthey — Learned Geek — markm@learnedgeek.com  
**Prepared for:** Dr. Martin Núñez — Infanzia® / Kezer-Lab  
**Date:** March 17, 2026 (rev 2 — scope questions confirmed)  
**Purpose:** Confirm our understanding of your answers, document agreed scope, and align on next steps

---

## Thank You for the Diagram

The process diagram you sent is exactly what was needed. It communicates the vision clearly and adds important detail that wasn't fully captured in our first conversation — particularly around the type of information the physician receives, the formal sign-off process, and the on/off control mechanism.

This document reflects that diagram. Before we confirm scope and schedule, there are a handful of questions worth answering together — not to complicate things, but to make sure what gets built matches what you actually need.

---

## What We Heard — Our Understanding of the System

Based on our meeting and your diagram, here is how we understand the system you want to build.

**The problem it solves:** Physicians cannot be available around the clock, but parents need a point of contact when their child is unwell. Rather than silence, you want parents to reach a calm, knowledgeable assistant that captures what they need to share, keeps them informed, and ensures the physician has everything ready to act when they return.

**How it works:**

When a physician activates the system — at the end of their shift, before a weekend, or before a vacation — it begins listening. Parents who contact via WhatsApp are greeted by the assistant in Spanish. The conversation is natural: the assistant asks about the child, listens to what the parent describes, and gathers the information the physician will need.

If a parent describes something that could be an emergency — difficulty breathing, seizures, loss of consciousness, or similar — the assistant immediately directs them to emergency services. No delay, no additional questions.

For everything else, the assistant reassures the parent, confirms that the physician will review the information and follow up, and closes the conversation. Everything captured is stored and waiting when the physician returns.

**What the physician sees:** A prioritized queue of conversations — urgent cases at the top. For each one: date and time, the child's name and age, a summary of symptoms, any attachments the parent sent, and a suggested clinical impression for the physician to confirm or set aside. The physician reviews, takes action, and formally signs off that the case has been handled.

**The clinical boundary:** The assistant captures and calms. It never diagnoses. Every interaction states clearly that the physician will make all clinical decisions. This boundary is built into the system and cannot be changed by the conversation.

---

## The Two Systems — Side by Side

You mentioned two parallel goals in our meeting. Both are included in this proposal.

**System 1 — Infanzia Product Assistant**

A WhatsApp assistant for the Infanzia product line. Parents ask questions about Biomilk, Infabiotix, Infavit supplements, and other products — and receive accurate, reliable answers at any hour. This system is simpler to build, can be delivered faster, and provides immediate value while the physician system is being developed.

**System 2 — Physician After-Hours Patient Communication System**

The system described above — the core of what you shared in our conversation and your diagram. More complex, more sensitive, and more valuable. This is the one that requires careful design, proper compliance work, and a structured approach to get right.

Both are included in the phased plan below.

---

## Scope Confirmed — Your Answers

Thank you for your detailed written responses. They are clear, precise, and give us everything we need to move into the Discovery phase with confidence. Here is our understanding of what you confirmed.

**1. Images — three categories, three approaches**

You drew an important distinction between types of images. Radiology images (X-ray, CT, MRI, ultrasound) require integration with clinical records systems and are best addressed in a future phase. Camera photos — skin conditions, injuries, secretions — are appropriate for AI analysis with a mandatory disclaimer that the physician's opinion is always required. Lab result values are a meaningful input the AI can use to provide more specific guidance and assess urgency. We will design the system accordingly.

**2. Impresión diagnóstica — a substantiated clinical analysis**

You confirmed that "impresión diagnóstica" in medicine means a substantiated, verifiable analysis — distinct from a definitive diagnosis, which belongs exclusively to the treating physician. The AI will generate this impression for the physician's review and validation. The physician's audit of that impression is indispensable and will be built into the workflow.

**3. VoBo — mandatory for urgent cases, sampled for routine**

Validation is not required for every single interaction. For urgent and emergency cases it is mandatory — the physician cannot close the case without completing it. For routine consultations, a statistically significant sample is sufficient, consistent with the 80/20 principle you described. The system will support both workflows.

**4. Patient records — longitudinal by patient ID**

The system will recognize patients across contacts and connect their conversation history into a single ongoing record. We noted your broader vision for this data — including the possibility of a parent-facing subscription layer and future links to hospital records — and we will design the data architecture to keep those paths open.

**5. Voice responses — text is sufficient to start**

Confirmed. The system will respond in text for the initial version. Voice synthesis can be added in a later phase once the core system is validated.

**6. Resultados * — physician-ordered results, including critical values**

Confirmed as results from evaluations and referrals ordered by the treating physician. You also clarified that certain results are critical and cannot wait — when a parent reports one of these, the system must immediately direct them to contact the physician and go to the emergency room. We will incorporate critical result detection alongside the symptom-based emergency logic.

---

## Remaining Questions for Discovery

There are a small number of items that cannot be finalized without a direct conversation. These will be resolved in the Discovery phase.

- The specific numeric thresholds that define a "critical" lab result — needed before we can write the emergency detection logic
- Whether a child may be contacted by multiple guardians from different phone numbers, and how the system should handle that
- Timeline expectations and budget range
- Whether the physician network beyond yourself is already identified or still to be developed

---

## Compliance and Data Privacy

Health data is sensitive. In Peru, the Ley 29733 (Ley de Protección de Datos Personales) sets specific requirements for how it must be handled: explicit consent, secure storage, defined retention periods, and complete audit trails.

Before the physician system launches, we recommend a review by a Peruvian legal advisor to confirm the compliance approach. We will prepare all the technical documentation that review requires. This is not a blocker to starting the project — it runs in parallel with development.

Additionally, the consent framework needs to be established before the system goes live: how parents consent to data capture, how physicians formally acknowledge the system's scope and limitations, and how data can be removed on request.

---

## AI Accuracy — A Real Challenge We Are Designing For

You raised accuracy concerns in our first conversation — your reference to OpenEvidence was fundamentally about this — and it is worth being transparent about how we are approaching it.

AI systems that draw on external knowledge sources (whether a product knowledge base or medical literature) can sometimes generate plausible-sounding responses even when the evidence to support them isn't there. This is a known challenge in the field, and it is more consequential in a medical context than in almost any other. We are not treating it as an edge case.

**For the Infanzia Product Assistant**, the system is designed to answer only from the product documentation you provide. If a question cannot be answered from those documents with sufficient confidence, the assistant acknowledges the gap and routes the parent to a human representative — it does not invent an answer. Every factual claim must be traceable to a specific document.

**For the physician communication system**, the challenge is greater because the AI is generating a clinical impression, not just answering a product question. The approach has several layers:

If the medical literature search returns no relevant results, the system does not generate a clinical impression at all — the physician receives the captured symptoms with a clear note that no supporting evidence was retrieved, and the case is flagged for direct review.

If evidence is retrieved, the AI's clinical impression must be traceable to the specific sources it drew from. Responses that cannot be attributed are rejected automatically.

The physician's VoBo validation step is not just a compliance mechanism — it is the most important accuracy safeguard in the system. The AI may occasionally be wrong; the physician catches it, corrects it, and that correction feeds back into improving the system over time. This is the architecture Martin described in his process diagram, and it is the right design for a medically sensitive application.

**Testing will require your active involvement.** Before the system goes live, we will need you and ideally one or two other physicians to run realistic scenarios through it and review every clinical impression the AI generates. If the override rate — the percentage of AI assessments that physicians correct — is too high, the system is not ready. A formal sign-off on accuracy is a go-live requirement, not a formality.

We want to be direct about this because it is important: we build the safeguards, but you are the clinical authority who validates that they work in practice. This is a shared responsibility, and we think that framing is exactly right for a system that operates at the boundary between technology and medicine.

---

## Phased Approach

We recommend building in phases that deliver value progressively while managing complexity and risk.

---

### Phase 1 — Discovery & Specification (Weeks 1–3)

Before any building begins, we invest three weeks getting the details exactly right. This phase resolves all the questions above, maps the complete workflow, aligns on compliance approach, and produces a specification document that both parties review and approve before development starts.

Deliverables:
- Full workflow documentation
- Agreed answers to all open questions above
- Compliance approach confirmed with legal advisor
- Technical specification reviewed and approved by you
- Engagement letter signed

---

### Phase 2 — Infanzia Product Assistant (Weeks 3–14)

The simpler of the two systems, built first. A WhatsApp assistant that knows the Infanzia product line in detail — ingredients, dosing, certifications, where to buy — and can answer parent questions accurately at any hour.

You manage it through a simple web interface: upload updated product documents, review conversation logs, and turn the assistant on or off.

Deliverables:
- WhatsApp assistant in Spanish (additional languages Phase 2)
- Product knowledge base: Biomilk 1/2/3, Infabiotix, Infavit D3, Infavit Multivitamínico, Infavit Hierro, Coligas
- Admin interface for document management and conversation review
- Hard guardrail: no medical advice, always refer to physician

**Important dependency:** Activating WhatsApp for business use requires approval from Meta (WhatsApp's parent company). This process typically takes 2–4 weeks and must begin at the start of Phase 1 — it runs independently of development. Additionally, because this system will handle data from users in Peru, the hosting infrastructure must be configured to satisfy international data privacy requirements. Both of these are addressed during Discovery so they do not create surprises later, but they are real factors in the overall timeline.

---

### Phase 3 — Physician After-Hours System (Weeks 14–26)

The core build. Developed in stages with your review at each milestone.

The conversation engine — the part that talks to parents, captures symptoms, detects emergencies, and generates the structured summary — is built and tested first. The physician dashboard follows, incorporating everything from your diagram: prioritized queue, suggested clinical impression, VoBo sign-off, attachment viewing, and on/off control.

Deliverables:
- WhatsApp conversation system in Spanish
- Emergency detection with immediate redirect
- Structured symptom capture with clinical evidence integration (PubMed)
- Patient data store (longitudinal records, encrypted, compliant)
- Physician dashboard: queue, transcript, Impresión Dx., Desenlace classification, VoBo workflow
- Complete audit trail for every interaction

---

### Phase 4 — Voice & Expansion (Weeks 26–29, conditional)

Voice response output (if confirmed as a requirement), additional language support (English, Portuguese), and any refinements based on real-world usage.

---

### Phase 5 — Ongoing Retainer

Monthly maintenance, hosting, knowledge base updates (new products, updated clinical guidelines), and support. Scope and cost determined based on actual usage.

---

## Indicative Schedule

| Phase | Weeks | Milestone |
|---|---|---|
| Discovery & Specification | 1–3 | Specification approved, engagement letter signed |
| Infanzia Product Assistant | 3–14 | Assistant live on WhatsApp |
| Physician After-Hours System — Conversation Engine | 14–20 | Core system tested |
| Physician Dashboard | 20–26 | Full system integrated and reviewed |
| Voice & Expansion | 26–29 | Conditional on Phase 1 decisions |
| Ongoing retainer | Month 8+ | Maintenance and support begins |

### Schedule Dependencies

The timeline above assumes the following conditions are met. Delays in any of these areas will affect delivery dates, and will be communicated promptly.

- **WhatsApp Business API approval** — Meta's approval process begins at the start of Phase 1 and typically takes 2–4 weeks. Development proceeds in parallel, but the system cannot go live until approval is granted.
- **Hosting and data residency** — the appropriate hosting infrastructure for international health data must be confirmed during Discovery. This determination depends in part on the outcome of the legal review.
- **Timely feedback and sign-off** — each phase requires your review and written approval before the next begins. Extended review periods will shift subsequent milestones accordingly.
- **Access to product documentation** — the Infanzia Product Assistant knowledge base is built from the product documents you provide. Early access to these materials keeps Phase 2 on schedule.
- **Discovery phase decisions** — the open questions in this document directly determine the scope and complexity of Phase 3. Answers received during Discovery are incorporated into the final specification before any build work begins.

### Important Notice

All timelines, scope descriptions, and estimates in this document are indicative and subject to change. They are based on the information available at the time of writing and the assumptions stated above. Final scope, schedule, and investment figures will be confirmed in a formal Statement of Work, reviewed and signed by both parties before any billable development work begins. Changes to scope after sign-off are handled through a written change order process. Learned Geek is not responsible for delays resulting from third-party approvals, client-side dependencies, or regulatory requirements outside our control.

---

## Investment

We will discuss specific investment figures in our next conversation, once the open questions above are answered and scope is confirmed.

What we can share now: the scale of this project — two systems, a compliance framework, a physician dashboard, and the care required for a medically sensitive application — represents a meaningful investment. The Discovery phase gives us both the information needed to arrive at a number that reflects the actual scope, not a guess.

Discovery itself is a fixed-scope engagement with a defined cost. We can discuss that in our next call.

---

## Suggested Next Steps

1. Confirm that our interpretation of your answers above is accurate
2. Schedule a 30-minute Discovery call to resolve the remaining items — we will send two time options
3. Once Discovery is agreed, we formalize the engagement with a Statement of Work and begin

We are ready to move forward when you are.

---

*Prepared by Mark McArthey — Learned Geek — markm@learnedgeek.com*  
*Confidential — prepared exclusively for Dr. Martin Núñez / Infanzia® / Kezer-Lab*
