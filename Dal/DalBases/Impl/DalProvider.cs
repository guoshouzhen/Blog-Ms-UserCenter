using Infrastructure.Autofac.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dal.DalBases.Impl
{
    /// <summary>
    /// Dal提供类实现
    /// </summary>
    [Repository]
    public class DalProvider : IDalProvoder
    {
        private readonly IServiceProvider _serviceProvider;
        public DalProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 从容器中获取指定的Dal对象
        /// </summary>
        /// <typeparam name="TDal"></typeparam>
        /// <returns></returns>
        public TDal Dal<TDal>() where TDal : IDalBase
        {
            return _serviceProvider.GetRequiredService<TDal>();
        }
    }
}
