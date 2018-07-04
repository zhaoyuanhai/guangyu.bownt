using BowntDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerUI.Controllers
{
    public class SettingController : BaseController
    {
        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SetLanguage(int id)
        {
            HttpCookie cookie = cookie = new HttpCookie("language", id.ToString());
            Response.Cookies.Add(cookie);
            return Json(new { result = true });
        }
    }
}
