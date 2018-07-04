using BowntDAL;
using ManagerUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerUI.Controllers
{
    public class BaseController : Controller
    {
        protected LanguageModel Language
        {
            get
            {
                BowntdbEntities entity = new BowntdbEntities();
                var list = entity.tb_Language.ToList();
                list.Sort((a, b) => a.Sort - b.Sort);
                var f = list.First();

                HttpCookie cookie = Request.Cookies["language"];
                if (cookie != null)
                {
                    int id = int.Parse(cookie.Value);
                    tb_Language lang = entity.tb_Language.FirstOrDefault(a => a.Id == id);
                    if (lang != null)
                        return new LanguageModel(lang.Id, lang.Language, lang.Logogram);

                    return new LanguageModel(f.Id, f.Language, f.Logogram);
                }
                return new LanguageModel(f.Id, f.Language, f.Logogram);
            }
        }

        protected BowntdbEntities Entities = new BowntdbEntities();

        protected bool LanguageFilter<T>(T t)
        {
            if (Language.Id == 0)
                return true;
            int languageId = (int)typeof(T).GetProperty("LanguageId").GetValue(t);
            return languageId == Language.Id;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.Language = this.Language;
            base.OnActionExecuted(filterContext);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }
    }
}
