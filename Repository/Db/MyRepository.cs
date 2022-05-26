using Microsoft.EntityFrameworkCore;
using Model.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Db
{
    public class MyRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly DbContext _dbContext;
        public virtual DbSet<TEntity> Table => _dbContext.Set<TEntity>();

        public MyRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 向DB添加一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Add(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await Table.AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        /// <summary>
        /// 按条件查询对象集合
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<TEntity> GetByConditions(Func<TEntity, bool> condition)
        {
            return Table.Where(condition).ToList();
        }
        public async Task<List<TEntity>> GetByConditionsAsync(Func<TEntity, bool> condition)
        {
            return await Task.FromResult(Table.Where(condition).ToList());
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Remove(TEntity entity)
        {
            Table.Remove(entity);
            return true;
        }
        public async Task<bool> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(Remove(entity));
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Update(TEntity entity)
        {
            return Table.Update(entity).Entity;
        }
        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(Update(entity));
        }
    }
}
