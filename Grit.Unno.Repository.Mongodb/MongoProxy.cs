using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.Mongodb
{
    public class MongoProxy
    {
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _database;
        private MongoCollection _node;
        private MongoCollection _unit;
        private static object _lockObject = new object();
        private static MongoProxy _instance = null;

        public static MongoProxy Instance(string connectionString)
        {
            if(_instance == null)
            {
                lock(_lockObject)
                {
                    if(_instance == null)
                    {
                        _instance = new MongoProxy(connectionString);
                    }
                }
            }
            return _instance;
        }


        private MongoProxy(string connectionString)
        {
            _client = new MongoClient(connectionString);
            _server = _client.GetServer();
            _database = _server.GetDatabase("unno");
            _node = _database.GetCollection<NodeWrapper>("node");
            _unit = _database.GetCollection("unit");
            _node.CreateIndex("NodeId", "UnitId");
            _unit.CreateIndex("UnitId");
        }

        public MongoCollection Node
        {
            get
            {
                return _node;
            }
        }

        public MongoCollection Unit
        {
            get
            {
                return _unit;
            }
        }
    }
}
