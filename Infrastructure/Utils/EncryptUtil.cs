using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public class EncryptUtil
    {
        /// <summary>
        /// Md5加密实现
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Md5ToLower(string value)
        {
            return Md5(Md5(value));
        }

        public static string AesDecrypt(string value, string aesKey)
        {
            return value;
        }

        public static string AesEncrypt(string value, string aesKey)
        {
            return value;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Md5(string value)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(value));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "").ToLower();
            }
        }
    }
}
