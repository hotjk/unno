using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.MySql
{
    public class UnitRow
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }

    public class SpecRow
    {
        public int Id { get; set; }
        public Specs.SpecificationType Type { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Options { get; set; }

        public Nullable<T> GetMin<T>() where T:struct
        {
            if (string.IsNullOrEmpty(Min)) return null;
            T v = (T)Convert.ChangeType(Min, typeof(T));
            return v;
        }
        public Nullable<T> GetMax<T>() where T : struct
        {
            if (string.IsNullOrEmpty(Max)) return null;
            T v = (T)Convert.ChangeType(Max, typeof(T));
            return v;
        }
    }
}
