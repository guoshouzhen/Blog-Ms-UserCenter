using Infrastructure.Autofac.Attributes;
using Infrastructure.Log;
using Infrastructure.Models.Model;
using Infrastructure.Sockets;
using Infrastructure.Utils;
using Microsoft.Extensions.Options;
using Model.Constant;
using Model.Options;
using System;

namespace Service.SocketApi.Impl
{
    /// <summary>
    /// 向远程Socket服务请求数据
    /// </summary>
    [Service]
    public class SocketService : ISocketService
    {
        /// <summary>
        /// 注入强类型配置项
        /// </summary>
        private readonly SocketServerOptions _socketServerOptions;
        /// <summary>
        /// 注入HttpClient
        /// </summary>
        private readonly SocketClient _socketClient;
        /// <summary>
        /// 日志类注入
        /// </summary>
        private readonly ILoggerHelper<SocketService> _loggerHelper;

        public SocketService(IOptions<SocketServerOptions> options, SocketClient socketClient, ILoggerHelper<SocketService> loggerHelper)
        {
            _socketServerOptions = options.Value;
            _socketClient = socketClient;
            _loggerHelper = loggerHelper;
        }

        /// <summary>
        /// 向socket服务请求数据，获取用户表主键id
        /// </summary>
        /// <returns></returns>
        public long GetUserId()
        {
            long lUserId = 0;
            try
            {
                string strMethodId = SocketServerConstant.METHODID_IDSERVER;
                string param = JointStrs(SplitorConstant.SPLIT_16, "t_user");
                param = JointStrs(SplitorConstant.SPLIT_26, strMethodId, param);
                string receiveData = _socketClient.GetSocketData(_socketServerOptions.IdServer.Host, _socketServerOptions.IdServer.Port, param);
                ApiResult apiResult = JsonUtil.Json2Object<ApiResult>(receiveData);
                if (apiResult == null || apiResult.Result != ApiResultSuccessConstant.STATUS) 
                {
                    _loggerHelper.InfoLog($"申请用户id失败");
                    return lUserId;
                }
                long.TryParse(apiResult.Data.ToString(), out lUserId);
            }
            catch (Exception ex) 
            {
                _loggerHelper.ErrorLog($"向Socket服务申请用户id时发生异常，异常信息：{ex}");
            }
            return lUserId;
        }

        /// <summary>
        /// 使用指定分隔符拼接字符串
        /// </summary>
        /// <param name="splitor"></param>
        /// <param name="strs"></param>
        /// <returns></returns>
        private string JointStrs(string splitor, params string[] strs) => string.Join(splitor, strs);
    }
}
