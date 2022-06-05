using Infrastructure.Models.Attributes;
using Infrastructure.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Bo.Auth;
using Model.Dto.User;
using Model.Enums;
using Model.Vo.User;
using Service.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserCenterWebApi.Filters;

namespace UserCenterWebApi.Controllers.OpenApi
{
    /// <summary>
    /// 用户控制器
    /// ApiController自动实现参数自动序列化
    /// </summary>
    [ServiceFilter(typeof(OpenAuthFilterAttribute))]
    [Route("api/openapi")]
    [ApiController]
    public class UserController:ControllerBase
    {
        /// <summary>
        /// 注入
        /// </summary>
        private readonly IUserService _userService;
        private readonly LoginInfoBo _loginInfoBo;

        public UserController(IUserService userService, LoginInfoBo loginInfoBo)
        {
            _userService = userService;
            _loginInfoBo = loginInfoBo;
        }

        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResult>> Login(LoginDto loginDto) 
        {
            LoginVo loginVo = await _userService.LoginAsync(loginDto);
            return ApiResult.Success(loginVo);
        }

        /// <summary>
        /// 用户注册接口
        /// </summary>
        /// <param name="signupDto"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResult>> Signup(SignupDto signupDto) 
        {
            SignupVo signupVo = await _userService.SignupAsync(signupDto);
            return ApiResult.Success(signupVo);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="signupDto"></param>
        /// <returns></returns>
        [HttpGet("userinfo")]
        [RequiredAuthorities(AuthorityEnum.ROLE_USER)]
        public async Task<ActionResult<ApiResult>> GetCurrUserInfo()
        {
            UserInfoVo userInfoVo = await _userService.GetUserInfoAsync(_loginInfoBo.UserId);
            return ApiResult.Success(userInfoVo);
        }

        /// <summary>
        /// 获取用户列表（需要管理员权限）
        /// </summary>
        /// <param name="signupDto"></param>
        /// <returns></returns>
        [HttpGet("user_lists")]
        [RequiredAuthorities(AuthorityEnum.ROLE_ADMIN)]
        public async Task<ActionResult<ApiResult>> GetUserList()
        {
            List<UserInfoVo> lstUserInfos = await _userService.GetUserListAsync();
            return ApiResult.Success(lstUserInfos);
        }
    }
}
