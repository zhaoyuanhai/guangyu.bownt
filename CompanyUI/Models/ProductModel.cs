using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyUI.Models
{
    public class ProductModel : ModelBase
    {
        public ProductModel() { }

        public ProductModel(object obj)
            : base(obj) { }

        public string Icon { get; set; }
        public string ProdectName { get; set; }
        public int NormsId { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public string Conver { get; set; }
        public string Content { get; set; }
        public int LanguageId { get; set; }
    }
}