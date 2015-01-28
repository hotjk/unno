using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grit.Unno.Web.Controllers
{
    public class UnitController : Controller
    {
        private IUnnoService _unnoService;
        private IStructureRepository _structureRepository;
        //private IUnitRepository _unitRepository;

        public UnitController(IUnnoService unnoService,
            IStructureRepository structureRepository)
            //[Ninject.Named("mongo")] IUnitRepository unitRepository)
        {
            this._unnoService = unnoService;
            this._structureRepository = structureRepository;
            //this._unitRepository = unitRepository;
        }

        [HttpGet]
        public string Script()
        {
            return _structureRepository.GetUnitScript();
        }

        [HttpGet]
        public string NodeScript(Guid id)
        {
            var sqls = _structureRepository.GetNodeScript(id);
            return string.Join("\n\n", sqls);
        }

        public string test(Guid id)
        {
            var unitRepository = Grit.Unno.Web.App_Start.BootStrapper.Kernel.GetService(typeof(Grit.Unno.Repository.Mongodb.UnitRepository)) as IUnitRepository;
            unitRepository.SaveUnit(_unnoService.LoadUnit(id));
            return "done";
        }
    }
}