using AutoCSer.BinarySerialize;
using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 磁盘块客户端
    /// </summary>
    public sealed partial class DiskBlockClient
    {
        /// <summary>
        /// 磁盘块客户端套接字事件
        /// </summary>
        private readonly IDiskBlockClientSocketEvent client;
        /// <summary>
        /// 磁盘块客户端接口
        /// </summary>
        public IDiskBlockClient Client { get { return client.DiskBlockClient; } }
        /// <summary>
        /// 磁盘块客户端
        /// </summary>
        /// <param name="client">磁盘块客户端套接字事件</param>
        public DiskBlockClient(IDiskBlockClientSocketEvent client)
        {
            this.client = client;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task Write(SubArray<byte> data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                bool isIndex;
                BlockIndex index = BlockIndex.GetIndexSize(ref data, out isIndex);
                if (isIndex)
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    callback(index);
                    return;
                }
                if (await Client.Write(data, callback)) returnType = CommandClientReturnTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<BlockIndex> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public CommandClientReturnValue<BlockIndex> Write(SubArray<byte> data)
        {
            bool isIndex;
            BlockIndex index = BlockIndex.GetIndexSize(ref data, out isIndex);
            if (isIndex) return index;
            return Client.WaitWrite(data);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public ReturnCommand<BlockIndex> WriteAsync(SubArray<byte> data)
        {
            bool isIndex;
            BlockIndex index = BlockIndex.GetIndexSize(ref data, out isIndex);
            if (isIndex) return new CompletedReturnCommand<BlockIndex>(ref index);
            return Client.Write(data);
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
#if NetStandard21
        public async Task WriteString(string? data, Action<CommandClientReturnValue<BlockIndex>> callback)
#else
        public async Task WriteString(string data, Action<CommandClientReturnValue<BlockIndex>> callback)
#endif
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                bool isIndex;
                BlockIndex index = BlockIndex.GetIndexSize(data, out isIndex);
                if (isIndex)
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    callback(index);
                    return;
                }
                if (await Client.Write(data.notNull(), callback)) returnType = CommandClientReturnTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<BlockIndex> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
#if NetStandard21
        public CommandClientReturnValue<BlockIndex> WriteString(string? data)
#else
        public CommandClientReturnValue<BlockIndex> WriteString(string data)
#endif
        {
            bool isIndex;
            BlockIndex index = BlockIndex.GetIndexSize(data, out isIndex);
            if (isIndex) return index;
            return Client.WaitWrite(data.notNull());
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
#if NetStandard21
        public ReturnCommand<BlockIndex> WriteStringAsync(string? data)
#else
        public ReturnCommand<BlockIndex> WriteStringAsync(string data)
#endif
        {
            bool isIndex;
            BlockIndex index = BlockIndex.GetIndexSize(data, out isIndex);
            if (isIndex) return new CompletedReturnCommand<BlockIndex>(ref index);
            return Client.Write(data.notNull());
        }
        /// <summary>
        /// 写入 JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteJson<T>(T data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                if (data == null)
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    callback(BlockIndex.JsonNull);
                    return;
                }
                if (await Client.Write(new WriteBuffer(new JsonSerializer<T>(data)), callback)) returnType = CommandClientReturnTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<BlockIndex> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 写入 JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public CommandClientReturnValue<BlockIndex> WriteJson<T>(T data)
        {
            if (data == null) return BlockIndex.JsonNull;
            return Client.WaitWrite(new WriteBuffer(new JsonSerializer<T>(data)));
        }
        /// <summary>
        /// 写入 JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<BlockIndex> WriteJsonAsync<T>(T data)
        {
            return data == null ? BlockIndex.JsonNullCompletedReturnCommand : Client.Write(new WriteBuffer(new JsonSerializer<T>(data)));
        }
        /// <summary>
        /// 写入 JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteJsonMemberMap<T>(AutoCSer.Metadata.MemberMapValue<T> data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                if (data.Value == null)
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    callback(BlockIndex.JsonNull);
                    return;
                }
                if (await Client.Write(new WriteBuffer(new JsonSerializer<AutoCSer.Metadata.MemberMapValue<T>>(data)), callback)) returnType = CommandClientReturnTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<BlockIndex> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 写入 JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public CommandClientReturnValue<BlockIndex> WriteJsonMemberMap<T>(AutoCSer.Metadata.MemberMapValue<T> data)
        {
            if (data.Value == null) return BlockIndex.JsonNull;
            return Client.WaitWrite(new WriteBuffer(new JsonSerializer<AutoCSer.Metadata.MemberMapValue<T>>(data)));
        }
        /// <summary>
        /// 写入 JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<BlockIndex> WriteJsonMemberMapAsync<T>(AutoCSer.Metadata.MemberMapValue<T> data)
        {
            if (data.Value == null) return BlockIndex.JsonNullCompletedReturnCommand;
            return Client.Write(new WriteBuffer(new JsonSerializer<AutoCSer.Metadata.MemberMapValue<T>>(data)));
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
#if NetStandard21
        public async Task WriteBinary<T>(T? data, Action<CommandClientReturnValue<BlockIndex>> callback)
#else
        public async Task WriteBinary<T>(T data, Action<CommandClientReturnValue<BlockIndex>> callback)
#endif
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                if (data == null)
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    callback(new BlockIndex(BinarySerializer.NullValue, -4));
                    return;
                }
                if (await Client.Write(WriteBuffer.CreateWriteBufferSerializer(data), callback)) returnType = CommandClientReturnTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<BlockIndex> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
#if NetStandard21
        public CommandClientReturnValue<BlockIndex> WriteBinary<T>(T? data)
#else
        public CommandClientReturnValue<BlockIndex> WriteBinary<T>(T data)
#endif
        {
            if (data == null) return new BlockIndex(BinarySerializer.NullValue, -4);
            return Client.WaitWrite(WriteBuffer.CreateWriteBufferSerializer(data));
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ReturnCommand<BlockIndex> WriteBinaryAsync<T>(T? data)
#else
        public ReturnCommand<BlockIndex> WriteBinaryAsync<T>(T data)
#endif
        {
            if (data == null) return BlockIndex.BinarySerializeNullValueCompletedReturnCommand;
            return Client.Write(WriteBuffer.CreateWriteBufferSerializer(data));
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteBinaryMemberMap<T>(AutoCSer.Metadata.MemberMapValue<T> data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                if (data.Value == null)
                {
                    returnType = CommandClientReturnTypeEnum.Success;
                    callback(new BlockIndex(BinarySerializer.NullValue, -4));
                    return;
                }
                if (await Client.Write(WriteBuffer.CreateWriteBufferSerializer(data), callback)) returnType = CommandClientReturnTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<BlockIndex> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public CommandClientReturnValue<BlockIndex> WriteBinaryMemberMap<T>(AutoCSer.Metadata.MemberMapValue<T> data)
        {
            if (data.Value == null) return new BlockIndex(BinarySerializer.NullValue, -4);
            return Client.WaitWrite(WriteBuffer.CreateWriteBufferSerializer(data));
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<BlockIndex> WriteBinaryMemberMapAsync<T>(AutoCSer.Metadata.MemberMapValue<T> data)
        {
            if (data.Value == null) return BlockIndex.BinarySerializeNullValueCompletedReturnCommand;
            return Client.Write(WriteBuffer.CreateWriteBufferSerializer(data));
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
#if NetStandard21
        public async Task Read(BlockIndex index, Action<ReadResult<byte[]?>> callback)
#else
        public async Task Read(BlockIndex index, Action<ReadResult<byte[]>> callback)
#endif
        {
            bool isCallback = false;
#if NetStandard21
            ReadResult<byte[]?> result = default(ReadResult<byte[]?>);
#else
            ReadResult<byte[]> result = default(ReadResult<byte[]>);
#endif
            try
            {
                if (!index.GetResult(out result))
                {
                    ReadCallback readCallback = new ReadCallback(callback);
                    isCallback = await Client.Read(readCallback, index, readCallback.Callback);
                }
            }
            finally
            {
                if (!isCallback) callback(result);
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public ReadResult<byte[]?> Read(BlockIndex index)
#else
        public ReadResult<byte[]> Read(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<byte[]?> result;
#else
            ReadResult<byte[]> result;
#endif
            if (index.GetResult(out result)) return result;
            ReadCallback readCallback = new ReadCallback();
            return readCallback.GetResult(Client.WaitRead(readCallback, index));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public ReadAwaiter<byte[]?> ReadAsync(BlockIndex index)
#else
        public ReadAwaiter<byte[]> ReadAsync(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<byte[]?> result;
            if (index.GetResult(out result)) return new CompletedReadAwaiter<byte[]?>(result);
#else
            ReadResult<byte[]> result;
            if (index.GetResult(out result)) return new CompletedReadAwaiter<byte[]>(result);
#endif
            return new ReadAwaiter(Client, index);
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
#if NetStandard21
        public async Task ReadString(BlockIndex index, Action<ReadResult<string?>> callback)
#else
        public async Task ReadString(BlockIndex index, Action<ReadResult<string>> callback)
#endif
        {
            bool isCallback = false;
#if NetStandard21
            ReadResult<string?> result = default(ReadResult<string?>);
#else
            ReadResult<string> result = default(ReadResult<string>);
#endif
            try
            {
                if (!index.GetResult(out result))
                {
                    ReadStringCallback readStringCallback = new ReadStringCallback(callback);
                    isCallback = await Client.Read(readStringCallback, index, readStringCallback.Callback);
                }
            }
            finally
            {
                if (!isCallback) callback(result);
            }
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public ReadResult<string?> ReadString(BlockIndex index)
#else
        public ReadResult<string> ReadString(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<string?> result;
#else
            ReadResult<string> result;
#endif
            if (index.GetResult(out result)) return result;
            ReadStringCallback readStringCallback = new ReadStringCallback();
            return readStringCallback.GetResult(Client.WaitRead(readStringCallback, index));
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public ReadAwaiter<string?> ReadStringAsync(BlockIndex index)
#else
        public ReadAwaiter<string> ReadStringAsync(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<string?> result;
            if (index.GetResult(out result)) return new CompletedReadAwaiter<string?>(result);
#else
            ReadResult<string> result;
            if (index.GetResult(out result)) return new CompletedReadAwaiter<string>(result);
#endif
            return new ReadStringAwaiter(Client, index);
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
#if NetStandard21
        public async Task ReadJson<T>(BlockIndex index, Action<ReadResult<T?>> callback)
#else
        public async Task ReadJson<T>(BlockIndex index, Action<ReadResult<T>> callback)
#endif
        {
            bool isCallback = false;
#if NetStandard21
            ReadResult<T?> result = default(ReadResult<T?>);
#else
            ReadResult<T> result = default(ReadResult<T>);
#endif
            try
            {
                if (!index.GetJsonResult(out result))
                {
                    ReadJsonCallback<T> readJsonCallback = new ReadJsonCallback<T>(callback);
                    isCallback = await Client.Read(readJsonCallback, index, readJsonCallback.Callback);
                }
            }
            finally
            {
                if (!isCallback) callback(result);
            }
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public ReadResult<T?> ReadJson<T>(BlockIndex index)
#else
        public ReadResult<T> ReadJson<T>(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<T?> result;
#else
            ReadResult<T> result;
#endif
            if (index.GetJsonResult(out result)) return result;
            ReadJsonCallback<T> readJsonCallback = new ReadJsonCallback<T>();
            return readJsonCallback.GetResult(Client.WaitRead(readJsonCallback, index));
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public ReadAwaiter<T?> ReadJsonAsync<T>(BlockIndex index)
#else
        public ReadAwaiter<T> ReadJsonAsync<T>(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<T?> result;
            if (index.GetJsonResult(out result)) return new CompletedReadAwaiter<T?>(result);
            return new ReadJsonAwaiter<T?>(Client, index);
#else
            ReadResult<T> result;
            if (index.GetJsonResult(out result)) return new CompletedReadAwaiter<T>(result);
            return new ReadJsonAwaiter<T>(Client, index);
#endif
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
        public async Task ReadJsonMemberMap<T>(BlockIndex index, Action<ReadResult<AutoCSer.Metadata.MemberMapValue<T>>> callback)
        {
            bool isCallback = false;
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result = default(ReadResult<AutoCSer.Metadata.MemberMapValue<T>>);
            try
            {
                if (!index.GetJsonResult(out result))
                {
                    ReadJsonCallback<AutoCSer.Metadata.MemberMapValue<T>> readJsonCallback = new ReadJsonCallback<AutoCSer.Metadata.MemberMapValue<T>>(callback);
                    isCallback = await Client.Read(readJsonCallback, index, readJsonCallback.Callback);
                }
            }
            finally
            {
                if (!isCallback) callback(result);
            }
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public ReadResult<AutoCSer.Metadata.MemberMapValue<T>> ReadJsonMemberMap<T>(BlockIndex index)
        {
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result;
            if (index.GetJsonResult(out result)) return result;
            ReadJsonCallback<AutoCSer.Metadata.MemberMapValue<T>> readJsonCallback = new ReadJsonCallback<AutoCSer.Metadata.MemberMapValue<T>>();
            return readJsonCallback.GetResult(Client.WaitRead(readJsonCallback, index));
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public ReadAwaiter<AutoCSer.Metadata.MemberMapValue<T>> ReadJsonMemberMapAsync<T>(BlockIndex index)
        {
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result;
            if (index.GetJsonResult(out result)) return new CompletedReadAwaiter<AutoCSer.Metadata.MemberMapValue<T>>(result);
            return new ReadJsonAwaiter<AutoCSer.Metadata.MemberMapValue<T>>(Client, index);
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
#if NetStandard21
        public async Task ReadBinary<T>(BlockIndex index, Action<ReadResult<T?>> callback)
#else
        public async Task ReadBinary<T>(BlockIndex index, Action<ReadResult<T>> callback)
#endif
        {
            bool isCallback = false;
#if NetStandard21
            ReadResult<T?> result = default(ReadResult<T?>);
#else
            ReadResult<T> result = default(ReadResult<T>);
#endif
            try
            {
                if (!index.GetBinaryResult(out result))
                {
                    ReadBinaryCallback<T> readBinaryCallback = new ReadBinaryCallback<T>(callback);
                    isCallback = await Client.Read(readBinaryCallback, index, readBinaryCallback.Callback);
                }
            }
            finally
            {
                if (!isCallback) callback(result);
            }
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public ReadResult<T?> ReadBinary<T>(BlockIndex index)
#else
        public ReadResult<T> ReadBinary<T>(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<T?> result;
#else
            ReadResult<T> result;
#endif
            if (index.GetBinaryResult(out result)) return result;
            ReadBinaryCallback<T> readBinaryCallback = new ReadBinaryCallback<T>();
            return readBinaryCallback.GetResult(Client.WaitRead(readBinaryCallback, index));
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public ReadAwaiter<T?> ReadBinaryAsync<T>(BlockIndex index)
#else
        public ReadAwaiter<T> ReadBinaryAsync<T>(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<T?> result;
            if (index.GetBinaryResult(out result))
            {
                return new CompletedReadAwaiter<T?>(result);
            }
            return new ReadBinaryAwaiter<T?>(Client, index);
#else
            ReadResult<T> result;
            if (index.GetBinaryResult(out result)) return new CompletedReadAwaiter<T>(result);
            return new ReadBinaryAwaiter<T>(Client, index);
#endif
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
        public async Task ReadBinaryMemberMap<T>(BlockIndex index, Action<ReadResult<AutoCSer.Metadata.MemberMapValue<T>>> callback)
        {
            bool isCallback = false;
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result = default(ReadResult<AutoCSer.Metadata.MemberMapValue<T>>);
            try
            {
                if (!index.GetBinaryResult(out result))
                {
                    ReadBinaryCallback<AutoCSer.Metadata.MemberMapValue<T>> readBinaryCallback = new ReadBinaryCallback<AutoCSer.Metadata.MemberMapValue<T>>(callback);
                    isCallback = await Client.Read(readBinaryCallback, index, readBinaryCallback.Callback);
                }
            }
            finally
            {
                if (!isCallback) callback(result);
            }
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public ReadResult<AutoCSer.Metadata.MemberMapValue<T>> ReadBinaryMemberMap<T>(BlockIndex index)
        {
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result;
            if (index.GetBinaryResult(out result)) return result;
            ReadBinaryCallback<AutoCSer.Metadata.MemberMapValue<T>> readBinaryCallback = new ReadBinaryCallback<AutoCSer.Metadata.MemberMapValue<T>>();
            return readBinaryCallback.GetResult(Client.WaitRead(readBinaryCallback, index));
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public ReadAwaiter<AutoCSer.Metadata.MemberMapValue<T>> ReadBinaryMemberMapAsync<T>(BlockIndex index)
        {
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result;
            if (index.GetBinaryResult(out result)) return new CompletedReadAwaiter<AutoCSer.Metadata.MemberMapValue<T>>(result);
            return new ReadBinaryAwaiter<AutoCSer.Metadata.MemberMapValue<T>>(Client, index);
        }

        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">磁盘块当前写入位置</param>
        /// <returns></returns>
        public async Task SwitchBlock(uint identity, Action<CommandClientReturnValue<long>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                if (await Client.SwitchBlock(identity, new CommandClientCallback<BlockIndex, long>(callback, p => p.Index))) returnType = CommandClientReturnTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<long> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>磁盘块当前写入位置</returns>
        public CommandClientReturnValue<long> WaitSwitchBlock(uint identity)
        {
            return Client.WaitSwitchBlock(identity).Cast(p => p.Index);
        }
        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>磁盘块当前写入位置</returns>
        public async Task<CommandClientReturnValue<long>> SwitchBlockAsync(uint identity)
        {
            return (await Client.SwitchBlock(identity)).Cast(p => p.Index);
        }
    }
}
