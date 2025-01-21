using AutoCSer.Extensions;
using System;

namespace AutoCSer.Extensions.Culture
{
    /// <summary>
    /// 扩展中文配置
    /// </summary>
    public class Chinese : Configuration
    {
        /// <summary>
        /// 日志流持久化文件数据解压缩失败
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <param name="position">错误数据位置</param>
        /// <returns></returns>
        public override string GetStreamPersistenceLoaderDecompressFailed(string fileName, long position)
        {
            return $"文件 {fileName} 位置 {position} 处数据解压缩失败";
        }
        /// <summary>
        /// 日志流持久化文件数据块长度错误
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <param name="position">错误数据位置</param>
        /// <returns></returns>
        public override string GetStreamPersistenceLoaderDataSizeError(string fileName, long position)
        {
            return $"文件 {fileName} 位置 {position} 处数据长度错误";
        }
        /// <summary>
        /// 日志流持久化文件头部数据不足
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <param name="unreadSize">文件未读取数据字节数量</param>
        /// <param name="fileHeadSize">需要读取的文件头部字节数量</param>
        /// <returns></returns>
        public override string GetStreamPersistenceLoaderHeaderSizeError(string fileName, int unreadSize, int fileHeadSize)
        {
            return $"文件 {fileName} 头部数据不足 {unreadSize.toString()} < {fileHeadSize.toString()}";
        }
        /// <summary>
        /// XML 节点类型不匹配
        /// </summary>
        /// <param name="type">XML 节点类型</param>
        /// <param name="matchType">XML 节点需要的匹配节点类型</param>
        /// <returns></returns>
        public override string GetXmlNodeTypeNotMatch(XmlNodeTypeEnum type, XmlNodeTypeEnum matchType)
        {
            return $"节点类型 {type} 不匹配 {matchType}";
        }

        /// <summary>
        /// 默认扩展中文配置
        /// </summary>
        public static readonly new Chinese Default = new Chinese();
    }
}
