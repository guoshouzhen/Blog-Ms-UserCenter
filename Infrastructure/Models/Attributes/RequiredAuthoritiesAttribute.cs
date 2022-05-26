using Model.Enums;
using System;
using System.Collections.Generic;

namespace Infrastructure.Models.Attributes
{
    /// <summary>
    /// 使用该特性标记的Controller方法将进行权限验证（对外接口使用）
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RequiredAuthoritiesAttribute : Attribute
    {
        public IList<AuthorityEnum> AuthList { get; private set; }
        public RequiredAuthoritiesAttribute(params AuthorityEnum[] authorityEnums) 
        {
            AuthList = authorityEnums;
        }
    }
}
