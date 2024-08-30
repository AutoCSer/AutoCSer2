using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 磁盘块客户端接口（IO 线程同步回调接口避免线程调度开销，必须保证客户端调用 await 后续操作无阻塞，否则可能严重影响性能甚至死锁，如果不能保证无阻塞环境请替换为 IDiskBlockTaskClient 接口避免死锁）
    /// </summary>
    public interface IDiskBlockClient : IDiskBlockClientBase
    {
        /// <summary>
        /// 获取磁盘块当前写入位置
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="callback"></param>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand GetPosition(uint identity, Action<CommandClientReturnValue<long>, KeepCallbackCommand> callback);
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入数据缓冲区</param>
        /// <param name="callback">写入数据起始位置</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand Write(WriteBuffer buffer, Action<CommandClientReturnValue<BlockIndex>> callback);
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入数据缓冲区</param>
        /// <returns>写入数据起始位置</returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<BlockIndex> Write(WriteBuffer buffer);
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index"></param>
        /// <param name="callback">读取数据缓冲区</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand Read(ReadBuffer returnValue, BlockIndex index, Action<CommandClientReturnValue<ReadBuffer>> callback);
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index"></param>
        /// <returns>读取数据缓冲区</returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<ReadBuffer> Read(ReadBuffer returnValue, BlockIndex index);
        /// <summary>
        /// 获取磁盘块信息集合
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">null 表示失败</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand GetBlocks(uint identity, Action<CommandClientReturnValue<BlockInfo[]>> callback);
        /// <summary>
        /// 获取磁盘块信息集合
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>null 表示失败</returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<BlockInfo[]> GetBlocks(uint identity);
        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">磁盘块当前写入位置</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand SwitchBlock(uint identity, Action<CommandClientReturnValue<BlockIndex>> callback);
        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>磁盘块当前写入位置</returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<BlockIndex> SwitchBlock(uint identity);
        /// <summary>
        /// 删除垃圾磁盘块（只有在确认不再存在引用的情况下才删除）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="startIndex">磁盘块起始位置，不允许删除当前写操作的磁盘块</param>
        /// <param name="callback">是否删除成功</param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand DeleteBlock(uint identity, long startIndex, Action<CommandClientReturnValue<bool>> callback);
        /// <summary>
        /// 删除垃圾磁盘块（只有在确认不再存在引用的情况下才删除）
        /// </summary>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="startIndex">磁盘块起始位置，不允许删除当前写操作的磁盘块</param>
        /// <returns>是否删除成功</returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        ReturnCommand<bool> DeleteBlock(uint identity, long startIndex);
    }
}
