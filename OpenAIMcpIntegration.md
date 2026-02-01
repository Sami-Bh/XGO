# OpenAI Function Integration Documentation

## Overview

This document describes the implementation of Azure AI Foundry agent integration in the XGO API Gateway. The integration exposes the `GetExpiringItems` endpoint from the Storage API as a callable function for AI agents through an auto-generated OpenAPI specification.

## Architecture

```
Azure AI Foundry Agent → API Gateway (OpenAIFunctionsController)
                              ↓ [Proxy with JWT Token]
                         Storage API (GetExpiringItems)
```

The integration uses **auto-generated OpenAPI specification** for Azure AI Foundry Agent Service integration. The OpenAPI specification is automatically generated from controller attributes and XML documentation comments using Microsoft.AspNetCore.OpenApi.

---

## Implementation Journey - Step by Step

This section documents the complete journey of implementing the OpenAPI integration, including all iterations, challenges, and solutions.

### Step 1: Initial Requirements & Approach Selection
**Goal**: Expose GetExpiringItems endpoint to AI agents

**Initial Discussion**:
- User mentioned wanting "MCP" integration
- Clarified they wanted OpenAI function calling (JSON schema) format
- Chose OpenAPI specification approach for Azure AI Foundry compatibility

**Decision**: Build OpenAPI 3.0 integration for universal AI platform support

### Step 2: First Attempt - Manual OpenAPI Schema Building
**Approach**: Manually create OpenAPI 3.1.0 specification using custom classes

**Files Created**:
- `OpenApiFunctionSchema.cs` - Complete OpenAPI 3.1.0 object model (OpenApiInfo, OpenApiServer, OpenApiOperation, OpenApiParameter, etc.)
- `GetExpiringItemsRequest.cs` - Request parameter binding model
- `OpenAIFunctionTool.cs` - Alternative function calling format

**Controller Implementation**: Manual schema construction in controller action

**User Feedback**: "How come we have to create all these classes by hand, is there no nuget that contains all these?"

**Lesson Learned**: Should use automatic OpenAPI generation instead of manual construction

### Step 3: Refactoring to Auto-Generation
**New Approach**: Use Microsoft.AspNetCore.OpenApi for runtime schema generation

**Changes Made**:
- Added `Microsoft.AspNetCore.OpenApi` NuGet package (initially v10.0.2)
- Added `builder.Services.AddOpenApi()` and `app.MapOpenApi()` in Program.cs
- Removed manual schema building code

**Problem Encountered**: Build errors with package version 10.0.2
```
Error: Source generator couldn't find IOpenApiOperationTransformer, OpenApiOperation types
```

**Root Cause**: Package version incompatibility with project

### Step 4: Resolving Package Version Issues
**Solution by User**:
1. Changed `Microsoft.AspNetCore.OpenApi` from 10.0.2 → **9.0.1**
2. Added `Microsoft.Extensions.ApiDescription.Server` **10.0.2**
3. Updated `Swashbuckle.AspNetCore` to **7.2.0**
4. Commented out `AddOpenApi()` and `MapOpenApi()` calls

**Result**: Build succeeded, project compiled successfully

**Lesson Learned**: Package versions matter - .NET 8 project needed v9.0.1, not v10.0.2

### Step 5: Simplification - Removing Unnecessary Code
**User Feedback**: "I didn't ask for Direct OpenAI API approach"

**Context**: Had implemented both:
- OpenAPI format (for Azure AI Foundry)
- OpenAI function calling format (for direct API)

**Actions Taken**:
1. Deleted `OpenAIFunctionTool.cs` file
2. Removed `/function-definition` endpoint from controller
3. Kept only Azure AI Foundry-focused implementation

**Final Endpoints**:
- GET `/api/OpenAIFunctions/schema` - Returns OpenAPI spec
- GET `/api/OpenAIFunctions/get-expiring-items` - Invokes function

