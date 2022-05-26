using Dal.Dal.Users;
using Dal.DalBases;
using Infrastructure.Autofac.Attributes;
using Infrastructure.Exceptions;
using Infrastructure.HttpClients;
using Infrastructure.Models.Model;
using Infrastructure.Utils;
using Model.Constant;
using Model.Dto.User;
using Model.Entities;
using Model.Enums;
using Model.Vo;
using Model.Vo.User;
using Service.SocketApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Users.Impl
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [Service]
    public class UserService : IUserService
    {
        /// <summary>
        /// 用户dal
        /// </summary>
        private readonly IUserDal _userDal;
        /// <summary>
        /// HttpClientHelper
        /// </summary>
        private readonly IHttpClientHelper _httpClientHelper;
        /// <summary>
        /// socket服务
        /// </summary>
        private readonly ISocketService _socketService;

        public UserService(IDalProvoder dalProvoder, IHttpClientHelper httpClientHelper, ISocketService socketService)
        {
            _userDal = dalProvoder.Dal<IUserDal>();
            _httpClientHelper = httpClientHelper;
            _socketService = socketService;
        }

        /// <summary>
        /// 账密登录
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        public async Task<LoginVo> LoginAsync(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password)) 
            {
                throw new ServiceException(ErrorCodeVo.U0100);
            }
            var user = await _userDal.SelectUserByUsernameAsync(loginDto.Username);
            //判断用户是否存在
            if (user == null) 
            {
                throw new ServiceException(ErrorCodeVo.U0001);
            }
            //判断密码是否正确
            string s = EncryptUtil.Md5ToLower(loginDto.Password);
            if (EncryptUtil.Md5ToLower(loginDto.Password) != user.PassWord) 
            {
                throw new ServiceException(ErrorCodeVo.U0002);
            }
            //判断账户是否被禁用
            if (user.Status == (int)UserStatusEnum.BAN) 
            {
                throw new ServiceException(ErrorCodeVo.U0003);
            }
            //验证通过，调用鉴权中心获取token
            string token = await GetTokenFromAuthCenterAsync(user.Id, DbUtil.GetDbId(user.UserName));
            if (string.IsNullOrWhiteSpace(token)) 
            {
                throw new ServiceException(ErrorCodeVo.U0102);
            }
            return new LoginVo() { Token = token };
        }

        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="signupDto"></param>
        /// <returns></returns>
        public async Task<SignupVo> SignupAsync(SignupDto signupDto)
        {
            //检查用户名
            if (string.IsNullOrWhiteSpace(signupDto.Username)) 
            {
                throw new ServiceException(ErrorCodeVo.U0103);
            }
            var user = await _userDal.SelectUserByUsernameAsync(signupDto.Username);
            //判断用户是否存在
            if (user != null)
            {
                throw new ServiceException(ErrorCodeVo.U0104);
            }

            //检查密码
            if (string.IsNullOrWhiteSpace(signupDto.Password)) 
            {
                throw new ServiceException(ErrorCodeVo.U0105);
            }
            if (signupDto.Password.Length < 6)
            {
                throw new ServiceException(ErrorCodeVo.U0105);
            }

            //检查邮箱
            if (string.IsNullOrWhiteSpace(signupDto.Email))
            {
                throw new ServiceException(ErrorCodeVo.U0107);
            }

            //TODO 保存头像图片到文件服务器，获取文件名
            string strAvatarName = "default.png";

            //申请用户id
            long lUserId = _socketService.GetUserId();
            if (lUserId == 0) 
            {
                throw new ServiceException(ErrorCodeVo.U0111);
            }
            User newUser = new User()
            {
                Id = lUserId,
                UserName = signupDto.Username,
                PassWord = EncryptUtil.Md5ToLower(signupDto.Password),
                FullName = signupDto.FullName,
                Email = signupDto.Email,
                Mobile = signupDto.Mobile,
                Avatar = strAvatarName,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                Status = (int)UserStatusEnum.VALID
            };
            user = await _userDal.InsertUserAsync(newUser);
            if (user == null) 
            {
                throw new ServiceException(ErrorCodeVo.U0111);
            }
            return new SignupVo() { Username = user.UserName };
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="lUserId"></param>
        /// <returns></returns>
        public async Task<UserInfoVo> GetUserInfoAsync(long lUserId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserInfoVo>> GetUserListAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 向鉴权中心申请用户token
        /// </summary>
        /// <param name="lUserId"></param>
        /// <param name="strDbId"></param>
        /// <returns></returns>
        private async Task<string> GetTokenFromAuthCenterAsync(long lUserId, string strDbId) 
        {
            Dictionary<string, object> dicPostData = new Dictionary<string, object>();
            dicPostData["userId"] = lUserId;
            dicPostData["dbId"] = strDbId;

            ApiResult apiResult = await _httpClientHelper.GetDataFromAuthCenterAsync("authcenter/api/innerapi/token_get", dicPostData);
            if (apiResult == null || apiResult.Result != ApiResultSuccessConstant.STATUS) 
            {
                return string.Empty;
            }
            var result = JsonUtil.Json2Object<LoginVo>(JsonUtil.Object2Json(apiResult.Data));
            return result?.Token;
        }
    }
}
