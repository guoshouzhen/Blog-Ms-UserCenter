using Infrastructure.Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserCenterWebApi
{
    public class Program
    {
        /// <summary>
        /// asp.net core�������
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //ʹ��Autofac�ӹ�DI����
                .UseServiceProviderFactory(new CustomizedServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //ʹ������������
                    webBuilder.UseStartup<Startup>();
                });
    }
}
