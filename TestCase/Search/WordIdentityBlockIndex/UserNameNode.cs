using AutoCSer.CommandService.Search;
using AutoCSer.CommandService;
using System;
using AutoCSer.CommandService.Search.DiskBlockIndex;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 用户名称分词结果磁盘块索引信息节点
    /// </summary>
    internal sealed class UserNameNode : UserNode
    {
        /// <summary>
        /// 获取分词结果磁盘块索引信息节点单例
        /// </summary>
        protected override AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>> loadClientNode { get { return CommandClientSocketEvent.UserNameNodeCache; } }
        /// <summary>
        /// 带移除标记的可重用哈希索引节点单例
        /// </summary>
        protected override StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashKeyIndexNodeClientNode<int>> diskBlockIndexNode { get { return DiskBlockIndexCommandClientSocketEvent.UserNameDiskBlockIndexNodeCache; } }
        /// <summary>
        /// 根据关键字获取需要分词的文本数据
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <returns>null 表示没有找到关键字数据</returns>
        public override AutoCSer.Net.ReturnCommand<string> GetText(int key)
        {
            return DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient.GetName(key);
        }
    }
}
