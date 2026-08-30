# 📚 Library Management API

A foundational CRUD backend for managing a library's book catalog, built with **C# / .NET 10 / ASP.NET Core Web API / Entity Framework Core / MySQL (Pomelo provider)**.

This is a smaller, first-principles project compared to a full microservices or auth-heavy backend — its purpose is to get the fundamentals of a clean, layered ASP.NET Core Web API exactly right: controllers that stay thin, services that hold business logic, repositories that isolate persistence, DTOs that guard the API boundary, and centralized error handling that keeps failure responses consistent. Everything more advanced (authentication, transactions, concurrency, event-driven messaging) builds on top of these fundamentals in later projects.

---

## 🧱 What This Project Demonstrates

| Concern | How It's Handled |
|---|---|
| Keeping controllers free of business logic | Controllers only translate HTTP ↔ service calls; all logic lives in `BookService` |
| Keeping persistence swappable/testable | `IBookRepository` abstraction in front of EF Core, injected via DI |
| Never exposing internal entities directly | `Book` (EF entity) is mapped to `BookResponse` before leaving the service layer |
| Validating input before it reaches business logic | Data-annotation validation on `CreateBookRequest` / `UpdateBookRequest` |
| Returning consistent errors instead of raw stack traces | A single `ExceptionHandlingMiddleware` wraps the whole pipeline |
| Schema evolving safely over time | EF Core Code-First migrations instead of hand-written SQL |
| Not hand-rolling REST conventions | Standard verbs/status codes: `200`, `201 Created` with `Location`, `204 No Content`, `404 Not Found` |

---

## 🏗️ Architecture

```text
HTTP Request
     |
     v
BooksController        (routes, model binding, HTTP status codes)
     |
     v
IBookService / BookService   (business logic, DTO <-> entity mapping)
     |
     v
IBookRepository / BookRepository   (EF Core queries, persistence)
     |
     v
LibraryDbContext        (DbSet<Book>, EF Core change tracking)
     |
     v
MySQL — LibraryManagementDb
```

Every layer only knows about the layer directly beneath it:

- The **controller** never touches `LibraryDbContext` or raw `Book` entities — only the service and DTOs.
- The **service** never writes SQL or LINQ-to-EF queries directly — it delegates to the repository and is responsible for shaping `BookResponse` objects.
- The **repository** is the only layer that talks to `DbContext`, so swapping MySQL for another provider (or swapping EF Core for something else entirely) would only touch this one layer.

Cross-cutting:

```text
Middleware → ExceptionHandlingMiddleware wraps every request
DTOs       → CreateBookRequest / UpdateBookRequest / BookResponse guard the boundary
Migrations → EF Core Code-First migrations version the schema
```

---

## 🗂️ Data Model

**`Book`** (EF Core entity, table `Books`)

| Field | Type | Notes |
|---|---|---|
| `Id` | `int` | Primary key, identity column (MySQL auto-increment) |
| `Title` | `string` | Required |
| `Author` | `string` | Required |
| `ISBN` | `string` | Required |
| `Price` | `decimal` | Stored as `decimal(65,30)` in MySQL |
| `AvailableCopies` | `int` | Current stock of the book |

### Request/response shapes

**`CreateBookRequest`** — used on `POST /api/books`

| Field | Validation |
|---|---|
| `Title` | Required, max length 200 |
| `Author` | Required, max length 150 |
| `ISBN` | Required, max length 20 |
| `Price` | Range 0.01 – 1,000,000 |
| `AvailableCopies` | Range 0 – 10,000 |

**`UpdateBookRequest`** — used on `PUT /api/books/{id}`, same validation rules as `CreateBookRequest`.

**`BookResponse`** — what every endpoint actually returns: `Id`, `Title`, `Author`, `ISBN`, `Price`, `AvailableCopies`. The raw EF Core entity is never serialized directly to the client.

---

## 📡 API Reference

Base route: `/api/books`

| Method | Endpoint | Description | Success | Failure |
|---|---|---|---|---|
| `GET` | `/api/books` | Get every book in the catalog | `200 OK` — array of `BookResponse` | — |
| `GET` | `/api/books/{id}` | Get a single book by id | `200 OK` — `BookResponse` | `404 Not Found` if no book with that id |
| `POST` | `/api/books` | Create a new book | `201 Created` with `Location: /api/books/{id}` and the created `BookResponse` | `400 Bad Request` if validation fails |
| `PUT` | `/api/books/{id}` | Update an existing book | `200 OK` — updated `BookResponse` | `404 Not Found` if no book with that id |
| `DELETE` | `/api/books/{id}` | Delete a book | `204 No Content` | `404 Not Found` if no book with that id |

### Example — create a book

Request:
```http
POST /api/books
Content-Type: application/json

{
  "title": "Clean Architecture",
  "author": "Robert C. Martin",
  "isbn": "9780134494166",
  "price": 899.00,
  "availableCopies": 5
}
```