**Lesson Learned**: Keep it simple - focus on actual requirements, avoid over-engineering

### Step 6: Missing XML Documentation in Generated Schema
**Problem Discovered**: Generated OpenAPI spec had no parameter descriptions

**Initial Testing**: User tested `/api/OpenAIFunctions/schema` endpoint
- Schema generated successfully
- But all `description` fields were empty
- Parameters had no explanatory text

**Investigation**:
1. Verified XML file (`XGO.ApiGateway.xml`) was being generated correctly
2. XML contained all documentation comments
3. Issue: `Microsoft.AspNetCore.OpenApi` doesn't automatically include XML comments

**Root Cause**: Microsoft.AspNetCore.OpenApi lacks built-in XML documentation integration

### Step 7: Switching to Swashbuckle for XML Documentation
**Solution**: Replace Microsoft.AspNetCore.OpenApi generation with Swashbuckle

**Changes Made in Program.cs**:
```csharp
// Added Swashbuckle configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "XGO Storage Functions API",
        Version = "v1.0.0",
        Description = "OpenAPI specification for XGroceries Optimizer storage management functions"
    });

    // Critical: Include XML documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Enable Swagger middleware
app.UseSwagger();
```

**XML Documentation Added to All Classes**:
```csharp
/// <summary>
/// Number of days until expiration to filter by.
/// If null, returns all items with expiration dates.
/// Example: 7 returns items expiring within the next week.
/// </summary>
public int? ExpiresInDays { get; set; }
```

**Result**: User confirmed all XML documentation now appears in generated OpenAPI JSON

**Lesson Learned**: Swashbuckle has better XML documentation support than Microsoft.AspNetCore.OpenApi

### Step 8: Final Testing & Validation
**Testing Performed**:
1. Rebuilt project - OpenAPI schema auto-generated successfully
2. Accessed `/swagger/v1/swagger.json` - Full documentation present
3. Verified schema endpoint redirects correctly
4. All XML comments appear in parameter descriptions

**Final OpenAPI JSON Structure**:
```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "XGO Storage Functions API",
    "description": "OpenAPI specification for XGroceries Optimizer storage management functions",
    "version": "v1.0.0"
  },
  "paths": {
    "/api/OpenAIFunctions/get-expiring-items": {
      "get": {
        "summary": "Invokes the GetExpiringItems function with authentication",
        "parameters": [
          {
            "name": "ExpiresInDays",
            "in": "query",
            "description": "Number of days until expiration to filter by. If null, returns all items with expiration dates. Example: 7 returns items expiring within the next week.",
            "schema": { "type": "integer", "nullable": true }
          }
          // ... other parameters with full descriptions
        ]
      }
    }
  }
}
```

**Success**: Complete auto-generated OpenAPI spec with comprehensive XML documentation

### Final Implementation Summary

**What Was Built**:
- 2 files created (Controller + Request Model)
- 2 files modified (Program.cs + .csproj)
- Auto-generated OpenAPI 3.0 specification
- JWT authentication with token forwarding
- Proxy pattern to Storage API
- Comprehensive XML documentation

**Key Technologies**:
- Swashbuckle.AspNetCore 7.2.0 for OpenAPI generation
- Microsoft.AspNetCore.OpenApi 9.0.1 for build-time generation
- Microsoft.Extensions.ApiDescription.Server 10.0.2 for tooling

**Architecture Pattern**:
```
Azure AI Foundry Agent
       ↓ (fetches schema once)
GET /api/OpenAIFunctions/schema
       ↓ (redirects to)
GET /swagger/v1/swagger.json
       ↓ (agent stores schema)

Then at runtime:
Azure AI Foundry Agent
       ↓ (function call with JWT)
GET /api/OpenAIFunctions/get-expiring-items?expiresInDays=7
       ↓ (proxy with JWT forwarding)
Storage API /api/StoredItems/GetExpiringItems
       ↓ (returns data)
Agent receives & formats response
```

