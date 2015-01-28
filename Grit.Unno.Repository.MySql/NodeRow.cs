using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.MySql
{
    public class NodeRow
    {
        public NodeRow(int parentIndex, int nodeIndex)
        {
            this.ParentIndex = parentIndex;
            this.NodeIndex = nodeIndex;
            this.Columns = new Dictionary<string, object>();
        }
        public int ParentIndex { get; private set; }
        public int NodeIndex { get; private set; }
        public Dictionary<string, object> Columns { get; private set; }

        public void AddColumn(string k, object v)
        {
            Columns[k] = v;
        }
    }

    public class NodeTable
    {
        public NodeTable(string name)
        {
            this.Name = name;
            this.Rows = new List<NodeRow>();
        }
        public string Name { get; private set; }
        public List<NodeRow> Rows { get; private set; }

        public void AddRow(NodeRow row)
        {
            this.Rows.Add(row);
        }
    }
}
