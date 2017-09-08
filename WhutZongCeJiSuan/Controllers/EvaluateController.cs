using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WhutZongCeJiSuan.Controllers
{
    public class EvaluateController : Controller
    {
        // GET: Evaluate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Score(int id)
        {
            return View();
        }
    }
}