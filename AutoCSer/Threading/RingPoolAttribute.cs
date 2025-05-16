using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 缓存环池配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RingPoolAttribute : Attribute
    {
        /// <summary>
        /// 缓存分块数量二进制位，默认为最小值 8，最大值为 17，0 表示不启用默认缓存
        /// </summary>
        public byte BlockCountBits = 8;
        /// <summary>
        /// 释放空闲缓存对象定时间隔秒数，默认为 3600s
        /// </summary>
        public int ReleaseFreeTimeoutSeconds = 60 * 60;
        /// <summary>
        /// 是否添加到公共清除缓存数据，默认为 true
        /// </summary>
        public bool IsClearCache = true;

        /// <summary>
        /// 默认缓存环池构造函数传参参数
        /// </summary>
        internal RingPoolParameter Parameter
        {
            get
            {
                return new RingPoolParameter
                {
                    BlockCountBits = BlockCountBits,
                    ReleaseFreeTimeoutSeconds = ReleaseFreeTimeoutSeconds,
                    //IsClearCache = IsClearCache,
                };
            }
        }
    }
}
