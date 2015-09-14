using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace UnitTest
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/js").Include(
				  "~/resource/js/jquery.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/resource/css/ht_reset.css"));
		}
	}
}