﻿using AutoCSer.BinarySerialize;
using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 磁盘块客户端
    /// </summary>
    public sealed class DiskBlockClient
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
        public async Task<CommandClientReturnValue<BlockIndex>> WriteAsync(SubArray<byte> data)
        {
            bool isIndex;
            BlockIndex index = BlockIndex.GetIndexSize(ref data, out isIndex);
            if (isIndex) return index;
            return await Client.Write(data);
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteString(string data, Action<CommandClientReturnValue<BlockIndex>> callback)
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
                if (await Client.Write(data, callback)) returnType = CommandClientReturnTypeEnum.Success;
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
        public CommandClientReturnValue<BlockIndex> WriteString(string data)
        {
            bool isIndex;
            BlockIndex index = BlockIndex.GetIndexSize(data, out isIndex);
            if (isIndex) return index;
            return Client.WaitWrite(data);
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteStringAsync(string data)
        {
            bool isIndex;
            BlockIndex index = BlockIndex.GetIndexSize(data, out isIndex);
            if (isIndex) return index;
            return await Client.Write(data);
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
        public async Task<CommandClientReturnValue<BlockIndex>> WriteJsonAsync<T>(T data)
        {
            if (data == null) return BlockIndex.JsonNull;
            return await Client.Write(new WriteBuffer(new JsonSerializer<T>(data)));
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
        public async Task<CommandClientReturnValue<BlockIndex>> WriteJsonMemberMapAsync<T>(AutoCSer.Metadata.MemberMapValue<T> data)
        {
            if (data.Value == null) return BlockIndex.JsonNull;
            return await Client.Write(new WriteBuffer(new JsonSerializer<AutoCSer.Metadata.MemberMapValue<T>>(data)));
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteBinary<T>(T data, Action<CommandClientReturnValue<BlockIndex>> callback)
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
                if (await Client.Write(new WriteBuffer(new WriteBufferSerializer<ServerReturnValue<T>>(new ServerReturnValue<T>(data))), callback)) returnType = CommandClientReturnTypeEnum.Success;
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
        public CommandClientReturnValue<BlockIndex> WriteBinary<T>(T data)
        {
            if (data == null) return new BlockIndex(BinarySerializer.NullValue, -4);
            return Client.WaitWrite(new WriteBuffer(new WriteBufferSerializer<ServerReturnValue<T>>(new ServerReturnValue<T>(data))));
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteBinaryAsync<T>(T data)
        {
            if (data == null) return new BlockIndex(BinarySerializer.NullValue, -4);
            return await Client.Write(new WriteBuffer(new WriteBufferSerializer<ServerReturnValue<T>>(new ServerReturnValue<T>(data))));
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
                if (await Client.Write(new WriteBuffer(new WriteBufferSerializer<ServerReturnValue<AutoCSer.Metadata.MemberMapValue<T>>>(new ServerReturnValue<AutoCSer.Metadata.MemberMapValue<T>>(data))), callback)) returnType = CommandClientReturnTypeEnum.Success;
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
            return Client.WaitWrite(new WriteBuffer(new WriteBufferSerializer<ServerReturnValue<AutoCSer.Metadata.MemberMapValue<T>>>(new ServerReturnValue<AutoCSer.Metadata.MemberMapValue<T>>(data))));
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteBinaryMemberMapAsync<T>(AutoCSer.Metadata.MemberMapValue<T> data)
        {
            if (data.Value == null) return new BlockIndex(BinarySerializer.NullValue, -4);
            return await Client.Write(new WriteBuffer(new WriteBufferSerializer<ServerReturnValue<AutoCSer.Metadata.MemberMapValue<T>>>(new ServerReturnValue<AutoCSer.Metadata.MemberMapValue<T>>(data))));
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
        public async Task Read(BlockIndex index, Action<ReadResult<byte[]>> callback)
        {
            bool isCallback = false;
            ReadResult<byte[]> result = default(ReadResult<byte[]>);
            try
            {
                if (!index.GetResult(out result))
                {
                    ReadCallback readCallback = new ReadCallback(callback);
                    isCallback = await Client.Read((ReadBuffer)readCallback.Deserialize, index, readCallback.Callback);
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
        public ReadResult<byte[]> Read(BlockIndex index)
        {
            ReadResult<byte[]> result;
            if (index.GetResult(out result)) return result;
            ReadCallback readCallback = new ReadCallback();
            return readCallback.GetResult(Client.WaitRead((ReadBuffer)readCallback.Deserialize, index));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public async Task<ReadResult<byte[]>> ReadAsync(BlockIndex index)
        {
            ReadResult<byte[]> result;
            if (index.GetResult(out result)) return result;
            ReadCallback readCallback = new ReadCallback();
            return readCallback.GetResult(await Client.Read((ReadBuffer)readCallback.Deserialize, index));
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
        public async Task ReadString(BlockIndex index, Action<ReadResult<string>> callback)
        {
            bool isCallback = false;
            ReadResult<string> result = default(ReadResult<string>);
            try
            {
                if (!index.GetResult(out result))
                {
                    ReadStringCallback readStringCallback = new ReadStringCallback(callback);
                    isCallback = await Client.Read((ReadBuffer)readStringCallback.Deserialize, index, readStringCallback.Callback);
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
        public ReadResult<string> ReadString(BlockIndex index)
        {
            ReadResult<string> result;
            if (index.GetResult(out result)) return result;
            ReadStringCallback readStringCallback = new ReadStringCallback();
            return readStringCallback.GetResult(Client.WaitRead((ReadBuffer)readStringCallback.Deserialize, index));
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public async Task<ReadResult<string>> ReadStringAsync(BlockIndex index)
        {
            ReadResult<string> result;
            if (index.GetResult(out result)) return result;
            ReadStringCallback readStringCallback = new ReadStringCallback();
            return readStringCallback.GetResult(await Client.Read((ReadBuffer)readStringCallback.Deserialize, index));
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
        public async Task ReadJson<T>(BlockIndex index, Action<ReadResult<T>> callback)
        {
            bool isCallback = false;
            ReadResult<T> result = default(ReadResult<T>);
            try
            {
                if (!index.GetJsonResult(out result))
                {
                    ReadJsonCallback<T> readJsonCallback = new ReadJsonCallback<T>(callback);
                    isCallback = await Client.Read((ReadBuffer)readJsonCallback.Deserialize, index, readJsonCallback.Callback);
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
        public ReadResult<T> ReadJson<T>(BlockIndex index)
        {
            ReadResult<T> result;
            if (index.GetJsonResult(out result)) return result;
            ReadJsonCallback<T> readJsonCallback = new ReadJsonCallback<T>();
            return readJsonCallback.GetResult(Client.WaitRead((ReadBuffer)readJsonCallback.Deserialize, index));
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public async Task<ReadResult<T>> ReadJsonAsync<T>(BlockIndex index)
        {
            ReadResult<T> result;
            if (index.GetJsonResult(out result)) return result;
            ReadJsonCallback<T> readJsonCallback = new ReadJsonCallback<T>();
            return readJsonCallback.GetResult(await Client.Read((ReadBuffer)readJsonCallback.Deserialize, index));
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
                    isCallback = await Client.Read((ReadBuffer)readJsonCallback.Deserialize, index, readJsonCallback.Callback);
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
            return readJsonCallback.GetResult(Client.WaitRead((ReadBuffer)readJsonCallback.Deserialize, index));
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public async Task<ReadResult<AutoCSer.Metadata.MemberMapValue<T>>> ReadJsonMemberMapAsync<T>(BlockIndex index)
        {
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result;
            if (index.GetJsonResult(out result)) return result;
            ReadJsonCallback<AutoCSer.Metadata.MemberMapValue<T>> readJsonCallback = new ReadJsonCallback<AutoCSer.Metadata.MemberMapValue<T>>();
            return readJsonCallback.GetResult(await Client.Read((ReadBuffer)readJsonCallback.Deserialize, index));
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="callback">读取数据结果</param>
        /// <returns></returns>
        public async Task ReadBinary<T>(BlockIndex index, Action<ReadResult<T>> callback)
        {
            bool isCallback = false;
            ReadResult<T> result = default(ReadResult<T>);
            try
            {
                if (!index.GetBinaryResult(out result))
                {
                    ReadBinaryCallback<T> readBinaryCallback = new ReadBinaryCallback<T>(callback);
                    isCallback = await Client.Read((ReadBuffer)readBinaryCallback.DeserializeNotReference, index, readBinaryCallback.Callback);
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
        public ReadResult<T> ReadBinary<T>(BlockIndex index)
        {
            ReadResult<T> result;
            if (index.GetBinaryResult(out result)) return result;
            ReadBinaryCallback<T> readBinaryCallback = new ReadBinaryCallback<T>();
            return readBinaryCallback.GetResult(Client.WaitRead((ReadBuffer)readBinaryCallback.DeserializeNotReference, index));
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public async Task<ReadResult<T>> ReadBinaryAsync<T>(BlockIndex index)
        {
            ReadResult<T> result;
            if (index.GetBinaryResult(out result)) return result;
            ReadBinaryCallback<T> readBinaryCallback = new ReadBinaryCallback<T>();
            return readBinaryCallback.GetResult(await Client.Read((ReadBuffer)readBinaryCallback.DeserializeNotReference, index));
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
                    isCallback = await Client.Read((ReadBuffer)readBinaryCallback.DeserializeNotReference, index, readBinaryCallback.Callback);
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
            return readBinaryCallback.GetResult(Client.WaitRead((ReadBuffer)readBinaryCallback.DeserializeNotReference, index));
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
        public async Task<ReadResult<AutoCSer.Metadata.MemberMapValue<T>>> ReadBinaryMemberMapAsync<T>(BlockIndex index)
        {
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result;
            if (index.GetBinaryResult(out result)) return result;
            ReadBinaryCallback<AutoCSer.Metadata.MemberMapValue<T>> readBinaryCallback = new ReadBinaryCallback<AutoCSer.Metadata.MemberMapValue<T>>();
            return readBinaryCallback.GetResult(await Client.Read((ReadBuffer)readBinaryCallback.DeserializeNotReference, index));
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