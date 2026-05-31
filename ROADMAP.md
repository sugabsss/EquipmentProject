# EquipmentManager — Roadmap de Aprendizado

> Objetivo: transformar o projeto em uma aplicação profissional cobrindo DDD, Mensageria, Auth, Cache, Testes e React.

---

## Estado Atual

### O que já está bem feito
- Clean Architecture (Domain → Application → Infra → API)
- CQRS com MediatR (Commands / Queries / Handlers)
- Repository Pattern
- Result Pattern (`Result<T>`)
- Paginação (`PagedResult<T>`)
- API Versioning
- React Query + React Hook Form no frontend

### Bugs e problemas imediatos a corrigir

| # | Problema | Arquivo | Detalhe |
|---|---|---|---|
| 1 | `await` faltando | `DeleteEquipmentHandler.cs:25` | Compara `Task` com `null`, nunca entra no `if` |
| 2 | DTO sem `Id` | `EquipmentDto.cs` | Frontend usa `any` para contornar |
| 3 | `ExceptionMiddleware` não registrado | `Program.cs` | Criou mas não chamou `app.UseMiddleware<>()` |
| 4 | Botão Delete sem handler | `ListEquipment.tsx` | Clique não faz nada |
| 5 | Sem validação de inputs | Toda a aplicação | Zod importado mas não usado |
| 6 | Typo no DbSet | `AppDbContext.cs` | `Equipaments` → `Equipments` |

---

## Fase 1 — DDD (Domain-Driven Design)

> O domínio atual é anêmico: só contém dados, sem comportamento nem regras de negócio.

### O que adicionar

```
EquipmentManagement.Domain/
├── Entities/
│   └── Equipment.cs              → Adicionar regras de negócio (ex: ValidarCertificado())
├── ValueObjects/
│   └── NumeroSerie.cs            → Encapsular e validar o número de série
├── Enums/
│   └── StatusEquipamento.cs      → Ativo | Manutenção | Descartado
├── Exceptions/
│   └── DomainException.cs        → Exceções de regras de negócio
└── Events/
    └── EquipamentoCadastradoEvent.cs  → Base para Mensageria (Fase 4)
```

### Conceitos a aprender
- **Entidade rica**: métodos que encapsulam comportamento (ex: `Ativar()`, `EnviarParaManutencao()`)
- **Value Object**: objeto imutável sem identidade própria (ex: `NumeroSerie` que valida o formato)
- **Domain Events**: algo que aconteceu no domínio e outros módulos precisam saber
- **Exceções de domínio**: erros de negócio distintos de erros técnicos

### Exemplo prático

```csharp
// Antes (anêmico)
public class Equipment
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int RefSetor { get; set; }
}

// Depois (rico)
public class Equipment
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public StatusEquipamento Status { get; private set; }

    public void EnviarParaManutencao()
    {
        if (Status == StatusEquipamento.Descartado)
            throw new DomainException("Equipamento descartado não pode ir para manutenção.");

        Status = StatusEquipamento.Manutencao;
        AddDomainEvent(new EquipamentoAtualizadoEvent(Id));
    }
}
```

---

## Fase 2 — Validação (FluentValidation)

> Antes de partir para Auth e Cache, o projeto precisa de validação robusta.

### O que adicionar

```
EquipmentManagement.Application/
├── Validators/
│   ├── InsertEquipmentCommandValidator.cs
│   └── UpdateEquipmentCommandValidator.cs
└── Behaviors/
    └── ValidationBehavior.cs     → Pipeline MediatR: valida antes de chegar no Handler
```

### Pacotes

```
FluentValidation
FluentValidation.DependencyInjectionExtensions
```

### Conceitos a aprender
- **Pipeline Behavior do MediatR**: intercepta a requisição antes/depois do handler
- **FluentValidation**: validação fluente com regras legíveis
- Retornar `Result.Invalid(errors)` quando a validação falhar

---

## Fase 3 — Autenticação e Autorização

> JWT + Refresh Token + OAuth2.0 (Google).

### O que adicionar

```
EquipmentManagement.Domain/
└── Entities/
    └── Usuario.cs                → Entidade de usuário com roles

EquipmentManagement.Application/
├── Commands/Auth/
│   ├── LoginCommand.cs
│   ├── RefreshTokenCommand.cs
│   └── LogoutCommand.cs
└── Handlers/Auth/
    ├── LoginHandler.cs
    ├── RefreshTokenHandler.cs
    └── LogoutHandler.cs

EquipmentManagement.Infra/
└── Repositories/
    └── UsuarioRepository.cs

EquipmentManagement.API/
├── Controllers/
│   └── AuthController.cs         → POST /auth/login, /auth/refresh, /auth/logout
└── Extensions/
    └── AuthExtensions.cs         → Configuração do JWT no Program.cs
```

### Pacotes

```
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.AspNetCore.Authentication.Google
System.IdentityModel.Tokens.Jwt
BCrypt.Net-Next
```

### Fluxo JWT + Refresh Token

