# MicroserviceSolution

`MicroserviceSolution` is a `.NET 8` microservices sample built around an Ocelot API Gateway and three backend services: `Customer`, `Product`, and `Order`.

The solution demonstrates:

- API Gateway routing with `Ocelot`
- service separation by business domain
- layered architecture in backend services
- mixed persistence approaches
  Product uses SQL Server
  Order uses MongoDB
- Docker Compose for running the services together

## Solution Structure

```text
MicroserviceSolution
|-- Gateway/
|   `-- Gateway/
|-- Microservice/
|   |-- Customer/
|   |-- Product/
|   `-- Order/
|-- docker-compose.yml
`-- MicroserviceSolution.sln
```

## Projects

### Gateway

The gateway project is the public entry point to the system. It uses `Ocelot` to forward incoming HTTP requests to the internal microservices.

Main responsibilities:

- expose a single API surface
- route requests to the correct service
- simplify communication with internal services

Current gateway routing is configured in `Gateway/Gateway/ocelot.json`.

### Customer Service

The customer service is a lightweight ASP.NET Core Web API used for basic customer-related routing and service testing.

Current endpoint:

- `GET /api/Customer`

### Product Service

The product service follows a layered architecture and uses SQL Server through Entity Framework Core.

Layers:

- `Controllers`
- `Dto`
- `Services`
- `Repository`
- `Models`

Main features:

- product CRUD endpoints
- category CRUD endpoints
- product lookup by category

### Order Service

The order service uses the same layered structure as the product service, but keeps MongoDB as its persistence layer.

Layers:

- `Controllers`
- `Dto`
- `Services`
- `Repository`
- `Models`
- `Helper` for MongoDB configuration and context access

Main features:

- order CRUD endpoints
- order service health-style ping endpoint

## Architecture Overview

### API Flow

```text
                  +---------+
                  | Client  |
                  +----+----+
                       |
                       v
              +------------------+
              |  Ocelot Gateway  |
              +----+------+------+ 
                 |       |      |   
     +------------       |      ------------+
     |                   |                  |
     v                   v                  v
+------------+     +-------------+     +-----------+
| Customer   |     | Product     |     | Order     |
| Service    |     | Service     |     | Service   |
+------------+     +-------------+     +-----------+
```

### Product Service Architecture

```text
Controller -> Service -> Repository -> SQL Server
```

### Order Service Architecture

```text
Controller -> Service -> Repository -> MongoDB
```

## Technologies

- `.NET 8`
- `ASP.NET Core Web API`
- `Ocelot`
- `Entity Framework Core`
- `SQL Server`
- `MongoDB`
- `Docker Compose`

## Requirements

Before running the solution, make sure you have:

- `.NET 8 SDK`
- `Docker Desktop` if you want to run with containers
- `SQL Server` available for the product service
- `MongoDB` available for the order service

## Configuration

### Product Service

The product service reads its SQL Server connection string from:

- `Microservice/Product/appsettings.json`

Expected key:

```json
"ConnectionStrings": {
  "DefaultConnection": "YOUR_SQL_SERVER_CONNECTION_STRING"
}
```

Use your own local or development connection string. Do not commit production credentials.

### Order Service

The order service reads MongoDB settings from:

- `Microservice/Order/appsettings.json`

Expected keys:

```json
"MongoDb": {
  "ConnectionURl": "mongodb://localhost:27017",
  "DatabaseName": "MyDb",
  "CollectionName": "Orders"
}
```

## Running the Project

### Option 1: Run with Visual Studio

1. Open `MicroserviceSolution.sln`
2. Set multiple startup projects if needed
3. Start the gateway and the required services

### Option 2: Run from the command line

Run each service in a separate terminal.

Gateway:

```powershell
dotnet run --project Gateway/Gateway/Gateway.csproj
```

Customer:

```powershell
dotnet run --project Microservice/Customer/Customer.csproj
```

Product:

```powershell
dotnet run --project Microservice/Product/Product.csproj
```

Order:

```powershell
dotnet run --project Microservice/Order/Order.csproj
```

### Option 3: Run with Docker Compose

```powershell
docker compose up --build
```

This starts:

- `gateway`
- `customer-service`
- `product-service`
- `order-service`

Note:

- the compose file starts the API containers
- external database containers are not currently defined in the active compose configuration
- you may need local SQL Server and MongoDB running separately unless you extend `docker-compose.yml`

## Gateway Routes

Current configured routes include:

- `GET /api/Customer`
- `GET, POST /api/Product`
- `GET /api/Product/ping`
- `GET /api/Product/category/{categoryId}`
- `GET, PUT, DELETE /api/Product/{id}`
- `GET, POST /api/Category`
- `GET, PUT, DELETE /api/Category/{id}`
- `GET, POST /api/Order`
- `GET /api/Order/ping`
- `GET, PUT, DELETE /api/Order/{orderId}`

These routes are defined in `Gateway/Gateway/ocelot.json`.

## Example Endpoints

Customer:

- `GET /api/Customer`

Product:

- `GET /api/Product`
- `POST /api/Product`
- `GET /api/Product/{id}`
- `PUT /api/Product/{id}`
- `DELETE /api/Product/{id}`
- `GET /api/Product/category/{categoryId}`
- `GET /api/Product/ping`

Category:

- `GET /api/Category`
- `POST /api/Category`
- `GET /api/Category/{id}`
- `PUT /api/Category/{id}`
- `DELETE /api/Category/{id}`

Order:

- `GET /api/Order`
- `POST /api/Order`
- `GET /api/Order/{orderId}`
- `PUT /api/Order/{orderId}`
- `DELETE /api/Order/{orderId}`
- `GET /api/Order/ping`

## Notes

- The product service depends on a valid SQL Server connection string.
- The order service depends on a reachable MongoDB instance.
- The customer service is currently minimal compared to the product and order services.
- Gateway configuration should be kept in sync with controller routes as the services evolve.

## Future Improvements

- add authentication and authorization across services
- add database migrations for the product service
- add containerized SQL Server and MongoDB services to `docker-compose.yml`
- add tests for controllers, services, and repositories
- add centralized logging and health checks

## License

This project is provided for learning and development purposes.
