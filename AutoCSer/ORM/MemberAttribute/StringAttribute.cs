using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 字符串列自定义配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StringAttribute : MemberAttribute
    {
        /// <summary>
        /// 字符串是否ASCII
        /// </summary>
        public bool IsAscii;
        /// <summary>
        /// 是否固定长度
        /// </summary>
        public bool IsFixed;
        /// <summary>
        /// 是否允许空值
        /// </summary>
        public bool IsNullable;
        /// <summary>
        /// 是否允许空字符串，默认为 允许
        /// </summary>
        public bool IsEmpty = true;
        /// <summary>
        /// 数据长度，ASCII 最大 8000，否则最大 4000，默认为 0 表示 max
        /// </summary>
        public ushort Size;

        /// <summary>
        /// 自定义验证，验证失败需要抛出异常
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual string Verify(string value)
        {
            return value;
        }

        /// <summary>
        /// 默认字符串列自定义配置
        /// </summary>
        internal static readonly StringAttribute Default = new StringAttribute();
    }
}
