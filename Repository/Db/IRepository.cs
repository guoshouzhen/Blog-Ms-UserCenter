using Model.Entities.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Db
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> 
        where TEntity : Entity
    {
        /// <summary>
        /// 向DB添加一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Remove(TEntity entity);
        Task<bool> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据指定条件查询实体对象
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        List<TEntity> GetByConditions(Func<TEntity, bool> condition);
        Task<List<TEntity>> GetByConditionsAsync(Func<TEntity, bool> condition);
    }
}
