using NTDLS.SqlManagedConnectivity;

namespace TestHarness
{
    public static class Program
    {
        public static void Main()
        {
            using var connection = new SqlManagedConnection(".", "Dummyload");
            using var reader = connection.ExecuteQuery("SELECT * FROM LoadA WHERE Account <> @Account", new { Account = 4104 });

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
}
