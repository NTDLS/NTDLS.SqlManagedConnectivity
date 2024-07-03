namespace NTDLS.SqlManagedConnectivity
{
    public class SqlManagedReaderRow : IEnumerable<SqlManagedFieldValue>
    {
        private readonly SqlManagedReader _reader;

        internal SqlManagedReaderRow(SqlManagedReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Returns the enumerator containing the fields and their values.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<SqlManagedFieldValue> GetEnumerator()
        {
            for (int i = 0; i < _reader.NativeReader.FieldCount; i++)
            {
                yield return new SqlManagedFieldValue(_reader.Fields.AtIndex(i), _reader.NativeReader[i]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Converters.

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public byte[]? AsByteArray(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);

            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }

            int dataLength = (int)_reader.NativeReader.GetBytes(columnIndex, 0, null, 0, int.MaxValue);
            var byteArray = new byte[dataLength];
            _reader.NativeReader.GetBytes(columnIndex, 0, byteArray, 0, dataLength);
            return byteArray;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public byte[] AsByteArray(string fieldName, byte[] defaultValue)
            => AsByteArray(fieldName) ?? defaultValue;

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public byte[] AsByteArrayNotNull(string fieldName)
        {
            return AsByteArray(fieldName) ?? throw new Exception("The value was unexpectedly null.");
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public T? Value<T>(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return default;
            }
            var value = _reader.NativeReader[_reader.Fields.IndexOf(fieldName)];

            return (T?)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public T ValueNotNull<T>(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                throw new Exception($"The value of [{fieldName}] was unexpectedly null.");
            }

            var value = _reader.NativeReader[_reader.Fields.IndexOf(fieldName)]
                ?? throw new Exception($"The value of [{fieldName}] was unexpectedly null.");

            return (T)Convert.ChangeType(value, typeof(T));
        }

        #endregion
    }
}