using Infrastructure.Log;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UserCenterWebApi.Middlewares
{
    /// <summary>
    /// 请求监控中间件
    /// </summary>
    public class RequestDetailMiddleware
    {
        /// <summary>
        /// RequestDelegate，用于将请求传递到下个中间件
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 日志类
        /// </summary>
        private readonly ILoggerHelper<RequestDetailMiddleware> _logger;
        /// <summary>
        /// 计时器
        /// </summary>
        private Stopwatch _stopWatch;

        /// <summary>
        /// 构造器注入
        /// </summary>
        /// <param name="next"></param>
        public RequestDetailMiddleware(RequestDelegate next, ILoggerHelper<RequestDetailMiddleware> loggerHelper)
        {
            _next = next;
            _logger = loggerHelper;
            _stopWatch = new Stopwatch();
        }

        /// <summary>
        /// 固定方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context) 
        {
            bool enableMinotor = false;
            //TODO 根据配置判断是否开启请求监控
            if (!enableMinotor)
            {
                await _next(context);
            }
            else 
            {
                _stopWatch.Restart();
                HttpRequest request = context.Request;
                HttpResponse httpResponse = context.Response;
                try
                {
                    await _next(context);
                    _stopWatch.Stop();
                    httpResponse = context.Response;
                    //请求成功的逻辑
                }
                catch (Exception ex)
                {
                    //请求失败了，记录监控信息，继续向外抛
                    _logger.ErrorLog(ex.Message);
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// 请求监控日志定义
    /// </summary>
    public class RequestDetailLogBo 
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath { get; set; }
        /// <summary>
        /// 花费时间（ms）
        /// </summary>
        public int CostTime { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// 请求头
        /// </summary>
        public string RequestHeaders { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestParams { get; set; }
        /// <summary>
        /// 请求响应
        /// </summary>
        public string Response { get; set; }
    }
}
