using Infrastructure.Autofac.Attributes;
using Infrastructure.Autofac.Enums;
using Infrastructure.Log;
using Repository.Db.DbContexts.DbPartial;

namespace Repository.Db.DbUnitOfWork.UnitPartial
{
    [Repository(LifeCycle = ObjectLifeCycleEnum.InstancePerLifetimeScope)]
    public class Blog2UnitOfWork : UnitOfWorkBase
    {
        public Blog2UnitOfWork(Blog2DbContext dbContext, ILoggerHelper<UnitOfWorkBase> loggerHelper) : base(dbContext, loggerHelper)
        {
        }
    }
}
