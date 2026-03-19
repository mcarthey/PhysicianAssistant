# Sistema de Asistente IA para Médicos
## Alineación del Proyecto y Enfoque por Fases

> **Nota para Karen:** Este es el documento actualizado que incorpora las respuestas del Dr. Núñez a las seis preguntas de alcance, así como una nueva sección sobre precisión de la IA. Por favor, revisa especialmente esa sección — el lenguaje necesita ser claro pero no alarmante. ¡Gracias!

---

**Preparado por:** Mark McArthey — Learned Geek — markm@learnedgeek.com  
**Preparado para:** Dr. Martin Núñez — Infanzia® / Kezer-Lab  
**Fecha:** 17 de marzo de 2026 (rev 2 — preguntas de alcance confirmadas)  
**Propósito:** Confirmar nuestra interpretación de sus respuestas, documentar el alcance acordado y alinear los próximos pasos

---

## Gracias por el Diagrama

El diagrama de proceso que nos envió es exactamente lo que necesitábamos. Comunica la visión con claridad y agrega detalles importantes que no quedaron completamente capturados en nuestra primera conversación — en particular, el tipo de información que recibe el médico, el proceso de validación formal y el mecanismo de activación y desactivación del sistema.

Este documento refleja ese diagrama. Antes de confirmar el alcance y el cronograma, hay algunas preguntas que vale la pena responder juntos — no para complicar las cosas, sino para asegurarnos de que lo que se construya coincida exactamente con lo que usted necesita.

---

## Lo Que Entendemos — Nuestra Interpretación del Sistema

Con base en nuestra reunión y su diagrama, así es como entendemos el sistema que usted desea construir.

**El problema que resuelve:** Los médicos no pueden estar disponibles las 24 horas del día, pero los padres necesitan un punto de contacto cuando su hijo no se encuentra bien. En lugar de silencio, usted quiere que los padres puedan comunicarse con un asistente tranquilo y bien informado que capture lo que necesitan compartir, los mantenga informados y asegure que el médico tenga todo listo para actuar cuando regrese.

**Cómo funciona:**

Cuando un médico activa el sistema — al terminar su turno, antes de un fin de semana o antes de vacaciones — el sistema comienza a operar. Los padres que se comunican a través de WhatsApp son recibidos por el asistente en español. La conversación es natural: el asistente pregunta sobre el niño, escucha lo que el padre describe y recopila la información que el médico necesitará.

Si un padre describe algo que podría ser una emergencia — dificultad para respirar, convulsiones, pérdida de consciencia o situaciones similares — el asistente los dirige inmediatamente a los servicios de emergencia. Sin demoras, sin preguntas adicionales.

Para todo lo demás, el asistente tranquiliza a los padres, confirma que el médico revisará la información y dará seguimiento, y cierra la conversación. Todo lo capturado queda almacenado y disponible cuando el médico regresa.

**Lo que ve el médico:** Una cola de conversaciones ordenada por prioridad — los casos más urgentes al inicio. Para cada uno: fecha y hora, nombre y edad del niño, resumen de síntomas, adjuntos enviados por los padres y una impresión diagnóstica sugerida para que el médico confirme o descarte. El médico revisa, toma acción y firma formalmente que el caso ha sido atendido.

**El límite clínico:** El asistente captura y tranquiliza. Nunca diagnostica. Cada interacción indica claramente que el médico tomará todas las decisiones clínicas. Este límite está incorporado al sistema y no puede ser modificado por la conversación.

---

## Los Dos Sistemas — En Paralelo

En nuestra reunión usted mencionó dos objetivos paralelos. Ambos están incluidos en esta propuesta.

**Sistema 1 — Asistente de Productos Infanzia**

Un asistente de WhatsApp para la línea de productos Infanzia. Los padres hacen preguntas sobre Biomilk, Infabiotix, suplementos Infavit y otros productos — y reciben respuestas precisas y confiables a cualquier hora. Este sistema es más sencillo de construir, puede entregarse más rápido y proporciona valor inmediato mientras se desarrolla el sistema médico.

