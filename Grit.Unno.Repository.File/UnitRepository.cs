using Grit.Unno.Specs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.File
{
    public class UnitRepository : IUnitRepository
    {
        public UnitRepository(FileOptions options)
        {
            _unitPath = System.IO.Path.Combine(options.Path, "unit");
            if (!System.IO.Directory.Exists(_unitPath))
            {
                Directory.CreateDirectory(_unitPath);
            }
        }
        private string _unitPath;
        private static JsonSerializerSettings _settings;
        private static JsonConverter[] _converter = new JsonConverter[] { new SpecConverter() };

        static UnitRepository()
        {
            _settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            _settings.Converters.Add(new StringEnumConverter());
        }

        public UnitWrapper LoadUnit(Guid id)
        {
            string file = Path.Combine(_unitPath, id.ToString());
            if(!System.IO.File.Exists(file))
            {
                return null;
            }
            string json = System.IO.File.ReadAllText(file);
            return JsonConvert.DeserializeObject<UnitWrapper>(json, _converter);
        }

        public void SaveUnit(UnitWrapper wrapper)
        {
            string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented, _settings);
            System.IO.File.WriteAllText(Path.Combine(_unitPath, wrapper.UnitId.ToString()), json, Encoding.UTF8);
        }
    }
}
