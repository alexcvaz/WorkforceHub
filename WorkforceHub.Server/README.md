# WorkforceHub Server

Backend da aplicação WorkforceHub - API de registro de ponto.

**Stack**: ASP.NET Core 10 + Entity Framework Core

## Setup Rápido

```bash
# Restaurar dependências
dotnet restore

# Aplicar migrations (criar banco de dados)
dotnet ef database update

# Desenvolvimento
dotnet run

# Build para produção
dotnet publish -c Release
```

**API**: `http://localhost:5000`

## Configuração

### appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=workforcehub.db"
  },
  "Jwt": {
    "SecretKey": "sua-chave-super-secreta-aqui-minimo-32-caracteres",
    "ExpirationMinutes": 60,
    "Issuer": "WorkforceHub",
    "Audience": "WorkforceHubClient"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

## Estrutura do Projeto

```
WorkforceHub.Server/
??? Controllers/
?   ??? AuthController.cs        # Login/Register
?   ??? PunchController.cs       # Registro de ponto
?
??? Application/
?   ??? Services/
?   ?   ??? IPunchService.cs
?   ?   ??? PunchService.cs
?   ??? Interfaces/
?   ??? DTOs/
?   ?   ??? CreatePunchRequest.cs
?   ?   ??? PunchResponse.cs
?   ??? Extensions/
?       ??? ApplicationDependencyInjection.cs
?
??? Domain/
?   ??? Entities/
?   ?   ??? User.cs
?   ?   ??? Punch.cs
?   ??? Enums/
?       ??? PunchType.cs
?
??? Infrastructure/
?   ??? Data/
?   ?   ??? WorkforceHubDbContext.cs
?   ?   ??? DatabaseInitializer.cs
?   ??? Repositories/
?   ?   ??? PunchRepository.cs
?   ??? Migrations/
?   ??? Extensions/
?       ??? InfrastructureDependencyInjection.cs
?
??? Program.cs
??? appsettings.json
??? appsettings.Development.json
```

## Endpoints Principais

### Autenticação

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/login` | Login do usuário |
| POST | `/api/auth/register` | Registrar novo usuário |

### Punch

| Método | Endpoint | Descrição | Requer Auth |
|--------|----------|-----------|------------|
| POST | `/api/punch` | Registrar novo ponto | ? |
| GET | `/api/punch/today` | Pontos de hoje | ? |
| GET | `/api/punch/date/{date}` | Pontos de um dia específico | ? |
| GET | `/api/punch/range?startDate=&endDate=` | Pontos em período | ? |

### Exemplo de Uso

```bash
# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "usuario@example.com",
    "password": "senha123"
  }'

# Registrar ponto (com token JWT)
curl -X POST http://localhost:5000/api/punch \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_JWT" \
  -d '{"type": 1}'
```

## Banco de Dados

### Usando SQLite (Desenvolvimento)

```bash
# Criar migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Remover última migration
dotnet ef migrations remove
```

### Modelo de Dados

**Punches:**
```csharp
public class Punch
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public PunchType Type { get; set; }           // 1: ClockIn, 2: LunchStart, 3: LunchEnd, 4: ClockOut
    public DateTime Timestamp { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum PunchType
{
    ClockIn = 1,
    LunchStart = 2,
    LunchEnd = 3,
    ClockOut = 4
}
```

## Autenticação JWT

### Token Structure

```json
{
  "sub": "user-id",
  "email": "usuario@example.com",
  "name": "Usuário",
  "iat": 1704067200,
  "exp": 1704070800,
  "iss": "WorkforceHub",
  "aud": "WorkforceHubClient"
}
```

### Como Funciona

1. Usuário faz login
2. Backend gera JWT com expiração
3. Frontend armazena token em localStorage
4. Frontend envia token em cada requisição (header Authorization)
5. Backend valida token antes de processar

## Padrões de Código

### Service

```csharp
public interface IPunchService
{
    Task<PunchResponse> RegisterPunchAsync(string userId, CreatePunchRequest request, CancellationToken cancellationToken);
    Task<List<PunchResponse>> GetTodayPunchesAsync(string userId, CancellationToken cancellationToken);
}

public class PunchService : IPunchService
{
    private readonly IPunchRepository _repository;
    private readonly ILogger<PunchService> _logger;

    public PunchService(IPunchRepository repository, ILogger<PunchService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PunchResponse> RegisterPunchAsync(...)
    {
        // Implementação
    }
}
```

### Controller

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // Requer autenticação
public class PunchController : ControllerBase
{
    private readonly IPunchService _service;

    [HttpPost]
    public async Task<ActionResult<PunchResponse>> RegisterPunch(...)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(await _service.RegisterPunchAsync(...));
    }
}
```

## Logging

```csharp
_logger.LogInformation("Ponto registrado para usuário {UserId}", userId);
_logger.LogError(ex, "Erro ao registrar ponto para {UserId}", userId);
```

## CORS

Configurado em `Program.cs` para aceitar requisições do frontend:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## Deployment

### Pré-requisitos

- .NET 10 Runtime
- SQL Server (ou SQLite)

### Passos

1. Publicar a aplicação:
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. Configurar `appsettings.json` para produção

3. Executar:
   ```bash
   dotnet WorkforceHub.Server.dll
   ```

## Testes

```bash
# Executar todos os testes
dotnet test

# Testes com coverage
dotnet test /p:CollectCoverage=true

# Testes de um projeto específico
dotnet test WorkforceHub.Tests
```

## Troubleshooting

### Erro de conexão com banco de dados

```bash
# Deletar banco e recriar
rm workforcehub.db
dotnet ef database update
```

### Erro de migration

```bash
# Verificar migrations
dotnet ef migrations list

# Remover última migration (se não foi aplicada)
dotnet ef migrations remove
```

## Links Úteis

- [ASP.NET Core Docs](https://learn.microsoft.com/dotnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [JWT Authentication](https://tools.ietf.org/html/rfc7519)
- [REST API Best Practices](https://restfulapi.net/)

## Para Mais Informações

Veja o [README principal](../README.md) na raiz do projeto.
