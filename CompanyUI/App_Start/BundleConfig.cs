using System.Web;
using System.Web.Optimization;

namespace CompanyUI
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/Css").Include(
                    "~/Content/Plugins/bootstrap/css/bootstrap.css",
                    "~/Content/Plugins/Font-Awesome-3.2.1/css/font-awesome.css",
                    "~/Content/Plugins/layer2.4/skin/layer.css",
                    "~/Content/Site.css"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/Js").Include(
                    "~/Content/Plugins/jquery-3.0.0/jquery-3.0.0.js",
                    "~/Scripts/pages/bownTable.js",
                    "~/Content/Plugins/bootstrap/js/bootstrap.js",
                    "~/Content/Plugins/layer2.4/layer.js"
            ));
        }
    }
}