using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BowntDAL;
using System.IO;

namespace ManagerUI.Controllers
{
    public class PictureController : BaseController
    {
        string basePath = "/Content/projectImg/";//<img src="~/Content/projectImg/%e4%ba%a7%e5%93%81%e4%b8%ad%e5%bf%83.jpg" />
        public ActionResult List()
        {
            List<tb_Picture> model = new List<tb_Picture>();
            model = Entities.tb_Picture.OrderBy(a => a.PicTypeId).Where(a => a.LanguageId == null || a.LanguageId == Language.Id).ToList();

            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.PicTypes = Entities.tb_PicType.ToList();
            var model = Entities.tb_Picture.FirstOrDefault(a => a.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(tb_Picture model)
        {
            var pic = Entities.tb_Picture.FirstOrDefault(a => a.Id == model.Id);
            List<string> filenames = new List<string>();
            foreach (string key in Request.Files.Keys)
            {
                HttpPostedFileWrapper pid = Request.Files[key] as HttpPostedFileWrapper;
                if (pid.ContentLength > 0)
                {
                    string fileName = basePath + Common.CommonHelper.GetNowLangTime() + key + Path.GetExtension(pid.FileName);
                    pid.SaveAs(Server.MapPath("~" + fileName));
                    filenames.Add(fileName);
                }
            }
            pic.Path = String.Join(",", filenames);
            if (model.PicTypeId == 2)
            {
                model.LanguageId = Language.Id;
            }
            pic.Name = model.Name;
            Entities.SaveChanges();
            return RedirectToAction("List", new { picSort = 2 });
        }

        public ActionResult Add()
        {
            ViewBag.PicTypes = Entities.tb_PicType.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Add(tb_Picture model, HttpPostedFileWrapper picture)
        {
            List<string> filenames = new List<string>();
            foreach (string key in Request.Files.Keys)
            {
                HttpPostedFileWrapper pid = Request.Files[key] as HttpPostedFileWrapper;
                if (pid.ContentLength > 0)
                {
                    string fileName = basePath + Common.CommonHelper.GetNowLangTime() + key + Path.GetExtension(pid.FileName);
                    pid.SaveAs(Server.MapPath("~" + fileName));
                    filenames.Add(fileName);
                }
            }
            model.Path = String.Join(",", filenames);
            if (model.PicTypeId == 2)
                model.LanguageId = Language.Id;

            Entities.tb_Picture.Add(model);
            Entities.SaveChanges();

            return RedirectToAction("List", new { picSort = 2 });
        }

        public ActionResult Delete(int id)
        {
            var model = Entities.tb_Picture.FirstOrDefault(a => a.Id == id);
            if (model != null)
            {
                Entities.tb_Picture.Remove(model);
                Entities.SaveChanges();
            }

            return RedirectToAction("List");
        }
    }
}
