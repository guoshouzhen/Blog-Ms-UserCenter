using Dal.Dal.Demo;
using Dal.DalBases;
using Infrastructure.Autofac.Attributes;
using Infrastructure.Log;
using Infrastructure.Models.Model;
using Infrastructure.Sockets;
using Infrastructure.Utils;
using Model.Constant;
using Model.Entities;
using Model.Enums;
using System.Collections.Generic;
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
        public async Task<string> TestLogsAsync()
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
        /// socket服务测试
        /// </summary>
        /// <returns></returns>
        public async Task<List<long>> TestIdServerAsync()
        {
            string ipAddr = "116.205.186.117";
            int port = 55666;
            string param = "100" + SplitorConstant.SPLIT_26 + "idserver";
            List<long> lstIds = new List<long>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int count = 0;
            for (int i = 0; i < 4000; i++) 
            {

                string result = _socketClient.GetSocketData(ipAddr, port, param);
                var apiResult = JsonUtil.Json2Object<ApiResult>(result);
                if (apiResult != null && apiResult.Result == ApiResultSuccessConstant.STATUS)
                {
                    lstIds.Add((long)apiResult.Data);
                    count++;
                }
                
            }
            watch.Stop();
            long costTime = watch.ElapsedMilliseconds;
            _loggerHelper.InfoLog($"总计调用成功{count}次，花费时间{costTime}ms，平均{(costTime * 1.0) / count}ms");
            return await Task.FromResult(lstIds);
        }

        /// <summary>
        /// mysql测试
        /// </summary>
        /// <returns></returns>
        public async Task<User> TestMysqlAsync() 
        {
            var user = await _demoDal.GetUserByIdAsync(1);

            //var authName = await _demoDal.GetAuthoritiesOfUserAsync(1);
            return user;
        }
    }
}
