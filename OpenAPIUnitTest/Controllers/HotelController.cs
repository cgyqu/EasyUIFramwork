using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenAPIUnitTest.Controllers
{
	public class HotelController : Controller
	{
		//
		// GET: /Hotel/

		public ActionResult OldAPI()
		{
			return View();
		}

		public ActionResult NewAPI()
		{
			return View();
		}

	}
}
