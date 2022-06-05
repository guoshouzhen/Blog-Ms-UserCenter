using Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Demo
{
    public interface IDemoService
    {
        /// <summary>
        /// socket服务测试
        /// </summary>
        /// <returns></returns>
        Task<List<long>> TestIdServerAsync();
        /// <summary>
        /// 日志测试
        /// </summary>
        /// <returns></returns>
        Task<string> TestLogsAsync();
        /// <summary>
        /// mysql测试
        /// </summary>
        /// <returns></returns>
        Task<User> TestMysqlAsync();
    }
}
