using Infrastructure.Autofac.Enums;
using System;

namespace Infrastructure.Autofac.Attributes
{
    /// <summary>
    /// 服务或者组件注入标签（只有包含该特性的类才会被注入DI容器）
    /// 注：一般注入的都是业务相关的自定义接口和其实现类，不要去实现某些集合等泛型接口，如IList<T>、IEnumberable<T>等，否则扫描注册时可能会报错
    /// </summary>
    public class ComponentAttribute : Attribute
    {
        public ComponentAttribute() { }

        public ComponentAttribute(Type tService)
        {
            ServiceType = tService;
        }

        /// <summary>
        /// 组件声明周期
        /// </summary>
        public ObjectLifeCycleEnum LifeCycle { get; set; } = ObjectLifeCycleEnum.Default;

        /// <summary>
        /// 注册顺序，越小的先被注册
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 服务注册类型
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// 注册名称（可由Name + Type确定唯一实例）
        /// </summary>
        public string Name { get; set; }
    }
}
