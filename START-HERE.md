# ✨ Web API .NET 8 - Projeto Completo Criado!

## 🎉 Parabéns! Seu projeto está pronto!

Foi criada uma **Web API completa em .NET 8** com todas as funcionalidades solicitadas:

### ✅ Funcionalidades Implementadas

- ✅ **.NET 8** - Framework mais recente da Microsoft
- ✅ **Code First** - Entity Framework Core com migrações automáticas
- ✅ **Swagger** - Documentação interativa e testável da API
- ✅ **Repository Pattern** - Arquitetura limpa e testável
- ✅ **Multi-Banco** - Suporte para MySQL, PostgreSQL e SQL Server
- ✅ **RESTful** - API seguindo princípios REST corretamente
- ✅ **Logging no Console** - Logs estruturados e detalhados

## 📁 Estrutura Criada

```
web-api-dotnet8/
├── WebApi.Domain/              # Entidades e interfaces
├── WebApi.Infrastructure/      # Repositórios e DbContext
├── WebApi/                     # Controllers e API
├── WebApi.sln                  # Solution file
├── README.md                   # Documentação completa
├── QUICKSTART.md              # Guia de início rápido
├── ARCHITECTURE.md            # Detalhes da arquitetura
├── COMMANDS.md                # Comandos úteis
├── docker-compose.yml         # Bancos de dados em Docker
└── .gitignore                 # Arquivo Git
```

## 🚀 Como Começar (3 Passos Simples)

### 1️⃣ Instalar Dependências
```powershell
dotnet restore
```

### 2️⃣ Escolher o Banco de Dados
Edite `WebApi/appsettings.json`:
```json
"DatabaseProvider": "SqlServer"  // ou "MySQL" ou "PostgreSQL"
```

### 3️⃣ Executar
```powershell
cd WebApi
dotnet ef migrations add InitialCreate --project ../WebApi.Infrastructure --startup-project .
dotnet run
```

**Pronto!** Acesse: https://localhost:5001

## 🐳 Opção Fácil: Usar Docker para o Banco

```powershell
# Inicia todos os 3 bancos de dados
docker-compose up -d

# Ou apenas um:
docker-compose up -d sqlserver  # SQL Server
docker-compose up -d mysql      # MySQL
docker-compose up -d postgres   # PostgreSQL
```

## 📚 Exemplos de Uso

### Criar uma Categoria
```powershell
$category = @{
    name = "Eletrônicos"
    description = "Produtos eletrônicos"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/categories" `
    -Method POST -Body $category -ContentType "application/json"
