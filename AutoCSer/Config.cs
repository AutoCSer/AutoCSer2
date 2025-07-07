using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Threading;
using AutoCSer.Memory;
using System.Text;
using System.IO.Compression;
using System.Linq;
using AutoCSer.Reflection;
using AutoCSer.Net;

namespace AutoCSer
{
    /// <summary>
    /// Public configuration parameters
    /// 公共配置参数
    /// </summary>
    public partial class Config
    {
        /// <summary>
        /// Global encoding
        /// 全局编码
        /// </summary>
        public Encoding Encoding = Encoding.UTF8;
        /// <summary>
        /// The binary bit length of the two-dimensional second-level timer array container is set to a minimum of 8 and a maximum of 12 by default
        /// 二维秒级定时器数组容器二进制位长度，默认为最小值 8，最大值为 12
        /// </summary>
        public byte TimeoutCapacityBitSize = 8;
        /// <summary>
        /// The default is false, indicating that the memory of statically type members is not scanned
        /// 默认为 false 表示不扫描静态类型成员内存
        /// </summary>
        public bool IsMemoryScanStaticType;
        /// <summary>
        /// The default time interval for temporary cache clearing is 3600 seconds
        /// 临时性缓存清理时间间隔秒数，默认为 3600 秒
        /// </summary>
        public int MemoryCacheClearSeconds = 60 * 60;
        /// <summary>
        /// The maximum binary number of the buffer in the byte array buffer pool is 17 by default for 128KB, the minimum value is 4 for 16B, and the maximum value is 30 for 1GB
        /// 字节数组缓冲区池最大缓冲区二进制位数，默认为 17 为 128KB，最小值为 4 为 16B，最大值为 30 为 1GB
        /// </summary>
        public BufferSizeBitsEnum MaxByteArrayPoolSizeBits = BufferSizeBitsEnum.Kilobyte128;
        /// <summary>
        /// A collection of basic legal remote types
        /// 基础合法远程类型集合
        /// </summary>
        private readonly HashSet<HashObject<Type>> remoteTypes;
        /// <summary>
        /// A collection of basic legal remote types
        /// 基础合法远程类型集合
        /// </summary>
        public IEnumerable<Type> RemoteTypes
        {
            get
            {
                foreach (HashObject<Type> type in remoteTypes) yield return type;
            }
        }
        /// <summary>
        /// A collection of assemblies for verifying legitimate remote type tags
        /// 验证合法远程类型标记的程序集集合
        /// </summary>
        private readonly HashSet<HashObject<Assembly>> remoteTypeAssemblys;
        /// <summary>
        /// A collection of assemblies for verifying legitimate remote type tags
        /// 验证合法远程类型标记的程序集集合
        /// </summary>
        public IEnumerable<Assembly> RemoteTypeAssemblys
        {
            get
            {
                foreach (HashObject<Assembly> assembly in remoteTypeAssemblys) yield return assembly;
            }
        }
        /// <summary>
        /// Serialization initialization of unmanaged memory pools, with a default of 4KB. For open servers, it is recommended to modify it to 256B or 1KB
        /// 序列化初始化非托管内存池，默认为 4KB，开放服务端建议修改为 256B 或者 1KB
        /// </summary>
        public virtual UnmanagedPool SerializeUnmanagedPool { get { return UnmanagedPool.Default; } }
        /// <summary>
        /// Public configuration parameters
        /// 公共配置参数
        /// </summary>
        public Config()
        {
            remoteTypeAssemblys = HashSetCreator.CreateHashObject<Assembly>();
            remoteTypeAssemblys.Add(typeof(Config).Assembly);

            remoteTypes = HashSetCreator.CreateHashObject<Type>();
            remoteTypes.Add(typeof(bool));
            remoteTypes.Add(typeof(sbyte));
            remoteTypes.Add(typeof(byte));
            remoteTypes.Add(typeof(short));
            remoteTypes.Add(typeof(ushort));
            remoteTypes.Add(typeof(int));
            remoteTypes.Add(typeof(uint));
            remoteTypes.Add(typeof(long));
            remoteTypes.Add(typeof(ulong));
            remoteTypes.Add(typeof(float));
            remoteTypes.Add(typeof(double));
            remoteTypes.Add(typeof(decimal));
            remoteTypes.Add(typeof(char));
            remoteTypes.Add(typeof(DateTime));
            remoteTypes.Add(typeof(TimeSpan));
            remoteTypes.Add(typeof(Guid));
            remoteTypes.Add(typeof(SubString));
            remoteTypes.Add(typeof(string));
            remoteTypes.Add(typeof(RemoteType));
            remoteTypes.Add(typeof(JsonNode));
            remoteTypes.Add(typeof(HostEndPoint));

            remoteTypes.Add(typeof(Nullable<>));
            remoteTypes.Add(typeof(LeftArray<>));
            remoteTypes.Add(typeof(HeadLeftArray<>));
            remoteTypes.Add(typeof(ListArray<>));
            remoteTypes.Add(typeof(SubArray<>));
            remoteTypes.Add(typeof(List<>));
            remoteTypes.Add(typeof(HashSet<>));
            remoteTypes.Add(typeof(Queue<>));
            remoteTypes.Add(typeof(Stack<>));
            remoteTypes.Add(typeof(SortedSet<>));
            remoteTypes.Add(typeof(HashObject<>));
            remoteTypes.Add(typeof(ReferenceHashKey<>));
        }
        /// <summary>
        /// Get the number of seconds between temporary cache cleanup times
        /// 获取临时性缓存清理时间间隔秒数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetMemoryCacheClearSeconds()
        {
            return MemoryCacheClearSeconds > 0 ? MemoryCacheClearSeconds : 60;
        }
        /// <summary>
        /// Log file operation exception handling (because logs cannot be written), note that the application layer should not call this log to record this exception, nor should the logs of two exceptions be called mutually to avoid an infinite loop
        /// 日志文件操作异常处理（因为没法写日志），注意应用层也不要调用该日志记录这个异常，也不要让两个异常的日志相互调用避免死循环
        /// </summary>
        /// <param name="log">Log processing interface
        /// 日志处理接口</param>
        /// <param name="exception"></param>
        public virtual Task OnLogFileException(ILog log, Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Gets the default linked list cache pool parameters
        /// 获取默认链表缓存池参数
        /// </summary>
        /// <param name="type">Cache data type
        /// 缓存数据类型</param>
        /// <returns>Linked list cache pool parameters
        /// 链表缓存池参数</returns>
        public virtual LinkPoolParameter GetLinkPoolParameter(Type type)
        {
            return LinkPoolParameter.Default;
        }
        ///// <summary>
        ///// 默认分块缓存池构造函数传参参数
        ///// </summary>
        ///// <param name="type">缓存数据类型</param>
        ///// <returns>分块缓存池构造函数传参参数</returns>
        //public virtual BlockPoolParameter GetBlockPoolParameter(Type type)
        //{
        //    return BlockPoolParameter.Default;
        //}
        ///// <summary>
        ///// 获取缓存环池参数重定向类型
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns>重定向类型</returns>
        //public virtual Type GetRingPoolType(Type type)
        //{
        //    if (type.IsGenericType)
        //    {
        //        Type genericType = type.GetGenericTypeDefinition();
        //        if (genericType == typeof(AutoCSer.Net.CommandServer.ServerOutput<>)
        //            || genericType == typeof(AutoCSer.Net.CommandServer.ServerReturnValue<>)
        //            || genericType == typeof(AutoCSer.Net.CommandServer.SynchronousOutputCommand<,>)
        //            || genericType == typeof(AutoCSer.Net.CommandServer.SynchronousCommand<>)
        //            || genericType == typeof(AutoCSer.Net.CommandServer.SynchronousOutputCommand<>)
        //            )
        //        {
        //            type = type.GetGenericArguments()[0];
        //        }
        //    }
        //    return type;
        //}
        ///// <summary>
        ///// 默认缓存环池参数
        ///// </summary>
        ///// <param name="type">缓存数据类型</param>
        ///// <returns>缓存环池参数</returns>
        //public virtual RingPoolParameter GetRingPoolParameter(Type type)
        //{
        //    type = GetRingPoolType(type);
        //    if (type != null)
        //    {
        //        var attribute = type.GetCustomAttribute(typeof(RingPoolAttribute), false);
        //        if (attribute != null) return ((RingPoolAttribute)attribute).Parameter;
        //    }
        //    return RingPoolParameter.Default;
        //}
#if DEBUG
        /// <summary>
        /// Check the timestamp when the lock times out. The default is 60 seconds
        /// 锁超时检查时间戳，默认为 60s
        /// </summary>
        public int LockTimeoutSeconds = 60;
#endif
        /// <summary>
        /// Check whether the remote type is valid
        /// 检查远程类型是否合法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool CheckRemoteType(Type type)
        {
            if (type.IsArray) return CheckRemoteType(type.GetElementType().notNull());
            else
            {
                if (type.IsGenericType && remoteTypes.Contains(type.GetGenericTypeDefinition()))
                {
                    return CheckRemoteType(type.GetGenericArguments()[0]);
                }
                if (remoteTypes.Contains(type)) return true;
            }
            if (type.IsEnum) return true;
            if (remoteTypeAssemblys.Contains(type.Assembly) && type.GetCustomAttribute(typeof(RemoteTypeAttribute)) != null)
            {
                if (!type.IsGenericType) return true;
                bool isType = true;
                foreach (Type checkType in type.GetGenericArguments())
                {
                    if (!CheckRemoteType(checkType))
                    {
                        isType = false;
                        break;
                    }
                }
                if (isType) return true;
            }
#if DEBUG
            AutoCSer.ConsoleWriteQueue.Breakpoint(type.FullName);
#endif
            return false;
        }
        /// <summary>
        /// Add the basic legal remote type
        /// 添加基础合法远程类型
        /// </summary>
        /// <param name="type"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendRemoteType(Type type)
        {
            remoteTypes.Add(type);
        }
        /// <summary>
        /// Add assemblies that verify legitimate remote type tags
        /// 添加验证合法远程类型标记的程序集
        /// </summary>
        /// <param name="assembly"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendRemoteTypeAssembly(Assembly assembly)
        {
            remoteTypeAssemblys.Add(assembly);
        }
        /// <summary>
        /// Add basic legitimate remote type (log by default)
        /// 添加基础合法远程类型（默认写日志）
        /// </summary>
        /// <param name="type">Basic legitimate remote type
        /// 基础合法远程类型</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
#if NetStandard21
        public virtual async Task AppendRemoteTypeAsync(Type type, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public virtual async Task AppendRemoteTypeAsync(Type type, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            await AutoCSer.LogHelper.Debug(type.fullName().notNull(), LogLevelEnum.Debug, callerMemberName, callerFilePath, callerLineNumber);
            remoteTypes.Add(type);
        }
        /// <summary>
        /// Add assemblies that validate valid remote type flags (default log writing)
        /// 添加验证合法远程类型标记的程序集（默认写日志）
        /// </summary>
        /// <param name="assembly">An assembly that validates a valid remote type tag
        /// 验证合法远程类型标记的程序集</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
#if NetStandard21
        public virtual async Task AppendRemoteTypeAssemblyAsync(Assembly assembly, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public virtual async Task AppendRemoteTypeAssemblyAsync(Assembly assembly, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            await AutoCSer.LogHelper.Debug(assembly.FullName.notNull(), LogLevelEnum.Debug, callerMemberName, callerFilePath, callerLineNumber);
            remoteTypeAssemblys.Add(assembly);
        }
#if NetStandard21
        /// <summary>
        /// Compress data
        /// 压缩数据
        /// </summary>
        /// <param name="data">Original data
        /// 原始数据</param>
        /// <param name="buffer">Compressed output buffer
        /// 压缩输出缓冲区</param>
        /// <param name="compressData">Compressed data
        /// 压缩后的数据</param>
        /// <param name="seek">The starting position of the compressed output
        /// 压缩输出起始位置</param>
        /// <param name="compressHeadSize">The size of the compressed excess head
        /// 压缩多余头部大小</param>
        /// <param name="level"></param>
        /// <returns>Whether the compression was successful
        /// 是否压缩成功</returns>
        internal virtual bool Compress(ReadOnlySpan<byte> data, ref ByteArrayBuffer buffer, ref SubArray<byte> compressData, int seek, int compressHeadSize, CompressionLevel level)
        {
            int length = data.Length + seek;
            using (MemoryStream dataStream = GetExpandableMemoryStream(ref buffer, length))
            {
                if (seek != 0) dataStream.Seek(seek, SeekOrigin.Begin);
                using (DeflateStream compressStream = new DeflateStream(dataStream, level, true)) compressStream.Write(data);
                if (dataStream.Position + compressHeadSize < length)
                {
                    byte[] streamBuffer = dataStream.GetBuffer();
                    if (object.ReferenceEquals(streamBuffer, buffer.Buffer?.Buffer) && buffer.Buffer.Pool != null) compressData.Set(streamBuffer, buffer.StartIndex + seek, (int)dataStream.Position - seek);
                    else compressData.Set(streamBuffer, seek, (int)dataStream.Position - seek);
                    return true;
                }
            }
            return false;
        }
#endif
        /// <summary>
        /// Compress data
        /// 压缩数据
        /// </summary>
        /// <param name="data">Original data
        /// 原始数据</param>
        /// <param name="startIndex">The starting position of the original data
        /// 原始数据起始位置</param>
        /// <param name="count">The number of bytes of the original data to be compressed
        /// 原始数据待压缩字节数</param>
        /// <param name="buffer">Compressed output buffer
        /// 压缩输出缓冲区</param>
        /// <param name="compressData">Compressed data
        /// 压缩后的数据</param>
        /// <param name="seek">The starting position of the compressed output
        /// 压缩输出起始位置</param>
        /// <param name="compressHeadSize">The size of the compressed excess head
        /// 压缩多余头部大小</param>
        /// <param name="level"></param>
        /// <returns>Whether the compression was successful
        /// 是否压缩成功</returns>
        internal virtual bool Compress(byte[] data, int startIndex, int count, ref ByteArrayBuffer buffer, ref SubArray<byte> compressData, int seek, int compressHeadSize, CompressionLevel level)
        {
            int length = count + seek;
            using (MemoryStream dataStream = GetExpandableMemoryStream(ref buffer, length))
            {
                if (seek != 0) dataStream.Seek(seek, SeekOrigin.Begin);
                using (DeflateStream compressStream = new DeflateStream(dataStream, level, true)) compressStream.Write(data, startIndex, count);
                if (dataStream.Position + compressHeadSize < length)
                {
                    byte[] streamBuffer = dataStream.GetBuffer();
                    if (object.ReferenceEquals(streamBuffer, buffer.Buffer?.Buffer) && buffer.Buffer.Pool != null) compressData.Set(streamBuffer, buffer.StartIndex + seek, (int)dataStream.Position - seek);
                    else compressData.Set(streamBuffer, seek, (int)dataStream.Position - seek);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Compress data
        /// 压缩数据
        /// </summary>
        /// <param name="data">Original data
        /// 原始数据</param>
        /// <param name="startIndex">The starting position of the original data
        /// 原始数据起始位置</param>
        /// <param name="count">The number of bytes of the original data to be compressed
        /// 原始数据待压缩字节数</param>
        /// <param name="seek">The starting position of the compressed output
        /// 压缩输出起始位置</param>
        /// <param name="level"></param>
        /// <returns>Compressed data
        /// 压缩后的数据</returns>
        internal virtual SubArray<byte> Compress(byte[] data, int startIndex, int count, int seek, CompressionLevel level)
        {
            using (MemoryStream dataStream = new MemoryStream())
            {
                if (seek != 0) dataStream.Seek(seek, SeekOrigin.Begin);
                using (DeflateStream compressStream = new DeflateStream(dataStream, level, true)) compressStream.Write(data, startIndex, count);
                {
                    byte[] streamBuffer = dataStream.GetBuffer();
                    return new SubArray<byte>(seek, (int)dataStream.Position - seek, dataStream.GetBuffer());
                }
            }
        }
        /// <summary>
        /// Data decompression
        /// 数据解压
        /// </summary>
        /// <param name="compressData">Compressed data
        /// 压缩后的数据</param>
        /// <param name="destinationData">The original data buffer waiting to be written
        /// 等待写入的原始数据缓冲区</param>
        /// <returns>Whether the decompression was successful
        /// 是否解压成功</returns>
        internal virtual bool Decompress(ref SubArray<byte> compressData, ref SubArray<byte> destinationData)
        {
            using (MemoryStream memoryStream = compressData.createMemoryStream())
            using (DeflateStream compressStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true))
            {
                int index, count;
                byte[] data = destinationData.GetArray(out index, out count);
                while (count > 0)
                {
                    int readSize = compressStream.Read(data, index, count);
                    if ((count -= readSize) == 0) return true;
                    if (readSize <= 0) return false;
                    index += readSize;
                }
            }
            return false;
        }
#if AOT
        /// <summary>
        /// Information on the setting method for memory flow expansion
        /// 内存流扩容设置方法信息
        /// </summary>
        private static readonly FieldInfo? setMemoryStreamExpandable;
#else
        /// <summary>
        /// Set up memory flow expansion
        /// 设置内存流扩容
        /// </summary>
#if NetStandard21
        private static readonly Action<MemoryStream, bool>? setMemoryStreamExpandable;
#else
        private static readonly Action<MemoryStream, bool> setMemoryStreamExpandable;
#endif
#endif
        /// <summary>
        /// Get an expandable memory stream
        /// 获取可扩容内存流
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        internal virtual MemoryStream GetExpandableMemoryStream(ref ByteArrayBuffer buffer, int size)
        {
            if (setMemoryStreamExpandable != null)
            {
                ByteArrayPool.GetSingleBuffer(ref buffer, size);
                var byteArray = buffer.Buffer.notNull();
                MemoryStream memoryStream = new MemoryStream(byteArray.Buffer, buffer.StartIndex, byteArray.BufferSize, true, true);
#if AOT
                setMemoryStreamExpandable.SetValue(memoryStream, true);
#else
                setMemoryStreamExpandable(memoryStream, true);
#endif
                return memoryStream;
            }
            buffer.Buffer = null;
            return new MemoryStream(size);
        }

        static Config()
        {
#if AOT
            setMemoryStreamExpandable = typeof(MemoryStream).GetField("_expandable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
#else
            setMemoryStreamExpandable = AutoCSer.Reflection.Emit.Field.UnsafeSetField<MemoryStream, bool>("_expandable");
#endif
        }
    }
}
