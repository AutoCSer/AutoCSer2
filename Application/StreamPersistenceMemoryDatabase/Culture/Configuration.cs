using System;
using System.Diagnostics;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture
{
    /// <summary>
    /// 扩展系统语言文化配置
    /// </summary>
    public abstract class Configuration
    {
        /// <summary>
        /// 服务端节点创建失败
        /// </summary>
        /// <param name="type">服务端节点接口类型</param>
        /// <returns></returns>
        public abstract string GetServerNodeCreateFailed(Type type);
        /// <summary>
        /// 服务端节点类型未实现快照接口
        /// </summary>
        /// <param name="type">服务端节点类型</param>
        /// <returns></returns>
        public abstract string GetServerSnapshotNodeNotImplemented(Type type);
        /// <summary>
        /// 内存数据库初始化加载数据失败
        /// </summary>
        /// <param name="state">错误状态</param>
        /// <param name="fileName">加载的持久化文件名称</param>
        /// <param name="position">当前数据块所在持久化流中的位置</param>
        /// <param name="bufferIndex">当前数据块内索引位置</param>
        /// <returns></returns>
        public abstract string GetServiceLoaderFailed(CallStateEnum state, string fileName, long position, int bufferIndex);
        /// <summary>
        /// 内存数据库初始化加载数据失败
        /// </summary>
        /// <param name="fileName">加载的持久化文件名称</param>
        /// <param name="position">当前数据块所在持久化流中的位置</param>
        /// <param name="bufferIndex">当前数据块内索引位置</param>
        /// <returns></returns>
        public abstract string GetServiceLoaderFailed(string fileName, long position, int bufferIndex);
        /// <summary>
        /// 持久化回调异常位置文件长度不可识别
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件名称</param>
        /// <param name="unreadSize">未读数据字节数量</param>
        /// <returns></returns>
        public abstract string GetServiceLoaderExceptionPositionFileSizeUnrecognized(string fileName, long unreadSize);
        /// <summary>
        /// 持久化回调异常位置文件重建索引位置与数据库文件位置不匹配
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件名称</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="databaseRebuildPosition">内存数据库持久化流重建起始位置</param>
        /// <returns></returns>
        public abstract string GetServiceLoaderExceptionPositionRebuildPositionNotMatch(string fileName, ulong rebuildPosition, ulong databaseRebuildPosition);
        /// <summary>
        /// 持久化回调异常位置文件头部识别失败
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件名称</param>
        /// <returns></returns>
        public abstract string GetServiceLoaderExceptionPositionFileHeaderNotMatch(string fileName);
        /// <summary>
        /// 持久化回调异常位置文件头部数据不足
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件名称</param>
        /// <param name="unreadSize">文件未读数据字节数量</param>
        /// <param name="fileHeadSize">文件头部需求字节数量</param>
        /// <returns></returns>
        public abstract string GetServiceLoaderExceptionPositionFileHeaderSizeNotMatch(string fileName, int unreadSize, int fileHeadSize);
        /// <summary>
        /// 持久化文件头部识别失败
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <returns></returns>
        public abstract string GetServiceLoaderFileHeaderNotMatch(string fileName);
        /// <summary>
        /// 持久化文件头部版本号不被支持
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <param name="verison">内存数据库版本号</param>
        /// <returns></returns>
        public abstract string GetServiceLoaderFileVersionNotSupported(string fileName, byte verison);
        /// <summary>
        /// 持久化文件缺失
        /// </summary>
        /// <param name="fileName">持久化文件</param>
        /// <returns></returns>
        public abstract string GetNotFoundPersistenceFile(string fileName);
        /// <summary>
        /// 持久化回调异常位置文件缺失
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件</param>
        /// <returns></returns>
        public abstract string GetNotFoundExceptionPositionFile(string fileName);
        /// <summary>
        /// 内存数据库创建客户端节点异常信息
        /// </summary>
        /// <param name="clientType">客户端节点接口类型</param>
        /// <param name="serverType">服务端节点接口类型</param>
        /// <returns></returns>
#if NetStandard21
        public abstract string GetClientNodeCreatorException(Type clientType, Type? serverType);
#else
        public abstract string GetClientNodeCreatorException(Type clientType, Type serverType);
#endif
        /// <summary>
        /// 客户端节点没有找到匹配的服务端节点接口类型
        /// </summary>
        /// <param name="type">客户端节点接口类型</param>
        /// <returns></returns>
        public abstract string GetClientNodeCreatorNotMatchType(Type type);
        /// <summary>
        /// 节点客户端生成警告信息
        /// </summary>
        /// <param name="type">客户端节点接口类型</param>
        /// <param name="messages">节点构造提示信息</param>
        /// <returns></returns>
        public abstract string GetClientNodeCreatorWarning(Type type, string[] messages);
#if !AOT
        /// <summary>
        /// 守护的新进程启动失败
        /// </summary>
        /// <param name="process">退出的进程信息</param>
        /// <returns></returns>
        public abstract string GetGuardProcessStartFailed(ProcessGuardInfo process);
        /// <summary>
        /// 服务启动失败
        /// </summary>
        /// <param name="listener">命令服务端监听</param>
        /// <returns></returns>
        public abstract string GetCommandListenerStartFailed(AutoCSer.Net.CommandListener listener);
#endif
        /// <summary>
        /// 默认扩展系统语言文化配置
        /// </summary>
        internal static readonly Configuration Default = AutoCSer.Configuration.Common.Get<Configuration>()?.Value ?? (AutoCSer.Culture.Configuration.IsChinese ? (Configuration)Chinese.Default : English.Default);
    }
}
