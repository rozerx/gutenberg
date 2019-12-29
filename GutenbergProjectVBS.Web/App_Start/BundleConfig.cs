using System.Web.Optimization;

namespace GutenbergProjectVBS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Assets/js").Include(
                        "~/Assets/Vendor/jquery/jquery.min.js",
                        "~/Assets/Vendor/bootstrap/bootstrap.bundle.min.js",
                        "~/Assets/Vendor/jquery-easing/jquery.easing.min.js",
                        "~/Assets/Sb-Admin/sb-admin-2.min.js"));

            bundles.UseCdn = true;
            var fontsGoogleApis = "https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i";

            bundles.Add(new StyleBundle("~/Assets/css", fontsGoogleApis).Include(
                      "~/Assets/style.css",
                      "~/Assets/Sb-Admin/sb-admin-2.min.css",
                      "~/Assets/Vendor/fontawesome-free/css/all.min.css"));
        }
    }
}
