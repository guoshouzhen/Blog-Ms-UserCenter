using Infrastructure.Exceptions;
using Infrastructure.Log;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Model.Constant;
using Model.Options;
using Model.Vo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenterWebApi.Filters
{
    /// <summary>
    /// 鉴权抽象类
    /// 要做：
    /// 1. 验证sign和timestamp（支持安全的http请求）
    /// 2. 调用鉴权中心接口，验证Token，验证权限，并获取用户信息
    /// </summary>
    public abstract class AsyncAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter, IFilterMetadata, IOrderedFilter
    {
        /// <summary>
        /// 属性注入
        /// </summary>
        public IOptions<AuthOptions> TheAuthOptions { get; set; }
        public ILoggerHelper<AsyncAuthFilterAttribute> LoggerHelper { get; set; }

        public virtual int Order => 0;

        /// <summary>
        /// 具体鉴权操作，必须由子类重写
        /// </summary>
        /// <param name="context"></param>
        public virtual async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //空任务，走不到这
            await Task.Yield();
        }

        /// <summary>
        /// 验证sign和时间戳
        /// </summary>
        /// <param name="context"></param>
        /// <param name="privateKey">加密key</param>
        /// <exception cref="ServiceException"></exception>
        protected void CheckSignAndTimestamp(AuthorizationFilterContext context, string privateKey) 
        {
            var dictRequestParams = GetRequestParams(context);
            if (dictRequestParams != null && dictRequestParams.Count > 0) 
            {
                //时间戳不能为空
                if (dictRequestParams.ContainsKey(HttpContextConstant.TIMESTAMP_KEY) == false || string.IsNullOrWhiteSpace(dictRequestParams[HttpContextConstant.TIMESTAMP_KEY].ToString())) 
                {
                    throw new ServiceException(ErrorCodeVo.A0002);
                }

                //TODO 验证时间戳

                //sign不能为空
                if (dictRequestParams.ContainsKey(HttpContextConstant.SIGN_KEY) == false || string.IsNullOrWhiteSpace(dictRequestParams[HttpContextConstant.SIGN_KEY].ToString())) 
                {
                    throw new ServiceException(ErrorCodeVo.A0004);
                }

                //TODO 验证sign
            }
        }

        /// <summary>
        /// 获取请求参数（post请求暂仅支持json格式数据）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetRequestParams(AuthorizationFilterContext context) 
        {
            HttpRequest request = context.HttpContext.Request;
            Dictionary<string, object> dictParams = null;
            try
            {
                if (request.Method.ToUpper() == HttpContextConstant.GET_METHOD)
                {
                    dictParams = request.Query.ToDictionary(kvp => kvp.Key.ToLower(), kvp => kvp.Value as object);
                }
                else
                {
                    request.Body.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(request.Body, Encoding.UTF8);
                    string strParams = reader.ReadToEnd();
                    dictParams = JsonUtil.Json2Object<Dictionary<string, object>>(strParams);
                    //重置request.body stream的开始位置
                    request.Body.Seek(0, SeekOrigin.Begin);
                    //限制POST请求时参数不能为空
                    if (dictParams == null || dictParams.Count == 0) 
                    {
                        throw new Exception("POST请求体不能为空");
                    }
                }
            }
            catch (Exception ex) 
            {
                LoggerHelper.ErrorLog($"用户中心接口获取请求参数失败，请求路径：{request.Path}，异常信息：{ex}");
                throw new ServiceException(ErrorCodeVo.A0001);
            }
            return dictParams;
        }
    }
}
