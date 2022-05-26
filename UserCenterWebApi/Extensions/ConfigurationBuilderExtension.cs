using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace UserCenterWebApi.Extensions
{
    /// <summary>
    /// IConfigurationBuilder扩展类
    /// </summary>
    public static class ConfigurationBuilderExtension
    {
        /// <summary>
        /// 根据环境变量启用配置文件
        /// </summary>
        /// <param name="configurationBuilder">configurationBuilder</param>
        /// <param name="env">环境信息</param>
        /// <returns></returns>
        public static IConfigurationBuilder ConfigureJsonFile(this IConfigurationBuilder configurationBuilder, IHostEnvironment env)
        {
            configurationBuilder.SetBasePath(env.ContentRootPath);

            //EnvironmentName可预先通过命令在服务器上进行设置
            //设置示例：
            //Linux：export ASPNETCORE_ENVIRONMENT=dev
            //Windows：setx ASPNETCORE_ENVIRONMENT "Development"
            switch (env.EnvironmentName)
            {
                //生产环境
                case "prod":
                    //所有配置文件为必须，且有修改过需要重新加载
                    configurationBuilder.AddJsonFile("appsettings.prod.json", optional: false, reloadOnChange: true);
                    break;
                //开发环境
                case "dev":
                case "Development":
                    configurationBuilder.AddJsonFile("appsettings.dev.json", optional: false, reloadOnChange: true);
                    break;
                //其他环境...
                default:
                    configurationBuilder.AddJsonFile("appsettings.prod.json", optional: false, reloadOnChange: true);
                    break;
            }
            configurationBuilder.AddEnvironmentVariables();
            return configurationBuilder;
        }
    }
}
