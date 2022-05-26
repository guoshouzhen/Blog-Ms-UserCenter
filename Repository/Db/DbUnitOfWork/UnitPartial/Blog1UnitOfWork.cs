using Infrastructure.Autofac.Attributes;
using Infrastructure.Autofac.Enums;
using Infrastructure.Log;
using Repository.Db.DbContexts.DbPartial;

namespace Repository.Db.DbUnitOfWork.UnitPartial
{
    [Repository(LifeCycle = ObjectLifeCycleEnum.InstancePerLifetimeScope)]
    public class Blog1UnitOfWork : UnitOfWorkBase
    {
        public Blog1UnitOfWork(Blog1DbContext dbContext, ILoggerHelper<UnitOfWorkBase> loggerHelper) : base(dbContext, loggerHelper)
        {
        }
    }
}
