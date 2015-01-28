using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.Mongodb
{
    public class UnitRepository : IUnitRepository
    {
        private MongoDBOptions _options;
        private MongoProxy _proxy;

        public UnitRepository()
        {
            MongoDBOptions options = new MongoDBOptions("mongodb://localhost", false);
            _options = options;
            _proxy = MongoProxy.Instance(_options.ConnectionString);
        }
        public UnitRepository(MongoDBOptions options)
        {
            _options = options;
            _proxy = MongoProxy.Instance(_options.ConnectionString);
        }
        static UnitRepository()
        {
            BsonClassMap.RegisterClassMap<UnitWrapper>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(x => x.UnitId).SetRepresentation(BsonType.String);
            });
            BsonClassMap.RegisterClassMap<Specs.Specification>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
            });
            BsonClassMap.RegisterClassMap<Specs.BooleanSpecification>();
            BsonClassMap.RegisterClassMap<Specs.CompositeSpecification>();
            BsonClassMap.RegisterClassMap<Specs.DateTimeSpecification>();
            BsonClassMap.RegisterClassMap<Specs.DecimalSpecification>();
            BsonClassMap.RegisterClassMap<Specs.IntegerSpecification>();
            BsonClassMap.RegisterClassMap<Specs.LookupSpecification>();
            BsonClassMap.RegisterClassMap<Specs.RequiredSpecification>();
            BsonClassMap.RegisterClassMap<Specs.StringSpecification>();
        }

        public UnitWrapper LoadUnit(Guid id)
        {
            UnitWrapper4Mongo wrapper = _proxy.Unit.FindOneAs<UnitWrapper4Mongo>(Query<UnitWrapper4Mongo>.EQ(e => e.UnitId, id));
            return wrapper;
        }

        public void SaveUnit(UnitWrapper wrapper)
        {
            UnitWrapper4Mongo wrapper4Mongo = new UnitWrapper4Mongo(wrapper);
            var reload = LoadUnit(wrapper.UnitId);
            if(reload != null)
            {
                wrapper4Mongo.Id = (reload as UnitWrapper4Mongo).Id;
            }
            _proxy.Unit.Save(wrapper4Mongo);
        }
    }
}