Response — `201 Created`:
```json
{
  "id": 1,
  "title": "Clean Architecture",
  "author": "Robert C. Martin",
  "isbn": "9780134494166",
  "price": 899.00,
  "availableCopies": 5
}
```

### Example — book not found

```http
GET /api/books/999
```

Response — `404 Not Found` (empty body; the controller short-circuits before the service has anything to return).

### Example — unhandled error

If anything downstream throws an unhandled exception, `ExceptionHandlingMiddleware` catches it, logs it, and returns a uniform shape instead of leaking a stack trace:

```json
{
  "statusCode": 500,
  "message": "An unexpected error occurred."
}
```

---

## ⚠️ Centralized Error Handling

`ExceptionHandlingMiddleware` sits at the very top of the pipeline (registered before `MapControllers`), wraps every request in a `try/catch`, logs the exception with `ILogger`, and writes a consistent JSON error response with a `500` status code. This means individual controller actions don't need repetitive `try/catch` blocks — a single choke point owns "what does an unexpected failure look like to the client."

---

## ⚙️ Tech Stack

| Category | Technology |
|---|---|
| Language | C# |
| Framework | ASP.NET Core / .NET 10 |
| ORM | Entity Framework Core 9 |
| Database Provider | Pomelo.EntityFrameworkCore.MySql |
| Database | MySQL |
| API Docs | Built-in ASP.NET Core OpenAPI (`AddOpenApi` / `MapOpenApi`) |
| Architecture | Controller → Service → Repository layering |
| Schema Management | EF Core Code-First migrations |

---

## 🚀 Running Locally

### Prerequisites
- .NET 10 SDK
- A running MySQL server (local install or Docker container)

### 1. Clone and restore
```bash
git clone https://github.com/ShivamNayak-dev/LibraryManagementApi.git
cd LibraryManagementApi
dotnet restore
```

### 2. Configure the connection string

`appsettings.json` holds the MySQL connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=LibraryManagementDb;User=root;Password=root;"
}
```

Update the `User`/`Password`/`Port` to match your local MySQL instance. Don't commit real credentials for anything beyond local development.

### 3. Apply migrations

The project ships with an `InitialCreate` migration that creates the `Books` table. Apply it with:

```bash
dotnet ef database update
```

(Install the EF Core CLI tool once with `dotnet tool install --global dotnet-ef` if you don't already have it.)

### 4. Run the API
```bash
dotnet run
```

By default the app listens on the port configured in `Properties/launchSettings.json`. In development, the built-in OpenAPI document is available via `MapOpenApi()`.

### 5. Try it

The included `LibraryManagementApi.http` file has a ready-made request you can run directly from an editor with REST client support, or you can hit the endpoints with curl/Postman:

```bash
curl -X POST http://localhost:5092/api/books \
  -H "Content-Type: application/json" \
  -d '{"title":"Clean Code","author":"Robert C. Martin","isbn":"9780132350884","price":799,"availableCopies":3}'
```

---

## 📁 Project Structure

```text
LibraryManagementApi/
├── Controllers/
│   └── BooksController.cs
├── Data/
│   └── LibraryDbContext.cs
├── DTOs/
│   └── Books/
│       ├── BookResponse.cs
│       ├── CreateBookRequest.cs
│       └── UpdateBookRequest.cs
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs
├── Migrations/
│   ├── InitialCreate.cs
│   ├── InitialCreate.Designer.cs
│   └── LibraryDbContextModelSnapshot.cs
├── Models/
│   └── Book.cs
├── Repositories/
│   ├── IBookRepository.cs
│   └── BookRepository.cs
├── Services/
│   ├── IBookService.cs
│   └── BookService.cs
├── appsettings.json
├── appsettings.Development.json
├── LibraryManagementApi.http
├── LibraryManagementApi.csproj
└── Program.cs
```

---

## 🔮 Possible Next Steps

Since this project is intentionally scoped to core CRUD fundamentals, natural extensions include:

- Authentication and role-based authorization (as implemented in later projects)
- Pagination, filtering, and sorting on `GET /api/books`
- Unique constraint/validation on `ISBN` to prevent duplicate catalog entries
- Soft delete instead of hard delete
- Unit tests for `BookService` against a mocked `IBookRepository`
- Swagger UI (Swashbuckle) alongside the built-in OpenAPI document for interactive testing
- Docker Compose for one-command local MySQL setup

---

## 📄 Note

This is a learning-stage project focused on getting a clean layered architecture right before adding complexity — it intentionally does not include authentication, transactions, or concurrency handling, which are covered in later, more advanced projects in this series.

## 👨‍💻 Author

**Shivam Nayak**
GitHub: https://github.com/ShivamNayak-dev
