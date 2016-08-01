using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace MyDotNetPatterns.Lib.DALPattern.Core
{
    public class DALFunctions
    {
        private string _ConnectionString { get; set; }

        private const int _CommandTimeOut = 600;

        private DbProviderFactory _DbProviderFactory { get; set; }

        public DALFunctions(string connectionString)
        {
            this._ConnectionString = connectionString;
            this._DbProviderFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
        }

        #region Private Methods

        private SqlConnection GetSqlConnection()
        {
            SqlConnection connection = (SqlConnection)this._DbProviderFactory.CreateConnection();
            connection.ConnectionString = _ConnectionString;
            return connection;
        }

        private SqlCommand GetSqlCommand(SqlConnection connection, CommandType commandType, string commandText)
        {
            return new SqlCommand(commandText, connection)
            {
                CommandType = commandType,
                CommandTimeout = _CommandTimeOut
            };
        }

        private DataSet GetDataSetFromCommand(SqlCommand command)
        {
            DataSet result = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(result);
            return result;
        }

        private DataTable GetDataTableFromCommand(SqlCommand command)
        {
            DataTable result = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(result);
            return result;
        }

        private static void AddParameters(SqlParameterCollection parameterCollection, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    parameterCollection.Add(new SqlParameter(parameter.Key, parameter.Value));
                }
            }
        }

        #endregion Private Methods

        #region Public Methods

        public DataTable GetSchemaFromTableName(string sqlTableName)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                string[] restrictions = new string[4]
                {
                    null,
                    null,
                    sqlTableName,
                    null
                };

                DataTable schemaTable = connection.GetSchema("Columns", restrictions);
                DataTable result = new DataTable();
                foreach (DataRow dr in schemaTable.AsEnumerable())
                {
                    string columnName = dr.Field<string>("Column_Name");
                    string sqlType = dr.Field<string>("Data_Type");

                    result.Columns.Add(new DataColumn(columnName, TransformType(sqlType)));
                }

                return result;
            }
        }

        public DataSet ExecuteSprocReturnDS(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                using (SqlCommand command = GetSqlCommand(connection, CommandType.StoredProcedure, storedProcedure))
                {
                    AddParameters(command.Parameters, parameters);
                    return GetDataSetFromCommand(command);
                }
            }
        }

        public DataTable ExecuteSprocReturnDT(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                using (SqlCommand command = GetSqlCommand(connection, CommandType.StoredProcedure, storedProcedure))
                {
                    AddParameters(command.Parameters, parameters);
                    return GetDataTableFromCommand(command);
                }
            }
        }

        public object ExecuteSprocReturnObject(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                using (SqlCommand command = GetSqlCommand(connection, CommandType.StoredProcedure, storedProcedure))
                {
                    AddParameters(command.Parameters, parameters);
                    return command.ExecuteScalar();
                }
            }
        }

        public void ExecuteSproc(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                using (SqlCommand command = GetSqlCommand(connection, CommandType.StoredProcedure, storedProcedure))
                {
                    AddParameters(command.Parameters, parameters);
                    command.ExecuteNonQuery();
                }
            }
        }

        public DataSet ExecuteRawSqlReturnDS(string rawSql, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                using (SqlCommand command = GetSqlCommand(connection, CommandType.Text, rawSql))
                {
                    AddParameters(command.Parameters, parameters);
                    return GetDataSetFromCommand(command);
                }
            }
        }

        public DataTable ExecuteRawSqlReturnDT(string rawSql, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                using (SqlCommand command = GetSqlCommand(connection, CommandType.Text, rawSql))
                {
                    AddParameters(command.Parameters, parameters);
                    return GetDataTableFromCommand(command);
                }
            }
        }

        public object ExecuteRawSqlReturnObject(string rawSql, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                using (SqlCommand command = GetSqlCommand(connection, CommandType.Text, rawSql))
                {
                    AddParameters(command.Parameters, parameters);
                    return command.ExecuteScalar();
                }
            }
        }

        public void ExecRawSql(string rawSQL, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                using (SqlCommand command = GetSqlCommand(connection, CommandType.Text, rawSQL))
                {
                    AddParameters(command.Parameters, parameters);
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion Public Methods

        #region Static

        private static Type TransformType(string sqlTypeName)
        {
            switch (sqlTypeName.ToLower())
            {
                case "int":
                case "smallint":
                case "tinyint":
                    return typeof(int);

                case "varchar":
                    return typeof(string);

                case "bit":
                    return typeof(bool);

                case "date":
                case "datetime":
                    return typeof(DateTime);

                case "uniqueidentifier":
                    return typeof(Guid);

                case "decimal":
                case "money":
                case "float":
                    return typeof(decimal);

                default:
                    throw new Exception(String.Format("Unknown SQL type '{0}' encountered. Please add to this switch/case", sqlTypeName));
            }
        }

        #endregion Static
    }
}