using Dal;
using Infrastructure;
using Infrastructure.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.Bo.Auth;
using Model.Options;
using NLog;
using NLog.Config;
using Repository;
using Service;
using System;
using UserCenterWebApi.Exceptions;
using UserCenterWebApi.Extensions;
using UserCenterWebApi.Middlewares;

namespace UserCenterWebApi
{
    public class Startup
    {
        /// <summary>
        /// 构造器，注入环境信息
        /// </summary>
        /// <param name="env">运行环境信息</param>
        public Startup(IHostEnvironment env)
        {
            //根据不同环境加载不同的配置文件
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.ConfigureJsonFile(env);
            Configuration = builder.Build();
        }

        /// <summary>
        /// 程序配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 向DI容器注入服务类，由运行时调用
        /// 建议通过扩展方法注入服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //将controller核心服务注入到容器
            //接口结果序列化配置
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });

            //启用异步响应
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true);
            services.Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            //全局配置NLog
            LogManager.Configuration = new XmlLoggingConfiguration(AppDomain.CurrentDomain.BaseDirectory + @"Configs/nlog.config");

            //注入配置文件配置项
            services.AddConfigOptions(Configuration);

            //注入HttpClientFactory
            services.AddHttpClientFactory(Configuration);

            //注入Api相关的服务
            services.AddApiServices(Configuration);

            //注入Infrastructure层服务
            services.AddInfrastructure(Configuration);

            //注入数据仓储层服务
            services.AddRepository(Configuration).SetDbIdConfigFactory(p => new DbIdConfig() { DbId = p.GetService<LoginInfoBo>()?.DbId });

            //注入Dal层服务
            services.AddDal(Configuration);

            //注入Service层服务
            services.AddServices(Configuration);
        }

        /// <summary>
        /// 配置HTTP请求管道，向其中添加中间件，应用不同的相应方式，由运行时调用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env">程序运行的环境信息</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //全局异常处理
            app.UseExceptionHandler(configure =>
            {
                configure.Run(new MyExceptionHandler().HandleExceptionAsync);
            });

            app.UseHttpsRedirection();

            app.Use(next => context =>
            {
                //设置请求体流可以读取多次
                context.Request.EnableBuffering();
                return next(context);
            });

            //请求监控
            app.UseMiddleware<RequestDetailMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
