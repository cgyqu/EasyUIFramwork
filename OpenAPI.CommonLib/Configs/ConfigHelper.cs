using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TCBase.ConfigCenter.Component;

namespace CommonLib
{
    public class ConfigHelper
    {
        /// <summary>
        /// 获取AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 从统一配置中心读取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigFromConfigCenter(string key)
        {
            return TCConfigCenter.Get(key);
        }

        public static void ConfigCenterInitialization()
        {
            ConfigCenterInitializer.Instance.Initialization();
        }

        public static string TryGetConfigFromConfigCenter(string key, string defaultValue)
        {
            return TCConfigCenter.TryGet(key, defaultValue);
        }
    }
}
