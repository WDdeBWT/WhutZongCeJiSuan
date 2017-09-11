using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhutZongCeJiSuan.Models;
using System.Text;
using System.Data.Entity.Validation;

namespace WhutZongCeJiSuan.Controllers
{
    public class LoginController : Controller
    {
        private ZongCeEntities db = new ZongCeEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string ID, string Psw)
        {
            var user = from T_Account in db.T_Account where (T_Account.ID == ID.ToString().Trim()) && (T_Account.password == Psw.ToString().Trim()) select T_Account;

            if (user.Any() && user.Count() == 1)
            {
                //从数据集中提取
                Session["UserID"] = user.First().ID;
                Session["UserPsd"] = user.First().password;
                if (user.First().ID == "999999")
                {
                    return RedirectToAction("SuperUser", "Login");
                }
                else
                {
                    return RedirectToAction("Index", "Evaluate");
                }
            }
            else
            {
                return Content("<script>alert('用户名或密码错误！');history.go(-1);</script>");
            }
        }

        public ActionResult MdfPsw()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MdfPsw(string Psw0, string Psw1, string Psw2)
        {
            if (Session["UserID"] == null)
            {
                return Content("<script>alert('用户登陆状态已失效，请重新登录');window.location.href='../Login/Index';</script>");
            }
            string ID = Session["UserID"].ToString().Trim();
            var user = from T_Account in db.T_Account where (T_Account.ID == ID) && (T_Account.password == Psw0.ToString().Trim()) select T_Account;
            if (user.Any() && user.Count() == 1)
            {
                if (0 == string.Compare(Psw1, Psw2))
                {
                    T_Account user1 = user.First();
                    user1.password = Psw1;
                    db.SaveChanges();
                    return Content("<script>alert('密码修改成功');window.location.href='../Evaluate/index';</script>");
                }
                else
                {
                    return Content("<script>alert('错误：两次新密码输入不一致');history.go(-1);</script>");
                }
            }
            else
            {
                return Content("<script>alert('错误：原密码输入错误');history.go(-1);</script>");
            }
        }

        public ActionResult SuperUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuperUser(string ClassID, string ClassNum)
        {
            if (Session["UserID"].ToString().Trim() != "999999")
            {
                return Content("<script>alert('用户权限错误');window.location.href='../Login/Index';</script>");
            }
            int num = int.Parse(ClassNum);
            for (int i=0; i<num; i++)
            {
                T_Account user = new T_Account();
                user.ID = ClassID + (i + 1).ToString("00");
                user.password = ClassID + (i + 1).ToString("00");
                db.T_Account.Add(user);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    return Content("<script>alert('错误：数据库保存错误，请联系管理员');history.go(-1);</script>");
                }
            }
            return Content("<script>alert('保存成功！');history.go(-1);</script>");
        }
        }
}