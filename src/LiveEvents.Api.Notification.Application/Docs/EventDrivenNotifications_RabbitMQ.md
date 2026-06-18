# Notificaciones Event-Driven (RabbitMQ) — Diseño Futuro

Este documento describe una solución “correcta” (arquitectónicamente) para evolucionar el módulo de notificaciones hacia un enfoque **event-driven** usando un broker como **RabbitMQ**, manteniendo **abierto a extensión** (nuevos eventos/canales) y **cerrado a modificación** (evitar tocar lógica existente al agregar capacidades).

La implementación actual del módulo `Notification` ya soporta **Strategy por canal** (`INotificationChannelHandler` + `NotificationDispatcher`). El paso futuro es desacoplar “quién dispara” (cualquier API) de “quién decide y envía” (Notification Service) mediante **eventos + colas**.

---

## Objetivo

- Las APIs de negocio (Authentication / Appointments / Orders / etc.) NO envían correos/SMS/push directamente.
- Las APIs publican **eventos de negocio**: `UserPasswordChanged`, `AppointmentReserved`, etc.
- `LiveEvents.Api.Notification` consume esos eventos y aplica:
  - reglas de notificación por evento,
  - selección de canales (InApp, Email, SMS, Push),
  - persistencia,
  - reintentos, idempotencia y trazabilidad.

---

## Componentes (Recomendado)

### 1) Productores (cualquier API)

- Publican eventos hacia RabbitMQ.
- Recomendado: usar **Outbox Pattern** para publicación confiable.

**Outbox Pattern (por servicio productor)**
- Tabla `outbox_messages` (en la misma DB/transacción del negocio).
- Background worker “OutboxPublisher” que:
  1) lee mensajes pendientes,
  2) publica a RabbitMQ,
  3) marca como publicados.

Esto evita “se guardó la cita, pero no se publicó el evento” por caídas.

### 2) Broker RabbitMQ

- Exchange tipo `topic` para eventos de negocio.
- Un queue dedicado para el servicio Notification.
- Dead-letter exchange (DLX) para mensajes que fallan repetidamente.

### 3) Consumidor (Notification Service)

- “EventConsumer” (HostedService) que:
  1) deserializa el evento,
  2) aplica idempotencia (Inbox/Dedup),
  3) ejecuta la lógica de notificación,
  4) confirma ack del mensaje.

### 4) Dispatcher por canal (ya existe)

- `INotificationChannelHandler` por canal (InApp hoy, Email/SMS/Push mañana).
- `NotificationDispatcher` decide el handler según `NotificationChannel`.

---

## Topología RabbitMQ (Ejemplo)

### Exchange
- `domain.events` (type: `topic`, durable)

### Routing keys (convención)
- `auth.user.password_changed.v1`
- `appointments.appointment.reserved.v1`
- `appointments.appointment.cancelled.v1`

### Queue Notification
- `notification-service.events` (durable)
- Bindings:
  - `auth.*.*.v1`
  - `appointments.*.*.v1`
  - o más específico según necesidad

### Dead-letter
- DLX: `domain.events.dlx`
- DLQ: `notification-service.events.dlq`

---

## Contrato de Mensaje (Recomendado)

Usar un envelope estándar tipo “CloudEvents-like” para versionar y trazar.

```json
{
  "id": "b0f5e1c5-2e34-4bdb-8d9e-2b8c420d2b0e",
  "type": "appointments.appointment.reserved.v1",
  "occurredAt": "2026-06-18T12:34:56Z",
  "correlationId": "d8f7d8b6-7e37-4b5a-9f25-8c1de1e0c22a",
  "causationId": "f3efb1c2-62dd-4e8f-bb7f-01b7b28e6d17",
  "producer": "LiveEvents.Api.Appointments",
  "data": {
    "appointmentId": "…",
    "userId": "…",
    "startsAt": "…",
    "serviceName": "…"
  }
}
```

Notas:
- `type` incluye versión (`v1`) para evolucionar sin romper consumidores.
- `correlationId` se propaga desde la request original (observabilidad).
- `id` es clave para idempotencia (deduplicación).

