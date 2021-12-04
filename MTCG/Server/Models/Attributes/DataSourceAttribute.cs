using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models.Attributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class DataSourceAttribute : Attribute {
        public string TableName { get; private set; }

        public DataSourceAttribute(string tableName) {
            TableName = tableName;
        }
    }
}
