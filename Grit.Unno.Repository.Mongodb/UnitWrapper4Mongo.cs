using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.Mongodb
{
    public class UnitWrapper4Mongo : UnitWrapper
    {
        public ObjectId Id { get; set; }
        
        public UnitWrapper4Mongo() { }
        public UnitWrapper4Mongo(UnitWrapper wrapper)
        {
            this.UnitId = wrapper.UnitId;
            this.Name = wrapper.Name;
            this.UpdateAt = wrapper.UpdateAt;
            this.Version = wrapper.Version;
            this.Unit = wrapper.Unit;
        }
    }
}
