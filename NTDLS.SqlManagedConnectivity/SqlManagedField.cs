namespace NTDLS.SqlManagedConnectivity
{
    /// <summary>
    /// Provides information about a SQL Server column, its data type and type.
    /// </summary>
    public class SqlManagedField
    {
        internal string Key { get; private set; }

        /// <summary>
        /// The name of the column.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The type of the column.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// The name of the database data type.
        /// </summary>
        public string DataTypeName { get; private set; }

        internal SqlManagedField(string name, Type type, string dataTypeName)
        {
            Key = name.ToLowerInvariant();
            DataTypeName = dataTypeName;
            Name = name;
            Type = type;
        }
    }
}
