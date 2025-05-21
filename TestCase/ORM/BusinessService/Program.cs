using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    class Program : ProcessGuardSwitchProcess
    {
        static async Task Main(string[] args)
        {
            Program program = new Program(args);
            if (!await program.switchProcess())
            {
                program.start().NotWait();
                Console.WriteLine("Press quit to exit.");
                while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                await program.exit();
            }
        }

        private Program(string[] args) : base(args) { }
        /// <summary>
        /// 命令服务端配置
        /// </summary>
        protected CommandServerConfig commandServerConfig;
        /// <summary>
        /// 业务数据服务
        /// </summary>
        protected CommandListener commandListener;
        /// <summary>
        /// 获取进程守护节点客户端
        /// </summary>
        protected override StreamPersistenceMemoryDatabaseClientNodeCache<IProcessGuardNodeClientNode> getProcessGuardClient
        {
            get { return ProcessGuardClientSocketEvent.ProcessGuardNodeCache; }
        }
        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <returns></returns>
        protected override async Task initialize()
        {
            await Persistence.Initialize();

            commandServerConfig = new CommandServerCompressConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ORM, null) };
            commandListener = new CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString)) //添加服务认证接口
                .Append<IAutoIdentityModelService>(string.Empty, server => new AutoIdentityModelService())
                .Append<IFieldModelService>(string.Empty, server => new FieldModelService())
                .Append<IPropertyModelService>(string.Empty, server => new PropertyModelService())
                .Append<ICustomColumnFieldModelService>(string.Empty, server => new CustomColumnFieldModelService())
                .Append<ICustomColumnPropertyModelService>(string.Empty, server => new CustomColumnPropertyModelService())
                .CreateCommandListener(commandServerConfig);

            await base.initialize();
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        protected override async Task onStart()
        {
            await base.onStart();

            if (await commandListener.Start())
            {
                ConsoleWriteQueue.WriteLine($"业务数据服务启动成功 {commandServerConfig.Host.Host}:{commandServerConfig.Host.Port}");
            }
        }
        /// <summary>
        /// 退出运行
        /// </summary>
        /// <returns></returns>
        protected override Task onExit()
        {
            commandListener?.DisposeSocket();
            return base.onExit();
        }
    }
}
