using System.Web;
using System.Web.Optimization;

namespace DSS.MoHra
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.matchHeight.js",
                        "~/Scripts/jquery.draggable.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/moment-with-locales.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminlte").Include(
                      "~/Scripts/select2.full.js",
                      "~/Scripts/select2.i18n.ru.js",
                      "~/Scripts/adminlte.app.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/Site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/select2.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/AdminLTE.css",
                      "~/Content/AdminLTE_Skins/_all-skins.min.css",
                      "~/Content/site.css"));
        }
    }
}
