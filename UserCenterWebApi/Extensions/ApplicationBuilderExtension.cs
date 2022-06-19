using Microsoft.AspNetCore.Builder;
using UserCenterWebApi.Middlewares;

namespace UserCenterWebApi.Extensions
{
    /// <summary>
    /// IApplicationBuilder扩展类
    /// </summary>
    public static class ApplicationBuilderExtension
    {
        /// <summary>
        /// 统一向管道注册自定义中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestDetailLogging(this IApplicationBuilder app) 
        {
            //注册请求监控中间件
            app.UseMiddleware<RequestDetailMiddleware>();
            return app;
        }
    }
}
