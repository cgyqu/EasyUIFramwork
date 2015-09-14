using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace CommonLib
{

	public class APIContentResult : ActionResult
	{
		public string Content { get; set; }
		public string ContentType { get; set; }
		public Encoding ContentEncoding { get; set; }


		public override void ExecuteResult(ControllerContext context)
		{
			ContentResult result = new ContentResult
			{
				Content = Content,
				ContentEncoding = ContentEncoding ?? Encoding.UTF8,
				ContentType = ContentType
			};
			result.ExecuteResult(context);
		}
	}
}