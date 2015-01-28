using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public class Node
    {
        public Node(string key, object value = null)
        {
            key.EnsoureRegularKey();
            this.Key = key;
            this.Value = value;
        }

        private int index = -1;

        #region Properties

        public string Key { get; private set; }
        public object Value { get; set; }
        public List<Dictionary<string, Node>> Children {get; set;}
        public bool Leaf
        {
            get
            {
                return (Children == null || Children.Count == 0);
            }
        }

        #endregion

        #region UI

        public Unit Unit { get; set; }
        public Node Parent { get; private set; }

        public string UIInputName
        {
            get
            {
                if(this.Parent==null)
                {
                    return string.Format("{0}-{1}", Key, index);
                }
                return string.Format("{0}_{1}-{2}", Parent.UIInputName, Key, index);
            }
        }

        public string UIDispayName
        {
            get
            {
                if(this.Parent == null)
                {
                    return Unit.Name;
                }
                return string.Format("{0}.{1}", Parent.UIDispayName, Unit.Name);
            }
        }

        #endregion

        #region Add

        public Node AddRow()
        {
            if(Children == null)
            {
                Children = new List<Dictionary<string, Node>>();
            }
            Children.Add(new Dictionary<string, Node>());
            index++;
            return this;
        }

        public Node AddNode(Node node)
        {
            if(index == -1 || Children == null || Children.Count < index + 1)
            {
                throw new ApplicationException("MUST add row before add node.");
            }
            node.index = index;
            node.Parent = this;
            Children[index][node.Key] = node;
            return this;
        }

        #endregion

        #region Find

        public Node Find(string path)
        {
            var tuples = new List<Tuple<string, int>>();
            var parts = path.Split(UIConstants.SEPRATOR_NODE);
            foreach (var part in parts)
            {
                var tuple = part.Split(UIConstants.SEPRATOR_INDEX);
                tuples.Add(new Tuple<string, int>(tuple[0], int.Parse(tuple[1])));
            }
            return Find(tuples);
        }

        private Node Find(IEnumerable<Tuple<string, int>> refPath)
        {
            var tuple = refPath.First();
            refPath = refPath.Skip(1);
            if (Key == tuple.Item1)
            {
                if (refPath.Count() == 0)
                {
                    return this;
                }

                if(Children != null)
                {
                    foreach (var item in Children[tuple.Item2])
                    {
                        var found = item.Value.Find(refPath);
                        if (found != null)
                        {
                            return found;
                        }
                    }
                }
                return null;
            }
            return null;
        }

        #endregion

        #region UI Parse

        /// <summary>
        /// Count out the node children number.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="keys"></param>
        /// <param name="refDictionary"></param>
        /// <param name="refPrefix"></param>
        public static void Count(Unit unit, IEnumerable<string> keys, IDictionary<string, IList<string>> refDictionary, string refPrefix = "")
        {
            refPrefix = refPrefix + unit.Key + "-";
            refDictionary[refPrefix] = keys.Where(n => n.StartsWith(refPrefix))
                .Select(n =>
                    n.IndexOf('_', refPrefix.Length) < 0 ? n.Substring(refPrefix.Length) : n.Substring(refPrefix.Length, (n.IndexOf('_', refPrefix.Length) - refPrefix.Length)))
                .Distinct().ToList();
            if (unit.Children != null)
            {
                foreach (Unit child in unit.Children)
                {
                    foreach (string id in refDictionary[refPrefix])
                    {
                        Count(child, keys, refDictionary, refPrefix + id + "_");
                    }
                }
            }
        }

        public static Node Parse(Unit unit, NameValueCollection collection, IDictionary<string, IList<string>> dict, string refPrefix = "")
        {
            Node node = new Node(unit.Key);
            node.Unit = unit;
            IList<string> ids;
            refPrefix = refPrefix + unit.Key;
            if (dict.TryGetValue(refPrefix + "-", out ids))
            {
                if (unit.Children == null)
                {
                    if(ids.Count == 1)
                    {
                        node.Value = collection[refPrefix + "-" + ids[0]];
                    }
                }
                else
                {
                    foreach (string id in ids)
                    {
                        node.AddRow();
                        foreach (Unit child in unit.Children)
                        {
                            node.AddNode(Node.Parse(child, collection, dict, refPrefix + "-" + id + "_"));
                        }
                    }
                }
            }
            return node;
        }

        public void Validate(IDictionary<string, string> refErrors)
        {
            if(Unit.Specifications!=null)
            {
                foreach (var spec in Unit.Specifications)
                {
                    if(spec.IsSatisfiedBy(this) == false)
                    {
                        refErrors[UIInputName] = spec.Message(UIDispayName);
                    }
                }
            }

            if(this.Children != null)
            {
                foreach(var row in Children)
                {
                    foreach(var item in row)
                    {
                        item.Value.Validate(refErrors);
                    }
                }
            }
        }

        #endregion
    }
}
