---
name: unicore-backend
description: >-
  UniCore .NET backend (be_unicore): handlers, announcements, SQL on API VM,
  OCR/Face AI over LAN (172.29.50.x), CCCD front 10MiB, face PIN 6 digits.
  Use for be_unicore, UniCore.API, AiOcr, FaceAi, identity, or dev deploy docs.
---

# UniCore Backend

## Repo layout

`UniCore.Backend/` (API, Application, Infrastructure, Helper), `UniCore.Database/`, `docs/`, `scripts/`.

Remote: `https://github.com/ducdat0402/be_unicore` — branch `main`.

## Dev deploy (LAN / VM)

**Full guide:** [docs/DEV-AI-DEPLOY.md](../../docs/DEV-AI-DEPLOY.md)

| Host | Role |
|------|------|
| API VM (e.g. `172.29.50.31:5290`) | UniCore.API + **SQL Server `localhost`** |
| AI VM (e.g. `172.29.50.34`) | OCR `:8000`, Face `:8001` |
| Developer laptop | HTTP to API only; optional direct AI curl if firewall allows |

Config on **API VM** `appsettings.json`: `AiOcr.BaseUrl`, `FaceAi.BaseUrl` → AI VM. Not on laptop.

**Smoke:** `./scripts/smoke-ai.sh [cccd-front.jpg] [face.jpg]` — env `API_BASE`, `AI_OCR_BASE`, `AI_FACE_BASE`.

## Application patterns

Custom `IRequest` / `IRequestHandler` + `HandleAsync` (not MediatR). Handlers registered via `*Handler` scan in `ApplicationDependencyInjection.cs`.

## Identity + AI

| Feature | BE route | AI | Limits |
|---------|----------|-----|--------|
| CCCD OCR | `POST /api/v1/identity/cccd/ocr` field **`image`** | `POST /ocr` field **`file`** | 10 MiB, **front only** |
| Face enroll | `POST /api/v1/auth/face/enroll` | `/ai/face/enroll` | 1–5 images, 10 MiB each |
| Face login + PIN | `login` → `verify-pin` | `/ai/face/recognize` | PIN **6 digits**, embedding on **AI** |

Clients: `AiOcrHttpClient`, `FaceAiHttpClient`. SQL stores `embedding_id` + PIN hash only.

## Announcements

See [reference.md](reference.md). Controllers: `AnnouncementControllers.cs`. Target search: `{ data, meta }` without `BaseAPIResponse`.

## Email simulation (dev, same VM as API)

**Guide:** [docs/DEV-EMAIL-SIM.md](../../docs/DEV-EMAIL-SIM.md)

- Run `StimulationEmailProvider` on **`http://127.0.0.1:5289`**
- API: `Email:Enabled=true`, `Provider=HttpSimulation`, `SimulationBaseUrl=http://127.0.0.1:5289`
- `HttpSimulationEmailSender` → `POST /email/send`; inbox `GET /email/messages`
- Used by `AnnouncementDeliveryService` (IMPORTANT/URGENT + whitelist)

**Smoke:** `./scripts/smoke-email.sh`

## Key paths

- AI deploy: `docs/DEV-AI-DEPLOY.md`
- Email sim: `docs/DEV-EMAIL-SIM.md`
- REST samples: `Identity-Face.http`, `Announcements.http`, `StimulationEmailProvider.http`
- Example config: `UniCore.Backend/UniCore.API/appsettings.Example.json`
