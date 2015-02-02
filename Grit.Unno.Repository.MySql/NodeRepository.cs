using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace Grit.Unno.Repository.MySql
{
    public class NodeRepository : BaseRepository, INodeRepository
    {
        private const string UNNO_TABLE_PREFIX = "unno";
        private IUnitRepository _unitRepository;
        public NodeRepository(SqlOptions options, IUnitRepository unitRepository)
        {
            _options = options;
            _unitRepository = unitRepository;
        }

        public NodeWrapper LoadNode(Guid id)
        {
            NodeWrapper wrapper = null;
            int rootId = 0;
            UnitWrapper unitWrapper = null;
            List<NodeTable> tables = new List<NodeTable>();
            using (MySqlConnection connection = OpenConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT `NodeId`, `UnitId`, `RootId`, `Version`, `UpdateAt` FROM `unno_node_wrapper` WHERE `NodeId` = @NodeId;";
                    cmd.Parameters.AddWithValue("@NodeId", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            wrapper = new NodeWrapper();
                            rootId = (int)reader["RootId"];
                            wrapper.NodeId = id;
                            wrapper.UnitId = Guid.Parse(reader["UnitId"].ToString());
                            wrapper.Version = (int)reader["Version"];
                            wrapper.UpdateAt = (DateTime)reader["UpdateAt"];
                        }
                    }
                }
                if (wrapper != null)
                {
                    unitWrapper = _unitRepository.LoadUnit(wrapper.UnitId);
                    LoadDataFromDB(connection, rootId, unitWrapper.Unit, UNNO_TABLE_PREFIX, tables);
                }
            }

            if (wrapper == null) return null;
            wrapper.Node = BuildNode(unitWrapper.Unit, tables, UNNO_TABLE_PREFIX, 0);
            return wrapper;
        }

        private Node BuildNode(Unit unit, List<NodeTable> tables, string tableName, int parentIndex)
        {
            tableName = tableName + "_" + unit.Key;

            var table = tables.SingleOrDefault(n => n.Name == tableName);
            var rows = table.Rows.Where(n => n.ParentIndex == parentIndex);

            var leaves = unit.Children.Where(x => x.Leaf);
            var trunks = unit.Children.Where(x => !x.Leaf);

            Node node = new Node(unit.Key);

            foreach(var row in rows)
            {
                node.AddRow();
                foreach (var leaf in leaves)
                {
                    if(leaf.Specifications.Any(n=>n.Type == Specs.SpecificationType.Boolean))
                    {
                        node.AddNode(new Node(leaf.Key, (int)row.Columns[leaf.Key] == 1?true:false));
                    }
                    else{
                        node.AddNode(new Node(leaf.Key, row.Columns[leaf.Key]));
                    }
                }
                foreach(var trunk in trunks)
                {
                    var child = BuildNode(trunk, tables, tableName, row.NodeIndex);
                    node.AddNode(child);
                }
            }
            return node;
        }

        private void LoadDataFromDB(MySqlConnection connection, int rootId, Unit unit, string tableName, List<NodeTable> refTables)
        {
            tableName = tableName + "_" + unit.Key;

            var leaves = unit.Children.Where(x => x.Leaf);
            var trunks = unit.Children.Where(x => !x.Leaf);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT `ParentIndex`, `NodeIndex`");
            foreach (var leaf in leaves)
            {
                sb.AppendFormat(", `{0}`", leaf.Key);
            }
            sb.AppendFormat(" FROM `{0}` WHERE `RootId` = @RootId ORDER BY `ParentIndex`, `NodeIndex`;", tableName);

            var table = new NodeTable(tableName);
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@RootId", rootId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NodeRow row = new NodeRow((int)reader["ParentIndex"], (int)reader["NodeIndex"]);
                        foreach (var leaf in leaves)
                        {
                            row.AddColumn(leaf.Key, reader[leaf.Key]);
                        }
                        table.AddRow(row);
                    }
                }
            }
            refTables.Add(table);

            foreach (var trunk in trunks)
            {
                LoadDataFromDB(connection, rootId, trunk, tableName, refTables);
            }
        }

        public void SaveNode(NodeWrapper wrapper)
        {
            using (MySqlConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    if (SaveWrapper(connection, wrapper))
                    {
                        int rootId = connection.Query<int>(@"SELECT `RootId` FROM `unno_node_wrapper` WHERE `NodeId` = @NodeId;",
                            new { NodeId = wrapper.NodeId }).Single();

                        SaveNode(connection, rootId, wrapper.Node, UNNO_TABLE_PREFIX, 0);
                        transaction.Commit();
                    }
                }
            }
        }

        private bool SaveWrapper(MySqlConnection connection, NodeWrapper wrapper)
        {
            NodeWrapper found = connection.Query<NodeWrapper>(
@"SELECT `Version` FROM `unno_node_wrapper` WHERE `NodeId` = @NodeId FOR UPDATE;",
new { NodeId = wrapper.NodeId }).SingleOrDefault();
            if (found == null)
            {
                connection.Execute(
@"INSERT INTO `unno_node_wrapper` (`NodeId`, `UnitId`, `Version`, `UpdateAt`)
VALUES (@NodeId, @UnitId, @Version, @UpdateAt);", wrapper);
            }
            else
            {
                if (found.Version != wrapper.Version)
                {
                    return false;
                }
                wrapper.Version++;
                connection.Execute(
@"UPDATE `unno_node_wrapper` SET
`UnitId` = @UnitId, 
`Version` = @Version, 
`UpdateAt` = @UpdateAt 
WHERE `NodeId` = @NodeId;", wrapper);
            }
            return true;
        }

        private void SaveNode(MySqlConnection connection, int rootId, Node node, string tableName, int parentIndex)
        {
            tableName = tableName + "_" + node.Key;
            connection.Execute(string.Format("DELETE FROM `{0}` WHERE `RootId` = @RootId AND `ParentIndex` = @ParentIndex; ", tableName),
                new { RootId = rootId, ParentIndex = parentIndex });

            int index = 0;
            foreach (Dictionary<string, Node> dict in node.Children)
            {
                var leaves = dict.Where(x => x.Value.Unit.Leaf);
                var trunks = dict.Where(x => !x.Value.Unit.Leaf);

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("INSERT INTO `{0}` (`RootId`, `ParentIndex`, `NodeIndex`", tableName);
                foreach (var kv in leaves)
                {
                    sb.AppendFormat(", `{0}`", kv.Key);
                }
                sb.AppendFormat(") VALUES (@RootId, @ParentIndex, @NodeIndex ");
                foreach (var leaf in leaves)
                {
                    sb.AppendFormat(", @{0}", leaf.Key);
                }
                sb.AppendFormat(");");
                DynamicParameters p = new DynamicParameters();
                p.Add("RootId", rootId);
                p.Add("ParentIndex", parentIndex);
                p.Add("NodeIndex", index);
                foreach (var leaf in leaves)
                {
                    p.Add(leaf.Key, leaf.Value.Value);
                }
                connection.Execute(sb.ToString(), p);

                foreach (var trunk in trunks)
                {
                    if (trunk.Value.Children != null)
                    {
                        SaveNode(connection, rootId, trunk.Value, tableName, index);
                    }
                }
                index++;
            }
        }
    }
}
