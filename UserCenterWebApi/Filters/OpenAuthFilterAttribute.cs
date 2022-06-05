using Infrastructure.Exceptions;
using Infrastructure.HttpClients;
using Infrastructure.Models.Attributes;
using Infrastructure.Models.Model;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.Bo.Auth;
using Model.Constant;
using Model.Enums;
using Model.Vo;
using Model.Vo.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserCenterWebApi.Filters
{
    /// <summary>
    /// 对外接口鉴权
    /// </summary>
    public class OpenAuthFilterAttribute : AsyncAuthFilterAttribute
    {
        /// <summary>
        /// 接口权限列表
        /// </summary>
        private IList<AuthorityEnum> _lstAuths;
        /// <summary>
        /// token
        /// </summary>
        private string _token;

        /// <summary>
        /// 注入HttpClientHelper
        /// </summary>
        private readonly IHttpClientHelper _httpClientHelper;
        public OpenAuthFilterAttribute(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }

        /// <summary>
        /// 鉴权
        /// </summary>
        /// <param name="context"></param>
        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //验证签名和时间戳
            CheckSignAndTimestamp(context, TheAuthOptions.Value.OpenApiKey);

            //获取请求处理方法上标记的元数据
            List<object> lstMetas = context.ActionDescriptor.EndpointMetadata as List<object>;

            //是否允许匿名访问
            if (lstMetas.Any(x => x is IAllowAnonymous)) 
            {
                return;
            }

            //获取需要验证的权限
            var authAttr = lstMetas.Find(x => x is RequiredAuthoritiesAttribute);
            if (authAttr != null) 
            {
                RequiredAuthoritiesAttribute authoritiesAttribute = authAttr as RequiredAuthoritiesAttribute;
                _lstAuths = authoritiesAttribute.AuthList;

            }

            //获取token
            _token = context.HttpContext.Request.Headers[HttpContextConstant.TOKEN_HEADER_KEY];

            //调用鉴权中心
            await DoAuthorizationAsync(context);
        }

        /// <summary>
        /// 调用鉴权中心，获取用户信息，鉴权
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task DoAuthorizationAsync(AuthorizationFilterContext context) 
        {
            if (string.IsNullOrWhiteSpace(_token)) 
            {
                throw new ServiceException(ErrorCodeVo.A0005);
            }
            //调用鉴权中心接口，获取登录用户信息和权限信息
            Dictionary<string, object> dictPostData = new Dictionary<string, object>();
            dictPostData["token"] = _token;
            dictPostData["auths"] = _lstAuths?.Select(x => x.ToString())?.ToArray();

            ApiResult apiResult = await _httpClientHelper.GetDataFromAuthCenterAsync("api/innerapi/check_auth", dictPostData);
            if (apiResult == null) 
            {
                throw new ServiceException(ErrorCodeVo.A0008);
            }
            if (apiResult.Result != ApiResultSuccessConstant.STATUS) 
            {
                ErrorCodeVo errorCodeVo = ErrorCodeVo.A0008;
                switch (apiResult.Code) 
                {
                    case "A0005":
                        errorCodeVo = ErrorCodeVo.A0005;
                        break;
                    case "A0006":
                        errorCodeVo = ErrorCodeVo.A0006;
                        break;
                    default:
                        break;
                }
                throw new ServiceException(errorCodeVo);
            }
            AuthVo authVo = JsonUtil.Json2Object<AuthVo>(JsonUtil.Object2Json(apiResult.Data));
            //鉴权中心返回的用户id和数据库不能为空，否则后续处理可能会报错
            if (authVo == null || authVo.LoginInfo == null || authVo.LoginInfo.UserId == 0 || string.IsNullOrWhiteSpace(authVo.LoginInfo.DbId) || authVo.Authorities == null) 
            {
                throw new ServiceException(ErrorCodeVo.A0008);
            }
            LoginInfoBo loginInfoBo = authVo.LoginInfo;
            loginInfoBo.Authorities = authVo.Authorities.Where(x => x.IsAuthorized).Select(x => EnumUtil.ParseToEnum<AuthorityEnum>(x.AuthName)).ToList();
            //鉴权
            if (!CheckUserAuth(_lstAuths, loginInfoBo.Authorities)) 
            {
                throw new ServiceException(ErrorCodeVo.A0007);
            }
            context.HttpContext.Items.Add(HttpContextConstant.LOGIN_INFO_KEY, loginInfoBo);
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="lstRequiredAuths">需要的权限</param>
        /// <param name="lstAuthorizedAuths">用户拥有的权限</param>
        private bool CheckUserAuth(IList<AuthorityEnum> lstRequiredAuths, IList<AuthorityEnum> lstAuthorizedAuths) 
        {
            if (lstRequiredAuths == null || lstRequiredAuths.Count == 0) 
            {
                //无需权限
                return true;
            }
            if (lstAuthorizedAuths == null || lstAuthorizedAuths.Count == 0) 
            {
                return false;
            }
            return !lstRequiredAuths.Any(x => !lstAuthorizedAuths.Contains(x));
        }
    }
}
