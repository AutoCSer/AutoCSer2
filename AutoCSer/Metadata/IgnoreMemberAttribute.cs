using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// Ignore the members
    /// 忽略成员
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public abstract class IgnoreMemberAttribute : Attribute
    {
        /// <summary>
        /// Is ignore the current member
        /// 是否忽略当前成员
        /// </summary>
        public bool IsIgnoreCurrent;
        /// <summary>
        /// Is ignore the current member
        /// 是否忽略当前成员
        /// </summary>
        internal virtual bool GetIsIgnoreCurrent
        {
            get { return IsIgnoreCurrent; }
        }
    }
}