```

### Criar um Produto
```powershell
$product = @{
    name = "Notebook Dell"
    description = "Notebook profissional"
    price = 3500.00
    stock = 10
    isActive = $true
    categoryId = 1
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/products" `
    -Method POST -Body $product -ContentType "application/json"
```

### Listar Todos os Produtos
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/api/products" -Method GET
```

## 🎯 Endpoints Disponíveis

### Products API
- `GET /api/products` - Lista todos os produtos
- `GET /api/products/{id}` - Obtém produto específico
- `GET /api/products/active` - Lista produtos ativos
- `GET /api/products/category/{categoryId}` - Produtos por categoria
- `GET /api/products/price-range?minPrice=X&maxPrice=Y` - Produtos por preço
- `POST /api/products` - Cria novo produto
- `PUT /api/products/{id}` - Atualiza produto
- `DELETE /api/products/{id}` - Remove produto

### Categories API
- `GET /api/categories` - Lista todas as categorias
- `GET /api/categories/{id}` - Obtém categoria específica
- `POST /api/categories` - Cria nova categoria
- `PUT /api/categories/{id}` - Atualiza categoria
- `DELETE /api/categories/{id}` - Remove categoria

## 🔧 Trocar de Banco de Dados

É **muito fácil** trocar de banco! Basta editar `appsettings.json`:

### Para SQL Server (Padrão)
```json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "SqlServer": "Server=localhost;Database=WebApiDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  }
}
```

### Para MySQL
```json
{
  "DatabaseProvider": "MySQL",
  "ConnectionStrings": {
    "MySQL": "Server=localhost;Port=3306;Database=WebApiDb;Uid=root;Pwd=YourPassword;"
  }
}
```

### Para PostgreSQL
```json
{
  "DatabaseProvider": "PostgreSQL",
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=WebApiDb;Username=postgres;Password=YourPassword;"
  }
}
```

## 📖 Arquitetura do Projeto

### Camada de Domínio (`WebApi.Domain`)
- **Entities**: Product, Category, BaseEntity
- **Interfaces**: IRepository, IProductRepository, ICategoryRepository

### Camada de Infraestrutura (`WebApi.Infrastructure`)
- **Data**: ApplicationDbContext (EF Core)
- **Repositories**: Repository, ProductRepository, CategoryRepository

### Camada de API (`WebApi`)
- **Controllers**: ProductsController, CategoriesController
- **DTOs**: CreateProductDto, UpdateProductDto, ProductDto, etc.
- **Configuration**: Program.cs, appsettings.json

## 🎨 Padrões Aplicados

✅ **Repository Pattern** - Abstração de acesso a dados
✅ **Dependency Injection** - Inversão de controle
✅ **DTOs** - Separação entre API e entidades
✅ **SOLID Principles** - Código limpo e manutenível
✅ **RESTful Design** - API seguindo padrões REST
✅ **Code First** - Migrações automáticas do banco
✅ **Logging** - Logs estruturados em todos os níveis

## 📊 Logs no Console

A aplicação exibe logs detalhados:

```
[2024-10-17 10:30:15] info: Program[0]
      Starting application with SqlServer database
[2024-10-17 10:30:16] info: Program[0]
      Applying database migrations...
[2024-10-17 10:30:17] info: Program[0]
      Database migrations applied successfully
[2024-10-17 10:30:18] info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

## 🔍 Testando com Swagger

1. Execute a aplicação: `dotnet run --project WebApi`
2. Abra o navegador em: https://localhost:5001
3. Você verá a interface do Swagger
4. Clique em qualquer endpoint
5. Clique em "Try it out"
6. Preencha os dados (se necessário)
7. Clique em "Execute"
8. Veja a resposta!

## 📝 Dados de Exemplo (Seed)

O banco já vem com dados iniciais:

**Categorias:**
1. Electronics
2. Books
3. Clothing

**Produtos:**
1. Smartphone XYZ - $999.99
2. Laptop Pro - $1499.99
3. Programming C# - $49.99

## 🛠️ Comandos Úteis

### Ver logs detalhados
```powershell
cd WebApi
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run
```

### Criar nova migração
```powershell
cd WebApi
dotnet ef migrations add NomeDaMigracao --project ../WebApi.Infrastructure --startup-project .
```

### Aplicar migrações
```powershell
cd WebApi
dotnet ef database update --project ../WebApi.Infrastructure --startup-project .
```

### Limpar e recompilar
```powershell
dotnet clean
dotnet build
```

## 📚 Documentação Adicional

- **README.md** - Documentação completa do projeto
- **QUICKSTART.md** - Guia de início rápido detalhado
- **ARCHITECTURE.md** - Detalhes técnicos da arquitetura
- **COMMANDS.md** - Lista completa de comandos úteis

## 🎓 Características Educacionais

Este projeto é um **exemplo completo** de:
- Arquitetura em camadas
- Boas práticas .NET
- Padrões de design
- Clean Code
- SOLID principles
- RESTful APIs
- Entity Framework Core
- Dependency Injection
- Logging
- Documentação API

## 💡 Dicas

1. **Sempre comece pelo Swagger** para testar a API
2. **Leia os logs** para entender o que está acontecendo
3. **Experimente trocar de banco** para ver como é fácil
4. **Use o Docker** para não precisar instalar bancos localmente
5. **Explore o código** para aprender os padrões aplicados

## 🚀 Próximos Passos Sugeridos

1. Teste todos os endpoints no Swagger
2. Crie suas próprias entidades
3. Adicione novos repositórios
4. Implemente autenticação JWT
5. Adicione caching
6. Implemente paginação
7. Adicione filtros avançados

## ❓ Precisa de Ajuda?

Consulte os arquivos de documentação:
- Problemas iniciais? → **QUICKSTART.md**
- Dúvidas técnicas? → **ARCHITECTURE.md**
- Comandos específicos? → **COMMANDS.md**
- Visão geral? → **README.md**

---

## 🎉 Está Tudo Pronto!

Seu projeto está **100% funcional** e pronto para uso!

Execute: `dotnet run --project WebApi`

Acesse: **https://localhost:5001**

**Boa codificação! 🚀**
