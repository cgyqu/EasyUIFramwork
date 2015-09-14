using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    /// <summary>
    /// 全部记录器
    /// 目前只能用于http环境
    /// 会保持一个List对象到System.Web.HttpContext.Current.Items
    /// </summary>
    public class GlobalLogger
    {
        const string GlobalLoggerKey = "";
        public static void Add(string log)
        {
            try
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items != null)
                {
                    lock (System.Web.HttpContext.Current.Items)
                    {
                        List<string> logList = System.Web.HttpContext.Current.Items[GlobalLoggerKey] as List<string>;

                        if (logList == null)
                        {
                            logList = new List<string>();
                            System.Web.HttpContext.Current.Items[GlobalLoggerKey] = logList;
                        }

                        logList.Add(string.Format("{0}  LOGTIME:{1}", log, DateTime.Now));
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static string Log
        {
            get
            {
                try
                {
                    if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items != null)
                    {
                        lock (System.Web.HttpContext.Current.Items)
                        {
                            List<string> logList = System.Web.HttpContext.Current.Items[GlobalLoggerKey] as List<string>;

                            if (logList == null)
                            {
                                logList = new List<string>();
                            }

                            return string.Join(Environment.NewLine, logList);
                        }
                    }
                    return "NoLog";
                }
                catch (Exception)
                {
                    return "ErrorLog";
                }
            }
        }
    }
}
