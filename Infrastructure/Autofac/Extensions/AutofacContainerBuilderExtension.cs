using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Module = Autofac.Module;

namespace Infrastructure.Autofac.Extensions
{
    /// <summary>
    /// ContainerBuilder的扩展类
    /// </summary>
    public static class AutofacContainerBuilderExtension
    {
        /// <summary>
        /// 将IServiceCollection容器中的服务注册到Autofac的IServiceProvider容器中
        /// </summary>
        /// <param name="builder">ContainerBuilder对象</param>
        /// <param name="services">原始服务容器</param>
        public static void PopulateServices(this ContainerBuilder builder, IServiceCollection services)
        {
            //在此预先注入IServiceProvider，供后续手动解析服务使用，该实例允许从Autofac外部控制生命周期
            //注：这个IServiceProvider实例就是之前IServiceProviderFactory中的CreateServiceProvider方法提供的，此处的IServiceProvider在被请求时才生成，此时Factory中IServiceProvider实例已构建完毕。
            builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>().ExternallyOwned();

            //AutofacServiceScopeFactory必须注入，否则在解析IServiceProvider对象时会报错
            var autofacServiceScopeFactory = typeof(AutofacServiceProvider).Assembly.GetType("Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory");
            if (autofacServiceScopeFactory == null)
            {
                throw new Exception("Unable get type of Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory!");
            }
            builder.RegisterType(autofacServiceScopeFactory).As<IServiceScopeFactory>();
            Register(builder, services);
        }

        /// <summary>
        /// 将Autofac module注册到容器中
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="modules"></param>
        public static void PopulateModules(this ContainerBuilder builder, params Module[] modules)
        {
            if (modules == null || modules.Length == 0)
            {
                return;
            }
            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }
        }

        /// <summary>
        /// 进行注册
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceDescriptors"></param>
        private static void Register(ContainerBuilder builder, IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            //ServiceDescriptor：
            //net core DI中对注入服务的描述，包括：
            //组件类型ServiceType，对应直接注入组件
            //暴露服务（接口）ImplementationType，对应ServiceType、接口暴露注入
            //工厂ImplementationFactory，对应工厂注入，注入一个有返回值的预定义委托Func<IServiceProvider, IServiceType>
            //以及服务的生命周期
            foreach (var descriptor in serviceDescriptors)
            {
                if (descriptor.ImplementationType != null)
                {
                    //注册带暴露接口的服务
                    var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                    if (serviceTypeInfo.IsGenericTypeDefinition)
                    {
                        builder
                            .RegisterGeneric(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .PropertiesAutowired()
                            .ConfigureLifecycle(descriptor.Lifetime);
                    }
                    else
                    {
                        builder
                            .RegisterType(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .PropertiesAutowired()
                            .ConfigureLifecycle(descriptor.Lifetime);
                    }
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    //注册工厂
                    var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                    {
                        //需要预先注入IServiceProvider实例
                        //注：在启动时注册服务时，该部分不会被立即执行，在IServiceProvider容器生成后才进行IServiceProvider实例注入并执行该委托
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return descriptor.ImplementationFactory.Invoke(serviceProvider);
                    })
                    .ConfigureLifecycle(descriptor.Lifetime)
                    .CreateRegistration();

                    builder.RegisterComponent(registration);
                }
                else
                {
                    //注册组件
                    builder
                        .RegisterInstance(descriptor.ImplementationInstance)
                        .As(descriptor.ServiceType)
                        .PropertiesAutowired()
                        .ConfigureLifecycle(descriptor.Lifetime);
                }
            }
        }
    }
}
