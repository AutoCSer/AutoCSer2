using AutoCSer.Extensions;
using System;

namespace AutoCSer.Extensions.Culture
{
    /// <summary>
    /// 扩展系统语言文化配置
    /// </summary>
    public abstract class Configuration
    {
        /// <summary>
        /// 日志流持久化文件数据解码失败
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <param name="position">错误数据位置</param>
        /// <returns></returns>
        public abstract string GetStreamPersistenceLoaderDecodeFailed(string fileName, long position);
        /// <summary>
        /// 日志流持久化文件数据块长度错误
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <param name="position">错误数据位置</param>
        /// <returns></returns>
        public abstract string GetStreamPersistenceLoaderDataSizeError(string fileName, long position);
        /// <summary>
        /// 日志流持久化文件头部数据不足
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <param name="unreadSize">文件未读取数据字节数量</param>
        /// <param name="fileHeadSize">需要读取的文件头部字节数量</param>
        /// <returns></returns>
        public abstract string GetStreamPersistenceLoaderHeaderSizeError(string fileName, int unreadSize, int fileHeadSize);
        /// <summary>
        /// XML 节点类型不匹配
        /// </summary>
        /// <param name="type">XML 节点类型</param>
        /// <param name="matchType">XML 节点需要的匹配节点类型</param>
        /// <returns></returns>
        public abstract string GetXmlNodeTypeNotMatch(XmlNodeTypeEnum type, XmlNodeTypeEnum matchType);

        /// <summary>
        /// 默认扩展系统语言文化配置
        /// </summary>
        internal static readonly Configuration Default = AutoCSer.Configuration.Common.Get<Configuration>()?.Value ?? (AutoCSer.Culture.Configuration.IsChinese ? (Configuration)Chinese.Default : English.Default);
    }
}
