namespace Model.Enums
{
    /// <summary>
    /// 日志目录枚举，可根据用途归类，方便查找
    /// </summary>
    public enum LogFolderEnum
    {
        /// <summary>
        /// Http请求监控日志目录
        /// </summary>
        ApiMonitor,

        /// <summary>
        /// 鉴权中心接口调用失败日志
        /// </summary>
        CallAuthCenterFailedLog,

        /// <summary>
        /// Redis日志
        /// </summary>
        RedisLog,

        /// <summary>
        /// 数据库日志目录
        /// </summary>
        DbLog,

        /// <summary>
        /// 方法耗时监控日志目录
        /// </summary>
        MethodCostTime,

        /// <summary>
        /// 登陆监控日志
        /// </summary>
        LoginMonitor,
    }
}
