# Ítems de Acción Pre-Discovery — Dr. Núñez

**De:** Mark McArthey, Learned Geek LLC
**Fecha:** 18 de marzo de 2026
**Propósito:** Puntos que necesitan tu aporte o acción antes o durante nuestra fase de Discovery. Estos nos ayudarán a avanzar de manera eficiente una vez que comencemos.

---

Martín,

Gracias por las respuestas tan detalladas — nos ayudaron mucho a dar forma a la dirección técnica. Hemos avanzado bastante de nuestro lado con la arquitectura, los requisitos de cumplimiento y la planificación, y llegamos a un punto donde necesitamos tu ayuda en varias cosas que solo se pueden resolver desde Perú.

No te preocupes por abordar todo de golpe — te los paso ahora para que los vayas pensando, y así podemos priorizar los más importantes para nuestra llamada de Discovery.

---

## Prioridad 1 — Necesitamos Respuestas Antes de Construir

Estos puntos podrían cambiar el alcance, la arquitectura o la viabilidad del proyecto. Necesitamos claridad sobre ellos antes de comprometernos con un plan de construcción.

### 1.1 — Asesoría Legal (Privacidad de Datos en Perú)

Necesitamos un abogado peruano especializado en privacidad de datos (Ley 29733) e idealmente en regulación de datos de salud. El sistema almacenará registros longitudinales de salud pediátrica y transferirá datos a servicios en la nube con sede en EE.UU. (Anthropic, Twilio). Preguntas específicas para el abogado:

- **Registro en la APDP:** ¿El sistema requiere registro ante la Autoridad Nacional de Protección de Datos Personales? ¿Quién registra — tú como médico (responsable del tratamiento) o nosotros como proveedor tecnológico (encargado del tratamiento)?
- **Transferencia internacional de datos:** ¿Qué mecanismo legal se requiere para transferir datos de salud pediátrica a EE.UU.? ¿Es suficiente el consentimiento explícito o se necesita una determinación formal de nivel adecuado de protección?
- **Consentimiento para menores:** ¿Quién puede consentir legalmente el procesamiento de datos de salud de un niño? ¿Solo el tutor legal? ¿Ambos padres? ¿Hay umbrales de edad?
- **Retención de datos vs. derecho de supresión:** Si un padre solicita que se eliminen los datos de su hijo, ¿eso entra en conflicto con los requisitos de retención de registros médicos?

**Lo que necesito de ti:** ¿Puedes recomendar un abogado peruano que maneje esto? Si no, puedo investigar firmas, pero alguien en tu red profesional que entienda tanto datos de salud como la Ley 29733 sería ideal. **Este es el punto más importante de toda la lista.**

### 1.2 — Clasificación como Dispositivo Médico / Regulatoria

Esta es una pregunta que necesita respuesta del asesor legal, pero quiero que la tengas en cuenta: **¿podría DIGEMID clasificar este sistema como dispositivo médico?** La IA genera una impresión diagnóstica basada en síntomas y valores de laboratorio. Algunas jurisdicciones clasifican las herramientas de IA de apoyo a decisiones clínicas como dispositivos médicos, lo que requeriría registro y aprobación regulatoria.

De manera similar: **¿este sistema cae bajo la ley de telemedicina de Perú (Ley 30421)?** Una IA que se comunica con pacientes en nombre de un médico fuera de horario podría considerarse telemedicina.

**Lo que necesito de ti:** Plantea ambas preguntas al asesor legal. Si la respuesta a cualquiera de las dos es "sí" o "tal vez," necesitamos entender los requisitos antes de construir — no después.

### 1.3 — Restricciones del Acuerdo de Franquicia

¿Tu acuerdo de franquicia con Infanzia / Kezer-Lab restringe o requiere aprobación para implementar herramientas de IA que interactúen con clientes en nombre de la marca?

**Lo que necesito de ti:** Revisa tu acuerdo de franquicia para cualquier cláusula sobre implementación de tecnología, o consulta con tu contacto de franquicia.

### 1.4 — Rango de Presupuesto

Aún no hemos hablado de presupuesto, y quiero prepararte una propuesta que tenga sentido para ti. El sistema completo (chatbot de productos + sistema de triaje + panel de control) es una inversión importante. Puedo armar opciones escalonadas — desde un MVP más básico hasta la visión completa — pero me ayuda saber por dónde andas.

**Lo que necesito de ti:** Un rango general o nivel de comodidad. No te compromete a nada — simplemente me ayuda a enfocar la propuesta en lo que realmente tiene sentido.

### 1.5 — Expectativas de Cronograma

¿Cuál es tu objetivo para tener el primer sistema operativo? ¿Hay plazos externos que impulsen esto (conferencia, revisión de franquicia, hito de volumen de pacientes)?

