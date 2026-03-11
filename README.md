# 🎸 BandHub Backend

<p align="center">
  <strong>Plataforma para conectar músicos, bandas e fãs</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8" />
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL" />
  <img src="https://img.shields.io/badge/EF_Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="EF Core" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
  <img src="https://img.shields.io/badge/xUnit-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="xUnit" />
</p>

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Configuração e Instalação](#-configuração-e-instalação)
- [Banco de Dados e Migrations](#-banco-de-dados-e-migrations)
- [Executando a Aplicação](#-executando-a-aplicação)
- [Testes](#-testes)
- [Endpoints da API](#-endpoints-da-api)
- [Contribuindo](#-contribuindo)

---

## 📖 Sobre o Projeto

O **BandHub** é uma plataforma backend construída com arquitetura de microsserviços para conectar músicos, bandas e fãs. Cada serviço é independente, com seu próprio banco de dados e responsabilidades bem definidas.

### Microsserviços

| Serviço | Porta | Banco de Dados | Descrição |
|---------|-------|----------------|-----------|
| **UserService** | `5293` | `users_db` | Gerenciamento de contas (registro, login e consulta) |
| **BandService** | `5081` | `bands_db` | Gerenciamento de bandas (vinculadas a contas) |

---

## 🏗 Arquitetura

O projeto segue a arquitetura **Vertical Slice Architecture**, onde cada feature é organizada em sua própria pasta contendo todos os componentes necessários (endpoint, handler, request, response e validator).

```
Feature/
├── Endpoint.cs      → Define a rota HTTP (Minimal API)
├── Handler.cs       → Lógica de negócio
├── Request.cs       → Contrato de entrada
├── Response.cs      → Contrato de saída
└── Validator.cs     → Validação de entrada
```

### Princípios aplicados

- **Vertical Slice Architecture** — cada feature isolada com seus próprios componentes
- **Minimal APIs** — endpoints leves e performáticos
- **Repository Pattern** — abstração do acesso a dados
- **Dependency Injection** — inversão de dependência nativa do .NET
- **Database per Service** — cada microsserviço com seu próprio banco

---

## 🛠 Tecnologias

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET | 8.0 | Framework principal |
| ASP.NET Core | 8.0 | Web API com Minimal APIs |
| Entity Framework Core | 8.0.24 | ORM / Acesso a dados |
| Npgsql | 8.0.11 | Provider PostgreSQL para EF Core |
| PostgreSQL | — | Banco de dados relacional |
| Swagger / Swashbuckle | 6.6.2 | Documentação da API |
| xUnit | 2.5.3 | Framework de testes |
| Moq | 4.20.72 | Mocking para testes unitários |
| FluentAssertions | 8.8.0 | Assertions expressivas |

---

## 📁 Estrutura do Projeto

```
bandhub-backend-dotnet/
│
├── BandHub.UserService/                    # Microsserviço de Contas
│   ├── Features/
│   │   └── Accounts/
│   │       ├── RegisterAccount/
│   │       │   ├── RegisterAccountEndpoint.cs
│   │       │   ├── RegisterAccountHandler.cs
│   │       │   ├── RegisterAccountRequest.cs
│   │       │   ├── RegisterAccountResponse.cs
│   │       │   └── RegisterAccountValidator.cs
│   │       ├── Login/
│   │       │   ├── LoginEndpoint.cs
│   │       │   ├── LoginHandler.cs
│   │       │   ├── LoginRequest.cs
│   │       │   └── LoginResponse.cs
│   │       ├── GetAccounts/
│   │       │   ├── GetAccountsEndpoint.cs
│   │       │   ├── GetAccountsHandler.cs
│   │       │   └── GetAccountsResponse.cs
│   │       └── Domain/
│   │           ├── Account.cs
│   │           ├── AccountType.cs
│   │           └── IAccountRepository.cs
│   ├── Infrastructure/
│   │   └── Persistence/
│   │       ├── AccountDbContext.cs
│   │       └── AccountRepository.cs
│   ├── Common/
│   │   └── DependencyInjection.cs
│   ├── Migrations/
│   ├── Program.cs
│   ├── appsettings.json
│   └── BandHub.UserService.csproj
│
├── BandHub.BandService/                    # Microsserviço de Bandas
│   ├── Features/
│   │   └── Bands/
│   │       ├── CreateBand/
│   │       │   ├── CreateBandEndpoint.cs
│   │       │   ├── CreateBandHandler.cs
│   │       │   ├── CreateBandRequest.cs
│   │       │   ├── CreateBandResponse.cs
│   │       │   └── CreateBandValidator.cs
│   │       ├── GetBands/
│   │       │   ├── GetBandsEndpoint.cs
│   │       │   ├── GetBandsHandler.cs
│   │       │   └── GetBandsResponse.cs
│   │       └── Domain/
│   │           ├── Band.cs
│   │           └── IBandRepository.cs
│   ├── Infrastructure/
│   │   └── Persistence/
│   │       ├── BandDbContext.cs
│   │       └── BandRepository.cs
│   ├── Common/
│   │   └── DependencyInjection.cs
│   ├── Migrations/
│   ├── Program.cs
│   ├── appsettings.json
│   └── BandHub.BandService.csproj
│
├── tests/
│   ├── BandHub.UserService.UnitTests/     # Testes unitários do UserService
│   │   └── Features/
│   │       └── Accounts/
│   │           ├── CreateAccount/
│   │           │   ├── CreateAccountHandlerTests.cs
│   │           │   └── CreateAccountValidatorTests.cs
│   │           └── GetAccounts/
│   │               └── GetAccountsHandlerTests.cs
│   │
│   └── BandHub.BandService.UnitTests/     # Testes unitários do BandService
│       └── Features/
│           └── Bands/
│               ├── CreateBand/
│               │   ├── CreateBandHandlerTests.cs
│               │   └── CreateBandValidatorTests.cs
│               └── GetBands/
│                   └── GetBandsHandlerTests.cs
│
├── bandhub-backend-dotnet.sln
├── .gitignore
└── README.md
```

---

## ✅ Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0)
- [**Docker**](https://www.docker.com/products/docker-desktop/) (para rodar o PostgreSQL)
- [**EF Core CLI**](#instalando-o-ef-core-cli) (para gerenciar migrations)

### Instalando o EF Core CLI

```bash
dotnet tool install --global dotnet-ef
```

Para verificar se já está instalado:

```bash
dotnet ef --version
```

> 💡 **Dica:** Se já tiver uma versão antiga, atualize com:
> ```bash
> dotnet tool update --global dotnet-ef
> ```

---

## ⚙ Configuração e Instalação

### 1. Clone o repositório

```bash
git clone https://github.com/bandhub-br/bandhub-backend-dotnet.git
cd bandhub-backend-dotnet
```

### 2. Restaure as dependências

```bash
dotnet restore
```

### 3. Configure o PostgreSQL via Docker

> ⚠️ O banco de dados é sempre executado via **Docker**. A configuração da infraestrutura (Docker Compose, containers, etc.) está no repositório de infra.

Certifique-se de que o container do PostgreSQL está rodando e acessível na porta `5432` antes de prosseguir.

### 4. Verifique as connection strings

As connection strings estão nos arquivos `appsettings.json` de cada serviço:

**`BandHub.UserService/appsettings.json`**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=users_db;Username=bandhub;Password=bandhub"
  }
}
```

**`BandHub.BandService/appsettings.json`**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=bands_db;Username=bandhub;Password=bandhub"
  }
}
```

---

## 🗄 Banco de Dados e Migrations

O projeto utiliza **Entity Framework Core** com **Code-First Migrations** para gerenciar o schema do banco de dados.

### Conceitos importantes

| Conceito | Descrição |
|----------|-----------|
| **Migration** | Um arquivo C# que descreve alterações no schema do banco de dados |
| **DbContext** | Classe que representa a sessão com o banco de dados |
| **Snapshot** | Arquivo que guarda o estado atual do modelo para comparação |

### Comandos de Migration

> ⚠️ **Importante:** Todos os comandos devem ser executados na **raiz da solution** (onde está o arquivo `.sln`).

#### 📌 Criar uma nova migration

Para o **UserService**:
```bash
dotnet ef migrations add NomeDaMigration --project .\BandHub.UserService\BandHub.UserService.csproj --startup-project .\BandHub.UserService\BandHub.UserService.csproj
```

Para o **BandService**:
```bash
dotnet ef migrations add NomeDaMigration --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj
```

> **Exemplo prático:** Imagine que você adicionou uma nova propriedade `Phone` na classe `Account`:
> ```bash
> dotnet ef migrations add AddPhoneToAccount --project .\BandHub.UserService\BandHub.UserService.csproj --startup-project .\BandHub.UserService\BandHub.UserService.csproj
> ```
> Isso vai gerar um arquivo em `Migrations/` com as instruções de `Up()` e `Down()`.

#### 📌 Aplicar migrations no banco de dados

Para o **UserService**:
```bash
dotnet ef database update --project .\BandHub.UserService\BandHub.UserService.csproj --startup-project .\BandHub.UserService\BandHub.UserService.csproj
```

Para o **BandService**:
```bash
dotnet ef database update --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj
```

#### 📌 Remover a última migration (se ainda não foi aplicada)

Para o **UserService**:
```bash
dotnet ef migrations remove --project .\BandHub.UserService\BandHub.UserService.csproj --startup-project .\BandHub.UserService\BandHub.UserService.csproj
```

Para o **BandService**:
```bash
dotnet ef migrations remove --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj
```

#### 📌 Listar todas as migrations

```bash
dotnet ef migrations list --project .\BandHub.UserService\BandHub.UserService.csproj --startup-project .\BandHub.UserService\BandHub.UserService.csproj
```

#### 📌 Gerar script SQL (para ambientes de produção)

```bash
dotnet ef migrations script --project .\BandHub.UserService\BandHub.UserService.csproj --startup-project .\BandHub.UserService\BandHub.UserService.csproj -o script.sql
```

### Fluxo completo de uma migration

```
1. Altere a entidade (Domain)
       ↓
2. Crie a migration
   $ dotnet ef migrations add AlteracaoFeita \
       --project .\BandHub.XxxService\BandHub.XxxService.csproj \
       --startup-project .\BandHub.XxxService\BandHub.XxxService.csproj
       ↓
3. Revise o arquivo gerado em Migrations/
       ↓
4. Aplique no banco
   $ dotnet ef database update \
       --project .\BandHub.XxxService\BandHub.XxxService.csproj \
       --startup-project .\BandHub.XxxService\BandHub.XxxService.csproj
       ↓
5. Teste a aplicação ✅
```

---

## 🚀 Executando a Aplicação

### Executar um serviço individualmente

```bash
# UserService (porta 5293)
dotnet run --project BandHub.UserService

# BandService (porta 5081)
dotnet run --project BandHub.BandService
```

### Acessar o Swagger

Após iniciar um serviço, acesse a documentação interativa:

| Serviço | URL |
|---------|-----|
| UserService | http://localhost:5293/swagger |
| BandService | http://localhost:5081/swagger |

### Build da solution completa

```bash
dotnet build
```

---

## 🧪 Testes

O projeto utiliza **xUnit** como framework de testes, **Moq** para mocking e **FluentAssertions** para assertions expressivas.

### Executar todos os testes

```bash
dotnet test
```

### Executar testes de um projeto específico

```bash
dotnet test tests/BandHub.UserService.UnitTests
```

### Executar com output detalhado

```bash
dotnet test --verbosity detailed
```

### Executar com cobertura de código

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Estrutura dos testes

Os testes seguem a mesma organização de pastas do projeto principal:

```
tests/
├── BandHub.UserService.UnitTests/
│   └── Features/Accounts/
│       ├── CreateAccount/
│       │   ├── CreateAccountHandlerTests.cs   → Testa lógica de registro
│       │   └── CreateAccountValidatorTests.cs → Testa validações de entrada
│       └── GetAccounts/
│           └── GetAccountsHandlerTests.cs     → Testa busca por email
│
└── BandHub.BandService.UnitTests/
    └── Features/Bands/
        ├── CreateBand/
        │   ├── CreateBandHandlerTests.cs       → Testa lógica de criação
        │   └── CreateBandValidatorTests.cs     → Testa validações de entrada
        └── GetBands/
            └── GetBandsHandlerTests.cs         → Testa listagem de bandas
```

### Padrão dos testes

Todos os testes seguem o padrão **AAA (Arrange-Act-Assert)**:

```csharp
[Fact]
public async Task HandleAsync_ShouldCreateAccount_WhenRequestIsValid()
{
    // Arrange - preparar dados e mocks
    var request = new RegisterAccountRequest("John", "john@example.com", "password123", AccountType.User);

    // Act - executar a ação
    var response = await _handler.HandleAsync(request, CancellationToken.None);

    // Assert - verificar o resultado
    response.Name.Should().Be("John");
}
```

---

## 📡 Endpoints da API

### UserService (Accounts)

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/accounts/register` | Registrar uma nova conta |
| `POST` | `/accounts/login` | Autenticar uma conta |
| `GET` | `/accounts/getaccountbyemail?email=` | Buscar conta por email |

#### `POST /accounts/register`

**Request:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "password123",
  "accountType": 1
}
```

> `accountType`: `1` = User, `2` = Band

**Response (201):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "John Doe",
  "email": "john@example.com",
  "accountType": "User",
  "createdAt": "2026-03-07T15:30:00Z"
}
```

#### `POST /accounts/login`

**Request:**
```json
{
  "email": "john@example.com",
  "password": "password123"
}
```

**Response (200):**
```json
{
  "accountId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "John Doe",
  "email": "john@example.com",
  "accountType": "User"
}
```

#### `GET /accounts/getaccountbyemail?email=john@example.com`

**Response (200):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "John Doe",
  "email": "john@example.com",
  "accountType": 1,
  "createdAt": "2026-03-07T15:30:00Z"
}
```

### BandService

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/bands` | Criar uma nova banda (vinculada a uma conta) |
| `GET` | `/bands` | Listar todas as bandas |

#### `POST /bands`

**Request:**
```json
{
  "accountId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Arctic Monkeys",
  "description": "Banda inglesa de indie rock",
  "genre": "Indie Rock",
  "spotifyId": "7Ln80lUS6He07XvHI8qqHH"
}
```

**Response (201):**
```json
{
  "id": "660e8400-e29b-41d4-a716-446655440000",
  "accountId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Arctic Monkeys",
  "genre": "Indie Rock",
  "description": "Banda inglesa de indie rock",
  "spotifyId": "7Ln80lUS6He07XvHI8qqHH",
  "createdAt": "2026-03-07T15:30:00Z"
}
```

---

## 🤝 Contribuindo

1. Crie uma branch a partir da `main`:
   ```bash
   git checkout -b feature/minha-feature
   ```

2. Faça suas alterações seguindo a **Vertical Slice Architecture**

3. Escreva testes unitários para sua feature

4. Execute os testes e garanta que todos passam:
   ```bash
   dotnet test
   ```

5. Faça o commit e abra um Pull Request

---

<p align="center">
  Feito com ❤️ pelo time <strong>BandHub</strong>
</p>
