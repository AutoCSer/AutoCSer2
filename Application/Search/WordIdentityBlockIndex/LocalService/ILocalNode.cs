using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search.WordIdentityBlockIndex
{
    /// <summary>
    /// Word segmentation result disk block index information node interface
    /// 分词结果磁盘块索引信息节点接口
    /// </summary>
    /// <typeparam name="T">Keyword type for word segmentation data
    /// 分词数据关键字类型</typeparam>
    [ServerNode(IsClient = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface ILocalNode<T> : IWordIdentityBlockIndexNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
    }
}
