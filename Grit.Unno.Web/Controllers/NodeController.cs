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

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            NodeWrapper nodeWrapper = _unnoService.LoadNode(id);
            UnitWrapper unitWrapper = null;
            if(nodeWrapper == null)
            {
                unitWrapper = _unnoService.LoadUnit(id);
                if(unitWrapper == null)
                {
                    return new HttpNotFoundResult();
                }
                nodeWrapper = new NodeWrapper
                {
                    NodeId = unitWrapper.UnitId,
                    UnitId = unitWrapper.UnitId,
                    Node = new Node(unitWrapper.Unit.Key)
                };
            }
            else
            {
                unitWrapper = _unnoService.LoadUnit(nodeWrapper.UnitId);
            }
            ViewBag.Unit = unitWrapper.Unit;
            ViewBag.Node = nodeWrapper.Node;
            return View(nodeWrapper.UnitId.ToString(), nodeWrapper);
        }

        [HttpPost]
        public ActionResult Edit(Guid id, int version, string name = null)
        {
            NodeWrapper nodeWrapper = _unnoService.LoadNode(id);
            UnitWrapper unitWrapper = null;
            if (nodeWrapper == null)
            {
                unitWrapper = _unnoService.LoadUnit(id);
                if (unitWrapper == null)
                {
                    return new HttpNotFoundResult();
                }
                nodeWrapper = new NodeWrapper { NodeId = Guid.NewGuid(), UnitId = unitWrapper.UnitId };
            }
            else
            {
                unitWrapper = _unnoService.LoadUnit(nodeWrapper.UnitId);
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