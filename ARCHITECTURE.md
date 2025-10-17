# 🏗️ Arquitetura da Web API .NET 8

## 📐 Visão Geral da Arquitetura

Este projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**, dividido em 3 camadas principais:

```
┌─────────────────────────────────────────────────┐
│             WebApi (Presentation)               │
│  Controllers, DTOs, Configuration, Swagger      │
└────────────────┬────────────────────────────────┘
                 │ Depends on
┌────────────────▼────────────────────────────────┐
│        WebApi.Infrastructure (Data)             │
│  DbContext, Repositories, Migrations            │
└────────────────┬────────────────────────────────┘
                 │ Depends on
┌────────────────▼────────────────────────────────┐
│          WebApi.Domain (Core)                   │
│       Entities, Interfaces, Business Logic      │
└─────────────────────────────────────────────────┘
```

## 📦 Camadas do Projeto

### 1. WebApi.Domain (Camada de Domínio)
**Responsabilidade**: Define as regras de negócio e entidades do domínio.

**Componentes**:
- `Entities/`: Classes de entidades (Product, Category, BaseEntity)
- `Interfaces/`: Contratos dos repositórios (IRepository, IProductRepository, ICategoryRepository)

**Características**:
- ✅ Sem dependências externas
- ✅ Regras de negócio puras
- ✅ Entidades com relacionamentos
- ✅ Interfaces para inversão de dependência

### 2. WebApi.Infrastructure (Camada de Infraestrutura)
**Responsabilidade**: Implementa a persistência de dados e acesso ao banco.

**Componentes**:
- `Data/ApplicationDbContext.cs`: Contexto do Entity Framework Core
- `Repositories/`: Implementações concretas dos repositórios

**Características**:
- ✅ Entity Framework Core
- ✅ Code First com Migrations
- ✅ Suporte multi-database
- ✅ Repository Pattern implementado
- ✅ Logging integrado

**Dependências**:
- WebApi.Domain
- Microsoft.EntityFrameworkCore
- Providers específicos (MySQL, PostgreSQL, SQL Server)

### 3. WebApi (Camada de Apresentação)
**Responsabilidade**: Expõe a API REST e gerencia requisições HTTP.

**Componentes**:
- `Controllers/`: Endpoints REST (ProductsController, CategoriesController)
- `DTOs/`: Data Transfer Objects para entrada/saída
- `Program.cs`: Configuração e startup
- `appsettings.json`: Configurações da aplicação

**Características**:
- ✅ RESTful API
- ✅ Swagger/OpenAPI
- ✅ Dependency Injection
- ✅ Validação de dados
- ✅ Logging estruturado
- ✅ CORS configurado

**Dependências**:
- WebApi.Domain
- WebApi.Infrastructure
- Swashbuckle.AspNetCore

## 🔄 Fluxo de Requisição

```
1. Cliente HTTP
   ↓
2. Controller (WebApi)
   ↓
3. Validação de DTOs
   ↓
4. Repository Interface (Domain)
   ↓
5. Repository Implementation (Infrastructure)
   ↓
6. Entity Framework Core
   ↓
7. Banco de Dados (MySQL/PostgreSQL/SQL Server)
```

## 🎯 Padrões Implementados

### Repository Pattern
```csharp
// Interface (Domain)
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync();
}

// Implementation (Infrastructure)
public class ProductRepository : Repository<Product>, IProductRepository
{
    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _dbSet.Where(p => p.IsActive).ToListAsync();
    }
}

// Usage (API)
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    
    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }
}
```

### Dependency Injection
```csharp
// Program.cs
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
```

### DTOs (Data Transfer Objects)
```csharp
// Input DTO
public class CreateProductDto
{
    [Required]
    public string Name { get; set; }
    
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
}

// Output DTO
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }
}
```

## 🗄️ Modelo de Dados

```
┌──────────────┐         ┌──────────────┐
│  Category    │────1:N──│   Product    │
├──────────────┤         ├──────────────┤
│ Id (PK)      │         │ Id (PK)      │
│ Name         │         │ Name         │
│ Description  │         │ Description  │
│ CreatedAt    │         │ Price        │
│ UpdatedAt    │         │ Stock        │
└──────────────┘         │ IsActive     │
                         │ CategoryId(FK)│
                         │ CreatedAt    │
                         │ UpdatedAt    │
                         └──────────────┘
```

## 🔌 Configuração Multi-Database

O projeto suporta 3 bancos de dados através de configuração:

