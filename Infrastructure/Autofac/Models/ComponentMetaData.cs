using Infrastructure.Autofac.Attributes;
using System;
using System.Collections.Generic;

namespace Infrastructure.Autofac.Models
{
    /// <summary>
    /// 组件元数据定义
    /// </summary>
    public class ComponentMetaData
    {
        /// <summary>
        /// 组件Type信息
        /// </summary>
        public Type CType { get; set; }
        /// <summary>
        /// 组件DI标签
        /// </summary>
        public ComponentAttribute DIMetaData { get; set; }
    }
}
