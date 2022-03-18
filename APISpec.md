# Todo API Spec

## API Resources

* **User** - A User represents an owner of Todos and corresponds one-to-many to a Todo.

* **Todo** - A Todo represents a task to be completed and corresponds many-to-one to a User.

## Data Tables

### User:
```
Username - String (PK)
```

### Todo:
```
ID - Guid (PK)
Title - String
Username - String (FK User)
Status - Enumified String
CreationDate - Date
DueDate - Date
```

## Data Models

### User
```
Username - String
Todos - Todo[] (List of Todos for the user)
```

### Todo
```
ID - Guid
User - User
Title - String
Status - Status
CreationDate - Date
DueDate - Date
```

### Status
```
OPEN
IN_PROGRESS
COMPLETED
```

## Endpoints

### Create New Todo for User
---

```
POST /todo/{UserId}
```

Body: 
```
{
    "meta": {},
    "data": {
        "title": "Buy groceries",
        "dueDate": "2022-04-01"
    }
}
```

**Response** - 204 No Content

**Error Codes**:

`404 Not Found` - User ID does not exist in the API

`400 Bad Request` - Request payload invalid OR dueDate provided not a valid date


### Fetch all Todos for a User
---

**Request**:
``` 
GET /todo/{username}
```

Body: None

**Response** - 200 OK

Body:
```
{
    "meta": {
        "todoCount": 2
    }, 
    "data": {
         "todos": [
             {
                "id": "8a480c40-e4be-4b89-91c9-844de091a214",
                "title": "A todo",
                "status": "OPEN|IN_PROGRESS|COMPLETE",
                "creationDate": "2022-03-18",
                "dueDate": "2022-04-01"
            },
            {
                ...
            }
         ]
    }
}
```

**Error Codes**:

`404 Not Found` - User with username does not exist in the API

`404 Bad Request` - Request payload invalid OR status provided not a valid enum value


### Delete Todo
---

**Request**:
```
DEL /todo/{id}
```

Body: None

**Response** - 204 No Content

**Error Codes**:

`404 Not Found` - Todo with given ID does not exist in the API


### Update a Todo
---

**Request**:
```
PUT /todo/{id}
```

Body:
```
{
    "meta": {},
    "data": {
        "title": "My updated todo",
        "status": "OPEN|IN_PROGRESS|COMPLETED",
        "dueDate": "2022-04-02"
    }
}
```

**Response** - 204 No Content

**Error Codes**:
`404 Not Found` - Todo with given ID does not exist in the API

`400 Bad Request` - Request payload invalid OR status provided not a valid enum value


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
        "username": "tom_jerry"
    }
}
```

**Response** - 204 No Content

**Error Codes**:
`400 Bad Request` - Request payload invalid

`409 Conflict` - User with username already exists in API
Body:
```
{
    "meta": {},
    "data": {
        "msg": "username already exists"
    }
}
```


### Delete a User
---

**Request**:
```
DEL /users/{username}
```

Body: None

**Response** - 204 No Content

**Error Codes**:
`404 Not Found` - User with username does not exist in the API