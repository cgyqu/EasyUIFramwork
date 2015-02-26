using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace CommonLib
{
	public static class JsonConvertExd
	{
		public static string SerializeObjectWithIgnore(object value, List<string> ignoreList)
		{
			if (ignoreList == null)
			{
				ignoreList = new List<string>();
			}
			IsoDateTimeConverter dateC = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff" };
			var formatting = Formatting.None;
			var converters = new JsonConverter[] { dateC };
			JsonSerializerSettings settings = (converters != null && converters.Length > 0)
										 ? new JsonSerializerSettings { Converters = converters }
										 : null;

			JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
			jsonSerializer.ContractResolver = new JsonIgnoreExDefaultContractResolver(false, ignoreList);
			StringBuilder sb = new StringBuilder(256);
			StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
			{
				jsonWriter.Formatting = formatting;
				jsonSerializer.Serialize(jsonWriter, value);
			}
			string s = sw.ToString();
			return s;
		}

		public static T Deserialize<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}
	}
}
