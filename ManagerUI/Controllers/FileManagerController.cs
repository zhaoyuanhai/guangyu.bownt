using BowntDAL;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerUI.Controllers
{
    public class FileManagerController : BaseController
    {
        private BowntdbEntities entities;

        public FileManagerController()
        {
            entities = new BowntdbEntities();
        }

        public ActionResult FileType()
        {
            var datas = entities.tb_FileType.Where(x => x.LanguageId == Language.Id).ToList();
            return View(datas);
        }

        public ActionResult List()
        {
            var datas = (from file in entities.tb_File
                         join fileType in entities.tb_FileType
                         on file.TypeId equals fileType.Id
                         where fileType.LanguageId == Language.Id
                         select file).ToList();
            return View(datas);
        }

        public ActionResult SetFileType(int? id)
        {
            var entity = entities.tb_FileType.Find(id);
            return View(entity ?? new tb_FileType());
        }

        [HttpPost]
        public ActionResult SetFileType(tb_FileType model)
        {
            model.LanguageId = Language.Id;
            if (model.Id == 0)
            {
                entities.tb_FileType.Add(model);
            }
            else
            {
                entities.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }

            entities.SaveChanges();
            return RedirectToAction("FileType");
        }

        [HttpGet]
        public ActionResult SetFile(int? id)
        {
            var entity = entities.tb_File.Find(id);
            return View(entity ?? new tb_File());
        }

        [HttpPost]
        public ActionResult SetFile(tb_File model, HttpPostedFileWrapper file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string path = $"/Files/{Guid.NewGuid()}_{file.FileName}";
                file.SaveAs(Server.MapPath("~" + path));
                model.Path = path;
            }

            if (model.Id == 0)
            {
                entities.tb_File.Add(model);
            }
            else
            {
                entities.Entry(model).State = System.Data.Entity.EntityState.Modified;
                if (model.Path == null)
                    entities.Entry(model).Property(x => x.Path).IsModified = false;
            }

            entities.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult DeleteFile(int id)
        {
            var file = entities.tb_File.Find(id);
            entities.tb_File.Remove(file);
            entities.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult DeleteFileType(int id)
        {
            var file = entities.tb_FileType.Find(id);
            entities.tb_FileType.Remove(file);
            entities.SaveChanges();
            return RedirectToAction("FileType");
        }
    }
}