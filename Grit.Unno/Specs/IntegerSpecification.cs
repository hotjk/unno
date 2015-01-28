using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public class IntegerSpecification : Specification
    {
        public int? Min { get; set; }
        public int? Max { get; set; }

        public IntegerSpecification() { }
        public IntegerSpecification(int? min = null, int? max = null)
        {
            Type = SpecificationType.Integer;
            this.Min = min;
            this.Max = max;
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
            int value;
            if (!int.TryParse(node.Value.ToString(), out value))
            {
                return false;
            }
            node.Value = value;
            if (Min.HasValue && value < Min.Value)
            {
                return false;
            }
            if (Max.HasValue && value > Max.Value)
            {
                return false;
            }
            return true;
        }

        public override string Message(string name)
        {
            if (Min.HasValue && Max.HasValue)
            {
                return string.Format("{0} 必须为整数，且取值范围从 {1} 到 {2}。", name, Min.Value, Max.Value);
            }
            else if (Min.HasValue)
            {
                return string.Format("{0} 必须为整数，且值应该大于 {1}。", name, Min.Value);
            }
            else if (Max.HasValue)
            {
                return string.Format("{0} 必须为整数，且值应该小于 {1}。", name, Max.Value);
            }
            return string.Empty;
        }

        public override void UIValidateProperties(string name, IDictionary<string, object> UIProperties)
        {
            UIProperties["data-val-number"] = string.Format("{0} 必须为数字。", name);
            if (Min.HasValue && Max.HasValue)
            {
                UIProperties["data-val-range-min"] = Min.Value;
                UIProperties["data-val-range-max"] = Max.Value;
                UIProperties["data-val-range"] = string.Format("{0} 的取值范围从 {1} 到 {2}。", name, Min, Max);
            }
            else if (Min.HasValue)
            {
                UIProperties["data-val-range-min"] = Min.Value;
                UIProperties["data-val-range"] = string.Format("{0} 的值应该大于 {1}。", Min);
            }
            else if (Max.HasValue)
            {
                UIProperties["data-val-range-max"] = Max.Value;
                UIProperties["data-val-range"] = string.Format("{0} 的值应该小于 {1}。", Max);
            }
            base.UIValidateProperties(name, UIProperties);
        }
    }
}
