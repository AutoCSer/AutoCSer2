using AutoCSer.Threading;
using System;
using System.IO;
using System.Text;

namespace AutoCSer.TestCase.DatabaseBackupClient
{
    /// <summary>
    /// 配置文件
    /// </summary>
    internal sealed class ConfigFile
    {
#pragma warning disable
        /// <summary>
        /// 备份时间
        /// </summary>
        public DateTime BackupTime;
        /// <summary>
        /// 获取备份定时
        /// </summary>
        /// <returns></returns>
        internal TaskRunTimer GetTaskRunTimer()
        {
            return new TaskRunTimer(BackupTime.Hour, BackupTime.Minute, BackupTime.Second);
        }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerHost;
        /// <summary>
        /// 服务监听端口
        /// </summary>
        public ushort ServerPort;
        /// <summary>
        /// 验证字符串
        /// </summary>
        public string VerifyString;
        /// <summary>
        /// 数据库备份文件目录
        /// </summary>
        public string BackupPath;
#pragma warning restore
        /// <summary>
        /// 检查配置
        /// </summary>
        private void Check()
        {
            if (string.IsNullOrEmpty(ServerHost))
            {
                ConsoleWriteQueue.WriteLine("没有找到配置 服务器地址", ConsoleColor.Red);
                return;
            }
            if (string.IsNullOrEmpty(VerifyString))
            {
                ConsoleWriteQueue.WriteLine("没有找到配置 验证字符串", ConsoleColor.Red);
                return;
            }
            if (string.IsNullOrEmpty(BackupPath))
            {
                ConsoleWriteQueue.WriteLine("没有找到配置 数据库备份文件目录", ConsoleColor.Red);
                return;
            }
            if (ServerPort == 0) ServerPort = 3005;
            if (!Directory.Exists(BackupPath)) Directory.CreateDirectory(BackupPath);
        }

        /// <summary>
        /// 默认配置文件
        /// </summary>
        public static readonly ConfigFile Default;
        static ConfigFile()
        {
            FileInfo configFile = new FileInfo("AutoCSer.TestCase.DatabaseBackupClient.Config.json");
            if (configFile.Exists)
            {
                Default = AutoCSer.JsonDeserializer.Deserialize<ConfigFile>(File.ReadAllText(configFile.FullName, Encoding.UTF8));
                if (Default == null)
                {
                    ConsoleWriteQueue.WriteLine($"配置文件解析失败 {configFile.FullName}", ConsoleColor.Red);
                    return;
                }
                Default.Check();
            }
            else ConsoleWriteQueue.WriteLine($"没有找到配置文件 {configFile.FullName}", ConsoleColor.Red);
        }
    }
}
