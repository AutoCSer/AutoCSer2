using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Node method information
    /// 节点方法信息
    /// </summary>
    public sealed class ServerNodeMethodInfo
    {
        /// <summary>
        /// The method number for initialize and load the persistent data
        /// 初始化加载持久化数据的方法编号
        /// </summary>
        public readonly int LoadPersistenceMethodIndex;
        /// <summary>
        /// Node method information
        /// 节点方法信息
        /// </summary>
        /// <param name="loadPersistenceMethodIndex">The method number for initialize and load the persistent data
        /// 初始化加载持久化数据的方法编号</param>
        public ServerNodeMethodInfo(int loadPersistenceMethodIndex)
        {
            LoadPersistenceMethodIndex = loadPersistenceMethodIndex;
        }
    }
}
