using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CompanyUI.App_Start
{
    public class WhitespaceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var response = HttpContext.Current.Response;

            response.Filter = new WhiteSpaceFilter(response, s =>
            {
                s = Regex.Replace(s, @"\s+(?=<)|\s+$|(?<=>)\s+", "");

                //single-line doctype must be preserved
                var firstEndBracketPosition = s.IndexOf(">");
                if (firstEndBracketPosition >= 0)
                {
                    s = s.Remove(firstEndBracketPosition, 1);
                    s = s.Insert(firstEndBracketPosition, ">");
                }
                return s;
            });
        }
    }
}