# API quick reference

Base: `http://<API-VM>:5290/api/v1`

## Identity

```
GET  /identity/cccd
POST /identity/cccd/ocr     multipart field: image  (≤ 10 MiB, CCCD front)
```

## Face auth

```
POST /auth/face/enroll      JWT, face_1..face_5
POST /auth/face/set-pin     JWT, pin + confirmPin (6 digits)
GET  /auth/face/status      JWT
POST /auth/face/login       anonymous, field: face
POST /auth/face/verify-pin  challengeToken + pin
```

## Admin announcement targets

```
GET /admin/announcements/targets/{courses|classes|departments|students}?search=&limit=
```

## AI direct (AI VM)

```
POST http://<AI-VM>:8000/ocr              -F file=@cccd-front.jpg
POST http://<AI-VM>:8001/ai/face/enroll  user_id, username, face_1...
POST http://<AI-VM>:8001/ai/face/recognize -F face=@face.jpg
```

Default team LAN examples: API `172.29.50.31`, AI `172.29.50.34` — override in env/scripts.

## Email simulation (API VM localhost)

```
POST http://127.0.0.1:5289/email/send
GET  http://127.0.0.1:5289/email/messages?page=1&pageSize=20
```

API config: `Email:Provider=HttpSimulation`, `SimulationBaseUrl=http://127.0.0.1:5289`.
