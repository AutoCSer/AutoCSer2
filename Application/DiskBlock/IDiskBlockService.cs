using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 磁盘块服务接口
    /// </summary>
    public interface IDiskBlockService
    {
        /// <summary>
        /// 获取磁盘块当前写入位置
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">磁盘块当前写入位置</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void GetPosition(CommandServerSocket socket, CommandServerCallQueue queue, uint identity, CommandServerKeepCallback<long> callback);
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">写入数据缓冲区，来源于同步 IO 数据缓冲区，需要同步处理数据，否则需要复制数据</param>
        /// <param name="callback">写入数据起始位置</param>
        void Write(CommandServerSocket socket, WriteBuffer buffer, CommandServerCallback<BlockIndex> callback);
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        void Read(CommandServerSocket socket, CommandServerCallQueue queue, BlockIndex index, CommandServerCallback<ReadBuffer> callback);

        /// <summary>
        /// 获取磁盘块信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>null 表示失败</returns>
#if NetStandard21
        BlockInfo[]? GetBlocks(CommandServerSocket socket, uint identity);
#else
        BlockInfo[] GetBlocks(CommandServerSocket socket, uint identity);
#endif
        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">磁盘块当前写入位置</param>
        void SwitchBlock(CommandServerSocket socket, uint identity, CommandServerCallback<BlockIndex> callback);
        /// <summary>
        /// 删除垃圾磁盘块（只有在确认不再存在引用的情况下才删除）
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="startIndex">磁盘块起始位置，不允许删除当前写操作的磁盘块</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteBlock(CommandServerSocket socket, uint identity, long startIndex);
    }
}
