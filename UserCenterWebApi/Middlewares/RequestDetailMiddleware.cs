using Infrastructure.Exceptions;
using Infrastructure.Log;
using Infrastructure.Models.Model;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Model.Bo;
using Model.Enums;
using Model.Options;
using Model.Vo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
        /// 启用项配置
        /// </summary>
        private readonly EnableItemOptions _enableItemOptions;

        /// <summary>
        /// 构造器注入
        /// </summary>
        /// <param name="next"></param>
        public RequestDetailMiddleware(RequestDelegate next, ILoggerHelper<RequestDetailMiddleware> loggerHelper, IOptions<EnableItemOptions> options)
        {
            _next = next;
            _logger = loggerHelper;
            _enableItemOptions = options.Value;
        }

        /// <summary>
        /// 固定方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            //根据配置判断是否开启请求监控
            if (!_enableItemOptions.EnableRequestDetailLog)
            {
                await _next(context);
            }
            else
            {
                RequestDetailLogBO requestLogBO = new RequestDetailLogBO();
                Stopwatch stopWatch = Stopwatch.StartNew();
                var request = context.Request;
                requestLogBO.RequestPath = request.Path;
                requestLogBO.RequestMethod = request.Method;
                requestLogBO.RequestHeaders = GetRequestHeader(request);
                requestLogBO.RequestQuery = GetRequestQuery(request);
                requestLogBO.RequestForm = await GetRequestFormAsync(request);
                requestLogBO.RequestBody = await GetRequestBodyAsync(request);

                //临时变量保存原始响应流
                var originResponseStream = context.Response.Body;
                //创建一个可读可倒带的流
                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;
                    try
                    {
                        await _next(context);
                        stopWatch.Stop();
                        requestLogBO.CostTime = stopWatch.Elapsed.TotalSeconds;
                        requestLogBO.ResponseBody = await GetResponseBodyAsync(context.Response);
                        //追加流
                        await memoryStream.CopyToAsync(originResponseStream);
                        context.Response.Body = originResponseStream;

                        //响应完成时，记录日志
                        context.Response.OnCompleted(() =>
                        {
                            SaveRequestDetailLog(requestLogBO);
                            return Task.CompletedTask;
                        });
                    }
                    catch (Exception ex)
                    {
                        //请求失败了，记录监控信息，异常继续向外抛
                        stopWatch.Stop();
                        requestLogBO.CostTime = stopWatch.Elapsed.TotalSeconds;
                        requestLogBO.ResponseBody = GetResponseInfoFromException(ex);
                        await memoryStream.CopyToAsync(originResponseStream);
                        context.Response.Body = originResponseStream;

                        SaveRequestDetailLog(requestLogBO);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 获取请求头
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetRequestHeader(HttpRequest request)
        {
            var headers = request.Headers;
            List<string> headerList = new List<string>(request.Headers.Count);
            foreach (var key in request.Headers.Keys)
            {
                headerList.Add(string.Format("{0}={1}", key, headers[key]));
            }
            return string.Join(",", headerList);
        }

        /// <summary>
        /// 获取URL请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetRequestQuery(HttpRequest request)
        {
            List<string> list = new List<string>(request.Query.Count);
            foreach (string key in request.Query.Keys)
            {
                list.Add(string.Format("{0}={1}&", key, request.Query[key]));
            }
            return string.Join(",", list);
        }

        /// <summary>
        /// 获取表单内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<string> GetRequestFormAsync(HttpRequest request)
        {
            if (request.ContentType == "application/json") 
            {
                return string.Empty;
            }
            return await GetRequestBodyAsync(request);
        }

        /// <summary>
        /// 获取请求体信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            string res = string.Empty;
            try
            {
                if (request.ContentLength > 0)
                {
                    //倒带
                    request.Body.Seek(0, SeekOrigin.Begin);
                    res = await new StreamReader(request.Body, Encoding.UTF8).ReadToEndAsync();
                    //重置request.body stream的开始位置
                    request.Body.Seek(0, SeekOrigin.Begin);
                }
            }
            catch
            {
                throw;
            }
            return res;
        }

        /// <summary>
        /// 获取响应体信息
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> GetResponseBodyAsync(HttpResponse response)
        {
            string strResponse = string.Empty;
            try
            {
                if (response.ContentLength > 0)
                {
                    response.Body.Seek(0, SeekOrigin.Begin);
                    strResponse = await new StreamReader(response.Body, Encoding.UTF8).ReadToEndAsync();
                    response.Body.Seek(0, SeekOrigin.Begin);
                }
            }
            catch
            {
                throw;
            }
            return strResponse;
        }

        /// <summary>
        /// 根据异常信息判断响应内容
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string GetResponseInfoFromException(Exception ex) 
        {
            string strRes;
            if (ex is ServiceException)
            {
                //业务异常，会返回对应的错误码和提示信息
                var serviceException = ex as ServiceException;
                strRes = JsonUtil.Object2Json(ApiResult.Fail(serviceException.Code, serviceException.Msg));
            }
            else
            {
                strRes = JsonUtil.Object2Json(ApiResult.Fail(ErrorCodeVo.A0000.Code, ErrorCodeVo.A0000.Message));
            }
            return strRes;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="requestDetailLogBO"></param>
        private void SaveRequestDetailLog(RequestDetailLogBO logBO) 
        {
            if (logBO != null) 
            {
                string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string log =
                    $"\n" +
                    $"请求时间：{strDateTime}\n" +
                    $"请求耗时：{logBO.CostTime}s\n" +
                    $"请求路径：{logBO.RequestPath}\n" +
                    $"请求方法：{logBO.RequestMethod}\n" +
                    $"请求头：{logBO.RequestHeaders}\n" +
                    $"请求URL参数：{logBO.RequestQuery}\n" +
                    $"请求表单数据：{logBO.RequestForm}\n" +
                    $"请求体数据：{logBO.RequestBody}\n" +
                    $"响应体数据：{logBO.ResponseBody}\r\n";

                _logger.InfoLog(log, LogFolderEnum.ApiMonitor);
            }
        }
    }
}
