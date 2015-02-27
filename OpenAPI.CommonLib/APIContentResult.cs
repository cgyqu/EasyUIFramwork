//======================================================================
// Copyright (c) 同程网络科技有限公司. All rights reserved.
// 所属项目：$safeprojectname$
// 创 建 人：tc11099
// 创建日期：2010-9-27 9:02:56
// 用    途：请一定在此描述类用途
//====================================================================== 

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