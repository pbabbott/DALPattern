using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MyDotNetPatterns.Lib.DALPattern.Core
{
    public static class Hydrator<T> where T : new()
    {
        public static T Hydrate(DataRow dataRow, bool thowErrorIfAttributeNotFound = false)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            return Hydrate(dataRow, properties, GetAttributes(properties), thowErrorIfAttributeNotFound);
        }

        public static List<T> Hydrate(DataTable dt, bool thowErrorIfAttributeNotFound = false)
        {
            List<T> results = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (DataRow row in dt.Rows)
            {
                results.Add(Hydrate(row, properties, GetAttributes(properties), thowErrorIfAttributeNotFound));
            }
            return results;
        }

        private static T Hydrate(DataRow dataRow, PropertyInfo[] properties, DataStoreFieldAttribute[] attributes, bool thowErrorIfAttributeNotFound = false)
        {
            T result = new T();

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                DataStoreFieldAttribute attribute = attributes[i];

                if (attribute == null && thowErrorIfAttributeNotFound)
                {
                    throw new Exception(String.Format("No DataStoreFieldAttribute found for property {0}", property.Name));
                }
                else if (attribute != null)
                {
                    object dataRowValue = null;
                    object propertyValue;
                    try
                    {
                        dataRowValue = dataRow[attribute.FieldName];
                    }
                    catch (ArgumentException aex)
                    {
                        throw new ArgumentException(String.Format("Could not find field '{0}' in the DataRow for property '{1}'.  Are you sure the DataStoreField name matches the SQL column name precisely?", attribute.FieldName, property.Name), aex);
                    }

                    try
                    {
                        // Value from the database is DBNull and developer has not specified a default null value
                        if (dataRowValue == DBNull.Value && attribute.DefaultNullValue == null)
                        {
                            // create a new instance of the value type (if it's not a nullable type)
                            if (property.PropertyType.IsValueType && Nullable.GetUnderlyingType(property.PropertyType) == null)
                            {
                                propertyValue = Activator.CreateInstance(property.PropertyType);
                            }
                            else if (property.PropertyType == typeof(string))
                            {
                                propertyValue = String.Empty;
                            }
                            else
                            {
                                propertyValue = null;
                            }
                        }
                        // Value from the database is DBNull, and developer has specified a default null value
                        else if (dataRowValue == DBNull.Value && attribute.DefaultNullValue != null)
                        {
                            propertyValue = attribute.DefaultNullValue;
                        }
                        // Value from the database is not null, just use it.
                        else
                        {
                            propertyValue = dataRowValue;
                        }

                        property.SetValue(result, propertyValue);
                    }
                    catch (ArgumentException aex)
                    {
                        string message = String.Format("DataRow[\"{0}\"] is of type '{1}' but property '{2}' is of type '{3}'. Please change your DataStoreField type to be of type '{1}'.",
                            attribute.FieldName, dataRowValue.GetType(), property.Name, property.PropertyType);
                        throw new InvalidCastException(message, aex);
                    }
                }
            }
            return result;
        }

        private static DataStoreFieldAttribute[] GetAttributes(PropertyInfo[] properties)
        {
            List<DataStoreFieldAttribute> attributes = new List<DataStoreFieldAttribute>();

            foreach (PropertyInfo property in properties)
            {
                attributes.Add((DataStoreFieldAttribute)property.GetCustomAttributes(typeof(DataStoreFieldAttribute), false).SingleOrDefault());
            }

            return attributes.ToArray();
        }
    }
}