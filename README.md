# Solution

## Run
Building and executing the docker-compose project will setup and create a container with SQL server for the database and a container for the API. Once it's up and running you can visit http://localhost:5000/swagger/index.html on a web browser to use the Swagger UI and test the API endpoints.

#### GET Example
**Request**
```curl -X GET "http://localhost:5000/api/expense/user/1?orderByAmount=false&orderByDate=false" -H "accept: text/plain"```

**Response**
200 OK
```json
[
  {
    "id": 2,
    "userFullName": "Anthony Stark",
    "date": "2022-12-30T11:02:04.3916493",
    "expenseType": "Restaurant",
    "amount": 10,
    "currency": "USD",
    "comment": "Expense User #1"
  }
]
```
#### POST Example
-**Request**
```curl -X POST "http://localhost:5000/api/expense" -H "accept: text/plain" -H "Content-Type: application/json" -d "{\"userId\": 1,\"date\": \"2022-12-30T11:33:54.341Z\",\"expenseType\": \"Restaurant\",\"amount\": 80,\"currency\": \"USD\",\"comment\": \"New expense\"}```

**Response**
201 CREATED
```json
{
  "id": 3,
  "userFullName": "Anthony Stark",
  "date": "2022-12-30T11:33:54.341Z",
  "expenseType": "Hotel",
  "amount": 60,
  "currency": "USD",
  "comment": "New expense"
}
```

## Architecture
I applied the project structure I've used on previous .NET projects (including the one at my current job). It is composed by the following layers:
 1. **Domain** Contains the enterprise logic, like the entities and their specifications.
 2. **Infrastructure** Contains the database context configuration. Because I used the code first approach, this layer contains the database DTOs which are used to map objects to the database tables. I also use a repository pattern to query and manipulate the objects in the database. The DB converters map the DB DTOs to their respective domain entitties. There is also a DB context initializer to initialize the database with some default data.
 3. **Application** Contains the business logic. I divided the logic between query service (operations that read and list data) and command service (operations that create / update / deletes data). The DTOs are structured in the same way, there is a command DTO which contains the properties the user must provide to create an entity and there is a query DTO which contains the properties that will be shown to the user when they query some information. The converters again are used to map the DTOs with the domain entities.
 4. **WebAPI** Contains the Web API management. The controllers serve the API endpoints and communicate with the application service to query or manipulate the data.
