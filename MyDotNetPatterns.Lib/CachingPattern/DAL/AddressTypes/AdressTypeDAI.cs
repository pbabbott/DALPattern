using MyDotNetPatterns.Lib.DALPattern.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDotNetPatterns.Lib.CachingPattern.DAL.AddressTypes
{
    public static class AdressTypeDAI
    {
        private static DALFunctions _DALFunctions
            = new DALFunctions(ConfigurationManager.ConnectionStrings["AdventureWorks"].ToString());

        public static List<AddressTypeDTO> GetAddressTypes()
        {
            List<AddressTypeDTO> results = new List<AddressTypeDTO>();

            string sql = "SELECT [AddressTypeId], [Name], [rowguid], [ModifiedDate] FROM [Person].[AddressType]";
            DataTable dt = _DALFunctions.ExecuteRawSqlReturnDT(sql);

            foreach (DataRow dr in dt.Rows)
            {
                results.Add(HydrateAddressTypeDTO(dr));
            }

            return results;
        }

        public static void CreateAddressType(string name, Guid rowGuid, DateTime modifiedDate)
        {
            //throw new NotImplementedException("insert into the [Person].[AddressType] table");
            return;
        }

        private static AddressTypeDTO HydrateAddressTypeDTO(DataRow dr)
        {
            AddressTypeDTO result = new AddressTypeDTO();
            result.AddressTypeId = dr.Field<int>("AddressTypeId");
            result.Name = dr.Field<string>("Name");
            result.RowGuid = dr.Field<Guid>("rowguid");
            result.ModifiedDate = dr.Field<DateTime>("ModifiedDate");

            return result;
        }
    }
}
