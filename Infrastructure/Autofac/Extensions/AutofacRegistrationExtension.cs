using Autofac.Builder;
using Infrastructure.Autofac.Enums;
using Infrastructure.Autofac.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Autofac.Extensions
{
    /// <summary>
    /// IRegistrationBuilder对象的扩展类
    /// </summary>
    public static class AutofacRegistrationExtension
    {
        /// <summary>
        /// 对向Autofac ContainerBuilder注册的服务进行生命周期配置
        /// 来自netcore原始容器的服务
        /// </summary>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="registrationBuilder"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder, ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    //单例
                    registrationBuilder.SingleInstance();
                    break;
                case ServiceLifetime.Scoped:
                    //作用域单例
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
                case ServiceLifetime.Transient:
                    //瞬时
                    registrationBuilder.InstancePerDependency();
                    break;
            }
            return registrationBuilder;
        }

        /// <summary>
        /// 配置注入autofac容器的服务
        /// 来自扫描到的服务
        /// </summary>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="registrationBuilder"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureComponentService<TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder, ServiceDetail service)
        {
            if (!string.IsNullOrEmpty(service.Name))
            {
                registrationBuilder
                    .Keyed(service.Name, service.ServiceType)
                    .Named(service.ServiceType.FullName, service.ServiceType);
            }
            else
            {
                registrationBuilder
                    .As(service.ServiceType)
                    .Named(service.ServiceType.FullName, service.ServiceType); //通过集合注入Autowired拿到所有
            }
            return registrationBuilder;
        }

        /// <summary>
        /// 配置注入autofac容器服务的生命周期
        /// 来自扫描到的服务
        /// </summary>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="registrationBuilder"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureComponentLifeCycle<TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder, ServiceDetail service)
        {
            switch (service.LifeCycle)
            {
                case ObjectLifeCycleEnum.Default:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
                case ObjectLifeCycleEnum.InstancePerDependency:
                    registrationBuilder.InstancePerDependency();
                    break;
                case ObjectLifeCycleEnum.SingleInstance:
                    registrationBuilder.SingleInstance();
                    break;
                case ObjectLifeCycleEnum.InstancePerLifetimeScope:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
            }

            return registrationBuilder;
        }
    }
}