**Sistema 2 — Sistema de Comunicación con Pacientes Fuera de Horario**

El sistema descrito anteriormente — el núcleo de lo que usted compartió en nuestra conversación y en su diagrama. Más complejo, más delicado y más valioso. Es el que requiere un diseño cuidadoso, un trabajo de cumplimiento normativo adecuado y un enfoque estructurado para hacerlo bien.

Ambos están incluidos en el plan por fases que se presenta a continuación.

---

## Alcance Confirmado — Sus Respuestas

Gracias por sus detalladas respuestas escritas. Son claras, precisas y nos dan todo lo que necesitamos para avanzar hacia la fase de Descubrimiento con confianza. A continuación presentamos nuestra interpretación de lo que usted confirmó.

**1. Imágenes — tres categorías, tres enfoques**

Usted hizo una distinción importante entre tipos de imágenes. Las imágenes de radiología (rayos X, TAC, RM, ecografías) requieren integración con sistemas de registros clínicos y es mejor abordarlas en una fase futura. Las fotografías de cámara — afecciones de la piel, lesiones, secreciones — son apropiadas para el análisis de IA con un aviso obligatorio de que siempre se requiere la opinión del médico. Los valores numéricos de resultados de laboratorio son un insumo significativo que la IA puede utilizar para proporcionar orientación más específica y evaluar la urgencia. Diseñaremos el sistema en consecuencia.

**2. Impresión diagnóstica — un análisis clínico fundamentado**

Usted confirmó que "impresión diagnóstica" en medicina significa un análisis fundamentado y verificable — distinto del diagnóstico definitivo, que corresponde exclusivamente al médico tratante. La IA generará esta impresión para la revisión y validación del médico. La auditoría de esa impresión por parte del médico es indispensable y estará incorporada al flujo de trabajo.

**3. VoBo — obligatorio para casos urgentes, por muestreo para los rutinarios**

La validación no se requiere en cada interacción individual. Para los casos urgentes y de emergencia es obligatoria — el médico no puede cerrar el caso sin completarla. Para las consultas rutinarias, una muestra estadísticamente significativa es suficiente, conforme al principio 80/20 que usted describió. El sistema admitirá ambos flujos de trabajo.

**4. Registros de pacientes — longitudinal por ID de paciente**

El sistema reconocerá a los pacientes a lo largo de los contactos y conectará su historial de conversaciones en un registro continuo. Tomamos nota de su visión más amplia para estos datos — incluyendo la posibilidad de una capa de suscripción para los padres y futuros vínculos con registros hospitalarios — y diseñaremos la arquitectura de datos para mantener esas posibilidades abiertas.

**5. Respuestas por voz — el texto es suficiente para comenzar**

Confirmado. El sistema responderá en texto para la versión inicial. La síntesis de voz podrá incorporarse en una fase posterior una vez que el sistema principal haya sido validado.

**6. Resultados * — resultados ordenados por el médico, incluidos los críticos**

Confirmado como resultados de evaluaciones e interconsultas ordenadas por el médico tratante. Usted también aclaró que ciertos resultados son críticos y no pueden esperar — cuando un padre reporta uno de estos, el sistema debe dirigirlos inmediatamente a contactar al médico e ir a urgencias. Incorporaremos la detección de resultados críticos junto con la lógica de emergencia basada en síntomas.

---

## Preguntas Pendientes para el Descubrimiento

Hay un pequeño número de elementos que no pueden finalizarse sin una conversación directa. Estos se resolverán en la fase de Descubrimiento.

- Los umbrales numéricos específicos que definen un resultado de laboratorio "crítico" — necesarios antes de que podamos escribir la lógica de detección de emergencias
- Si un niño puede ser contactado por múltiples tutores desde diferentes números de teléfono, y cómo el sistema debe manejar eso
- Expectativas de cronograma y rango de presupuesto
- Si la red de médicos más allá de usted ya está identificada o aún está por desarrollarse

