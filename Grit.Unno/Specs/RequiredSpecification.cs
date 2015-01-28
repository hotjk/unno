using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public class RequiredSpecification : Specification
    {
        public RequiredSpecification()
        {
            Type = SpecificationType.Required;
        }

        public override bool IsSatisfiedBy(Node node)
        {
            if((node.Value == null || node.Value.ToString() == string.Empty) && node.Children == null)
            {
                return false;
            }
            return true;
        }

        public override string Message(string name)
        {
            return string.Format("{0} 必须填写。", name);
        }

        public override void UIValidateProperties(string name, IDictionary<string, object> UIProperties)
        {
            UIProperties["data-val-required"] = string.Format("{0} 必须填写。", name);
            base.UIValidateProperties(name, UIProperties);
        }
    }
}
