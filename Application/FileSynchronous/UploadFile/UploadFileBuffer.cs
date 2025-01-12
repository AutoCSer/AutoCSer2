using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.IO;

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
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            byte* data = serializer.Stream.GetBeforeMove(sizeof(int) * 2 + sizeof(uint) * 2);
            if (data != null)
            {
                *(int*)data = UploaderIndex.Index;
                *(uint*)(data + sizeof(int)) = UploaderIndex.Identity;
                *(int*)(data + (sizeof(int) + sizeof(uint))) = FileIndex.Index;
                *(uint*)(data + (sizeof(int) * 2 + sizeof(uint))) = FileIndex.Identity;
#if DEBUG
                SerializeSize = serializer.CustomWriteFree(Buffer.Array, Buffer.Start, Buffer.Length);
                if (serializer.Stream.IsResizeError) AutoCSer.ConsoleWriteQueue.Breakpoint();
#else
                SerializeSize = serializer.CustomWriteFree(Buffer.Array, Buffer.Start, Buffer.Length);
#endif
            }
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            byte* data = deserializer.GetBeforeMove(sizeof(int) * 2 + sizeof(uint) * 2);
            if (data != null && deserializer.DeserializeBuffer(ref Buffer, true))
            {
                UploaderIndex.Index = *(int*)data;
                UploaderIndex.Identity = *(uint*)(data + sizeof(int));
                FileIndex.Index = *(int*)(data + (sizeof(int) + sizeof(uint)));
                FileIndex.Identity = *(uint*)(data + (sizeof(int) * 2 + sizeof(uint)));
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
