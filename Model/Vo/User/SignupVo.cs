using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Vo.User
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class SignupVo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
    }
}
