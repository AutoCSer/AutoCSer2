using AutoCSer.CommandService.DeployTask;
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
            CommandServerConfig commandServerConfig = new CommandServerCompressConfig
            {
                MinCompressSize = 1024,
                Host = new HostEndPoint(configFile.ServerPort, configFile.ServerHost)
            };
            await using (CommandListener commandListener = new CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, configFile.VerifyString))
                .Append<IDatabaseBackupService>(server => new DatabaseBackupService())
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine($"数据库备份服务启动成功 {commandServerConfig.Host.Host}:{commandServerConfig.Host.Port}");
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
                else
                {
                    ConsoleWriteQueue.WriteLine($"数据库备份服务启动失败 {commandServerConfig.Host.Host}:{commandServerConfig.Host.Port}", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }
        }
    }
}
