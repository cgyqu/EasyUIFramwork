using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
	public interface IApiLogger<T>
	{
		void Start();
		void End();
		void Log(T log);
	}
}
