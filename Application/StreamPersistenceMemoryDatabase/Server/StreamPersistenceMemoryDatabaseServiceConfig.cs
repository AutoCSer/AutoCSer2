using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
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
        /// 套接字自定义会话对象操作
        /// </summary>
        public ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService> CommandServerSocketSessionObject;
        /// <summary>
        /// 持久化文件名称，建议使用绝对路径
        /// </summary>
        public string PersistenceFileName = DefaultPersistenceFileName;
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
        /// 持久化数据启用压缩最低字节数量，默认为 4KB
        /// </summary>
        public int MinCompressSize = 4 << 10;
        /// <summary>
        /// 压缩级别默认为快速压缩
        /// </summary>
        public CompressionLevel CompressionLevel = CompressionLevel.Fastest;
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
