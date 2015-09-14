using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace CommonLib
{
    public class RedisHelper
    {
        private static string GetRedisAddress(string addressConfigName)
        {
            return "localhost,6379";
        }

        public static RedisClient GetRedisClient(string addressConfigName)
        {
            string redisAddress = GetRedisAddress(addressConfigName);
            return new RedisClient(redisAddress.Split(',')[0], int.Parse(redisAddress.Split(',')[1]));
        }

        public static void AddItemToList(string addressConfigName, string listId, string value)
        {
            using (RedisClient redis = RedisHelper.GetRedisClient(addressConfigName))
            {
                redis.AddItemToList(listId, value);
            }
        }

        public static string RemoveStartFromList(string addressConfigName, string listId)
        {
            using (RedisClient redis = RedisHelper.GetRedisClient(addressConfigName))
            {
                return redis.RemoveStartFromList(listId);
            }
        }

        public static string RemoveStartFromList(RedisClient redis, string listId)
        {
            return redis.RemoveStartFromList(listId);
        }

        public static string PopItemFromList(string addressConfigName, string listId)
        {
            using (RedisClient redis = RedisHelper.GetRedisClient(addressConfigName))
            {
                return redis.PopItemFromList(listId);
            }
        }
        public long Count(string addressConfigName, string listId)
        {
            using (RedisClient redis = RedisHelper.GetRedisClient(addressConfigName))
            {
                return redis.GetListCount(listId);
            }
        }
    }
}
