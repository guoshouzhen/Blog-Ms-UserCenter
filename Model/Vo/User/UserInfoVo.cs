using Newtonsoft.Json;

namespace Model.Vo.User
{
    /// <summary>
    /// 返回给前端的用户信息
    /// </summary>
    public class UserInfoVo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [JsonProperty(PropertyName = "userId")]
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }

        /// <summary>
        /// 头像（base64）
        /// </summary>
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
    }
}
