using BowntDAL;
using ManagerUI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerUI.Controllers
{
    public class ProductManagerController : BaseController
    {
        string basePath = "/ImgProject/";

        /// <summary>
        /// 产品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var model = Entities.tb_Prodect.OrderByDescending(a => a.Order).Where(LanguageFilter).ToList();
            return View(model);
        }

        /// <summary>
        /// 产品编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            ViewBag.Norms = Entities.tb_Norms.ToList();
            ViewBag.Languages = Entities.tb_Language.ToList();
            tb_Prodect model = Entities.tb_Prodect.FirstOrDefault(t => t.Id == id);
            return View(model);
        }

        /// <summary>
        /// 产品编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tb_Prodect model,
            HttpPostedFileWrapper Icon,
            HttpPostedFileWrapper Conver)
        {
            tb_Prodect pronew = Entities.tb_Prodect.FirstOrDefault(t => t.Id == model.Id);
            string pathFileName = string.Empty;
            if (Icon?.ContentLength > 0)
            {
                pathFileName = $"{basePath}{CommonHelper.GetNowLangTime()}1{Path.GetExtension(Icon.FileName)}";
                Icon.SaveAs(Server.MapPath("~" + pathFileName));
                pronew.Icon = pathFileName;
            }

            if (Conver?.ContentLength > 0)
            {
                pathFileName = $"{basePath}{CommonHelper.GetNowLangTime()}2{Path.GetExtension(Conver.FileName)}";
                Conver.SaveAs(Server.MapPath("~" + pathFileName));
                pronew.Conver = pathFileName;
            }
            pronew.Title = model.Title;
            pronew.Content = model.Content;
            pronew.LanguageId = Language.Id;
            pronew.NormsId = model.NormsId;

            Entities.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            ViewBag.Norms = Entities.tb_Norms.ToList();
            ViewBag.Languages = Entities.tb_Language.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(tb_Prodect model,
            HttpPostedFileWrapper Icon,
            HttpPostedFileWrapper Conver)
        {
            string pathFileName = string.Empty;

            pathFileName = basePath + CommonHelper.GetNowLangTime() + "1" + Path.GetExtension(Icon.FileName);
            Icon.SaveAs(Server.MapPath("~" + pathFileName));
            model.Icon = pathFileName;

            pathFileName = basePath + CommonHelper.GetNowLangTime() + "2" + Path.GetExtension(Conver.FileName);
            Conver.SaveAs(Server.MapPath("~" + pathFileName));
            model.Conver = pathFileName;

            model.LanguageId = Language.Id;
            Entities.tb_Prodect.Add(model);

            Entities.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            tb_Prodect pro = Entities.tb_Prodect.FirstOrDefault(t => t.Id == id);
            Entities.tb_Prodect.Remove(pro);
            Entities.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult SetTop(int id)
        {
            var model = Entities.tb_Prodect.First(a => a.Id == id);
            int? max = Entities.tb_Prodect.OrderByDescending(a => a.Order).FirstOrDefault().Order;
            model.Order = Convert.ToInt32(max) + 1;
            Entities.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
