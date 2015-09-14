using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class ApiAuthUtil
    {
        private string module;
        private string category;
        private string methodName;
        private string accountId;
        private string password;

        /// <summary>
        /// 请求时间
        /// </summary>
        public string RequestTime { get; private set; }

        /// <summary>
        /// 数字签名
        /// </summary>
        public string DigitalSign { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="category">分类</param>
        /// <param name="methodName">方法</param>
        /// <param name="accountId">账户ID</param>
        /// <param name="password">账户密码</param>
        public ApiAuthUtil(
            string module,
            string category,
            string methodName,
            string accountId,
            string password,
            string requestTime)
        {
            this.RequestTime = requestTime;
            this.module = module;
            this.category = category;
            this.methodName = methodName;
            this.accountId = accountId;
            this.password = password;

            EncryptDigitalSign();
        }

        /// <summary>
        /// 数字签名计算
        /// </summary>
        private void EncryptDigitalSign()
        {
            string fullActionName = string.Format("{0}.{1}.{2}", module, category, methodName);
            string token = string.Format("{0}&{1}&{2}&{3}", fullActionName, accountId, password, this.RequestTime).ToLower(); ;
            string digitalSign = SHAEncryption.Encrypt(token);
            this.DigitalSign = digitalSign;
        }
    }
}
