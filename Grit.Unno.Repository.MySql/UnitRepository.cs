using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Grit.Unno.Repository.MySql
{
    public class UnitRepository : BaseRepository, IUnitRepository
    {
        public UnitRepository(SqlOptions options)
        {
            _options = options;
        }

        public UnitWrapper LoadUnit(Guid id)
        {
            using (MySqlConnection connection = OpenConnection())
            {
                UnitWrapper wrapper = connection.Query<UnitWrapper>(
@"SELECT `Name`, `Version`, `UpdateAt` FROM `unno_unit_wrapper` WHERE `UnitId` = @UnitId;",
new { UnitId = id }).SingleOrDefault();
                if (wrapper == null) return null;
                wrapper.UnitId = id;
                var units = connection.Query<UnitRow>(
@"SELECT `Id`, `ParentId`, `Key`, `Name` FROM `unno_unit` WHERE `UnitId` = @UnitId ORDER BY Id;",
new { UnitId = id});
                var specs = connection.Query<SpecRow>(
@"SELECT `Id`, `Type`, `Min`, `Max`, `Options` FROM `unno_unit_specs` WHERE `UnitId` = @UnitId ORDER BY `Id`, `SpecId`;",
new { UnitId = id });

                var dummy = new Unit("dummy",null);
                BindUnit(units, specs, dummy, null);
                wrapper.Unit = dummy.Children.FirstOrDefault();
                return wrapper;
            }
        }

        private void BindUnit(IEnumerable<UnitRow> units, IEnumerable<SpecRow> specs, Unit parent, int? parentId)
        {
            var rows = units.Where(n => n.ParentId == parentId);
            foreach(var unitRow in rows)
            {
                Unit unit = new Unit(unitRow.Key, unitRow.Name);
                foreach(var specRow in specs.Where(n=>n.Id == unitRow.Id))
                {
                    Specs.Specification spec = null;
                    switch(specRow.Type)
                    {
                        case Specs.SpecificationType.Boolean:
                            spec = new Specs.BooleanSpecification();
                            break;
                        case Specs.SpecificationType.Required:
                            spec = new Specs.RequiredSpecification();
                            break;
                        case Specs.SpecificationType.Composite:
                            spec = new Specs.CompositeSpecification(specRow.GetMin<int>(), specRow.GetMax<int>());
                            break;
                        case Specs.SpecificationType.Integer:
                            spec = new Specs.IntegerSpecification(specRow.GetMin<int>(), specRow.GetMax<int>());
                            break;
                        case Specs.SpecificationType.String:
                            spec = new Specs.StringSpecification(specRow.GetMin<int>(), specRow.GetMax<int>());
                            break;
                        case Specs.SpecificationType.DateTime:
                            spec = new Specs.DateTimeSpecification(specRow.GetMin<DateTime>(), specRow.GetMax<DateTime>());
                            break;
                        case Specs.SpecificationType.Decimal:
                            spec = new Specs.DecimalSpecification(specRow.GetMin<decimal>(), specRow.GetMax<decimal>());
                            break;
                        case Specs.SpecificationType.Lookup:
                            spec = new Specs.LookupSpecification(specRow.Options.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries).ToList());
                            break;
                    }
                    unit.AddSpecification(spec);
                }
                parent.AddChild(unit);
                BindUnit(units, specs, unit, unitRow.Id);
            }
        }

        public void SaveUnit(UnitWrapper wrapper)
        {
            using (MySqlConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    if (SaveWrapper(connection, wrapper))
                    {
                        int id = 0;
                        SaveUnit(connection, wrapper.Unit, wrapper.UnitId, null, ref id);
                        transaction.Commit();
                    }
                }
            }
        }

        private bool SaveWrapper(MySqlConnection connection, UnitWrapper wrapper)
        {
            UnitWrapper found = connection.Query<UnitWrapper>(
@"SELECT `Version` FROM `unno_unit_wrapper` WHERE `UnitId` = @UnitId FOR UPDATE;",
new { UnitId = wrapper.UnitId }).SingleOrDefault();
            if (found == null)
            {
                connection.Execute(
@"INSERT INTO `unno_unit_wrapper` (`UnitId`, `Name`, `Version`, `UpdateAt`)
VALUES (@UnitId, @Name, @Version, @UpdateAt);", wrapper);
            }
            else
            {
                wrapper.Version++;
                connection.Execute(
@"UPDATE `unno_unit_wrapper` SET
`Name` = @Name, 
`Version` = @Version, 
`UpdateAt` = @UpdateAt 
WHERE `UnitId` = @UnitId;", wrapper);

                connection.Execute(
@"DELETE FROM `unno_unit` WHERE `UnitId` = @UnitId;
DELETE FROM `unno_unit_specs` WHERE `UnitId` = @UnitId;", new { UnitId = wrapper.UnitId });
            }
            return true;
        }

        private void SaveUnit(MySqlConnection connection, Unit unit, Guid unitId, int? parentId, ref int id)
        {
            connection.Execute(
@"INSERT INTO `unno_unit`(`UnitId`, `Id`, `ParentId`, `Key`, `Name`)
VALUES (@UnitId, @Id, @ParentId, @Key, @Name);",
                new { UnitId = unitId, Id = id, ParentId = parentId, Key = unit.Key, Name = unit.Name });

            int pid = id;

            SaveSpecs(connection, unit, unitId, id);

            if (unit.Children != null)
            {
                foreach (var child in unit.Children)
                {
                    id++;
                    SaveUnit(connection, child, unitId, pid, ref id);
                }
            }
        }

        private static void SaveSpecs(MySqlConnection connection, Unit unit, Guid unitId, int id)
        {
            if (unit.Specifications != null)
            {
                int specId = 0;
                foreach (var spec in unit.Specifications)
                {
                    switch (spec.Type)
                    {
                        case Specs.SpecificationType.Boolean:
                            SaveSpec<int?>(connection, unitId, id, specId, spec);
                            break;
                        case Specs.SpecificationType.Required:
                            SaveSpec<int?>(connection, unitId, id, specId, spec);
                            break;
                        case Specs.SpecificationType.Composite:
                            SaveSpec<int?>(connection, unitId, id, specId, spec, (spec as Specs.CompositeSpecification).Min, (spec as Specs.CompositeSpecification).Max);
                            break;
                        case Specs.SpecificationType.Integer:
                            SaveSpec<int?>(connection, unitId, id, specId, spec, (spec as Specs.IntegerSpecification).Min, (spec as Specs.IntegerSpecification).Max);
                            break;
                        case Specs.SpecificationType.String:
                            SaveSpec<int?>(connection, unitId, id, specId, spec, (spec as Specs.StringSpecification).Min, (spec as Specs.StringSpecification).Max);
                            break;
                        case Specs.SpecificationType.DateTime:
                            SaveSpec<DateTime?>(connection, unitId, id, specId, spec, (spec as Specs.DateTimeSpecification).Min, (spec as Specs.DateTimeSpecification).Max);
                            break;
                        case Specs.SpecificationType.Decimal:
                            SaveSpec<decimal?>(connection, unitId, id, specId, spec, (spec as Specs.DecimalSpecification).Min, (spec as Specs.DecimalSpecification).Max);
                            break;
                        case Specs.SpecificationType.Lookup:
                            SaveSpec<int?>(connection, unitId, id, specId, spec, null, null, string.Join("|", (spec as Specs.LookupSpecification).Options));
                            break;
                    }
                    specId++;
                }
            }
        }

        private static void SaveSpec<T>(MySqlConnection connection, Guid unitId, int id, int specId, Specs.Specification spec, 
            T min =default(T), T max=default(T), string options = null)
        {
            connection.Execute(
@"INSERT INTO `unno_unit_specs`(`UnitId`, `Id`, `SpecId`, `Type`, `Min`, `Max`, `Options`) 
VALUES (@UnitId, @Id, @SpecId, @Type, @Min, @Max, @Options);",
                  new
                  {
                      UnitId = unitId,
                      Id = id,
                      SpecId = specId,
                      Type = spec.Type,
                      Min = min,
                      Max = max,
                      Options = options
                  });
        }
    }
}
