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
    /// Log stream persistence in memory database service configuration
    /// 日志流持久化内存数据库服务配置
    /// </summary>
    public class StreamPersistenceMemoryDatabaseServiceConfig
    {
        /// <summary>
        /// Persistent Database file extension (AutoCSer Memory Database)
        /// 持久化数据库文件扩展名（AutoCSer Memory Database）
        /// </summary>
        public const string PersistenceExtensionName = ".amd";
        /// <summary>
        /// Persist the callback exception location file extension (Callback Exception Position)
        /// 持久化回调异常位置文件扩展名（Callback Exception Position）
        /// </summary>
        public const string PersistenceCallbackExceptionPositionExtensionName = ".cep";
        /// <summary>
        /// Default persistent file name
        /// 默认持久化文件名称
        /// </summary>
        public const string DefaultPersistenceFileName = AutoCSer.Common.NamePrefix + "MemoryDatabase" + PersistenceExtensionName;

        /// <summary>
        /// Persist the file path. It is recommended to use an absolute path (to optimize the cold start read speed, a normally formatted blank disk should be used)
        /// 持久化文件路径，建议使用绝对路径（为了最优化冷启动读取速度，应该使用一个正常格式化的空白磁盘）
        /// </summary>
#if NetStandard21
        public string? PersistencePath;
#else
        public string PersistencePath;
#endif
        /// <summary>
        /// When rebuilding the path of a persistent file, it is recommended to use an absolute path. (To optimize the cold start reading speed and file rebuild speed, a normally formatted blank disk should be used separately, that is, two normally formatted blank disks need to be switched.)
        /// 重建持久化文件路径，建议使用绝对路径（为了最优化冷启动读取速度与文件重建速度，应该另外使用一个正常格式化的空白磁盘，也就是说需要两个正常格式化的空白磁盘切换）
        /// </summary>
#if NetStandard21
        public string? PersistenceSwitchPath;
#else
        public string PersistenceSwitchPath;
#endif
        /// <summary>
        /// The name of the persistent file
        /// 持久化文件名称
        /// </summary>
        public string PersistenceFileName = DefaultPersistenceFileName;
        /// <summary>
        /// Get persistent file information
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
        /// Get persistence rebuild file information
        /// 获取持久化重建文件信息
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
        /// The repair node method saves the directory name, with the default being RepairNodeMethod
        /// 修复节点方法保存目录名称，默认为 RepairNodeMethod
        /// </summary>
        public string RepairNodeMethodDirectoryName = "RepairNodeMethod";
        /// <summary>
        /// Fix the node method assembly save the file name, which is defaulted to assembly.dll
        /// 修复节点方法程序集保存文件名称，默认为 assembly.dll
        /// </summary>
        public string RepairNodeMethodAssemblyFileName = "assembly.dll";
        /// <summary>
        /// The repair node method specifies the file name of the static method name information, with the default being method.json
        /// 修复节点方法指定静态方法名称信息文件名称，默认为 method.json
        /// </summary>
        public string RepairNodeMethodNameFileName = "method.json";
        /// <summary>
        /// The maximum number of bytes in the persistent data buffer is 1MB by default and 4KB by minimum
        /// 持久化数据缓冲区最大字节数，默认为 1MB，最小值为 4KB
        /// </summary>
        public int BufferMaxSize = 1 << 20;
        /// <summary>
        /// The byte size of the persistent data cache pool pool is the number of binary bits. By default, 17 represents 128KB, and the minimum value is 12 represents 4KB
        /// 持久化数据缓存区池字节大小二进制位数量，默认为 17 表示 128KB，最小值为 12 表示 4KB
        /// </summary>
        public BufferSizeBitsEnum BufferSizeBits = BufferSizeBitsEnum.Kilobyte128;
        /// <summary>
        /// Persistent type
        /// 持久化类型
        /// </summary>
        public PersistenceTypeEnum PersistenceType;
#if !AOT
        /// <summary>
        /// By default, false indicates that the creation of slave nodes is not allowed. If no slave node is required and set to true, it will lead to the waste of memory space
        /// 默认为 false 表示不允许创建从节点，如果没有从节点需求设置为 true 会导致内存空间浪费
        /// </summary>
        public bool CanCreateSlave;
#endif
        /// <summary>
        /// By default, true indicates that string serialization directly copies memory data. Setting it to false can reduce space occupation by encoding ASCII
        /// 默认为 true 表示字符串序列化直接复制内存数据，设置为 false 则对 ASCII 进行编码可以降低空间占用
        /// </summary>
        public bool IsSerializeCopyString = true;
        /// <summary>
        /// Get the backup file time suffix
        /// 获取备份文件时间后缀
        /// </summary>
        /// <returns></returns>
        public virtual string GetBackupFileNameSuffix()
        {
            return AutoCSer.Threading.SecondTimer.Now.ToString(".yyyyMMddHHmmss");
        }
        /// <summary>
        /// Obtain the Utc time for deleting persistent files. By default, the maximum time indicates that the files are not deleted
        /// 获取删除历史持久化文件 Utc 时间，默认为时间最大值表示不删除
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetRemoveHistoryFileTime()
        {
            return DateTime.MaxValue;
        }
        /// <summary>
        /// The task of deleting persistent files is started. By default, files are not deleted
        /// 启动删除历史持久化文件任务，默认不删除文件
        /// </summary>
        /// <param name="service"></param>
        public virtual void RemoveHistoryFile(StreamPersistenceMemoryDatabaseService service) { }
        /// <summary>
        /// Determine whether persistent files need to be rebuilt (the default value is more than 100MB and the size of the snapshot version is doubled). Determine the size of the rebuild file based on actual requirements to avoid frequent rebuild operations
        /// 判断持久化文件是否需要重建（默认为超过 100MB 并且相对上次重建的快照版本增加一倍大小以后触发），要根据实际需求确定重建文件大小避免频繁触发重建操作
        /// </summary>
        /// <param name="service"></param>
        /// <returns>Whether the persistent file needs to be rebuilt
        /// 持久化文件是否需要重建</returns>
        public virtual bool CheckRebuild(StreamPersistenceMemoryDatabaseService service)
        {
            long persistencePosition = service.GetPersistencePosition();
            return (persistencePosition >> 1) >= service.RebuildSnapshotPosition && persistencePosition > 100 << 20;
        }
        /// <summary>
        /// Persistent data encoding
        /// 持久化数据编码
        /// </summary>
        /// <param name="data">Original data
        /// 原始数据</param>
        /// <param name="startIndex">The starting position of the original data
        /// 原始数据起始位置</param>
        /// <param name="dataSize">The number of original data bytes
        /// 原始数据字节数</param>
        /// <param name="buffer">Output data buffer
        /// 输出数据缓冲区</param>
        /// <param name="outputData">Output data
        /// 输出数据</param>
        /// <param name="outputSeek">Start position of output data
        /// 输出数据起始位置</param>
        /// <param name="outputHeadSize">The output data exceeds the header size
        /// 输出数据多余头部大小</param>
        /// <returns>Whether the persistent data is encoded
        /// 持久化数据是否编码</returns>
        public virtual bool PersistenceEncode(byte[] data, int startIndex, int dataSize, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return dataSize >= 4 << 10 && AutoCSer.Common.Config.Compress(data, startIndex, dataSize, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel.Fastest);
        }
        /// <summary>
        /// Persistent data decoding
        /// 持久化数据解码
        /// </summary>
        /// <param name="transferData">The encoded data
        /// 编码后的数据</param>
        /// <param name="outputData">Original data buffer waiting to be written
        /// 等待写入的原始数据缓冲区</param>
        /// <returns>Whether the decoding was successful
        /// 是否解码成功</returns>
        public virtual bool PersistenceDecode(ref SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            return AutoCSer.Common.Config.Decompress(ref transferData, ref outputData);
        }

        /// <summary>
        /// Create a default log stream to persist the in-memory database server (primary service node)
        /// 创建默认日志流持久化内存数据库服务（主服务节点）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StreamPersistenceMemoryDatabaseService Create()
        {
            return Create<IServiceNode>(service => new ServiceNode(service));
        }
        /// <summary>
        /// Create a log stream persistence in-memory database server for custom basic services (primary service node)
        /// 创建自定义基础服务的日志流持久化内存数据库服务（主服务节点）
        /// </summary>
        /// <typeparam name="T">Customize the basic service interface type
        /// 自定义基础服务接口类型</typeparam>
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
