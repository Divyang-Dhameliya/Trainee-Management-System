# Task Tracker Application

## Tech Stack

- ASP.NET Core Web ApI
- Entity Framework Core
- MySQL
- JWT Authentication

---

# Features Completed

- JWT Authentication
- Password Hashing
- CRUD APIs
- Pagination and Search
- Structured Logging
- Global Exception Hnadling
- CORS Configuration
- MySQL Db with EF Core

---

# Project Setup
 
## Prerequisites
 
- .NET 10 SDK
- MySQL Server
- Visual Studio / VS Code
 
---
 
# Clone Repository
 
```bash
git clone <repository-url>
cd TraineeManagement.Api
```
 
---
 
# Database Configuration
 
Update `appsettings.json`
 
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=trainee_management_db;user=root;password=your_password;"
  }
}
```
 
---
 
# EF Core Migration Commands
 
Create Migration
 
```bash
dotnet ef migrations add InitialCreate
```
 
Update Database
 
```bash
dotnet ef database update
```
 
Remove Migration
 
```bash
dotnet ef migrations remove
```
 
---
 
# Run Application
 
```bash
dotnet run
```
 
Swagger
 
```
https://localhost:xxxx/swagger
```
 
---

# Authentication
 
## Login API
 
### POST
 
```
/api/auth/login
```
 
Request
 
```json
{
  "username": "admin",
  "password": "Admin@123"
}
```
 
Response
 
```json
{
  "token": "jwt-token",
  "expiresIn": 3600,
  "user": {
    "id": 1,
    "username": "admin",
    "role": "Admin"
  }
}
```
 
---

# JWT Usage
 
Add token in request header
 
```http
Authorization: Bearer <JWT_TOKEN>
```
 
---

# API List
 
## Health
 
```
GET /api/health
```
 
---
 
## Authentication
 
```
POST /api/auth/register
POST /api/auth/login
```
 
---
 
## Trainees
 
```
GET    /api/trainees
GET    /api/trainees/{id}
POST   /api/trainees
PUT    /api/trainees/{id}
DELETE /api/trainees/{id}
```
 
Supports
 
```
GET /api/trainees?pageNumber=1&pageSize=10&search=amit&status=Active
```
 
---
 
## Mentors
 
```
GET    /api/mentors
GET    /api/mentors/{id}
POST   /api/mentors
PUT    /api/mentors/{id}
DELETE /api/mentors/{id}
```
 
---
 
## Learning Tasks
 
```
GET    /api/learning-tasks
GET    /api/learning-tasks/{id}
POST   /api/learning-tasks
PUT    /api/learning-tasks/{id}
DELETE /api/learning-tasks/{id}
```
 
---
 
## Task Assignments
 
```
POST /api/task-assignments
GET  /api/task-assignments
GET  /api/task-assignments/{id}
PUT  /api/task-assignments/{id}/status
```
 
---
 
## Submissions
 
```
POST /api/submissions
GET  /api/submissions
GET  /api/submissions/{id}
```
 
---
 
## Reviews
 
```
POST /api/reviews
GET  /api/reviews
GET  /api/reviews/{id}
```
 
---
  
## Sample Request & Response JSON

GET /api/health
  
- Response JSON
```
{
    "status": "running",
    "application": "Trainee Management API",
    "timestamp": "2026-06-08T10:20:38.1217329Z"
}
```

GET /api/trainees

- Response JSON
```
[
    {
        "id": 1,
        "firstName": "divyang",
        "lastName": "dhameliya",
        "email": "dd@gmail.com",
        "techStack": "html",
        "status": "Active",
        "createdDate": "2026-06-08T10:26:21.9333008Z",
        "updatedDate": "2026-06-08T10:26:21.9333532Z"
    },
    {
        "id": 2,
        "firstName": "khanjan",
        "lastName": "dhameliya",
        "email": "dd@gmail.com",
        "techStack": "html",
        "status": "Active",
        "createdDate": "2026-06-08T10:26:30.1974125Z",
        "updatedDate": "2026-06-08T10:26:30.1974132Z"
    }
]
```

GET /api/trainees?search=param

- param = "dhameliya"

- Response JSON 
```
[
    {
        "id": 1,
        "firstName": "divyang",
        "lastName": "dhameliya",
        "email": "dd@gmail.com",
        "techStack": "html",
        "status": "Active",
        "createdDate": "2026-06-08T10:26:21.9333008Z",
        "updatedDate": "2026-06-08T10:26:21.9333532Z"
    },
    {
        "id": 2,
        "firstName": "khanjan",
        "lastName": "dhameliya",
        "email": "dd@gmail.com",
        "techStack": "html",
        "status": "Active",
        "createdDate": "2026-06-08T10:26:30.1974125Z",
        "updatedDate": "2026-06-08T10:26:30.1974132Z"
    }
]
```

GET /api/trainees/{id}

- /api/trainees/1

- Response Json
```
{
    "id": 1,
    "firstName": "divyang",
    "lastName": "dhameliya",
    "email": "dd@gmail.com",
    "techStack": "html",
    "status": "Active",
    "createdDate": "2026-06-08T10:26:21.9333008Z",
    "updatedDate": "2026-06-08T10:26:21.9333532Z"
}
```

POST /api/trainees

- Request JSON
```
{
    "firstName": "Yash", 
    "lastName": "dhameliya", 
    "email": "yd@gmail.com", 
    "techStack": "html", 
    "status": "Active" 
}
```

- Response JSON
```
{
    "id": 3,
    "firstName": "Yash",
    "lastName": "dhameliya",
    "email": "yd@gmail.com",
    "techStack": "html",
    "status": "Active",
    "createdDate": "2026-06-08T10:33:56.3650615Z",
    "updatedDate": "2026-06-08T10:33:56.3650617Z"
}
```

DELETE /api/trainees/{id}

- /api/trainees/1

- Response: 
     `204 No Content` & `404 Not Found` 

PUT /api/trainees

- Request JSON
```
{
  "id": 1,
  "firstName": "changed",
  "lastName": "dhameliya",
  "email": "dd2@gmail.com",
  "techStack": "html",
  "status": "Active"
}
```

- Response JSON
```
{
    "id": 1,
    "firstName": "changed",
    "lastName": "dhameliya",
    "email": "dd2@gmail.com",
    "techStack": "html",
    "status": "Active",
    "createdDate": "2026-06-08T09:27:41.7218174Z",
    "updatedDate": "2026-06-08T10:01:44.165108Z"
}
```

---

# Security Checklist
 
✅ Passwords stored as hash only
 
✅ JWT Authentication enabled
 
✅ Protected APIs require token
 
✅ DTOs used to prevent excessive data exposure
 
✅ EF Core prevents SQL injection
 
✅ Secrets are not hardcoded
 
✅ CORS restricted to allowed origins
  
✅ Passwords and JWT tokens are not logged
 
---

# Known Limitations
 
- Refresh token support not implemented.
- Role-based authorization can be enhanced.
- No caching mechanism (Redis) is implemented.
  
---
 
# Future Improvements
 
- Refresh Tokens
- Role-based Authorization
- Docker Support
- Unit Testing
- Redis Caching
- CI/CD Pipeline
 
---

## Challenges Faced
 - Faced Security restriction related issues during initializing and running project
 - Faced challenges during api creation, To overcome that I have refered Docs & Web Search.
