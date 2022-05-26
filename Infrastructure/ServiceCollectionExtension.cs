using Infrastructure.Autofac.Cache;
using Infrastructure.Autofac.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using Module = Autofac.Module;

namespace Infrastructure
{
    /// <summary>
    /// IServiceCollection扩展类
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 将Infrastructure层的服务注入容器
        /// </summary>
        /// <param name="services">IServiceCollection容器</param>
        /// <param name="configuration">配置</param>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            //按程序集注入
            services.AddAutofacModule(new AssemblysModule(new List<Assembly>() { Assembly.GetExecutingAssembly() }));
            return services;
        }

        /// <summary>
        /// 通过module注册进autofac容器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="module"></param>
        public static void AddAutofacModule(this IServiceCollection services, Module module) 
        {
            ModuleCache.Modules.Add(module);
        }
    }
}
