using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public interface IConverter<T>
    {
        T NodeToSerializableObject(Node node);
        Node SerializableObjectToNode(T obj, Unit unit);
    }
}
