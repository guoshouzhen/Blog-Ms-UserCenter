using Infrastructure.Autofac.Attributes;
using Infrastructure.Autofac.Enums;
using Microsoft.Extensions.Options;
using Model.Options;

namespace Repository.Db.DbContexts.DbPartial
{
    [Repository(LifeCycle = ObjectLifeCycleEnum.InstancePerLifetimeScope)]
    public class Blog1DbContext : DbContextBase
    {
        /// <summary>
        /// 1库连接字符串
        /// </summary>
        protected override string ConnectionString { get; }

        public Blog1DbContext(IOptions<DbConfigOptions> dbConfigOptions, IOptions<EncryptOptions> encryptOptions)
        {
            //TODO：解密实现
            ConnectionString = dbConfigOptions.Value.Blog1.ConnectionString;
        }
    }
}
