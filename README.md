# TransitStopApp
A simple web app to display the next scheduled bus time for transit stops. 

## Submission Answers

**1. How many hours did you spend on it?**  
I spent around 15 hours designing, building, and testing this project.

**2. How would you scale this app for production under heavy load?**  
To ensure that production users can access the stop information under heavy load, infrastructure would need to be setup to horizontally scale the ASP.NET backend. First, the backend can be containerized using a tool such as Docker, creating an image that has all external dependencies. Then, a container orchestration tool such as Kubernetes can be used to dynamically scale the instances of the backend. This would include setting up a load balancer to distribute the requests between instances and creating rules for dynamically scaling the backend such as monitoring CPU and/or memory usage. It would also be important to factor the hosting environment (Cloud or Onprem) when designing this infrastructure.

For the database, The SQLite db could be swapped with a high performing and scalable relational DB such as SQL Server or PostgreSQL. Both DBs are good options and the decision would come down to factors such as standardization, cost, etc. 

**3. If you used AI tools, which ones and how?**  
For this assessment, I used ChatGPT to assist with the frontend. For example, I used it to with help structuring and apply minimal styling to the HTML. On the backend, I also utilized ChatGPT as a better version of Stack Overflow. For example, I was able to get information on the Entity Framework `.EnsureCreated()` method and how this method will create an SQLite DB if it doesn't already exist.

## API Endpoints

### `GET /api/stops`
Returns all stops ordered by `StopOrder`.

**Sample Response:**
```json
[
  { "id": 1, "name": "Beaudry Ave & 3rd St", "stopOrder": 1 },
  { "id": 2, "name": "Flower St & 7th St", "stopOrder": 2 }
]
```

### `GET /api/stops/{stopId}/next`

Returns the next scheduled stop time for the given `stopId`. If no stop is available for the current day, the earliest stop of the next day is returned. Returns 404 if no stop times are available.

Sample Success Response
```json
{ "nextStop": "7:33 PM" }
```

## Tech Stack
- **Backend**: ASP.NET Core, EF Core, SQLite
- **Frontend**: React
- **Testing**: xUnit, Moq

## Database Info
On the first run, the server will initialize the SQLite db in the `Database` folder according to the schema defined in the `Utility/TransitStopDbContext` class. Some sample data from route F will also be inserted on db initialization. On future runs, the existing .db file is used.

**Stop Table**: Each stop along a transit route.

**StopTime Table**: Scheduled stop time at a specific stop referencing the Stop table by `StopId`.
- `StopMinuteOfDay` stores the stop time in minutes since midnight for fast time comparison operations.
- An index on the StopTime table, (StopId, StopMinuteOfDay) is used to improve from `O(n)` to `O(log(n))` time complexity when querying next stop.

## Unit Testing
The `StopsController` class relies on several services that are provided with DI. Each of these low-level services has their own individual tests. Each controller endpoint also has high-level tests, using Moq to cover all scenarios without the overhead of configuring real dependencies.

## Exception Handling
There is intentionally no exception handling, as ASP.NET will return a 500 error from any endpoint that throws an exception. In production, there will likely need to be extra middleware added to correctly log exceptions. Any exceptions that can be handled with additional logic can be handled in the individual classes.

## How to Run
Note: Running in Visual Studio will start both the client and server.

### Server
```
cd TransitStopApp.Server
dotnet run
```
### Client
```
cd TransitStopApp.Client
npm run dev
```
### Unit Tests
```
cd TransitStopApp.Server.Tests
dotnet test
```
