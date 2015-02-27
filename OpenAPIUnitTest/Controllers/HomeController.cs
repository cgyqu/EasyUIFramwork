using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonLib;

namespace OpenAPIUnitTest.Controllers
{
	public class HomeController : Controller
	{
		//
		// GET: /Home/

		public ActionResult Index()
		{
			return new APIContentResult() { Content = "Index", ContentType = "text/plain" };
		}


		public ActionResult Test()
		{
			return View();
		}
	}
}
