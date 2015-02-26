using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Cryptography;

namespace CommonLib
{
	#region 用于联系 Internet 资源的请求方法。默认值是 GET。
	/// <summary>
	/// 用于联系 Internet 资源的请求方法。默认值是 GET。
	/// </summary>
	public enum HttpWebRequestMethod
	{
		///<summary>
		/// GET方式
		///</summary>
		GET,
		///<summary>
		/// Post方式
		///</summary>
		POST
	}

	public class ContenType
	{
		public const string CommonJson = "text/json";
		public const string Json = "application/json";
		public const string Xml = "application/xml;charset=utf-8";
		public const string Common = "application/x-www-form-urlencoded";
	}

	#endregion

	public class HttpException : Exception
	{
		public HttpException(string url, long time, Exception ex)
			: base(string.Format("http异常,{0},url:{1},耗时:{2}", ex.Message, url, time, ex))
		{

		}
	}

	public static class CommonHelper
	{
		public static string Foramt(this DateTime date, string f = "yyyy-MM-dd HH:mm:ss")
		{
			return date.ToString(f);
		}
		public static string RemoveHtmlTag(this string html)
		{
			System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

			html = regex1.Replace(html, ""); //过滤<script></script>标记
			html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
			html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on事件
			html = regex4.Replace(html, ""); //过滤iframe
			html = regex5.Replace(html, ""); //过滤frameset
			html = regex6.Replace(html, ""); //过滤frameset
			html = regex7.Replace(html, ""); //过滤frameset
			html = regex8.Replace(html, ""); //过滤frameset
			html = html.Replace(" ", "");
			html = html.Replace("</strong>", "");
			html = html.Replace("<strong>", "");

			html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
			html = html.Replace("<", "");
			html = html.Replace(">", "");
			html = html.Replace("\r\n", "").Replace("&mdash;", "").Replace("&ldquo;", "").Replace("&rdquo;", "");

			return html.Trim().TrimStart(';').TrimStart(',');
		}

		#region 访问远程URL
		/// <summary>
		/// 通过HttpWebRequest方法从服务器取方法
		/// </summary>
		/// <param name="url">需请求的URL</param>
		/// <returns>返回的字符串</returns>
		public static string GetDataFromServer(string url)
		{
			return GetDataFromServer(url, string.Empty);
		}

		/// <summary>
		/// 通过HttpWebRequest方法从服务器取方法
		/// </summary>
		/// <param name="url">需请求的URL</param>
		/// <param name="inputCharset">字符集</param>
		/// <returns>返回的字符串</returns>
		public static string GetDataFromServer(string url, string inputCharset)
		{
			HttpWebRequest webRequest;
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				webRequest = (HttpWebRequest)WebRequest.Create(url);
				webRequest.KeepAlive = false;
				webRequest.Timeout = 30 * 1000;
				using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
				{
					using (Stream resStream = response.GetResponseStream())
					{
						Encoding encoding = null;

						if (!string.IsNullOrEmpty(inputCharset))
						{
							try
							{
								encoding = Encoding.GetEncoding(inputCharset);
							}
							catch (Exception)
							{
							}
						}

						StreamReader reader = null;

						if (encoding != null)
						{
							reader = new StreamReader(resStream, encoding);
						}
						else
						{
							reader = new StreamReader(resStream);
						}
						timer.Stop();
						return reader.ReadToEnd();
					}
				}
			}
			catch (Exception ex)
			{
				timer.Stop();
				throw new HttpException(url, timer.ElapsedMilliseconds, ex);
			}
		}


