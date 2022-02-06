using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistry.Service
{
    /// <summary>
    /// 服务版本测试
    /// </summary>
    internal sealed class ServiceVersion
    {
        /// <summary>
        /// 服务版本
        /// </summary>
        private readonly int version;
        /// <summary>
        /// 服务版本测试
        /// </summary>
        /// <param name="version"></param>
        internal ServiceVersion(int version)
        {
            this.version = version;
        }
        /// <summary>
        /// 启动测试服务
        /// </summary>
        /// <returns></returns>
        internal async Task Start()
        {
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)(30000 + version)), ServiceName = "AutoCSer.TestCase.ServiceRegistry" };
            using (CommandListener commandListener = new CommandListener(commandServerConfig
                    , CommandServerInterfaceControllerCreator.GetCreator<IService>(new Service(version))
                    ))
            {
                if (await commandListener.Start())
                {
                    Console.Write(version);
                    await Task.Delay(5 * 1000);
                    CatchTask.AddIgnoreException(new ServiceVersion(version == 9 ? 0 : version + 1).Start());
                    await Task.Delay(1 * 1000);
                }
            }
        }
    }
}
