using System.Web;
using System.Web.Optimization;

namespace TaskManagementSystem
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery-ui-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-multiselect.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-multiselect.js",
                      "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
          "~/Scripts/DataTables/jquery.dataTables.js",
          "~/Scripts/DataTables/dataTables.responsive.min.js",
          "~/Scripts/DataTables/dataTables.bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                      "~/Content/DataTables/css/jquery.dataTables.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                        "~/Scripts/jquery.datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
                      "~/Content/jquery.datetimepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                       "~/Scripts/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/toastr").Include(
                      "~/Content/toastr.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/list").Include(
                      "~/Scripts/list.min.js",
                      "~/Scripts/list.pagination.min.js"));
        }
    }
}
