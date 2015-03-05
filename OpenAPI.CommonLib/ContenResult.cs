using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CommonLib
{
    public class APIContent : ActionResult
    {
        public string Content { get; set; }
        public string ContentType { get; set; }

        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// 在自定义输出类
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            ContentResult result = new ContentResult
            {
                Content = this.Content,
                ContentEncoding = this.ContentEncoding ?? Encoding.UTF8,
                ContentType = this.ContentType
            };
            result.ExecuteResult(context);
        }
    }
}