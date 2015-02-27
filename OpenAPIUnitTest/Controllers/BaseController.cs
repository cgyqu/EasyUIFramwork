using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonLib;

namespace OpenAPIUnitTest.Controllers
{
	public class BaseController : Controller
	{
		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			string meau = GetMeauList();
			ViewBag.meau = meau;
		}

		public string GetMeauList()
		{
			return "Test ";
		}
	}
}
