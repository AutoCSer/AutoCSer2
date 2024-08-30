using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 从服务节点加载数据接口
    /// </summary>
    internal interface ISlaveLoader
    {
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        void GetPersistenceFile(long position, ref SubArray<byte> buffer);
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        void GetPersistenceCallbackExceptionPositionFile(long position, ref SubArray<byte> buffer);
    }
}
