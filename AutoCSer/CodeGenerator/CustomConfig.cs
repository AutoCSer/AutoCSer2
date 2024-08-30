using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 自定义配置
    /// </summary>
    internal sealed class CustomConfig
    {
        /// <summary>
        /// 是否调用 AutoCSer 的代码生成组件
        /// </summary>
        internal bool IsAutoCSer = true;

        /// <summary>
        /// 自定义配置
        /// </summary>
        internal static readonly CustomConfig Default;
        /// <summary>
        /// 自定义代码生成程序集名称
        /// </summary>
        internal const string CustomAssemblyName = AutoCSer.Common.NamePrefix + ".CodeGenerator.Custom";
        /// <summary>
        /// 自定义配置文件
        /// </summary>
        private const string configFileName = AutoCSer.Common.NamePrefix + ".CodeGenerator.CustomConfig";
        /// <summary>
        /// 程序集
        /// </summary>
        internal static readonly Assembly Assembly;

        static CustomConfig()
        {
            FileInfo jsonFile = new FileInfo(configFileName + ".json");
            Default = jsonFile.Exists ? AutoCSer.JsonDeserializer.Deserialize<CustomConfig>(File.ReadAllText(jsonFile.FullName, Encoding.UTF8)) : new CustomConfig();
            Default = new CustomConfig();
            FileInfo assemblyFile = new FileInfo(Path.Combine(ProjectParameter.CurrentAssembly.Location, $"{CustomAssemblyName}.dll"));
            if (assemblyFile.Exists) Assembly = Assembly.LoadFrom(assemblyFile.FullName);
        }
    }
}
