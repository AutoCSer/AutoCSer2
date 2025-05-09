using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 节点方法信息
    /// </summary>
    public sealed class ServerNodeMethodInfo
    {
        /// <summary>
        /// 冷启动加载持久化方法编号
        /// </summary>
        public readonly int LoadPersistenceMethodIndex;
        /// <summary>
        /// 节点方法信息
        /// </summary>
        /// <param name="loadPersistenceMethodIndex">冷启动加载持久化方法编号</param>
        public ServerNodeMethodInfo(int loadPersistenceMethodIndex)
        {
            LoadPersistenceMethodIndex = loadPersistenceMethodIndex;
        }
    }
}
