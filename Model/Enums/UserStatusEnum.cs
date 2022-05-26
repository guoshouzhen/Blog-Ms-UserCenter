using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enums
{
    /// <summary>
    /// 用户状态枚举
    /// </summary>
    public enum UserStatusEnum
    {
        /// <summary>
        /// 删除状态
        /// </summary>
        DELETED = 0,

        /// <summary>
        /// 有效
        /// </summary>
        VALID = 1,
        
        /// <summary>
        /// 禁用状态
        /// </summary>
        BAN = 2,

    }
}
