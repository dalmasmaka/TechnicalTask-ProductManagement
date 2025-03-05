# Product Management System

## Description
This is a simple Product Management system built using .NET 8. The project provides CRUD operations for managing products efficiently. The backend is developed with ASP.NET Core Web API, utilizing Entity Framework Core for database operations.

## Prerequisites
Make sure you have the following installed on your machine:
- .NET 8 SDK
- SQL Server
- Visual Studio or VS Code (Optional but recommended)
- Git

## Installation and Setup
Follow these steps to set up and run the project:

1. **Clone the repository**
   ```sh
   git clone https://github.com/dalmasmaka/TechnicalTask-ProductManagement.git
   ```
2. **Navigate to the project directory**
   ```sh
   cd TechnicalTask-ProductManagement
   ```
3. **Open the project in Visual Studio or VS Code**
4. **Restore dependencies**
   ```sh
   dotnet restore
   ```
5. **Update the database connection string**
   - Open `appsettings.json`
   - Modify the connection string under `ConnectionStrings` to match your SQL Server instance.
   
   Example:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=TechnicalTask-PM;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
6. **Add database migrations**
   ```sh
   dotnet ef migrations add InitialCreate --project PM-Infrastructure --startup-project PM-API
   ```
7. **Update the database**
   ```sh
   dotnet ef database update --project PM-Infrastructure --startup-project PM-API
   ```
8. **Run the application**
   ```sh
   dotnet run
   ```

## API Endpoints
Once the application is running, you can access the API via Swagger

## Contribution
Feel free to fork the repository, make changes, and submit a pull request.

## License
This project is open-source and free to use under the [MIT License](LICENSE).

