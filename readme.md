# Restaurant App

This is a restaurant app built with .NET 8 and PostgreSQL.

## Prerequisites

Before running the application, make sure you have the following installed:

- .NET 8 SDK
- PostgreSQL

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/restaurant-app.git
   ```

2. Navigate to the project directory:

   ```bash
   cd restaurant-app
   ```

3. Install the dependencies:

   ```bash
   dotnet restore
   ```

4. Set up the database:

   - Create a new PostgreSQL database.
   - Update the connection string in the `appsettings.json` file with your database credentials.

5. Run the application:

   ```bash
   dotnet run

   ```

6. Migration

   ```bash
   -Add-Migration "init"
   -Update-Database
   ```
