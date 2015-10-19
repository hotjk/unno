using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public class UIDisplayParams
    {
        public UIDisplayParams(string key, 
            string unit = null, 
            UIControlFormat format = UIControlFormat.String,
            string cssClass = "form-control",
            string style = null)
        {
            this.Key = key;
            this.Unit = unit;
            this.Format = format;
            this.CssClass = cssClass;
            this.Style = style;
        }
        public string Key { get; private set; }
        public string CssClass { get; private set; }
        public string Unit { get; private set; }
        public UIControlFormat Format { get; private set; }
        public string Style { get; private set; }

        public void AddClass(string @class)
        {
            if(string.IsNullOrEmpty(this.CssClass))
            {
                this.CssClass = @class;
            }
            else
            {
                this.CssClass = this.CssClass + " " + @class;
            }
        }
    }
}
