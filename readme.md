## ⚙️ Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- MySQL Database (a AWS MySql Database already provided )

---

## 🚀 Backend Setup

1. Restore NuGet packages:

    ```bash
    dotnet restore
    ```

2. (If using EF Core) Apply migrations: (not needed as it is already applied)

    ```bash
    dotnet ef migrations add CreateTreasureTable 
    dotnet ef database update
    ```

3. Run the server:

    ```bash
    dotnet run
    ```

   The API will start on `https://localhost:5001` or `http://localhost:5000`.

---
