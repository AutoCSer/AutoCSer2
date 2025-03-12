using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search.WordIdentityBlockIndex
{
    /// <summary>
    /// 分词结果磁盘块索引信息节点接口
    /// </summary>
    /// <typeparam name="T">分词数据关键字类型</typeparam>
    [ServerNode(IsAutoMethodIndex = false, IsClient = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface ILocalNode<T> : IWordIdentityBlockIndexNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
    }
}
