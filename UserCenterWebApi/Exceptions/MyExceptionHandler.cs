using Infrastructure.Exceptions;
using Infrastructure.Log;
using Infrastructure.Models.Model;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Model.Vo;
using System.Threading.Tasks;

namespace UserCenterWebApi.Exceptions
{
    /// <summary>
    /// 异常处理类
    /// </summary>
    public class MyExceptionHandler
    {
        /// <summary>
        /// 全局异常处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task HandleExceptionAsync(HttpContext context)
        {
            var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionHandler?.Error;
            if (ex != null)
            {
                string strContent;
                if (ex is ServiceException)
                {
                    //业务异常，直接返回对应的错误码和提示信息
                    var serviceException = ex as ServiceException;
                    strContent = JsonUtil.Object2Json(ApiResult.Fail(serviceException.Code, serviceException.Msg));
                }
                else 
                {
                    //普通异常，记录错误日志，返回通用错误响应
                    var loggerHelper = context.RequestServices.GetRequiredService<ILoggerHelper<MyExceptionHandler>>();
                    loggerHelper.ErrorLog(string.Format("用户中心发生异常，异常信息：{0}，栈追踪：\n{1}", ex.Message, ex.StackTrace));
                    strContent = JsonUtil.Object2Json(ApiResult.Fail(ErrorCodeVo.A0000.Code, ErrorCodeVo.A0000.Message));
                }
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(strContent);
            }
        }
    }
}
