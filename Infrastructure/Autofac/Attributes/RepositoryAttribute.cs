using System;

namespace Infrastructure.Autofac.Attributes
{
    /// <summary>
    /// 数据访问层服务注入标签
    /// </summary>
    public class RepositoryAttribute:ComponentAttribute
    {
        public RepositoryAttribute() : base() { }
        public RepositoryAttribute(Type tService) : base(tService) { }
    }
}
