using Model.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Model.Bo.Auth
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginInfoBo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [JsonProperty(PropertyName = "userId")]
        public long UserId { get; set; }

        /// <summary>
        /// 数据库id
        /// </summary>
        [JsonProperty(PropertyName = "dbId")]
        public string DbId { get; set; }

        /// <summary>
        /// 用户拥有的权限
        /// </summary>
        [JsonIgnore]
        public IList<AuthorityEnum> Authorities { get; set; } = new List<AuthorityEnum>();
    }
}
