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
            if (Session["UserID"] == null)
            {
                return Content("<script>alert('用户登陆状态已失效，请重新登录');window.location.href='../Login/Index';</script>");
            }
            string ID = Session["UserID"].ToString().Trim();
            string id = ID.Remove(0, 4);
            if (id == "01")
            {
                ViewBag.isadm = true;
            }
            else
            {
                ViewBag.isadm = false;
            }
            return View();
        }

        public ActionResult EvaForm(int id)
        {
            ViewBag.EvaID = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EvaForm(string score1, string score2, string score3, string score4, string EvaID)
        {
            if ((score1 == null) || (score2 == null) || (score3 == null) || (score4 == null))
            {
                return Content("<script>alert('错误：请评价全部四项后再提交评分');history.go(-1);</script>");
            }
            if (Session["UserID"] == null)
            {
                return Content("<script>alert('用户登陆状态已失效，请重新登录');window.location.href='../Login/Index';</script>");
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
            float s4 = float.Parse(score4);
            T_Score sc = new T_Score();
            sc.ID = ID;
            sc.EvaID = EvaID;
            sc.s1 = s1;
            sc.s2 = s2;
            sc.s3 = s3;
            sc.s4 = s4;
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
            if (Session["UserID"] == null)
            {
                return Content("<script>alert('用户登陆状态已失效，请重新登录');window.location.href='../Login/Index';</script>");
            }
            int Num = 0;
            string ID = Session["UserID"].ToString().Trim();
            string msg = "";
            var allrecord = from T_Score in db.T_Score where (T_Score.ID == ID) orderby T_Score.EvaID select T_Score;
            //这里还有一个问题，就是通过字符串形式排序，会导致1503002排在15030018后面
            foreach (T_Score record in allrecord)
            {
                msg = msg + record.EvaID.Remove(0, 6) + "号；";
                Num++;
            }
            msg = msg + "共计" + Num.ToString() + "个。";
            ViewBag.msg = msg;
            return View();
        }

        public ActionResult Result()
        {
            if (Session["UserID"] == null)
            {
                return Content("<script>alert('用户登陆状态已失效，请重新登录');window.location.href='../Login/Index';</script>");
            }
            string ID = Session["UserID"].ToString().Trim();
            string ClassId = ID.Substring(0, 4);
            ResultModel rm = new ResultModel();
            for (int i=0; i<50; i++)
            {
                int j = 0;
                float s1 = 0;
                float s2 = 0;
                float s3 = 0;
                float s4 = 0;
                string EvaID = ClassId + "00" + (i+1).ToString();
                var allrecord = from T_Score in db.T_Score where (T_Score.EvaID == EvaID) orderby T_Score.EvaID select T_Score;
                if (allrecord.Count() < 3)
                {
                    rm.isvalid[i] = false;
                    rm.num[i] = allrecord.Count();
                    continue;
                }
                foreach (T_Score record in allrecord)
                {
                    s1 += (float)record.s1;
                    s2 += (float)record.s2;
                    s3 += (float)record.s3;
                    s4 += (float)record.s4;
                    j++;
                }
                rm.isvalid[i] = true;
                rm.num[i] = allrecord.Count();
                rm.sc1[i] = s1 / j;
                rm.sc2[i] = s2 / j;
                rm.sc3[i] = s3 / j;
                rm.sc4[i] = s4 / j;
                rm.sct[i] = rm.sc1[i] + rm.sc2[i] + rm.sc3[i] + rm.sc4[i] + 8;
            }
            ViewBag.rm = rm;
            return View();
        }
    }

    public class ResultModel
    {
        public bool[] isvalid = new bool[50];
        public float[] sc1 = new float[50];
        public float[] sc2 = new float[50];
        public float[] sc3 = new float[50];
        public float[] sc4 = new float[50];
        public float[] sct = new float[50];
        public int[] num = new int[50];
    }
}