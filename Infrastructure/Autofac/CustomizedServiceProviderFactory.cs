using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Autofac.Cache;
using Infrastructure.Autofac.Extensions;
using Infrastructure.Autofac.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Infrastructure.Autofac
{
    /// <summary>
    /// 自定义一个ServiceProviderFactory，实现IServiceProviderFactory接口
    /// 使用第三方DI容器需要解决从原始的服务注册IServiceCollection容器到最终的DI容器IServiceProvider对象的适配问题
    /// 通过Autofac的ContainerBuilder对象，将原始服务注册到Autofac容器中，然后提供IServiceProvider对象接管DI容器
    /// </summary>
    public class CustomizedServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        /// <summary>
        /// 创建一个ContainerBuilder对象
        /// </summary>
        /// <param name="services">原始服务集合</param>
        /// <returns></returns>
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            //注册扫描到的组件
            builder.PopulateModules(ModuleCache.Modules.ToArray());
            //注册module时，在调用builder.build()时才会调用module的load方法，这里不会立即注册module
            builder.PopulateModules(new ServiceCollectionModule(services));
            return builder;
        }

        /// <summary>
        /// 提供最终的IServiceProvider容器
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            //调用builder.build()时，执行所有的build regiser
            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}
