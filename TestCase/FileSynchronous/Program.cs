using AutoCSer.CommandService.FileSynchronous;
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
                UploadFileServiceConfig uploadFileServiceConfig = new UploadFileServiceConfig
                {
                    BackupPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.FileSynchronous), nameof(UploadFileServiceConfig.BackupPath)),
                    CommandServerSocketSessionObject = new CommandServerSocketSessionObject()
                };
                UploadFileService uploadFileService = uploadFileServiceConfig.Create();
                CommandServerConfig commandServerConfig = new CommandServerConfig
                {
                    Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.FileSynchronous),
                    SessionObject = uploadFileService.CommandServerSocketSessionObject
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                    .Append<IPullFileService>(new PullFileService())
                    .Append<IUploadFileService>(uploadFileService)
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
