using Library.ManagedConnectivity;

namespace TestHarness
{
    public static class Program
    {
        public static void Main()
        {
            using (var connection = new SqlManagedConnection(".", "Dummyload"))
            {
                using (var reader = connection.ExecuteQuery("SELECT * FROM Test"))
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
        }
    }
}
