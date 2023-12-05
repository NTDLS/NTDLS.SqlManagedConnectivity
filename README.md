# NTDLS.SqlManagedConnectivity

📦 Be sure to check out the NuGet pacakge: https://www.nuget.org/packages/NTDLS.SqlManagedConnectivity

Wraps a native SQL Server connection, allows for easy field/value enumeration and manages cleanup.

>**Simple example:**
>
>In this example we are opening a connection to the local SQL Server (".") and the database ("Dummyload"), then selecting all rows and columns from the table [Test].
> This demonstrates how we can enumaerate the fields and their types as well as the rows and their values with several options for conversion.
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
                Console.WriteLine($"{value.Field.Name} -> '{value.Value.ToString()?.Trim()}'");
            }

            var doublePercentTaxable = row.AsDouble("PercentTaxable");
            var decimalPercentTaxable = row.AsDecimal("OriginalAmount");
            var stringPercentTaxable = row.AsString("OriginalAmount");
            var intPercentTaxable = row.AsInt("OriginalAmount");
        }
    }
}
```

## License
[Apache-2.0](https://choosealicense.com/licenses/apache-2.0/)
