using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Model;

namespace UnitTest.Core
{
	public class Response
	{
		/// <summary>
		/// 是否成功
		/// </summary>
		public bool IsSuccess { get; set; }
		/// <summary>
		/// 返回信息
		/// </summary>
		public string Message { get; set; }
	}
}
