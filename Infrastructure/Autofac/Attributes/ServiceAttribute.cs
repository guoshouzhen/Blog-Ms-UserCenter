using System;

namespace Infrastructure.Autofac.Attributes
{
    /// <summary>
    /// Service层服务注入标签
    /// </summary>
    public class ServiceAttribute : ComponentAttribute
    {
        public ServiceAttribute() : base() { }
        public ServiceAttribute(Type tService) : base(tService) { }
    }
}
