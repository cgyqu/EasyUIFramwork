using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Model;
using UnitTest.Model.Data;

namespace UnitTest.Core
{
	public class CommonBusiness
	{
		public Response AddAPI(OldModel apiInfo)
		{
			try
			{
				using (UnitTestModelDataContext context = new UnitTestModelDataContext())
				{
					OldAPIInfo oldInfo = new OldAPIInfo()
					{
						Cat = apiInfo.Cat,
						SubCat = apiInfo.SubCat,
						CreateTime = DateTime.Now,
						Desc = apiInfo.Desc,
						MethodName = apiInfo.Name,
						RequestData = apiInfo.RequestData,
						FullTypeNameSHA1 = ""
					};
					context.OldAPIInfo.InsertOnSubmit(oldInfo);
					context.SubmitChanges();
					return new Response() { IsSuccess = true, Message = "添加成功。" };
				}
			}
			catch (Exception ex)
			{
				return new Response() { IsSuccess = false, Message = "添加失败，" + ex.Message };
			}
		}
	}
}
