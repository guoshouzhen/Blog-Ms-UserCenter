using Model.Constant;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    /// <summary>
    /// httpclient工具类，调用api
    /// </summary>
    public class HttpClientUtil
    {
        #region 鉴权中心
        /// <summary>
        /// 请求鉴权中心数据
        /// </summary>
        /// <param name="httpClient">HttpClient，通过IHttpClientFactory指定客户端名称获取</param>
        /// <param name="strUrl"></param>
        /// <param name="strPostData"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAuthCenterApiDataByPost(HttpClient httpClient, string strUrl, string strPostData, Dictionary<string, string> headers = null) 
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
        #endregion

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
    }
}
