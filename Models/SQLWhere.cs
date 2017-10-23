using System.Collections.Generic;

namespace dotnet_sandbox.Models {

    public class SQLWhere {
        public string FieldName {get;set;}
        public string Operator {get;set;}
        public IList<SQLValue> Values {get;set;}
    }
}