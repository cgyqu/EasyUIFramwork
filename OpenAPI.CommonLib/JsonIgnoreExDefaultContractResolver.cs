using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace CommonLib
{
    public class JsonIgnoreExDefaultContractResolver : DefaultContractResolver
    {
        private List<string> ignoreList;
        public JsonIgnoreExDefaultContractResolver(bool shareCache, List<string> ignoreList)
			: base(shareCache)
        {
            this.ignoreList = ignoreList;
        }


        protected override JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, System.Reflection.ParameterInfo parameterInfo)
        {
            return base.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
        }

        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var jsp = base.CreateProperty(member, memberSerialization);
            {
                if (ignoreList != null && ignoreList.Count > 0)
                {
                    string fieldName = member.DeclaringType + "." + member.Name;
                    jsp.Ignored = ignoreList.Contains(fieldName);
                }
            }
            return jsp;
        }
    }
}
