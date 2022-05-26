using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public class EnumUtil
    {
        /// <summary>
        /// 将指定字符串转成指定的枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseToEnum<T>(string value) 
        {
            if (string.IsNullOrWhiteSpace(value)) 
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
