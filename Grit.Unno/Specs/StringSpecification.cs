using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public class StringSpecification : Specification
    {
        public int? Min { get; set; }
        public int? Max { get; set; }

        public StringSpecification() { }
        public StringSpecification(int? min = null, int? max = null)
        {
            Type = SpecificationType.String;
            this.Min = min;
            this.Max = max;
            if(Min == null && Max == null)
            {
                throw new ArgumentException("StringSpecification min and max can not both empty.");
            }
        }

        public override bool IsSatisfiedBy(Node node)
        {
            if (node.Children != null)
            {
                return false;
            }
            if (node.Value == null)
            {
                return true;
            }
            string value = node.Value.ToString();
            node.Value = value;
            if (Min.HasValue && value.Length < Min.Value)
            {
                return false;
            }
            if (Max.HasValue && value.Length > Max.Value)
            {
                return false;
            }
            return true;
        }

        public override string Message(string name)
        {
            if (Min.HasValue && Max.HasValue)
            {
                return string.Format("{0} 的长度范围从 {1} 到 {2}。", name, Min.Value, Max.Value);
            }
            else if (Min.HasValue)
            {
                return string.Format("{0} 的长度应该大于 {1}。", name, Min.Value);
            }
            else if (Max.HasValue)
            {
                return string.Format("{0} 的长度应该小于 {1}。", name, Min.Value);
            }
            return string.Empty;
        }

        public override void UIValidateProperties(string name, IDictionary<string, object> UIProperties)
        {
            if (Min.HasValue && Max.HasValue)
            {
                UIProperties["data-val-length-min"] = Min.Value;
                UIProperties["data-val-length-max"] = Max.Value;
                UIProperties["data-val-length"] = string.Format("{0} 的长度范围从 {1} 到 {2}。", name, Min.Value, Max.Value);
            }
            else if (Min.HasValue)
            {
                UIProperties["data-val-length-min"] = Min.Value;
                UIProperties["data-val-length"] = string.Format("{0} 的长度应该大于 {1}。", Min.Value);
            }
            else if (Max.HasValue)
            {
                UIProperties["data-val-length-max"] = Max.Value;
                UIProperties["data-val-length"] = string.Format("{0} 的长度应该小于 {1}。", Min.Value);
            }
            base.UIValidateProperties(name, UIProperties);
        }
    }
}
