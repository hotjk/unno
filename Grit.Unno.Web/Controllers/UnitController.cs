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

        public UnitController(IUnnoService unnoService,
            IStructureRepository structureRepository)
        {
            this._unnoService = unnoService;
            this._structureRepository = structureRepository;
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
    }
}