**Integration with Azure AI Foundry**:
1. User opens Azure AI Foundry UI
2. Creates/edits agent
3. Adds "OpenAPI tool" (not MCP - different protocol)
4. Pastes JSON from `/swagger/v1/swagger.json`
5. Configures authentication (Anonymous for schema, JWT for invocation)
6. Agent now can call `get_expiring_items` function

**Key Learnings**:
1. Package versions matter significantly (.NET 8 needs specific versions)
2. Swashbuckle > Microsoft.AspNetCore.OpenApi for XML documentation
3. Keep implementation simple - focus on requirements
4. Auto-generation beats manual schema construction
5. Azure AI Foundry supports both OpenAPI and MCP protocols
6. Agent caches schema - doesn't refetch on every call

---

## Classes Summary

### NuGet Packages Used

- **Microsoft.AspNetCore.OpenApi (v9.0.1)** - Auto-generates OpenAPI specifications from ASP.NET Core controllers and XML documentation
- **Microsoft.Extensions.ApiDescription.Server (v10.0.2)** - Build-time OpenAPI document generation tooling
- **Swashbuckle.AspNetCore (v7.2.0)** - Swagger tooling for API documentation

### Classes Overview

| Source | Class | Purpose |
|--------|-------|---------|
| **XGO.ApiGateway.Models** | `GetExpiringItemsRequest` | Request model binding query parameters (expiresInDays, includeAcknowledgedExpiredItems, pageSize, pageIndex) |
| **XGO.ApiGateway.Controllers** | `OpenAIFunctionsController` | Main controller with 2 endpoints: schema and get-expiring-items |

### Quick Class Reference by Purpose

**For OpenAPI Specification (auto-generated):**
- Generated automatically from controller attributes, XML documentation, and model definitions
- Uses `[ApiExplorerSettings(IgnoreApi = true)]` to exclude the schema endpoint from the spec

**For Request Handling:**
- `GetExpiringItemsRequest` - Query parameter model

**For API Endpoints:**
- `OpenAIFunctionsController` - Two endpoints (schema, invocation)

---

## Project Structure

```
XGO.ApiGateway/
├── Controllers/
│   └── OpenAIFunctionsController.cs    # Main controller with 2 endpoints
├── Models/
│   └── GetExpiringItemsRequest.cs      # Request model for function invocation
├── Program.cs                          # Modified to add HttpClientFactory
├── XGO.ApiGateway.csproj               # Added OpenAPI generation packages and settings
└── XGO.ApiGateway.xml                  # Auto-generated XML documentation file
```

---

## Dependencies

### Microsoft.AspNetCore.OpenApi (v9.0.1)

**Purpose:** Enables automatic OpenAPI specification generation from ASP.NET Core controllers using attributes and XML documentation.

**Key Features:**
- Auto-generates OpenAPI documents from controller methods
- Uses XML documentation comments for descriptions
- Supports `[ApiExplorerSettings]` to control which endpoints are included
- Compatible with .NET 9 project structure

### Microsoft.Extensions.ApiDescription.Server (v10.0.2)

**Purpose:** Build-time tooling for generating OpenAPI documents during compilation.

**Configuration:**
```xml
<PropertyGroup>
  <OpenApiGenerateDocuments>true</OpenApiGenerateDocuments>
  <OpenApiDocumentsDirectory>$(MSBuildProjectDirectory)</OpenApiDocumentsDirectory>
</PropertyGroup>
```

### Swashbuckle.AspNetCore (v7.2.0)

**Purpose:** Swagger tooling for API documentation and testing UI (optional, for development).

### XML Documentation Generation

**Purpose:** Enables XML comments to be included in the OpenAPI specification.

**Configuration:**
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

---

## Models Summary

### GetExpiringItemsRequest.cs

**Location:** `XGO.ApiGateway/Models/GetExpiringItemsRequest.cs`

