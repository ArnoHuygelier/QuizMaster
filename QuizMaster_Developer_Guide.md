
# 🧠 QuizMaster – Developer Guide

## 📁 Project Structure

```
QuizMaster.sln
│
├── QuizMaster.MVC/                  ← Presentation Layer (ASP.NET Core MVC)
│   ├── Controllers/                 ← Web Layer Controllers
│   ├── Models/                      ← ViewModels for Razor Views
│   ├── Views/                       ← Razor Views (cshtml)
│   ├── wwwroot/                     ← Static assets (CSS, JS, Images)
│   └── appsettings.json            ← App configuration
│
├── QuizMaster.Services/            ← Business Logic Layer
│   ├── Interfaces/                 ← Service interfaces
│   └── Services/                   ← Business logic classes
│
├── QuizMaster.Repository/          ← Data Access Layer
│   ├── Interfaces/                 ← Repository interfaces
│   ├── Repositories/              ← EF Core repository implementations
│   ├── Data/                       ← DbContext
│   └── Migrations/                ← EF Migrations
│
├── QuizMaster.DTO/                ← Data Transfer Objects
│   └── DTOs/
│
├── QuizMaster.Model/              ← Domain Entities (EF Core models)
│
└── QuizMaster.Tests/              ← Unit + Integration Tests
```

## 🎨 UI Color Scheme

| Element         | Color    | Use Case                                |
|----------------|----------|------------------------------------------|
| Primary Color  | #007BFF  | Buttons, key highlights                  |
| Secondary      | #0056b3  | Hover states, active elements            |
| Background     | #F0F8FF  | General background                       |
| Accent         | #FFC107  | UI highlights, scoring                   |
| Text           | #333333  | Primary readable text                    |
| Success        | #28A745  | Correct answers, positive feedback       |
| Error          | #DC3545  | Validation errors, wrong answers         |

> 💡 Maintain consistent styling via `_Layout.cshtml` or component partials.

## 🧭 Naming Conventions

| Type        | Convention        | Example                  |
|-------------|-------------------|--------------------------|
| Class       | PascalCase        | `QuizService`            |
| Interface   | Prefix `I`        | `IQuizRepository`        |
| Method      | PascalCase        | `GetQuizById()`          |
| Parameters  | camelCase         | `int quizId`             |
| ViewModels  | Suffix `ViewModel`| `UserProfileViewModel`   |
| DTOs        | Suffix `Dto`      | `QuestionDto`            |
| Controllers | PascalCase plural | `QuizzesController`      |

## 🔄 CRUD Naming

| Operation | Description                        |
|-----------|------------------------------------|
| `Find()`  | Return all records                 |
| `Get(id)` | Return one record by ID           |
| `Update(id)` | Update a record by ID          |
| `Delete(id)` | Delete a record by ID          |

## 🧱 ViewModels

```csharp
public class QuizViewModel
{
    public string Title { get; set; }
    public List<Question> Questions { get; set; }
    public string Mode { get; set; }
}
```

## 📦 DTOs (Data Transfer Objects) (Optional)

```csharp
public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public string CorrectAnswer { get; set; }
}
```

## 🛠️ Setup Instructions

### 1. Prerequisites

- [.NET SDK 9.0+](https://dotnet.microsoft.com/)
- SQL Server
- Optional: Visual Studio 2022+, Rider, or VS Code

### 2. Clone and Build

```bash
git clone https://github.com/your-org/QuizMaster.git
cd QuizMaster
dotnet build
```

### 3. DB Setup

Update connection string in `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=QuizMasterDb;Trusted_Connection=True;"
  }
}
```

Run migrations:

```bash
dotnet ef database update --project QuizMaster.Repository
```

### 4. Run the App

```bash
dotnet run --project QuizMaster.MVC
```

## 🧩 Dependencies / Tools

| Tool         | Purpose                        |
|--------------|--------------------------------|
| ASP.NET Core | Web framework                  |
| EF Core      | ORM / Data access              |
| AutoMapper   | DTO mapping & ViewModels       |   (Optional)
| Bootstrap 5  | UI Framework                   |
| SQL Server   | Relational DBMS                |
| Git + GitHub | Source control                 |
| Figma        | UI Wireframes (optional)       |

Install AutoMapper: (Optional)

```bash
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

Register: (Optional)

```csharp
builder.Services.AddAutoMapper(typeof(Startup));
```

!
## 🧰 AutoMapper Config

```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Quiz, QuizDto>().ReverseMap();
        CreateMap<Question, QuestionDto>();
    }
}
```

## ⚙️ Configuration

Store env-specific config in appropriate `appsettings.{env}.json` files.
  - appsettings.json: base/default config

  - appsettings.Development.json: overrides for dev

  - appsettings.Production.json: overrides for prod

## 🧑‍💼 User Roles

**Admin**: Manage quizzes, view user data.  
**User**: Take quizzes, view scores, earn badges.

## 🧼 Coding Standards

- Keep controllers thin
- Use async when you can
- Use DI, no `new` in services
- DTOs ≠ ViewModels


