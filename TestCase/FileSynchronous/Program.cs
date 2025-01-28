using AutoCSer.CommandService.DeployTask;
using AutoCSer.Net;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.FileSynchronous
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                CommandServerConfig commandServerConfig = new CommandServerConfig
                {
                    Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.FileSynchronous),
                };
                UploadFileServiceConfig uploadFileServiceConfig = new UploadFileServiceConfig
                {
                    BackupPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.DeployTask), nameof(UploadFileServiceConfig.BackupPath)),
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                    .Append<IPullFileService>(new PullFileService())
                    .Append<IUploadFileService>(uploadFileServiceConfig.Create())
                    .CreateCommandListener(commandServerConfig))
                {
                    if (await commandListener.Start())
                    {
                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
                    }
                }
            }
            catch(Exception exception)
            {
                ConsoleWriteQueue.Breakpoint(exception.ToString());
                Console.ReadLine();
            }
        }
    }
}