```csharp
// Program.cs - Seleção dinâmica do provider
var databaseProvider = builder.Configuration["DatabaseProvider"];

switch (databaseProvider)
{
    case "MySQL":
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        break;
    case "PostgreSQL":
        options.UseNpgsql(connectionString);
        break;
    case "SqlServer":
        options.UseSqlServer(connectionString);
        break;
}
```

## 📊 Endpoints RESTful

### Convenções REST Seguidas

| HTTP Method | Endpoint | Action | Status Code |
|------------|----------|--------|-------------|
| GET | `/api/products` | Lista todos | 200 OK |
| GET | `/api/products/{id}` | Obtém um | 200 OK, 404 Not Found |
| POST | `/api/products` | Cria novo | 201 Created, 400 Bad Request |
| PUT | `/api/products/{id}` | Atualiza | 204 No Content, 404 Not Found |
| DELETE | `/api/products/{id}` | Remove | 204 No Content, 404 Not Found |

### Recursos Adicionais
- Filtros: `/api/products/active`, `/api/products/category/{id}`
- Query strings: `/api/products/price-range?minPrice=10&maxPrice=100`

## 📝 Logging

Sistema de logging estruturado em múltiplos níveis:

```csharp
// Repository
_logger.LogInformation("Getting product with ID: {Id}", id);

// Controller
_logger.LogWarning("Product with ID {Id} not found", id);

// Program.cs
logger.LogError(ex, "An error occurred while migrating the database");
```

**Níveis de Log**:
- Information: Operações normais
- Warning: Situações inesperadas
- Error: Erros e exceções

## 🔐 Validação

Validação em múltiplas camadas:

1. **DTOs**: Data Annotations
   ```csharp
   [Required(ErrorMessage = "Name is required")]
   [StringLength(200, MinimumLength = 3)]
   public string Name { get; set; }
   ```

2. **Controller**: ModelState
   ```csharp
   if (!ModelState.IsValid)
       return BadRequest(ModelState);
   ```

3. **Database**: Constraints e validações EF Core
   ```csharp
   entity.Property(e => e.Name)
       .IsRequired()
       .HasMaxLength(200);
   ```

## 🚀 Performance e Escalabilidade

### Otimizações Implementadas

1. **Eager Loading**: Carregamento de relacionamentos
   ```csharp
   return await _dbSet.Include(p => p.Category).ToListAsync();
   ```

2. **Indexes**: Índices para consultas rápidas
   ```csharp
   entity.HasIndex(e => e.CategoryId);
   entity.HasIndex(e => e.IsActive);
   ```

3. **Retry Policy**: Reconexão automática
   ```csharp
   options.EnableRetryOnFailure(
       maxRetryCount: 5,
       maxRetryDelay: TimeSpan.FromSeconds(30));
   ```

4. **Async/Await**: Operações assíncronas
   ```csharp
   public async Task<Product?> GetByIdAsync(int id)
   ```

## 🧪 Testabilidade

A arquitetura facilita testes unitários:

```csharp
// Mock do repository
var mockRepo = new Mock<IProductRepository>();
mockRepo.Setup(r => r.GetByIdAsync(1))
        .ReturnsAsync(new Product { Id = 1, Name = "Test" });

// Testar controller
var controller = new ProductsController(mockRepo.Object, logger);
var result = await controller.GetProduct(1);
```

## 📚 Boas Práticas Aplicadas

✅ **SOLID Principles**
- Single Responsibility: Cada classe tem uma única responsabilidade
- Open/Closed: Extensível através de interfaces
- Liskov Substitution: Repositórios substituíveis
- Interface Segregation: Interfaces específicas
- Dependency Inversion: Depende de abstrações

✅ **DRY (Don't Repeat Yourself)**
- Repository genérico reutilizável
- BaseEntity para propriedades comuns
- DTOs separados por operação

✅ **Separation of Concerns**
- Camadas bem definidas
- Responsabilidades claras
- Baixo acoplamento

✅ **Clean Code**
- Nomes descritivos
- Métodos pequenos e focados
- Comentários XML
- Logs informativos

## 🔜 Próximos Passos (Extensões Possíveis)

1. **Autenticação e Autorização**: JWT, Identity
2. **Caching**: Redis, In-Memory Cache
3. **Rate Limiting**: Proteção contra abuso
4. **API Versioning**: Versionamento de endpoints
5. **Health Checks**: Monitoramento de saúde
6. **Unit of Work**: Transações complexas
7. **CQRS**: Separação de comandos e queries
8. **MediatR**: Pipeline de requisições
9. **AutoMapper**: Mapeamento automático de objetos
10. **FluentValidation**: Validações complexas

---

Esta arquitetura fornece uma base sólida, escalável e de fácil manutenção para aplicações enterprise em .NET 8.
