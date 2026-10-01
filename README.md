# SAVi — Backend

API do SAVi, app de controle financeiro pessoal. O frontend (PWA em Vue 3) fica em um repositório separado: [SAVI-Front](https://github.com/wesley-beluca/SAVI-Front).

## Stack

- **.NET 10** + **ASP.NET Core** (Web API)
- **Clean Architecture**: Domain, Application, Infrastructure, Api
- **MediatR** (CQRS) + **FluentValidation**
- **Entity Framework Core** + **PostgreSQL** (Npgsql)
- **ASP.NET Core Identity** + **JWT**, com login por e-mail/senha ou Google (ID token validado com `Google.Apis.Auth`)
- **Swagger** para documentação da API

## Estrutura

```
src/
  SAVi.Domain/          entidades e regras de domínio
  SAVi.Application/     casos de uso (commands/queries MediatR), validators, interfaces
  SAVi.Infrastructure/  EF Core, migrations, Identity, JWT, Google
  SAVi.Api/             controllers, middleware, Program.cs
tests/
  SAVi.Domain.Tests/
  SAVi.Application.Tests/
  SAVi.Api.IntegrationTests/
docker-compose.yml      Postgres local para desenvolvimento
Dockerfile              imagem de produção (Cloud Run)
```

## Configuração e segredos

**Nenhuma credencial fica no repositório.** Os `appsettings*.json` versionados só têm valores não sensíveis. Os segredos ficam:

- **em desenvolvimento:** no [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) do .NET, salvo no seu perfil de usuário, fora da pasta do projeto;
- **em produção:** no Google Secret Manager, injetado como variável de ambiente pelo Cloud Run.

| Chave | Obrigatória | Descrição |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | Sim | Connection string do PostgreSQL |
| `Jwt:Secret` | Sim | Chave de assinatura dos tokens JWT (mínimo de 32 caracteres, aleatória) |
| `Google:ClientId` | Só para login com Google | Client ID OAuth do Google Cloud Console |

Configure uma vez, a partir da raiz do repositório:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=savi;Username=savi;Password=<sua-senha>" --project src/SAVi.Api
dotnet user-secrets set "Jwt:Secret" "<chave-aleatoria-com-32+-caracteres>" --project src/SAVi.Api
dotnet user-secrets set "Google:ClientId" "<client-id>.apps.googleusercontent.com" --project src/SAVi.Api
```

Para gerar uma chave JWT aleatória (PowerShell 7+):

```powershell
[Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(48))
```

Para conferir o que está configurado: `dotnet user-secrets list --project src/SAVi.Api`.

> Nunca coloque credenciais em `appsettings*.json`, no `docker-compose.yml` ou em qualquer arquivo versionado. Arquivos `.env` e `appsettings.*.local.json` são ignorados pelo git.

## Rodando localmente

Pré-requisito: **.NET 10 SDK**.

### 1. Banco de dados

Escolha uma das opções:

**Postgres local via Docker**

```powershell
cp .env.example .env   # defina POSTGRES_PASSWORD
docker compose up -d
```

Use a mesma senha do `.env` na connection string dos user-secrets.

**Neon (Postgres na nuvem, plano gratuito)**

Crie um branch de dev no [Neon](https://neon.tech) e coloque a connection string nos user-secrets. Use o endpoint **direto** (sem `-pooler` no host). O pooler roda PgBouncer em modo transação, que quebra o protocolo do Npgsql em alguns comandos.

### 2. Migrations e API

```powershell
dotnet tool restore
dotnet tool run dotnet-ef database update --project src/SAVi.Infrastructure --startup-project src/SAVi.Api
dotnet run --project src/SAVi.Api
```

A API sobe em `http://localhost:5285`:

- Swagger: `http://localhost:5285/swagger`
- Health check: `http://localhost:5285/health`

O CORS libera `http://localhost:5173` (frontend em dev) por padrão. Isso é configurado em `FrontendOrigins` no `appsettings.Development.json`.

### Criando uma migration

```powershell
dotnet tool run dotnet-ef migrations add <NomeDaMigration> --project src/SAVi.Infrastructure --startup-project src/SAVi.Api
```

## Testes

```powershell
dotnet test
```

Os testes de integração usam configurações fixas de teste e não precisam de user-secrets nem de banco.

## Publicação

| Camada | Serviço | Observação |
|---|---|---|
| API | Google Cloud Run | Exige conta de billing no GCP com cartão (sem cobrança dentro do tier sempre-gratuito). |
| Banco | [Neon](https://neon.tech) | Plano gratuito permanente, sem cartão. |

### Workflows

- [`.github/workflows/backend.yml`](.github/workflows/backend.yml): build e testes em todo push/PR. Em push na `main`, também publica a imagem no Artifact Registry e faz deploy no Cloud Run.
- [`.github/workflows/backend-migrate.yml`](.github/workflows/backend-migrate.yml): aplica as migrations no banco de produção. É manual (`Actions → Backend - Aplicar migrations (manual) → Run workflow`); rode sempre que houver migration nova.

### Segredos de produção

No GitHub (`Settings → Secrets and variables → Actions`):

- **Secrets:** `GCP_SA_KEY`, `GCP_PROJECT_ID`, `GCP_REGION`, `PROD_DATABASE_URL`
- **Variables:** `FRONTEND_URL` (URL do Cloudflare Pages, usada no CORS)

No **Google Secret Manager** do projeto GCP (lidos pelo Cloud Run no deploy):

- `savi-db-connection-string`
- `savi-jwt-secret`
- `savi-google-client-id`

## Licenciamento

O **MediatR** é dual-licenciado (RPL 1.5 / comercial) a partir da v13. Ele é gratuito para indivíduos e empresas com receita anual abaixo de US$ 5M, o que cobre este projeto. Veja o [anúncio oficial](https://www.jimmybogard.com/automapper-and-mediatr-going-commercial/).
