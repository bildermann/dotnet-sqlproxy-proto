using System.Collections.Generic;
using Newtonsoft.Json;

namespace dotnet_sandbox.Models
{

    public class SQLRequest
    {

        public SQLRequest()
        {
            this.Limit = 100;
        }

        public string[] Select { get; set; }
        public string From { get; set; }
        public IList<SQLWhere> Where { get; set; }

        public string[] OrderBy { get; set; }

        public int Limit { get; set; }
        public int Offset { get; set; }
    }

}