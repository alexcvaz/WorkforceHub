# WorkforceHub

Sistema completo de registro de ponto (punch clock) com interface moderna e responsiva.

![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow)
![.NET](https://img.shields.io/badge/.NET-10-blue)
![Vue](https://img.shields.io/badge/Vue-3-green)
![License](https://img.shields.io/badge/License-MIT-blue)

## Sobre o Projeto

WorkforceHub é uma aplicação full-stack para controle de ponto de entrada e saída de funcionários. Oferece uma interface limpa e intuitiva para registrar pontos, visualizar histórico mensal e gerenciar registros de forma eficiente.

### Características Principais

- **Autenticação JWT** - Segurança com tokens JWT
- **Registro de Ponto** - Entrada, saída e intervalo
- **Visualização Mensal** - Tabela com histórico de registros
- **Relógio em Tempo Real** - Data e hora sincronizadas
- **Responsivo** - Desktop, tablet e mobile
- **Interface Moderna** - Design clean e profissional
- **Persistência** - Banco de dados SQLite/SQL Server
- **Histórico Completo** - Rastreamento de IP e User-Agent

## Arquitetura

### Stack Tecnológico

#### Frontend
- **Framework**: Vue 3 (Composition API)
- **Linguagem**: TypeScript
- **Build**: Vite
- **Roteamento**: Vue Router
- **Requisições**: Fetch API
- **Estado**: Composables Vue

#### Backend
- **Framework**: ASP.NET Core 10
- **ORM**: Entity Framework Core
- **Banco de Dados**: SQLite (dev) / SQL Server (prod)
- **Autenticação**: JWT
- **Padrão**: Clean Architecture

### Estrutura do Projeto

```
WorkforceHub/
??? WorkforceHub.Server/              # Backend .NET 10
?   ??? Controllers/                  # Endpoints HTTP
?   ??? Application/
?   ?   ??? Services/                 # Lógica de negócio
?   ?   ??? Interfaces/               # Contratos
?   ?   ??? DTOs/                     # Transfer Objects
?   ??? Domain/
?   ?   ??? Entities/                 # Modelos de domínio
?   ?   ??? Enums/                    # Enumerações
?   ??? Infrastructure/
?       ??? Data/                     # DbContext e Migrations
?       ??? Repositories/             # Data Access
?
??? WorkforceHub.Client/              # Frontend Vue 3
?   ??? src/
?       ??? components/               # Componentes reutilizáveis
?       ??? composables/              # Vue Composables
?       ??? views/                    # Páginas
?       ??? services/                 # API Client
?       ??? types/                    # TypeScript Types
?       ??? router/                   # Configuração de Rotas
?       ??? assets/                   # CSS e Imagens
?
??? .github/
    ??? copilot-instructions.md       # Instruções para IA
```

## ?? Quick Start

### Pré-requisitos

- Node.js 18+ (Frontend)
- .NET 10 SDK (Backend)
- SQLite ou SQL Server

### Setup Frontend

```bash
cd WorkforceHub.Client

# Instalar dependências
npm install

# Desenvolvimento (com hot-reload)
npm run dev

# Build para produção
npm run build

# Lint
npm run lint
```

**Frontend estará disponível em**: `http://localhost:5173`

### Setup Backend

```bash
cd WorkforceHub.Server

# Restaurar dependências
dotnet restore

# Aplicar migrations
dotnet ef database update

# Desenvolvimento
dotnet run

# Build para produção
dotnet publish -c Release
```

**Backend estará disponível em**: `http://localhost:5000`

### Variáveis de Ambiente

#### Frontend (`.env.local`)
```env
VITE_API_URL=http://localhost:5000/api
VITE_APP_NAME=WorkforceHub
```

#### Backend (`appsettings.Development.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=workforcehub.db"
  },
  "Jwt": {
    "SecretKey": "sua-chave-secreta-muito-segura",
    "ExpirationMinutes": 60
  }
}
```

## Funcionalidades

### Para o Usuário

- Login/Logout com JWT
- Registrar novo ponto
- Ver histórico de pontos do dia
- Visualizar registros em formato tabular (mensal)
- Navegar entre meses
- Relógio em tempo real
- Notificações de sucesso/erro

### Para o Sistema

- Armazenamento seguro de registros
- Rastreamento de IP e User-Agent
- Validação de dados
- Tratamento de erros
- Logging estruturado
- Sincronização de estado com Provide/Inject

## Segurança

- JWT com expiração configurável
- Validação de entrada no servidor
- CORS configurado
- Proteção contra XSS
- Hash de senhas (quando aplicável)
- Endpoints protegidos com `[Authorize]`

## Banco de Dados

### Tabelas Principais

- **Users** - Usuários do sistema
- **Punches** - Registros de ponto
- **Migrations** - Histórico de migrations EF Core

### Schema

```sql
-- Punches
CREATE TABLE Punches (
  Id GUID PRIMARY KEY,
  UserId NVARCHAR(450),
  Type INT,                    -- 1: ClockIn, 2: LunchStart, 3: LunchEnd, 4: ClockOut
  Timestamp DATETIME2,
  IpAddress NVARCHAR(50),
  UserAgent NVARCHAR(500),
  CreatedAt DATETIME2
);
```

## Testes

### Frontend

```bash
npm run lint              # ESLint
npm run type-check        # TypeScript check
```

### Backend

```bash
dotnet test              # Executar testes
```

## Padrões de Código

### Frontend (Vue 3 + TypeScript)

```typescript
<script setup lang="ts">
// Usar setup script e TypeScript
const value = ref<string>('')
const handleClick = () => { /* ... */ }
</script>

<style scoped>
/* Sempre usar scoped */
</style>
```

### Backend (C# .NET 10)

```csharp
// Async/Await
public async Task<ActionResult<T>> GetAsync(...)
{
  return Ok(await _service.GetAsync(...));
}

// Dependency Injection
public MyController(IService service) 
{
  _service = service;
}
```

## Como Contribuir

1. **Criar uma branch** para sua feature
   ```bash
   git checkout -b feature/minha-feature
   ```

2. **Implementar mudanças** seguindo os padrões

3. **Testar** antes de commitar
   ```bash
   npm run lint          # Frontend
   dotnet test           # Backend
   ```

4. **Commitar** com mensagem descritiva
   ```bash
   git commit -m "feat: adicionar nova funcionalidade"
   ```

5. **Push** e criar Pull Request
   ```bash
   git push origin feature/minha-feature
   ```

### Formato de Commits

```
[TIPO] Descrição breve (máx 50 chars)

- O quê foi feito
- Por quê foi feito
- Como impacta o projeto

Tipos: feat, fix, refactor, style, chore, docs
```

## Convenções

### Nomenclatura

- **Componentes**: `PascalCase` (PunchButton.vue)
- **Variáveis/funções**: `camelCase` (getPunches)
- **Classes C#**: `PascalCase` (PunchService)
- **Constantes**: `UPPER_SNAKE_CASE` (API_BASE_URL)
- **Interfaces**: `IPascalCase` (IPunchService)

### Estrutura de Pastas

- Agrupar por funcionalidade
- Manter componentes pequenos e reutilizáveis
- Services para lógica de negócio
- Types centralizados

## Problemas Conhecidos

- SQLite é apenas para desenvolvimento (usar SQL Server em produção)
- Sem autenticação de 2FA
- Sem refresh automático de dados (requer F5)
- Sem backup automático

## Roadmap

- [ ] Autenticação com 2FA
- [ ] Relatórios de ponto
- [ ] Exportação para Excel/PDF
- [ ] Dashboard administrativo
- [ ] Notificações por email
- [ ] App mobile (Flutter/React Native)
- [ ] Sincronização offline

## Documentação Adicional

- [Setup Inicial](./docs/SETUP.md)
- [Guia de Arquitetura](./docs/ARCHITECTURE.md)
- [API Reference](./docs/API.md)
- [Instruções para IA](./.github/copilot-instructions.md)

## IDE Setup Recomendado

### Frontend

- **VS Code** + [Vue (Official)](https://marketplace.visualstudio.com/items?itemName=Vue.volar)
- **ESLint Extension**
- **Prettier Extension**
- **TypeScript Vue Plugin (Volar)**

### Backend

- **Visual Studio 2022** ou **VS Code**
- **C# Extension (Official)**
- **.NET Extension Pack**

### Extensões Navegador

- **Chrome/Edge**: [Vue.js DevTools](https://chromewebstore.google.com/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)
- **Firefox**: [Vue.js DevTools](https://addons.mozilla.org/en-US/firefox/addon/vue-js-devtools/)

## Suporte

Para dúvidas ou problemas:
- Abrir uma **Issue** no GitHub
- Verificar discussões existentes
- Consultar a documentação

## License

Este projeto está sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## Autor

Desenvolvido como aplicação de registro de ponto.

---

**Última atualização**: Fevereiro 2026  
**Versão**: 1.0.0-beta
