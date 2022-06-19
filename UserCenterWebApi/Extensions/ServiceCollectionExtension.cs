using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Model.Bo.Auth;
using Model.Enums;
using Model.Options;
using Service.Auth;
using System;
using UserCenterWebApi.Filters;

namespace UserCenterWebApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注入配置项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //加密配置注入
            services.Configure<EncryptOptions>(configuration.GetSection("EncryptOptions"));
            //文件路径配置注入
            services.Configure<FilePathOptions>(configuration.GetSection("FilePathOptions"));
            //Http请求TraceID注入，跟踪日志用
            services.Configure<TraceIdOptionsSnapshot>(options => options.TraceId = Guid.NewGuid().ToString());
            //鉴权相关配置注入
            services.Configure<AuthOptions>(configuration.GetSection("AuthOptions"));
            //socket服务配置注入
            services.Configure<SocketServerOptions>(configuration.GetSection("SocketServerOptions"));
            //启用项注入
            services.Configure<EnableItemOptions>(configuration.GetSection("EnableItemOptions"));
            return services;
        }

        /// <summary>
        /// 将Api相关的的服务注入容器
        /// </summary>
        /// <param name="services">IServiceCollection容器</param>
        /// <param name="configuration">配置</param>
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            //注入HttpContextAccessor，方便从其他服务获取http请求上下文信息
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped(CreateLoginInfo);
            services.AddScoped<OpenAuthFilterAttribute>();
            return services;
        }

        /// <summary>
        /// 注册并配置HttpClientFactory
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientFactory(this IServiceCollection services, IConfiguration configuration) 
        {
            //调用鉴权中心的客户端
            services.AddHttpClient(HttpClientNameEnum.AuthCenter.ToString(), httpClient => 
            {
                httpClient.BaseAddress = new Uri(configuration.GetSection("HttpClientOptions")["AuthCenterAddress"]);
                httpClient.Timeout = TimeSpan.FromSeconds(10);
            });
            return services;
        }

        /// <summary>
        /// 生成登录数据，当前登录用户的，并注入DI容器
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static LoginInfoBo CreateLoginInfo(IServiceProvider service)
        {
            IAuthService authService = service.GetRequiredService<IAuthService>();
            return authService?.GetLoginInfo();
        }
    }
}
