# NTDLS.SqlManagedConnectivity

📦 Be sure to check out the NuGet package: https://www.nuget.org/packages/NTDLS.SqlManagedConnectivity

Wraps a native SQL Server connection, allows for easy field/value enumeration and manages cleanup.

This library is a replacement for native-style SQL Server access, if you are looking something more POCO/Dapper,
then check out the https://github.com/NTDLS/NTDLS.SqlServerDapperWrapper.

>**Simple example:**
>
>In this example we are opening a connection to the local SQL Server (".") and the database ("Dummyload"), then selecting all rows and columns from the table [Test].
> This demonstrates how we can enumerate the fields and their types as well as the rows and their values with several options for conversion.
```csharp
using (var connection = new SqlManagedConnection(".", "Dummyload"))
{
    using (var reader = connection.ExecuteQuery("SELECT * FROM Test WHERE Account <> @Account", new { Account = 4104 }))
    {
        //Loop through all fields:
        foreach (var field in reader.Fields)
        {
            Console.WriteLine($"Field: '{field.Name}', Data Type: '{field.DataTypeName}', Type: '{field.Type.Name}'");
        }

        //Loop though all rows:
        foreach (var row in reader)
        {
            //Loop though all values in the row:
            foreach (var value in row)
            {
                Console.WriteLine($"{value.Field.Name} -> '{value.As<string>()?.Trim()}'");
            }

                var doublePercentTaxable1 = row.Value<double>("PercentTaxable");
                var decimalPercentTaxable1 = row.Value<decimal>("OriginalAmount");
                var stringPercentTaxable1 = row.Value<string>("OriginalAmount");
                var intPercentTaxable1 = row.Value<int>("OriginalAmount");
        }
    }
}
```

## License
[Apache-2.0](https://choosealicense.com/licenses/apache-2.0/)
