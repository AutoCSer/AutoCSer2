using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 自增 ID 分段
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public partial struct IdentityFragment
    {
        /// <summary>
        /// 当前分配 ID
        /// </summary>
        private long identity;
        /// <summary>
        /// 当前分配 ID
        /// </summary>
        public long Identity
        {
            get { return identity; }
        }
        /// <summary>
        /// 剩余 ID 数量
        /// </summary>
        private int count;
        /// <summary>
        /// 剩余 ID 数量
        /// </summary>
        public int Count { get { return count; } }
        /// <summary>
        /// 自增 ID 分段
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="count"></param>
        internal IdentityFragment(ref long identity, int count)
        {
            this.identity = identity;
            this.count = count;
            identity += count;
        }
        /// <summary>
        /// 获取下一个自增ID
        /// </summary>
        /// <param name="identity">分配的分配 ID</param>
        /// <returns>返回 false 表示没有可分配 ID</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Next(out long identity)
        {
            if (count > 0)
            {
                identity = this.identity++;
                --count;
                return true;
            }
            identity = long.MinValue;
            return false;
        }
    }
}
