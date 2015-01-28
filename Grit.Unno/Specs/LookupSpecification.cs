using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Specs
{
    public class LookupSpecification: Specification
    {
        public LookupSpecification() { }
        public LookupSpecification(List<string> options)
        {
            if (options.Count() == 0)
            {
                throw new ApplicationException("LookupSpecification options needed.");
            }
            this.Type = SpecificationType.Lookup;
            Options = options;
        }
        /// <summary>
        /// Json.net need public set
        /// </summary>
        public List<string> Options { get; set; }
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
            if(!Options.Any(n=>n == value))
            {
                return false;
            }
            return true;
        }

        public override string Message(string name)
        {
            return string.Format("{0} 必须为从可选值中选取。", name);
        }
    }
}