```
1. POST /auth/login
   └── Retorna: AccessToken (15 min) + RefreshToken (7 dias, salvo no banco)

2. Requisições autenticadas
   └── Header: Authorization: Bearer <AccessToken>

3. AccessToken expirado (API retorna 401)
   └── Frontend chama POST /auth/refresh com o RefreshToken
   └── API valida RefreshToken, gera novo par de tokens

4. Logout
   └── Invalida o RefreshToken no banco
```

### Fluxo OAuth2.0 (Google)

```
1. Frontend redireciona para /auth/google
2. Google autentica o usuário
3. Callback retorna para /auth/google/callback
4. API cria ou recupera o usuário no banco
5. Gera JWT próprio e retorna ao frontend
```

### Interceptor Axios (Refresh automático)

```typescript
// src/api/api.ts
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      const novoToken = await refreshToken();
      error.config.headers.Authorization = `Bearer ${novoToken}`;
      return api(error.config); // retenta a requisição original
    }
    return Promise.reject(error);
  }
);
```

---

## Fase 4 — Mensageria com RabbitMQ + MassTransit

> Publicar eventos de domínio em filas para processamento assíncrono.

### O que adicionar

```
EquipmentManagement.Messaging/   (novo projeto)
├── Consumers/
│   ├── EquipamentoCadastradoConsumer.cs
│   └── EquipamentoAtualizadoConsumer.cs
├── Events/
│   ├── EquipamentoCadastradoEvent.cs
│   └── EquipamentoAtualizadoEvent.cs
└── Extensions/
    └── MassTransitExtensions.cs
```

### Pacotes

```
MassTransit
MassTransit.RabbitMQ
MassTransit.EntityFrameworkCore    → Outbox pattern (garante entrega)
```

### Casos de uso práticos

| Evento | Consumer | O que faz |
|---|---|---|
| `EquipamentoCadastrado` | `AuditoriaConsumer` | Grava log de auditoria |
| `EquipamentoCadastrado` | `NotificacaoConsumer` | Envia e-mail/notificação |
| `EquipamentoAtualizado` | `CacheInvalidacaoConsumer` | Invalida o cache do Redis |
| `EquipamentoDescartado` | `RelatorioConsumer` | Atualiza relatórios |

### Conceitos a aprender
- **Producer/Consumer**: quem publica e quem consome mensagens
- **Exchange / Queue / Binding** no RabbitMQ
- **Outbox Pattern**: garante que a mensagem seja publicada mesmo se o banco cair
- **Retry / Dead Letter Queue**: reprocessar mensagens que falharam

---

## Fase 5 — Cache com Redis

> Evitar buscas repetidas no banco com cache em memória distribuída.

### O que adicionar

```
EquipmentManagement.Shared/
└── Interfaces/
    └── ICacheService.cs

EquipmentManagement.Infra/
└── Services/
    └── RedisCacheService.cs

EquipmentManagement.Application/
└── Behaviors/
    └── CachingBehavior.cs        → Pipeline MediatR: retorna cache antes do handler
```

### Pacotes

```
StackExchange.Redis
Microsoft.Extensions.Caching.StackExchangeRedis
```

### Como funciona o Pipeline de Cache

```
Query chega no MediatR
    └── CachingBehavior verifica Redis
        ├── Cache HIT  → retorna dado sem bater no banco
        └── Cache MISS → executa Handler → salva no Redis → retorna dado
```

### Estratégia de invalidação

```
Comando (Insert/Update/Delete) executado
    └── Handler publica evento → Consumer invalida chave no Redis
```

### Conceitos a aprender
- **Cache-aside pattern**: aplicação gerencia o cache manualmente
- **TTL (Time To Live)**: tempo de expiração das chaves
- **Invalidação de cache**: quando e como remover dados desatualizados
- **Cache distribuído vs. in-memory**: quando usar cada um

---

## Fase 6 — Testes

> Unitários para lógica de negócio, integração para fluxo completo.

### Estrutura

```
EquipmentManagement.Tests/
├── Unit/
│   ├── Domain/
│   │   └── EquipmentTests.cs              → Testar regras de negócio da entidade
│   ├── Handlers/
│   │   ├── GetEquipmentHandlerTests.cs
│   │   ├── InsertEquipmentHandlerTests.cs
│   │   └── DeleteEquipmentHandlerTests.cs
│   └── Validators/
│       └── InsertEquipmentCommandValidatorTests.cs
└── Integration/
    ├── Fixtures/
    │   └── WebApplicationFactory.cs       → Sobe a API em memória para testes
    ├── Controllers/
    │   └── EquipmentControllerTests.cs    → Testa endpoints HTTP reais
    └── Repositories/
        └── EquipmentRepositoryTests.cs    → Testa SQL real via Testcontainers
```

### Pacotes

```
# Unit Tests
xUnit
Moq
FluentAssertions

# Integration Tests
Microsoft.AspNetCore.Mvc.Testing
Testcontainers.SqlEdge              → Sobe SQL Server em Docker para testes
Testcontainers.RabbitMq             → Sobe RabbitMQ em Docker para testes
```

