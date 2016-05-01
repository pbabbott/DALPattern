using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MyDotNetPatterns.Lib.DALPattern.Core
{
    public class DALFunctions
    {
        private string _ConnectionString { get; set; }

        private const int _CommandTimeOut = 600;

        public DALFunctions(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #region Private Methods

        private SqlConnection GetSqlConnection(bool open = true)
        {
            SqlConnection result = new SqlConnection(_ConnectionString);
            if (open)
            {
                result.Open();
            }
            return result;
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
    }
}