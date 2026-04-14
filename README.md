# FinCouple

A couple's expense management app built with .NET 8 Clean Architecture and React.

## Architecture

```
FinCouple/
├── src/
│   ├── FinCouple.Domain/         # Entities, enums, repository interfaces
│   ├── FinCouple.Application/    # Services, DTOs, service interfaces
│   ├── FinCouple.Infrastructure/ # EF Core, repositories, Redis, hosted service
│   └── FinCouple.API/            # ASP.NET Core Web API, controllers, JWT, CORS
└── frontend/
    └── fincouple-web/            # Vite + React (plain JS)
```

### Layers
- **Domain**: Entities with private setters and factory methods; generic `IRepository<T>` and entity-specific interfaces
- **Application**: Services implementing interfaces; depend only on abstractions; BCrypt for passwords; JWT token generation
- **Infrastructure**: Entity Framework Core + PostgreSQL (Npgsql); Fluent API configurations; Redis pub/sub; RecurrenceHostedService runs daily at midnight
- **API**: REST controllers; JWT Bearer authentication; CORS; SSE streaming endpoint; Swagger/OpenAPI

## How to Run with Docker

1. Copy the environment file and edit values:
   ```bash
   cp .env.example .env
   ```

2. Build and start all services:
   ```bash
   docker-compose up --build
   ```

3. Apply EF migrations (first run):
   ```bash
   docker-compose exec api dotnet ef database update
   ```

4. Access the app:
   - Frontend: http://localhost
   - API: http://localhost:5000
   - Swagger: http://localhost:5000/swagger

## Database Schema

| Table               | Key Columns                                                                                         |
|---------------------|-----------------------------------------------------------------------------------------------------|
| Users               | Id, Name, Email (unique), PasswordHash, CoupleId (FK)                                              |
| Couples             | Id, Name, CreatedAt                                                                                 |
| Categories          | Id, Name, Color, Icon, IsDefault                                                                    |
| Expenses            | Id, CoupleId, CategoryId, PaidByUserId, RecurringExpenseId?, Description, Amount, Date, IsRecurring |
| RecurringExpenses   | Id, CoupleId, CategoryId, CreatedByUserId, Description, Amount, RecurrenceType, DayOfMonth, IsActive, NextDueDate |
| Budgets             | Id, CoupleId, CategoryId, LimitAmount, Month, Year (unique: CoupleId+CategoryId+Month+Year)         |

## Real-time Updates

When a new expense is created, the API publishes to the Redis channel `couple:{coupleId}`. The frontend subscribes to `GET /api/sse/stream?coupleId=<id>` using the native `EventSource` API and updates the expense list in real-time.

## Environment Variables

See `.env.example` for all required variables.
