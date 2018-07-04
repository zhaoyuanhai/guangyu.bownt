using BowntDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BowntBLL
{
    public class LanguageBll
    {
        BowntdbEntities dao;

        public LanguageBll()
        {
            dao = new BowntdbEntities();
        }

        public List<tb_Language> GetAllLanguage()
        {
            return dao.tb_Language.ToList();
        }
    }
}