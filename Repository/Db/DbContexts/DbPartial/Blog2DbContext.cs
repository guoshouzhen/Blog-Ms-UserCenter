using Infrastructure.Autofac.Attributes;
using Infrastructure.Autofac.Enums;
using Microsoft.Extensions.Options;
using Model.Options;

namespace Repository.Db.DbContexts.DbPartial
{
    [Repository(LifeCycle = ObjectLifeCycleEnum.InstancePerLifetimeScope)]
    public class Blog2DbContext : DbContextBase
    {
        /// <summary>
        /// 2库连接字符串
        /// </summary>
        protected override string ConnectionString { get; }

        public Blog2DbContext(IOptions<DbConfigOptions> dbConfigOptions, IOptions<EncryptOptions> encryptOptions) 
        {
            //TODO：实现解密
            ConnectionString = dbConfigOptions.Value.Blog2.ConnectionString;
        }
    }
}
