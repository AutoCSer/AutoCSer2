using System;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 带移除标记的可重用哈希索引节点接口 客户端节点接口
    /// </summary>
    public partial interface IRemoveMarkHashKeyIndexNodeClientNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
    }
}
