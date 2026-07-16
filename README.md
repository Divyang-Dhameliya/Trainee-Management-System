# Trainee Management System

A backend system to manage trainees, mentors, learning tasks, task assignments, submissions, and reviews — with async file processing.

## Architecture

```
                    ┌──────────────────────┐
                    │   TraineeManagement  │  (main API — auth, trainees,
                    │   .Api  (port 8080)  │   mentors, tasks, submissions)
                    └──────────┬───────────┘
                         │           │
        ┌────────────────┼───────────┼──────────────────┐
        │                │           │                  │
   ┌────▼────┐      ┌────▼────┐  ┌───▼────┐     ┌────────▼────────┐
   │  MySQL  │      │  Redis  │  │RabbitMQ│     │ TrainingDirectory│
   │ (data)  │      │ (cache) │  │ (queue)│     │ .Api (port 8081) │
   └─────────┘      └─────────┘  └───┬────┘     │ (external trainee│
                                      │          │  profile lookup) │
                               ┌──────▼───────┐  └──────────────────┘
                               │ Submission   │
                               │ Processor    │
                               │ .Worker      │
                               │ (background) │
                               └──────────────┘
```

- **TraineeManagement.Api** – main REST API, talks to MySQL, Redis, RabbitMQ, and calls **TrainingDirectory.Api** for trainee profile info (inter-service call).
- **TrainingDirectory.Api** – small separate service exposing trainee profile data.
- **SubmisionProcessor.Worker** – background worker, consumes submission-processing messages from RabbitMQ and updates job/submission status in MySQL.
- Shared **uploads** volume between API and Worker for submission files.

## Configuration Guide

Config is via environment variables (see `.env.example`), consumed by `docker-compose.yml`.

Key groups:
- **Database**: `MYSQL_ROOT_PASSWORD`, `MYSQL_DATABASE`
- **Redis**: `REDIS_CONNECTION_STRING`
- **RabbitMQ**: `RABBITMQ_HOST/USER/PASS/QUEUE_NAME`
- **JWT Auth**: `JWT_SECRET_KEY`, `JWT_ISSUER`, `JWT_AUDIENCE`, `JWT_EXPIRY_MINUTES`
- **File storage**: `FILE_STORAGE_ROOT_PATH`, `WORKER_STORAGE_ROOT_PATH`, `FILE_MAX_SIZE_MB`, `ALLOWED_EXT_0..3`
- **Inter-service**: `TRAINING_DIRECTORY_API_URL` (main API → TrainingDirectory.Api)

Steps:
1. Copy `.env.example` → `.env`
2. Fill in real values (passwords, JWT secret, etc.)

## Run with Docker Compose

```bash
cp .env.example .env
# edit .env with real values
docker compose up --build
```

Services started: `mysql` (3307→3306), `redis` (6379), `rabbitmq` (5672 + management UI 15672), `trainingdirectory.api` (8081→8080), `traineemanagement.api` (8080), `submisionprocessor.worker` (no exposed port).

API and worker wait for MySQL/Redis/RabbitMQ health checks before starting.

## API List (TraineeManagement.Api)

| Area | Endpoints |
|---|---|
| Auth | `POST /api/auth/register`, `POST /api/auth/login` |
| Health | `GET /api/health`, `GET /api/health/ready`, `GET /api/health/live` |
| Trainees | `GET/POST /api/trainees`, `GET/PUT/DELETE /api/trainees/{id}`, `GET /api/trainees/{id}/dispatch` (inter-service) |
| Mentors | `GET/POST /api/mentors`, `GET/PUT/DELETE /api/mentors/{id}` |
| Learning Tasks | `GET/POST /api/learning-tasks`, `GET/PUT/DELETE /api/learning-tasks/{id}` |
| Task Assignments | `GET/POST /api/task-assignments`, `GET /api/task-assignments/{id}`, `PUT /api/task-assignments/{id}/status` |
| Submissions | `GET/POST /api/submissions`, `GET /api/submissions/{id}`, `POST /api/submissions/{id}/files`, `GET /api/submission-files/{fileId}/download`, `DELETE /api/submission-files/{fileId}` |
| Reviews | `GET/POST /api/reviews`, `GET /api/reviews/{id}` |
| Processing Jobs | `GET /api/processing-jobs/{id}` |

## Backend Services for Async Processing

- Submission file upload → API publishes a `SubmissionProcessingRequested` message to RabbitMQ and creates a `ProcessingJob` record.
- **SubmisionProcessor.Worker** consumes the queue (quorum queue, prefetch=1), processes the file (currently simulated with a delay), and updates `Submission` and `ProcessingJob` status (Processing → Completed/Failed) in MySQL.
- Retry/failure handling: transient errors are requeued (up to 2 retries via `x-delivery-limit`); permanent errors or exhausted retries route to a Dead Letter Queue (`{queue}.failed`) via a Dead Letter Exchange.
- Job status can be polled via `GET /api/processing-jobs/{id}`.

## Known Limitations

- File processing logic is a stub (`Task.Delay`), not real content processing.
- File storage is local disk (shared Docker volume), not cloud/object storage.
- TrainingDirectory.Api's endpoint currently returns hardcoded/mock trainee data.

## Design Decisions

- **Service separation**: trainee-profile lookups are split into a separate `TrainingDirectory.Api` to simulate/allow independent scaling and ownership boundaries, called via inter-service HTTP.
- **Async processing via message queue**: heavy/slow submission processing is decoupled from the request/response cycle using RabbitMQ + a dedicated worker, with job status tracked in the database for polling.
- **Reliability**: quorum queues + DLQ pattern for guaranteed processing with bounded retries instead of infinite retry loops.
- **Caching**: Redis used as a cache layer (`ICacheService`) in the main API to reduce DB load.
- **JWT-based auth**: stateless authentication for API access control.
- **Containerized microservices**: each component (API, secondary API, worker, MySQL, Redis, RabbitMQ) runs as its own container, orchestrated via Docker Compose with health-check gated startup order.
