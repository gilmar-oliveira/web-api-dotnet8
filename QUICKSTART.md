# 🚀 Início Rápido - Web API .NET 8

## Opção 1: Iniciar Rapidamente (Recomendado)

### 1. Iniciar o banco de dados com Docker
```powershell
# Iniciar todos os bancos de dados (SQL Server, MySQL e PostgreSQL)
docker-compose up -d

# Ou iniciar apenas um específico:
docker-compose up -d sqlserver    # SQL Server
docker-compose up -d mysql        # MySQL
docker-compose up -d postgres     # PostgreSQL
```

### 2. Restaurar dependências
```powershell
dotnet restore
```

### 3. Configurar o banco de dados
Edite `WebApi/appsettings.json` e escolha o banco:
```json
"DatabaseProvider": "SqlServer"  // ou "MySQL" ou "PostgreSQL"
```

### 4. Criar a primeira migração
```powershell
cd WebApi
dotnet ef migrations add InitialCreate --project ../WebApi.Infrastructure --startup-project .
```

### 5. Executar a aplicação
```powershell
dotnet run --project WebApi
```

### 6. Acessar o Swagger
Abra seu navegador em: **https://localhost:5001**

## Opção 2: Banco de Dados Existente

Se você já tem um banco de dados instalado:

### 1. Configure a connection string em `appsettings.json`

### 2. Escolha o provider correto
```json
"DatabaseProvider": "SqlServer"  // SqlServer, MySQL ou PostgreSQL
```

### 3. Execute os comandos
```powershell
dotnet restore
cd WebApi
dotnet ef migrations add InitialCreate --project ../WebApi.Infrastructure --startup-project .
dotnet run
```

## 🧪 Testando a API

### Via Swagger (Mais Fácil)
1. Acesse https://localhost:5001
2. Explore os endpoints
3. Clique em "Try it out"
4. Execute as requisições

### Via PowerShell
```powershell
# Listar produtos
Invoke-RestMethod -Uri "https://localhost:5001/api/products" -Method GET

# Criar categoria
$category = @{
    name = "Eletrônicos"
    description = "Produtos eletrônicos diversos"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/categories" -Method POST -Body $category -ContentType "application/json"

# Criar produto
$product = @{
    name = "Notebook"
    description = "Notebook de alta performance"
    price = 3500.00
    stock = 10
    isActive = $true
    categoryId = 1
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/products" -Method POST -Body $product -ContentType "application/json"
```

## 📊 Comandos Docker Úteis

### Ver logs dos containers
```powershell
docker-compose logs -f
```

### Parar os bancos de dados
```powershell
docker-compose down
```

### Parar e remover volumes (limpar dados)
```powershell
docker-compose down -v
```

### Verificar status
```powershell
docker-compose ps
```

## 🔍 Verificando Logs da Aplicação

A aplicação exibe logs detalhados no console:
- ✅ Conexão com banco de dados
- ✅ Migrações aplicadas
- ✅ Operações CRUD
- ✅ Requisições HTTP
- ✅ Erros e warnings

## 🎯 Próximos Passos

1. ✅ Testar todos os endpoints no Swagger
2. ✅ Criar novos produtos e categorias
3. ✅ Experimentar filtros (por categoria, preço, etc.)
4. ✅ Testar validações (enviar dados inválidos)
5. ✅ Trocar de banco de dados e testar novamente

## ⚠️ Problemas Comuns

### Erro de conexão com banco de dados
- Verifique se o container Docker está rodando: `docker ps`
- Confirme a connection string no appsettings.json
- Verifique se a porta está disponível

### Erro ao criar migração
- Certifique-se de estar na pasta correta: `cd WebApi`
- Verifique se o projeto Infrastructure existe
- Execute `dotnet restore` primeiro

### Porta já em uso
- Mude a porta em `WebApi/Properties/launchSettings.json`
- Ou mate o processo usando a porta: `Get-Process -Id (Get-NetTCPConnection -LocalPort 5001).OwningProcess | Stop-Process`

## 📚 Documentação Completa

Veja o arquivo `README.md` para documentação completa da arquitetura e funcionalidades.

## 💬 Suporte

Para mais comandos e exemplos, consulte o arquivo `COMMANDS.md`.
