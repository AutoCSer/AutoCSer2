using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 磁盘块客户端接口转换封装
    /// </summary>
    public sealed class DiskBlockTaskClient : IDiskBlockClient
    {
        /// <summary>
        /// 磁盘块客户端接口
        /// </summary>
        private IDiskBlockTaskClient client;
        /// <summary>
        /// 磁盘块客户端接口转换封装
        /// </summary>
        /// <param name="client">磁盘块客户端接口</param>
        public DiskBlockTaskClient(IDiskBlockTaskClient client)
        {
            this.client = client;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入数据缓冲区</param>
        /// <returns>写入数据起始位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<BlockIndex> WaitWrite(WriteBuffer buffer)
        {
            return client.WaitWrite(buffer);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index"></param>
        /// <returns>读取数据缓冲区</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<ReadBuffer> WaitRead(ReadBuffer returnValue, BlockIndex index)
        {
            return client.WaitRead(returnValue, index);
        }
        /// <summary>
        /// 获取磁盘块信息集合
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>null 表示失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<BlockInfo[]> WaitGetBlocks(uint identity)
        {
            return client.WaitGetBlocks(identity);
        }
        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>磁盘块当前写入位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<BlockIndex> WaitSwitchBlock(uint identity)
        {
            return client.WaitSwitchBlock(identity);
        }
        /// <summary>
        /// 删除垃圾磁盘块（只有在确认不再存在引用的情况下才删除）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="startIndex">磁盘块起始位置，不允许删除当前写操作的磁盘块</param>
        /// <returns>是否删除成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<bool> WaitDeleteBlock(uint identity, long startIndex)
        {
            return client.WaitDeleteBlock(identity, startIndex);
        }

        /// <summary>
        /// 获取磁盘块当前写入位置
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeepCallbackCommand GetPosition(uint identity, Action<CommandClientReturnValue<long>, KeepCallbackCommand> callback)
        {
            return client.GetPosition(identity, callback);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入数据缓冲区</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand Write(WriteBuffer buffer, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            return client.Write(buffer, callback);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入数据缓冲区</param>
        /// <returns>写入数据起始位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<BlockIndex> Write(WriteBuffer buffer)
        {
            return client.Write(buffer);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index"></param>
        /// <param name="callback">读取数据缓冲区</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand Read(ReadBuffer returnValue, BlockIndex index, Action<CommandClientReturnValue<ReadBuffer>> callback)
        {
            return client.Read(returnValue, index, callback);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index"></param>
        /// <returns>读取数据缓冲区</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<ReadBuffer> Read(ReadBuffer returnValue, BlockIndex index)
        {
            return client.Read(returnValue, index);
        }
        /// <summary>
        /// 获取磁盘块信息集合
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">null 表示失败</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CallbackCommand GetBlocks(uint identity, Action<CommandClientReturnValue<BlockInfo[]?>> callback)
#else
        public CallbackCommand GetBlocks(uint identity, Action<CommandClientReturnValue<BlockInfo[]>> callback)
#endif
        {
            return client.GetBlocks(identity, callback);
        }
        /// <summary>
        /// 获取磁盘块信息集合
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>null 表示失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ReturnCommand<BlockInfo[]?> GetBlocks(uint identity)
#else
        public ReturnCommand<BlockInfo[]> GetBlocks(uint identity)
#endif
        {
            return client.GetBlocks(identity);
        }
        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">磁盘块当前写入位置</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand SwitchBlock(uint identity, Action<CommandClientReturnValue<BlockIndex>> callback)
        {
            return client.SwitchBlock(identity, callback);
        }
        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>磁盘块当前写入位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<BlockIndex> SwitchBlock(uint identity)
        {
            return client.SwitchBlock(identity);
        }
        /// <summary>
        /// 删除垃圾磁盘块（只有在确认不再存在引用的情况下才删除）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="startIndex">磁盘块起始位置，不允许删除当前写操作的磁盘块</param>
        /// <param name="callback">是否删除成功</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand DeleteBlock(uint identity, long startIndex, Action<CommandClientReturnValue<bool>> callback)
        {
            return client.DeleteBlock(identity, startIndex, callback);
        }
        /// <summary>
        /// 删除垃圾磁盘块（只有在确认不再存在引用的情况下才删除）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="startIndex">磁盘块起始位置，不允许删除当前写操作的磁盘块</param>
        /// <returns>是否删除成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<bool> DeleteBlock(uint identity, long startIndex)
        {
            return client.DeleteBlock(identity, startIndex);
        }
    }
}
