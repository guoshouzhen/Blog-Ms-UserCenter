using Infrastructure.Autofac.Attributes;
using Microsoft.AspNetCore.Http;
using Model.Bo.Auth;
using Model.Constant;
using System;

namespace Service.Auth.Impl
{
    /// <summary>
    /// 获取登录信息
    /// 登录信息check Token时调用鉴权中心获取，并设置到请求上下文，该服务从其中获取并注入DI容器
    /// </summary>
    [Service]
    public class AuthService : IAuthService
    {
        /// <summary>
        /// HttpContextAccessor对象
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LoginInfoBo GetLoginInfo()
        {
            LoginInfoBo loginInfoBo = new LoginInfoBo();
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Items.ContainsKey(HttpContextConstant.LOGIN_INFO_KEY)) 
            {
                loginInfoBo = (LoginInfoBo)httpContext.Items[HttpContextConstant.LOGIN_INFO_KEY];
            }
            return loginInfoBo;
        }
    }
}
