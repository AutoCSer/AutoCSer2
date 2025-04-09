using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 代码生成器配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class GeneratorAttribute : Attribute
    {
#pragma warning disable
        /// <summary>
        /// 自动安装依赖,指定当前安装必须后于依赖安装
        /// </summary>
        public Type DependType;
#pragma warning restore
        /// <summary>
        /// 生成器名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 是否自动生成
        /// </summary>
        public bool IsAuto;
        /// <summary>
        /// 是否生成模板代码
        /// </summary>
        public bool IsTemplate = true;
#if !DotNet45
        /// <summary>
        /// 是否检查 .NET Framework 4.5 项目环境（忽略该框架）
        /// </summary>
        public bool CheckDotNet45 = false;
#endif
        /// <summary>
        /// 是否 C# AOT 代码
        /// </summary>
        public bool IsAOT;
        /// <summary>
        /// 代码生成语言
        /// </summary>
        public CodeLanguageEnum Language = CodeLanguageEnum.CSharp;
        /// <summary>
        /// 代码生成语言编号
        /// </summary>
        internal CodeLanguageEnum CodeLanguage
        {
            get { return IsAOT && Language == CodeLanguageEnum.CSharp ? CodeLanguageEnum.COUNT : Language; }
        }
        /// <summary>
        /// 获取模板文件名，不包括扩展名
        /// </summary>
        /// <param name="type">模板数据视图</param>
        /// <returns>模板文件名</returns>
        public string GetFileName(Type type)
        {
            return type.Name;
        }
    }
}
