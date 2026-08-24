# ProductSalesAPI

A modern Product Sales Analytics solution built using **ASP.NET Core 8**, **Clean Architecture**, **CQRS**, **FluentValidation**

The application provides sales reporting, product analytics, revenue insights, and dashboard metrics through a RESTful API.

---

## 🚀 Features

### API Features

- ASP.NET Core 8 Web API
- Clean Architecture
- CQRS Pattern
- Repository Pattern
- FluentValidation
- Global Exception Handling
- Dependency Injection
- Swagger/OpenAPI
- Pagination Support
- Sales Analytics
- Revenue Reporting
- Unit Testing

## 🏗 Architecture

The solution follows Clean Architecture principles and separates responsibilities across distinct layers.

```
ProductSalesEnterprise.sln

├── ProductSales.Api
│   ├── Controllers
│   ├── Middleware
│   └── Configuration
│
├── ProductSales.Application
│   ├── Contracts
│   ├── Services
│   ├── Validation
│
├── ProductSales.Domain
│   └── Entities
│
├── ProductSales.Infrastructure
│   ├── Repositories
│   ├── Persistence
│   └── External APIs
│
└── ProductSales.UnitTests
```

---

## 🛠 Technology Stack

### Backend

- .NET 8
- ASP.NET Core Web API
- C#
- SQL Server
- Entity Framework Core
- FluentValidation
- Swagger
- xUnit

## 📋 Design Principles

- SOLID Principles
- Clean Architecture
- Separation of Concerns
- Dependency Injection
- Testability
- Maintainability
- Scalability

---

## ⚙️ Getting Started

### Prerequisites

Install:

- .NET 8 SDK
- SQL Server
- Visual Studio 2022
### Clone Repository

```bash
git clone https://github.com/Nqosh/ProductSalesAPI.git
```

### Restore Packages

```bash
dotnet restore
```

### Build Solution

```bash
dotnet build
```

### Run API

```bash
dotnet run --project ProductSales.Api
```

Swagger will be available at:
https://localhost:5050/swagger

<img width="1881" height="861" alt="image" src="https://github.com/user-attachments/assets/4db24d9d-d0d8-4932-b4af-fecb1a4c5f90" />


---

## 📊 Sample Endpoints

### Dashboard Summary

```http
GET /api/dashboard
```
---

## 🧪 Running Tests

```bash
dotnet test
```
---

## 👨‍💻 Author

### Nqobile Moyo