---

## Cumplimiento Normativo y Privacidad de Datos

Los datos de salud son sensibles. En Perú, la Ley 29733 (Ley de Protección de Datos Personales) establece requisitos específicos para su manejo: consentimiento explícito, almacenamiento seguro, períodos de retención definidos y registros de auditoría completos.

Antes de que el sistema médico entre en funcionamiento, recomendamos una revisión por parte de un asesor legal peruano para confirmar el enfoque de cumplimiento. Prepararemos toda la documentación técnica que esa revisión requiera. Esto no es un obstáculo para iniciar el proyecto — se ejecuta en paralelo con el desarrollo.

Además, el marco de consentimiento debe establecerse antes de que el sistema entre en operación: cómo los padres dan su consentimiento para la captura de datos, cómo los médicos reconocen formalmente el alcance y las limitaciones del sistema, y cómo se puede solicitar la eliminación de datos.

---

## Precisión de la IA — Un Desafío Real que Estamos Diseñando para Superar

Usted planteó preocupaciones sobre la precisión en nuestra primera conversación — su referencia a OpenEvidence fue fundamentalmente sobre este tema — y vale la pena ser transparentes sobre cómo lo estamos abordando.

Los sistemas de IA que se basan en fuentes de conocimiento externas (ya sea una base de conocimiento de productos o literatura médica) pueden en ocasiones generar respuestas que suenan plausibles incluso cuando la evidencia que las respalda no está disponible. Este es un desafío conocido en el campo, y sus consecuencias son más significativas en un contexto médico que en casi cualquier otro. No lo estamos tratando como un caso excepcional.

**Para el Asistente de Productos Infanzia**, el sistema está diseñado para responder únicamente a partir de la documentación de productos que usted proporcione. Si una pregunta no puede responderse con suficiente confianza a partir de esos documentos, el asistente reconoce la limitación y dirige a los padres a un representante humano — no inventa una respuesta. Cada afirmación factual debe ser rastreable a un documento específico.

**Para el sistema de comunicación médica**, el desafío es mayor porque la IA está generando una impresión diagnóstica, no simplemente respondiendo una pregunta sobre un producto. El enfoque tiene varias capas:

Si la búsqueda en la literatura médica no arroja resultados relevantes, el sistema no genera una impresión diagnóstica en absoluto — el médico recibe los síntomas capturados con una nota clara de que no se recuperó evidencia de respaldo, y el caso queda marcado para revisión directa.

Si se recupera evidencia, la impresión diagnóstica de la IA debe ser rastreable a las fuentes específicas de las que provino. Las respuestas que no puedan ser atribuidas son rechazadas automáticamente.

El paso de validación VoBo del médico no es solo un mecanismo de cumplimiento — es la salvaguarda de precisión más importante del sistema. La IA puede estar ocasionalmente equivocada; el médico lo detecta, lo corrige, y esa corrección retroalimenta la mejora del sistema a lo largo del tiempo. Esta es la arquitectura que usted describió en su diagrama de proceso, y es el diseño correcto para una aplicación de carácter médico.

**Las pruebas requerirán su participación activa.** Antes de que el sistema entre en funcionamiento, necesitaremos que usted y, en la medida de lo posible, uno o dos médicos más ejecuten escenarios realistas y revisen cada impresión diagnóstica que genere la IA. Si la tasa de corrección — el porcentaje de evaluaciones de IA que los médicos modifican — es demasiado alta, el sistema no está listo. Una aprobación formal sobre la precisión es un requisito para la puesta en marcha, no una formalidad.

Queremos ser directos sobre esto porque es importante: nosotros construimos las salvaguardas, pero usted es la autoridad clínica que valida que funcionan en la práctica. Esta es una responsabilidad compartida, y consideramos que ese enfoque es exactamente el correcto para un sistema que opera en la frontera entre la tecnología y la medicina.

