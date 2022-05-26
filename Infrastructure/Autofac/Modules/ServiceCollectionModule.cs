using Autofac;
using Infrastructure.Autofac.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Autofac.Modules
{
    /// <summary>
    /// 通过Autofac模块化注入ServiceCollection
    /// </summary>
    internal class ServiceCollectionModule : Module
    {
        /// <summary>
        /// IServiceCollection对象
        /// </summary>
        private readonly IServiceCollection _services;
        public ServiceCollectionModule(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// 注册原始服务到Autofac容器
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.PopulateServices(_services);
        }
    }
}
