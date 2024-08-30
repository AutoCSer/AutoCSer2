using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// decimal 列自定义配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DecimalAttribute : MemberAttribute
    {
        /// <summary>
        /// 最大有效位数 38
        /// </summary>
        public const byte MaxPrecision = 38;

        /// <summary>
        /// 有效位数，默认为 18 位，最大值 38 位
        /// </summary>
        public byte Precision = 18;
        /// <summary>
        /// 小数点后有效位数，默认为 2 位
        /// </summary>
        public byte Scale = 2;

        /// <summary>
        /// 整数位数
        /// </summary>
        internal int Integer
        {
            get { return Precision - Scale; }
        }
        /// <summary>
        /// 自定义验证，验证失败需要抛出异常
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual decimal Verify(decimal value) { return value; }

        /// <summary>
        /// 默认 decimal 列自定义配置
        /// </summary>
        internal static readonly DecimalAttribute Default = new DecimalAttribute();
    }
}
