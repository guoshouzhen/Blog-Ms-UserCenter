namespace Infrastructure.Utils
{
    /// <summary>
    /// 数据库相工具类
    /// </summary>
    public class DbUtil
    {
        /// <summary>
        /// 数据库数量
        /// </summary>
        private const int DBNUM = 2;
        /// <summary>
        /// 根据用户名获取数据库id
        /// </summary>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public static string GetDbId(string strUserName) 
        {
            //返回的是有符号int值
            int hashcode = strUserName.GetHashCode();
            hashcode = hashcode < 0 ? -1 * hashcode : hashcode;
            return ((hashcode % DBNUM) + 1).ToString();
        }
    }
}
