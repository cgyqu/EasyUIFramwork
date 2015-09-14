using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace CommonLib
{

	public class SHAEncryption
	{
		public static string Encrypt(string source)
		{
			byte[] StrRes = Encoding.Default.GetBytes(source);
			HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
			StrRes = iSHA.ComputeHash(StrRes);
			StringBuilder EnText = new StringBuilder();
			foreach (byte iByte in StrRes)
			{
				EnText.AppendFormat("{0:x2}", iByte);
			}
			return EnText.ToString();
		}
	}
	/// <summary>
	/// Des加密--机票用
	/// </summary>
	public class DES
	{
		/// <summary>
		/// 加密密钥
		/// </summary>
		public static readonly string SECRET = "L82V6ZVD6J";

		//public static readonly string SECRET = "0123456789";
		//默认密钥向量
		private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
		/// <summary>
		/// DES加密字符串
		/// </summary>
		/// <param name="encryptString">待加密的字符串</param>
		/// <param name="encryptKey">加密密钥,要求为8位</param>
		/// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
		public static string Encode(string encryptString, string encryptKey)
		{
			byte[] bKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
			byte[] bIV = Keys;
			byte[] bStr = Encoding.UTF8.GetBytes(encryptString);
			try
			{
				DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
				MemoryStream mStream = new MemoryStream();
				CryptoStream cStream = new CryptoStream(mStream, desc.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write);
				cStream.Write(bStr, 0, bStr.Length);
				cStream.FlushFinalBlock();
				return Convert.ToBase64String(mStream.ToArray());
			}
			catch
			{
				return string.Empty;
			}
		}
		/// <summary>
		/// DES解密字符串
		/// </summary>
		/// <param name="decryptString">待解密的字符串</param>
		/// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
		/// <returns>解密成功返回解密后的字符串,失败返源串</returns>
		public static string Decode(string decryptString, string decryptKey)
		{
			try
			{
				byte[] bKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
				byte[] bIV = Keys;
				byte[] bStr = Convert.FromBase64String(decryptString);
				DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
				MemoryStream mStream = new MemoryStream();
				CryptoStream cStream = new CryptoStream(mStream, desc.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write);
				cStream.Write(bStr, 0, bStr.Length);
				cStream.FlushFinalBlock();
				return Encoding.UTF8.GetString(mStream.ToArray());
			}
			catch
			{
				return string.Empty;
			}

		}

	}
	/// <summary>
	/// URL编码/解码类
	/// </summary>
	public sealed class URL
	{
		/// <summary>
		/// URL地址编码(HttpServerUtility.URLEncode方法进行编码)
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public static string Encode(string Input)
		{
			return System.Web.HttpUtility.UrlEncode(Input);
			//return System.Web.HttpUtility.UrlEncode(Input);
			//return HttpContext.Current.Server.UrlEncode(Input);
		}

		/// <summary>
		/// URL地址解码(HttpServerUtility.URLEncode方法进行解码)
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public static string Decode(string Input)
		{
			return System.Web.HttpUtility.UrlDecode(Input);
		}

		/// <summary>
		/// 用HttpUtility.UrlEncode方法对URL进行编码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string URLEncode(string str)
		{
			return HttpUtility.UrlEncode(str);
		}

		/// <summary>
		/// 用HttpUtility.UrlEncode方法对URL进行解码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string URLDecode(string str)
		{
			return HttpUtility.UrlDecode(str);
		}
	}

	/// <summary>
	/// AES 加密类
	/// </summary>
	public class AESEncryption
	{
		#region AES ECB模式算法
		/// <summary>
		/// AES加密 ECB模式算法
		/// </summary>
		/// <param name="toEncrypt">需要加密的原文</param>
		/// <param name="key">加密的密钥（128位，196位，256位）</param>
		/// <returns>加密的密文</returns>
		public static byte[] ECBEncrypt(string toEncrypt, string key)
		{
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
			RijndaelManaged rDel = new RijndaelManaged();
			rDel.KeySize = 128;
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.Zeros;
			ICryptoTransform cTransform = rDel.CreateEncryptor();
			return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
		}

		/// <summary>
		/// AES加密 ECB模式算法
		/// 转Base64
		/// </summary>  
		/// <param name="plainText">需要加密的密文</param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string ECBEncriptyToBase64(string plainText, string key)
		{
			byte[] resultArray = ECBEncrypt(plainText, key);
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		/// <summary>
		/// AES解密 ECB模式算法
		/// </summary>
		/// <param name="toDecrypt">需要解密的密文</param>
		/// <param name="key">解密的密钥（128位，196位，256位）</param>
		/// <returns>解密的原文</returns>
		public static string ECBDecrypt(string toDecrypt, string key)
		{
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
			byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
			RijndaelManaged rDel = new RijndaelManaged();
			rDel.KeySize = 128;
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.Zeros;
			ICryptoTransform cTransform = rDel.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			return UTF8Encoding.UTF8.GetString(resultArray);
		}

		/// <summary>
		/// AES解密 ECB模式算法by PKCS7
		/// </summary>
		/// <param name="toDecrypt">需要解密的密文</param>
		/// <param name="key">解密的密钥（128位，196位，256位）</param>
		/// <returns>解密的原文</returns>
		public static string ECBDecryptByPKC(string toDecrypt, string key)
		{
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
			byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

			RijndaelManaged rDel = new RijndaelManaged();
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = rDel.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

			return UTF8Encoding.UTF8.GetString(resultArray);
		}

		public static string ECBEncryptByPKC(string toEncrypt, string key)
		{
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
			RijndaelManaged rDel = new RijndaelManaged();
			rDel.KeySize = 128;
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.PKCS7;
			ICryptoTransform cTransform = rDel.CreateEncryptor();
			var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}
		#endregion
		/// <summary>
		/// 兼容目前数字签名的加密
		/// </summary>
		/// <param name="toEncrypt"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string EncryptByPKC(string toEncrypt, string key, string time)
		{
			DateTime dt = time.ToDateTime();
			int site = dt.Second % 10;
			if (site == 9)
			{
				site = 0;
			}
			return AESEncryption.ECBEncryptByPKC(toEncrypt, key.Substring(site, 32));
		}
		/// <summary>
		/// 兼容目前数字签名的解密
		/// </summary>
		/// <param name="toDecrypt">要解密的字符串</param>
		/// <param name="key">数字签名</param>
		/// <param name="time">请求时间</param>
		/// <returns></returns>
		public static string DecryptByPKC(string toDecrypt, string key, string time)
		{
			DateTime dt = time.ToDateTime();
			int site = dt.Second % 10;
			if (site == 9)
			{
				site = 0;
			}
			return AESEncryption.ECBDecryptByPKC(toDecrypt, key.Substring(site, 32));
		}



		void test()
		{
			//<cardNumber>tuFZQHdj2mWqKdsD0zvpe7Awka7krzdRMYUVHiqHclo=</cardNumber>
			//<cardType>wkFq72LhjA7HVJ3GHH61Wg==</cardType>
			//<valiCode>4xFR/FMKwgC8ryk2dPRxdg==</valiCode>
			//<masterName>Lm90BVxUFLt0JnmCotyUtg==</masterName>
			//<periodDate>V1ime0pVFONymADt2MBE0g==</periodDate>
			//<certificatesType>89RM/Tik/jlSIKsXrymtsA==</certificatesType>
			//<certificatesNumber>DBdJMLS0thlIVosIT0YAIE0cmARXz290TP4wnLoqOP0=</certificatesNumber>
			string key = "4d3622eedee194e165dbaddc84a97ac42ffe4b0f";
			string time = "2014-04-02 10:58:14.457";
			string s = EncryptByPKC("测试", key, time);
			Console.WriteLine(s);
			Console.WriteLine(DecryptByPKC(s, key, time));
			Console.WriteLine();

			Console.WriteLine(DES.Decode("YdXVjzbbRAI%3d", DES.SECRET));
			Console.WriteLine(DES.Decode("2ni9KeWmmxY%3d", DES.SECRET));



			string d = "4b2af30b398dd27f8128584ad3af716a";

			Console.WriteLine(ECBDecryptByPKC("Kj6/nWHUK86IYPJUOWnZ9A==", d) + "---信用卡");

			Console.WriteLine(ECBDecryptByPKC("qDcT+PL98sgPBF7b7LXTpg==", "5d5448906fc8f105dc71888b89145226") + "xxxxxxxx");
			Console.WriteLine(ECBDecryptByPKC("wkFq72LhjA7HVJ3GHH61Wg==", "52c5e78db000a553a98825c16dcb4516"));
			Console.WriteLine(ECBDecryptByPKC("4xFR/FMKwgC8ryk2dPRxdg==", "52c5e78db000a553a98825c16dcb4516"));
			Console.WriteLine(ECBDecryptByPKC("Lm90BVxUFLt0JnmCotyUtg==", "52c5e78db000a553a98825c16dcb4516"));
			Console.WriteLine(ECBDecryptByPKC("V1ime0pVFONymADt2MBE0g==", "52c5e78db000a553a98825c16dcb4516"));
			Console.WriteLine(ECBDecryptByPKC("89RM/Tik/jlSIKsXrymtsA==", "52c5e78db000a553a98825c16dcb4516"));
			Console.WriteLine(ECBDecryptByPKC("DBdJMLS0thlIVosIT0YAIE0cmARXz290TP4wnLoqOP0=", "52c5e78db000a553a98825c16dcb4516"));


			Console.WriteLine("-----------------");
			Console.WriteLine("<cardNumber>" + ECBEncryptByPKC("4432260017325612", d) + "</cardNumber>");
			Console.WriteLine("<cardType>" + ECBEncryptByPKC("中信银行信用卡", d) + "</cardType>");
			//Console.WriteLine("<cardNumber>" + ECBEncryptByPKC("3568891104058409", d) + "</cardNumber>");
			//Console.WriteLine("<cardType>" + ECBEncryptByPKC("招商银行信用卡", d) + "</cardType>");

			//            3568891104058409 
			//招商银行信用卡

			Console.WriteLine("<valiCode>" + ECBEncryptByPKC("866", d) + "</valiCode>");
			Console.WriteLine("<masterName>" + ECBEncryptByPKC("叶xx", d) + "</masterName>");
			Console.WriteLine("<periodDate>" + ECBEncryptByPKC("2016-1-1", d) + "</periodDate>");
			Console.WriteLine("<certificatesType>" + ECBEncryptByPKC("身份证", d) + "</certificatesType>");
			Console.WriteLine("<certificatesNumber>" + ECBEncryptByPKC("320586199003290510", d) + "</certificatesNumber>");
			Console.WriteLine("<masterMobileNumber>" + ECBEncryptByPKC("18605121225", d) + "</masterMobileNumber>");

			Console.WriteLine("-----------------");


			Console.WriteLine("<cardholderName>" + ECBEncryptByPKC("汤晓华", d) + "</cardholderName>");
			Console.WriteLine("<cardNumber>" + ECBEncryptByPKC("4013992278833442", d) + "</cardNumber>");
			Console.WriteLine("<cardType>" + ECBEncryptByPKC("中信银行信用卡", d) + "</cardType>");
			Console.WriteLine("<validYear>" + ECBEncryptByPKC("2013", d) + "</validYear>");
			Console.WriteLine("<validMonth>" + ECBEncryptByPKC("9", d) + "</validMonth>");
			Console.WriteLine("<verifyCode>" + ECBEncryptByPKC("291", d) + "</verifyCode>");
			Console.WriteLine("<certificateType>" + ECBEncryptByPKC("身份证", d) + "</certificateType>");
			Console.WriteLine("<certificateNumber>" + ECBEncryptByPKC("220821198609260025", d) + "</certificateNumber>");

			//    <cardholderName>祁济</cardholderName>
			//<cardNumber>5203823130018954</cardNumber>
			//<cardType>广东发展银行信用卡</cardType>
			//<validYear>2013</validYear>
			//<validMonth>9</validMonth>
			//<verifyCode>291</verifyCode>
			//<certificateType>身份证</certificateType>
			//<certificateNumber>220821198609260025</certificateNumber>
		}

	}

}
