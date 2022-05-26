using Newtonsoft.Json;

namespace Infrastructure.Utils
{
    /// <summary>
    /// Json工具类
    /// </summary>
    public class JsonUtil
    {
        /// <summary>
        /// 将对象序列化为Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Object2Json<T>(T data) 
        {
            if (data == null) 
            {
                return "";
            }
            try
            {
                return JsonConvert.SerializeObject(data);
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// 将json序列化为指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Json2Object<T>(string json) 
        {
            if (string.IsNullOrWhiteSpace(json)) 
            {
                return default(T);
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch 
            {
                throw;
            }
        }
    }
}
