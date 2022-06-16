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
        /// ��������ע�뻷����Ϣ
        /// </summary>
        /// <param name="env">���л�����Ϣ</param>
        public Startup(IHostEnvironment env)
        {
            //���ݲ�ͬ�������ز�ͬ�������ļ�
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.ConfigureJsonFile(env);
            Configuration = builder.Build();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ��DI����ע������࣬������ʱ����
        /// ����ͨ����չ����ע�����
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //��controller���ķ���ע�뵽����
            //�ӿڽ�����л�����
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });

            //�����첽��Ӧ
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true);
            services.Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            //ȫ������NLog
            LogManager.Configuration = new XmlLoggingConfiguration(AppDomain.CurrentDomain.BaseDirectory + @"Configs/nlog.config");

            //ע�������ļ�������
            services.AddConfigOptions(Configuration);

            //ע��HttpClientFactory
            services.AddHttpClientFactory(Configuration);

            //ע��Api��صķ���
            services.AddApiServices(Configuration);

            //ע��Infrastructure�����
            services.AddInfrastructure(Configuration);

            //ע�����ݲִ������
            services.AddRepository(Configuration).SetDbIdConfigFactory(p => new DbIdConfig() { DbId = p.GetService<LoginInfoBo>()?.DbId });

            //ע��Dal�����
            services.AddDal(Configuration);

            //ע��Service�����
            services.AddServices(Configuration);
        }

        /// <summary>
        /// ����HTTP����ܵ�������������м����Ӧ�ò�ͬ����Ӧ��ʽ��������ʱ����
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env">�������еĻ�����Ϣ</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //ȫ���쳣����
            app.UseExceptionHandler(configure =>
            {
                configure.Run(new MyExceptionHandler().HandleExceptionAsync);
            });

            app.UseHttpsRedirection();

            app.Use(next => context =>
            {
                //���������������Զ�ȡ���
                context.Request.EnableBuffering();
                return next(context);
            });

            //������
            app.UseMiddleware<RequestDetailMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
