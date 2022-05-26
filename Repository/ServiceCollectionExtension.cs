using Infrastructure;
using Infrastructure.Autofac.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.Options;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Repository
{
    /// <summary>
    /// IServiceCollection扩展类
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注入数据仓储层服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration) 
        {
            //扫描程序集
            services.AddAutofacModule(new AssemblysModule(new List<Assembly>() { Assembly.GetExecutingAssembly() }));

            //数据库配置注入
            services.Configure<DbConfigOptions>(configuration.GetSection("DbConfigOptions"));
            return services;
        }

        /// <summary>
        /// 注入数据库id配置类
        /// </summary>
        /// <param name="services"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IServiceCollection SetDbIdConfigFactory(this IServiceCollection services, Func<IServiceProvider, DbIdConfig> factory) 
        {
            services.AddScoped(factory);
            return services;
        }
    }
}
