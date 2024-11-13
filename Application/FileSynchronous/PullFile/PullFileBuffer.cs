using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 拉取文件数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public sealed class PullFileBuffer : AutoCSer.BinarySerialize.ICustomSerialize<PullFileBuffer>
    {
        /// <summary>
        /// 客户端写文件操作
        /// </summary>
#if NetStandard21
        private readonly PullFile? fileWriter;
#else
        private readonly PullFile fileWriter;
#endif
        /// <summary>
        /// 读取文件数据回调委托
        /// </summary>
#if NetStandard21
        private readonly CommandServerKeepCallbackCount<PullFileBuffer>? callback;
#else
        private readonly CommandServerKeepCallbackCount<PullFileBuffer> callback;
#endif
        /// <summary>
        /// 输出错误处理释放序列化等待锁
        /// </summary>
#if NetStandard21
        private Action? releaseSerializeLockHandle;
#else
        private Action releaseSerializeLockHandle;
#endif
        /// <summary>
        /// 序列化等待锁
        /// </summary>
#if DEBUG && NetStandard21
        [AllowNull]
#endif
        private AutoCSer.Threading.SemaphoreSlimLock serializeLock;
        /// <summary>
        /// 读取数据缓冲区
        /// </summary>
        private ByteArrayBuffer buffer;
        /// <summary>
        /// 读取文件字节数
        /// </summary>
        private int bufferSize;
        /// <summary>
        /// 实际序列化字节数
        /// </summary>
        private int serializeSize;
        /// <summary>
        /// 文件读取状态
        /// </summary>
        internal PullFileStateEnum ReadState;
        /// <summary>
        /// 服务端读取文件数据
        /// </summary>
        /// <param name="callback">读取文件数据回调委托</param>
        internal PullFileBuffer(CommandServerKeepCallbackCount<PullFileBuffer> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 客户端写文件操作
        /// </summary>
        /// <param name="fileWriter">客户端写文件操作</param>
        internal PullFileBuffer(PullFile fileWriter)
        {
            this.fileWriter = fileWriter;
        }
        /// <summary>
        /// 读取文件数据
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <returns>是否已经结束</returns>
        internal async Task<bool> Read(SynchronousFileInfo fileInfo, int bufferSize)
        {
            FileInfo file = new FileInfo(fileInfo.FullName);
            if (await AutoCSer.Common.Config.FileExists(file))
            {
                long unreadSize = file.Length - fileInfo.Length;
                if (bufferSize > unreadSize) bufferSize = (int)unreadSize;
#if NetStandard21
                await using (FileStream readStream = await AutoCSer.Common.Config.CreateFileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize, FileOptions.SequentialScan))
#else
                using (FileStream readStream = await AutoCSer.Common.Config.CreateFileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize, FileOptions.SequentialScan))
#endif
                {
                    unreadSize = readStream.Length;
                    if (fileInfo.Length != 0)
                    {
                        await AutoCSer.Common.Config.Seek(readStream, fileInfo.Length, SeekOrigin.Begin);
                        unreadSize -= fileInfo.Length;
                    }
                    if (unreadSize > 0)
                    {
                        if (new FileInfo(fileInfo.FullName).LastWriteTimeUtc == fileInfo.LastWriteTime)
                        {
                            buffer = ByteArrayPool.GetBuffer(bufferSize);
                            try
                            {
                                this.bufferSize = 0;
                                ByteArray byteArray = buffer.Buffer.notNull();
                                byte[] bufferArray = byteArray.Buffer;
                                bufferSize = byteArray.BufferSize;
                                do
                                {
                                    int readSize = await readStream.ReadAsync(bufferArray, buffer.StartIndex + this.bufferSize, bufferSize - this.bufferSize);
                                    unreadSize -= readSize;
                                    this.bufferSize += readSize;
                                    if (!await callbackAsync()) return true;
                                }
                                while (unreadSize > 0);
                                while (this.bufferSize > 0)
                                {
                                    if (!await callbackAsync()) return true;
                                }
                                ReadState = PullFileStateEnum.Success;
                                return true;
                            }
                            finally { buffer.Free(); }
                        }
                        else ReadState = PullFileStateEnum.LastWriteTimeNotMatch;
                    }
                    else ReadState = PullFileStateEnum.LengthLess;
                }
            }
            else ReadState = PullFileStateEnum.NotExist;
            return false;
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <returns></returns>
        private async Task<bool> callbackAsync()
        {
            if (releaseSerializeLockHandle == null)
            {
                serializeLock = new AutoCSer.Threading.SemaphoreSlimLock(0, 1);
                releaseSerializeLockHandle = releaseSerializeLock;
            }
            ReadState = PullFileStateEnum.Success;
            if (await callback.notNull().CallbackAsync(this, releaseSerializeLockHandle))
            {
                await serializeLock.EnterAsync();
                ReadState = PullFileStateEnum.Unknown;
                if (serializeSize > 0)
                {
                    if ((bufferSize -= serializeSize) != 0)
                    {
                        byte[] bufferArray = buffer.Buffer.notNull().Buffer;
                        AutoCSer.Common.Config.CopyTo(bufferArray, buffer.StartIndex + serializeSize, bufferArray, buffer.StartIndex, bufferSize);
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 输出错误处理释放序列化等待锁
        /// </summary>
        private void releaseSerializeLock()
        {
            serializeSize = -1;
            serializeLock.Exit();
        }
        /// <summary>
        /// 服务端序列化
        /// </summary>
        /// <param name="serializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<PullFileBuffer>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            stream.Write((uint)(byte)ReadState);
#if DEBUG
            if (ReadState == PullFileStateEnum.Success && !stream.IsResizeError)
            {
                serializeSize = serializer.CustomWriteFree(buffer.Buffer.notNull().Buffer, buffer.StartIndex, bufferSize);
                if (stream.IsResizeError) AutoCSer.ConsoleWriteQueue.Breakpoint();
            }
#else
            if (ReadState == PullFileStateEnum.Success) serializeSize = serializer.CustomWriteFree(buffer.Buffer.notNull().Buffer, buffer.StartIndex, bufferSize);
#endif
            if (!stream.IsResizeError) serializeLock.Exit();
        }
        /// <summary>
        /// 客户端反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<PullFileBuffer>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            int state;
            PullFile fileWriter = this.fileWriter.notNull();
            if (deserializer.Read(out state))
            {
                if(state == (byte)PullFileStateEnum.Success)
                {
                    SubArray<byte> buffer = new SubArray<byte>();
                    if (deserializer.DeserializeBuffer(ref buffer, true))
                    {
                        fileWriter.Write(ref buffer);
                        return;
                    }
                }
                else fileWriter.Error(state);
                return;
            }
            fileWriter.Error((byte)PullFileStateEnum.DeserializeError);
        }
    }
}
