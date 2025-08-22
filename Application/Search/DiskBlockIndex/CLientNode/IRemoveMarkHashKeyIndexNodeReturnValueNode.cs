using System;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    public sealed partial class IRemoveMarkHashKeyIndexNodeReturnValueNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
    }
}
