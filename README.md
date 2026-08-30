# 📚 Library Management API

A production-style **Library Management REST API** built with **C#, .NET 10, ASP.NET Core Web API, Entity Framework Core, and MySQL**.

This project was created as my first practical .NET backend project to transition from a **Java/Spring Boot background into the .NET ecosystem**.

The goal was not just to build CRUD APIs, but to understand how a real ASP.NET Core backend is structured using **Dependency Injection, Service Layer, Repository Pattern, EF Core, asynchronous programming, validation, and global exception handling**.

---

## 🚀 Tech Stack

| Technology | Purpose |
|---|---|
| **C#** | Programming Language |
| **.NET 10** | Application Platform |
| **ASP.NET Core Web API** | REST API Framework |
| **Entity Framework Core** | ORM / Database Access |
| **Pomelo.EntityFrameworkCore.MySql** | MySQL Provider |
| **MySQL** | Relational Database |
| **Swagger / OpenAPI** | API Documentation & Testing |
| **Git / GitHub** | Version Control |
| **VS Code** | Development Environment |

---

# 🏗️ Architecture

The application follows a layered architecture:

```text
                    HTTP Request
                         │
                         ▼
              ┌─────────────────────┐
              │      Controller     │
              └──────────┬──────────┘
                         │
                         ▼
              ┌─────────────────────┐
              │       Service       │
              └──────────┬──────────┘
                         │
                         ▼
              ┌─────────────────────┐
              │     Repository      │
              └──────────┬──────────┘
                         │
                         ▼
              ┌─────────────────────┐
              │    LibraryDbContext │
              └──────────┬──────────┘
                         │
                         ▼
              ┌─────────────────────┐
              │       EF Core       │
              └──────────┬──────────┘
                         │
                         ▼
              ┌─────────────────────┐
              │        MySQL        │
              └─────────────────────┘
