using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public class UnnoService : IUnnoService
    {
        private IUnitRepository _unitRepository;
        private INodeRepository _nodeRepository;
        public UnnoService(IUnitRepository unitRepository, INodeRepository nodeRepository)
        {
            _unitRepository = unitRepository;
            _nodeRepository = nodeRepository;
        }

        public bool LoadUnitAndNode(Guid id, out UnitWrapper unitWrapper, out NodeWrapper nodeWrapper)
        {
            nodeWrapper = LoadNode(id);
            unitWrapper = null;
            if (nodeWrapper == null)
            {
                unitWrapper = LoadUnit(id);
            }
            else
            {
                unitWrapper = LoadUnit(nodeWrapper.UnitId);
            }

            return unitWrapper != null;
        }

        public UnitWrapper LoadUnit(Guid id)
        {
            UnitWrapper wrapper = _unitRepository.LoadUnit(id);
            return wrapper;
        }

        public void SaveUnit(UnitWrapper wrapper)
        {
            wrapper.UpdateAt = DateTime.Now;
            _unitRepository.SaveUnit(wrapper);
        }

        public NodeWrapper LoadNode(Guid id)
        {
            NodeWrapper wrapper = _nodeRepository.LoadNode(id);
            if (wrapper != null)
            {
                UnitWrapper unitWrapper = _unitRepository.LoadUnit(wrapper.UnitId);
            }
            return wrapper;
        }

        public void SaveNode(NodeWrapper wrapper)
        {
            wrapper.UpdateAt = DateTime.Now;
            _nodeRepository.SaveNode(wrapper);
        }

        public Node Parse(Unit unit, NameValueCollection collection)
        {
            if (collection.Count > UIConstants.MaxHttpCollectionKeys)
            {
                throw new ApplicationException(string.Format("Post data include too many collection keys, max http collection key is {0}.", 
                    UIConstants.MaxHttpCollectionKeys));
            }
            var keys = collection.Keys.Cast<string>().Where(n => n.IndexOf(UIConstants.ID) == -1).ToList();
            var dict = new Dictionary<string, IList<string>>();

            Node.Count(unit, keys, dict);
            Node node = Node.Parse(unit, collection, dict);

            return node;
        }
    }
}
