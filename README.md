# Shopping Application

## Introduction 
This is a comprehensive solution for tracking family expenses, including paid bills and purchased goods from stores.

### Project Goals
There are three main reasons for this solution:
 * Providing a customized tool for my parents to store and analyze expense data
 * Having a pet project that allows me to test different libraries and approaches in a kind-of-production solution  
 * Creating a portfolio solution to demonstrate my expertise and skills to potential employers

## Architecture
The solution is built using:
- **Frontend**: Blazor WebAssembly (.NET 8)
- **Backend**: ASP.NET Core Web API  
- **Database**: Microsoft SQL Server
- **Containerization**: Docker & Docker Compose
- **Testing**: Grafana K6 for load testing, SpecFlow/Reqnroll for BDD tests

## Prerequisites
- .NET 8 SDK (for development mode)
- Docker and Docker Compose (recommended for all scenarios)
- Visual Studio Community 2022 or later (optional, for development)

## Getting Started
There are two ways to run the application: Development and Production mode.

The solution requires MS SQL Server as the main data storage. I strongly recommend using Docker for both run modes.

### Infrastructure Setup
The `docker-compose.infrastructure.yml` file is provided in the root directory. It runs MS SQL Server in Docker and configures networking to allow service communication.

```bash
docker-compose -f docker-compose.infrastructure.yml up -d
```

### Development Mode
To run the solution in Development mode:

1. Ensure you have .NET 10 SDK installed
2. Start the infrastructure services (see above)
3. Run the Shopping\Server & Shopping\Client services

This can be done using Visual Studio Community 2022 or via command line:

```bash
# Start the API server
cd Shopping/Server
dotnet run

# Start the Blazor client (in another terminal)
cd Shopping/Client  
dotnet run
```

### Production Mode
To run the solution in Production mode:

1. Ensure Docker is installed
2. Use the `docker-compose.yml` file in the root directory to build and run all services:

```bash
docker-compose up --build
```

## Testing
The solution provides two types of testing approaches:

### Load Testing
- **Grafana K6** with implemented load and smoke test scripts
- Located in the `K6/` directory
- Includes various test scenarios for different endpoints

### BDD Testing  
- **Behavior-Driven Development** tests implemented using the Reqnroll framework
- Located in the `Shopping/SpecFlow/` directory
- Covers end-to-end functionality and business scenarios

## Project Structure
```
Shopping/
├── Client/           # Blazor WebAssembly frontend
├── Server/           # ASP.NET Core Web API backend  
├── Shared/           # Shared models and contracts
├── Shopping/         # Core business logic and data access
└── SpecFlow/         # BDD tests
```

## Contributing
This is a personal project, but feedback and suggestions are welcome through issues.
