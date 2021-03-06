﻿using BowntDAL;
using CompanyUI.App_Start;
using CompanyUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CompanyUI.Controllers
{
    public class BaseController : Controller
    {
        private LanguageModel languageModel;

        public LanguageModel Language
        {
            get
            {
                if (languageModel != null)
                    return languageModel;

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
                languageModel = new LanguageModel(f.Id, f.Language, f.Logogram);
                return languageModel;
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.Language = this.Language;
            base.OnActionExecuted(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Response.Filter = new WhiteSpaceFilter(System.Web.HttpContext.Current.Response, html =>
            //{
            //    //s = Regex.Replace(s, @"\s+(?=<)|\s+$|(?<=>)\s+", "");
            //    //s = s.Replace("\r\n", " ");
            //    //single - line doctype must be preserved
            //    //var firstEndBracketPosition = s.IndexOf(">");
            //    //if (firstEndBracketPosition >= 0)
            //    //{
            //    //    s = s.Remove(firstEndBracketPosition, 1);
            //    //    s = s.Insert(firstEndBracketPosition, ">");
            //    //}
            //    return html;
            //});
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }
    }
}
