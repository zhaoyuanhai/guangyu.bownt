using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyUI.Models
{
    public class LanguageModel
    {
        public LanguageModel()
        { }

        public LanguageModel(int id, string language, string logogram)
        {
            this.Id = id;
            this.Language = language;
            this.Logogram = logogram;
        }

        public int Id { get; set; }

        public string Language { get; set; }

        public string Logogram { get; set; }
    }
}