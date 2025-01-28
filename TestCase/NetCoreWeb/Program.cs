using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;
using System.IO;

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
                program.start().NotWait();
                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
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
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        private void createHost()
        {
            AutoCSer.NetCoreWeb.Startup<ViewMiddleware>.CreateHostBuilder(arguments);
        }
    }
}
