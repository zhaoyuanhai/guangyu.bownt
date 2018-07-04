using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Common
{
    public class CommonHelper
    {
        public static string GetNowLangTime()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmss");
        }
    }
}