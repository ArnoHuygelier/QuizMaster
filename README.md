# QuizMaster

**QuizMaster** is a web-based quiz application designed for both casual and competitive users.
Built for both casual users and competitive players, it’s suitable for all ages and learning levels.

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core MVC (.NET 9)
- **Frontend:** Razor Views, Bootstrap
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** ASP.NET Core Identity

---

## 🚀 Features

### Quiz playing
- **Timed Quiz Mode:** All quizzes are played in a single timed mode, challenging users to answer questions as quickly and accurately as possible, more points for fast answers.
- **Multiple-Choice Questions:** Each quiz consists of multiple-choice questions, optionally with hints.
- **Categories:** Quizzes are organized by category for focused learning.
- **Leaderboards:** Real-time public rankings based on quiz scores.
- **Achievements & Badges:** Earn badges for reaching milestones and high performance.
- **App-Wide Hints:** Each user has a limited number of hints available throughout the application.
- **Earning Hints:** Users can earn additional hints by meeting specific criteria, such as achieving flawless quiz results or consistently high scores.
- **Hint Usage:** Hints can be used on any quiz question, but their number is limited, so use them wisely.

### User Management
- **User Accounts:** Secure registration and login are implemented using ASP.NET Core Identity.
- **Profile Management:** Users can edit their profile, select avatars, and view their quiz history and progress.


### Admin features
- **Admin Tools:** Admins can manage users, quizzes, categories, badges, avatars and questions.
---


## 📦 Setup & Deployment

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Getting Started

1. **Clone the repository:**
2. **Open the solution:**
- Launch Visual Studio 2022.
- Open the `QuizMaster.sln` file.

3. **Configure the database connection:**
- Add `appsettings.Development.json` file in the main (MVC) project.
- Add
  ```bash
  {
    "ConnectionStrings": {
      "QuizMasterDbContext": "Data Source={YOURSQLCONNECTION};Initial Catalog=QuizMaster;Integrated Security=True;Trust Server Certificate=True"
    }
  }
  ```
  


4. **Apply database migrations:**
- Open the Package Manager Console in Visual Studio.
- Set the default project to the repository project.
- Run:
  ```bash
  Update-Database
  ```

5. **Run the application:**
- Press `F5` (with debugging) or `Ctrl+F5` (without debugging) in Visual Studio.

---

## 📝 Project Structure

- `QuizMaster.Ui.Mvc/` – Main ASP.NET Core MVC web application (controllers, views,viewModels, tag helpers)
- `QuizMaster.Services/` – Business logic and service interfaces/implementations
- `QuizMaster.Models/` – Domain models
- `QuizMaster.Data/` – Database context and migrations

---

## ⚠️ Roadmap

Some features described in earlier documentation (such as daily challenges, multiple quiz modes, API integration) are not currently implemented. These, along with additional gamification and quiz features, may be added in future releases.

---


## 🙋 Support

For questions or support, please open an issue on GitHub.


