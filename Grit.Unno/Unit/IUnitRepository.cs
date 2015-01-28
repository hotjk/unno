using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public interface IUnitRepository
    {
        UnitWrapper LoadUnit(Guid id);
        void SaveUnit(UnitWrapper wrapper);
    }
}
