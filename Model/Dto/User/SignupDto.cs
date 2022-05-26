using Newtonsoft.Json;

namespace Model.Dto.User
{
    public class SignupDto
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
        /// 全名
        /// </summary>
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// 手机号	
        /// </summary>
        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// 头像（base64）
        /// </summary>
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
    }
}
