using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BowntDAL;
using CompanyUI.Models;
using CompanyUI.App_Start;

namespace CompanyUI.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult HomePage() => View();

        public ActionResult AboutBownt() => View();

        public ActionResult Applications() => View();

        public ActionResult Products() => View();

        public ActionResult Solution() => View();

        public ActionResult Innovation() => View();

        public ActionResult DownLoad() => View();

        public ActionResult PreView(int id, string type)
        {
            BowntdbEntities entity = new BowntdbEntities();
            string url = string.Empty;
            if (type == "tb_Picture")
            {
                var picture = entity.tb_Picture.Find(id);
                url = picture.Path;
            }
            else
            {
                var file = entity.tb_File.Find(id);
                url = file.Path;
            }
            return View(url);
        }

        public ActionResult GetPdf(int id)
        {
            BowntdbEntities entity = new BowntdbEntities();
            var pdf = entity.tb_DownLoad.FirstOrDefault(a => a.Id == id);
            try
            {
                if (Request["isLoad"].Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    return File(Server.MapPath(pdf.FilePath), "application/pdf");
                }
                else
                {
                    return File(Server.MapPath(pdf.FilePath), "application/pdf", pdf.Name + ".pdf");
                }
            }
            catch (Exception ex)
            {
                return Content("未找到文件");
            }
        }

        public ActionResult ProductDetial(int id)
        {
            BowntdbEntities entity = new BowntdbEntities();
            var product = entity.tb_Prodect.FirstOrDefault(a => a.Id == id);
            return View(product);
        }

        public ActionResult GetProdectByNormId(int id, PageingModel<ProductModel> pageingModel)
        {
            BowntdbEntities entity = new BowntdbEntities();
            IQueryable<tb_Prodect> productList;

            if (id == 0)
                productList = entity.tb_Prodect.Where(a => a.LanguageId == Language.Id);
            else
                productList = entity.tb_Prodect.Where(a => a.LanguageId == Language.Id && a.NormsId == id);

            //分页
            int total = productList.Count();
            var data = productList.OrderBy(m => m.Id)
                .Skip((pageingModel.PageIndex - 1) * pageingModel.PageSize)
                .Take(pageingModel.PageSize)
                .ToList().Select(a => new ProductModel(a));

            //初始化分页对象
            pageingModel.Data = data.ToList();
            pageingModel.Total = total;
            pageingModel.TotalPage = (int)Math.Ceiling(total / (pageingModel.PageSize * 1.0));
            pageingModel.TotalPage = pageingModel.TotalPage == 0 ? 1 : pageingModel.TotalPage;
            return Json(pageingModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test() => View();
    }
}
