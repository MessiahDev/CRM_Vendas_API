# CRM_Vendas_API
==============

CRM_Vendas_API uma API RESTful desenvolvida em .NET 8, com arquitetura baseada em DDD
(Domain-Driven Design), voltada para a gesto de vendas. A API permite o gerenciamento de
clientes, leads, negociaes e interaes comerciais de forma segura e escalável.

 Tecnologias Utilizadas
-------------------------
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- AutoMapper
- JWT (JSON Web Tokens) para autenticao
- PostgreSQL
- FluentValidation
- Serilog para logging
- Arquitetura em camadas (DDD)
 Estrutura do Projeto
-----------------------
CRM_Vendas_API.sln

CRM.Vendas_API // Camada de apresentação (controllers, middlewares, etc.)

CRM.Vendas.Application // Casos de uso e serviços de aplicação

CRM.Vendas.Domain // Entidades, interfaces e lógica de domínio

CRM.Vendas.Infrastructure // Implementações de repositórios, contexto EF, etc.

CRM.Vendas.Mapping // Mapeamendo das entidades e DTOs

Autenticação
---------------
A autenticao baseada em JWT. Ao realizar login com sucesso, o sistema retorna um token que
deve ser enviado no header das requisies subsequentes.

Authorization: Bearer <token>

 Funcionalidades
------------------
- Cadastro, edição, listagem e remoção de:
 - Clientes
 - Leads
 - Negociações (Deals)
 - Interaes (Calls, Meetings, Emails)
- Funil de vendas por etapa
- Histórico de interações com cada cliente ou lead
- Controle de acesso por usuário autenticado
- Validaes com mensagens claras de erro
- Logs centralizados com Serilog
 Instalação e Execução
------------------------
Pré-requisitos
- .NET 8 SDK
- PostgreSQL

Configuração do Banco de Dados

Crie um banco no PostgreSQL e atualize a ConnectionStrings no arquivo

appsettings.Development.json

    "ConnectionStrings": {
    
       "DefaultConnection": "Host=localhost;Database=CRM_Vendas;Username=postgres;Password=suasenha"
    }

Execuo do Projeto

# Restaure os pacotes

dotnet restore

# Aplique as migrações (caso ja existam)

dotnet ef database update --project CRM.Vendas.Infrastructure

# Rode a aplicação

dotnet run --project CRM.Vendas.API

A API estará disponível em uma URL parecida com: https://localhost:5001 ou http://localhost:5000

 Endpoints Principais
 
-----------------------
| Recurso | Mtodo | Endpoint | Descrio |
|---------------|--------|---------------------|--------------------------|
| Auth | POST | /api/auth/login | Login e gerao de token |
| Clientes | GET | /api/customers | Listar clientes |
| Leads | GET | /api/leads | Listar leads |
| Negocio | GET | /api/deals | Listar negociaes |
| Interacoes | POST | /api/interactions | Cadastrar nova interação |
Documentao completa em breve com Swagger ou Postman Collection.
 
---------------
Alex Alle

Linkedin.com/in/alex-alle
