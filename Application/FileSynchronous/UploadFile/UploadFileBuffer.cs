using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件上传数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public sealed class UploadFileBuffer : AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>
    {
        /// <summary>
        /// 上传索引信息
        /// </summary>
        internal UploadFileIndex UploaderIndex;
        /// <summary>
        /// 上传文件索引信息
        /// </summary>
        internal UploadFileIndex FileIndex;
        /// <summary>
        /// 上传文件数据
        /// </summary>
        internal SubArray<byte> Buffer;
        /// <summary>
        /// 序列化数据字节数
        /// </summary>
        internal int SerializeSize;
        /// <summary>
        /// 文件上传状态
        /// </summary>
        internal UploadFileStateEnum State;
        /// <summary>
        /// 默认空文件上传数据
        /// </summary>
        private UploadFileBuffer() { }
        /// <summary>
        /// 文件上传数据
        /// </summary>
        /// <param name="uploaderIndex">上传索引信息</param>
        /// <param name="fileIndex">上传文件索引信息</param>
        internal UploadFileBuffer(UploadFileIndex uploaderIndex, UploadFileIndex fileIndex)
        {
            this.UploaderIndex = uploaderIndex;
            this.FileIndex = fileIndex;
        }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            stream.Write(UploaderIndex.Index);
            stream.Write(UploaderIndex.Identity);
            stream.Write(FileIndex.Index);
            stream.Write(FileIndex.Identity);
#if DEBUG
            if (!stream.IsResizeError)
            {
                SerializeSize = serializer.CustomWriteFree(Buffer.Array, Buffer.Start, Buffer.Length);
                if (stream.IsResizeError) AutoCSer.ConsoleWriteQueue.Breakpoint();
            }
#else
            SerializeSize = serializer.CustomWriteFree(Buffer.Array, Buffer.Start, Buffer.Length);
#endif
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            if (deserializer.Read(out UploaderIndex.Index) && deserializer.Read(out UploaderIndex.Identity) && deserializer.Read(out FileIndex.Index) && deserializer.Read(out FileIndex.Identity)
                && deserializer.DeserializeBuffer(ref Buffer, true))
            {
                CommandServerController<IUploadFileService> controller = (CommandServerController<IUploadFileService>)deserializer.Context.castType<CommandServerSocket>().notNull().CurrentController;
                State = ((UploadFileService)controller.Controller).UploadFileData(this);
            }
        }

        /// <summary>
        /// 默认空文件上传数据
        /// </summary>
        internal static readonly UploadFileBuffer Null = new UploadFileBuffer();
    }
}