### O que testar em cada camada

| Camada | O que testar |
|---|---|
| Domain | Regras de negócio, exceções de domínio, value objects |
| Application | Handlers (mock do repositório), validators |
| Infra | Repositórios contra banco real (Testcontainers) |
| API | Endpoints HTTP ponta a ponta (WebApplicationFactory) |

### Exemplo: teste unitário de handler

```csharp
[Fact]
public async Task GetEquipmentByIdHandler_DeveRetornarNotFound_QuandoNaoExistir()
{
    // Arrange
    var repoMock = new Mock<IEquipmentRepository>();
    repoMock.Setup(r => r.GetEquipmentByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((Equipment?)null);

    var handler = new GetEquipmentByIdHandler(repoMock.Object, _mapper);
    var query = new GetEquipmentByIdQuery(Guid.NewGuid());

    // Act
    var result = await handler.Handle(query, default);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
}
```

---

## Fase 7 — React (Refatoração e novas features)

### Componentes a criar

```
React/frontend/src/
├── components/
│   ├── ui/
│   │   ├── Button.tsx
│   │   ├── Input.tsx
│   │   ├── Table.tsx
│   │   ├── Modal.tsx
│   │   └── Skeleton.tsx
│   └── layout/
│       ├── Sidebar.tsx
│       └── Header.tsx
├── hooks/
│   ├── useAuth.ts              → Estado de autenticação
│   └── useEquipment.ts        → Queries e mutations de equipamento
├── context/
│   └── AuthContext.tsx         → Contexto global do usuário logado
└── pages/
    ├── Login.tsx
    └── Equipment/
        ├── ListEquipment.tsx   → Refatorar com Tailwind + componentes
        ├── CreateEquipment.tsx → Adicionar validação com Zod
        └── EditEquipment.tsx   → Idem
```

### Melhorias prioritárias

| # | O que fazer | Por que |
|---|---|---|
| 1 | Adicionar validação Zod nos formulários | Zod está instalado mas não usado |
| 2 | Implementar handler do botão Delete | Funcionalidade básica faltando |
| 3 | Trocar inline styles por Tailwind | Tailwind está instalado mas não usado |
| 4 | Adicionar tela de Login | Necessário para Auth |
| 5 | Interceptor Axios para refresh automático | Experiência de auth transparente |
| 6 | Componentizar tabela e formulários | Evitar repetição entre Create e Edit |
| 7 | Adicionar Error Boundaries | Erros não devem travar a tela toda |
| 8 | Loading skeletons | Melhor UX que "Carregando..." |

---

## Infraestrutura (Docker Compose)

> Quando as fases acima estiverem implementadas, orquestrar tudo com Docker.

```yaml
# docker-compose.yml
services:
  api:
    build: ./EquipmentManagement.API
    ports: ["5039:8080"]
    depends_on: [sqlserver, redis, rabbitmq]

  frontend:
    build: ./React/frontend
    ports: ["5173:80"]

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "SuaSenhaForte123!"
      ACCEPT_EULA: "Y"

  redis:
    image: redis:7-alpine
    ports: ["6379:6379"]

  rabbitmq:
    image: rabbitmq:3-management
    ports: ["5672:5672", "15672:15672"]  # 15672 = painel admin
```

---

## Ordem de execução recomendada

```
[x] Fase 0  — Corrigir bugs imediatos
[ ] Fase 1  — DDD: enriquecer domínio (ValueObjects, Eventos, Exceções)
[ ] Fase 2  — Validação com FluentValidation + Pipeline Behavior
[ ] Fase 3  — JWT + Refresh Token + OAuth2.0
[ ] Fase 4  — Mensageria com RabbitMQ + MassTransit
[ ] Fase 5  — Cache com Redis + Pipeline Behavior
[ ] Fase 6  — Testes unitários e de integração
[ ] Fase 7  — Refatoração React (Tailwind, Zod, componentes, auth)
[ ] Fase 8  — Docker Compose orquestrando tudo
```

---

## Resumo de pacotes NuGet

| Pacote | Fase |
|---|---|
| `FluentValidation` | 2 |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 3 |
| `Microsoft.AspNetCore.Authentication.Google` | 3 |
| `BCrypt.Net-Next` | 3 |
| `MassTransit` | 4 |
| `MassTransit.RabbitMQ` | 4 |
| `MassTransit.EntityFrameworkCore` | 4 |
| `StackExchange.Redis` | 5 |
| `Microsoft.Extensions.Caching.StackExchangeRedis` | 5 |
| `xUnit` | 6 |
| `Moq` | 6 |
| `FluentAssertions` | 6 |
| `Microsoft.AspNetCore.Mvc.Testing` | 6 |
| `Testcontainers.SqlEdge` | 6 |
| `Testcontainers.RabbitMq` | 6 |

## Resumo de pacotes NPM

| Pacote | Fase |
|---|---|
| `@hookform/resolvers` + `zod` (já instalados) | 7 |
| `@tanstack/react-query` (já instalado) | — |
| `axios` (já instalado) | 3 |
