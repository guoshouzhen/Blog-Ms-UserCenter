namespace Dal.DalBases
{
    /// <summary>
    /// service层注入该对象，然后具体使用到的Dal对象从该对象获取
    /// </summary>
    public interface IDalProvoder
    {
        /// <summary>
        /// 获取指定类型的Dal对象
        /// </summary>
        /// <typeparam name="TDal"></typeparam>
        /// <returns></returns>
        TDal Dal<TDal>() where TDal : IDalBase;
    }
}
