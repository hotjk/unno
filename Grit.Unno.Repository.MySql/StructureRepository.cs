using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Grit.Unno.Repository.MySql
{
    public class StructureRepository : BaseRepository, IStructureRepository
    {
        private IUnitRepository _unitRepository;
        public StructureRepository(SqlOptions options, IUnitRepository unitRepository)
        {
            _options = options;
            _unitRepository = unitRepository;
        }

        #region static scripts

        private static string unno_unit_script = 
@"CREATE TABLE `unno_node_wrapper` (
  `NodeId` varchar(36) NOT NULL,
  `UnitId` varchar(36) NOT NULL,
  `Version` int(11) DEFAULT NULL,
  `UpdateAt` datetime DEFAULT NULL,
  PRIMARY KEY (`NodeId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `unno_unit_wrapper` (
  `UnitId` varchar(36) NOT NULL,
  `Name` varchar(200) NOT NULL,
  `Version` int(11) DEFAULT NULL,
  `UpdateAt` datetime DEFAULT NULL,
  PRIMARY KEY (`UnitId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `unno_unit` (
  `UnitId` varchar(36) NOT NULL,
  `Id` int(11) NOT NULL,
  `ParentId` int(11) DEFAULT NULL,
  `Key` varchar(200) NOT NULL,
  `Name` varchar(200) NOT NULL,
  PRIMARY KEY (`UnitId`,`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `unno_unit_specs` (
  `UnitId` varchar(36) NOT NULL,
  `Id` int(11) NOT NULL,
  `SpecId` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `Min` varchar(100) DEFAULT NULL,
  `Max` varchar(100) DEFAULT NULL,
  `Options` varchar(2000) DEFAULT NULL,
  PRIMARY KEY (`UnitId`,`SpecId`,`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
";

        #endregion

        public string GetUnitScript()
        {
            return unno_unit_script;
        }

        public IList<string> GetNodeScript(Guid id)
        {
            IList<string> sqls = new List<string>();
            var wrapper = _unitRepository.LoadUnit(id);
            if (wrapper != null)
            {
                CreateTableSQL(sqls, wrapper.Unit, "unno");
            }
            return sqls;
        }

        private void CreateTableSQL(IList<string> sqls, Unit unit, string table)
        {
            table = table + "_" + unit.Key;

            var leaves = unit.Children.Where(x => x.Leaf);
            var trunks = unit.Children.Where(x => !x.Leaf);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
@"CREATE TABLE `{0}` (
  `NodeId` VARCHAR(36) NOT NULL,
  `ParentIndex` INT NOT NULL,
  `NodeIndex` INT NOT NULL,", table);
            foreach (var leaf in leaves)
            {
                foreach (var spec in leaf.Specifications)
                {
                    switch (spec.Type)
                    {
                        case Specs.SpecificationType.Boolean:
                            sb.AppendFormat("  `{0}` INT,\n", leaf.Key);
                            break;
                        case Specs.SpecificationType.DateTime:
                            sb.AppendFormat("  `{0}` DATETIME,\n", leaf.Key);
                            break;
                        case Specs.SpecificationType.Decimal:
                            sb.AppendFormat("  `{0}` DECIMAL(20,6),\n", leaf.Key);
                            break;
                        case Specs.SpecificationType.Integer:
                            sb.AppendFormat("  `{0}` INT,\n", leaf.Key);
                            break;
                        case Specs.SpecificationType.String:
                            sb.AppendFormat("  `{0}` VARCHAR({1}),\n", leaf.Key, (spec as Specs.StringSpecification).Max ?? 1000);
                            break;
                        case Specs.SpecificationType.Lookup:
                            sb.AppendFormat("  `{0}` VARCHAR(200),\n", leaf.Key);
                            break;
                        default:
                            break;
                    }
                }
            }
            sb.AppendFormat(
@"  PRIMARY KEY (`NodeId`, `ParentIndex`, `NodeIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;");
            sqls.Add(sb.ToString());

            foreach(var trunk in trunks)
            {
                CreateTableSQL(sqls, trunk, table);
            }
        }
    }
}
