using System.Reflection.PortableExecutable;

namespace NTDLS.SqlManagedConnectivity
{
    /// <summary>
    /// Encapsulates a field and its value.
    /// </summary>
    public class SqlManagedFieldValue
    {
        /// <summary>
        /// The field for which the value is associated.
        /// </summary>
        public SqlManagedField Field { get; private set; }

        /// <summary>
        /// The value of the field.
        /// </summary>
        public object Value { get; private set; }

        internal SqlManagedFieldValue(SqlManagedField field, object value)
        {
            Field = field;
            Value = value;
        }

        /// <summary>
        /// Returns the best string representation of the field value.
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return Value?.ToString();
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public T? As<T>()
        {
            if (Value == null || Value == DBNull.Value)
            {
                return default;
            }

            return (T?)Convert.ChangeType(Value, typeof(T));
        }

        /// <summary>
        /// Retrieves the value of the column, converts it to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public T AsNotNull<T>()
        {
            if (Value == null || Value == DBNull.Value)
            {
                throw new Exception($"The value was unexpectedly null.");
            }

            return (T)Convert.ChangeType(Value, typeof(T));
        }
    }
}
