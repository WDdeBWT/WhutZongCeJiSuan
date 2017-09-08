using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhutZongCeJiSuan.Models;
using System.Text;

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
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return Content("<script>alert('用户名或密码错误！请手动返回上一页重新登录');</script>");
            }
        }
    }
}