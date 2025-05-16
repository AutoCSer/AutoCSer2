using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表缓存池配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LinkPoolAttribute : Attribute
    {
        /// <summary>
        /// 最大缓存对象数量，默认为  AutoCSer.Common.ProcessorCount * 2
        /// </summary>
        public int MaxObjectCount = AutoCSer.Common.ProcessorCount * 2;
        /// <summary>
        /// 释放空闲缓存对象定时间隔秒数，默认为 3600s
        /// </summary>
        public int ReleaseFreeTimeoutSeconds = 60 * 60;
        /// <summary>
        /// 是否添加到公共清除缓存数据，默认为 true
        /// </summary>
        public bool IsClearCache = true;

        /// <summary>
        /// 链表缓存池参数
        /// </summary>
        internal LinkPoolParameter Parameter
        {
            get
            {
                return new LinkPoolParameter
                {
                    MaxObjectCount = MaxObjectCount,
                    ReleaseFreeTimeoutSeconds = ReleaseFreeTimeoutSeconds,
                    IsClearCache = IsClearCache,
                };
            }
        }
    }
}
