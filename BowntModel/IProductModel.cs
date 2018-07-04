using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyModel
{
    public interface IProductModel
    {
        int Id { get; set; }

        int TypeId { get; set; }

        int LanguageId { get; set; }

        string Title { get; set; }

        string Content { get; set; }
    }
}
