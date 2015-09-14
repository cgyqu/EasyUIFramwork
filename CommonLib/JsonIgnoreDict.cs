using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CommonLib
{
	public class ResultIgnoreSource<TSource>
	{
		private Dictionary<string, int> IgnoreDict = new Dictionary<string, int>();
		public List<string> IgnoreList
		{
			get
			{
				return IgnoreDict.Keys.ToList();
			}
		}
		public ResultIgnoreSource<TSource> Ignore<TProperty>(Expression<Func<TSource, TProperty>> express)
		{
			if (express == null)
			{
				throw new ArgumentNullException("express");
			}
			MemberExpression memberExpression = express.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("请为类型 \"" + typeof(TSource).FullName + "\" 的指定一个字段（Field）或属性（Property）作为 Lambda 的主体（Body）。");
			}
			//获取命名空间+名称
			string fullName = memberExpression.Member.DeclaringType.FullName + "." + memberExpression.Member.Name;
			if (!IgnoreDict.ContainsKey(fullName))
			{
				IgnoreDict.Add(fullName, 0);
			}
			else
			{
				throw new ResultIgnoreDictEx(fullName, express.Body.ToString());
			}
			return this;
		}
	}

	public class Test1111
	{
		void Test()
		{
			for (int i = 0; i < 5; i++)
			{
				T2();
				T1();
			}
		}

		private void T1()
		{
			ResultIgnoreSource<Testsss> ss = new ResultIgnoreSource<Testsss>();
			// ss.Ignore(x => x.InCom);
			ss.Ignore(x => x.Id);
			ss.Ignore(x => x.DateTimeNow);
			ss.Ignore(x => x.Hobby[0].Id);

			Testsss test = new Testsss()
			{
				Hobby = new List<Hobby>() { new Hobby { Id = 1, Name = "Yesy" } }
			};
			string s = JsonConvertExd.SerializeObjectWithIgnore(test, ss.IgnoreList);
			Console.WriteLine(s);
		}

		private void T2()
		{
			ResultIgnoreSource<Testsss> ss = new ResultIgnoreSource<Testsss>();
			ss.Ignore(x => x.InCom);


			Testsss test = new Testsss()
			{
				Hobby = new List<Hobby>() { new Hobby { Id = 1, Name = "Yesy" } }
			};
			string s = JsonConvertExd.SerializeObjectWithIgnore(test, ss.IgnoreList);
			Console.WriteLine(s);
		}
	}

	public class Testsss
	{
		public string Id { get; set; }
		public DateTime DateTimeNow { get; set; }
		public int Age { get; set; }
		public int SA { get; set; }
		public decimal InCom { get; set; }
		public List<Hobby> Hobby { get; set; }
	}

	public class Hobby
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal InCom { get; set; }
	}



}
