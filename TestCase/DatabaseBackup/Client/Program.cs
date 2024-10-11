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
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                MinCompressSize = 1024, Host = new HostEndPoint(configFile.ServerPort, configFile.ServerHost),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CommandClientSocket commandClientSocket = await commandClient.GetSocketAsync();
                if (commandClientSocket == null) ConsoleWriteQueue.WriteLine("数据库备份服务连接失败", ConsoleColor.Red);
                else Console.WriteLine("数据库备份服务连接成功");
                CommandClientSocketEvent socketEvent = (CommandClientSocketEvent)commandClient.SocketEvent;
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
