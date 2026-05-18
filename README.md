# 🔐 RBAC Microservice

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](LICENSE.md)
[![Tests](https://img.shields.io/badge/tests-passing-success)](tests/)
[![Coverage](https://img.shields.io/badge/coverage-80%25-green)]()
[![Docker](https://img.shields.io/badge/docker-ready-2496ED?logo=docker)](Dockerfile)

A production-ready microservice for Role-Based Access Control (RBAC) built with .NET 9. Implements Clean Architecture, DDD, and CQRS patterns to manage permissions, roles, and resource access across distributed systems.

---

## What is this microservice?

The RBAC microservice is the security brain of the platform. It answers the question "can this user perform this action on this endpoint?" by mapping roles to specific API operations across all microservices. It solves the problem of centralized, dynamic authorization without hardcoding permissions into application code. Every microservice queries RBAC on startup (and periodically refreshes) to know which roles are allowed to call which endpoints. Platform administrators use the REST API to add or revoke permissions in real time, and those changes propagate automatically to all services without redeployment.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Technology Stack](#️-technology-stack)
- [Prerequisites](#️-prerequisites)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)
- [RBAC Model](#-rbac-model)
- [Configuration](#️-configuration)
- [Use Cases & Scenarios](#-use-cases--scenarios)
- [Architecture](#️-architecture)
- [Testing](#-testing)
- [Best Practices](#-best-practices)
- [Troubleshooting](#-troubleshooting)
- [Authorization Flow](#-authorization-flow)
- [Integration](#-integration)
- [Security](#-security)
- [FAQ](#-faq)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

The RBAC microservice provides a centralized authorization system for managing role-based access control across microservices. It defines permissions by mapping roles to resources (microservice endpoints) and exposes these mappings via REST and gRPC APIs for real-time authorization checks.

- **Permission Management**: Define which roles can access which resources (Module → Service → Controller → Action → HTTP Method)
- **Role-to-Resource Mapping**: Associate roles with specific API endpoints across all microservices
- **Multi-Tenant Support**: Isolate RBAC configurations by tenant
- **Real-Time Authorization**: Other microservices query RBAC to validate user permissions
- **Distributed Caching**: Redis caching for high-performance authorization checks
- **Dynamic Updates**: Add/remove permissions without redeploying microservices
- **gRPC Integration**: Fast inter-service permission lookups via gRPC

### 🚀 Quick Start

```bash
# 1. Start infrastructure services
git clone https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev
cd CodeDesignPlus.Environment.Dev/resources
docker-compose up -d

# 2. Configure Vault secrets
cd ../../tools/vault
./config-vault.sh

# 3. Run the microservice
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Rbac.Rest

# 4. Access Swagger UI
open http://localhost:5000/swagger
```

### 📊 High-Level Architecture

```
┌─────────────────┐         ┌─────────────────┐
│  Microservice A │         │  Microservice B │
│   (Users API)   │         │  (Orders API)   │
└────────┬────────┘         └────────┬────────┘
         │ gRPC: GetRbac(ms-users)   │ gRPC: GetRbac(ms-orders)
         │                            │
         └──────────┬─────────────────┘
                    │
         ┌──────────▼──────────────────────────────────┐
         │     RBAC Microservice (gRPC + REST)         │
         │  ┌────────────┐  ┌──────────┐  ┌─────────┐ │
         │  │ Controllers│─▶│ MediatR  │─▶│Handlers │ │
         │  └────────────┘  └──────────┘  └────┬────┘ │
         │                                      │      │
         │  ┌───────────────────────────────────▼───┐ │
         │  │       RbacAggregate                   │ │
         │  │  - Role: "Admin"                      │ │
         │  │  - Resource: ms-users/UserController/ │ │
         │  │              GetUsers/GET             │ │
         │  └───────────────────────────────────────┘ │
         └──┬─────────────────┬──────────────────┬────┘
            │                 │                  │
       ┌────▼────┐      ┌─────▼─────┐    ┌──────▼──────┐
       │ MongoDB │      │   Redis   │    │  RabbitMQ   │
       │ (Perms) │      │ (Cache)   │    │  (Events)   │
       └─────────┘      └───────────┘    └─────────────┘
```

## 🚀 Key Features

### Core Capabilities

- ✅ **Role Management**: Define and manage roles with descriptive names and metadata
- ✅ **Permission Mapping**: Map roles to specific resources (Controller.Action + HTTP Method)
- ✅ **Resource Hierarchy**: Organize permissions by Module → Service → Controller → Action → Method
- ✅ **Multi-Tenant RBAC**: Separate permission sets per tenant
- ✅ **Dynamic Authorization**: Add/remove/update permissions without code changes
- ✅ **Real-Time Queries**: Fast permission lookups for authorization middleware
- ✅ **gRPC Service Discovery**: Other microservices query RBAC via gRPC for their permission sets
- ✅ **Distributed Caching**: Redis caching for sub-millisecond authorization checks
- ✅ **Permission Lifecycle**: Create, update, activate/deactivate, delete permissions
- ✅ **Domain Events**: Publish RBAC changes to RabbitMQ for system-wide notification

### Technical Features

- Clean Architecture with DDD and CQRS
- Domain events for permission state changes
- MongoDB for permission persistence
- RabbitMQ for event publishing
- Redis for distributed caching with automatic refresh
- OAuth2/OpenID Connect security
- Multi-tenancy support
- Swagger/OpenAPI documentation (REST)
- gRPC reflection for service discovery
- Docker containerization
- Comprehensive test coverage (Unit, Integration)

## 🛠️ Technology Stack

### Core
- **.NET 9** - Runtime and framework
- **ASP.NET Core** - Web API framework
- **C# 13** - Programming language
- **gRPC** - High-performance inter-service communication

### Storage & Data
- **MongoDB** - Permission persistence and queries
- **Redis** - Distributed caching for authorization checks

### Messaging & Events
- **RabbitMQ** - Event publishing for permission changes

### Architecture & Patterns
- **MediatR** - CQRS command/query handling
- **FluentValidation** - Input validation
- **Mapster** - Object mapping
- **NodaTime** - Date/time handling

### Security & Configuration
- **Vault** - Secret management
- **OAuth2/OpenID Connect** - Authentication
- **JWT Bearer** - Token-based security
- **HTTPS** - Encrypted communication

### DevOps & Testing
- **Docker** - Containerization
- **xUnit** - Unit/integration testing
- **Swagger/OpenAPI** - REST API documentation
- **gRPC Reflection** - gRPC service discovery

## ⚙️ Prerequisites

### Required
- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker & Docker Compose** - For infrastructure services
- **MongoDB 6.0+** - Document database
- **Redis 7.0+** - Caching layer
- **RabbitMQ 3.12+** - Message broker

### Optional
- **Vault** - Secret management (can use appsettings for local dev)
- **gRPC UI** - Test gRPC endpoints (grpcui)

## 🚀 Getting Started

The following instructions will help you set up the project on your local machine for development and testing purposes.

1. Clone the repository:
```bash
git clone <repository-url>
cd CodeDesignPlus.Net.Microservice.Rbac
```

2. Run the MongoDB, Redis, and RabbitMQ services using Docker Compose. Clone this repository [CodeDesignPlus.Environment.Dev](https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev) and run the following command:

```bash
cd resources
docker-compose up -d
```

3. Run the script to config the vault:

```bash
cd tools/vault
./config-vault.sh
```

4. Build the solution:
```bash
dotnet build
```

5. Run the desired entry point:
   
   - For REST API:
      ```bash
      dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Rbac.Rest
      ```

   - For gRPC:
      ```bash
      dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Rbac.gRpc
      ```

6. Access the APIs:
   - REST API: `http://localhost:5000/swagger`
   - gRPC: `http://localhost:5001` (use grpcui or Postman)

## 📡 API Endpoints

### REST API

#### Create RBAC Configuration
```http
POST /api/rbac
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "User Management RBAC",
  "description": "Permissions for user management operations",
  "rbacPermissions": [
    {
      "id": "perm-001",
      "role": {
        "id": "role-admin",
        "name": "Admin"
      },
      "resource": {
        "id": "res-001",
        "module": "UserManagement",
        "service": "ms-users",
        "controller": "UserController",
        "action": "GetUsers",
        "method": "GET"
      }
    },
    {
      "id": "perm-002",
      "role": {
        "id": "role-admin",
        "name": "Admin"
      },
      "resource": {
        "id": "res-002",
        "module": "UserManagement",
        "service": "ms-users",
        "controller": "UserController",
        "action": "CreateUser",
        "method": "POST"
      }
    },
    {
      "id": "perm-003",
      "role": {
        "id": "role-viewer",
        "name": "Viewer"
      },
      "resource": {
        "id": "res-003",
        "module": "UserManagement",
        "service": "ms-users",
        "controller": "UserController",
        "action": "GetUsers",
        "method": "GET"
      }
    }
  ]
}
```

**Response**: `204 No Content`

#### Get RBAC by ID
```http
GET /api/rbac/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `200 OK` with RBAC details
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "User Management RBAC",
  "description": "Permissions for user management operations",
  "permissions": [
    {
      "id": "perm-001",
      "role": {
        "id": "role-admin",
        "name": "Admin"
      },
      "resource": {
        "id": "res-001",
        "module": "UserManagement",
        "service": "ms-users",
        "controller": "UserController",
        "action": "GetUsers",
        "method": "GET"
      }
    }
  ],
  "isActive": true
}
```

#### Get All RBACs (with Criteria)
```http
GET /api/rbac?page=1&limit=10&orderBy=name&sort=asc&filter=isActive eq true
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `200 OK` with paginated list
```json
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "User Management RBAC",
      "description": "Permissions for user management operations",
      "permissions": [],
      "isActive": true
    }
  ],
  "total": 1,
  "page": 1,
  "limit": 10
}
```

#### Update RBAC
```http
PUT /api/rbac/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "Updated RBAC",
  "description": "Updated description",
  "isActive": true
}
```

**Response**: `204 No Content`

#### Delete RBAC
```http
DELETE /api/rbac/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `204 No Content` (soft delete)

#### Add Permission to RBAC
```http
POST /api/rbac/{id}/permissions
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "idRbacPermission": "perm-004",
  "role": {
    "id": "role-editor",
    "name": "Editor"
  },
  "resource": {
    "id": "res-004",
    "module": "UserManagement",
    "service": "ms-users",
    "controller": "UserController",
    "action": "UpdateUser",
    "method": "PUT"
  }
}
```

**Response**: `204 No Content`

#### Remove Permission from RBAC
```http
DELETE /api/rbac/{id}/permissions/{idRbacPermission}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `204 No Content`

### gRPC API

#### GetRbac (Used by other microservices)
```protobuf
service Rbac {
  rpc GetRbac (GetRbacRequest) returns (GetRbacResponse);
}

message GetRbacRequest {
  string Microservice = 1;  // e.g., "ms-users"
}

message GetRbacResponse {
  repeated RbacResource Resources = 1;
}

message RbacResource {
  string Role = 1;          // e.g., "Admin"
  string Module = 2;        // e.g., "UserManagement"
  string Controller = 3;    // e.g., "UserController"
  string Action = 4;        // e.g., "GetUsers"
  HttpMethod Method = 5;    // GET, POST, PUT, DELETE
}
```

**Example gRPC Call (C# Client)**:
```csharp
var client = new Rbac.RbacClient(channel);
var request = new GetRbacRequest { Microservice = "ms-users" };
var response = await client.GetRbacAsync(request);

foreach (var resource in response.Resources)
{
    Console.WriteLine($"{resource.Role} can {resource.Method} {resource.Controller}/{resource.Action}");
}
```

**Example Output**:
```
Admin can GET UserController/GetUsers
Admin can POST UserController/CreateUser
Viewer can GET UserController/GetUsers
```

## 🔐 RBAC Model

### Core Entities

#### RbacAggregate

##### What is it and what is it for?

The RbacAggregate represents a named set of permission rules that map roles to specific API operations. It answers "which roles can call which endpoints" for the entire platform. Each permission inside it links a role (e.g., "Admin") to a resource (a specific controller action and HTTP method in a specific microservice), enabling centralized, dynamic authorization management without hardcoding access rules.

The main aggregate root representing a collection of permissions for a specific context.

```csharp
public class RbacAggregate
{
    public Guid Id { get; set; }
    public string Name { get; set; }               // e.g., "User Management RBAC"
    public string Description { get; set; }        // Description of this RBAC configuration
    public List<RbacPermissionEntity> Permissions { get; set; }
    public bool IsActive { get; set; }
}
```

#### RbacPermissionEntity
Individual permission associating a role with a resource.

```csharp
public class RbacPermissionEntity
{
    public Guid Id { get; set; }
    public Role Role { get; set; }           // Value Object: Role ID + Name
    public Resource Resource { get; set; }   // Value Object: Module, Service, Controller, Action, Method
}
```

#### Role (Value Object)
Represents a user role within the system.

```csharp
public class Role
{
    public Guid Id { get; set; }         // Unique identifier for the role
    public string Name { get; set; }     // e.g., "Admin", "Editor", "Viewer"
}
```

#### Resource (Value Object)
Defines a specific API endpoint that can be protected.

```csharp
public class Resource
{
    public Guid Id { get; set; }
    public string Module { get; set; }        // High-level module (e.g., "UserManagement")
    public string Service { get; set; }       // Microservice name (e.g., "ms-users")
    public string Controller { get; set; }    // Controller name (e.g., "UserController")
    public string Action { get; set; }        // Action name (e.g., "GetUsers")
    public HttpMethodEnum Method { get; set; } // GET, POST, PUT, DELETE, PATCH
}
```

### Permission Example

A complete permission mapping:

```json
{
  "role": {
    "id": "role-admin",
    "name": "Admin"
  },
  "resource": {
    "id": "res-001",
    "module": "OrderManagement",
    "service": "ms-orders",
    "controller": "OrderController",
    "action": "CancelOrder",
    "method": "DELETE"
  }
}
```

**Interpretation**: Users with the "Admin" role can call `DELETE /api/order/{id}` (OrderController.CancelOrder) in the ms-orders microservice.

### HTTP Method Enum

```csharp
public enum HttpMethodEnum
{
    None,    // Not specified
    GET,     // Read operations
    POST,    // Create operations
    PUT,     // Update operations
    DELETE,  // Delete operations
    PATCH    // Partial update operations
}
```

## ⚙️ Configuration

### appsettings.json (REST API)

```json
{
  "Core": {
    "Id": "a1e98ea5-66c9-4822-956d-75453dac4fbb",
    "PathBase": "/ms-rbac",
    "AppName": "ms-rbac",
    "TypeEntryPoint": "rest",
    "Version": "v1",
    "Description": "Microservice to manage RBAC",
    "Business": "CodeDesignPlus"
  },
  "Security": {
    "ValidateAudience": true,
    "ValidateIssuer": true,
    "ValidateLifetime": true,
    "ValidIssuer": "https://auth.example.com",
    "ValidAudiences": ["ms-rbac"],
    "ValidateRbac": false,         // Don't validate RBAC in RBAC service itself
    "RefreshRbacInterval": 10       // Refresh interval in minutes
  },
  "Redis": {
    "Instances": {
      "Core": {
        "ConnectionString": "localhost:6379"
      }
    }
  },
  "RedisCache": {
    "Enable": true,
    "Expiration": "00:05:00"         // Cache RBAC queries for 5 minutes
  },
  "Mongo": {
    "Enable": true,
    "Database": "db-ms-rbac"
  },
  "RabbitMQ": {
    "Enable": true,
    "Host": "localhost",
    "Port": 5672,
    "UserName": "user",
    "Password": "pass"
  },
  "Vault": {
    "Enable": true,
    "Address": "http://localhost:8200",
    "AppName": "ms-rbac",
    "Solution": "security-codedesignplus",
    "Token": "root"
  }
}
```

### appsettings.json (gRPC)

```json
{
  "Kestrel": {
    "Endpoints": {
      "Grpc": {
        "Url": "http://*:5001",
        "Protocols": "Http2"
      }
    }
  },
  "Core": {
    "AppName": "ms-rbac",
    "TypeEntryPoint": "grpc"
  }
}
```

### Environment Variables

For containerized deployments:

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000         # REST
GRPC_PORT=5001                         # gRPC
MONGO_CONNECTION_STRING=mongodb://mongo:27017
REDIS_CONNECTION_STRING=redis:6379
RABBITMQ_HOST=rabbitmq
VAULT_ADDRESS=http://vault:8200
VAULT_TOKEN=your-vault-token
RBAC_CACHE_EXPIRATION=00:05:00
```

## 🎯 Use Cases & Scenarios

### 1. Microservice Self-Authorization

Each microservice queries RBAC during startup to load its permission mappings.

```bash
# ms-users service queries RBAC on startup
gRPC Call: GetRbac("ms-users")

Response:
- Admin can GET UserController/GetUsers
- Admin can POST UserController/CreateUser
- Admin can DELETE UserController/DeleteUser
- Editor can GET UserController/GetUsers
- Editor can PUT UserController/UpdateUser
- Viewer can GET UserController/GetUsers

# ms-users caches this in memory and refreshes every 10 minutes
```

**Authorization Middleware Flow**:
```csharp
// In ms-users authorization middleware
public async Task<bool> IsAuthorized(HttpContext context, string userRole)
{
    var controller = context.GetRouteValue("controller");
    var action = context.GetRouteValue("action");
    var method = context.Request.Method;
    
    // Check cached RBAC permissions
    return rbacCache.HasPermission(userRole, controller, action, method);
}
```

### 2. Dynamic Permission Management

Admin updates permissions without restarting microservices.

```bash
# Step 1: Admin adds new permission for "Manager" role
POST /api/rbac/{id}/permissions
{
  "role": { "name": "Manager" },
  "resource": {
    "service": "ms-orders",
    "controller": "OrderController",
    "action": "ApproveOrder",
    "method": "POST"
  }
}

# Step 2: RBAC publishes PermissionAddedDomainEvent to RabbitMQ

# Step 3: ms-orders listens to event and refreshes its RBAC cache

# Step 4: Managers can now approve orders without code changes
```

### 3. Multi-Tenant Isolation

Different tenants have separate RBAC configurations.

```bash
# Tenant A: Strict permissions
POST /api/rbac
Headers: X-Tenant: tenant-a
Body: Only Admins can delete users

# Tenant B: Relaxed permissions
POST /api/rbac
Headers: X-Tenant: tenant-b
Body: Admins and Managers can delete users

# RBAC queries are isolated by tenant
gRPC Call: GetRbac("ms-users")
Headers: X-Tenant: tenant-a
→ Returns Tenant A's permissions only
```

### 4. Centralized Permission Audit

Track who can do what across all microservices.

```bash
# Audit: What can the "Admin" role do?
GET /api/rbac?filter=permissions/role/name eq 'Admin'

Response:
- ms-users: GET/POST/PUT/DELETE UserController/*
- ms-orders: GET/POST/PUT/DELETE OrderController/*
- ms-payments: GET PaymentController/*

# Audit: What endpoints are protected in ms-orders?
GET /api/rbac?filter=permissions/resource/service eq 'ms-orders'

Response:
- Admin: Full access
- Manager: Read + Approve
- Viewer: Read only
```

### 5. Role Hierarchy Simulation

Implement role inheritance using multiple permissions.

```bash
# Define permissions for hierarchical roles
POST /api/rbac
{
  "name": "Role Hierarchy",
  "rbacPermissions": [
    // Viewer: Read-only access
    { "role": "Viewer", "resource": { "action": "GetUsers", "method": "GET" } },
    
    // Editor: Viewer + Write access
    { "role": "Editor", "resource": { "action": "GetUsers", "method": "GET" } },
    { "role": "Editor", "resource": { "action": "UpdateUser", "method": "PUT" } },
    
    // Admin: Editor + Delete access
    { "role": "Admin", "resource": { "action": "GetUsers", "method": "GET" } },
    { "role": "Admin", "resource": { "action": "UpdateUser", "method": "PUT" } },
    { "role": "Admin", "resource": { "action": "DeleteUser", "method": "DELETE" } }
  ]
}
```

## 🏗️ Architecture

### Clean Architecture Layers

```
src/
├── domain/                          # Domain Layer
│   ├── Domain/                      # Aggregates, Entities, Value Objects
│   │   ├── RbacAggregate.cs        # Main aggregate root
│   │   ├── Entities/               # RbacPermissionEntity
│   │   ├── ValueObjects/           # Role, Resource
│   │   ├── Enums/                  # HttpMethodEnum
│   │   ├── DomainEvents/           # RbacCreated, PermissionAdded, etc.
│   │   ├── Repositories/           # IRbacRepository
│   │   └── Errors.cs               # Domain error codes
│   ├── Application/                 # Application Layer
│   │   ├── Commands/               # CreateRbac, UpdateRbac, AddPermission, RemovePermission
│   │   ├── Queries/                # GetById, GetAll, GetRbacByMicroservice
│   │   ├── DTOs/                   # RbacDto, RbacPermissionDto, RbacResourceDto
│   │   └── Validators/             # FluentValidation rules
│   └── Infrastructure/              # Infrastructure Layer
│       └── Repositories/           # RbacRepository (MongoDB)
└── entrypoints/                     # Presentation Layer
    ├── Rest/                        # REST API
    │   ├── Controllers/            # RbacController
    │   └── Program.cs              # Startup configuration
    └── gRpc/                        # gRPC API
        ├── Services/               # RbacService
        ├── Protos/                 # rbac.proto
        └── Program.cs              # Startup configuration
```

### CQRS Pattern

**Commands** (Write operations):
- `CreateRbacCommand` - Create new RBAC configuration
- `UpdateRbacCommand` - Update RBAC name/description/status
- `DeleteRbacCommand` - Soft delete RBAC
- `AddPermissionCommand` - Add permission to RBAC
- `RemovePermissionCommand` - Remove permission from RBAC

**Queries** (Read operations):
- `GetRbacByIdQuery` - Get RBAC by ID
- `GetAllRbacQuery` - List with pagination and criteria filtering
- `GetRbacByMicroserviceQuery` - Get permissions for specific microservice (used by gRPC)

### Domain Events

Published to RabbitMQ after successful operations:
- `RbacCreatedDomainEvent` - New RBAC configuration created
- `RbacUpdatedDomainEvent` - RBAC configuration updated
- `RbacDeletedDomainEvent` - RBAC configuration deleted
- `PermissionAddedDomainEvent` - New permission added
- `PermissionUpdatedDomainEvent` - Permission modified
- `PermissionRemovedDomainEvent` - Permission removed

### Authorization Flow (Microservice Integration)

```
[ms-users] Startup
     ↓
gRPC Call: GetRbac("ms-users")
     ↓
[RBAC gRPC Service] → Handles request
     ↓
[GetRbacByMicroserviceQuery] → Queries cache
     ↓
Cache Miss? → [RbacRepository] → MongoDB query
     ↓
Filter permissions by service = "ms-users"
     ↓
[CacheManager] → Stores in Redis (5 min TTL)
     ↓
Returns List<RbacResourceDto>
     ↓
[ms-users] Builds permission map:
  {
    "Admin": ["GET:UserController/GetUsers", "POST:UserController/CreateUser", ...],
    "Viewer": ["GET:UserController/GetUsers"]
  }
     ↓
[ms-users] Stores in-memory cache
     ↓
[Authorization Middleware] Uses cache for every request:
  - Extract user role from JWT
  - Extract controller/action/method from route
  - Check if role has permission → Allow/Deny
```

### Repository Pattern

**Custom Repository Methods**:
```csharp
public interface IRbacRepository : IRepositoryBase
{
    // Check if any active RBAC exists (for validation)
    Task<bool> HasActiveRbacAsync(CancellationToken cancellationToken);
    
    // Check if any other active RBAC exists (for uniqueness)
    Task<bool> HasActiveRbacAsync(Guid id, CancellationToken cancellationToken);
    
    // Get permissions for specific microservice (gRPC query)
    Task<List<RbacPermissionEntity>> GetPermissionsByMicroserviceAsync(
        string microservice, 
        CancellationToken cancellationToken
    );
}
```

**MongoDB Query Example** (GetPermissionsByMicroserviceAsync):
```csharp
var filter = Builders<RbacAggregate>.Filter.And(
    Builders<RbacAggregate>.Filter.Eq(x => x.IsActive, true),
    Builders<RbacAggregate>.Filter.ElemMatch(
        x => x.Permissions, 
        p => p.Resource.Service == microservice
    )
);

var projection = Builders<RbacAggregate>.Projection.Expression(rbac => 
    rbac.Permissions.Where(p => p.Resource.Service == microservice).ToList()
);

var permissions = await collection
    .Find(filter)
    .Project(projection)
    .FirstOrDefaultAsync(cancellationToken);
```

## 🧪 Testing

### Run All Tests
```bash
dotnet test
```

### Run Unit Tests Only
```bash
dotnet test --filter Category=Unit
```

### Run Integration Tests Only
```bash
dotnet test --filter Category=Integration
```

### Test Structure
```
tests/
├── unit/
│   ├── Domain.Test/              # 83 test files total
│   │   ├── RbacAggregateTest.cs
│   │   ├── RoleTest.cs
│   │   ├── ResourceTest.cs
│   │   └── DomainEvents/
│   ├── Application.Test/
│   │   ├── Commands/
│   │   │   ├── CreateRbacCommandHandlerTest.cs
│   │   │   └── AddPermissionCommandHandlerTest.cs
│   │   └── Queries/
│   │       └── GetRbacByMicroserviceQueryHandlerTest.cs
│   ├── Infrastructure.Test/
│   │   └── Repositories/
│   │       └── RbacRepositoryTest.cs
│   └── Rest.Test/
│       └── Controllers/
│           └── RbacControllerTest.cs
└── integration/
    ├── Rest.Test/
    │   └── RbacControllerIntegrationTest.cs
    └── gRpc.Test/
        └── RbacServiceIntegrationTest.cs
```

### Example Test: Create RBAC
```csharp
[Fact]
public async Task CreateRbac_ValidCommand_Success()
{
    // Arrange
    var command = new CreateRbacCommand(
        Id: Guid.NewGuid(),
        Name: "Test RBAC",
        Description: "Test description",
        RbacPermissions: new List<RbacPermissionDto>
        {
            new RbacPermissionDto
            {
                Id = Guid.NewGuid(),
                Role = new Role(Guid.NewGuid(), "Admin"),
                Resource = new Resource(
                    Guid.NewGuid(),
                    "UserManagement",
                    "ms-users",
                    "UserController",
                    "GetUsers",
                    HttpMethodEnum.GET
                )
            }
        }
    );

    // Act
    await mediator.Send(command);

    // Assert
    var rbac = await repository.GetByIdAsync(command.Id);
    Assert.NotNull(rbac);
    Assert.Equal("Test RBAC", rbac.Name);
    Assert.Single(rbac.Permissions);
}
```

### Example Test: gRPC GetRbac
```csharp
[Fact]
public async Task GetRbac_ValidMicroservice_ReturnsPermissions()
{
    // Arrange
    var request = new GetRbacRequest { Microservice = "ms-users" };

    // Act
    var response = await client.GetRbacAsync(request);

    // Assert
    Assert.NotEmpty(response.Resources);
    Assert.Contains(response.Resources, r => 
        r.Role == "Admin" && 
        r.Controller == "UserController" && 
        r.Action == "GetUsers" && 
        r.Method == HttpMethod.Get
    );
}
```

### Load Testing

Test RBAC query performance under load:

```bash
# Install k6
brew install k6  # macOS
choco install k6  # Windows

# Run load test
cd tools/load-testing
k6 run rbac-load-test.js
```

**Sample k6 Script** (rbac-load-test.js):
```javascript
import grpc from 'k6/net/grpc';
import { check } from 'k6';

const client = new grpc.Client();
client.load(['../../src/entrypoints/CodeDesignPlus.Net.Microservice.Rbac.gRpc/Protos'], 'rbac.proto');

export const options = {
  vus: 100,          // 100 virtual users
  duration: '30s',   // 30 seconds
};

export default function () {
  client.connect('localhost:5001', { plaintext: true });

  const request = { Microservice: 'ms-users' };
  const response = client.invoke('Rbac.Rbac/GetRbac', request);

  check(response, {
    'status is OK': (r) => r && r.status === grpc.StatusOK,
    'response has resources': (r) => r && r.message.resources.length > 0,
  });

  client.close();
}
```

**Expected Results**:
```
checks.........................: 100.00% ✓ 30000      ✗ 0
grpc_req_duration..............: avg=2.5ms   p(95)=5ms   p(99)=10ms
iterations.....................: 15000   500/s
```

## 📚 Best Practices

### 1. Permission Granularity

**Do**: Define granular permissions per action
```json
{
  "permissions": [
    { "role": "Editor", "action": "UpdateUser", "method": "PUT" },
    { "role": "Editor", "action": "UpdateUserProfile", "method": "PATCH" }
  ]
}
```

**Don't**: Use wildcard or overly broad permissions
```json
{
  "permissions": [
    { "role": "Editor", "action": "*", "method": "*" }  // Too broad!
  ]
}
```

### 2. Role Naming Conventions

Use clear, descriptive role names:
- ✅ `Admin`, `ContentEditor`, `ReportViewer`, `BillingManager`
- ❌ `Role1`, `User`, `SuperUser`

### 3. Module Organization

Group related permissions under logical modules:
```
Module: UserManagement
  └── Service: ms-users
      ├── UserController.GetUsers (GET)
      ├── UserController.CreateUser (POST)
      └── UserController.DeleteUser (DELETE)

Module: OrderManagement
  └── Service: ms-orders
      ├── OrderController.GetOrders (GET)
      └── OrderController.CancelOrder (DELETE)
```

### 4. Cache Strategy

**Microservice Cache Refresh**:
```csharp
// In microservice startup
public async Task RefreshRbacCache()
{
    var interval = TimeSpan.FromMinutes(10);
    var timer = new PeriodicTimer(interval);

    while (await timer.WaitForNextTickAsync())
    {
        try
        {
            var permissions = await rbacClient.GetRbacAsync("ms-users");
            rbacCache.Refresh(permissions);
            logger.LogInformation("RBAC cache refreshed at {Time}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to refresh RBAC cache");
        }
    }
}
```

### 5. Event-Driven Updates

**Subscribe to RBAC events** for real-time updates:
```csharp
// In microservice RabbitMQ consumer
public async Task HandlePermissionAddedEvent(PermissionAddedDomainEvent @event)
{
    // Check if event affects this microservice
    if (@event.Resource.Service == "ms-users")
    {
        // Refresh RBAC cache immediately
        await RefreshRbacCache();
        logger.LogInformation("RBAC cache updated due to permission change");
    }
}
```

### 6. Error Handling

**Domain Validation**:
```csharp
// In RbacAggregate
public void AddPermission(Guid id, Role role, Resource resource, Guid updatedBy)
{
    DomainGuard.GuidIsEmpty(id, Errors.PermissionIdIsInvalid);
    DomainGuard.IsNull(role, Errors.RoleIsInvalid);
    DomainGuard.IsNull(resource, Errors.ResourceIsInvalid);
    DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdatedByIsInvalid);

    // Check for duplicate permission
    var exists = Permissions.Any(p => 
        p.Role.Name == role.Name && 
        p.Resource.Action == resource.Action &&
        p.Resource.Method == resource.Method
    );
    
    if (exists)
        throw new DomainException(Errors.PermissionAlreadyExists);

    // Add permission
    Permissions.Add(new RbacPermissionEntity { Id = id, Role = role, Resource = resource });
}
```

### 7. Multi-Tenancy

**Always filter by tenant** in queries:
```csharp
// In RbacRepository
public async Task<List<RbacPermissionEntity>> GetPermissionsByMicroserviceAsync(
    string microservice, 
    Guid tenantId,
    CancellationToken cancellationToken)
{
    var filter = Builders<RbacAggregate>.Filter.And(
        Builders<RbacAggregate>.Filter.Eq(x => x.IsActive, true),
        Builders<RbacAggregate>.Filter.Eq(x => x.TenantId, tenantId),  // Tenant isolation
        Builders<RbacAggregate>.Filter.ElemMatch(
            x => x.Permissions, 
            p => p.Resource.Service == microservice
        )
    );
    // ... rest of query
}
```

### 8. Monitoring & Observability

**Track RBAC query performance**:
```csharp
using var activity = activitySource.StartActivity("GetRbacByMicroservice");
activity?.SetTag("microservice", request.Microservice);
activity?.SetTag("tenant", tenantId);

var stopwatch = Stopwatch.StartNew();
var permissions = await repository.GetPermissionsByMicroserviceAsync(microservice, cancellationToken);
stopwatch.Stop();

activity?.SetTag("permission.count", permissions.Count);
activity?.SetTag("duration.ms", stopwatch.ElapsedMilliseconds);
```

## 🔧 Troubleshooting

### Issue: Permissions Not Loading in Microservice

**Symptoms**:
- Microservice returns 403 Forbidden for all requests
- Logs show "No RBAC permissions found for ms-users"

**Diagnosis**:
```bash
# 1. Check if RBAC service is running
curl http://localhost:5000/health

# 2. Query RBAC for the microservice
grpcurl -plaintext -d '{"Microservice": "ms-users"}' \
  localhost:5001 Rbac.Rbac/GetRbac

# 3. Check Redis cache
redis-cli
> GET ms-users
```

**Solution**:
```bash
# If no permissions exist, create them
POST /api/rbac
{
  "name": "Users RBAC",
  "rbacPermissions": [
    {
      "role": { "name": "Admin" },
      "resource": {
        "service": "ms-users",
        "controller": "UserController",
        "action": "GetUsers",
        "method": "GET"
      }
    }
  ]
}

# Restart or refresh cache in ms-users
```

### Issue: Stale Cache After Permission Update

**Symptoms**:
- Permission added in RBAC, but microservice still denies access
- Cache not refreshing automatically

**Diagnosis**:
```bash
# Check when cache was last updated
redis-cli
> TTL ms-users
> GET ms-users
```

**Solution**:
```bash
# Option 1: Clear cache manually
redis-cli
> DEL ms-users

# Option 2: Restart microservice to force refresh

# Option 3: Check if microservice subscribes to RBAC events
# Look for RabbitMQ consumer logs
```

### Issue: gRPC Connection Refused

**Symptoms**:
- Microservice logs: "Failed to connect to RBAC gRPC at localhost:5001"
- Error: `Grpc.Core.RpcException: Status(StatusCode="Unavailable")`

**Diagnosis**:
```bash
# 1. Check if gRPC service is running
netstat -an | grep 5001

# 2. Test gRPC endpoint
grpcurl -plaintext localhost:5001 list
# Should show: Rbac.Rbac
```

**Solution**:
```bash
# Start gRPC entrypoint
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Rbac.gRpc

# Verify with grpcurl
grpcurl -plaintext localhost:5001 list Rbac.Rbac
```

### Issue: Duplicate Permissions

**Symptoms**:
- Multiple permissions with same role/resource combination
- Inconsistent authorization behavior

**Diagnosis**:
```bash
# Query RBAC and look for duplicates
GET /api/rbac/{id}

# Check for duplicate permissions
{
  "permissions": [
    { "role": "Admin", "action": "GetUsers", "method": "GET" },
    { "role": "Admin", "action": "GetUsers", "method": "GET" }  // Duplicate!
  ]
}
```

**Solution**:
```bash
# Remove duplicate permission
DELETE /api/rbac/{id}/permissions/{duplicate-permission-id}

# Implement uniqueness validation in domain
```

### Issue: 403 Forbidden Despite Correct Permissions

**Symptoms**:
- User has correct role in JWT
- Permission exists in RBAC
- Still getting 403 Forbidden

**Diagnosis**:
```bash
# 1. Decode JWT and check role claim
# Use jwt.io or:
echo "your-jwt-token" | base64 -d

# 2. Check claim name mapping
# JWT might use "role", "roles", or custom claim
{
  "sub": "user-123",
  "role": "Admin",           # ← Check claim name
  "email": "admin@example.com"
}

# 3. Verify RBAC permission format
GET /api/rbac?filter=permissions/role/name eq 'Admin'
```

**Solution**:
```csharp
// In microservice authorization middleware
public string ExtractRole(ClaimsPrincipal principal)
{
    // Try multiple claim types
    var role = principal.FindFirst("role")?.Value
            ?? principal.FindFirst("roles")?.Value
            ?? principal.FindFirst(ClaimTypes.Role)?.Value;
    
    if (string.IsNullOrEmpty(role))
        throw new UnauthorizedException("No role claim found in token");
    
    return role;
}
```

### Issue: High Latency on Authorization Checks

**Symptoms**:
- Requests taking 500ms+ due to authorization
- High database/cache load

**Diagnosis**:
```bash
# Check cache hit rate
redis-cli
> INFO stats
# Look for: keyspace_hits, keyspace_misses

# Check MongoDB slow queries
# In MongoDB shell:
db.setProfilingLevel(2)
db.system.profile.find().sort({ millis: -1 }).limit(5)
```

**Solution**:
```bash
# 1. Increase cache TTL (if permissions rarely change)
"RedisCache": {
  "Expiration": "01:00:00"  # 1 hour instead of 5 minutes
}

# 2. Add MongoDB index on service field
db.rbac.createIndex({ "Permissions.Resource.Service": 1, "IsActive": 1 })

# 3. Implement in-memory cache in microservice
# Cache RBAC permissions locally and only query RBAC on cache miss
```

## 🔐 Authorization Flow

### 1. Initial Setup (Microservice Startup)

```
[Microservice Startup]
     ↓
Create gRPC channel to RBAC service
     ↓
Call GetRbac("ms-users")
     ↓
Receive List<RbacResource>
     ↓
Build in-memory permission map:
  Dictionary<string, List<Permission>> permissionsByRole
     ↓
Register authorization middleware
     ↓
Start periodic refresh timer (every 10 minutes)
```

### 2. Request Authorization (Per HTTP Request)

```
[HTTP Request] → GET /api/user/123
     ↓
[Authorization Middleware]
     ↓
Extract JWT from Authorization header
     ↓
Validate JWT signature and expiration
     ↓
Extract role claim from JWT (e.g., "Admin")
     ↓
Extract route: controller="UserController", action="GetUserById", method="GET"
     ↓
Lookup in permission cache:
  permissionsByRole["Admin"].Contains("UserController.GetUserById.GET")
     ↓
Permission found? → Continue to controller
Permission not found? → Return 403 Forbidden
```

### 3. Cache Refresh Flow

```
[Periodic Timer] → Every 10 minutes
     ↓
Call GetRbac("ms-users") via gRPC
     ↓
[RBAC Service]
     ↓
Check Redis cache for key "ms-users"
     ↓
Cache hit? → Return cached data
Cache miss? → Query MongoDB
     ↓
[MongoDB] Filter by: IsActive=true AND Service="ms-users"
     ↓
Project only relevant permissions
     ↓
Store in Redis with 5-minute TTL
     ↓
Return to microservice
     ↓
[Microservice] Updates in-memory permission map
     ↓
Log: "RBAC cache refreshed: 15 permissions loaded"
```

### 4. Event-Driven Refresh

```
[Admin] → Adds new permission via REST API
     ↓
POST /api/rbac/{id}/permissions
     ↓
[RBAC Service] → AddPermissionCommand
     ↓
[RbacAggregate] → Validates and adds permission
     ↓
Publishes PermissionAddedDomainEvent to RabbitMQ
     ↓
[RabbitMQ] → Broadcasts event to all microservices
     ↓
[Microservice Consumer] → Receives event
     ↓
Checks if event.Resource.Service == "ms-users"
     ↓
If yes: Immediately refresh RBAC cache (bypass timer)
     ↓
Log: "RBAC cache updated due to permission change"
```

## 🔗 Integration

### Integrating RBAC into a Microservice

#### Step 1: Add gRPC Client Dependency

```csharp
// In your microservice's .csproj
<ItemGroup>
  <PackageReference Include="Grpc.Net.Client" Version="2.60.0" />
  <PackageReference Include="Google.Protobuf" Version="3.25.0" />
  <PackageReference Include="Grpc.Tools" Version="2.60.0" PrivateAssets="All" />
</ItemGroup>

<ItemGroup>
  <Protobuf Include="Protos\rbac.proto" GrpcServices="Client" />
</ItemGroup>
```

#### Step 2: Copy rbac.proto

```bash
# Copy proto file from RBAC service
cp ../CodeDesignPlus.Net.Microservice.Rbac/src/entrypoints/CodeDesignPlus.Net.Microservice.Rbac.gRpc/Protos/rbac.proto \
   ./Protos/rbac.proto
```

#### Step 3: Create RBAC Client Service

```csharp
public interface IRbacClientService
{
    Task<List<RbacPermission>> LoadPermissionsAsync(string microservice);
}

public class RbacClientService : IRbacClientService
{
    private readonly Rbac.RbacClient _client;
    private readonly ILogger<RbacClientService> _logger;

    public RbacClientService(Rbac.RbacClient client, ILogger<RbacClientService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<List<RbacPermission>> LoadPermissionsAsync(string microservice)
    {
        var request = new GetRbacRequest { Microservice = microservice };
        var response = await _client.GetRbacAsync(request);

        var permissions = response.Resources.Select(r => new RbacPermission
        {
            Role = r.Role,
            Module = r.Module,
            Controller = r.Controller,
            Action = r.Action,
            Method = r.Method.ToString()
        }).ToList();

        _logger.LogInformation("Loaded {Count} permissions from RBAC for {Microservice}", 
            permissions.Count, microservice);

        return permissions;
    }
}
```

#### Step 4: Register gRPC Client in Startup

```csharp
// In Program.cs
builder.Services.AddGrpcClient<Rbac.RbacClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcClients:Rbac"] 
        ?? "http://ms-rbac-grpc:5001");
});

builder.Services.AddSingleton<IRbacClientService, RbacClientService>();
```

#### Step 5: Create Authorization Service

```csharp
public class RbacAuthorizationService
{
    private Dictionary<string, HashSet<string>> _permissionsByRole = new();
    private readonly IRbacClientService _rbacClient;
    private readonly ILogger<RbacAuthorizationService> _logger;

    public RbacAuthorizationService(
        IRbacClientService rbacClient, 
        ILogger<RbacAuthorizationService> logger)
    {
        _rbacClient = rbacClient;
        _logger = logger;
    }

    public async Task InitializeAsync(string microservice)
    {
        var permissions = await _rbacClient.LoadPermissionsAsync(microservice);
        
        _permissionsByRole = permissions
            .GroupBy(p => p.Role)
            .ToDictionary(
                g => g.Key,
                g => g.Select(p => $"{p.Controller}.{p.Action}.{p.Method}")
                      .ToHashSet()
            );

        _logger.LogInformation("RBAC initialized with {RoleCount} roles", 
            _permissionsByRole.Count);
    }

    public bool IsAuthorized(string role, string controller, string action, string method)
    {
        if (!_permissionsByRole.TryGetValue(role, out var permissions))
            return false;

        var key = $"{controller}.{action}.{method}";
        return permissions.Contains(key);
    }
}
```

#### Step 6: Create Authorization Middleware

```csharp
public class RbacAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RbacAuthorizationService _authService;

    public RbacAuthorizationMiddleware(
        RequestDelegate next, 
        RbacAuthorizationService authService)
    {
        _next = next;
        _authService = authService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authorization for public endpoints
        if (context.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await _next(context);
            return;
        }

        // Extract user role from JWT
        var role = context.User.FindFirst("role")?.Value;
        if (string.IsNullOrEmpty(role))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("No role claim found");
            return;
        }

        // Extract route information
        var controller = context.GetRouteValue("controller")?.ToString();
        var action = context.GetRouteValue("action")?.ToString();
        var method = context.Request.Method;

        // Check authorization
        if (!_authService.IsAuthorized(role, controller, action, method))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync($"Role '{role}' is not authorized to {method} {controller}/{action}");
            return;
        }

        await _next(context);
    }
}
```

#### Step 7: Register Middleware in Startup

```csharp
// In Program.cs
var app = builder.Build();

// Initialize RBAC on startup
var rbacService = app.Services.GetRequiredService<RbacAuthorizationService>();
await rbacService.InitializeAsync("ms-users");

app.UseAuthentication();
app.UseMiddleware<RbacAuthorizationMiddleware>();  // ← Add RBAC middleware
app.UseAuthorization();

app.MapControllers();
app.Run();
```

#### Step 8: Add Periodic Refresh

```csharp
// In Program.cs (after app.Build())
var rbacService = app.Services.GetRequiredService<RbacAuthorizationService>();
await rbacService.InitializeAsync("ms-users");

// Start background refresh timer
_ = Task.Run(async () =>
{
    var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));
    while (await timer.WaitForNextTickAsync())
    {
        try
        {
            await rbacService.InitializeAsync("ms-users");
            logger.LogInformation("RBAC cache refreshed at {Time}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to refresh RBAC cache");
        }
    }
});
```

### Example: Using CodeDesignPlus.Net.Security Package

If using the CodeDesignPlus SDK Security package, RBAC integration is built-in:

```csharp
// In appsettings.json
{
  "Security": {
    "ValidateRbac": true,                    // Enable RBAC validation
    "ServerRbac": "http://ms-rbac-grpc:5001", // RBAC gRPC endpoint
    "RefreshRbacInterval": 10                 // Refresh every 10 minutes
  }
}

// In Program.cs
builder.Services.AddSecurity(builder.Configuration);

var app = builder.Build();

app.UseSecurity();  // Automatically adds RBAC middleware

app.MapControllers();
app.Run();
```

## 🔒 Security

### Authentication

RBAC service requires JWT authentication for all REST endpoints:

```http
Authorization: Bearer eyJhbGciOiJSUzI1NiIs...
```

**Required JWT Claims**:
- `sub`: User ID
- `role` or `roles`: User role(s)
- `tenant`: Tenant ID (for multi-tenancy)

### Authorization

The RBAC service itself does **not** validate RBAC (to avoid circular dependency). However, it validates:
- JWT signature and expiration
- Tenant isolation (users can only manage their tenant's RBAC)

### Tenant Isolation

All RBAC configurations are isolated by tenant:

```csharp
// Automatic tenant filtering in repository
var filter = Builders<RbacAggregate>.Filter.And(
    Builders<RbacAggregate>.Filter.Eq(x => x.TenantId, tenantId),
    // ... other filters
);
```

### Secrets Management

Sensitive configuration stored in Vault:
- MongoDB connection string
- RabbitMQ credentials
- Redis password
- JWT signing keys

```bash
# Configure Vault secrets
cd tools/vault
./config-vault.sh

# Vault path: secret/security-codedesignplus/ms-rbac
# Keys:
#   - mongo-username
#   - mongo-password
#   - rabbitmq-username
#   - rabbitmq-password
```

### Network Security

**Production Deployment**:
- gRPC: Internal only (not exposed to internet)
- REST API: Public (protected by API Gateway + JWT)
- MongoDB: Internal network only
- Redis: Internal network only
- RabbitMQ: Internal network only

**Kubernetes Network Policies**:
```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: rbac-grpc-policy
spec:
  podSelector:
    matchLabels:
      app: ms-rbac-grpc
  policyTypes:
    - Ingress
  ingress:
    - from:
        - podSelector:
            matchLabels:
              rbac-client: "true"  # Only microservices with this label
      ports:
        - protocol: TCP
          port: 5001
```

## 🐳 Docker Support

### Build Docker Images

#### REST API
```bash
docker build -t ms-rbac-rest:latest . \
  -f src/entrypoints/CodeDesignPlus.Net.Microservice.Rbac.Rest/Dockerfile

docker run -d -p 5000:5000 \
  --network=backend \
  -e ASPNETCORE_ENVIRONMENT=Docker \
  -e MONGO_CONNECTION_STRING=mongodb://mongo:27017 \
  -e REDIS_CONNECTION_STRING=redis:6379 \
  --name ms-rbac-rest \
  ms-rbac-rest:latest
```

#### gRPC API
```bash
docker build -t ms-rbac-grpc:latest . \
  -f src/entrypoints/CodeDesignPlus.Net.Microservice.Rbac.gRpc/Dockerfile

docker run -d -p 5001:5001 \
  --network=backend \
  -e ASPNETCORE_ENVIRONMENT=Docker \
  -e MONGO_CONNECTION_STRING=mongodb://mongo:27017 \
  -e REDIS_CONNECTION_STRING=redis:6379 \
  --name ms-rbac-grpc \
  ms-rbac-grpc:latest
```

### Docker Compose

```yaml
version: '3.8'

services:
  ms-rbac-rest:
    image: ms-rbac-rest:latest
    ports:
      - "5000:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=Docker
      - MONGO_CONNECTION_STRING=mongodb://mongo:27017
      - REDIS_CONNECTION_STRING=redis:6379
      - RABBITMQ_HOST=rabbitmq
      - VAULT_ADDRESS=http://vault:8200
    depends_on:
      - mongo
      - redis
      - rabbitmq
    networks:
      - backend

  ms-rbac-grpc:
    image: ms-rbac-grpc:latest
    ports:
      - "5001:5001"
    environment:
      - ASPNETCORE_ENVIRONMENT=Docker
      - MONGO_CONNECTION_STRING=mongodb://mongo:27017
      - REDIS_CONNECTION_STRING=redis:6379
    depends_on:
      - mongo
      - redis
    networks:
      - backend

networks:
  backend:
    driver: bridge
```

### Kubernetes Deployment

**Helm Chart** (charts/ms-rbac-grpc/values.yaml):
```yaml
replicaCount: 3

image:
  repository: codedesignplus/ms-rbac-grpc
  tag: "0.0.1"
  pullPolicy: IfNotPresent

service:
  type: ClusterIP
  port: 5001

resources:
  requests:
    cpu: 100m
    memory: 128Mi
  limits:
    cpu: 500m
    memory: 512Mi

autoscaling:
  enabled: true
  minReplicas: 3
  maxReplicas: 10
  targetCPUUtilizationPercentage: 70

env:
  - name: ASPNETCORE_ENVIRONMENT
    value: "Production"
  - name: MONGO_CONNECTION_STRING
    valueFrom:
      secretKeyRef:
        name: mongo-secret
        key: connection-string
  - name: REDIS_CONNECTION_STRING
    valueFrom:
      secretKeyRef:
        name: redis-secret
        key: connection-string
```

**Deploy to Kubernetes**:
```bash
helm upgrade --install ms-rbac-grpc ./charts/ms-rbac-grpc \
  --namespace codedesignplus \
  --values ./charts/ms-rbac-grpc/Production.yaml
```

## ❓ FAQ

### Q1: How does RBAC differ from traditional authorization?

**Traditional Authorization** (e.g., [Authorize(Roles = "Admin")]):
- Permissions hard-coded in application code
- Requires code changes and redeployment to update permissions
- Difficult to audit across multiple microservices

**RBAC Microservice**:
- Permissions stored in database and queried at runtime
- Add/remove permissions dynamically via API
- Centralized permission management across all microservices
- Easy to audit: Query "What can role X do?" or "Who can access endpoint Y?"

### Q2: What happens if RBAC service is down?

**Mitigation Strategies**:

1. **Local Cache**: Microservices cache permissions in-memory for 10+ minutes
2. **Redis Cache**: RBAC service caches queries in Redis
3. **Graceful Degradation**: Microservice can continue using stale cache
4. **High Availability**: Deploy multiple RBAC gRPC instances

**Example Fallback Logic**:
```csharp
public async Task<List<RbacPermission>> LoadPermissionsAsync(string microservice)
{
    try
    {
        return await _rbacClient.GetRbacAsync(microservice);
    }
    catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
    {
        _logger.LogWarning("RBAC service unavailable, using cached permissions");
        return _cache.GetCachedPermissions(microservice);  // Use stale cache
    }
}
```

### Q3: Can I implement role hierarchy (e.g., Admin inherits Editor permissions)?

**Yes**, but it requires application logic. RBAC stores flat role-to-resource mappings. To implement hierarchy:

**Option 1: Store all inherited permissions explicitly**
```json
{
  "permissions": [
    // Viewer permissions
    { "role": "Viewer", "action": "GetUsers", "method": "GET" },
    
    // Editor inherits Viewer + adds write permissions
    { "role": "Editor", "action": "GetUsers", "method": "GET" },
    { "role": "Editor", "action": "UpdateUser", "method": "PUT" },
    
    // Admin inherits Editor + adds delete permissions
    { "role": "Admin", "action": "GetUsers", "method": "GET" },
    { "role": "Admin", "action": "UpdateUser", "method": "PUT" },
    { "role": "Admin", "action": "DeleteUser", "method": "DELETE" }
  ]
}
```

**Option 2: Implement hierarchy in microservice**
```csharp
public class RoleHierarchyService
{
    private readonly Dictionary<string, List<string>> _hierarchy = new()
    {
        ["Viewer"] = new List<string>(),
        ["Editor"] = new List<string> { "Viewer" },
        ["Admin"] = new List<string> { "Editor", "Viewer" }
    };

    public bool IsAuthorized(string userRole, string requiredRole)
    {
        if (userRole == requiredRole)
            return true;

        // Check if userRole inherits requiredRole
        return _hierarchy[userRole].Contains(requiredRole);
    }
}
```

### Q4: How do I handle API versioning with RBAC?

**Best Practice**: Include version in the Resource definition:

```json
{
  "resource": {
    "module": "UserManagement",
    "service": "ms-users",
    "controller": "UserController",
    "action": "GetUsers_V2",        // ← Version in action name
    "method": "GET"
  }
}
```

Or use separate controllers:
```json
{
  "resource": {
    "controller": "UserControllerV1",  // ← Version in controller name
    "action": "GetUsers",
    "method": "GET"
  }
}
```

### Q5: Can I use RBAC for field-level permissions?

**No**, RBAC operates at the **endpoint level** (Controller.Action + HTTP Method). For field-level permissions, implement logic in the application layer:

```csharp
public async Task<UserDto> GetUserById(Guid id)
{
    var user = await _repository.GetByIdAsync(id);
    var dto = _mapper.Map<UserDto>(user);

    // Field-level permission logic
    var userRole = _httpContextAccessor.HttpContext.User.FindFirst("role")?.Value;
    
    if (userRole != "Admin")
    {
        dto.SocialSecurityNumber = null;  // Hide sensitive field for non-admins
        dto.Salary = null;
    }

    return dto;
}
```

### Q6: How do I audit permission changes?

**Domain events are published to RabbitMQ** for all permission changes. Set up an audit consumer:

```csharp
public class RbacAuditConsumer : IConsumer<PermissionAddedDomainEvent>
{
    private readonly IAuditLogger _auditLogger;

    public async Task Consume(ConsumeContext<PermissionAddedDomainEvent> context)
    {
        var @event = context.Message;
        
        await _auditLogger.LogAsync(new AuditEntry
        {
            Timestamp = @event.OccurredAt,
            EventType = "PermissionAdded",
            AggregateId = @event.AggregateId,
            UserId = @event.Metadata["UserId"],
            Details = $"Role '{@event.Role.Name}' granted access to {@event.Resource.Action}"
        });
    }
}
```

## 🤝 Contributing

Please read our Contributing Guide for details on our code of conduct and development process.

## 📄 License

This project is licensed under the GNU Lesser General Public License v3.0 - see the LICENSE.md file for details.

## 🔧 Tools

The repository includes several utility scripts in the tools directory:

- `convert-crlf-to-lf.sh`: Converts line endings
- `update-packages/`: Updates NuGet packages
- `upgrade-dotnet/`: Upgrades .NET version
- `vault/`: Vault configuration scripts
- `sonarqube/`: SonarQube analysis configuration

## 📦 Update Packages

To update the NuGet packages, run the following script:

```bash
cd tools/update-packages
./update-packages.sh
```

## 📦 Upgrading .NET Version

To upgrade the .NET version, run the following script:

```bash
cd tools/upgrade-dotnet
./upgrade-dotnet.sh
```

## 🧪 SonarQube Analysis

To run the SonarQube analysis, follow the instructions in the sonarqube directory.

1. Replace the SonarQube URL and token in the sonarqube.sh script to analyze with SonarQube locally.
2. Run the script:

   ```bash
   cd tools/sonarqube
   ./sonarqube.sh
   ```

## 📚 Documentation

1. REST API documentation available at `/swagger`
2. gRPC service definitions in `Protos/rbac.proto` or use `MapGrpcReflectionService` for reflection
3. Full SDK documentation: [CodeDesignPlus Doc Site](https://codedesignplus.github.io/)

## 📦 CodeDesignPlus Packages

This microservice uses the `CodeDesignPlus.Net.Sdk` package to simplify the development process. For more information, visit the [Doc Site](https://codedesignplus.github.io/).

---

**Built with ❤️ by CodeDesignPlus**
