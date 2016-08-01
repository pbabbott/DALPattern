using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDotNetPatterns.Lib.DALPattern.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataStoreFieldAttribute : Attribute
    {
        public string FieldName { get { return _FieldName; } }
        private string _FieldName = null;

        public object DefaultNullValue { get { return _DefaultNullValue; } }
        private object _DefaultNullValue = null;

        public DataStoreFieldAttribute(string fieldName)
        {
            this._FieldName = fieldName;
        }

        public DataStoreFieldAttribute(string fieldName, object defaultNullValue)
        {
            this._FieldName = fieldName;
            this._DefaultNullValue = defaultNullValue;
        }

    }
}