---

## Idempotencia (Necesaria)

Cuando hay reintentos, un mismo mensaje puede procesarse más de una vez. El consumidor debe ser idempotente.

### Inbox Pattern (por consumidor)
- Tabla `inbox_messages` (en Notification DB):
  - `message_id` (PK/unique)
  - `type`
  - `received_at`
  - `processed_at`
  - `status`
- Flujo:
  1) al recibir mensaje, intenta insertar `message_id`
  2) si ya existe, hace `ack` y termina
  3) si no existe, procesa
  4) marca `processed_at`

Esto hace que procesar sea “exactly-once” *a nivel de efectos*.

---

## Reintentos y Errores

### Recomendación
- Reintentos controlados (en consumidor) para fallos transitorios.
- Para fallos permanentes:
  - `nack` con requeue false y enviar a DLQ (vía DLX),
  - o publicar un “error event” para auditoría.

### Observabilidad mínima
- log estructurado con `messageId`, `type`, `correlationId`.
- métricas:
  - mensajes consumidos,
  - mensajes fallidos,
  - mensajes enviados a DLQ,
  - latencia de procesamiento.

---

## Cómo “mapear” eventos a notificaciones (OCP real)

Separar:
1) **Evento** (lo que pasó)
2) **Política** (qué notificaciones generar)
3) **Canales** (cómo enviarlas)

### Diseño recomendado en Notification.Application

1) `INotificationEventHandler<TEvent>`
- Un handler por tipo de evento.
- Convierte evento -> una o varias “NotificationDispatchRequest”.

2) `INotificationPolicy`
- Decide canales por usuario/tipo (ej. usuarios prefieren email, otros solo in-app).

3) `NotificationDispatcher` (ya existe)
- Ejecuta envío por canal.

La extensión se vuelve:
- agregar un nuevo `TEvent` + handler,
- agregar un nuevo canal handler,
- sin tocar el dispatcher existente.

---

## Ejemplo de interfaces (futuro)

```csharp
public interface IIntegrationEvent
{
    Guid Id { get; }
    string Type { get; }
    DateTime OccurredAtUtc { get; }
    string? CorrelationId { get; }
}

public interface IIntegrationEventHandler<in TEvent>
    where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}

public interface IEventBusPublisher
{
    Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken);
}

public interface IEventBusConsumer
{
    Task StartAsync(CancellationToken cancellationToken);
}
```

---

## Flujo End-to-End (Ejemplo: reserva de cita)

1) `Appointments API` hace `ReserveAppointment` y guarda la cita.
2) En la misma transacción, inserta en `outbox_messages`:
   - `type = appointments.appointment.reserved.v1`
   - `data = { appointmentId, userId, … }`
3) `OutboxPublisher` publica el evento en `domain.events` con routing key `appointments.appointment.reserved.v1`.
4) `Notification Service` consume del queue `notification-service.events`.
5) Inserta `messageId` en `inbox_messages` (idempotencia).
6) Ejecuta handler del evento:
   - crea notificación InApp,
   - (futuro) crea notificación Email/SMS según política.
7) `ack` del mensaje.

---

## Nota sobre seguridad

- No publiques PII innecesaria en eventos (correo/teléfono) si no es requerido.
- Publica identificadores (`userId`) y datos mínimos; Notification consulta detalles si hace falta.
- Firmar/encriptar mensajes solo si el entorno lo requiere (depende de infraestructura).

---

## Integración con lo ya implementado

La lógica actual de envío por canal se reutiliza:
- `NotificationDispatcher` sigue siendo el punto central para “enviar por canal”.
- Los futuros `EventHandlers` solo producen `NotificationDispatchRequest` y llaman al dispatcher.

---

## Resumen

- Para una plantilla “tipo prueba técnica”, la solución ideal a futuro es:
  - **Outbox (en productores) + RabbitMQ + Inbox (en consumidor)**
  - Handlers por evento + handlers por canal (Strategy)
  - DLQ, reintentos e idempotencia desde el día 1 del enfoque event-driven

