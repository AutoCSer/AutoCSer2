using AutoCSer.Extensions;
using System;
using System.IO;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Culture
{
    /// <summary>
    /// Extended English configuration
    /// </summary>
    public class English : Configuration
    {
        /// <summary>
        /// RPC method name conflict
        /// </summary>
        /// <param name="type">Type of the server interface</param>
        /// <param name="enumType">Method ordinal mapping enumeration type</param>
        /// <param name="enumName">Enumeration name</param>
        /// <returns></returns>
        public override string GetCommandServerMethodNameConflict(Type type, Type enumType, string enumName)
        {
            return $"{type} generates name conflicts with {enumType.fullName()}.{enumName}";
        }
        /// <summary>
        /// The memory database node method name conflicts
        /// </summary>
        /// <param name="type">Server node interface type</param>
        /// <param name="enumTypeName">Method number Mapping enumeration type name</param>
        /// <param name="enumName">Enumeration name</param>
        /// <returns></returns>
        public override string GetStreamPersistenceMemoryDatabaseNodeMethodNameConflict(Type type, string enumTypeName, string enumName)
        {
            return $"{type} generates name conflicts with {enumTypeName}.{enumName}";
        }
        /// <summary>
        /// The template node is missing a turn node
        /// </summary>
        public override string GetTreeTemplateNotFoundRoundNode { get { return "The template node is missing a turn node."; } }
        /// <summary>
        /// 未知的模板回合节点
        /// </summary>
        public override string GetTreeTemplateUnknownRoundNode { get { return "Unknown template turn node."; } }
        /// <summary>
        /// Unrecognized RPC API
        /// </summary>
        /// <param name="method">method information</param>
        /// <param name="error">Error message</param>
        /// <returns></returns>
        public override string GetCommandServerUnrecognizedMethod(MethodInfo method, string error)
        {
            return $"{method.DeclaringType.fullName()}.{method.Name} is an unrecognized API {error}";
        }
        /// <summary>
        /// The WEB Data view did not find the corresponding HTML template page file
        /// </summary>
        /// <param name="viewType">WEB Data view type</param>
        /// <param name="templateFile">HTML template page file information</param>
        /// <returns></returns>
        public override string GetWebViewNotFoundTemplateFile(Type viewType, FileInfo templateFile)
        {
            return $"Data view {viewType.fullName()} does not find the corresponding HTML template page file {templateFile.FullName}";
        }
        /// <summary>
        /// WEB Data view HTML template file name conflict
        /// </summary>
        /// <param name="viewType">WEB Data view type</param>
        /// <param name="otherViewType">Conflicting WEB data view types</param>
        /// <param name="templateFile">HTML template page file information</param>
        /// <returns></returns>
        public override string GetWebViewTemplateFileNameConflict(Type viewType, Type otherViewType, FileInfo templateFile)
        {
            return $"Data view {viewType.fullName()} conflicts with {otherViewType.fullName()} HTML template file name {templateFile.FullName}";
        }

        /// <summary>
        /// Default Extended English configuration
        /// </summary>
        public static readonly new English Default = new English();
    }
}
