using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BowntDAL;
using ManagerUI.Common;
using System.IO;

namespace ManagerUI.Controllers
{
    public class DownLoadManagerController : BaseController
    {
        public ActionResult List()
        {
            var list = Entities.tb_DownLoad.OrderByDescending(a => a.Order).Where(LanguageFilter).ToList();
            return View(list);
        }

        public ActionResult Add() => View();

        [HttpPost]
        public ActionResult Add(tb_DownLoad model, HttpPostedFileWrapper file)
        {
            string fileName = "/Files/" + CommonHelper.GetNowLangTime() + Path.GetExtension(file.FileName);
            file.SaveAs(Server.MapPath("~" + fileName));

            model.FilePath = fileName;
            model.LanguageId = Language.Id;
            model.CreateDate = DateTime.Now;

            Entities.tb_DownLoad.Add(model);

            Entities.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Edit(int id)
        {
            var model = Entities.tb_DownLoad.FirstOrDefault(a => a.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(tb_DownLoad model, HttpPostedFileWrapper file)
        {
            var newModel = Entities.tb_DownLoad.First(a => a.Id == model.Id);
            if (file?.ContentLength > 0)
            {
                string fileName = "/Files/" + CommonHelper.GetNowLangTime() + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~" + fileName));
                newModel.FilePath = fileName;
            }

            newModel.Name = model.Name;
            Entities.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            var model = Entities.tb_DownLoad.FirstOrDefault(a => a.Id == id);
            if (model != null)
                Entities.tb_DownLoad.Remove(model);
            Entities.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult SetTop(int id)
        {
            var model = Entities.tb_DownLoad.First(a => a.Id == id);
            int? max = Entities.tb_DownLoad.OrderByDescending(a => a.Order).FirstOrDefault()?.Order;

            model.Order = Convert.ToInt32(max) + 1;

            Entities.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
