# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SAVi.slnx ./
COPY src/SAVi.Domain/SAVi.Domain.csproj src/SAVi.Domain/
COPY src/SAVi.Application/SAVi.Application.csproj src/SAVi.Application/
COPY src/SAVi.Infrastructure/SAVi.Infrastructure.csproj src/SAVi.Infrastructure/
COPY src/SAVi.Api/SAVi.Api.csproj src/SAVi.Api/
RUN dotnet restore src/SAVi.Api/SAVi.Api.csproj

COPY src/ src/
RUN dotnet publish src/SAVi.Api/SAVi.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# O Cloud Run injeta a variável PORT em tempo de execução (padrão 8080).
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet SAVi.Api.dll"]
