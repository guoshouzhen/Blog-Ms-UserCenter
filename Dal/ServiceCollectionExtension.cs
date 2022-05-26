using Infrastructure;
using Infrastructure.Autofac.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    /// <summary>
    /// IServiceCollection扩展类
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注入Dal层服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddAutofacModule(new AssemblysModule(new List<Assembly>() { Assembly.GetExecutingAssembly() }));
            return services;
        }
    }
}
