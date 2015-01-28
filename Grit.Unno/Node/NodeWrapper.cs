using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public class NodeWrapper
    {
        public Guid NodeId { get; set; }
        public Guid UnitId { get; set; }
        public string Name { get; set; }
        public DateTime UpdateAt { get; set; }
        public int Version { get; set; }
        public Node Node { get; set; }
    }
}
