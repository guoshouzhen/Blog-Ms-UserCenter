namespace Dal.DalBases
{
    /// <summary>
    /// Dal基类约束
    /// </summary>
    public interface IDalBase
    {
        /// <summary>
        /// Dal层数据库上下文封装
        /// </summary>
        IDalContext DalContext { get; set; }
    }
}
