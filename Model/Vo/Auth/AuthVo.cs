using Model.Bo.Auth;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Model.Vo.Auth
{
    /// <summary>
    /// 鉴权结果
    /// </summary>
    public class AuthVo
    {
        /// <summary>
        /// 登录用户信息
        /// </summary>
        [JsonProperty(PropertyName = "loginInfo")]
        public LoginInfoBo LoginInfo { get; set; }

        /// <summary>
        /// 鉴权结果
        /// </summary>
        [JsonProperty(PropertyName = "authorities")]
        public List<AuthorityBo> Authorities { get; set; }
    }
}
