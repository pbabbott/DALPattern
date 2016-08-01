using MyDotNetPatterns.Lib.DALPattern.Core;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace MyDotNetPatterns.Lib.DALPattern.People
{
    public static class PersonDAI
    {
        private static DALFunctions _DALFunctions
            = new DALFunctions(ConfigurationManager.ConnectionStrings["AdventureWorks"].ToString());

        public static List<PersonDTO> GetPeople()
        {
            List<PersonDTO> results = new List<PersonDTO>();

            string sql = "SELECT [BusinessEntityId], [PersonType], [NameStyle], [Title] FROM [Person].[Person]";
            DataTable dt = _DALFunctions.ExecuteRawSqlReturnDT(sql);

            foreach (DataRow dr in dt.Rows)
            {
                results.Add(HydrateEmployeeDTO(dr));
            }

            return results;
        }

        public static PersonDTO GetPerson(int businessEntityId)
        {
            string sql = "SELECT [BusinessEntityId], [PersonType], [NameStyle], [Title] FROM [Person].[Person] where [BusinessEntityId] = @businessEntityId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@businessEntityId", businessEntityId);

            DataTable dt = _DALFunctions.ExecuteRawSqlReturnDT(sql, parameters);
            return HydrateEmployeeDTO(dt.Rows[0]);
        }

        private static PersonDTO HydrateEmployeeDTO(DataRow dr)
        {
            PersonDTO result = new PersonDTO();
            result.BusinessEntityId = dr.Field<int>("BusinessEntityID");
            result.PersonType = dr.Field<string>("PersonType");
            result.NameStyle = dr.Field<bool>("NameStyle");
            result.Title = dr.Field<string>("Title");
            return result;
        }
    }
}