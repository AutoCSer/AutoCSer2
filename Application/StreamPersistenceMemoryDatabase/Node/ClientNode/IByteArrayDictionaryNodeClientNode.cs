using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点接口 客户端节点接口
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    public partial interface IByteArrayDictionaryNodeClientNode<KT> where KT : IEquatable<KT>
    {
    }
}
