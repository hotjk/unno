using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grit.Unno.Web.Controllers
{
    public class NodeController : Controller
    {
        private IUnnoService _unnoService;

        public NodeController(IUnnoService unnoService)
        {
            this._unnoService = unnoService;
        }

        public ActionResult Index(Guid id)
        {
            NodeWrapper wrapper = _unnoService.LoadNode(id);
            return View(wrapper);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            UnitWrapper unitWrapper = null;
            NodeWrapper nodeWrapper = null;
            if (!_unnoService.LoadUnitAndNode(id, out unitWrapper, out nodeWrapper))
            {
                return new HttpNotFoundResult();
            }

            if(nodeWrapper == null)
            {
                nodeWrapper = new NodeWrapper { NodeId = unitWrapper.UnitId, 
                    UnitId = unitWrapper.UnitId, 
                    Node = new Node(unitWrapper.Unit.Key) };
            }

            ViewBag.Unit = unitWrapper.Unit;
            ViewBag.Node = nodeWrapper.Node;
            
            return View(unitWrapper.UnitId.ToString(), nodeWrapper);
        }

        [HttpPost]
        public ActionResult Edit(Guid id, int version, string name = null)
        {
            UnitWrapper unitWrapper = null;
            NodeWrapper nodeWrapper = null;
            if (!_unnoService.LoadUnitAndNode(id, out unitWrapper, out nodeWrapper))
            {
                return new HttpNotFoundResult();
            }

            if (nodeWrapper == null)
            {
                nodeWrapper = new NodeWrapper { NodeId = Guid.NewGuid(), UnitId = unitWrapper.UnitId };
            }

            Node node = _unnoService.Parse(unitWrapper.Unit, Request.Form);
            var errors = new Dictionary<string, string>();
            node.Validate(errors);
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            nodeWrapper.Name = name;
            nodeWrapper.Version = version;
            nodeWrapper.Node = node;

            if (ModelState.IsValid)
            {
                _unnoService.SaveNode(nodeWrapper);
            }

            ViewBag.Unit = unitWrapper.Unit;
            ViewBag.Node = nodeWrapper.Node;
            return RedirectToAction("Edit", new { id = nodeWrapper.NodeId });
        }
    }
}