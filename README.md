# 🧠 Quiz App – Full Stack Project

This project is a **full-stack web application** built with:
- 🟦 **ASP.NET Core 9.0** (User & Quiz microservices)  
- 🧭 **Ocelot API Gateway**  
- 🅰️ **Angular (Frontend)**  
- 🗄 **Entity Framework Core** for database communication  
- 🧰 **SQL Server** (Database managed through SQL Server Management Studio)  
- 🪵 **Serilog** for logging

---

## 📦 Project Structure

- **UserService** – handles user authentication, profile, and image uploads  
- **QuizService** – handles quiz creation, leaderboard, questions, and attempts  
- **APIGateway** – routes requests between frontend and backend microservices using Ocelot  
- **Frontend (Angular)** – user interface for interacting with the application

---

## 🧰 Prerequisites

Make sure you have installed:
- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- [Node.js 14.17.0](https://nodejs.org/en/)  
- Angular CLI 13.3.11  
- SQL Server + SQL Server Management Studio  
- npm 6.14.13  

---

## 🚀 How to Run the Project

### 1. Open the Solution
Open the `*.sln` solution file in Visual Studio or your preferred IDE.  
The solution contains:
- `UserService`
- `QuizService`
- `APIGateway`

---

### 2. Start Backend Services

Open **three separate Developer PowerShell terminals**:

#### Terminal 1 – User Service
```bash
cd path-to/UserService
dotnet build
dotnet run

Terminal 2 – Quiz Service
cd path-to/QuizService
dotnet build
dotnet run

Terminal 3 – API Gateway
cd path-to/APIGateway
dotnet build
dotnet run

cd path-to/frontend
npm install
ng serve
Once the server starts, open your browser and navigate to:

http://localhost:4200
