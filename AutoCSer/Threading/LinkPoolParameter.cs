using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 默认链表缓存池参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct LinkPoolParameter
    {
        /// <summary>
        /// 最大缓存对象数量，默认为  AutoCSer.Common.ProcessorCount * 2
        /// </summary>
        public int MaxObjectCount;
        /// <summary>
        /// 释放空闲缓存对象定时间隔秒数，默认为 3600s
        /// </summary>
        public int ReleaseFreeTimeoutSeconds;
        ///// <summary>
        ///// 是否添加到公共清除缓存数据，默认为 true
        ///// </summary>
        //public bool IsClearCache;
        ///// <summary>
        ///// 是否默认缓存环池构造函数传参参数
        ///// </summary>
        //internal bool IsDefault;

        /// <summary>
        /// 默认链表缓存池参数
        /// </summary>
        public static LinkPoolParameter Default
        {
            get
            {
                return new LinkPoolParameter
                {
                    MaxObjectCount = AutoCSer.Common.ProcessorCount * 2,
                    ReleaseFreeTimeoutSeconds = 60 * 60,
                    //IsClearCache = true,
                    //IsDefault = true
                };
            }
        }
    }
}
