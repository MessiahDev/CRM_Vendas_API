# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e solução para o container
COPY *.sln ./
COPY CRM_Vendas_API/*.csproj CRM_Vendas_API/
COPY CRM_Vendas.Infrastructure/*.csproj CRM_Vendas.Infrastructure/
COPY CRM_Vendas.Domain/*.csproj CRM_Vendas.Domain/
COPY CRM_Vendas.Application/*.csproj CRM_Vendas.Application/
COPY CRM_Vendas.Mapping/*.csproj CRM_Vendas.Mapping/

# Restaura as dependências
RUN dotnet restore

# Copia todo o código
COPY . .

# Publica o projeto startup
RUN dotnet publish CRM_Vendas_API/CRM_Vendas_API.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:${PORT:-5000}
EXPOSE 5000

ENTRYPOINT ["dotnet", "CRM_Vendas_API.dll"]