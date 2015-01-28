using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public class DecimalSpecification : Specification
    {
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }

        public DecimalSpecification() { }
        public DecimalSpecification(decimal? min = null, decimal? max = null)
        {
            Type = SpecificationType.Decimal;
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
            decimal value;
            if (!decimal.TryParse(node.Value.ToString(), out value))
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
                return string.Format("{0} 必须为金额，且取值范围从 {1} 到 {2}。", name, Min, Max);
            }
            else if (Min.HasValue)
            {
                return string.Format("{0} 必须为金额，且值应该大于 {1}。", name, Min);
            }
            else if (Max.HasValue)
            {
                return string.Format("{0} 必须为金额，且值应该小于 {1}。", name, Max);
            }
            return string.Empty;
        }

        public override void UIValidateProperties(string name, IDictionary<string, object> UIProperties)
        {
            UIProperties["data-val-number"] = string.Format("{0} 必须为金额。", name);
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
