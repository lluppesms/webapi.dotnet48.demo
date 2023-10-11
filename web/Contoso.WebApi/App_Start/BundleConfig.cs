using System.Diagnostics.CodeAnalysis;
using System.Web.Optimization;

namespace Contoso.WebApi
{
    [ExcludeFromCodeCoverage]
    public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Content/sitecss").Include(
			  "~/Content/bootstrap.css",
			  "~/Content/bootstrap.custom.css",
			  "~/Content/toastr.css",
			  "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/sitejslibs").Include(
			  "~/Scripts/jquery-{version}.js",
			  "~/Scripts/moment.js",
			  "~/Scripts/bootstrap.js",
			  "~/Scripts/jquery.unobtrusive*",
			  "~/Scripts/jquery.validate*"));

			bundles.Add(new StyleBundle("~/Content/DataTablescss").Include(
			  "~/Content/DataTables/css/jquery.dataTables.css",
			  "~/Content/DataTables/css/jquery.dataTables_themeroller.css"));

			bundles.Add(new ScriptBundle("~/bundles/DataTablesjs").Include(
			  "~/Scripts/DataTables/jquery.dataTables.js"));

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new Bundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));
		}
	}
}
