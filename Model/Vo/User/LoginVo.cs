using Newtonsoft.Json;

namespace Model.Vo.User
{
    public class LoginVo
    {
        /// <summary>
        /// 用户登陆令牌
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
