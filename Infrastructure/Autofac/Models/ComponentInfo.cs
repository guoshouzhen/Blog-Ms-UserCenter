using Infrastructure.Autofac.Enums;
using System;
using System.Collections.Generic;

namespace Infrastructure.Autofac.Models
{
    /// <summary>
    /// 扫描的组件信息封装
    /// </summary>
    public class ComponentInfo
    {
        /// <summary>
        /// 当前组件的类型
        /// </summary>
        public Type ComponentType { get; set; }

        /// <summary>
        /// 需要注册的所有服务
        /// </summary>
        public IDictionary<Type, IDictionary<string, ServiceDetail>> ServiceDict { get; set; }
            = new Dictionary<Type, IDictionary<string, ServiceDetail>>();

    }

    /// <summary>
    /// 需要注册进DI容器的服务详情
    /// </summary>
    public class ServiceDetail 
    {
        /// <summary>
        /// 服务类型
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// 服务别名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务生命周期
        /// </summary>
        public ObjectLifeCycleEnum LifeCycle { get; set; }
    }
}
