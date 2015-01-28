using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.File
{
    public class NodeWrapper4File : NodeWrapper
    {
        public dynamic Data { get; set; }

        public NodeWrapper4File() { }
        public NodeWrapper4File(NodeWrapper wrapper)
        {
            this.NodeId = wrapper.NodeId;
            this.UnitId = wrapper.UnitId;
            this.Name = wrapper.Name;
            this.UpdateAt = wrapper.UpdateAt;
            this.Version = wrapper.Version;
            this.Node = wrapper.Node;
        }
    }
}
