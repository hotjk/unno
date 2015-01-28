using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.Mongodb
{
    public class NodeRepository : INodeRepository
    {
        private IUnitRepository _unitRepository;
        private MongoDBOptions _options;
        private MongoProxy _proxy;
        public NodeRepository(MongoDBOptions options, IUnitRepository unitRepository)
        {
            _options = options;
            _proxy = MongoProxy.Instance(_options.ConnectionString);
            _unitRepository = unitRepository;
        }

        static NodeRepository()
        {
            BsonClassMap.RegisterClassMap<NodeWrapper>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(x => x.NodeId).SetRepresentation(BsonType.String);
                cm.GetMemberMap(x => x.UnitId).SetRepresentation(BsonType.String);
                cm.UnmapMember(x => x.Node);
            });
        }

        public NodeWrapper LoadNode(Guid id)
        {
            NodeWrapper4Mongo wrapper = _proxy.Node.FindOneAs<NodeWrapper4Mongo>(Query<NodeWrapper4Mongo>.EQ(e => e.NodeId, id));
            if (wrapper != null)
            {
                var unitWrapper = _unitRepository.LoadUnit(wrapper.UnitId);
                Converter converter = new Converter(_options.JsonSingleChildAsCollection);
                wrapper.Node = converter.SerializableObjectToNode(wrapper.Data, unitWrapper.Unit);
            }
            return wrapper;
        }

        public void SaveNode(NodeWrapper wrapper)
        {
            NodeWrapper4Mongo wrapper4Mongo = new NodeWrapper4Mongo(wrapper);
            var reload = LoadNode(wrapper.NodeId);
            if (reload != null)
            {
                if(reload.Version != wrapper.Version)
                {
                    throw new ConcurrencyException(wrapper.NodeId.ToString());
                }
                wrapper4Mongo.Id = (reload as NodeWrapper4Mongo).Id;
                wrapper4Mongo.Version = wrapper4Mongo.Version + 1;
            }
            Converter converter = new Converter(_options.JsonSingleChildAsCollection);
            wrapper4Mongo.Data = converter.NodeToSerializableObject(wrapper.Node);
            wrapper4Mongo.Node = null;
            _proxy.Node.Save(wrapper4Mongo);
        }
    }
}
