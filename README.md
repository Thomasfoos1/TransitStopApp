# TransitStopApp
A simple web app to display the next scheduled bus time for transit stops. 

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
{ "nextStop": "07:33" }
```

## Tech Stack
- **Backend**: ASP.NET Core, EF Core, SQLite
- **Frontend**: React
- **Testing**: XUnit, Moq

## How to Run
Note: Running in visual studio will start both the client and server.

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
