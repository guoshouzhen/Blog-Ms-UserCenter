namespace Model.Options
{
    /// <summary>
    /// 鉴权相关的配置项
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// 鉴权中心地址
        /// </summary>
        public string AuthCenterAddress { get; set; }

        /// <summary>
        /// 对内接口加密key
        /// </summary>
        public string InnerApiKey { get; set; }

        /// <summary>
        /// 对外接口加密key
        /// </summary>
        public string OpenApiKey { get; set; }

        /// <summary>
        /// 接口请求有效时间（秒）
        /// </summary>
        public int ApiTimeOut { get; set; }
    }
}
