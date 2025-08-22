using System;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    public sealed partial class IRemoveMarkHashIndexNodeReturnValueNode<KT, VT>
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
