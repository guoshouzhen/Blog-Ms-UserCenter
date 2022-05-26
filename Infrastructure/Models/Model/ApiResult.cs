using Model.Constant;
using Newtonsoft.Json;

namespace Infrastructure.Models.Model
{
    /// <summary>
    /// 接口响应结果格式定义
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 返回结果 0-失败 1-成功
        /// </summary>
        [JsonProperty(PropertyName = "result", Order = 1)]
        public int Result { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "code", Order = 2)]
        public string Code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty(PropertyName = "message", Order = 3)]
        public string Message { get; set; }
        /// <summary>
        /// 传送数据
        /// </summary>
        [JsonProperty(PropertyName = "data", Order = 4)]
        public object Data { get; set; }

        /// <summary>
        /// 成功，无传送数据
        /// </summary>
        /// <returns></returns>
        public static ApiResult Success() 
        {
            return Success<object>(null);
        }

        /// <summary>
        /// 成功，有传送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Success<T>(T data) where T : class
        {
            return new ApiResult()
            {
                Result = ApiResultSuccessConstant.STATUS,
                Code = ApiResultSuccessConstant.CODE,
                Message = ApiResultSuccessConstant.MESSAGE,
                Data = data
            };
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <returns></returns>
        public static ApiResult Fail()
        {
            return Fail<object>(ApiResultFailedConstant.CODE, ApiResultFailedConstant.MESSAGE, null);
        }

        /// <summary>
        /// 失败，有指定code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ApiResult Fail(string code)
        {
            return Fail<object>(code, ApiResultFailedConstant.MESSAGE, null);
        }

        /// <summary>
        /// 失败，有指定code，message
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult Fail(string code, string message) 
        {
            return Fail<object>(code, message, null);
        }

        /// <summary>
        /// 失败，有指定code，message，有传送数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Fail<T>(string code, string message, T data) where T : class
        {
            return new ApiResult()
            {
                Result = ApiResultFailedConstant.STATUS,
                Code = code,
                Message = message,
                Data = data
            };
        }
    }
}
