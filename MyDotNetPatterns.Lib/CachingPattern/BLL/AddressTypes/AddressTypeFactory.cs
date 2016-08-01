using MyDotNetPatterns.Lib.CachingPattern.DAL.AddressTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDotNetPatterns.Lib.CachingPattern.BLL.AddressTypes
{
    public static class AddressTypeFactory
    {

        public static IEnumerable<AddressType> GetAddressTypes()
        {
            var databaseAddressTypes = AdressTypeDAI.GetAddressTypes();

            return databaseAddressTypes.Select(x => new AddressType() { Id = x.AddressTypeId, Name = x.Name });
        }

        public static void AddAddressType(string addressTypeName)
        {
            Guid guid = Guid.NewGuid();

            
        }
    }
}
