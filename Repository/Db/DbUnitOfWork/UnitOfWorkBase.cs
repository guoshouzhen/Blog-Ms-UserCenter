using Infrastructure.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Model.Entities.Base;
using Repository.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Db.DbUnitOfWork
{
    /// <summary>
    /// 数据库工作单元
    /// </summary>
    public abstract class UnitOfWorkBase : IDisposable
    {
        protected readonly DbContext _dbContext;
        protected readonly ILoggerHelper<UnitOfWorkBase> _loggerHelper;
        protected ConcurrentDictionary<Type, object> _repositories;
        private IDbContextTransaction _dbContextTransaction;
        private bool _disposed = false;

        public UnitOfWorkBase(DbContext dbContext, ILoggerHelper<UnitOfWorkBase> loggerHelper) 
        {
            _dbContext = dbContext;
            _loggerHelper = loggerHelper;
            _repositories = new ConcurrentDictionary<Type, object>();
        }

        public IRepository<TEntity> Table<TEntity>() where TEntity : Entity 
        {
            return _repositories.GetOrAdd(typeof(TEntity), t => new MyRepository<TEntity>(_dbContext)) as IRepository<TEntity>;
        }

        #region 支持Sql语句执行
        /// <summary>
        /// 查询指定sql，返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataTable QueryBySql(string sql, params object[] parameters) 
        {
            return _dbContext.Database.SqlQuery(sql, parameters);
        }

        /// <summary>
        /// 查询指定sql，转成指定类型的对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> QueryBySql<T>(string sql, params object[] parameters)
            where T : class, new()
        {
            return _dbContext.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// 执行sql，返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql, params object[] parameters) 
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        #endregion

        #region 支持事务处理
        /// <summary>
        /// 开启一个事务
        /// </summary>
        /// <returns></returns>
        public virtual IDbContextTransaction BeginTransaction() 
        {
            _dbContextTransaction = _dbContext.Database.BeginTransaction();
            return _dbContextTransaction;
        }

        /// <summary>
        /// 异步开启事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified, CancellationToken cancellationToken = default) 
        {
            _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return _dbContextTransaction;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void CommitTransaction() 
        {
            _dbContextTransaction?.Commit();
            _dbContextTransaction = null;
        }

        /// <summary>
        /// 异步提交事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task CommitTransactionAsync(CancellationToken cancellationToken = default) 
        {
            await _dbContextTransaction?.CommitAsync(cancellationToken);
            _dbContextTransaction = null;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public virtual void RollbackTransaction()
        {
            _dbContextTransaction?.Rollback();
            _dbContextTransaction = null;
        }

        /// <summary>
        /// 异步回滚事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _dbContextTransaction?.RollbackAsync(cancellationToken);
            _dbContextTransaction = null;
        }
        #endregion

        #region SaveChanges
        public virtual void SaveChanges() 
        {
            _dbContext.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region 资源释放
        public void Dispose()
        {
            Dispose(true);
            //防止GC对不需要再回收的对象进行回收
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (_disposed == false && disposing) 
            {
                //释放仓储资源
                if (_repositories != null && _repositories.Count > 0) 
                {
                    foreach (var kvp in _repositories) 
                    {
                        var repos = kvp.Value as IDisposable;
                        repos?.Dispose();
                    }
                    _repositories?.Clear();
                }

                //处理事务
                if (_dbContextTransaction != null) 
                {
                    _loggerHelper.ErrorLog("存在未处理的事务，即将回滚");
                    _dbContextTransaction.Rollback();
                    _dbContextTransaction = null;
                }
                _dbContext?.Dispose();
                _disposed = true;
            }
        }
        #endregion
    }
}
