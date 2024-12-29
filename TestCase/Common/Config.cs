using System;
using System.IO;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// AutoCSer 项目配置
    /// </summary>
    public abstract class Config : AutoCSer.Configuration.Root
    {
        ///// <summary>
        ///// 公共配置类型集合
        ///// </summary>
        //public override IEnumerable<Type> PublicTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// 服务认证验证字符串
        /// </summary>
        public const string TimestampVerifyString = nameof(TimestampVerifyString);

        /// <summary>
        /// 项目文件路径
        /// </summary>
        public static readonly string AutoCSerPath;
        /// <summary>
        /// 临时文件路径
        /// </summary>
        public static readonly string AutoCSerTemporaryFilePath;
        static Config()
        {
            DirectoryInfo directory = AutoCSer.Common.ApplicationDirectory, parentDirectory = directory.Parent;
            while (parentDirectory != null && parentDirectory.Name != "AutoCSer2") parentDirectory = parentDirectory.Parent;
            AutoCSerPath = parentDirectory?.FullName;
            AutoCSerTemporaryFilePath = Path.Combine(parentDirectory?.Parent.FullName ?? directory.FullName, nameof(AutoCSerTemporaryFilePath));
        }
    }
}
