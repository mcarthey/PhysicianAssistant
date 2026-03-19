# Project: Dr. Núñez Response to Scope Questions
## Translated from Spanish — Original document: 260317_Proyecto_DrOk_-_M__McArthey.docx
**Translation date:** March 17, 2026

---

Hi Mark,

Here are my responses to the questions asked:

---

**1. Images — analyze or forward?**
*(Your question: Your diagram shows that parents can send images via WhatsApp — photos of a rash, a medication, or a lab result. When they arrive, do you want the assistant to attempt to describe or interpret what it sees? Or should it simply receive the image, acknowledge it, and forward it to the physician as an attachment? Both are possible. Forwarding is straightforward. Having the AI describe or interpret images is more complex and raises additional questions about accuracy and liability that we would want to address explicitly.)*

- **Radiology images (X-rays, CT scans, MRI, ultrasounds):**
  - Their analysis depends on more than one image.
  - Today AI is more accurate in its interpretation and diagnosis than specialist physicians.
  - My opinion: we could only use this information if we have a direct link to Electronic Medical Record systems — HIS, RIS, PACS (Picture Archiving and Communication System). This is probably a development for a future phase, but it is worth keeping in mind for consideration.

- **Camera/screenshot images:**
  - These may include dermatological lesions, traumatic injuries, secretions/body fluids, etc.
  - It is common for parents/guardians to consult persistently about these situations. AI interpretation is generally quite accurate, but a note should always appear clarifying: *"You must necessarily seek the opinion of your treating physician or specialist."*

- **Lab result images:**
  - Since results display specific numeric values, those figures are an important and analyzable input for the AI to provide more specific guidance.

---

**2. Suggested clinical impression — how should it be presented?**
*(Your question: Your diagram includes "Impresión Dx.?" — a suggested diagnostic impression with a question mark, visible to the physician. We want to confirm: this is intended as a starting hypothesis for the physician to confirm or override, not a conclusion. The phrasing, how prominently it is displayed, and the language surrounding it should be agreed upon before it is built.)*

- In medicine, speaking of a "diagnostic impression" implies that a substantiated and verifiable analysis took place; it is different from the definitive diagnosis, which belongs exclusively to the treating physician.
- The subsequent physician audit/validation process becomes indispensable.

---

**3. Physician sign-off — how does it work in practice?**
*(Your question: Your diagram includes "VoBo del proceso" — a formal acknowledgment from the physician that they have reviewed each case. Does this happen within the web dashboard, or would you prefer physicians to confirm via WhatsApp? Both options are viable; the right answer depends on how physicians actually work in practice.)*

- The treating physician must validate what the AI has worked on and suggested. This validation improves AI training and shapes the process and future interventions.
- Validation is not required for every single interaction; it is mandatory for urgent/emergency cases and should be statistically significant for the remaining consultations — which, following the Pareto 80/20 principle, represent the larger critical mass.

---

**4. Patient records — single conversation or ongoing history?**
*(Your question: Should the system recognize a parent and child who have contacted before and connect their conversations into a continuous record? Or should each contact be treated as an independent event? A continuous record is more powerful but involves a larger data structure and greater compliance surface area.)*

- The system must recognize the patient through their ID.
- It must connect with the conversation history to provide more specific responses and results.
- The physician should pay for this system; however, storing historical data opens the possibility of charging users (parents/guardians) a monthly fee for:
  - Information storage
  - Historical data download, etc.
- This also opens the possibility of linking with medical records from hospitals and health centers (databases, labs, imaging, physical therapy and rehabilitation centers, etc.)

---

**5. Voice responses — required from the start?**
*(Your question: The assistant can respond in text, which appears as a standard WhatsApp message. It can also synthesize a voice response and send it as an audio message. Is voice response output important for the first version, or is responding in text sufficient to begin?)*

Text is sufficient to start.

---

**6. What does "Resultados *" mean on your diagram?**
*(Your question: The parent input side of your diagram shows "Resultados *" with an asterisk. We want to confirm what this refers to — lab results, prior physician notes, or something else — so we design the right intake flow.)*

- Here I am referring to results from evaluations or referrals requested by the treating physician.
- There are results considered critical (cannot wait) — in these cases the patient must urgently contact their treating physician and go to the emergency room.
