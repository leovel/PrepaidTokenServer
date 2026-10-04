# MultiLsTokenServer

MultiLsTokenServer is a .NET 10 service for communicating with a security module (HSM) and exposing prepaid-token operations (for Energy, Water, Gas and Time) over HTTP and gRPC. The server builds HSM commands, validates and formats request data, sends commands to the configured HSM over TCP, and returns structured operation results.

The solution contains both an HTTP API and a gRPC API. The gRPC host provides the broader set of operations; the HTTP host currently exposes diagnostics and electricity vending.

## Capabilities

- Check HSM connectivity and query module identification, firmware, and date information.
- Generate STS transfer-credit and transfer-currency tokens for electricity, water, gas, and time vending through gRPC.
- Generate management tokens, including maximum power limit, clear credit, tariff rate, tamper condition, phase power unbalance, water meter factor, and key change operations.
- Verify an STS token and generate meter test/display tokens.
- Apply request validation, response headers, HSM response-code mapping, token formatting, transfer amount encoding, and Luhn check-digit calculation.

## Solution architecture

| Project | Responsibility |
| --- | --- |
| `MultiLsTokenServer.Web.Api` | ASP.NET Core HTTP API, controller routes, and Swagger UI. |
| `MultiLsTokenServer.Grpc.Api` | HTTP/2 gRPC host and service implementations. |
| `MultiLsTokenService.Contract` | Protobuf service definitions and generated gRPC types shared by the server. |
| `MultiLsTokenServer.Infrastructure` | HSM TCP client and command implementations for diagnostics, vending, management, and meter tests. |
| `MultiLsTokenServer.Domain` | Request/response models, validation, service interfaces, and domain exceptions. |
| `MultiLsTokenServer.Algorithms` | Check-digit algorithms, including Luhn. |

## Requirements

- .NET 10 SDK.
- Network access to a compatible HSM/security module over TCP.
- For building `MultiLsTokenServer.Grpc.Api`, the referenced `MultiLsVendingSystem.ServiceDefaults` project must be available at the relative path specified in `MultiLsTokenServer.Grpc.Api.csproj` (`../../MultiLsVendingSystem/MultiLsVendingSystem.ServiceDefaults/MultiLsVendingSystem.ServiceDefaults.csproj`). This dependency is outside the six projects listed in this solution.

## Configuration

Both hosts read the HSM endpoint from `HSM_HOST` and `HSM_PORT`. Set these values to the address and TCP port reachable from the server process. The checked-in appsettings use different host defaults for the HTTP and gRPC projects, so configure them explicitly for your environment.

| Host | `HSM_HOST` in appsettings | `HSM_PORT` |
| --- | --- | --- |
| HTTP | `172.19.128.1` | `5100` |
| gRPC | `172.25.16.1` | `5100` |

For local development in PowerShell, environment variables can be set before launching either host:

```powershell
$env:HSM_HOST = "<reachable-hsm-host>"
$env:HSM_PORT = "5100"
```

The values above are configuration examples; use the HSM endpoint for your environment. A running API does not indicate that the HSM is reachable—token operations require an active connection to the configured device.

## Build and run

Build the solution from its root directory:

```powershell
dotnet build .\MultiLsTokenServer.sln
```

Run the HTTP API:

```powershell
dotnet run --project .\MultiLsTokenServer.Web.Api\MultiLsTokenServer.Web.Api.csproj
```

The HTTP development profile listens at `http://localhost:8080`. Swagger UI is available at `http://localhost:8080/swagger`.

Run the gRPC API in a separate terminal:

```powershell
dotnet run --project .\MultiLsTokenServer.Grpc.Api\MultiLsTokenServer.Grpc.Api.csproj
```

The gRPC launch profiles configure HTTP/2 endpoints at `http://0.0.0.0:5480` and `https://0.0.0.0:5443`. The root endpoint returns `All Services Running.` The gRPC service and message definitions are in `MultiLsTokenService.Contract/Protos`.

## API overview

### HTTP

The HTTP API uses the `prepaid` route prefix:

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/prepaid/Diagnostic/online` | Check security-module connectivity. |
| `GET` | `/prepaid/Diagnostic/identification` | Get module and firmware identification. |
| `GET` | `/prepaid/Diagnostic/query-identification` | Query public-key, hardware, firmware, and firmware-hash identifiers. |
| `GET` | `/prepaid/Diagnostic/date` | Query the module's real-time clock and window size. |
| `POST` | `/prepaid/Electricity/credit` | Generate an electricity transfer-credit token. |

Interactive HTTP API documentation is available from Swagger when the HTTP host is running.

### gRPC

The gRPC host registers these protobuf services:

- `DiagnosticApi`
- `VendingElectricityApi`, `VendingWaterApi`, `VendingGasApi`, and `VendingTimeApi`
- `ManagementApi`
- `TestOrDisplayApi`

See the `.proto` files in `MultiLsTokenService.Contract/Protos` for RPC names, request/response schemas, and field definitions.

## Operational notes

- Configure the HSM address before running the service in any environment other than one matching the checked-in defaults.
- The API exposes token generation and verification operations. Restrict network access appropriately and apply the authentication/authorization controls required by your deployment; the current API startup does not configure an authentication scheme.
- The solution targets `net10.0` across its six listed projects. The gRPC project also references a shared ServiceDefaults project outside this solution, as noted above.
