using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public class UnitWrapper
    {
        public Guid UnitId { get; set; }
        public string Name { get; set; }
        public DateTime UpdateAt { get; set; }
        public int Version { get; set; }
        public Unit Unit { get; set; }
    }
}
