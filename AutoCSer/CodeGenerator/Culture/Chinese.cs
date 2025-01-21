using AutoCSer.Extensions;
using System;
using System.IO;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Culture
{
    /// <summary>
    /// 扩展中文配置
    /// </summary>
    public class Chinese : Configuration
    {
        /// <summary>
        /// RPC 方法名称冲突
        /// </summary>
        /// <param name="type">服务端接口类型</param>
        /// <param name="enumType">方法序号映射枚举类型</param>
        /// <param name="enumName">枚举名称</param>
        /// <returns></returns>
        public override string GetCommandServerMethodNameConflict(Type type, Type enumType, string enumName)
        {
            return $"{type} 生成 {enumType.fullName()}.{enumName} 名称冲突";
        }
        /// <summary>
        /// 内存数据库节点方法名称冲突
        /// </summary>
        /// <param name="type">服务端节点接口类型</param>
        /// <param name="enumTypeName">方法序号映射枚举类型名称</param>
        /// <param name="enumName">枚举名称</param>
        /// <returns></returns>
        public override string GetStreamPersistenceMemoryDatabaseNodeMethodNameConflict(Type type, string enumTypeName, string enumName)
        {
            return $"{type} 生成 {enumTypeName}.{enumName} 名称冲突";
        }
        /// <summary>
        /// 模板节点缺少回合节点
        /// </summary>
        public override string GetTreeTemplateNotFoundRoundNode { get { return "缺少回合节点"; } }
        /// <summary>
        /// 未知的模板回合节点
        /// </summary>
        public override string GetTreeTemplateUnknownRoundNode { get { return "未知的回合节点"; } }
        /// <summary>
        /// 不可识别的 RPC API
        /// </summary>
        /// <param name="method">API 方法信息</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public override string GetCommandServerUnrecognizedMethod(MethodInfo method, string error)
        {
            return $"{method.DeclaringType.fullName()}.{method.Name} 是不可识别的 API {error}";
        }
        /// <summary>
        /// WEB 数据视图没有找到对应的 HTML 模板页面文件
        /// </summary>
        /// <param name="viewType">WEB 数据视图类型</param>
        /// <param name="templateFile">HTML 模板页面文件信息</param>
        /// <returns></returns>
        public override string GetWebViewNotFoundTemplateFile(Type viewType, FileInfo templateFile)
        {
            return $"数据视图 {viewType.fullName()} 没有找到对应的 HTML 模板页面文件 {templateFile.FullName}";
        }
        /// <summary>
        /// WEB 数据视图 HTML 模板文件名称冲突
        /// </summary>
        /// <param name="viewType">WEB 数据视图类型</param>
        /// <param name="otherViewType">冲突的 WEB 数据视图类型</param>
        /// <param name="templateFile">HTML 模板页面文件信息</param>
        /// <returns></returns>
        public override string GetWebViewTemplateFileNameConflict(Type viewType, Type otherViewType, FileInfo templateFile)
        {
            return $"数据视图 {viewType.fullName()} 与 {otherViewType.fullName()} HTML 模板文件名称冲突 {templateFile.FullName}";
        }

        /// <summary>
        /// 默认扩展中文配置
        /// </summary>
        public static readonly new Chinese Default = new Chinese();
    }
}
