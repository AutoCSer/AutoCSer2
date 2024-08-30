using AutoCSer.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 默认启动配置
    /// </summary>
    public abstract class Startup
    {
        /// <summary>
        /// 服务添加到容器
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //Type helpControllerType = this.helpControllerType;
            //if (helpControllerType != null)
            //{
            //    AutoCSer.LogHelper.InfoIgnoreException(helpControllerType.GetType().fullName());
            //    services.AddTransient(helpControllerType);
            //}
            //services.AddResponseCompression(options =>
            //{
            //    options.EnableForHttps = true;
            //    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
            //    //options.MimeTypes = Microsoft.AspNetCore.ResponseCompression.ResponseCompressionDefaults.MimeTypes;
            //});
        }
    }
    /// <summary>
    /// 默认启动配置
    /// </summary>
    /// <typeparam name="T">数据视图中间件类型，定义类型需要和 Controller 在同一个程序集中</typeparam>
    public class Startup<T> : Startup where T : ViewMiddleware
    {
        /// <summary>
        /// 配置 HTTP 请求管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
#if DotNet45 || NetStandard2
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
#else
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#endif
        {
            //if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
#if DEBUG
            app.UseDeveloperExceptionPage();
#endif

            app.UseMiddleware(typeof(T));
            //app.UseMiddleware(typeof(AccessControlMiddleware));
            //app.UseRouting();
            ////app.UseAuthorization();
            //app.UseResponseCompression();

            //app.UseEndpoints(endpoints => endpoints.MapControllers());
            AutoCSer.LogHelper.InfoIgnoreException(nameof(Configure));
        }

        /// <summary>
        /// 运行默认启动配置
        /// </summary>
        /// <param name="args">Main 函数参数</param>
        /// <returns></returns>
        public static bool CreateHostBuilder(string[] args)
        {
            try
            {
#if DotNet45 || NetStandard2
                WebHost.CreateDefaultBuilder(args)
                    .UseStartup(typeof(Startup<T>))
#else
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup(typeof(Startup<T>)))
#endif
                    .Build()
                    .Run();
                return true;
            }
            catch (OperationCanceledException exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Debug | LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Fatal | LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            return false;
        }
    }
}
