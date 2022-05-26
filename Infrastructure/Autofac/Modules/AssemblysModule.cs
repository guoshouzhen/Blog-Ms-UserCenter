using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using Module = Autofac.Module;

namespace Infrastructure.Autofac.Modules
{
    /// <summary>
    /// 扫描传入的程序集，并注入到DI容器
    /// </summary>
    public class AssemblysModule : Module
    {
        /// <summary>
        /// 待扫描的程序集
        /// </summary>
        private IList<Assembly> _lstAssemblies;

        public AssemblysModule(IList<Assembly> lstAssemblies)
        {
            if (lstAssemblies == null || lstAssemblies.Count == 0) 
            {
                throw new ArgumentException("assemblies registered must be not null or empty", nameof(lstAssemblies));
            }
            _lstAssemblies = lstAssemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var lstComponents = ComponentLoader.GetAllComponentsFromAssemblies(_lstAssemblies);
            ComponentLoader.RegisterComponents(builder, lstComponents);
        }
    }
}
