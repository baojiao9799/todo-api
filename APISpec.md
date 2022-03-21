# Todo API Spec

## API Resources

* **Todo** - A Todo represents a task to be completed.

## Data Tables

### Todo:
```
ID - Guid (PK)
Title - String
Status - Enumified String
CreationDate - Datetime
DueDate - Datetime
```

## Data Models

### Todo
```
ID - Guid
Title - String
Status - Status
CreationDate - Datetime
DueDate - Datetime
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
POST /todo
```

Body: 
```
{
    "meta": {},
    "data": {
        "title": "Buy groceries",
        "dueDate": "2022-04-01T13:02:37.000Z"
    }
}
```

**Response** - `204 No Content`

**Error Codes**:

`400 Bad Request` - Request payload invalid OR dueDate provided not a valid date


### Fetch all Todos
---

**Request**:
``` 
GET /todo
```

Body: None

**Response** - `200 OK`

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
                "creationDate": "2022-03-18T13:03:03.000Z",
                "dueDate": "2022-04-01T00:00:00.000Z"
            },
            {
                ...
            }
         ]
    }
}
```

**Error Codes**:

`404 Bad Request` - Request payload invalid OR status provided not a valid enum value


### Delete Todo
---

**Request**:
```
DELETE /todo/{id}
```

Body: None

**Response** - `204 No Content`

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
        "status": "OPEN|IN_PROGRESS|COMPLETE",
        "dueDate": "2022-04-02T00:00:00.000Z"
    }
}
```

**Response** - `204 No Content`

**Error Codes**:
`404 Not Found` - Todo with given ID does not exist in the API

`400 Bad Request` - Request payload invalid OR status provided not a valid enum value