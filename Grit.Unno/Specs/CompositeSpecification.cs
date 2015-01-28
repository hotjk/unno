using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public class CompositeSpecification : Specification
    {
        /// <summary>
        /// Json.net need default constructor
        /// </summary>
        public CompositeSpecification() { }
        public CompositeSpecification(int? min = null, int? max = null)
        {
            Type = SpecificationType.Composite;
            this.Min = min;
            this.Max = max;
        }

        public int? Min { get; set; }
        public int? Max { get; set; }

        public override bool IsSatisfiedBy(Node node)
        {
            if (node.Value != null)
            {
                return false;
            }
            if(node.Children == null || node.Children.Count == 0)
            {
                return true;
            }
            if (Min.HasValue && node.Children.Count < Min.Value)
            {
                return false;
            }
            if (Max.HasValue && node.Children.Count > Max.Value)
            {
                return false;
            }
            return true;
        }

        public override string Message(string name)
        {
            if (Min.HasValue && Max.HasValue)
            {
                return string.Format("{0} 必须为复合类型，且行数应在 {1} 到 {2}。", name, Min.Value, Max.Value);
            }
            else if (Min.HasValue)
            {
                return string.Format("{0} 必须为复合类型，且行数应大于 {1}。", name, Min.Value);
            }
            else if (Max.HasValue)
            {
                return string.Format("{0} 必须为复合类型，且行数应小于 {1}。", name, Max.Value);
            }
            return string.Empty;
        }
    }
}
