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

## Table of Contents

- [Solution Structure](#solution-structure)
- [Projects](#projects)
  - [Gateway](#gateway)
  - [Customer Service](#customer-service)
  - [Product Service](#product-service)
  - [Order Service](#order-service)
- [Architecture Overview](#architecture-overview)
  - [API Flow](#api-flow)
  - [Product Service Architecture](#product-service-architecture)
  - [Order Service Architecture](#order-service-architecture)
- [Technologies](#technologies)
- [Running the Project](#running-the-project)
  - [Option 1: Run with Visual Studio](#option-1-run-with-visual-studio)
  - [Option 2: Run from the command line](#option-2-run-from-the-command-line)
  - [Option 3: Run with Docker Compose](#option-3-run-with-docker-compose)
- [Notes](#notes)
- [Future Improvements](#future-improvements)
  
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
                 |       |       |   
     +------------       |       ------------------+
     |                   |                         |
     v                   v                         v
+------------+     +-------------+              +-----------+
| Customer   |     | Product     | <---gRPC---> | Order     |
| Service    |     | Service     |              | Service   |
+------------+     +-------------+              +-----------+
      | EF               |                          | EF
+------------+     +-------------+              +-----------+
| Sql Server |     | Sql Server  |              | mongodb   |
+------------+     +-------------+              +-----------+
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


Note:

- the compose file starts the API containers
- external database containers are not currently defined in the active compose configuration
- you may need local SQL Server and MongoDB running separately unless you extend `docker-compose.yml`

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
