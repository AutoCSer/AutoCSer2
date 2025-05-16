using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 默认分块缓存池构造函数传参参数
    /// </summary>
    public struct BlockPoolParameter
    {
        /// <summary>
        /// 每个缓存分块包含 CPU 高速缓存块数量，默认为 1，CPU 高速缓存块默认大小为 64B
        /// </summary>
        public int CpuCacheBlockCountPerPoolBlock;
        /// <summary>
        /// 缓存分块数量，默认为 4, 必须大于等于 3，0 表示不启用默认缓存（总共可缓存区对象数量为 CpuCacheBlockCountPerPoolBlock * BlockCount * 8）
        /// </summary>
        public int BlockCount;
        /// <summary>
        /// 释放空闲缓存对象定时间隔秒数，默认为 3600s
        /// </summary>
        public int ReleaseFreeTimeoutSeconds;
        /// <summary>
        /// 是否添加到公共清除缓存数据，默认为 true
        /// </summary>
        public bool IsClearCache;

        /// <summary>
        /// 默认分块缓存池构造函数传参参数
        /// </summary>
        public static BlockPoolParameter Default
        {
            get
            {
                return new BlockPoolParameter
                {
                    CpuCacheBlockCountPerPoolBlock = 1,
                    BlockCount = 4,
                    ReleaseFreeTimeoutSeconds = 60 * 60,
                    IsClearCache = true
                };
            }
        }
    }
}
