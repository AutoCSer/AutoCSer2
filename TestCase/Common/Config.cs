using AutoCSer.Net;
using System;
using System.IO;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// AutoCSer project configuration
    /// AutoCSer 项目配置
    /// </summary>
    public abstract class Config : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// Command line parameter passing for the AOT test client
        /// AOT 测试客户端命令行传参
        /// </summary>
        public const string AotClientArgument = "AotClient";

        ///// <summary>
        ///// 公共配置类型集合
        ///// </summary>
        //public override IEnumerable<Type> PublicTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// Server authentication verification string
        /// 服务认证验证字符串
        /// </summary>
        public static string TimestampVerifyString { get { return JsonFileConfig.Default.TimestampVerifyString ?? nameof(TimestampVerifyString); } }
        /// <summary>
        /// Program file upload directory
        /// 程序文件上传目录
        /// </summary>
        public static string UploadPath { get { return JsonFileConfig.Default.UploadPath ?? AutoCSerTemporaryFilePath; } }

        /// <summary>
        /// AutoCSer project file path
        /// 项目文件路径
        /// </summary>
        public static readonly string AutoCSerPath;
        /// <summary>
        /// Test the temporary file path
        /// 测试临时文件路径
        /// </summary>
        public static readonly string AutoCSerTemporaryFilePath;

        static Config()
        {
            DirectoryInfo directory = AutoCSer.Common.ApplicationDirectory, parentDirectory = directory.Parent;
            while (parentDirectory != null && parentDirectory.Name != "AutoCSer2") parentDirectory = parentDirectory.Parent;
            AutoCSerPath = parentDirectory?.FullName;

            if (System.IO.Path.DirectorySeparatorChar != '/')
            {
                string defaultPath = @"d:\" + nameof(AutoCSerTemporaryFilePath);
                if (Directory.Exists(defaultPath)) AutoCSerTemporaryFilePath = defaultPath;
                else AutoCSerTemporaryFilePath = Path.Combine(parentDirectory?.Parent.FullName ?? directory.FullName, nameof(AutoCSerTemporaryFilePath));
            }
            else
            {
                DirectoryInfo temporaryFilePath = new DirectoryInfo("/var/" + nameof(AutoCSerTemporaryFilePath));
                if (!temporaryFilePath.Exists) temporaryFilePath.Create();
                AutoCSerTemporaryFilePath = temporaryFilePath.FullName;
            }
        }
    }
}
