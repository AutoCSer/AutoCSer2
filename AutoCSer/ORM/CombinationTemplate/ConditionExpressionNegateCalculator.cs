using System;
/*long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte;char,Char;double,Double;float,Float;decimal,Decimal*/

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 计算器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateLong(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateLong(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(long)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(long)value; }
            }
            return null;
        }
    }
}
