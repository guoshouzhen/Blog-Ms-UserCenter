namespace Dal.DalBases.Impl
{
    /// <summary>
    /// Dal基类
    /// </summary>
    public class DalBase : IDalBase
    {
        /// <summary>
        /// 属性自动注入
        /// </summary>
        public IDalContext DalContext { get; set; }
    }
}
