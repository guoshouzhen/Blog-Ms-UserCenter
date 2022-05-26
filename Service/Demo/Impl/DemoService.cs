using Dal.Dal.Demo;
using Dal.DalBases;
using Infrastructure.Autofac.Attributes;
using Infrastructure.Log;
using Infrastructure.Sockets;
using Model.Constant;
using Model.Entities;
using Model.Enums;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Service.Demo.Impl
{
    [Service]
    public class DemoService : IDemoService
    {
        private readonly ILoggerHelper<DemoService> _loggerHelper;
        private readonly IDemoDal _demoDal;
        private readonly SocketClient _socketClient;

        public DemoService(ILoggerHelper<DemoService> loggerHelper, IDalProvoder dalProvoder, SocketClient socketClient)
        {
            _loggerHelper = loggerHelper;
            _socketClient = socketClient;
            _demoDal = dalProvoder.Dal<IDemoDal>();
        }

        /// <summary>
        /// 日志测试
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetLogTestStringsAsync()
        {
            for (int i = 0; i < 2; i++)
            {
                _loggerHelper.DebugLog("调试错误AAA");
                _loggerHelper.InfoLog("数据库ID切换了");
                _loggerHelper.ErrorLog("用户ID不能为空");

                _loggerHelper.InfoLog("数据库sql：", LogFolderEnum.DbLog);
                _loggerHelper.InfoLog("请求内容：", LogFolderEnum.ApiMonitor);
            }
            return await Task.FromResult("log run seccessfuly");
        }

        /// <summary>
        /// api测试
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTestStringsAsync()
        {
            string param = "100" + SplitorConstant.SPLIT_26 + "idserver";

            string result = string.Empty;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int count = 0;
            for (int i = 0; i < 4000; i++) 
            {

                result = _socketClient.GetSocketData("127.0.0.1", 55666, param);
                if (!string.IsNullOrEmpty(result))
                {
                    count++;
                }
                
            }
            watch.Stop();
            long costTime = watch.ElapsedMilliseconds;
            _loggerHelper.InfoLog($"总计调用成功{count}次，花费时间{costTime}ms，平均{(costTime * 1.0) / count}ms");
            return await Task.FromResult("user center run successfuly!");
        }

        /// <summary>
        /// mysql测试
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetUserAsync() 
        {
            var user = await _demoDal.GetUserByIdAsync(1);

            var authName = await _demoDal.GetAuthoritiesOfUserAsync(1);
            return user;
        }
    }
}
