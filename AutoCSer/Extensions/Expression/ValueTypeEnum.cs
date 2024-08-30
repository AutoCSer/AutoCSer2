using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 表达式取值类型
    /// </summary>
    internal enum ValueTypeEnum : byte
    {
        /// <summary>
        /// 成员
        /// </summary>
        Member,
        /// <summary>
        /// 方法调用
        /// </summary>
        Call,
        /// <summary>
        /// 下一个成员 .
        /// </summary>
        NextMember,
        /// <summary>
        /// 非 null 成员 ?.
        /// </summary>
        IfNotNullMember,
        /// <summary>
        /// 客户端成员 # / $
        /// </summary>
        Client,
        /// <summary>
        /// 客户端编码调用 $ToHtml()
        /// </summary>
        ClientEncode,

        /// <summary>
        /// 字符串 "
        /// </summary>
        String,
        /// <summary>
        /// 十进制整数
        /// </summary>
        Decimalism,
        /// <summary>
        /// 十六进制整数 0x
        /// </summary>
        Hex,
        /// <summary>
        /// 带符号十进制整数
        /// </summary>
        SignedDecimalism,
        /// <summary>
        /// 带符号十六进制整数 0x
        /// </summary>
        SignedHex,
        /// <summary>
        /// 小数 .
        /// </summary>
        Decimal,
        /// <summary>
        /// 带符号小数 .
        /// </summary>
        SignedDecimal,
        /// <summary>
        /// 逻辑真值 true
        /// </summary>
        True,
        /// <summary>
        /// 逻辑假值 false
        /// </summary>
        False,

        /// <summary>
        /// 小于等于 
        /// </summary>
        LessOrEqual,
        /// <summary>
        /// 小于
        /// </summary>
        Less,
        /// <summary>
        /// 不等于 !=
        /// </summary>
        NotEqual,
        /// <summary>
        /// 等于 ==
        /// </summary>
        Equal,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterOrEqual,
        /// <summary>
        /// 大于
        /// </summary>
        Greater,

        /// <summary>
        /// 逻辑与
        /// </summary>
        And,
        /// <summary>
        /// 逻辑或 ||
        /// </summary>
        Or,
        /// <summary>
        /// 取反 !
        /// </summary>
        Not,

        /// <summary>
        /// 加上 +
        /// </summary>
        Add,
        /// <summary>
        /// 减去 -
        /// </summary>
        Subtract,
        /// <summary>
        /// 乘以 *
        /// </summary>
        Multiply,
        /// <summary>
        /// 除以 /
        /// </summary>
        Divide,
        /// <summary>
        /// 取余数 %
        /// </summary>
        Mod,
        /// <summary>
        /// 左移
        /// </summary>
        LeftShift,
        /// <summary>
        /// 右移
        /// </summary>
        RightShift,
        /// <summary>
        /// 二进制位与
        /// </summary>
        BitAnd,
        /// <summary>
        /// 二进制或 |
        /// </summary>
        BitOr,
        /// <summary>
        /// 二进制亦或 ^
        /// </summary>
        Xor,

        /// <summary>
        /// null 值判定 ??
        /// </summary>
        IfNullThen,
        /// <summary>
        /// 小括号 (
        /// </summary>
        Parenthesis,
        /// <summary>
        /// 三元取值 ?:
        /// </summary>
        IfElse,
        /// <summary>
        /// 索引调用
        /// </summary>
        Index,
        /// <summary>
        /// 非 null 索引 ?[
        /// </summary>
        IfNotNullIndex,
    }
}
