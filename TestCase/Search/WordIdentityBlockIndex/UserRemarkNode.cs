using System;
using AutoCSer.Net;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 用户备注分词结果磁盘块索引信息节点
    /// </summary>
    internal sealed class UserRemarkNode : UserNode
    {
        /// <summary>
        /// 用户备注分词结果磁盘块索引信息节点
        /// </summary>
        internal UserRemarkNode() : base(CommandClientSocketEvent.UserRemarkNodeCache, DiskBlockIndexCommandClientSocketEvent.UserRemarkDiskBlockIndexNodeCache) { }
        /// <summary>
        /// 获取初始化加载所有数据命令
        /// </summary>
        /// <returns></returns>
        protected override EnumeratorCommand<BinarySerializeKeyValue<int, string>> getLoadCommand()
        {
            return DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient?.GetAllUserRemark();
        }
        /// <summary>
        /// 根据关键字获取需要分词的文本数据
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <returns>null 表示没有找到关键字数据</returns>
        public override AutoCSer.Net.ReturnCommand<string> GetText(int key)
        {
            return DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient.GetRemark(key);
        }
    }
}
