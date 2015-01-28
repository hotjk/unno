using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public interface IStructureRepository
    {
        string GetUnitScript();
        IList<string> GetNodeScript(Guid id);
    }
}
