using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grit.Unno;

namespace Grit.Unno.Specs
{
    public class DateTimeSpecification : Specification
    {
        public DateTime? Min { get; set; }
        public DateTime? Max { get; set; }

        public DateTimeSpecification() { }
        public DateTimeSpecification(DateTime? min = null, DateTime? max = null)
        {
            Type = SpecificationType.DateTime;
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
            DateTime value;
            if(!DateTime.TryParse(node.Value.ToString(), out value))
            {
                return false;
            }
            node.Value = value;
            if(Min.HasValue && value < Min.Value)
            {
                return false;
            }
            if(Max.HasValue && value > Max.Value)
            {
                return false;
            }
            return true;
        }

        public override string Message(string name)
        {
            if (Min.HasValue && Max.HasValue)
            {
                return string.Format("{0} 必须为日期时间，且取值范围从 {1} 到 {2}。", name, Min.ToDateString(), Max.ToDateString());
            }
            else if (Min.HasValue)
            {
                return string.Format("{0} 必须为日期时间，且值应该大于 {1}。", name, Min.ToDateString());
            }
            else if (Max.HasValue)
            {
                return string.Format("{0} 必须为日期时间，且值应该小于 {1}。", name, Max.ToDateString());
            }
            return string.Empty;
        }

        public override void UIValidateProperties(string name, IDictionary<string, object> UIProperties)
        {
            UIProperties["data-val-date"] = string.Format("{0} 必须为日期时间。", name);
            base.UIValidateProperties(name, UIProperties);
        }
    }
}
