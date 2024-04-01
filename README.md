## Step 1: Blog-Server 

This package includes the backend API for a Blog Platform, equipped with a set of xUnit tests.

To get started, open the Package Manager Console within Visual Studio.
Run the following command to restore all the necessary dependencies for the project.

```sh
dotnet restore
```

This will ensure that all the required components are readily available for project.
Open appsettings.json file located in "Blog-Server" project folder and replace with your database connection string:

```json
  "DatabaseConnectionString": "YourConnectionStringHere",
```

For database changes, go back to the Package Manager Console. Use the next command to update your database.

```sh
Update-Database
```

That should be all you need to get the project up and running in debug mode using Visual Studio.
Alternatively, you can open the "Blog-Server" project folder in PowerShell. Just run the following command.

```sh
dotnet run
```

To run tests in Visual Studio, you can find the "Test" option. Click on it and then hit "Run tests". Or if you prefer shortcuts, just press "ctrl + R + A"

---
## Step 2: Blog-Client

This package includes the frontend React application for a Blog.

To get started, you need to set up all the necessary parts. Open PowerShell and use this command:

```sh
npm install
```

After setting up, you can start up the project using the following command:

```sh
npm run dev
```

---

## Part 3: SQL scripts
Stored produre SQL script can be found at "Sql-Scripts" folder
The BTB-SpitzerTableScript.sql file is responsible for creating the necessary procedures and preparing the solution.
BTB-SpitzerTableResult.sql will yield the results of the operation.

