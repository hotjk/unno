using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.Mongodb
{
    public class NodeWrapper4Mongo : NodeWrapper
    {
        public ObjectId Id { get; set; }
        public BsonDocument Data { get; set; }

        public NodeWrapper4Mongo() { }
        public NodeWrapper4Mongo(NodeWrapper wrapper)
        {
            this.NodeId = wrapper.NodeId;
            this.UnitId = wrapper.UnitId;
            this.Name = wrapper.Name;
            this.UpdateAt = wrapper.UpdateAt;
            this.Version = wrapper.Version;
            this.Node = wrapper.Node;
        }
    }
}
