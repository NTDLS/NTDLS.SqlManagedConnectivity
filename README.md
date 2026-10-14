# NTDLS.SqlManagedConnectivity

A small C# library that wraps a native SQL Server connection (`Microsoft.Data.SqlClient`). It opens the connection for you, exposes result sets as easily enumerable fields and rows with built-in type conversion, and takes care of closing and disposing everything when you are done.

📦 Be sure to check out the NuGet package: https://www.nuget.org/packages/NTDLS.SqlManagedConnectivity

## Why use it?

Use this package when you want direct, lightweight access to SQL Server results without managing `SqlConnection`, `SqlCommand` or `SqlDataReader` lifecycles by hand:

- **Connection management** — the connection is opened on construction and reliably closed/disposed via `using`, so you never leak connections.
- **Simple enumeration** — query results come back as enumerable fields and rows; iterate columns generically instead of writing index-based reader code.
- **Built-in conversion** — pull values by column name (case-insensitive) and convert them to the .NET type you need, with options for nullable or non-null expectations.
- **Parameterized execution** — pass an anonymous object as parameters and each property becomes a named `@parameter`.
- **Procedures and non-queries** — run stored procedures (with or without result sets) and plain statements in addition to ad-hoc queries.

If you are looking for something more POCO/Dapper-style, then check out the [NTDLS.SqlServerDapperWrapper](https://github.com/NTDLS/NTDLS.SqlServerDapperWrapper).

## Usage examples

### Connecting

```csharp
// Integrated security against a server and database.
using var connection = new SqlManagedConnection(".", "Dummyload");

// Username/password security.
using var sqlAuth = new SqlManagedConnection("myserver", "Dummyload", "sa", "P@ssw0rd!");

// Or supply your own connection string / builder.
using var fromString = new SqlManagedConnection("Server=.\\SQLEXPRESS;Database=Dummyload;Trusted_Connection=True;");
using var fromBuilder = new SqlManagedConnection(new SqlConnectionStringBuilder { DataSource = ".", InitialCatalog = "Dummyload" });
```

### Querying and enumerating results

In this example we open a connection to the local SQL Server ("."), select rows from `[Test]`, enumerate the fields and their types, then read row values with several conversion options.

```csharp
using var connection = new SqlManagedConnection(".", "Dummyload");

// ExecuteQuery returns a managed reader; parameters come from the anonymous object's properties.
using var reader = connection.ExecuteQuery(
    "SELECT * FROM Test WHERE Account <> @Account",
    new { Account = 4104 });

// Loop through all fields (column metadata):
foreach (var field in reader.Fields)
{
    Console.WriteLine($"Field: '{field.Name}', Data Type: '{field.DataTypeName}', Type: '{field.Type.Name}'");
}

// Loop through all rows:
foreach (var row in reader)
{
    // Loop through all values in the row:
    foreach (var value in row)
    {
        // Convert the value to a string (null if DBNull).
        Console.WriteLine($"{value.Field.Name} -> '{value.As<string>()?.Trim()}'");
    }

    // Or pull a single column by name, converted to the type you want:
    var percentTaxable = row.Value<double>("PercentTaxable");
    var originalAmountAsDecimal = row.Value<decimal>("OriginalAmount");
    var originalAmountAsString = row.Value<string>("OriginalAmount");
}
```

### Manual row stepping

```csharp
using var connection = new SqlManagedConnection(".", "Dummyload");
using var reader = connection.ExecuteQuery("SELECT TOP 5 Id, Name FROM Test");

// Fetch one row at a time; NextRow() returns null when the result set is exhausted.
while (reader.NextRow() is { } row)
{
    int id = row.ValueNotNull<int>("Id");   // Throws if the value is null.
    string? name = row.Value<string>("Name"); // Returns null if the value is null.
    Console.WriteLine($"{id}: {name}");
}
```

### Stored procedures and non-query statements

```csharp
using var connection = new SqlManagedConnection(".", "Dummyload");

// Procedure that returns rows.
using var reader = connection.ExecuteQueryProcedure("dbo.MyProcedure", new { BatchId = 7 });
foreach (var row in reader)
{
    Console.WriteLine(row.Value<string>("ResultColumn"));
}

// Procedure that only performs work.
connection.ExecuteNonQueryProcedure("dbo.CleanupOldRecords", new { CutoffDate = DateTime.Now.AddDays(-30) });

// Plain non-query statement (INSERT/UPDATE/DELETE), also parameterized.
connection.ExecuteNonQuery("UPDATE Test SET Account = @Account WHERE Id = @Id", new { Account = 99, Id = 42 });
```

### Quick scalar helpers

```csharp
using var connection = new SqlManagedConnection(".", "Dummyload");

// First value of the first column, typed.
int count = connection.GetValue<int>("SELECT COUNT(*) FROM Test");

// First value of a named column.
string? topName = connection.GetValue<string>("SELECT TOP 1 Name FROM Test ORDER BY Id", "Name");

// All values of a single-column result set.
List<int>? ids = connection.GetValues<int>("SELECT Id FROM Test");
```

## License

[MIT](https://choosealicense.com/licenses/mit/)
