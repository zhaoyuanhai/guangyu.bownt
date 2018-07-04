using BowntDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerUI.Controllers
{
    public class AboutUsManagerController : BaseController
    {
        public ActionResult List()
        {
            var model = Entities.tb_About.ToList();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            tb_About abount = Entities.tb_About.FirstOrDefault(a => a.Id == id);
            return View(abount);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tb_About ab)
        {
            tb_About abount = Entities.tb_About.FirstOrDefault(t => t.Id == ab.Id);
            abount.Content = ab.Content;
            abount.Title = ab.Title;
            abount.LanguageId = Language.Id;
            Entities.SaveChanges();
            return View(abount);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(tb_About model)
        {
            model.LanguageId = Language.Id;
            model.Content = model.Content ?? string.Empty;
            Entities.tb_About.Add(model);
            Entities.SaveChanges();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            tb_About pro = Entities.tb_About.FirstOrDefault(t => t.Id == id);
            Entities.tb_About.Remove(pro);
            Entities.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