---

## Enfoque por Fases

Recomendamos construir por fases que entreguen valor de forma progresiva, gestionando la complejidad y el riesgo.

---

### Fase 1 — Descubrimiento y Especificación (Semanas 1–3)

Antes de comenzar cualquier desarrollo, invertimos tres semanas en definir los detalles con precisión. Esta fase resuelve todas las preguntas anteriores, mapea el flujo de trabajo completo, alinea el enfoque de cumplimiento y produce un documento de especificación que ambas partes revisan y aprueban antes de que comience el desarrollo.

Entregables:
- Documentación completa del flujo de trabajo
- Respuestas acordadas a todas las preguntas pendientes
- Enfoque de cumplimiento confirmado con asesor legal
- Especificación técnica revisada y aprobada por usted
- Carta de compromiso firmada

---

### Fase 2 — Asistente de Productos Infanzia (Semanas 3–14)

El más sencillo de los dos sistemas, construido primero. Un asistente de WhatsApp que conoce en detalle la línea de productos Infanzia — ingredientes, dosificación, certificaciones, dónde comprar — y puede responder preguntas de los padres con precisión a cualquier hora.

Usted lo administra a través de una interfaz web sencilla: cargue documentos de productos actualizados, revise registros de conversaciones y active o desactive el asistente.

Entregables:
- Asistente de WhatsApp en español (idiomas adicionales en una fase posterior)
- Base de conocimiento: Biomilk 1/2/3, Infabiotix, Infavit D3, Infavit Multivitamínico, Infavit Hierro, Coligas
- Interfaz de administración para gestión de documentos y revisión de conversaciones
- Límite estricto: sin consejos médicos, siempre remitir al médico

**Dependencia importante:** La activación de WhatsApp para uso empresarial requiere la aprobación de Meta (empresa propietaria de WhatsApp). Este proceso generalmente toma entre 2 y 4 semanas y debe iniciarse al comienzo de la Fase 1 — se ejecuta de forma independiente al desarrollo. Además, debido a que este sistema manejará datos de usuarios en Perú, la infraestructura de alojamiento debe configurarse para cumplir con los requisitos internacionales de privacidad de datos. Ambos aspectos se abordan durante el Descubrimiento para que no generen sorpresas más adelante, pero son factores reales en el cronograma general.

---

### Fase 3 — Sistema de Comunicación Fuera de Horario (Semanas 14–26)

El desarrollo principal, construido por etapas con su revisión en cada hito.

El motor de conversación — la parte que habla con los padres, captura síntomas, detecta emergencias y genera el resumen estructurado — se construye y prueba primero. El panel del médico lo sigue, incorporando todo lo de su diagrama: cola priorizada, impresión diagnóstica sugerida, VoBo, visualización de adjuntos y control de activación.

Entregables:
- Sistema de conversación por WhatsApp en español
- Detección de emergencias con derivación inmediata
- Captura estructurada de síntomas con integración de evidencia clínica (PubMed)
- Almacén de datos de pacientes (registros longitudinales, cifrados, conformes con normativa)
- Panel del médico: cola, transcripción, Impresión Dx., clasificación Desenlace, flujo VoBo
- Registro de auditoría completo de cada interacción

---

### Fase 4 — Voz y Expansión (Semanas 26–29, condicional)

Respuesta por voz (si se confirma como requisito), soporte de idiomas adicionales (inglés, portugués) y cualquier refinamiento basado en el uso real.

---

### Fase 5 — Retención Mensual Continua

Mantenimiento mensual, alojamiento, actualizaciones de la base de conocimiento (nuevos productos, guías clínicas actualizadas) y soporte. Alcance y costo determinados según el uso real.

---

## Cronograma Indicativo

