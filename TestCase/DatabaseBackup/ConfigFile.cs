using System;
using System.IO;
using System.Text;

namespace AutoCSer.TestCase.DatabaseBackup
{
    /// <summary>
    /// 配置文件
    /// </summary>
    internal sealed class ConfigFile
    {
#pragma warning disable
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
        /// <summary>
        /// 数据库信息
        /// </summary>
        public Database[] DatabaseArray;
        /// <summary>
        /// 备份命令超时分钟数
        /// </summary>
        public int CommandTimeoutMinutes;
        /// <summary>
        /// 删除文件小时数
        /// </summary>
        public int DeleteFileHours;
#pragma warning restore
        /// <summary>
        /// 检查配置
        /// </summary>
        private void Check()
        {
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
            if (DatabaseArray == null || DatabaseArray.Length == 0)
            {
                ConsoleWriteQueue.WriteLine("没有找到配置 数据库信息", ConsoleColor.Red);
                return;
            }
            if (ServerPort == 0) ServerPort = (ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.DatabaseBackup;
            if (CommandTimeoutMinutes <= 0) CommandTimeoutMinutes = 60;
            if (DeleteFileHours <= 0) DeleteFileHours = 12;
            if (!Directory.Exists(BackupPath)) Directory.CreateDirectory(BackupPath);
        }

        /// <summary>
        /// 默认配置文件
        /// </summary>
        internal static readonly ConfigFile Default;
        static ConfigFile()
        {
            FileInfo configFile = new FileInfo("AutoCSer.TestCase.DatabaseBackup.Config.json");
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
