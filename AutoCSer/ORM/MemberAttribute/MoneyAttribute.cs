using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// money 列自定义配置（decimal）
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MoneyAttribute : MemberAttribute
    {
        /// <summary>
        /// 默认为 false 有效范围（-922337203685477.5808 ~ 9223372036854775807）+ decimal(19,4) ，否则有效范围（-214748.3648 ~ 214748.3647）+ decimal(10,4)
        /// </summary>
        public bool IsSmall;
        /// <summary>
        /// 自定义验证，验证失败需要抛出异常
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual decimal Verify(decimal value) { return value; }

        /// <summary>
        /// 最小金额
        /// </summary>
        public const decimal MinSmallMoney = -214748.3648M;
        /// <summary>
        /// 最大金额
        /// </summary>
        public const decimal MaxSmallMoney = 214748.3647M;

        /// <summary>
        /// 默认 money 列自定义配置
        /// </summary>
        internal static readonly DecimalAttribute Default = new DecimalAttribute();
    }
}
