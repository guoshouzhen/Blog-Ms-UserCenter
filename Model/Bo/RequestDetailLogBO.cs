namespace Model.Bo
{
    /// <summary>
    /// 请求日志
    /// </summary>
    public class RequestDetailLogBO
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// 花费时间
        /// </summary>
        public double CostTime { get; set; }
        /// <summary>
        /// 请求头数据
        /// </summary>
        public string RequestHeaders { get; set; }
        /// <summary>
        /// 请求url参数
        /// </summary>
        public string RequestQuery { get; set; }
        /// <summary>
        /// 请求表单参数
        /// </summary>
        public string RequestForm { get; set; }
        /// <summary>
        /// 请求体数据
        /// </summary>
        public string RequestBody { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public string ResponseBody { get; set; }
    }
}
