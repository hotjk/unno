using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.File
{
    public class Converter : IConverter<object>
    {
        public Converter(bool jsonSingleChildAsCollection = false)
        {
            _jsonSingleChildAsCollection = jsonSingleChildAsCollection;
        }

        private bool _jsonSingleChildAsCollection = false;

        #region Node -> dynamic

        public dynamic NodeToSerializableObject(Node node)
        {
            var obj = BuildCollection(node);
            if(obj is Array)
            {
                return obj[0];
            }
            return obj;
        }
        
        private dynamic BuildCollection(Node node)
        {
            if (!_jsonSingleChildAsCollection && node.Children.Count == 1)
            {
                return BuildDynamic(node.Children[0]);
            }
            dynamic[] list = new dynamic[node.Children.Count];
            int i = 0;
            foreach (var row in node.Children)
            {
                list[i++] = BuildDynamic(row);
            }
            return list;
        }

        private dynamic BuildDynamic(Dictionary<string, Node> nodes)
        {
            dynamic dyn = new ExpandoObject();
            var dict = dyn as IDictionary<string, object>;
            foreach (var child in nodes)
            {
                if (child.Value.Leaf)
                {
                    dict[child.Key] = child.Value.Value;
                }
                else
                {
                    dict[child.Key] = BuildCollection(child.Value);
                }
            }
            return dyn;
        }

        #endregion

        #region dynamic -> Node

        public Node SerializableObjectToNode(dynamic obj, Unit unit)
        {
            if(!(obj is JObject))
            {
                throw new Exception("obj MUST be Newtonsoft.Json.Linq.JObject");
            }
            Node node = new Node(unit.Key);
            BuildRow(obj, unit, node);
            return node;
        }

        private void BuildRow(JObject obj, Unit unit, Node refNode)
        {
            refNode.AddRow();
            foreach (var childUnit in unit.Children)
            {
                JToken token;
                obj.TryGetValue(childUnit.Key, out token);
                Node child = BuildNode(token, childUnit);
                if (child != null)
                {
                    refNode.AddNode(child);
                }
            }
        }

        private Node BuildNode(JToken token, Unit unit)
        {
            if (unit.Leaf)
            {
                return new Node(unit.Key, (token as JValue).Value);
            }
            else
            {
                Node node = new Node(unit.Key);
                if(token is JArray)
                {
                    foreach (var row in token)
                    {
                        BuildRow(row as JObject, unit, node);
                    }
                }
                else if((token as JObject) != null)
                {
                    BuildRow(token as JObject, unit, node);
                }
                return node;
            }
        }

        #endregion
    }
}
