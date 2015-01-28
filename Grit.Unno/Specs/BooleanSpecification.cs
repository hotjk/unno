using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public class BooleanSpecification : Specification
    {
        public BooleanSpecification()
        {
            Type = SpecificationType.Boolean;
        }

        public override bool IsSatisfiedBy(Node node)
        {
            if (node.Children != null)
            {
                return false;
            }
            if(node.Value == null) 
            {
                return true;
            }
            node.Value = (node.Value.ToString().ToLower().IndexOf("true")!=-1);
            return true;
        }

        public override string Message(string name)
        {
            return string.Format("{0} 必须为布尔值");
        }
    }
}