| Fase | Semanas | Hito |
|---|---|---|
| Descubrimiento y Especificación | 1–3 | Especificación aprobada, carta de compromiso firmada |
| Asistente de Productos Infanzia | 3–14 | Asistente activo en WhatsApp |
| Sistema Fuera de Horario — Motor de Conversación | 14–20 | Sistema central probado |
| Panel del Médico | 20–26 | Sistema completo integrado y revisado |
| Voz y Expansión | 26–29 | Condicional según decisiones de la Fase 1 |
| Retención mensual continua | Mes 8+ | Mantenimiento y soporte inician |

### Dependencias del Cronograma

El cronograma anterior asume que se cumplen las siguientes condiciones. Los retrasos en cualquiera de estas áreas afectarán las fechas de entrega y serán comunicados oportunamente.

- **Aprobación de la API de WhatsApp Business** — El proceso de aprobación de Meta comienza al inicio de la Fase 1 y generalmente toma entre 2 y 4 semanas. El desarrollo continúa en paralelo, pero el sistema no puede entrar en operación hasta que se otorgue la aprobación.
- **Alojamiento y residencia de datos** — La infraestructura de alojamiento adecuada para datos de salud internacionales debe confirmarse durante el Descubrimiento. Esta determinación depende en parte del resultado de la revisión legal.
- **Retroalimentación y aprobación oportuna** — Cada fase requiere su revisión y aprobación escrita antes de que comience la siguiente. Los períodos de revisión prolongados desplazarán los hitos subsiguientes.
- **Acceso a documentación de productos** — La base de conocimiento del Asistente de Productos Infanzia se construye a partir de los documentos que usted proporcione. El acceso temprano a estos materiales mantiene la Fase 2 en cronograma.
- **Decisiones de la fase de Descubrimiento** — Las preguntas pendientes en este documento determinan directamente el alcance y la complejidad de la Fase 3. Las respuestas recibidas durante el Descubrimiento se incorporan a la especificación final antes de que comience cualquier trabajo de desarrollo.

### Aviso Importante

Todos los cronogramas, descripciones de alcance y estimaciones en este documento son indicativos y están sujetos a cambios. Se basan en la información disponible al momento de su redacción y en los supuestos indicados anteriormente. El alcance, cronograma e inversión finales se confirmarán en un Enunciado de Trabajo formal, revisado y firmado por ambas partes antes de que comience cualquier trabajo de desarrollo facturable. Los cambios de alcance después de la firma se gestionan mediante un proceso de orden de cambio escrita. Learned Geek no es responsable de los retrasos derivados de aprobaciones de terceros, dependencias del lado del cliente o requisitos regulatorios fuera de nuestro control.

---

## Inversión

Conversaremos sobre las cifras de inversión específicas en nuestra próxima reunión, una vez que las preguntas pendientes anteriores estén respondidas y el alcance esté confirmado.

Lo que podemos compartir ahora: la escala de este proyecto — dos sistemas, un marco de cumplimiento normativo, un panel médico y el cuidado requerido para una aplicación de carácter médico — representa una inversión significativa. La fase de Descubrimiento nos proporciona a ambos la información necesaria para llegar a una cifra que refleje el alcance real, no una estimación a ciegas.

El Descubrimiento en sí es un compromiso de alcance fijo con un costo definido. Podemos conversarlo en nuestra próxima llamada.

---

## Próximos Pasos Sugeridos

1. Confirmar que nuestra interpretación de sus respuestas anteriores es correcta
2. Programar una llamada de Descubrimiento de 30 minutos para resolver los elementos pendientes — le enviaremos dos opciones de horario
3. Una vez acordado el Descubrimiento, formalizamos el compromiso con un Enunciado de Trabajo y comenzamos

Estamos listos para avanzar cuando usted lo esté.

---

*Preparado por Mark McArthey — Learned Geek — markm@learnedgeek.com*  
*Confidencial — preparado exclusivamente para el Dr. Martin Núñez / Infanzia® / Kezer-Lab*
