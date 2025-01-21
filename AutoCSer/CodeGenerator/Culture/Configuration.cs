using AutoCSer.Extensions;
using System;
using System.IO;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Culture
{
    /// <summary>
    /// 扩展系统语言文化配置
    /// </summary>
    public abstract class Configuration
    {
        /// <summary>
        /// RPC 方法名称冲突
        /// </summary>
        /// <param name="type">服务端接口类型</param>
        /// <param name="enumType">方法序号映射枚举类型</param>
        /// <param name="enumName">枚举名称</param>
        /// <returns></returns>
        public abstract string GetCommandServerMethodNameConflict(Type type, Type enumType, string enumName);
        /// <summary>
        /// 内存数据库节点方法名称冲突
        /// </summary>
        /// <param name="type">服务端节点接口类型</param>
        /// <param name="enumTypeName">方法序号映射枚举类型名称</param>
        /// <param name="enumName">枚举名称</param>
        /// <returns></returns>
        public abstract string GetStreamPersistenceMemoryDatabaseNodeMethodNameConflict(Type type, string enumTypeName, string enumName);
        /// <summary>
        /// 模板节点缺少回合节点
        /// </summary>
        public abstract string GetTreeTemplateNotFoundRoundNode { get; }
        /// <summary>
        /// 未知的模板回合节点
        /// </summary>
        public abstract string GetTreeTemplateUnknownRoundNode { get; }
        /// <summary>
        /// 不可识别的 RPC API
        /// </summary>
        /// <param name="method">API 方法信息</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public abstract string GetCommandServerUnrecognizedMethod(MethodInfo method, string error);
        /// <summary>
        /// WEB 数据视图没有找到对应的 HTML 模板页面文件
        /// </summary>
        /// <param name="viewType">WEB 数据视图类型</param>
        /// <param name="templateFile">HTML 模板页面文件信息</param>
        /// <returns></returns>
        public abstract string GetWebViewNotFoundTemplateFile(Type viewType, FileInfo templateFile);
        /// <summary>
        /// WEB 数据视图 HTML 模板文件名称冲突
        /// </summary>
        /// <param name="viewType">WEB 数据视图类型</param>
        /// <param name="otherViewType">冲突的 WEB 数据视图类型</param>
        /// <param name="templateFile">HTML 模板页面文件信息</param>
        /// <returns></returns>
        public abstract string GetWebViewTemplateFileNameConflict(Type viewType, Type otherViewType, FileInfo templateFile);

        /// <summary>
        /// 默认扩展系统语言文化配置
        /// </summary>
        internal static readonly Configuration Default = AutoCSer.Configuration.Common.Get<Configuration>()?.Value ?? (AutoCSer.Culture.Configuration.IsChinese ? (Configuration)Chinese.Default : English.Default);
    }
}
