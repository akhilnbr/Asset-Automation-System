using System.Web;
using System.Web.Optimization;

namespace Asset_Automation
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


  


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
         

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

             bundles.Add(new StyleBundle("~/Theme/Content/css").Include(
                      "~/Theme/Content/css/sb-admin-2.min.css",
                       "~/Theme/Content/vendor/fontawesome-free/css/all.min.css"));


            bundles.Add(new ScriptBundle("~/Theme/Content/js").Include(
                          "~/Theme/Content/js/sb-admin-2.min.js",
                          "~/Theme/Content/js/demo/chart-pie-demo.js",
                         "~/Theme/Content/js/demo/chart-area-demo.js"));


            bundles.Add(new ScriptBundle("~/Theme/Vendor/bootstrap/js").Include(
                  
                          "~/Theme/Content/vendor/bootstrap/js/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/Theme/Vendor/jquery-easing").Include(       
                       "~/Theme/Content/vendor/jquery-easing/jquery.easing.min.js"));

            bundles.Add(new ScriptBundle("~/Theme/Vendor/jquery").Include(
                      "~/Theme/Content/vendor/jquery/jquery.min.js"));



        }
    }
}
