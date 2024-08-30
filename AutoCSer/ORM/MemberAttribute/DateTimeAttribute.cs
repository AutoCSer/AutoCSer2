using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 日期时间列自定义配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DateTimeAttribute : MemberAttribute
    {
        /// <summary>
        /// 日期时间类型
        /// </summary>
        public DateTimeTypeEnum Type;

        /// <summary>
        /// 自定义验证，验证失败需要抛出异常
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual DateTime Verify(DateTime value) { return value; }

        /// <summary>
        /// 最小时间
        /// </summary>
        public static readonly DateTime MinDateTime = new DateTime(1753, 1, 1);
        /// <summary>
        /// 最小时间
        /// </summary>
        public static readonly DateTime MinSmallDateTime = new DateTime(1900, 1, 1);
        /// <summary>
        /// 最大时间
        /// </summary>
        public static readonly DateTime MaxSmallDateTime = new DateTime(2079, 6, 6);

        /// <summary>
        /// 默认日期时间列自定义配置
        /// </summary>
        internal static readonly DateTimeAttribute Default = new DateTimeAttribute();
    }
}
