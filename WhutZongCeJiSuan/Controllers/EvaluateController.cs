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

        public ActionResult EvaForm(int id)
        {
            ViewBag.EvaID = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EvaForm(string score1, string score2, string score3, string EvaID)
        {
            if ((score1 == null) || (score1 == null) || (score1 == null))
            {
                return Content("<script>alert('错误：请评价全部三项后再提交评分');history.go(-1);</script>");
            }
            float s1 = float.Parse(score1);
            float s2 = float.Parse(score2);
            float s3 = float.Parse(score3);
            int eva_id = int.Parse(EvaID);
            
            return View();
        }
    }
}