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
    public class SolutionManagerController : BaseController
    {
        public ActionResult Edit()
        {
            var model = Entities.tb_Solution.FirstOrDefault(t => t.LanguageId == Language.Id);
            return View(model ?? new tb_Solution());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tb_Solution model)
        {
            tb_Solution solnew = Entities.tb_Solution
                .FirstOrDefault(t => t.LanguageId == Language.Id) ?? new tb_Solution();

            solnew.LanguageId = Language.Id;
            solnew.Content = model.Content;

            //id=0 添加
            if(solnew.Id == 0)
                Entities.tb_Solution.Add(solnew);
            
            Entities.SaveChanges();
            return RedirectToAction("Edit");
        }

        public ActionResult Delete(int id)
        {
            tb_Solution pro = Entities.tb_Solution.FirstOrDefault(t => t.Id == id);
            Entities.tb_Solution.Remove(pro);
            Entities.SaveChanges();
            return RedirectToAction("List");

        }
    }
}
