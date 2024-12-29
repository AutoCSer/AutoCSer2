using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 分布式客户端
    /// </summary>
    public abstract class DistributedClient
    {
        /// <summary>
        /// 客户端集合
        /// </summary>
        protected readonly Dictionary<uint, KeyValue<DiskBlockClient, Task<DiskBlockClient>>> clients;
        /// <summary>
        /// 分布式客户端
        /// </summary>
        private DistributedClient()
        {
            clients = DictionaryCreator<uint>.Create<KeyValue<DiskBlockClient, Task<DiskBlockClient>>>();
        }
        /// <summary>
        /// 根据磁盘块服务唯一编号创建客户端
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>磁盘块客户端</returns>
        protected abstract Task<DiskBlockClient> createClient(uint identity);
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns></returns>
        private async Task<DiskBlockClient> getClient(uint identity)
        {
            DiskBlockClient client = await createClient(identity);
            clients.Add(identity, new KeyValue<DiskBlockClient, Task<DiskBlockClient>>(client, Task.FromResult(client)));
            return client;
        }
        /// <summary>
        /// 根据磁盘块服务唯一编号获取客户端
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>磁盘块客户端</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<DiskBlockClient> GetClient(uint identity)
        {
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            return clients.TryGetValue(identity, out client) ? client.Value : getClient(identity);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task Write(uint identity, SubArray<byte> data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
                returnType = CommandClientReturnTypeEnum.Success;
                await client.Key.Write(data, callback);
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<BlockIndex> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteAsync(uint identity, SubArray<byte> data)
        {
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
            return await client.Key.WriteAsync(data);
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteString(uint identity, string data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
                returnType = CommandClientReturnTypeEnum.Success;
                await client.Key.WriteString(data, callback);
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) callback(new CommandClientReturnValue<BlockIndex> { ReturnType = returnType });
            }
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteStringAsync(uint identity, string data)
        {
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
            return await client.Key.WriteStringAsync(data);
        }
        /// <summary>
        /// 写入 JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteJson<T>(uint identity, T data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
                returnType = CommandClientReturnTypeEnum.Success;
                await client.Key.WriteJson(data, callback);
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
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteJsonAsync<T>(uint identity, T data)
        {
            if (data == null) return BlockIndex.JsonNull;
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
            return await client.Key.WriteJsonAsync(data);
        }
        /// <summary>
        /// 写入 JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteJsonMemberMap<T>(uint identity, AutoCSer.Metadata.MemberMapValue<T> data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
                returnType = CommandClientReturnTypeEnum.Success;
                await client.Key.WriteJsonMemberMap(data, callback);
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
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteJsonMemberMapAsync<T>(uint identity, AutoCSer.Metadata.MemberMapValue<T> data)
        {
            if (data.Value == null) return BlockIndex.JsonNull;
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
            return await client.Key.WriteJsonMemberMapAsync(data);
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteBinary<T>(uint identity, T data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
                returnType = CommandClientReturnTypeEnum.Success;
                await client.Key.WriteBinary(data, callback);
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
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteBinaryAsync<T>(uint identity, T data)
        {
            if (data == null) return new BlockIndex(BinarySerializer.NullValue, -4);
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
            return await client.Key.WriteBinaryAsync(data);
        }
        /// <summary>
        /// 写入二进制序列化数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        public async Task WriteBinaryMemberMap<T>(uint identity, AutoCSer.Metadata.MemberMapValue<T> data, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
                returnType = CommandClientReturnTypeEnum.Success;
                await client.Key.WriteBinaryMemberMap(data, callback);
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
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="data">数据</param>
        /// <returns>写入数据起始位置</returns>
        public async Task<CommandClientReturnValue<BlockIndex>> WriteBinaryMemberMapAsync<T>(uint identity, AutoCSer.Metadata.MemberMapValue<T> data)
        {
            if (data.Value == null) return new BlockIndex(BinarySerializer.NullValue, -4);
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
            return await client.Key.WriteBinaryMemberMapAsync(data);
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
#if NetStandard21
            ReadResult<byte[]?> result = default(ReadResult<byte[]?>);
#else
            ReadResult<byte[]> result = default(ReadResult<byte[]>);
#endif
            try
            {
                if (!index.GetResult(out result))
                {
                    var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                    if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
                    result.ReturnType = CommandClientReturnTypeEnum.Success;
                    await client.Key.Read(index, callback);
                }
            }
            finally
            {
                if (result.ReturnType != CommandClientReturnTypeEnum.Success) callback(result);
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public async Task<ReadResult<byte[]?>> ReadAsync(BlockIndex index)
#else
        public async Task<ReadResult<byte[]>> ReadAsync(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<byte[]?> result;
#else
            ReadResult<byte[]> result;
#endif
            if (index.GetResult(out result)) return result;
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
            return await client.Key.ReadAsync(index);
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
#if NetStandard21
            ReadResult<string?> result = default(ReadResult<string?>);
#else
            ReadResult<string> result = default(ReadResult<string>);
#endif
            try
            {
                if (!index.GetResult(out result))
                {
                    var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                    if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
                    result.ReturnType = CommandClientReturnTypeEnum.Success;
                    await client.Key.ReadString(index, callback);
                }
            }
            finally
            {
                if (result.ReturnType != CommandClientReturnTypeEnum.Success) callback(result);
            }
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public async Task<ReadResult<string?>> ReadStringAsync(BlockIndex index)
#else
        public async Task<ReadResult<string>> ReadStringAsync(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<string?> result;
#else
            ReadResult<string> result;
#endif
            if (index.GetResult(out result)) return result;
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
            return await client.Key.ReadStringAsync(index);
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
#if NetStandard21
            ReadResult<T?> result = default(ReadResult<T?>);
#else
            ReadResult<T> result = default(ReadResult<T>);
#endif
            try
            {
                if (!index.GetJsonResult(out result))
                {
                    var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                    if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
                    result.ReturnType = CommandClientReturnTypeEnum.Success;
                    await client.Key.ReadJson(index, callback);
                }
            }
            finally
            {
                if (result.ReturnType != CommandClientReturnTypeEnum.Success) callback(result);
            }
        }
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public async Task<ReadResult<T?>> ReadJsonAsync<T>(BlockIndex index)
#else
        public async Task<ReadResult<T>> ReadJsonAsync<T>(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<T?> result;
#else
            ReadResult<T> result;
#endif
            if (index.GetJsonResult(out result)) return result;
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
            return await client.Key.ReadJsonAsync<T>(index);
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
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result = default(ReadResult<AutoCSer.Metadata.MemberMapValue<T>>);
            try
            {
                if (!index.GetJsonResult(out result))
                {
                    var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                    if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
                    result.ReturnType = CommandClientReturnTypeEnum.Success;
                    await client.Key.ReadJsonMemberMap(index, callback);
                }
            }
            finally
            {
                if (result.ReturnType != CommandClientReturnTypeEnum.Success) callback(result);
            }
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
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
            return await client.Key.ReadJsonMemberMapAsync<T>(index);
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
#if NetStandard21
            ReadResult<T?> result = default(ReadResult<T?>);
#else
            ReadResult<T> result = default(ReadResult<T>);
#endif
            try
            {
                if (!index.GetBinaryResult(out result))
                {
                    var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                    if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
                    result.ReturnType = CommandClientReturnTypeEnum.Success;
                    await client.Key.ReadBinary(index, callback);
                }
            }
            finally
            {
                if (result.ReturnType != CommandClientReturnTypeEnum.Success) callback(result);
            }
        }
        /// <summary>
        /// 读取二进制序列化对象（适合定义稳定不变的对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>读取数据结果</returns>
#if NetStandard21
        public async Task<ReadResult<T?>> ReadBinaryAsync<T>(BlockIndex index)
#else
        public async Task<ReadResult<T>> ReadBinaryAsync<T>(BlockIndex index)
#endif
        {
#if NetStandard21
            ReadResult<T?> result;
#else
            ReadResult<T> result;
#endif
            if (index.GetBinaryResult(out result)) return result;
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
            return await client.Key.ReadBinaryAsync<T>(index);
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
            ReadResult<AutoCSer.Metadata.MemberMapValue<T>> result = default(ReadResult<AutoCSer.Metadata.MemberMapValue<T>>);
            try
            {
                if (!index.GetBinaryResult(out result))
                {
                    var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                    if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
                    result.ReturnType = CommandClientReturnTypeEnum.Success;
                    await client.Key.ReadBinaryMemberMap(index, callback);
                }
            }
            finally
            {
                if (result.ReturnType != CommandClientReturnTypeEnum.Success) callback(result);
            }
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
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(index.Identity, out client)) client.Key = await getClient(index.Identity);
            return await client.Key.ReadBinaryMemberMapAsync<T>(index);
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
                var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
                if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
                returnType = CommandClientReturnTypeEnum.Success;
                await client.Key.SwitchBlock(identity, callback);
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
        public async Task<CommandClientReturnValue<long>> SwitchBlockAsync(uint identity)
        {
            var client = default(KeyValue<DiskBlockClient, Task<DiskBlockClient>>);
            if (!clients.TryGetValue(identity, out client)) client.Key = await getClient(identity);
            return await client.Key.SwitchBlockAsync(identity);
        }
    }
}
