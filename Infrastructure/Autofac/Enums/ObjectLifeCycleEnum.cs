namespace Infrastructure.Autofac.Enums
{
    /// <summary>
    /// 注入Autofac容器的对象生命周期枚举
    /// </summary>
    public enum ObjectLifeCycleEnum
    {
        /// <summary>
        /// 默认InstancePerLifetimeScope
        /// </summary>
        Default = 0,
        /// <summary>
        /// 瞬时
        /// </summary>
        InstancePerDependency = 1,
        /// <summary>
        /// 作用域单例
        /// </summary>
        InstancePerLifetimeScope = 2,
        /// <summary>
        /// 单例
        /// </summary>
        SingleInstance = 3,
    }
}
