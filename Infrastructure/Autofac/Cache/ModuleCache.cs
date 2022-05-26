using Autofac;
using System.Collections.Generic;

namespace Infrastructure.Autofac.Cache
{
    /// <summary>
    /// 缓存扫描的待注册服务
    /// </summary>
    internal class ModuleCache
    {
        private static IList<Module> _modules;

        static ModuleCache() 
        {
            _modules = new List<Module>();
        }

        public static IList<Module> Modules 
        {
            get 
            {
                return _modules;
            }
        }
    }
}
