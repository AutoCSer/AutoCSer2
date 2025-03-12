using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 用户名称分词结果磁盘块索引信息节点
    /// </summary>
    internal sealed class UserNameNode : UserNode
    {
        /// <summary>
        /// 用户名称分词结果磁盘块索引信息节点
        /// </summary>
        internal UserNameNode() : base(LocalWordIdentityBlockIndexServiceConfig.UserNameNodeCache, LocalDiskBlockIndexServiceConfig.UserNameNodeCache) { }
        /// <summary>
        /// 获取初始化加载所有数据命令
        /// </summary>
        /// <returns></returns>
        protected override EnumeratorCommand<BinarySerializeKeyValue<int, string>> getLoadCommand()
        {
            return DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient?.GetAllUserName();
        }
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
