using Infrastructure.Autofac.Attributes;
using Infrastructure.Autofac.Enums;
using Infrastructure.Log;
using Infrastructure.Models.Model;
using Infrastructure.Utils;
using Model.Constant;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HttpClients.Impl
{
    /// <summary>
    /// HttpClient帮助类
    /// </summary>
    [Component(LifeCycle = ObjectLifeCycleEnum.SingleInstance)]
    public class HttpClientHelper : IHttpClientHelper
    {
        /// <summary>
        /// 注入HttpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoggerHelper<HttpClientHelper> _loggerHelper;

        public HttpClientHelper(IHttpClientFactory httpClientFactory, ILoggerHelper<HttpClientHelper> loggerHelper)
        {
            _httpClientFactory = httpClientFactory;
            _loggerHelper = loggerHelper;
        }

        /// <summary>
        /// 调用鉴权中心接口
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="dictRequestData"></param>
        /// <param name="dictHeaders"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetDataFromAuthCenterAsync(string requestPath, Dictionary<string, object> dictRequestData, Dictionary<string, string> dictHeaders = null)
        {
            dictRequestData = dictRequestData ?? new Dictionary<string, object>();
            string strJsonData = JsonUtil.Object2Json(dictRequestData);
            HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNameEnum.AuthCenter.ToString());
            string strAuthResult = string.Empty;
            ApiResult apiResult = null;
            var httpResponse = await GetApiDataByPostAsync(httpClient, requestPath, strJsonData, dictHeaders);
            if (httpResponse != null)
            {
                strAuthResult = await httpResponse.Content.ReadAsStringAsync();
                apiResult = JsonUtil.Json2Object<ApiResult>(strAuthResult);
            }
            //调用失败时记录监控日志
            if (apiResult == null || apiResult.Result != ApiResultSuccessConstant.STATUS)
            {
                _loggerHelper.InfoLog($"鉴权中心调用失败\n请求路径：{httpClient.BaseAddress + requestPath}\n请求参数：{strJsonData}\n响应：{strAuthResult}\r\n", LogFolderEnum.CallAuthCenterFailedLog);
            }
            return apiResult;
        }

        #region 辅助方法

        /// <summary>
        /// 请求API数据
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="strUrl"></param>
        /// <param name="strPostData">json格式数据</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> GetApiDataByPostAsync(HttpClient httpClient, string strUrl, string strPostData, Dictionary<string, string> headers = null)
        {
            strPostData = strPostData ?? "";
            HttpResponseMessage responseMessage = null;
            using (HttpContent httpContent = new StringContent(strPostData, Encoding.UTF8))
            {
                SetHeaders(httpContent, headers);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(HttpContextConstant.CONTENT_TYPE_JSON);
                responseMessage = await httpClient.PostAsync(strUrl, httpContent);
            }
            return responseMessage;
        }

        /// <summary>
        /// 为HttpContent对象设置请求头
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="dicHeaders"></param>
        private static void SetHeaders(HttpContent httpContent, Dictionary<string, string> dicHeaders)
        {
            if (httpContent == null || dicHeaders == null || dicHeaders.Count == 0)
            {
                return;
            }
            httpContent.Headers.Clear();
            foreach (var kvp in dicHeaders)
            {
                if (string.IsNullOrWhiteSpace(kvp.Value) == false)
                {
                    httpContent.Headers.Add(kvp.Key, kvp.Value);
                }
            }
        }
        #endregion
    }
}
