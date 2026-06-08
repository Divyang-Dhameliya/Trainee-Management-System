# Task Tracker Application

## Technology Used
C#, .NET Core

## How to Run
- Import The project
- Run `dotnet run` command in command line terminal

## List of Apis
- GET /api/health 
- GET /api/trainees
- GET /api/trainees?search=param
- GET /api/trainees/{id}
- POST /api/trainees
- DELETE /api/trainees/{id}
- PUT /api/trainees
  
## Features Completed
- Health Api
- Trainee's CRUD operations

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

## Challenges Faced
 - Faced Security restriction related issues during initializing and running project
 - Faced challenges during api creation, To overcome that i have refer Docs & Web Search.
