using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DatabaseBackup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigFile configFile = ConfigFile.Default;
            if (configFile == null)
            {
                Console.ReadKey();
                return;
            }
            CommandServerConfig commandServerConfig = new CommandServerConfig
            {
                MinCompressSize = 1024,
                Host = new HostEndPoint(configFile.ServerPort, configFile.ServerHost)
            };
            using (CommandListener commandListener = new CommandListener(commandServerConfig
                , CommandServerInterfaceControllerCreator.GetCreator(server => (ITimestampVerify)new AutoCSer.CommandService.TimestampVerify(configFile.VerifyString))
                , CommandServerInterfaceControllerCreator.GetCreator(server => (IDatabaseBackup)new DatabaseBackupService())
                ))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine($"数据库备份服务启动成功 {commandServerConfig.Host.Host}:{commandServerConfig.Host.Port}");
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
                else
                {
                    Console.WriteLine($"数据库备份服务启动失败 {commandServerConfig.Host.Host}:{commandServerConfig.Host.Port}");
                    Console.ReadKey();
                }
            }
        }
    }
}
