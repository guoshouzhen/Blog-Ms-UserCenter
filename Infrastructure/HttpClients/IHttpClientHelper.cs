using Infrastructure.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HttpClients
{
    public interface IHttpClientHelper
    {
        /// <summary>
        /// 从鉴权中心获取数据，请求方式POST，传送Json格式数据
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="dictRequestData"></param>
        /// <param name="dictHeaders"></param>
        /// <returns></returns>
        Task<ApiResult> GetDataFromAuthCenterAsync(string requestPath, Dictionary<string, object> dictRequestData, Dictionary<string, string> dictHeaders = null);
    }
}
