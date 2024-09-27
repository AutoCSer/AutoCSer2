using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    class Program : AutoCSer.TestCase.Common.CommandServiceSwitchProcess
    {
        private Program(string[] args) : base(args) { }

        static async Task Main(string[] args)
        {
            await start(new Program(args));
        }
        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <returns></returns>
        protected override async Task initialize()
        {
            await Persistence.Initialize();

            commandServerConfig = new CommandServerConfig { MinCompressSize = 1024, Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ORM, null) };
            commandListener = new CommandListenerBuilder(0)
                .Append(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString)) //添加服务认证接口
                .Append<IAutoIdentityModelService>(string.Empty, server => new AutoIdentityModelService())
                .Append<IFieldModelService>(string.Empty, server => new FieldModelService())
                .Append<IPropertyModelService>(string.Empty, server => new PropertyModelService())
                .Append<ICustomColumnFieldModelService>(string.Empty, server => new CustomColumnFieldModelService())
                .Append<ICustomColumnPropertyModelService>(string.Empty, server => new CustomColumnPropertyModelService())
                .CreateCommandListener(commandServerConfig);

            await base.initialize();
        }
    }
}
