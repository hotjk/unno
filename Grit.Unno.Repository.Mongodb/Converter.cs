using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.Mongodb
{
    public class Converter : IConverter<BsonDocument>
    {
        public Converter(bool jsonSingleChildAsCollection = false)
        {
            _jsonSingleChildAsCollection = jsonSingleChildAsCollection;
        }

        private bool _jsonSingleChildAsCollection = false;

        public BsonDocument NodeToSerializableObject(Node node)
        {
            var obj = BuildCollection(node);
            if (obj is BsonArray)
            {
                return obj[0] as BsonDocument;
            }
            return obj as BsonDocument;
        }

        private BsonValue BuildCollection(Node node)
        {
            if (!_jsonSingleChildAsCollection && node.Children.Count == 1)
            {
                return BuildDocument(node.Children[0]);
            }
            BsonArray list = new BsonArray();
            foreach (var row in node.Children)
            {
                BsonDocument doc = BuildDocument(row);
                list.Add(doc);
            }
            return list;
        }

        private BsonDocument BuildDocument(Dictionary<string, Node> nodes)
        {
            BsonDocument doc = new BsonDocument();
            foreach (var child in nodes)
            {
                if (child.Value.Leaf)
                {
                    doc.Add(child.Key, Create(child.Value.Value));
                }
                else
                {
                    doc.Add(child.Key, (BuildCollection(child.Value)));
                }
            }
            return doc;
        }

        private BsonValue Create(object value)
        {
            if(value is decimal)
            {
                double doubleValue;
                double.TryParse(value.ToString(), out doubleValue);
                return BsonValue.Create(doubleValue);
            }
            return BsonValue.Create(value);
        }

        public Node SerializableObjectToNode(BsonDocument obj, Unit unit)
        {
            Node node = new Node(unit.Key);
            BuildRow(obj, unit, node);
            return node;
        }

        private void BuildRow(BsonValue obj, Unit unit, Node refNode)
        {
            refNode.AddRow();
            foreach (var childUnit in unit.Children)
            {
                var doc = obj as BsonDocument;
                BsonValue value;
                if (doc.TryGetValue(childUnit.Key, out value))
                {
                    Node child = BuildNode(value, childUnit);
                    if (child != null)
                    {
                        refNode.AddNode(child);
                    }
                }
            }
        }

        private Node BuildNode(BsonValue token, Unit unit)
        {
            if (unit.Leaf)
            {
                if (token.IsValidDateTime)
                {
                    return new Node(unit.Key, ((DateTime)BsonTypeMapper.MapToDotNetValue(token)).ToLocalTime());
                }
                return new Node(unit.Key, BsonTypeMapper.MapToDotNetValue(token));
            }
            else
            {
                Node node = new Node(unit.Key);
                if (token.IsBsonArray)
                {
                    foreach (var row in token.AsBsonArray)
                    {
                        BuildRow(row as BsonValue, unit, node);
                    }
                }
                else if (!token.IsBsonNull)
                {
                    BuildRow(token, unit, node);
                }
                return node;
            }
        }
    }
}
