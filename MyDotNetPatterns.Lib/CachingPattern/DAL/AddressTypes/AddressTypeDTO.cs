using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDotNetPatterns.Lib.CachingPattern.DAL.AddressTypes
{
    public class AddressTypeDTO
    {
        public int AddressTypeId { get; set; }
        public string Name { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
