# Comandos Úteis - Web API .NET 8

## Restaurar Dependências
```powershell
dotnet restore
```

## Compilar o Projeto
```powershell
dotnet build
```

## Executar a Aplicação
```powershell
cd WebApi
dotnet run
```

## Entity Framework Core - Migrações

### Criar nova migração
```powershell
cd WebApi
dotnet ef migrations add NomeDaMigracao --project ../WebApi.Infrastructure --startup-project .
```

### Aplicar migrações ao banco
```powershell
cd WebApi
dotnet ef database update --project ../WebApi.Infrastructure --startup-project .
```

### Remover última migração
```powershell
cd WebApi
dotnet ef migrations remove --project ../WebApi.Infrastructure --startup-project .
```

### Listar migrações
```powershell
cd WebApi
dotnet ef migrations list --project ../WebApi.Infrastructure --startup-project .
```

### Gerar script SQL
```powershell
cd WebApi
dotnet ef migrations script --project ../WebApi.Infrastructure --startup-project . --output migrations.sql
```

## Trocar Banco de Dados

### Para SQL Server
1. Abra `WebApi/appsettings.json`
2. Altere: `"DatabaseProvider": "SqlServer"`
3. Configure a connection string em `ConnectionStrings.SqlServer`

### Para MySQL
1. Abra `WebApi/appsettings.json`
2. Altere: `"DatabaseProvider": "MySQL"`
3. Configure a connection string em `ConnectionStrings.MySQL`

### Para PostgreSQL
1. Abra `WebApi/appsettings.json`
2. Altere: `"DatabaseProvider": "PostgreSQL"`
3. Configure a connection string em `ConnectionStrings.PostgreSQL`

## Docker (Opcional)

### SQL Server
```powershell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

### MySQL
```powershell
docker run --name mysql -e MYSQL_ROOT_PASSWORD=YourPassword -p 3306:3306 -d mysql:8.0
```

### PostgreSQL
```powershell
docker run --name postgres -e POSTGRES_PASSWORD=YourPassword -p 5432:5432 -d postgres:16
```

## Testar Endpoints com PowerShell

### GET - Listar produtos
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/api/products" -Method GET
```

### POST - Criar produto
```powershell
$body = @{
    name = "Produto Teste"
    description = "Descrição do produto"
    price = 99.99
    stock = 10
    isActive = $true
    categoryId = 1
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/products" -Method POST -Body $body -ContentType "application/json"
```

### PUT - Atualizar produto
```powershell
$body = @{
    name = "Produto Atualizado"
    description = "Nova descrição"
    price = 149.99
    stock = 5
    isActive = $true
    categoryId = 1
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/products/1" -Method PUT -Body $body -ContentType "application/json"
```

### DELETE - Remover produto
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/api/products/1" -Method DELETE
```

## Limpar e Recompilar
```powershell
dotnet clean
dotnet build
```

## Publicar para Produção
```powershell
dotnet publish -c Release -o ./publish
```

## Ver logs detalhados
```powershell
cd WebApi
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run
```
