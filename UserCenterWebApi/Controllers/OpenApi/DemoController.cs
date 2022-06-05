using Infrastructure.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Demo;
using System.Threading.Tasks;
using UserCenterWebApi.Filters;

namespace UserCenterWebApi.Controllers.OpenApi
{
    [Route("api/openapi/demo")]
    [ServiceFilter(typeof(OpenAuthFilterAttribute))]
    public class DemoController : ControllerBase
    {
        /// <summary>
        /// Api测试用服务
        /// </summary>
        private readonly IDemoService _demoService;

        public DemoController(IDemoService demoService)
        {
            _demoService = demoService;
        }

        /// <summary>
        /// 测试API
        /// </summary>
        /// <returns></returns>
        [HttpGet("apitest")]
        [AllowAnonymous]
        public async Task<ApiResult> DemoAction()
        {
            return await Task.FromResult(ApiResult.Success());
        }

        /// <summary>
        /// 日志测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("logtest")]
        [AllowAnonymous]
        public async Task<ApiResult> TestLog()
        {
            var result = await _demoService.TestLogsAsync();
            return ApiResult.Success(result);
        }

        /// <summary>
        /// 数据库测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("dbtest")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResult>> TestDb()
        {
            var user = await _demoService.TestMysqlAsync();
            return ApiResult.Success(user);
        }

        /// <summary>
        /// socket测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("sockettest")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResult>> TestSocket()
        {
            var ids = await _demoService.TestIdServerAsync();
            return ApiResult.Success(ids);
        }
    }
}
