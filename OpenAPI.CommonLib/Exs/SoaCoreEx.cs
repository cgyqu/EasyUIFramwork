using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TongCheng.SOA.Core.SOAHttp;
using TongCheng.SOA.Core.Entities;

namespace CommonLib
{
	public class SOAResponse<TRspEntity> where TRspEntity : class, IResonseEntity
	{
		public SOAResponse(RequestHeaderEntity requestHeader, SOAResponseEntity<TRspEntity> soaResponse)
		{
			this.RequestHeader = requestHeader;
			this.SoaResponse = soaResponse;
		}
		public RequestHeaderEntity RequestHeader { get; private set; }
		public SOAResponseEntity<TRspEntity> SoaResponse { get; private set; }
	}

	public class SoaException : Exception
	{
		private static string GetMsssagge(ResponseHeaderEntity header)
		{
			StringBuilder sb = new StringBuilder();
			if (header != null)
			{
				sb.AppendFormat("RC:{0}", header.RspCode);
				sb.AppendFormat("RD:{0}", string.Join("", header.RspDesc + "".Take(100)));
				sb.AppendFormat("RE:{0}", string.Join("", header.RspException + "".Take(200)));
			}
			return sb.ToString();
		}

		public string ErrorDesc { get; set; }

		public SoaException(ResponseHeaderEntity header, object soaResponseBody, string methodName)
			: base(GetMsssagge(header))
		{
			this.ErrorDesc = header.RspDesc.Split(new string[] { "\r\n" }, StringSplitOptions.None)[0];
			this.SoaResponseBody = soaResponseBody;
			this.MethodName = methodName;
			this.Header = header;
		}

		public ResponseHeaderEntity Header { get; private set; }

		public string MethodName { get; private set; }

		public object SoaResponseBody { get; private set; }

	}
	public static class SoaCoreEx
	{
		public static SoaException AsSoaEx(this Exception ex)
		{
			var _ex = ex as SoaException;
			return _ex;
		}

		public static T GetResponseBody<T>(this SoaException soaEx)
		{
			if (soaEx != null)
				return (T)soaEx.SoaResponseBody;
			return default(T);
		}

		public static void ThrowEx<TRspEntity>(this SOAResponse<TRspEntity> soaResponseEntity,
			string methodName,
			string errorMsg = "") where TRspEntity : class, IResonseEntity
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				soaResponseEntity.SoaResponse.Header.RspDesc = errorMsg;
			}

			throw new SoaException(soaResponseEntity.SoaResponse.Header, soaResponseEntity.SoaResponse.Body, methodName)
			{

			};
		}

		public static SOAResponse<TRspEntity> GetResposne<TRspEntity>(this IRequestEntity requestEntity,
			string methodName,
			int methodVersion = 1,
			bool throwEx = true)
			where TRspEntity : class, IResonseEntity
		{
			RequestHeaderEntity headerOrder = new RequestHeaderEntity(methodName, methodVersion)
			{
				RequestTimeout = 60000
			};
			SOAResponseEntity<TRspEntity> soaResponseEntity = SOAHttpHelper.GetResposne<TRspEntity>(requestEntity, headerOrder);

			if (NotSuccess(soaResponseEntity.Header) && !soaResponseEntity.IsRequestSuccess)
			{
				if (throwEx)
				{
					throw new SoaException(soaResponseEntity.Header, soaResponseEntity.Body, methodName)
					{

					};
				}
			}

			return new SOAResponse<TRspEntity>(headerOrder, soaResponseEntity);
		}

		private static bool NotSuccess(ResponseHeaderEntity responseHeaderEntity)
		{
			return !responseHeaderEntity.RspCode.StartsWith("0");
		}

		public static void HasBody<TRspEntity>(this SOAResponse<TRspEntity> resp, Action<TRspEntity> hasBodyAction, Action<SOAResponseEntity<TRspEntity>> noBodyAction) where TRspEntity : class, IResonseEntity
		{
			if (resp.SoaResponse.Body != null)
			{
				hasBodyAction(resp.SoaResponse.Body);
			}
			else
			{
				noBodyAction(resp.SoaResponse);
			}
		}
	}
}
