namespace Library.ManagedConnectivity
{
    public class SqlManagedReaderRow : IEnumerable<SqlManagedFieldValue>
    {
        private readonly SqlManagedReader _reader;

        internal SqlManagedReaderRow(SqlManagedReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Returns the enumerator contianing the fields and their values.
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
        public byte[] AsByteArray(string fieldName, byte[] defaultValue) => AsByteArray(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public byte[] AsByteArrayNotNull(string fieldName)
        {
            var value = AsByteArray(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public byte? AsByte(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToByte(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public byte AsByte(string fieldName, byte defaultValue) => AsByte(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public byte AsByteNotNull(string fieldName)
        {
            var value = AsByte(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (byte)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public float? AsFloat(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToSingle(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public float AsFloat(string fieldName, float defaultValue) => AsFloat(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public float AsFloatNotNull(string fieldName)
        {
            var value = AsFloat(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (float)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public DateTime? AsDateTime(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToDateTime(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public DateTime AsDateTime(string fieldName, DateTime defaultValue) => AsDateTime(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public DateTime AsDateTimeNotNull(string fieldName)
        {
            var value = AsDateTime(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (DateTime)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public bool? AsBoolean(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToBoolean(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public bool AsBoolean(string fieldName, bool defaultValue) => AsBoolean(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public bool AsBooleanNotNull(string fieldName)
        {
            var value = AsBoolean(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (bool)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public ushort? AsUShort(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToUInt16(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public ushort AsUShort(string fieldName, ushort defaultValue) => AsUShort(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public ushort AsUShortNotNull(string fieldName)
        {
            var value = AsUShort(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (ushort)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public short? AsShort(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToInt16(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public short AsShort(string fieldName, short defaultValue) => AsShort(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public short AsShortNotNull(string fieldName)
        {
            var value = AsShort(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (short)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public uint? AsUInt(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToUInt32(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public uint AsUInt(string fieldName, uint defaultValue) => AsUInt(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public uint AsUIntNotNull(string fieldName)
        {
            var value = AsUInt(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (uint)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public int? AsInt(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToInt32(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public int AsInt(string fieldName, int defaultValue) => AsInt(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public int AsIntNotNull(string fieldName)
        {
            var value = AsInt(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (int)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public ulong? AsULong(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToUInt64(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public ulong AsULong(string fieldName, ulong defaultValue) => AsULong(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public ulong AsULongNotNull(string fieldName)
        {
            var value = AsULong(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (ulong)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public long? AsLong(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToInt64(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public long AsLong(string fieldName, long defaultValue) => AsLong(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public long AsLongNotNull(string fieldName)
        {
            var value = AsLong(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (long)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public decimal? AsDecimal(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToDecimal(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public decimal AsDecimal(string fieldName, decimal defaultValue) => AsDecimal(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public decimal AsDecimalNotNull(string fieldName)
        {
            var value = AsDecimal(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (decimal)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public double? AsDouble(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToDouble(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public double AsDouble(string fieldName, double defaultValue) => AsDouble(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public double AsDoubleNotNull(string fieldName)
        {
            var value = AsDouble(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : (double)value;
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public string? AsString(string fieldName)
        {
            int columnIndex = _reader.Fields.IndexOf(fieldName);
            if (_reader.NativeReader.IsDBNull(columnIndex))
            {
                return null;
            }
            return Convert.ToString(_reader.NativeReader[_reader.Fields.IndexOf(fieldName)]);
        }
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public string AsString(string fieldName, string defaultValue) => AsString(fieldName) ?? defaultValue;
        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        public string AsStringNotNull(string fieldName)
        {
            var value = AsString(fieldName);
            return value == null ? throw new Exception("The value was unexpectedly NULL.") : value;
        }

        #endregion
    }
}
