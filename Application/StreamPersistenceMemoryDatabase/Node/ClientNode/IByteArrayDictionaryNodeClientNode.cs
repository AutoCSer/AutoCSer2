using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Dictionary node client interface
    /// 字典节点客户端接口
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    public partial interface IByteArrayDictionaryNodeClientNode<KT> where KT : IEquatable<KT>
    {
    }
}
