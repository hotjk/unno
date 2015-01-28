
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public enum SpecificationType
    {
        Required = 1,
        Boolean = 2,
        DateTime = 3,
        Decimal = 4,
        Integer = 5,
        String = 6,
        Lookup = 10,
        Composite = 20,
    }
}
