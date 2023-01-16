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
