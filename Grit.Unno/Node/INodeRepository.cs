using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public interface INodeRepository
    {
        NodeWrapper LoadNode(Guid id);
        void SaveNode(NodeWrapper wrapper);
    }
}