**Purpose:** Request model that maps HTTP query parameters to a strongly-typed object for the function invocation endpoint.

#### Class: `GetExpiringItemsRequest`

**Use:** Binds and validates query parameters when the OpenAI agent calls the function

**Properties:**
- `ExpiresInDays` (int?, optional) - Number of days until expiration to filter by
- `IncludeAcknowledgedExpiredItems` (bool, default: false) - Whether to include acknowledged items
- `PageSize` (int, default: 5) - Number of items per page
- `PageIndex` (int, default: 1) - Page number (1-based)

**Example Usage:**
```http
GET /api/OpenAIFunctions/get-expiring-items?expiresInDays=7&pageSize=10
```
Maps to:
```csharp
new GetExpiringItemsRequest {
    ExpiresInDays = 7,
    IncludeAcknowledgedExpiredItems = false,
    PageSize = 10,
    PageIndex = 1
}
```

---

## Controller Summary

### OpenAIFunctionsController.cs

**Location:** `XGO.ApiGateway/Controllers/OpenAIFunctionsController.cs`

**Purpose:** Exposes three endpoints for OpenAI function integration - schema serving, function definition serving, and function invocation with proxying to Storage API.

#### Dependencies:
- `IHttpClientFactory` - Creates HTTP clients for proxying requests to Storage API
- `IConfiguration` - Accesses configuration (Storage API URL)
- `ILogger` - Logs requests and errors

---

### Endpoints:

#### GET /api/OpenAIFunctions/schema

**Purpose:** Returns the auto-generated OpenAPI specification for the GetExpiringItems function

**Authentication:** `[AllowAnonymous]` - No authentication required (Azure AI Foundry needs to fetch schema)

**Response:** Redirects to `/openapi/v1.json` which contains the auto-generated OpenAPI specification

**Use Case:** Azure AI Foundry Agent Service reads this schema to discover what functions are available and how to call them

**Example Response:**
The auto-generated OpenAPI document includes:
- API information and version
- Path: `/api/OpenAIFunctions/get-expiring-items`
- GET operation with parameters from XML documentation
- Response schemas derived from controller attributes
- Security schemes (JWT Bearer)

