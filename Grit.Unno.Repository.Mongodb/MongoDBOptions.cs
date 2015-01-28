using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.Mongodb
{
    public class MongoDBOptions
    {
        public MongoDBOptions(string connectionString, 
            bool jsonSingleChildAsCollection = false)
        {
            this.ConnectionString = connectionString;
            this.JsonSingleChildAsCollection = jsonSingleChildAsCollection;
        }

        public string ConnectionString { get; private set; }
        public bool JsonSingleChildAsCollection { get; private set; }
    }
}
