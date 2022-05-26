using Newtonsoft.Json;

namespace Model.Dto.User
{
    public class LoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
    }
}
