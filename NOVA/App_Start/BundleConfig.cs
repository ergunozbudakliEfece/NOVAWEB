using System.Web;
using System.Web.Optimization;

namespace NOVA
{
    public class BundleConfig
    {
        // Paketleme hakkında daha fazla bilgi için lütfen https://go.microsoft.com/fwlink/?LinkId=301862 adresini ziyaret edin.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-3.6.1.js", "~/Scripts/tippy/popper.min.js", "~/Scripts/tippy/tippy-bundle.umd.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquerymin").Include(
                         "~/Scripts/jquery-3.6.1.min.js", "~/assets/vendor/css/scripts.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js"));
            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                         "~/assets/vendor/js/bootstrap.js", "~/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.js", "~/assets/vendor/js/menu.js", "~/assets/vendor/libs/apex-charts/apexcharts.js", "~/assets/js/main.js", "~/assets/js/dashboards-analytics.js", "~/Scripts/buttons.js", "~/Scripts/boxicons.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.8.3.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css"));
            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                     "~/assets/vendor/js/bootstrap.js", "~/assets/vendor/js/menu.js", "~/assets/js/main.js"));
            bundles.Add(new Bundle("~/bundles/config").Include(
                    "~/assets/vendor/js/helpers.js", "~/assets/js/config.js", "~/assets/js/mutluyillar.js", "~/assets/sweetalert/sweetalert.js"));
            bundles.Add(new StyleBundle("~/Content/test").Include(
                    "~/assets/boxicons-2.1.2/css/boxicons.css", "~/assets/boxicons-2.1.2/css/animations.css", "~/assets/boxicons-2.1.2/css/transformations.css", "~/assets/vendor/css/core.css", "~/assets/vendor/css/theme-default.css", "~/assets/css/demo.css", "~/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.css", "~/assets/vendor/libs/apex-charts/apex-charts.css"));
            bundles.Add(new StyleBundle("~/sevkmus").Include(
                   "~/assets/vendor/css/cssprogress.min.css", "~/assets/vendor/css/accordion.css", "~/assets/vendor/css/Chart.css", "~/Content/select2.css"));
        }
    }
}
