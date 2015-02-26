using System;
using System.Text;
using System.Web;

namespace CommonLib
{
    public static class IPHelper
    {
        /// <summary>
        /// 获取请求IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static long GetClientIpLong(this HttpRequest request)
        {
            try
            {
                var serverVariables = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(serverVariables))
                {
                    var items = serverVariables.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (items.Length > 0)
                    {
                        return GetLongIp(items[0]);
                    }
                }
                return GetLongIp(request.ServerVariables["REMOTE_ADDR"]);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 长整形IP值转IPv4
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static string LongToIPv4(this long ipaddress)
        {
            if (ipaddress == 0) return "";
            try
            {
                var builder = new StringBuilder();
                builder.Append((ipaddress >> 24));
                builder.Append(".");
                builder.Append((ipaddress & 0x00FFFFFF) >> 16);
                builder.Append(".");
                builder.Append((ipaddress & 0x0000FFFF) >> 8);
                builder.Append(".");
                builder.Append((ipaddress & 0x000000FF));
                return builder.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 判断一个IP是否为内网Ip
        /// </summary>
        /// <param name="ipLong"> </param>
        /// <returns>判断一个IP是否为内网Ip</returns>
        public static bool IsInnerIp(this long ipLong)
        {
            try
            {
                /**   
                私有IP：A类  10.0.0.0-10.255.255.255   
                       B类  172.16.0.0-172.31.255.255   
                       C类  192.168.0.0-192.168.255.255
                **/
                var aBegin = GetLongIp("10.0.0.0");
                var aEnd = GetLongIp("10.255.255.255");
                var bBegin = GetLongIp("172.16.0.0");
                var bEnd = GetLongIp("172.31.255.255");
                var cBegin = GetLongIp("192.168.0.0");
                var cEnd = GetLongIp("192.168.255.255");
                var isInnerIp = IsBetween(ipLong, aBegin, aEnd) ||
                    IsBetween(ipLong, bBegin, bEnd) ||
                    IsBetween(ipLong, cBegin, cEnd);
                return isInnerIp;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取IP的长整形值
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static long GetLongIp(this string ip)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ip))
                {
                    return 0;
                }
                var ips = ip.Split('.');
                if (ips.Length < 4)
                {
                    return 0;
                }
                var a = long.Parse(ips[0]);
                var b = long.Parse(ips[1]);
                var c = long.Parse(ips[2]);
                var d = long.Parse(ips[3]);
                var ipNum = a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
                return ipNum;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// IP是否再一个范围内
        /// </summary>
        /// <param name="userIp"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool IsBetween(this long userIp, long begin, long end)
        {
            return (userIp >= begin) && (userIp <= end);
        }
    }
}
