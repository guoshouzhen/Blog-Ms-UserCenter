using Newtonsoft.Json;

namespace Model.Bo.Auth
{
    public class AuthorityBo
    {
        /// <summary>
        /// 权限名
        /// </summary>
        [JsonProperty(PropertyName = "authName")]
        public string AuthName { get; set; }

        /// <summary>
        /// 是否有该权限
        /// </summary>
        [JsonProperty(PropertyName = "isAuthorized")]
        public bool IsAuthorized { get; set; }
    }
}
