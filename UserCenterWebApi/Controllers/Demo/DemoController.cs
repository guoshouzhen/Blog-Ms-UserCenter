using Infrastructure.Models.Attributes;
using Infrastructure.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Enums;
using Service.Demo;
using System.Threading.Tasks;
using UserCenterWebApi.Filters;

namespace UserCenterWebApi.Controllers.Demo
{
    [Route("usercenter/api/demo")]
    [ServiceFilter(typeof(OpenAuthFilterAttribute))]
    public class DemoController: ControllerBase
    {
        /// <summary>
        /// 测试用服务
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
        public async Task<string> DemoAction()
        {
            return await _demoService.GetTestStringsAsync();
        }

        /// <summary>
        /// 日志测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("logtest")]
        public async Task<ApiResult> TestLog() 
        {
            var result = await _demoService.GetLogTestStringsAsync();
            return ApiResult.Success(result);
        }

        /// <summary>
        /// 数据库测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("dbtest")]
        [RequiredAuthorities(AuthorityEnum.ROLE_ADMIN)]
        public async Task<ActionResult<ApiResult>> TestDb()
        {
            var user = await _demoService.GetUserAsync();
            return ApiResult.Success(user);
        }
    }
}
