using AutoCSer.Extensions;
using System;
using System.Diagnostics;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture
{
    /// <summary>
    /// 扩展中文配置
    /// </summary>
    public class Chinese : Configuration
    {
        /// <summary>
        /// 服务端节点创建失败
        /// </summary>
        /// <param name="type">服务端节点接口类型</param>
        /// <returns></returns>
        public override string GetServerNodeCreateFailed(Type type)
        {
            return $"节点 {type.fullName()} 创建失败。";
        }
        /// <summary>
        /// 服务端节点类型未实现快照接口
        /// </summary>
        /// <param name="type">服务端节点类型</param>
        /// <returns></returns>
        public override string GetServerSnapshotNodeNotImplemented(Type type)
        {
            return $"服务端节点类型 {type.fullName()} 未实现快照接口 {typeof(ISnapshot<>).fullName()} / {typeof(IEnumerableSnapshot<>).fullName()}";
        }
        /// <summary>
        /// 内存数据库初始化加载数据失败
        /// </summary>
        /// <param name="state">错误状态</param>
        /// <param name="fileName">加载的持久化文件名称</param>
        /// <param name="position">当前数据块所在持久化流中的位置</param>
        /// <param name="bufferIndex">当前数据块内索引位置</param>
        /// <returns></returns>
        public override string GetServiceLoaderFailed(CallStateEnum state, string fileName, long position, int bufferIndex)
        {
            return $"文件 {fileName} 位置 {position}+{bufferIndex} 处数据错误 {state}";
        }
        /// <summary>
        /// 内存数据库初始化加载数据失败
        /// </summary>
        /// <param name="fileName">加载的持久化文件名称</param>
        /// <param name="position">当前数据块所在持久化流中的位置</param>
        /// <param name="bufferIndex">当前数据块内索引位置</param>
        /// <returns></returns>
        public override string GetServiceLoaderFailed(string fileName, long position, int bufferIndex)
        {
            return $"文件 {fileName} 位置 {position}+{bufferIndex} 处数据错误";
        }
        /// <summary>
        /// 持久化回调异常位置文件长度不可识别
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件名称</param>
        /// <param name="unreadSize">未读数据字节数量</param>
        /// <returns></returns>
        public override string GetServiceLoaderExceptionPositionFileSizeUnrecognized(string fileName, long unreadSize)
        {
            return $"持久化回调异常位置文件 {fileName} 未读数据长度 {unreadSize} 不可识别";
        }
        /// <summary>
        /// 持久化回调异常位置文件重建索引位置与数据库文件位置不匹配
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件名称</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="databaseRebuildPosition">内存数据库持久化流重建起始位置</param>
        /// <returns></returns>
        public override string GetServiceLoaderExceptionPositionRebuildPositionNotMatch(string fileName, ulong rebuildPosition, ulong databaseRebuildPosition)
        {
            return $"持久化回调异常位置文件 {fileName} 重建索引位置 {rebuildPosition} 与数据库文件位置 {databaseRebuildPosition} 不匹配";
        }
        /// <summary>
        /// 持久化回调异常位置文件头部识别失败
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件名称</param>
        /// <returns></returns>
        public override string GetServiceLoaderExceptionPositionFileHeaderNotMatch(string fileName)
        {
            return $"持久化回调异常位置文件 {fileName} 头部识别失败。";
        }
        /// <summary>
        /// 持久化回调异常位置文件头部数据不足
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件名称</param>
        /// <param name="unreadSize">文件未读数据字节数量</param>
        /// <param name="fileHeadSize">文件头部需求字节数量</param>
        /// <returns></returns>
        public override string GetServiceLoaderExceptionPositionFileHeaderSizeNotMatch(string fileName, int unreadSize, int fileHeadSize)
        {
            return $"持久化回调异常位置文件 {fileName} 头部数据不足 {unreadSize.toString()} < {fileHeadSize.toString()}";
        }
        /// <summary>
        /// 持久化文件头部识别失败
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <returns></returns>
        public override string GetServiceLoaderFileHeaderNotMatch(string fileName)
        {
            return $"文件 {fileName} 头部识别失败。";
        }
        /// <summary>
        /// 持久化文件头部版本号不被支持
        /// </summary>
        /// <param name="fileName">持久化文件名称</param>
        /// <param name="verison">内存数据库版本号</param>
        /// <returns></returns>
        public override string GetServiceLoaderFileVersionNotSupported(string fileName, byte verison)
        {
            return $"文件 {fileName} 头部版本号不被支持 {verison.toString()}";
        }
        /// <summary>
        /// 持久化回调异常位置文件缺失
        /// </summary>
        /// <param name="fileName">持久化回调异常位置文件</param>
        /// <returns></returns>
        public override string GetNotFoundExceptionPositionFile(string fileName)
        {
            return $"持久化回调异常位置文件缺失 {fileName}，请确认文件组完整性。";
        }
        /// <summary>
        /// 内存数据库创建客户端节点异常信息
        /// </summary>
        /// <param name="clientType">客户端节点接口类型</param>
        /// <param name="serverType">服务端节点接口类型</param>
        /// <returns></returns>
#if NetStandard21
        public override string GetClientNodeCreatorException(Type clientType, Type? serverType)
#else
        public override string GetClientNodeCreatorException(Type clientType, Type serverType)
#endif
        {
            return $"{serverType?.fullName()} 客户端节点 {clientType.fullName()} 生成失败。";
        }
        /// <summary>
        /// 客户端节点没有找到匹配的服务端节点接口类型
        /// </summary>
        /// <param name="type">客户端节点接口类型</param>
        /// <returns></returns>
        public override string GetClientNodeCreatorNotMatchType(Type type)
        {
            return $"{type.fullName()} 的客户端节点没有找到匹配的服务端节点接口类型 {typeof(ClientNodeAttribute).fullName()}.{nameof(ClientNodeAttribute.ServerNodeType)}";
        }
        /// <summary>
        /// 节点客户端生成警告信息
        /// </summary>
        /// <param name="type">客户端节点接口类型</param>
        /// <param name="messages">节点构造提示信息</param>
        /// <returns></returns>
        public override string GetClientNodeCreatorWarning(Type type, string[] messages)
        {
            return $"{type.fullName()} 节点客户端生成警告信息\r\n{string.Join("\r\n", messages)}";
        }
        /// <summary>
        /// 守护的新进程启动失败
        /// </summary>
        /// <param name="process">退出的进程信息</param>
        /// <returns></returns>
        public override string GetGuardProcessStartFailed(ProcessGuardInfo process)
        {
            return $"守护的进程 {process.ProcessName} 退出后，新进程启动失败。";
        }

        /// <summary>
        /// 默认扩展中文配置
        /// </summary>
        public static readonly new Chinese Default = new Chinese();
    }
}
