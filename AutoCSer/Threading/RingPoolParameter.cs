using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 默认缓存环池构造函数传参参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RingPoolParameter
    {
        /// <summary>
        /// 缓存分块数量二进制位 最小值为 2，最大缓存对象数据为 8 * 4 = 32
        /// </summary>
        public const byte MinBlockCountBits = 2;
        /// <summary>
        /// 缓存分块数量二进制位 最大值为 17，最大缓存对象数据为 8 * 128K = 1M
        /// </summary>
        public const byte MaxBlockCountBits = 17;

        /// <summary>
        /// 缓存分块数量二进制位，默认为最小值 8，最大值为 17，0 表示不启用默认缓存
        /// </summary>
        public byte BlockCountBits;
        /// <summary>
        /// 缓存对象数量
        /// </summary>
        internal int CacheObjectCount
        {
            get
            {
                if (BlockCountBits != 0)
                {
                    return AutoCSer.Common.CpuCacheBlockObjectCount << Math.Min(Math.Max(BlockCountBits, MinBlockCountBits), MaxBlockCountBits);
                }
                return 0;
            }
        }
        /// <summary>
        /// 释放空闲缓存对象定时间隔秒数，默认为 3600s
        /// </summary>
        public int ReleaseFreeTimeoutSeconds;
        ///// <summary>
        ///// 是否添加到公共清除缓存数据，默认为 true
        ///// </summary>
        //public bool IsClearCache;
        /// <summary>
        /// 是否默认缓存环池构造函数传参参数
        /// </summary>
        internal bool IsDefault;

        /// <summary>
        /// 默认缓存环池构造函数传参参数
        /// </summary>
        public static RingPoolParameter Default
        {
            get
            {
                return new RingPoolParameter
                {
                    BlockCountBits = 8,
                    ReleaseFreeTimeoutSeconds = 60 * 60,
                    //IsClearCache = true,
                    IsDefault = true
                };
            }
        }
    }
}
