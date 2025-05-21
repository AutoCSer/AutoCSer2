using AutoCSer.Threading;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DatabaseBackupClient
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
            CommandClientConfig commandClientConfig = new CommandClientCompressConfig
            {
                Host = new HostEndPoint(configFile.ServerPort, configFile.ServerHost),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CommandClientSocketEvent socketEvent = (CommandClientSocketEvent)await commandClient.GetSocketEvent();
                if (socketEvent == null) ConsoleWriteQueue.WriteLine("数据库备份服务连接失败", ConsoleColor.Red);
                else Console.WriteLine("数据库备份服务连接成功");

                DatabaseBackupClient databaseBackupClient = new DatabaseBackupClient(commandClient, socketEvent);
                TaskRunTimer taskRunTimer = ConfigFile.Default.GetTaskRunTimer();
                do
                {
                    await taskRunTimer.Delay();
                    try
                    {
                        await databaseBackupClient.Start();
                    }
                    catch (Exception exception)
                    {
                        ConsoleWriteQueue.WriteLine(exception.Message, ConsoleColor.Red);
                        await AutoCSer.LogHelper.Exception(exception);
                    }
                }
                while (true);
            }
        }
    }
}