		public static string UnGZip(string value)
		{
			try
			{
				//Transform string into byte[]  
				byte[] byteArray = new byte[value.Length];
				int indexBA = 0;
				foreach (char item in value.ToCharArray())
				{
					byteArray[indexBA++] = (byte)item;
				}

				//Prepare for decompress  
				System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray);
				System.IO.Compression.GZipStream sr = new System.IO.Compression.GZipStream(ms,
					System.IO.Compression.CompressionMode.Decompress);


				//Reset variable to collect uncompressed result  
				byteArray = new byte[byteArray.Length];


				//Decompress  
				int rByte = sr.Read(byteArray, 0, byteArray.Length);


				//Transform byte[] unzip data to string  
				System.Text.StringBuilder sB = new System.Text.StringBuilder(rByte);
				//Read the number of bytes GZipStream red and do not a for each bytes in  
				//resultByteArray;  
				for (int i = 0; i < rByte; i++)
				{
					sB.Append((char)byteArray[i]);
				}
				sr.Close();
				ms.Close();
				sr.Dispose();
				ms.Dispose();
				return sB.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("GZIP解压时发生错误", ex);
			}
		}

		/// <summary>
		/// 通过HttpWebRequest方法从服务器取方法
		/// </summary>
		/// <param name="url">需请求的URL</param>
		/// <param name="inputCharset">字符集</param>
		/// <returns>返回的字符串</returns>
		public static string GetDataFromServerWithGZIP(string url, string inputCharset)
		{

			HttpWebRequest webRequest;
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				webRequest = (HttpWebRequest)WebRequest.Create(url);
				webRequest.KeepAlive = false;
				webRequest.Timeout = 30000;
				webRequest.Headers["Accept-Encoding"] = "gzip,deflate";
				webRequest.AutomaticDecompression = DecompressionMethods.GZip;

				string result = string.Empty;
				using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
				{
					using (Stream resStream = response.GetResponseStream())
					{
						Encoding encoding = null;

						if (!string.IsNullOrEmpty(inputCharset))
						{
							encoding = Encoding.GetEncoding(inputCharset);
						}

						StreamReader reader = null;

						if (encoding != null)
						{
							reader = new StreamReader(resStream, encoding);
						}
						else
						{
							reader = new StreamReader(resStream);
						}
						result = reader.ReadToEnd();
						timer.Stop();
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				timer.Stop();
				throw new HttpException(url, timer.ElapsedMilliseconds, ex);
			}
		}

		/***************************************************************************************************************
		 * 这里的PostDataToServer与CallUrl需要进行整合，原CallUrl方法分别由GetDataFromServer和PostDataToServer替代。
		 * 最终将去除CallUrl方法。
		 * Remark By Ljj, 2011-9-2 9:20
		 ***************************************************************************************************************/

		/// <summary>
		/// 向服务器提交XML数据
		/// </summary>
		/// <param name="url">远程访问的地址</param>
		/// <param name="data">参数</param>
		/// <param name="method">Http页面请求方法</param>
		/// <returns>远程页面调用结果</returns>
		public static string PostDataToServer(string url, string data, HttpWebRequestMethod method)
		{
			HttpWebRequest request;
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				request = WebRequest.Create(url) as HttpWebRequest;
				request.Timeout = 30000;
				switch (method)
				{
					case HttpWebRequestMethod.GET:
						request.Method = HttpWebRequestMethod.GET.ToString();
						break;
					case HttpWebRequestMethod.POST:
						{
							request.Method = HttpWebRequestMethod.POST.ToString();

							byte[] bdata = Encoding.UTF8.GetBytes(data);
							request.ContentType = "application/xml;charset=utf-8";
							request.ContentLength = bdata.Length;

							Stream streamOut = request.GetRequestStream();
							streamOut.Write(bdata, 0, bdata.Length);
							streamOut.Close();
						}
						break;
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream streamIn = response.GetResponseStream();

				StreamReader reader = new StreamReader(streamIn);
				string result = reader.ReadToEnd();
				reader.Close();
				streamIn.Close();
				response.Close();
				timer.Stop();
				return result;
			}
			catch (Exception ex)
			{
				timer.Stop();
				throw new HttpException(url, timer.ElapsedMilliseconds, ex);
			}
		}

		/// <summary>
		/// 向服务器提交普通数据
		/// </summary>
		/// <param name="url">远程访问的地址</param>
		/// <param name="data">参数</param>
		/// <param name="method">Http页面请求方法</param>
		/// <returns>远程页面调用结果</returns>
		public static string PostNormalDataToServer(string url, string data, HttpWebRequestMethod method)
		{
			HttpWebRequest request;
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				request = WebRequest.Create(url) as HttpWebRequest;
				request.Timeout = 30000;
				switch (method)
				{
					case HttpWebRequestMethod.GET:
						request.Method = HttpWebRequestMethod.GET.ToString();
						break;
					case HttpWebRequestMethod.POST:
						{
							request.Method = HttpWebRequestMethod.POST.ToString();

							byte[] bdata = Encoding.UTF8.GetBytes(data);
							request.ContentType = "application/x-www-form-urlencoded";
							request.ContentLength = bdata.Length;

							Stream streamOut = request.GetRequestStream();
							streamOut.Write(bdata, 0, bdata.Length);
							streamOut.Close();
						}
						break;
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream streamIn = response.GetResponseStream();

				StreamReader reader = new StreamReader(streamIn);
				string result = reader.ReadToEnd();
				reader.Close();
				streamIn.Close();
				response.Close();
				timer.Stop();
				return result;
			}
			catch (Exception ex)
			{
				timer.Stop();
				throw new HttpException(url, timer.ElapsedMilliseconds, ex);
			}
		}

		/// <summary>
		/// 向服务器提交XML数据
		/// </summary>
		/// <param name="url">远程访问的地址</param>
		/// <param name="data">参数</param>
		/// <returns>远程页面调用结果</returns>
		public static string PostDataToServer(string url, string data)
		{
			HttpWebRequest request;
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				request = WebRequest.Create(url) as HttpWebRequest;

				request.Method = HttpWebRequestMethod.POST.ToString();

				byte[] bdata = Encoding.UTF8.GetBytes(data);
				request.ContentType = "application/xml;charset=utf-8";
				request.ContentLength = bdata.Length;

				Stream streamOut = request.GetRequestStream();
				streamOut.Write(bdata, 0, bdata.Length);
				streamOut.Close();

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream streamIn = response.GetResponseStream();

				StreamReader reader = new StreamReader(streamIn);
				string result = reader.ReadToEnd();
				reader.Close();
				streamIn.Close();
				response.Close();
				timer.Stop();
				return result;
			}
			catch (Exception ex)
			{
				timer.Stop();
				throw new HttpException(url, timer.ElapsedMilliseconds, ex);
			}
		}

		///<summary>
		///</summary>
		///<param name="uriString"></param>
		///<param name="postString"></param>
		///<returns></returns>
		public static string PostDataByWebClient(string uriString, string postString)
		{
			WebClient webClient = new WebClient();
			webClient.Headers.Add("Content-Type", "application/xml;charset=utf-8");

			// 将字符串转换成字节数组
			byte[] postData = Encoding.UTF8.GetBytes(postString);
			// 上传数据，返回页面的字节数组
			byte[] responseData = webClient.UploadData(uriString, "POST", postData);
			//     ASP.NET 返回的页面一般是Unicode,如果是简体中文应使用 
			//     Encoding.GetEncoding("GB2312").GetString(responseData)
			// 返回的将字节数组转换成字符串(HTML)
			string srcString = Encoding.UTF8.GetString(responseData);

			return srcString;
		}

		/// <summary>
		/// post请求，制定contentType
		/// </summary>
		/// <param name="url"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string PostDataToServer(string url, string data, string contentType)
		{
			HttpWebRequest request;
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				request = WebRequest.Create(url) as HttpWebRequest;

				request.Method = HttpWebRequestMethod.POST.ToString();

				byte[] bdata = Encoding.UTF8.GetBytes(data);
				request.ContentType = contentType;
				request.ContentLength = bdata.Length;
				request.Accept = "application/json";

				Stream streamOut = request.GetRequestStream();
				streamOut.Write(bdata, 0, bdata.Length);
				streamOut.Close();

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream streamIn = response.GetResponseStream();

				StreamReader reader = new StreamReader(streamIn);
				string result = reader.ReadToEnd();
				reader.Close();
				streamIn.Close();
				response.Close();
				timer.Stop();
				return result;
			}
			catch (Exception ex)
			{
				timer.Stop();
				throw new HttpException(url, timer.ElapsedMilliseconds, ex);
			}
		}



		static void Test()
		{
			try
			{
				var str = PostDataByWebClient("http://58.240.86.161/esbadapter/ChangeDataPush/push ", postdata);
				//var str = PostDataToServer("http://58.240.86.161/esbadapter/RefundDataPush/push", postdata);
				Console.WriteLine(str);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private static string postdata = "<?xml version=\"1.0\" encoding=\"utf-8\"?><request><header><serviceName>PushChange</serviceName></header><body><order><cNo>123456</cNo><cDate>2012-04-12</cDate><oDate>2012-04-12</oDate><cNoB>1223</cNoB><orderId>123456</orderId><selfOrderId>123456</selfOrderId><cStatus>1</cStatus><rMsg>失败原因</rMsg><oa>SHA</oa><aa>PEK</aa><oCom>HU</oCom><nCom>HU</nCom><oFNo>F</oFNo><nFNo>A</nFNo><oFDate>2012-04-12</oFDate><nFDate>2012-04-12</nFDate><oFTime>2012-04-12</oFTime><nFTime>2012-04-12</nFTime><oRoom>C</oRoom><nRoom>C</nRoom><pList><passenger><pId>123456</pId><pIdB>588</pIdB><pName>乘客姓名</pName><pType>2</pType></passenger></pList></order></body></request>";

		/// <summary>
		/// 向服务器提交XML数据
		/// </summary>
		/// <param name="url">远程访问的地址</param>
		/// <param name="data">参数</param>
		/// <param name="method">Http页面请求方法</param>
		/// <param name="inputCharset"></param>
		/// <returns>远程页面调用结果</returns>
		public static string PostDataToServer(string url, string data, HttpWebRequestMethod method, string inputCharset)
		{
			HttpWebRequest request;

			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				request = WebRequest.Create(url) as HttpWebRequest;
				request.Timeout = 30000;
				Encoding encoding = null;
				if (!string.IsNullOrEmpty(inputCharset))
				{
					try
					{
						encoding = Encoding.GetEncoding(inputCharset);
					}
					catch (Exception)
					{
					}
				}

				if (encoding == null)
				{
					encoding = Encoding.Default;
				}

				switch (method)
				{
					case HttpWebRequestMethod.GET:
						request.Method = HttpWebRequestMethod.GET.ToString();
						break;
					case HttpWebRequestMethod.POST:
						{
							request.Method = HttpWebRequestMethod.POST.ToString();

							byte[] bdata = encoding.GetBytes(data);

							request.ContentType = string.Format("application/xml;charset={0}", encoding.WebName);
							request.ContentLength = bdata.Length;

							Stream streamOut = request.GetRequestStream();
							streamOut.Write(bdata, 0, bdata.Length);
							streamOut.Close();
						}
						break;
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream streamIn = response.GetResponseStream();

				StreamReader reader = new StreamReader(streamIn, encoding);
				string result = reader.ReadToEnd();
				reader.Close();
				streamIn.Close();
				response.Close();
				timer.Stop();
				return result;
			}
			catch (Exception ex)
			{
				timer.Stop();
				throw new HttpException(url, timer.ElapsedMilliseconds, ex);
			}
		}

		/// <summary>
		/// Form方式提交数据到url
		/// </summary>
		/// <param name="url">请求url</param>
		/// <param name="dataList">请求的参数</param>
		/// <param name="method">请求方式POST/GET</param>
		/// <param name="inputCharset">请求格式，默认utf-8</param>
		/// <returns></returns>
		public static string PostDataToServer(string url, NameValueCollection dataList, HttpWebRequestMethod method, string inputCharset = "utf-8")
		{
			Stopwatch timer = Stopwatch.StartNew();
			WebClient WebClientObj = new WebClient();
			try
			{
				Encoding encoding = null;
				if (!string.IsNullOrEmpty(inputCharset))
				{
					encoding = Encoding.GetEncoding(inputCharset);
				}

				byte[] byRemoteInfo = WebClientObj.UploadValues(url, method.ToString(), dataList);
				timer.Stop();
				return encoding.GetString(byRemoteInfo);
			}
			catch (Exception ex)
			{
				timer.Stop();
				throw new HttpException(url, timer.ElapsedMilliseconds, ex);
			}
		}
		#endregion


		public static T ParseFromJson<T>(string szJson)
		{
			using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
			{
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
				return (T)serializer.ReadObject(ms);
			}
		}

		public static string ParseToJson(object dd)
		{
			System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
			return jss.Serialize(dd);

			//DataContractJsonSerializer json = new DataContractJsonSerializer(dd.GetType());
			//using (MemoryStream stream = new MemoryStream())
			//{
			//    json.WriteObject(stream, dd);
			//    string szJson = Encoding.UTF8.GetString(stream.ToArray());
			//    return szJson;
			//}
		}

		public static string[] BubbleSort(string[] originalArray)
		{
			int i, j;
			string temp;
			bool exchange;          //交换标志 

			for (i = 0; i < originalArray.Length; i++)  //最多做R.Length-1趟排序 
			{
				exchange = false;   //本趟排序开始前，交换标志应为假

				for (j = originalArray.Length - 2; j >= i; j--)
				{
					//判断交换条件
					if (String.CompareOrdinal(originalArray[j + 1], originalArray[j]) < 0)
					{
						temp = originalArray[j + 1];
						originalArray[j + 1] = originalArray[j];
						originalArray[j] = temp;

						exchange = true; //发生了交换，故将交换标志置为真 
					}
				}

				if (!exchange) //本趟排序未发生交换，提前终止算法 
				{
					break;
				}
			}
			return originalArray;
		}

		public static string GetMD5ByArray(string[] sortedArray, string key, string charset)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < sortedArray.Length; i++)
			{
				if (i == sortedArray.Length - 1)
				{
					builder.Append(sortedArray[i]);
				}
				else
				{
					builder.Append(sortedArray[i] + "&");
				}
			}
			builder.Append(key);
			return GetMD5(builder.ToString(), charset);
		}
		public static string GetMD5(string input, string charset)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] data = md5.ComputeHash(Encoding.GetEncoding(charset).GetBytes(input));
			StringBuilder builder = new StringBuilder(32);
			for (int i = 0; i < data.Length; i++)
			{
				builder.Append(data[i].ToString("x2"));
			}
			return builder.ToString();
		}
	}

}
