# QuizMaster

**QuizMaster** is a web-based quiz application that supports multiple play modes, real-time scoring, leaderboards, and gamified rewards. Built for both casual users and competitive players, it’s suitable for all ages and learning levels.

## 🔧 Tech Stack

- **Backend**: ASP.NET MVC  
- **Database**: MS SQL Server  
- **ORM**: Entity Framework

## 🚀 Features

### 🧠 Quiz Modes
- **Timed Mode** – Answer under time pressure.
- **Casual Mode** – No time limit.
- **Challenge Mode** – Harder questions, higher rewards.

Each mode has unique scoring rules.

### 📚 Quizzes & Users
- Multiple-choice questions with optional hints
- Quizzes organized by category/topic

**Users can:**
- Register and log in
- Edit profile info
- Track quiz history and progress
- View leaderboard standings

**Admins can:**
- Manage users (create/update/delete)
- Manage quizzes and questions

### 🏆 Leaderboards
- Public, real-time rankings based on quiz scores
- Updates after every quiz completion

### 🎯 Gamification
- **Achievements & Badges**  
  - Milestones: e.g., 10 quizzes completed, perfect score
- **Daily Challenges**  
  - Daily quiz with bonus points/rewards

## 📦 Setup & Deployment

```bash
# Clone the repository
git clone https://github.com/yourusername/quizmaster.git

# Open the solution in Visual Studio
# Configure the connection string in appsettings.json

# Run database migrations
Update-Database

# Start the application
F5 or Ctrl+F5 in Visual Studio
```

