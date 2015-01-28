using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.File
{
    public class NodeRepository : INodeRepository
    {
        private IUnitRepository _unitRepository;
        private string _nodePath;
        private FileOptions _options;
        private static JsonSerializerSettings _settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        public NodeRepository(IUnitRepository unitRepository, FileOptions options)
        {
            _options = options;
            _nodePath = System.IO.Path.Combine(_options.Path, "node");
            if (!System.IO.Directory.Exists(_nodePath))
            {
                Directory.CreateDirectory(_nodePath);
            }
            _unitRepository = unitRepository;
        }


        public NodeWrapper LoadNode(Guid id)
        {
            string file = Path.Combine(_nodePath, id.ToString());
            if (!System.IO.File.Exists(file))
            {
                return null;
            }
            string json = System.IO.File.ReadAllText(Path.Combine(_nodePath, id.ToString()));
            NodeWrapper4File wrapper = JsonConvert.DeserializeObject<NodeWrapper4File>(json);
            if (wrapper != null && wrapper.Data != null && wrapper.Node == null)
            {
                var unitWrapper = _unitRepository.LoadUnit(wrapper.UnitId);
                Converter converter = new Converter(_options.JsonSingleChildAsCollection);
                wrapper.Node = converter.SerializableObjectToNode(wrapper.Data as JObject, unitWrapper.Unit);
            }
            return wrapper;
        }

        public void SaveNode(NodeWrapper wrapper)
        {
            NodeWrapper4File wrapper4File = new NodeWrapper4File(wrapper);
            var reload = LoadNode(wrapper.NodeId);
            if (reload != null)
            {
                if (reload.Version != wrapper.Version)
                {
                    throw new ConcurrencyException(wrapper.NodeId.ToString());
                }
                wrapper4File.Version = reload.Version + 1;
            }
            if (_options.JsonNormalize)
            {
                Converter converter = new Converter(_options.JsonSingleChildAsCollection);
                wrapper4File.Data = converter.NodeToSerializableObject(wrapper.Node);
                wrapper4File.Node = null;
            }
            string json = JsonConvert.SerializeObject(wrapper4File, Formatting.Indented, _settings);
            System.IO.File.WriteAllText(Path.Combine(_nodePath, wrapper.NodeId.ToString()), json, Encoding.UTF8);
        }
    }
}