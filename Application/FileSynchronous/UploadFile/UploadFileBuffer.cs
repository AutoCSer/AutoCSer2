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
        /// 是否输出反序列化错误日志
        /// </summary>
        private static bool isDeserializeLog = true;
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<UploadFileBuffer>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            if (deserializer.Read(out UploaderIndex.Index) && deserializer.Read(out UploaderIndex.Identity) && deserializer.Read(out FileIndex.Index) && deserializer.Read(out FileIndex.Identity)
                && deserializer.DeserializeBuffer(ref Buffer, true))
            {
                UploadFileService service = UploadFileService.SingleService;
                if (service != null) State = service.UploadFileData(this);
                else
                {
                    CommandServerSocket socket = (CommandServerSocket)deserializer.Context;
                    ICommandServerSocketSessionObject<UploadFileService, UploadFileService> sessionObject = socket.SessionObject as ICommandServerSocketSessionObject<UploadFileService, UploadFileService>;
                    if (sessionObject != null)
                    {
                        service = sessionObject.TryGetSessionObject(socket);
                        if (service != null) State = service.UploadFileData(this);
                        else
                        {
                            isDeserializeLog = false;
                            AutoCSer.LogHelper.ErrorIgnoreException($"文件上传服务无法从套接字自定义会话对象中获取文件上传服务端对象，请确认类型 {socket.SessionObject.GetType().fullName()} 是否正确实现 {typeof(ICommandServerSocketSessionObject<UploadFileService, UploadFileService>).fullName()}.{nameof(ICommandServerSocketSessionObject<UploadFileService, UploadFileService>.CreateSessionObject)}", LogLevelEnum.Error | LogLevelEnum.Fatal);
                        }
                    }
                    else if (isDeserializeLog)
                    {
                        isDeserializeLog = false;
                        if (socket.SessionObject == null) AutoCSer.LogHelper.ErrorIgnoreException($"文件上传服务缺少套接字自定义会话对象，请在初始化阶段调用 {typeof(IUploadFileClient).fullName()}.{nameof(IUploadFileClient.CreateSessionObject)}", LogLevelEnum.Error | LogLevelEnum.Fatal);
                        else AutoCSer.LogHelper.ErrorIgnoreException($"文件上传服务套接字自定义会话对象类型错误 {socket.SessionObject.GetType().fullName()} 未实现接口 {typeof(ICommandServerSocketSessionObject<UploadFileService, UploadFileService>).fullName()}", LogLevelEnum.Error | LogLevelEnum.Fatal);
                    }
                }
                if (State == UploadFileStateEnum.Unknown) State = UploadFileStateEnum.NotFoundSessionObject;
            }
        }
    }
}
