using Grit.Unno.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public class Unit
    {
        public Unit(string key, string name)
        {
            key.EnsoureRegularKey();
            this.Key = key;
            this.Name = name;
        }

        #region Properties

        public string Key { get; private set; }
        /// <summary>
        /// Validation message need name
        /// </summary>
        public string Name { get; private set; }
        public List<Unit> Children { get; set; }
        public List<Specification> Specifications { get; set; }
        public bool Leaf
        {
            get
            {
                return (Children == null || Children.Count == 0);
            }
        }

        #endregion

        #region Add

        public Unit AddSpecification(Specification spec)
        {
            if(Specifications == null)
            {
                Specifications = new List<Specification>();
            }
            Specifications.Add(spec);
            return this;
        }

        public Unit AddChild(Unit child)
        {
            if(Children == null)
            {
                Children = new List<Unit>();
            }
            Children.Add(child);
            return this;
        }

        #endregion

        #region Find

        public Unit Find(string path, ref string name)
        {
            var list = new List<string>();
            var parts = path.Split(UIConstants.SEPRATOR_NODE);
            foreach (var part in parts)
            {
                var tuple = part.Split(UIConstants.SEPRATOR_INDEX);
                list.Add(tuple[0]);
            }
            return Find(list, ref name);
        }

        private Unit Find(IEnumerable<string> path, ref string name)
        {
            var first = path.First();
            path = path.Skip(1);
            if (Key == first)
            {
                name = (name == string.Empty?Name:(name + "." + Name));
                if (path.Count() == 0)
                {
                    return this;
                }

                if(Children != null)
                {
                    foreach (var item in Children)
                    {
                        var found = item.Find(path, ref name);
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

        public IDictionary<string, object> UIValidateProperties(string name)
        {
            IDictionary<string, object> list = new Dictionary<string, object>();

            if(Specifications != null)
            {
                foreach(var spec in Specifications)
                {
                    spec.UIValidateProperties(name, list);
                }
            }

            if (list.Count() != 0)
            {
                list["data-val"] = "true";
            }
            return list;
        }
    }
}
