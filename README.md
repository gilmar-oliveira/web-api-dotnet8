# Web API .NET 8 - Exemplo Completo

Uma API RESTful completa construída com .NET 8, demonstrando as melhores práticas de desenvolvimento.

![Swagger UI - Web API](https://raw.githubusercontent.com/gilmar-oliveira/web-api-dotnet8/main/docs/swagger_screenshot.png)

> 📸 Interface Swagger com todos os endpoints da API documentados e testáveis

## 🚀 Características

- **✅ .NET 8** - Framework mais recente
- **✅ Code First** - Entity Framework Core com migrações automáticas
- **✅ Repository Pattern** - Separação de responsabilidades e testabilidade
- **✅ Multi-Banco de Dados** - Suporte para MySQL, PostgreSQL e SQL Server
- **✅ RESTful API** - Endpoints seguindo princípios REST
- **✅ Swagger/OpenAPI** - Documentação interativa da API
- **✅ Logging Estruturado** - Logs detalhados no console
- **✅ Dependency Injection** - Injeção de dependências nativa do .NET
- **✅ DTOs e Validação** - Data Transfer Objects com validação de dados
- **✅ Arquitetura em Camadas** - Domain, Infrastructure e API

## 📁 Estrutura do Projeto

```
WebApi/
├── WebApi.Domain/              # Camada de domínio
│   ├── Entities/               # Entidades do domínio
│   │   ├── BaseEntity.cs
│   │   ├── Product.cs
│   │   └── Category.cs
│   └── Interfaces/             # Interfaces dos repositórios
│       ├── IRepository.cs
│       ├── IProductRepository.cs
│       └── ICategoryRepository.cs
│
├── WebApi.Infrastructure/      # Camada de infraestrutura
│   ├── Data/                   # Contexto do banco de dados
│   │   └── ApplicationDbContext.cs
│   └── Repositories/           # Implementações dos repositórios
│       ├── Repository.cs
│       ├── ProductRepository.cs
│       └── CategoryRepository.cs
│
└── WebApi/                     # Camada de apresentação (API)
    ├── Controllers/            # Controllers REST
    │   ├── ProductsController.cs
    │   └── CategoriesController.cs
    ├── DTOs/                   # Data Transfer Objects
    │   ├── ProductDto.cs
    │   ├── CreateProductDto.cs
    │   ├── UpdateProductDto.cs
    │   ├── CategoryDto.cs
    │   ├── CreateCategoryDto.cs
    │   └── UpdateCategoryDto.cs
    ├── Program.cs              # Configuração da aplicação
    ├── appsettings.json        # Configurações
    └── WebApi.csproj
```

## 🗄️ Banco de Dados Suportados

### SQL Server (Padrão)
```json
"DatabaseProvider": "SqlServer",
"ConnectionStrings": {
  "SqlServer": "Server=localhost;Database=WebApiDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
}
```

### MySQL
```json
"DatabaseProvider": "MySQL",
"ConnectionStrings": {
  "MySQL": "Server=localhost;Port=3306;Database=WebApiDb;Uid=root;Pwd=YourPassword;"
}
```

### PostgreSQL
```json
"DatabaseProvider": "PostgreSQL",
"ConnectionStrings": {
  "PostgreSQL": "Host=localhost;Port=5432;Database=WebApiDb;Username=postgres;Password=YourPassword;"
}
```

## 🛠️ Como Executar

### Pré-requisitos
- .NET 8 SDK
- Um dos bancos de dados: SQL Server, MySQL ou PostgreSQL

### Passo 1: Restaurar dependências
```bash
dotnet restore
```

### Passo 2: Configurar o banco de dados
Edite o arquivo `WebApi/appsettings.json`:
- Altere `DatabaseProvider` para o banco desejado: `SqlServer`, `MySQL` ou `PostgreSQL`
- Configure a connection string correspondente

### Passo 3: Criar migrações (primeira vez)
```bash
cd WebApi
dotnet ef migrations add InitialCreate --project ../WebApi.Infrastructure --startup-project .
```

### Passo 4: Executar a aplicação
```bash
dotnet run --project WebApi
```

A API estará disponível em:
- **Swagger UI**: https://localhost:5001
- **HTTP**: http://localhost:5000

## 📚 Endpoints da API

### Products

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/products` | Lista todos os produtos |
| GET | `/api/products/{id}` | Obtém produto por ID |
| GET | `/api/products/category/{categoryId}` | Lista produtos por categoria |
| GET | `/api/products/active` | Lista produtos ativos |
| GET | `/api/products/price-range?minPrice={min}&maxPrice={max}` | Produtos por faixa de preço |
| POST | `/api/products` | Cria novo produto |
| PUT | `/api/products/{id}` | Atualiza produto |
| DELETE | `/api/products/{id}` | Remove produto |

### Categories

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/categories` | Lista todas as categorias |
| GET | `/api/categories/{id}` | Obtém categoria por ID |
| POST | `/api/categories` | Cria nova categoria |
| PUT | `/api/categories/{id}` | Atualiza categoria |
| DELETE | `/api/categories/{id}` | Remove categoria |

## 💡 Exemplos de Requisições

### Criar Produto
```json
POST /api/products
{
  "name": "Notebook Dell",
  "description": "Notebook profissional com 16GB RAM",
  "price": 3500.00,
  "stock": 10,
  "isActive": true,
  "categoryId": 1
}
```

### Criar Categoria
```json
POST /api/categories
{
  "name": "Informática",
  "description": "Produtos de informática e tecnologia"
}
```

### Atualizar Produto
```json
PUT /api/products/1
{
  "name": "Notebook Dell XPS",
  "description": "Notebook premium com 32GB RAM",
  "price": 4500.00,
  "stock": 5,
  "isActive": true,
  "categoryId": 1
}
```

## 🔍 Features Implementadas

### Repository Pattern
- Repositório genérico com operações CRUD
- Repositórios específicos para Product e Category
- Separação entre interface e implementação

### Code First
- Entidades com relacionamentos
- Migrações automáticas
- Seed de dados inicial
- Configurações fluentes no DbContext

### Logging
- Logs estruturados no console
- Diferentes níveis de log (Information, Warning, Error)
- Logs de todas as operações CRUD
- Logs de conexão com banco de dados

### Swagger
- Documentação interativa completa
- Descrições detalhadas dos endpoints
- Exemplos de requisições e respostas
- Códigos de status HTTP documentados

### Validação
- Data Annotations nos DTOs
- Validação automática pelo ModelState
- Mensagens de erro personalizadas
- Retorno de erros estruturados

## 🎯 Princípios Aplicados

- **SOLID**: Single Responsibility, Dependency Inversion
- **DRY**: Código reutilizável (Repository genérico)
- **Clean Architecture**: Separação de camadas
- **RESTful**: Recursos, verbos HTTP, status codes corretos
- **Dependency Injection**: Desacoplamento e testabilidade

## 🧪 Testando a API

### Usando Swagger
1. Execute a aplicação
2. Acesse https://localhost:5001
3. Teste os endpoints diretamente na interface

### Usando cURL
```bash
# Listar produtos
curl -X GET https://localhost:5001/api/products

# Criar produto
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Test Product","description":"Test","price":100,"stock":10,"isActive":true,"categoryId":1}'
```

## 📝 Notas Importantes

1. **Migrations**: As migrações são aplicadas automaticamente na inicialização
2. **Seed Data**: Dados de exemplo são criados automaticamente
3. **CORS**: Configurado para aceitar requisições de qualquer origem (ajuste em produção)
4. **HTTPS**: A API usa HTTPS por padrão
5. **Retry Policy**: Configurada para reconexão automática ao banco de dados

## 🔐 Segurança (Próximos Passos)

Para uso em produção, considere adicionar:
- Authentication/Authorization (JWT)
- Rate Limiting
- API Versioning
- Health Checks
- Response Caching
- Input Sanitization

## 📖 Recursos Adicionais

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Swagger/OpenAPI](https://swagger.io/)
- [Repository Pattern](https://docs.microsoft.com/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)

## 📄 Licença

Este projeto é um exemplo educacional e pode ser usado livremente.
