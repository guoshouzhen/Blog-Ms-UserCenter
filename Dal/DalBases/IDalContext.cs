using Repository.Db.DbUnitOfWork;

namespace Dal.DalBases
{
    /// <summary>
    /// Dal层数据库上下文封装
    /// 包括执行数据库操作的DBContext和及切换数据库的方法（部分数据可能只会保存在主库）
    /// </summary>
    public interface IDalContext
    {
        /// <summary>
        /// 对指定数据库执行操作的工作单元
        /// </summary>
        UnitOfWorkBase UnitOfWork { get; }

        /// <summary>
        /// 根据指定数据库ID切换数据库
        /// </summary>
        /// <param name="strDbId"></param>
        /// <returns></returns>
        IDalContext SwitchDb(string strDbId);

        /// <summary>
        /// 重置数据库为当前请求域数据库
        /// </summary>
        /// <returns></returns>
        IDalContext ResetDb();
    }
}
