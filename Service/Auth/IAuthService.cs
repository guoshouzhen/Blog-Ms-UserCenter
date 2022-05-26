using Model.Bo.Auth;

namespace Service.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        LoginInfoBo GetLoginInfo();
    }
}
