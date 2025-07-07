using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Load the data interface from the slave node
    /// 从服务节点加载数据接口
    /// </summary>
    internal interface ISlaveLoader
    {
        /// <summary>
        /// Get the persistent file data
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        void GetPersistenceFile(long position, ref SubArray<byte> buffer);
        /// <summary>
        /// Get the file data of the persistent callback exception location
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        void GetPersistenceCallbackExceptionPositionFile(long position, ref SubArray<byte> buffer);
    }
}
