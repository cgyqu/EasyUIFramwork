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
			return View();
		}


		public ActionResult Test()
		{
			return View();
		}

        public ActionResult AddTabs() 
        {
            return View();
        }
	}
}
