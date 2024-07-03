using Microsoft.Data.SqlClient;

namespace NTDLS.SqlManagedConnectivity
{
    /// <summary>
    /// Wraps a native SQLDataReader and provides easy row, field and value enumeration.
    /// </summary>
    public class SqlManagedReader : IEnumerable<SqlManagedReaderRow>, IDisposable
    {
        /// <summary>
        /// The native SqlDataReader object.
        /// </summary>
        public SqlDataReader NativeReader { get; private set; }

        private readonly SqlCommand _sqlCommand;

        private SqlManagedReaderFields? _fields = null;

        /// <summary>
        /// Collection of the fields that are present in the data reader.
        /// </summary>
        public SqlManagedReaderFields Fields
        {
            get
            {
                _fields ??= new SqlManagedReaderFields(NativeReader);
                return _fields;
            }
        }

        #region Disposable.

        private bool disposed = false;

        /// <summary>
        /// Releases resources associated with the data reader and its command.
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
                    try { NativeReader.Close(); } catch { }
                    try { _sqlCommand.Dispose(); } catch { }
                }

                disposed = true;
            }
        }

        ~SqlManagedReader()
        {
            Dispose(false);
        }

        #endregion

        internal SqlManagedReader(SqlCommand sqlCommand)
        {
            _sqlCommand = sqlCommand;
            NativeReader = _sqlCommand.ExecuteReader();
        }

        /// <summary>
        /// Retrieves the next row in the data reader. Useful when you want to manage the enumeration manually or only fetch a single row.
        /// </summary>
        /// <returns></returns>
        public SqlManagedReaderRow? NextRow()
        {
            if (NativeReader.Read())
            {
                return new SqlManagedReaderRow(this);
            }

            return null;
        }

        /// <summary>
        /// Returns the enumerator for the rows.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<SqlManagedReaderRow> GetEnumerator()
        {
            while (NativeReader.Read())
            {
                yield return new SqlManagedReaderRow(this);
            }
            NativeReader.Close();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
