using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public abstract class Specification
    {
        public SpecificationType Type { get; set; }
        public abstract bool IsSatisfiedBy(Node node);
        public abstract string Message(string name);
        public virtual void UIValidateProperties(string name, IDictionary<string, object> UIProperties)
        {
        }
    }
}
