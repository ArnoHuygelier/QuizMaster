
# 📘 QuizMaster Developer Guide

- [UI Color Scheme](#ui-color-scheme)
- [Project Structure (ASP.NET MVC)](#project-structure-aspnet-mvc)
- [Naming conventions](#naming-conventions)
- [ViewModels](#viewmodels)
- [DTOs (Data Transfer Objects)](#dtos-data-transfer-objects)
- [Development Guidelines](#development-guidelines)
- [Dependencies / Tools](#dependencies--tools)
- [Configuration](#configuration)



## UI Color Scheme

| Element            | Color       | Use Case                                    |
|--------------------|-------------|---------------------------------------------|
| Primary Color      | `#007BFF`   | Buttons, key highlights                     |
| Background         | `#F0F8FF`   | General page background                     |
| Secondary Color    | `#0056b3`   | Hover states, active elements               |
| Accent Color       | `#FFC107`   | Score indicators, special UI highlights     |
| Text Color         | `#333333`   | General text for readability                |
| Success            | `#28A745`   | Correct answers, success alerts             |
| Error              | `#DC3545`   | Incorrect answers, validation errors        |

Use consistent styling in all views to maintain UI/UX coherence.

## Project Structure (ASP.NET MVC)

```
QuizMaster.sln
│
├── QuizMaster.MVC/                     ← MVC UI Project (Presentation Layer)
│   ├── Controllers/
│   │   ├── HomeController.cs
│   │   ├── QuizzesController.cs
│   │   ├── UsersController.cs
│   │   └── AdminController.cs
│   ├── Models/
│   │   ├── QuizViewModel.cs
│   │   ├── UserProfileViewModel.cs
│   │   └── LeaderboardViewModel.cs
│   ├── Views/
│   │   └── (Razor Views: Home, Quizzes, Users, Admin, etc.)
│   ├── wwwroot/                       ← Static files (CSS, JS, images)
│   └── appsettings.json
│
├── QuizMaster.Services/               ← Business Logic Layer
│   ├── Interfaces/│   
│   │   └── IService.cs
│   │   
│   ├── Services/
│   │   ├── QuizService.cs
│   │   ├── UserService.cs
│   │   └── LeaderboardService.cs
├── QuizMaster.Repository/             ← Data Access Layer
│   ├── Interfaces/
│   │   └── IRepository.cs
│   ├── Repositories/
│   │   ├── QuizRepository.cs
│   │   ├── UserRepository.cs
│   │   └── QuestionRepository.cs
│   ├── Data/
│   │   └── QuizMasterDbContext.cs
│   └── Migrations/
├── QuizMaster.DTO/                   ← Optional: Data  Layer 
│    └── DTOs/                         
│       ├── QuizDto.cs
│       ├── UserDto.cs
│       └── AnswerDto.cs
│
├── QuizMaster.Model/                  ← Domain Models
│   ├── Answer.cs
│   ├── Avatar.cs
│   ├── Badge.cs
│   ├── Category.cs
│   ├── Question.cs
│   ├── Quiz.cs
│   ├── User.cs
│   ├── UserBadge.cs
│   └── Answer.cs
│
└── QuizMaster.Tests/                  ← Optional: Unit + Integration Tests
    ├── QuizServiceTests.cs
    ├── UserControllerTests.cs
    └── RepositoryTests.cs

```

## Naming conventions

### C# conventions

- **Services**: Interface prefixed with `I` (e.g., `IService`)
- **Methods**: Use `async` methods where appropriate
- **ViewModels**: Always suffixed with `ViewModel`
- **DTOs**: Always suffixed with `Dto`




| Object            | Notation      | Plural? |
|-------------------|---------------|---------|
| Classname         | PascalCase    | No      |
| Methodname        | PascalCase    | No      |
| Method parameters | camelCase     | No      |
| Controllers       | PascalCase    | Yes     |



### CRUD conventions
| Operation         | Description                         |
|-------------------|-------------------------------------|
| Find()            | Return all records                  |
| Get(int id)       | Return 1 record based on parameter  |
| Update(int id)    | Update 1 record based on parameter  |
| Delete(int id)    | Delete 1 record based on parameter  |

##  ViewModels

Used to send structured, often simplified or formatted data from controller to view.

**Example:**

```csharp
public class QuizViewModel
{
    public string Title { get; set; }
    public List<Question> Questions { get; set; }
    public string Mode { get; set; }
}
```

## DTOs (Data Transfer Objects)

Used to transport data between layers (especially service and controller), often hiding entity complexity or enforcing format.

**Example:**

```csharp
public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public string CorrectAnswer { get; set; }
}
```



## Development Guidelines

- Stick to **Separation of Concerns**:
  - Controller: Handles HTTP logic
  - Service: Business logic
  - Repository: Data access
- Use **ViewModels** only in views, never in services or repositories


Optional
- Use **DTOs** to pass data between services and controllers
- Write **unit tests** for services

## Dependencies / Tools

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap 5
- Git + GitHub for version control
- Figma for UI wireframes
Optional
- AutoMapper (for mapping between DTOs and ViewModels)


## Configuration

### Database Connection

Update your personal connection string in appsettings.Development.json


## Usage
### Admin Users

Log in with admin credentials
Access the admin dashboard to create and manage quizzes
Review user submissions and manage content

### Regular Users

Register or log in
Browse available quizzes
Take quizzes and view scores
Track progress on the user dashboard