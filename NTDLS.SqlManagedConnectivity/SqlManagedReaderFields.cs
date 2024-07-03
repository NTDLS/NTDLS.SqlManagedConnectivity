using Microsoft.Data.SqlClient;

namespace NTDLS.SqlManagedConnectivity
{
    /// <summary>
    /// A collection of fields that provide information about the SQL Server columns, their data types and types.
    /// </summary>
    public class SqlManagedReaderFields : IEnumerable<SqlManagedField>
    {
        private readonly SqlDataReader _reader;
        private List<SqlManagedField>? _collection = null;

        internal SqlManagedReaderFields(SqlDataReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// The collection of fields.
        /// </summary>
        public List<SqlManagedField> Collection
        {
            get
            {
                if (_collection == null)
                {
                    lock (this)
                    {
                        if (_collection == null) //Ensure another thread didn't fill these in.
                        {
                            _collection = new List<SqlManagedField>();
                            for (int i = 0; i < _reader.FieldCount; i++)
                            {
                                _collection.Add(new SqlManagedField(_reader.GetName(i), _reader.GetFieldType(i), _reader.GetDataTypeName(i)));
                            }
                        }
                    }
                }
                return _collection;
            }
        }

        /// <summary>
        /// Returns the field at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SqlManagedField AtIndex(int index)
        {
            if (index < 0 || index >= Collection.Count)
            {
                throw new Exception($"The field index [{index}] was outside of the collection bounds.");
            }
            return Collection[index];
        }

        /// <summary>
        /// Returns the index of a given field (not case sensitive).
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public int IndexOf(string fieldName)
        {
            fieldName = fieldName.ToLowerInvariant();

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i].Key == fieldName)
                {
                    return i;
                }
            }
            throw new Exception($"The field [{fieldName}] was not found in the collection.");
        }

        /// <summary>
        /// Returns the index of a given field (case sensitive).
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public int IndexOfSensitive(string fieldName)
        {
            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i].Name == fieldName)
                {
                    return i;
                }
            }
            throw new Exception($"The field [{fieldName}] was not found in the collection.");
        }

        /// <summary>
        /// Returns true if the given field is present in the collection and passes the index out through the our index parameter. (not case sensitive).        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool TryGetIndex(string fieldName, out int index)
        {
            fieldName = fieldName.ToLowerInvariant();

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i].Key == fieldName)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        /// <summary>
        /// Returns true if the given field is present in the collection and passes the index out through the our index parameter. (case sensitive).
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool TryGetIndexSensitive(string fieldName, out int index)
        {
            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i].Key == fieldName)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        /// <summary>
        /// Returns the enumerator containing the collection of fields.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<SqlManagedField> GetEnumerator()
        {
            foreach (var field in Collection)
            {
                yield return field;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