**Implementation Details:**
- Redirects to `/openapi/v1.json` endpoint
- OpenAPI document is auto-generated by Microsoft.AspNetCore.OpenApi from:
  - Controller method attributes
  - XML documentation comments (/// summaries)
  - Model properties and data annotations
  - `[ApiExplorerSettings(IgnoreApi = true)]` on schema endpoint to exclude it from the spec
- Schema generation configured via:
  ```xml
  <OpenApiGenerateDocuments>true</OpenApiGenerateDocuments>
  <OpenApiDocumentsDirectory>$(MSBuildProjectDirectory)</OpenApiDocumentsDirectory>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  ```

---

#### GET /api/OpenAIFunctions/get-expiring-items

**Purpose:** Invokes the actual function - acts as a proxy to the Storage API's GetExpiringItems endpoint

**Authentication:** `[Authorize]` in production, `[AllowAnonymous]` in DEBUG mode

**Parameters:** Query string parameters mapped to `GetExpiringItemsRequest`
- `expiresInDays` (optional) - Filter by days until expiration
- `includeAcknowledgedExpiredItems` (optional, default: false)
- `pageSize` (optional, default: 5)
- `pageIndex` (optional, default: 1)

**Response:** JSON object containing paginated list of expiring items from Storage API

**Example Request:**
```http
GET /api/OpenAIFunctions/get-expiring-items?expiresInDays=7&pageSize=10
Authorization: Bearer eyJhbGc...
```

**Example Response:**
```json
{
  "pageCount": 2,
  "items": [
    {
      "id": 123,
      "productId": 456,
      "productName": "Milk",
      "productExpiryDate": "2026-02-05",
      "quantity": 2,
      "storageLocationId": 789
    },
    ...
  ]
}
```

**Implementation Flow:**
1. Retrieves Storage API URL from configuration (`ReverseProxy:Clusters:Storage_Cluster:Destinations:destination1:Address`)
2. Creates HTTP client using `IHttpClientFactory`
3. Forwards `Authorization` header from incoming request to Storage API
4. Builds query string from request parameters
5. Makes GET request to Storage API: `{storageApiUrl}/api/StoredItems/GetExpiringItems?{queryParams}`
6. Logs the request for debugging
7. Returns Storage API response as-is if successful
8. Returns error status code and content if Storage API returns error
9. Returns 500 with error details if exception occurs

**Error Handling:**
- 500 if Storage API URL not configured
- Passes through Storage API error status codes (401, 404, etc.)
- 500 with exception details for unexpected errors
- All errors are logged with `ILogger`

---

## Configuration Changes

### Program.cs

**Location:** `XGO.ApiGateway/Program.cs`

**Changes:**
1. Added `builder.Services.AddHttpClient();` after `AddControllers()`
   - Registers `IHttpClientFactory` for proxying requests to Storage API
   - Manages HttpClient lifecycle properly (avoids socket exhaustion)
   - Provides connection pooling and thread-safety

2. OpenAPI generation configuration (currently commented out):
   ```csharp
   // builder.Services.AddOpenApi();  // Commented out
   // app.MapOpenApi();                // Commented out
   ```
   - OpenAPI document generation is handled by build-time tooling via `Microsoft.Extensions.ApiDescription.Server`
   - Build-time generation is configured in .csproj file

### XGO.ApiGateway.csproj

**Location:** `XGO.ApiGateway/XGO.ApiGateway.csproj`

**Changes:**

1. **Added NuGet Packages:**
   ```xml
   <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.1" />
   <PackageReference Include="Microsoft.Extensions.ApiDescription.Server" Version="10.0.2">
     <PrivateAssets>all</PrivateAssets>
     <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
   </PackageReference>
   <PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
   ```

2. **Added OpenAPI Generation Settings:**
   ```xml
   <PropertyGroup>
     <OpenApiGenerateDocuments>true</OpenApiGenerateDocuments>
     <OpenApiDocumentsDirectory>$(MSBuildProjectDirectory)</OpenApiDocumentsDirectory>
   </PropertyGroup>
   ```

3. **Added XML Documentation Generation:**
   ```xml
   <PropertyGroup>
     <GenerateDocumentationFile>true</GenerateDocumentationFile>
     <NoWarn>$(NoWarn);1591</NoWarn>
   </PropertyGroup>
   ```
   - Enables XML comments to be included in OpenAPI specification
   - Suppresses warning 1591 (missing XML comments)

---

## Usage Scenarios

### Azure AI Foundry Agent Service

**Steps:**

1. **Deploy API Gateway to Azure**
   - Schema endpoint becomes available at: `https://your-gateway.azurewebsites.net/api/OpenAIFunctions/schema`

2. **Create Agent in Azure AI Foundry** (Python)
```python
import requests
from azure.ai.projects import AIProjectClient
from azure.identity import DefaultAzureCredential

# Fetch OpenAPI schema
schema_url = "https://your-gateway.azurewebsites.net/api/OpenAIFunctions/schema"
openapi_spec = requests.get(schema_url).json()

# Create OpenAPI tool
xgo_storage_tool = {
    "type": "openapi",
    "openapi": {
        "name": "xgo_storage",
        "description": "XGroceries Optimizer storage functions",
        "spec": openapi_spec,
        "auth": {
            "type": "managed_identity",
            "security_scheme": {
                "audience": "your-azure-ad-app-id"
            }
        }
    }
}

# Create agent
with (
    DefaultAzureCredential() as credential,
    AIProjectClient(endpoint=endpoint, credential=credential) as project_client,
    project_client.get_openai_client() as openai_client,
):
    agent = project_client.agents.create_version(
        agent_name="GroceryInventoryAgent",
        definition=PromptAgentDefinition(
            model="gpt-4o",
            instructions="You help users manage grocery inventory and reduce food waste.",
            tools=[xgo_storage_tool]
        )
    )
```

3. **Use the Agent**
```python
response = openai_client.responses.create(
    input="What items are expiring in the next 3 days?",
    extra_body={"agent": {"name": agent.name, "type": "agent_reference"}}
)
print(response.output_text)
```

**Flow:**
1. User asks: "What items are expiring in the next 3 days?"
2. Agent reads OpenAPI schema from `/api/OpenAIFunctions/schema`
3. Agent determines it should call the `GetExpiringItems` operation with `expiresInDays=3`
4. Agent calls `/api/OpenAIFunctions/get-expiring-items?expiresInDays=3` with Managed Identity token
5. API Gateway validates JWT, proxies to Storage API
6. Storage API returns expiring items
7. API Gateway returns data to agent
8. Agent formats response: "You have 5 items expiring in the next 3 days: Milk (Feb 1), Yogurt (Feb 2)..."

---

## Local Testing

### Prerequisites
- .NET 8 SDK
- Both XGO.ApiGateway and XGO.Storage.Api running

### Start APIs
```bash
# Terminal 1 - Storage API
cd c:\Users\bouhajja.s\Documents\XGroceriesOptmizer\XGO.Storage\XGO.Storage.Api
dotnet run

# Terminal 2 - API Gateway
cd c:\Users\bouhajja.s\Documents\XGroceriesOptmizer\XGO.ApiGateway
dotnet run
```

### Test Endpoints

**1. Test Schema Endpoint**
```bash
curl http://localhost:{port}/api/OpenAIFunctions/schema
```
Expected: Redirect to `/openapi/v1.json` with auto-generated OpenAPI specification

**2. Test Function Invocation**
```bash
curl "http://localhost:{port}/api/OpenAIFunctions/get-expiring-items?expiresInDays=7&pageSize=10"
```
Expected: Paginated list of expiring items

---

## Security Considerations

1. **Schema Endpoint (`/schema`)**
   - Unauthenticated (safe - only returns metadata)
   - No sensitive data exposed

2. **Invocation Endpoint (`/get-expiring-items`)**
   - **Requires JWT authentication in production**
   - Forwards Authorization header to Storage API
   - Validates token using Microsoft Identity Web (Azure AD)
   - DEBUG mode allows anonymous for local testing only

3. **Token Forwarding**
   - Authorization header is forwarded from API Gateway to Storage API
   - No token is stored or logged
   - Uses same authentication mechanism as existing YARP proxy

---

## Performance Considerations

1. **HTTP Client Management**
   - Uses `IHttpClientFactory` for proper connection pooling
   - Avoids socket exhaustion
   - Automatically manages client lifecycle

2. **Response Proxying**
   - Returns Storage API response as-is (no JSON parsing overhead)
   - Content type forwarded correctly

3. **Error Handling**
   - Fails fast if Storage API URL not configured
   - Passes through Storage API errors without processing

---

## Extensibility

To add more functions in the future:

1. **Add new action method** to controller with XML documentation:
   ```csharp
   /// <summary>
   /// Adds a new item to storage inventory
   /// </summary>
   [HttpPost("add-item")]
   [Authorize]
   public async Task<IActionResult> AddItem([FromBody] AddItemRequest request)
   {
       // Proxying logic similar to GetExpiringItems
   }
   ```

2. **Add XML documentation comments** for parameters and models - these automatically appear in the OpenAPI spec

3. **Create new request model** with XML documentation:
   ```csharp
   /// <summary>
   /// Request model for adding items to storage
   /// </summary>
   public class AddItemRequest
   {
       /// <summary>
       /// The product ID to add
       /// </summary>
       public int ProductId { get; set; }
   }
   ```

The OpenAPI specification will automatically update based on:
- New controller methods (not marked with `[ApiExplorerSettings(IgnoreApi = true)]`)
- XML documentation comments
- Model properties and data annotations

---

## Troubleshooting

### Issue: Schema endpoint returns 404
- **Solution:** Ensure API Gateway is running and route is correct: `/api/OpenAIFunctions/schema`

### Issue: Function invocation returns 401
- **Solution:**
  - Verify JWT token is valid
  - Check Authorization header format: `Bearer {token}`
  - Ensure Azure AD configuration is correct in `appsettings.Development.json`

### Issue: Function invocation returns 500 "Storage API URL not configured"
- **Solution:** Check `appsettings.json` has `ReverseProxy:Clusters:Storage_Cluster:Destinations:destination1:Address`

### Issue: Agent can't find the function
- **Solution:**
  - Verify schema endpoint is accessible from Azure AI Foundry
  - Check OpenAPI spec is valid (validate with OpenAPI validator)
  - Ensure operationId matches what agent expects

---

## Future Enhancements

1. ~~**Auto-generate OpenAPI schema from controller attributes**~~ ✅ **Implemented**
   - Using Microsoft.AspNetCore.OpenApi for auto-generation
   - Schema automatically reflects controller changes

2. **Add more Storage functions**
   - AddItem, UpdateItem, DeleteItem
   - GetStorageLocations
   - AcknowledgeExpiredItem

3. **Rate limiting**
   - Throttle agent function calls
   - Prevent abuse

4. **Caching**
   - Cache schema endpoint responses
   - Reduce latency for agent discovery

5. **Monitoring and telemetry**
   - Application Insights integration
   - Track function call frequency
   - Monitor error rates

6. **Webhook notifications**
   - Proactive notifications instead of polling
   - Push notifications for expiring items

---

## Implementation Notes

### Current Architecture (v2.0 - Auto-generated)

The implementation uses **build-time OpenAPI document generation** via `Microsoft.Extensions.ApiDescription.Server`:

1. **During Build:**
   - Source generators analyze controllers and XML documentation
   - OpenAPI document is generated and saved to project directory
   - Document includes all controller endpoints except those marked with `[ApiExplorerSettings(IgnoreApi = true)]`

2. **At Runtime:**
   - Schema endpoint (`/api/OpenAIFunctions/schema`) redirects to `/openapi/v1.json`
   - The generated document is served statically
   - No runtime OpenAPI generation overhead

3. **Configuration:**
   - `AddOpenApi()` and `MapOpenApi()` are **commented out** in [Program.cs](XGO.ApiGateway/Program.cs)
   - Build-time generation is controlled by .csproj properties:
     ```xml
     <OpenApiGenerateDocuments>true</OpenApiGenerateDocuments>
     <OpenApiDocumentsDirectory>$(MSBuildProjectDirectory)</OpenApiDocumentsDirectory>
     ```

### Package Versions

The specific package versions used are important for compatibility:
- **Microsoft.AspNetCore.OpenApi 9.0.1** (not 10.0.2 - version mismatch caused build errors)
- **Microsoft.Extensions.ApiDescription.Server 10.0.2**
- **Swashbuckle.AspNetCore 7.2.0**

### Benefits of This Approach

- **Zero runtime overhead** - OpenAPI document generated at build time
- **Type-safe** - Compiler validates all attributes and XML comments
- **Maintainable** - Schema automatically reflects code changes
- **Developer-friendly** - Standard ASP.NET Core attributes and XML comments

---

## References

- [Azure AI Foundry Function Calling](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/function-calling)
- [Azure AI Foundry OpenAPI Tools](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/openapi)
- [OpenAPI Specification 3.1.0](https://spec.openapis.org/oas/v3.1.0)
- [Microsoft.AspNetCore.OpenApi Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi)
