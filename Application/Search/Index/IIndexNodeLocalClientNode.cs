using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 关键字索引节点接口 客户端节点接口
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    public partial interface IIndexNodeLocalClientNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
    }
}
