using Microsoft.Data.SqlClient;

namespace Library.ManagedConnectivity
{
    /// <summary>
    /// Wraps a native SQL Server connection, opens it and ensures it is cleaned up after it is used.
    /// </summary>
    public class SqlManagedConnection : IDisposable
    {
        /// <summary>
        /// The connection string that was used to establish the connection.
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// The native SQL Server connection object.
        /// </summary>
        public SqlConnection Native { get; private set; }

        #region Disposable.

        private bool disposed = false;

        /// <summary>
        /// Closes and releases native SQL Server connection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try { Native.Close(); } catch { }
                    try { Native.Dispose(); } catch { }
                }

                disposed = true;
            }
        }

        ~SqlManagedConnection()
        {
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// Establishes a new connection to SQL Server using the supplied SQL Server Connection String builder.
        /// </summary>
        /// <param name="builder"></param>
        public SqlManagedConnection(SqlConnectionStringBuilder builder)
        {
            ConnectionString = builder.ToString();
            Native = new SqlConnection(ConnectionString);
            Native.Open();
        }

        /// <summary>
        /// Establishes a new connection to SQL Server to the supplied server name and database name, using integrated security.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        public SqlManagedConnection(string server, string database)
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = server,
                InitialCatalog = database,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true,
                IntegratedSecurity = true
            };

            ConnectionString = builder.ToString();
            Native = new SqlConnection(ConnectionString);
            Native.Open();
        }

        /// <summary>
        /// Establishes a new connection to SQL Server to the supplied server name and database name, using username/password security.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public SqlManagedConnection(string server, string database, string username, string password)
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = server,
                InitialCatalog = database,
                TrustServerCertificate = true,
                IntegratedSecurity = false,
                MultipleActiveResultSets = true,
                UserID = username,
                Password = password
            };

            ConnectionString = builder.ToString();
            Native = new SqlConnection(ConnectionString);
            Native.Open();
        }

        /// <summary>
        /// Establishes a new connection to SQL Server to the supplied connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlManagedConnection(string connectionString)
        {
            ConnectionString = connectionString;
            Native = new SqlConnection(ConnectionString);
            Native.Open();
        }

        /// <summary>
        /// Executes a query and returns a managed reader object so that the results can be enumerated.
        /// </summary>
        /// <param name="queryText"></param>
        /// <returns></returns>
        public SqlManagedReader ExecuteQuery(string queryText)
        {
            return new SqlManagedReader(queryText, Native);
        }

        /// <summary>
        /// Executes a non-query statement.
        /// </summary>
        /// <param name="statementText"></param>
        public void ExecuteNonQuery(string statementText)
        {
            using var cmd = new SqlCommand(statementText, Native);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a query with a single expected column and returns a list of the values.
        /// </summary>
        /// <param name="queryText"></param>
        /// <returns></returns>
        public List<T?> GetValues<T>(string queryText)
        {
            var values = new List<T?>();

            using (var cmd = new SqlCommand(queryText, Native))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    values.Add((T?)reader[0]);
                }
                reader.Close();
            }
            return values;
        }

        /// <summary>
        /// Executes a query with a single expected column and returns the first value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryText"></param>
        /// <returns></returns>
        public T? GetValue<T>(string queryText)
        {
            using (var cmd = new SqlCommand(queryText, Native))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (T)reader[0];
                }
                reader.Close();
            }
            return default;
        }

        /// <summary>
        /// Executes a query with a single expected column and returns the first value of the column with the given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryText"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public T? GetValue<T>(string queryText, string columnName)
        {
            using (var cmd = new SqlCommand(queryText, Native))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (T)reader[columnName];
                }
                reader.Close();
            }
            return default;
        }

        /// <summary>
        /// Executes a query with a single expected column and returns the first value of the column at the given index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryText"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public T? GetValue<T>(string queryText, int columnIndex)
        {
            using (var cmd = new SqlCommand(queryText, Native))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (T)reader[columnIndex];
                }
                reader.Close();
            }
            return default;
        }
    }
}
