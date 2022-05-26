using Infrastructure;
using Infrastructure.Autofac.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace Service
{
    /// <summary>
    /// IServiceCollection扩展类
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 将service层组件注入容器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddAutofacModule(new AssemblysModule(new List<Assembly>() { Assembly.GetExecutingAssembly() }));
            return services;
        }
    }
}
