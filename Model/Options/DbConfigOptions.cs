namespace Model.Options
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfigOptions
    {
        /// <summary>
        /// 1库（默认主库）
        /// </summary>
        public DbConfiguration Blog1 { get; set; }
        /// <summary>
        /// 2库（分库）
        /// </summary>
        public DbConfiguration Blog2 { get; set; }
    }

    /// <summary>
    /// 数据库详细配置
    /// </summary>
    public class DbConfiguration
    {
        /// <summary>
        /// 数据库别名
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 数据库类型枚举
        /// </summary>
        public DbTypeEnum DbType { get; set; }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbTypeEnum 
    {
        MySql,
    }
}
