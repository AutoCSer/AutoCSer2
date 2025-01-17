using AutoCSer.Extensions;

namespace AutoCSer.Document.ServiceAuthentication
{
    internal class Program
    {
        /// <summary>
        /// https://zhuanlan.zhihu.com/p/11427440200
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            Console.WriteLine($"{nameof(TimestampVerify)} {await timestampVerify()}");
            Console.WriteLine($"{nameof(CustomVerify)} {await customVerify()}");
            Console.WriteLine($"{nameof(SetCommand)} {await setCommand()}");
            Console.WriteLine($"{nameof(TransferEncoding)} {await transferEncoding()}");

            Console.WriteLine("Press quit to exit.");
            while (Console.ReadLine() != "quit") ;
        }
        /// <summary>
        /// 无身份认证（字符串匹配认证）测试
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> timestampVerify()
        {
            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await TimestampVerify.CommandClientSocketEvent.Test();
            }
        }
        /// <summary>
        /// 自定义服务认证测试
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> customVerify()
        {
            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<CustomVerify.ICustomVerifyService>(new CustomVerify.CustomVerifyService())
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await CustomVerify.CommandClientSocketEvent.Test();
            }
        }
        /// <summary>
        /// 自定义服务认证测试
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> setCommand()
        {
            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<CustomVerify.ICustomVerifyService>(new SetCommand.CustomVerifyService())
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await SetCommand.CustomVerifyService.Test();
            }
        }
        /// <summary>
        /// 支持传输数据压缩的配置测试
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> transferEncoding()
        {
            AutoCSer.Net.CommandServerConfig commandServerConfig = new TransferEncoding.CommandServerCompressConfig
            {
                MinCompressSize = 1 << 10,
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await TransferEncoding.CommandClientCompressConfig.Test();
            }
        }
    }
}
