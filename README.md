# Management
# Management Gateway

## Visão Geral
O projeto **Management.Gateway** é uma solução em ASP.NET Core **.NET 9** que atua como um gateway de APIs com registro detalhado de requisições e respostas em MongoDB. A API principal configura injeções de dependência para MongoDB, serviço de logging e um *middleware* que intercepta todas as requisições para gravação dos logs.

## Estrutura do Repositório
```
Gateway/
└── Management.Gateway
    ├── Gateway.Api             # Web API com middleware de logging
    ├── Gateway.Application     # Camada de aplicação (serviços)
    ├── Gateway.Infrastructure  # Persistência e configurações
    ├── Gateway.SharedKernel    # Código compartilhado
    ├── Gatway.Domain           # (placeholder) camada de domínio
    ├── Gateway.UnitTests       # Testes unitários (exemplo)
    └── Gateway.IntegrationTests# Testes de integração (exemplo)
```

## Pré-requisitos

| Ferramenta       | Versão recomendada |
|------------------|--------------------|
| .NET SDK         | 9.0+               |
| Docker & Compose | 20.x+ / v2.17+     |
| Git              | 2.4+               |

## Configuração da Infraestrutura (Docker Compose)

O arquivo `docker-compose.yml` na raiz sobe os serviços de suporte:

| Serviço        | Porta(s) expostas | Observações |
|----------------|-------------------|-------------|
| **SQL Server** | 1433              | Autenticação `sa` (defina `MSSQL_SA_PASSWORD`). |
| **Redis**      | 6379              | Cache/distribuição. |
| **MinIO**      | 9000, 9001        | Storage S3-like (`MINIO_ROOT_USER`/`MINIO_ROOT_PASSWORD`). |
| **MongoDB**    | 27017             | Base para logs (`MONGO_INITDB_ROOT_USERNAME`/`MONGO_INITDB_ROOT_PASSWORD`). |
| **Mongo Express** | 8081          | UI do Mongo (`MONGO_UI_INITDB_ROOT_USERNAME`/`MONGO_UI_INITDB_ROOT_PASSWORD`). |
| **RabbitMQ**   | 5672, 15672       | Fila de mensagens (`RABBITMQ_DEFAULT_USER`/`RABBITMQ_DEFAULT_PASS`). |

1. Copie `.env.example` para `.env` e ajuste os valores conforme necessidade:
   ```bash
   cp .env.example .env
   ```
2. No diretório raiz (`/workspace/management`), execute:
   ```bash
   docker compose up -d
   ```
3. Aguarde todos os contêineres ficarem saudáveis.

## Configuração do Gateway

1. Acesse a pasta do projeto Web API:
   ```bash
   cd Gateway/Management.Gateway/Gateway.Api
   ```
2. Restaure dependências (opcional):
   ```bash
   dotnet restore
   ```
3. Execute a aplicação:
   ```bash
   dotnet run
   ```
4. A API sobe (por padrão) em `https://localhost:5001` (ou porta configurada). Todas as requisições e respostas são logadas em MongoDB conforme a configuração em `appsettings.json`.

## Endpoints de Exemplo

O controlador `FileController` oferece endpoints de demonstração:

| Verbo | Rota        | Descrição                    |
|-------|-------------|------------------------------|
| GET   | `/api/file` | Retorna "Finishing process." |
| POST  | `/api/file` | Retorna "Finishing process." |

## Serviços e Camadas Internas

- **RequestResponseLoggerMiddleware**: intercepta request/response e registra logs.
- **LoggingService**: camada de aplicação responsável por persistir os logs.
- **MongoLogRepository**: repositório que grava `LogEntry` na coleção `Gateway` do MongoDB.

## Execução de Testes

Os projetos `Gateway.UnitTests` e `Gateway.IntegrationTests` servem como ponto de partida para ampliar a cobertura. Para executá-los:

```bash
dotnet test Gateway/Management.Gateway/Management.Gateway.sln
```

## Passo a Passo Resumido

1. **Clonar o repositório**
   ```bash
   git clone <url> && cd management
   ```
2. **Subir a infraestrutura**
   ```bash
   docker compose up -d
   ```
3. **Executar o Gateway**
   ```bash
   cd Gateway/Management.Gateway/Gateway.Api
   dotnet run
   ```
4. **Consumir a API**
   - Acesse `https://localhost:5001/api/file`
   - Verifique logs no Mongo (`Logs` / `Gateway`).

## Próximos Passos

- Adicionar autenticação/autorização.
- Expandir regras de logging (ex.: mascarar dados sensíveis).
- Criar testes reais para cobrir o pipeline do gateway.
- Configurar reverse proxy completo via YARP.
