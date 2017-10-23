using System.Collections.Generic;

namespace dotnet_sandbox.Models
{
    public class SQLResponse
    {

        public IList<Dictionary<string, object>> ResultSet { get; set; }
        public SQLRequest Next { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }


    }
}