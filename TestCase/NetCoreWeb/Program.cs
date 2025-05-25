using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AutoCSer.TestCase.NetCoreWeb
{
    /// <summary>
    ///  
    /// </summary>
    public class Program : ProcessGuardSwitchProcess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            Program program = new Program(args);
            if (!await program.switchProcess())
            {
                program.start().Catch();
                Console.WriteLine("Press quit to exit.");
                while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                await program.exit();
            }
        }

        private Program(string[] args) : base(args) { }
        /// <summary>
        /// 获取切换执行进程文件信息
        /// </summary>
        /// <returns></returns>
        protected override Task<FileInfo> getSwitchProcessFile()
        {
            return getSwitchProcessFile("bin");
        }
        /// <summary>
        /// 获取进程守护节点客户端
        /// </summary>
        protected override StreamPersistenceMemoryDatabaseClientNodeCache<IProcessGuardNodeClientNode> getProcessGuardClient
        {
            get { return ProcessGuardCommandClientSocketEvent.ProcessGuardNodeCache; }
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        protected override Task onStart()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(createHost);
            return InterfaceRealTimeCallMonitorCommandClientSocketEvent.CommandClient.SocketEvent.Wait();
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        private void createHost()
        {
            if (!AutoCSer.TestCase.Common.JsonFileConfig.Default.IsRemote)
            {
                Console.WriteLine($@"http://localhost:5000/ExampleView.html#left=5&right=2
http://localhost:5000/Example/CallState
http://localhost:5000/Example/GetResult/5/2
http://localhost:5000/Example/GetPost/5
http://localhost:5000/IgnoreControllerRoute/5/2
http://localhost:5000/ViewHelp.html");
            }
            AutoCSer.NetCoreWeb.Startup.CreateHostBuilder(arguments, useStartup);
        }
        /// <summary>
        /// 调用 builder.UseStartup()
        /// </summary>
        /// <param name="builder"></param>
        private static void useStartup(IWebHostBuilder builder)
        {
            if (AutoCSer.TestCase.Common.JsonFileConfig.Default.IsRemote)
            {
                //如果有其它静态文件需求，需要设置 IWebHostEnvironment.ContentRootPath 避免引用 bin 目录
                builder.UseUrls("http://0.0.0.0:" + ((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.WebViewHttp).toString());
                if (!string.IsNullOrEmpty(AutoCSer.TestCase.Common.JsonFileConfig.Default.HttpsCertificateFileName))
                {
                    builder.ConfigureKestrel(serverOptions => {
                        serverOptions.ListenAnyIP(443
                            , listenOptions => listenOptions.UseHttps(AutoCSer.TestCase.Common.JsonFileConfig.Default.HttpsCertificateFileName, AutoCSer.TestCase.Common.JsonFileConfig.Default.HttpsCertificatePassword));
                    });
                }
            }
            builder.UseStartup(typeof(AutoCSer.NetCoreWeb.Startup<ViewMiddleware>));
        }
    }
}
