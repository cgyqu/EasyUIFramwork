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
		protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
		{
			return base.BeginExecute(requestContext, callback, state);
		}
		public string GetMeauList()
		{
			return "Test ";
		}
	}
}
