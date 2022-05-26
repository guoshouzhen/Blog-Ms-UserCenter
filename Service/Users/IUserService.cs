using Model.Dto.User;
using Model.Vo.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Users
{
    public interface IUserService
    {
        /// <summary>
        /// 账密登录
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        Task<LoginVo> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="signupDto"></param>
        /// <returns></returns>
        Task<SignupVo> SignupAsync(SignupDto signupDto);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="lUserId"></param>
        /// <returns></returns>
        Task<UserInfoVo> GetUserInfoAsync(long lUserId);

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        Task<List<UserInfoVo>> GetUserListAsync();
    }
}
