# Todo API Spec

## API Resources

* **User** - A User represents an owner of Todos and corresponds one-to-many to a Todo.

* **Todo** - A Todo represents a task to be completed and corresponds many-to-one to a User.

* **Session** - A Session represents an authenticated user session and corresponds many-to-one to a User.

## Data Tables

### Users:
```
ID - Guid (PK)
Username - String
Password - String (Hash)
```

### Todos:
```
ID - Guid (PK)
UserID - Guid (FK - ID in Users)
Title - String
Status - Enumified String
CreationDate - Datetime
DueDate - Datetime
```

### Tokens:
```
ID - Guid (PK)
UserID - Guid (FK - ID in Users)
Token - String
```

## Data Models

### User
```
ID - Guid
Username - String
Password - String (Hash)
Todos - Todo[] (List of Todos for the user)
```

### Todo
```
ID - Guid
User - User
Title - String
Status - Status
CreationDate - Datetime
DueDate - Datetime
```

### Session
```
ID - Guid
Token - String (JWT from auth0)
User - User
Expiration - Datetime
```

### Status
```
OPEN
IN_PROGRESS
COMPLETE
```

## Endpoints

### Create New Todo
---

```
POST /todos
```

Headers:
```
"Authorization": Bearer <Token>
```

Body: 
```
{
    "meta": {},
    "data": {
        "title": "Buy groceries",
        "status": "OPEN|IN_PROGRESS|COMPLETE"
        "dueDate": "2022-04-01T13:02:37.000Z"
    }
}
```

**Response** - `201 Created`

Headers: 
```
"Location": "/todos/{id}"
```

**Error Codes**:

`400 Bad Request` - Request payload invalid OR dueDate provided not a valid date

`401 Unauthorized` - User/Client is not authenticated

### Fetch all Todos
---

**Request**:
``` 
GET /todos
```
Headers:
```
"Authorization": Bearer <Token>
```

Body: None

**Response** - `200 OK`

Body:
```
{
    "meta": {
        "todoCount": 2
    }, 
    "data": [
        {
            "id": "8a480c40-e4be-4b89-91c9-844de091a214",
            "title": "A todo",
            "status": "OPEN|IN_PROGRESS|COMPLETE",
            "creationDate": "2022-03-18T13:03:03.000Z",
            "dueDate": "2022-04-01T00:00:00.000Z"
        },
        {
            ...
        }
    ]
}
```

**Error Codes**:

`401 Unauthorized` - User/Client is not authenticated

### Update a Todo
---

**Request**:
```
PUT /todos/{id}
```

Headers:
```
"Authorization": Bearer <Token>
```

Body:
```
{
    "meta": {},
    "data": {
        "title": "My updated todo",
        "status": "OPEN|IN_PROGRESS|COMPLETE",
        "dueDate": "2022-04-02T00:00:00.000Z"
    }
}
```

**Response** - `204 No Content`

**Error Codes**:

`404 Not Found` - Todo with given ID does not exist in the API

`400 Bad Request` - Request payload invalid OR status provided not a valid enum value

`401 Unauthorized` - User/Client is not authenticated

`403 Forbidden` - User/Client does not have permission to edit todo

### Delete a Todo
---

**Request**:
```
DELETE /todos/{id}
```

Headers:
```
"Authorization": Bearer <Token>
```

Body: None

**Response** - `204 No Content`

**Error Codes**:

`404 Not Found` - Todo with given ID does not exist in the API

`401 Unauthorized` - User/Client is not authenticated

`403 Forbidden` - User/Client does not have permission to delete todo

### Create a User
---

**Request**:
```
POST /users
```

Body:
```
{
    "meta": {},
    "data": {
        "username": "tom_jerry",
        "password": "P@ssw0rd!"
    }
}
```

**Response** - `201 Created`

Headers:
```
"Location": "/users/{id}"
```

**Error Codes**:

`400 Bad Request` - Request payload invalid

`409 Conflict` - User with username already exists in API
Body:
```
{
    "meta": {
        "success": false,
        "msg": "username already exists"
    },
    "data": {}
}
```

### Fetch Users
---

**Request**:
```
GET /users
```

Body: None

**Response** - `200 OK`
Body:
```
{
    "meta": {
        "userCount": 4
    },
    "data": [
        {
            "id": "3fad06ad-88f5-432d-b91d-0415fdddf015",
            "username": "one_ok_rock"
        },
        {
            "id": "93383ee7-447f-4896-b60a-8e5a4584810b",
            "username": "trash"
        },
        {
            "id": "57976ec6-29dc-4125-826b-4c7eddacc6ca",
            "username": "life_awaits"
        },
        {
            "id": "235d67a7-a511-4bd4-8e85-59ad3c80416d",
            "username": "beyond"
        }
    ]
}
```

**Error Codes**: None

### Update a User (only password can be updated)
---

**Request**:
```
PUT /users/{id}/
```

Headers: 
```
"Authorization": Bearer <Token>
```

Body:
```
{
    "meta": {},
    "data": {
        "oldPassword": "password",
        "newPassword": "P@ssw0rd!"
    }
}
```

**Response** - `204 No Content`

**Error Codes**:

`404 Not Found` - User with ID does not exist in the API

`401 Unauthorized` - User/Client is not authenticated

`403 Forbidden` - User/Client does not have permission to update user

`400 Bad Request` - Request payload invalid OR oldPassword does not match password in the API

### Delete a User
---

**Request**:
```
DELETE /users/{id}
```

Headers:
```
"Authorization": Bearer <Token>
```

Body: None

**Response** - `204 No Content`

**Error Codes**:

`404 Not Found` - User with ID does not exist in the API

`401 Unauthorized` - User/Client is not authenticated

`403 Forbidden` - User/Client does not have permission to delete user

### Create a User Session (login)
---

**Request**:
```
POST /login
```

Body:
```
{
    "meta": {},
    "data": {
        "username": "tomandjerry",
        "password": "P@ssw0rd!"
    }
}
```

**Response** - `200 OK`
Body:
```
{
    "meta": {
        "success": true/false
    },
    "data": {
        "sessionId": "03e91265-dee1-4692-a1e8-4ba031424643",
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2Mj5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
        "userId": "235d67a7-a511-4bd4-8e85-59ad3c80416d"
}
```

**Error Codes**: None