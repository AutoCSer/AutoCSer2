using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库服务端配置
    /// </summary>
    public class StreamPersistenceMemoryDatabaseServiceConfig
    {
        /// <summary>
        /// 持久化数据库文件扩展名 AutoCSer Memory Database
        /// </summary>
        public const string PersistenceExtensionName = ".amd";
        /// <summary>
        /// 持久化回调异常位置文件扩展名 Callback Exception Position
        /// </summary>
        public const string PersistenceCallbackExceptionPositionExtensionName = ".cep";
        /// <summary>
        /// 默认持久化文件名称
        /// </summary>
        public const string DefaultPersistenceFileName = AutoCSer.Common.NamePrefix + "MemoryDatabase" + PersistenceExtensionName;

        /// <summary>
        /// 持久化文件路径，建议使用绝对路径（为了最优化冷启动读取速度，应该使用一个正常格式化的空白磁盘）
        /// </summary>
#if NetStandard21
        public string? PersistencePath;
#else
        public string PersistencePath;
#endif
        /// <summary>
        /// 重建持久化文件路径，建议使用绝对路径（为了最优化冷启动读取速度与文件重建速度，应该另外使用一个正常格式化的空白磁盘，也就是说需要两个正常格式化的空白磁盘切换）
        /// </summary>
#if NetStandard21
        public string? PersistenceSwitchPath;
#else
        public string PersistenceSwitchPath;
#endif
        /// <summary>
        /// 持久化文件名称
        /// </summary>
        public string PersistenceFileName = DefaultPersistenceFileName;
        /// <summary>
        /// 获取持久化文件信息
        /// </summary>
        /// <returns></returns>
        internal FileInfo GetPersistenceFileInfo()
        {
            FileInfo file = new FileInfo(string.IsNullOrEmpty(PersistencePath) ? PersistenceFileName : Path.Combine(PersistencePath, PersistenceFileName));
            if (string.Equals(file.Extension, PersistenceExtensionName, StringComparison.OrdinalIgnoreCase)) return file; 
            return new FileInfo(file.FullName + PersistenceExtensionName);
        }
        /// <summary>
        /// 获取重建持久化文件信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal FileInfo? GetPersistenceSwitchFileInfo()
#else
        internal FileInfo GetPersistenceSwitchFileInfo()
#endif
        {
            if (string.IsNullOrEmpty(PersistenceSwitchPath) || PersistencePath == PersistenceSwitchPath) return null;
            FileInfo file = new FileInfo(Path.Combine(PersistenceSwitchPath, PersistenceFileName));
            if (string.Equals(file.Extension, PersistenceExtensionName, StringComparison.OrdinalIgnoreCase)) return file;
            return new FileInfo(file.FullName + PersistenceExtensionName);
        }
        /// <summary>
        /// 修复节点方法保存目录名称，默认为 RepairNodeMethod
        /// </summary>
        public string RepairNodeMethodDirectoryName = "RepairNodeMethod";
        /// <summary>
        /// 修复节点方法程序集保存文件名称，默认为 assembly.dll
        /// </summary>
        public string RepairNodeMethodAssemblyFileName = "assembly.dll";
        /// <summary>
        /// 修复节点方法指定静态方法名称信息文件名称，默认为 method.json
        /// </summary>
        public string RepairNodeMethodNameFileName = "method.json";
        /// <summary>
        /// 持久化数据缓冲区最大字节数，默认为 1MB，最小值为 4KB
        /// </summary>
        public int BufferMaxSize = 1 << 20;
        /// <summary>
        /// 持久化数据缓存区池字节大小二进制位数量，默认为 17 表示 128KB，最小值为 12 表示 4KB
        /// </summary>
        public BufferSizeBitsEnum BufferSizeBits = BufferSizeBitsEnum.Kilobyte128;
        /// <summary>
        /// 默认为 false 表示不允许创建从节点，如果没有从节点需求设置为 true 会导致内存空间浪费
        /// </summary>
        public bool CanCreateSlave;
        /// <summary>
        /// 获取备份文件时间后缀
        /// </summary>
        /// <returns></returns>
        public virtual string GetBackupFileNameSuffix()
        {
            return AutoCSer.Threading.SecondTimer.Now.ToString(".yyyyMMddHHmmss");
        }
        /// <summary>
        /// 获取删除历史持久化文件 Utc 时间，默认为时间最大值表示不删除
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetRemoveHistoryFileTime()
        {
            return DateTime.MaxValue;
        }
        /// <summary>
        /// 启动删除历史持久化文件任务，默认不删除文件
        /// </summary>
        /// <param name="service"></param>
        public virtual void RemoveHistoryFile(StreamPersistenceMemoryDatabaseService service) { }
        /// <summary>
        /// 判断持久化文件是否需要重建（默认为超过 100MB 并且相对上次重建的快照版本增加一倍大小以后触发），要根据实际需求确定重建文件大小避免频繁触发重建操作
        /// </summary>
        /// <param name="service"></param>
        /// <returns>持久化文件是否需要重建</returns>
        public virtual bool CheckRebuild(StreamPersistenceMemoryDatabaseService service)
        {
            long persistencePosition = service.GetPersistencePosition();
            return (persistencePosition >> 1) >= service.RebuildSnapshotPosition && persistencePosition > 100 << 20;
        }
        /// <summary>
        /// 持久化数据编码
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="startIndex">原始数据起始位置</param>
        /// <param name="dataSize">原始数据字节数</param>
        /// <param name="buffer">输出数据缓冲区</param>
        /// <param name="outputData">输出数据</param>
        /// <param name="outputSeek">输出数据起始位置</param>
        /// <param name="outputHeadSize">输出数据多余头部大小</param>
        /// <returns>持久化数据是否编码</returns>
        public virtual bool PersistenceEncode(byte[] data, int startIndex, int dataSize, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return dataSize >= 4 << 10 && AutoCSer.Common.Config.Compress(data, startIndex, dataSize, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel.Fastest);
        }
        /// <summary>
        /// 持久化数据解码
        /// </summary>
        /// <param name="transferData">编码后的数据</param>
        /// <param name="outputData">等待写入的原始数据缓冲区</param>
        /// <returns>是否解码成功</returns>
        public virtual bool PersistenceDecode(ref SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            return AutoCSer.Common.Config.Decompress(ref transferData, ref outputData);
        }

        /// <summary>
        /// 创建默认日志流持久化内存数据库服务端（主服务节点）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StreamPersistenceMemoryDatabaseService Create()
        {
            return Create<IServiceNode>(service => new ServiceNode(service));
        }
        /// <summary>
        /// 日志流持久化内存数据库服务端（主服务节点）
        /// </summary>
        /// <typeparam name="T">节点服务接口类型</typeparam>
        /// <param name="createServiceNode"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StreamPersistenceMemoryDatabaseService Create<T>(Func<StreamPersistenceMemoryDatabaseService, T> createServiceNode)
            where T : class, IServiceNode
        {
            return new StreamPersistenceMemoryDatabaseService(this, service => ServiceNode.CreateServiceNode(service, createServiceNode(service)), true);
        }
    }
}
