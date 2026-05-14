# DMC Verifier

A lightweight Windows Forms application for validating DMC (Data Matrix Code) values against a SQL Server database. Built with VB.NET and .NET Framework 4.8.

## How It Works

1. The application connects to a SQL Server Express database on startup.
2. The user enters or scans a DMC value into the text box and presses **Enter**.
3. The app sends the DMC as a JSON payload to the stored procedure.
4. The stored procedure returns a JSON response indicating success or failure, which is displayed to the user.
5. If database returns TotalCount = 0 the form background would become red, if TotalCount = 1 then background would be green else would be yellow, meaning multiple records with that DMC exist in the database.

## Prerequisites

- Windows
- [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (for development)
- SQL Server Express with stored procedure

## Getting Started

1. Clone the repository.
2. Open `sql_dmc_check.sln` in Visual Studio 2022.
3. Restore NuGet packages (`nuget restore sql_dmc_check.sln`).
4. Update the connection string in `MainForm.vb` to point to your SQL Server instance.
5. Build and run.

## Configuration

The database connection string is defined in `MainForm.vb`:

```
Server=<your_server>\SQLEXPRESS,1433;Database=<your_database_name>;User Id=<user>;Password=<password>;
```

Update it to match your SQL Server environment.

## Required Database Objects

The application expects a stored procedure that:

- Accepts an input parameter `@payload` (`NVARCHAR(MAX)`) containing JSON: `{"dmc": "<value>"}`
- Returns an output parameter `@result` (`NVARCHAR(MAX)`) containing JSON: `{"success": 1, "message": "..."}` or `{"success": 0, "message": "..."}`

## Dependencies

- [Newtonsoft.Json 13.0.4](https://www.nuget.org/packages/Newtonsoft.Json/13.0.4)

## License

This project is provided as-is for internal use.