---

## Prioridad 2 — Necesarios Durante la Fase de Discovery

Estos son puntos que trabajaremos juntos durante Discovery, pero pensar en ellos ahora hará la llamada más productiva.

### 2.1 — Umbrales Críticos de Valores de Laboratorio (Futuro — No Urgente)

En algún momento durante la construcción, vamos a necesitar umbrales numéricos específicos para valores pediátricos que deberían activar la vía de emergencia — como lo que mencionaste en tu respuesta a la P6 (hemoglobina, glucosa, etc. por grupo de edad). Vamos a preparar el sistema para soportar esto, pero no necesitamos los números todavía. Solo es algo para que lo vayas pensando cuando tengas tiempo — lo trabajamos juntos cuando lleguemos a esa parte.

### 2.2 — Tasa de Muestreo VoBo

Mencionaste que los casos de rutina (no urgentes) no necesitan 100% de revisión médica — una muestra estadísticamente significativa es suficiente. ¿Qué tasa de muestreo quieres iniciar? ¿20%? ¿30%? ¿Debería variar por categoría (por ejemplo, mayor muestreo para casos que involucren imágenes)?

### 2.3 — Red de Médicos

Has mencionado otros médicos además de ti. ¿La red de usuarios potenciales está identificada y en contacto contigo, o es aspiracional en esta etapa? Esto afecta si construimos multi-inquilino desde el día uno o lo agregamos después.

### 2.4 — Estado del Mercado en EE.UU.

Estabas en Expo West en Anaheim cuando nos conectamos inicialmente. ¿Hay interés activo en el mercado estadounidense para los productos Infanzia, y el chatbot de productos necesitaría soportar inglés y padres en EE.UU.?

### 2.5 — Documentación de Productos

Para el Chatbot de Productos Infanzia (Línea de trabajo A), necesitaremos el catálogo completo de productos para construir la base de conocimiento:

- Fichas de producto para Biomilk, Infabiotix, Infavit y todos los demás productos
- Tablas de dosificación / uso recomendado
- Listas de ingredientes y certificaciones (certificaciones EU, detalles de fabricación)
- Preguntas frecuentes que comúnmente respondes
- Cualquier referencia clínica que quieras que el chatbot cite

**Lo que necesito de ti:** Comienza a reunir estos materiales. Formatos digitales (PDF, Word) están bien. Cuanto más completa sea la documentación, más preciso será el chatbot.

---

## Prioridad 3 — Para Tu Conocimiento

Cosas que estamos resolviendo de nuestro lado, pero que quiero que sepas.

### 3.1 — Revisión de Cumplimiento de Proveedores

Estamos revisando las políticas de uso aceptable de Anthropic (el proveedor de IA) y Meta/WhatsApp para confirmar que el uso médico/clínico está permitido. No anticipamos problemas, pero estamos verificando antes de construir sobre estas plataformas.

### 3.2 — Acuerdos de Procesamiento de Datos

Ejecutaremos Acuerdos de Procesamiento de Datos (DPAs) con Twilio y Anthropic antes de cualquier implementación en producción. Tu asesor legal puede querer copias de estos para su revisión de cumplimiento.

### 3.3 — Estructura Empresarial

Learned Geek LLC es una empresa de responsabilidad limitada con sede en EE.UU. Estamos evaluando si el alcance de este proyecto amerita cobertura adicional de responsabilidad profesional específica para sistemas médicos asistidos por IA.

### 3.4 — Aprobación de WhatsApp Business

La aprobación de la API de WhatsApp Business puede tomar de 1 a 4 semanas. Comenzaremos este proceso al inicio de Discovery para que no se convierta en un cuello de botella.

---

## Llamada de Discovery

Me encantaría coordinar nuestra llamada de Discovery cuando te acomode. Sé que te dije "sin prisa, a tu ritmo" — y eso sigue siendo así en general — pero arrancar la conversación de Discovery pronto nos permite resolver las preguntas legales y regulatorias temprano, que es donde realmente está el riesgo de cronograma. La parte técnica la tenemos clara; lo de cumplimiento es lo que toma más tiempo.

**Agenda sugerida:**
1. Revisar estos puntos — confirmar prioridades, aclarar dudas
2. Presupuesto y cronograma — alinearnos
3. Asesoría legal — próximos pasos
4. Valores de laboratorio críticos — empezar a definirlos
5. Documentación de productos — qué hay disponible hoy
6. Red de médicos — estado actual
7. Próximos pasos y cómo seguimos

Dime 2–3 horarios que te sirvan. Soy flexible con la zona horaria.

---

Saludos,

Mark
Learned Geek LLC
markm@learnedgeek.com
