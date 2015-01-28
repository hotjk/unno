using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.File
{
    public class FileOptions
    {
        public FileOptions(string path, 
            bool jsonNormalize = true, 
            bool jsonSingleChildAsCollection = false)
        {
            this.Path = path;
            this.JsonNormalize = jsonNormalize;
            this.JsonSingleChildAsCollection = jsonSingleChildAsCollection;
        }

        public string Path { get; private set; }
        public bool JsonNormalize { get; private set; }
        public bool JsonSingleChildAsCollection { get; private set; }
    }
}
