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
            var picList = Entities.tb_PicType.ToList();
            foreach (var picType in picList)
            {
                picType.tb_Picture.Clear();
            }
            ViewBag.PicTypes = picList;

            var model = Entities.tb_Picture.FirstOrDefault(a => a.Id == id);
            model.tb_PicType = null;
            model.tb_Language = null;
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
            var pics = Entities.tb_PicType.ToList();
            foreach (var pictype in pics)
            {
                pictype.tb_Picture.Clear();
            }
            ViewBag.PicTypes = pics;

            return View();
        }

        [HttpGet]
        public ActionResult SetPicture(int? id)
        {
            var pics = Entities.tb_PicType.ToList();
            foreach (var pictype in pics)
            {
                pictype.tb_Picture.Clear();
            }
            ViewBag.PicTypes = pics;
            var model = Entities.tb_Picture.Find(id);
            var entity = new tb_Picture();
            if (model != null)
            {
                entity.Id = model.Id;
                entity.Name = model.Name;
                entity.Path = model.Path;
                entity.PicTypeId = model.PicTypeId;
                entity.LanguageId = model.LanguageId;
                entity.Conver = model.Conver;
            }
            return View(entity);
        }

        [HttpPost]
        public ActionResult SetPicture(tb_Picture model, bool isFile = false)
        {
            if (model.Id == 0)
            {
                Entities.tb_Picture.Add(model);
            }
            else
            {
                Entities.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }

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

            if (model.PicTypeId == 1)
            {
                model.Path = filenames[0];
                if (isFile)
                {
                    model.Conver = filenames[1];
                }
            }
            else
            {
                model.Path = String.Join(",", filenames);
            }

            if (model.PicTypeId == 2)
                model.LanguageId = Language.Id;

            Entities.SaveChanges();

            return RedirectToAction("List", new { picSort = 2 });
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
