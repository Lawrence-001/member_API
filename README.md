.NET 8 Web API

This is a .NET 8 Web API project that demonstrates development practices including RESTful design, EF Core integration, authentication with JWT, and role-based authorization.

Key concepts

Dependency Injection (DI) – Built-in .NET 8 service container used to inject repositories, services, and validators.

Repository Pattern – Separates business logic from data access for better testability and maintainability.

RESTful API – Follows standard HTTP verbs (`GET`, `POST`, `PUT`, `DELETE`) with proper status codes.

Entity Framework Core (EF Core 8) – ORM for database operations using `DbContext`, LINQ, and code-first migrations.

Data Transfer Objects (DTOs) – Used for input/output models and avoid exposing domain models directly.

AutoMapper – Automates the mapping between domain models and DTOs to reduce boilerplate.

JWT Authentication – Secure login flow using JSON Web Tokens, issued upon successful login.

Role-Based Access Control (RBAC) – Protect endpoints based on user roles like `Admin`,  or `User`.


Setup

git clone https://github.com/Lawrence-001/member_API.git
cd member_API


Restore packages
dotnet restore

Setup database
Ensure your database connection string is set in appsettings.json

Apply migration
dotnet ef database update

Run the project
dotnet run







