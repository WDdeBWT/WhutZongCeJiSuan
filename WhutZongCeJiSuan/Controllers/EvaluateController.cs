using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhutZongCeJiSuan.Models;
using System.Data.Entity.Validation;

namespace WhutZongCeJiSuan.Controllers
{
    public class EvaluateController : Controller
    {
        private ZongCeEntities db = new ZongCeEntities();
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
            string ID = Session["UserID"].ToString().Trim();
            EvaID = ID.Substring(0, 4) + "00" + EvaID;//制作被评价学生的学号，例如1503001或者15030038
            var record = from T_Score in db.T_Score where (T_Score.ID == ID) && (T_Score.EvaID == EvaID) select T_Score;
            if (record.Any() && record.Count() == 1)
            {
                return Content("<script>alert('错误：你已经评价过该学生，请勿重新评分');history.go(-1);</script>");
            }
            float s1 = float.Parse(score1);
            float s2 = float.Parse(score2);
            float s3 = float.Parse(score3);
            T_Score sc = new T_Score();
            sc.ID = ID;
            sc.EvaID = EvaID;
            sc.s1 = s1;
            sc.s2 = s2;
            sc.s3 = s3;
            db.T_Score.Add(sc);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                return Content("<script>alert('错误：数据库保存错误，请联系管理员');history.go(-1);</script>");
            }
            return Content("<script>alert('保存成功');window.location.href='../index';</script>");
        }
        public ActionResult Evaed()
        {
            string ID = Session["UserID"].ToString().Trim();
            string msg = "";
            var allrecord = from T_Score in db.T_Score where (T_Score.ID == ID) orderby T_Score.EvaID select T_Score;
            //这里还有一个问题，就是通过字符串形式排序，会导致1503002排在15030018后面
            foreach (T_Score record in allrecord) 
            {
                msg = msg + record.EvaID.Remove(0, 6) + "号；";
            }
            ViewBag.msg = msg;
            return View();
        }
    }
}