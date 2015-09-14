using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UnitTest.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Windows()
        {
            return View();
        }

        public ActionResult Test(string name, string pwd)
        {
            string result = string.Format("{0}:{1}", name, pwd);
            return Content(result, "text/plain", System.Text.Encoding.UTF8);
        }

    }
}