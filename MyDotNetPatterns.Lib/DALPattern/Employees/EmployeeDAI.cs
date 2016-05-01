using MyDotNetPatterns.Lib.DALPattern.Core;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace MyDotNetPatterns.Lib.DALPattern.Employees
{
    public static class EmployeeDAI
    {
        private static DALFunctions _DALFunctions 
            = new DALFunctions(ConfigurationManager.ConnectionStrings["AdventureWorks"].ToString());

        public static List<EmployeeDTO> GetEmployees()
        {
            List<EmployeeDTO> results = new List<EmployeeDTO>();

            string sql = "SELECT [employee_id], [first_name], [last_name], [email] FROM [dbo].[Employee]";
            DataTable dt = _DALFunctions.ExecuteRawSqlReturnDT(sql);

            foreach (DataRow dr in dt.Rows)
            {
                results.Add(HydrateEmployeeDTO(dr));
            }

            return results;
        }

        public static EmployeeDTO GetEmployee(int employeeId)
        {
            string sql = "SELECT [employee_id], [first_name], [last_name], [email] FROM [dbo].[Employee] where [employee_id] = @employee_id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@employee_id", employeeId);

            DataTable dt = _DALFunctions.ExecuteRawSqlReturnDT(sql, parameters);
            return HydrateEmployeeDTO(dt.Rows[0]);
        }

        private static EmployeeDTO HydrateEmployeeDTO(DataRow dr)
        {
            EmployeeDTO result = new EmployeeDTO();
            result.EmployeeId = dr.Field<int>("employee_id");
            result.FirstName = dr.Field<string>("first_name");
            result.LastName = dr.Field<string>("last_name");
            result.EmailAddress = dr.Field<string>("email");
            return result;
        }
    }
}