using BowntDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerUI.Controllers
{
    public class AppManagerController : BaseController
    {
        public ActionResult List()
        {
            var model = Entities.tb_Applocation.Where(LanguageFilter).ToList();
            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(tb_Applocation model, HttpPostedFileWrapper ImgAddress, HttpPostedFileWrapper Conver)
        {
            string fileName = null;

            fileName = $"/ImgProject/{Common.CommonHelper.GetNowLangTime()}1{Path.GetExtension(ImgAddress.FileName)}"; 
            ImgAddress.SaveAs(Server.MapPath(fileName));
            model.ImgAddress = fileName;

            fileName = $"/ImgProject/{Common.CommonHelper.GetNowLangTime()}2{Path.GetExtension(Conver.FileName)}";
            Conver.SaveAs(Server.MapPath(fileName));
            model.Conver = fileName;

            model.LanguageId = Language.Id;
            model.AppName = model.AppName;
            model.CreateTime = DateTime.Now;

            Entities.tb_Applocation.Add(model);
            Entities.SaveChanges();

            return View();
        }

        public ActionResult Edit(int id)
        {
            var model = Entities.tb_Applocation.FirstOrDefault(t => t.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(tb_Applocation model, HttpPostedFileWrapper ImgAddress, HttpPostedFileWrapper Conver)
        {
            tb_Applocation appnew = Entities.tb_Applocation.FirstOrDefault(t => t.Id == model.Id);

            if (ImgAddress?.ContentLength > 0)
            {
                string fileName = $"/ImgProject/{Common.CommonHelper.GetNowLangTime()}1{Path.GetExtension(ImgAddress.FileName)}";
                ImgAddress.SaveAs(Server.MapPath(fileName));
                appnew.ImgAddress = fileName;
            }

            if (Conver?.ContentLength > 0)
            {
                string fileName = $"/ImgProject/{Common.CommonHelper.GetNowLangTime()}2{Path.GetExtension(Conver.FileName)}";
                Conver.SaveAs(Server.MapPath(fileName));
                appnew.Conver = fileName;
            }

            appnew.LanguageId = Language.Id;
            appnew.AppName = model.AppName;
            appnew.CreateTime = DateTime.Now;

            Entities.SaveChanges();
            return View(appnew);
        }
        public ActionResult Delete(int id)
        {
            tb_Applocation pro = Entities.tb_Applocation.FirstOrDefault(t => t.Id == id);
            Entities.tb_Applocation.Remove(pro);
            Entities.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
