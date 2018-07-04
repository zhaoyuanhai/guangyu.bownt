using BowntDAL;
using ManagerUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ManagerUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                Response.Write("<script>alert('用户名或密码不能为空！')</script>");
                return View();
            }
            else
            {
                BowntDAL.BowntdbEntities dal = new BowntDAL.BowntdbEntities();
                if (dal.tb_SysUser.FirstOrDefault(a => a.UserName == model.UserName && a.Password == model.Password) != null)
                {
                    //Session["UserName"] = model.UserName;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    model.UserName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    "admins"
                    );
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                    return RedirectToAction("Edit");
                }
                else
                {
                    Response.Write("<script>alert('用户名或密码不正确！')</script>");
                }
            }
            return View("login");
        }

        public ActionResult Edit() => View();

        [HttpPost]
        public ActionResult Edit(string oldPassword,string newPassword,string rePassword)
        {
            if (!newPassword.Equals(rePassword))
            {
                ViewBag.Msg = "两次密码输入不一致";
                return View();
            }

            BowntdbEntities dao = new BowntdbEntities();
            var user = dao.tb_SysUser.FirstOrDefault(a => a.Password == oldPassword);
            if (user == null)
            {
                ViewBag.Msg = "密码错误";
                return View();
            }
            else
            {
                user.Password = newPassword;
                dao.SaveChanges();
                ViewBag.Msg = "密码修改成功";
            }
            return View();
        }
    }
}
