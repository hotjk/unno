using System;
using System.Collections.Specialized;
namespace Grit.Unno
{
    public interface IUnnoService
    {
        NodeWrapper LoadNode(Guid id);
        UnitWrapper LoadUnit(Guid id);
        /// <summary>
        /// Parse http post base on unit structure, build node tree.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        Node Parse(Unit unit, NameValueCollection collection);
        void SaveNode(NodeWrapper wrapper);
        void SaveUnit(UnitWrapper wrapper);
    }
}
