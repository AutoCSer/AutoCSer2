//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateLong(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateLong(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (long)left + (long)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (long)left + (long)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (long)left - (long)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (long)left - (long)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (long)left * (long)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (long)left * (long)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (long)left / (long)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (long)left % (long)right;
                case System.Linq.Expressions.ExpressionType.Or: return (long)left | (long)right;
                case System.Linq.Expressions.ExpressionType.And: return (long)left & (long)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (long)left ^ (long)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (long)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (long)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateUInt(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateUInt(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (uint)left + (uint)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (uint)left + (uint)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (uint)left - (uint)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (uint)left - (uint)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (uint)left * (uint)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (uint)left * (uint)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (uint)left / (uint)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (uint)left % (uint)right;
                case System.Linq.Expressions.ExpressionType.Or: return (uint)left | (uint)right;
                case System.Linq.Expressions.ExpressionType.And: return (uint)left & (uint)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (uint)left ^ (uint)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (uint)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (uint)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateInt(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateInt(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (int)left + (int)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (int)left + (int)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (int)left - (int)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (int)left - (int)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (int)left * (int)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (int)left * (int)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (int)left / (int)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (int)left % (int)right;
                case System.Linq.Expressions.ExpressionType.Or: return (int)left | (int)right;
                case System.Linq.Expressions.ExpressionType.And: return (int)left & (int)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (int)left ^ (int)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (int)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (int)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateUShort(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateUShort(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (ushort)left + (ushort)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (ushort)left + (ushort)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (ushort)left - (ushort)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (ushort)left - (ushort)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (ushort)left * (ushort)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (ushort)left * (ushort)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (ushort)left / (ushort)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (ushort)left % (ushort)right;
                case System.Linq.Expressions.ExpressionType.Or: return (ushort)left | (ushort)right;
                case System.Linq.Expressions.ExpressionType.And: return (ushort)left & (ushort)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (ushort)left ^ (ushort)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (ushort)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (ushort)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateShort(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateShort(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (short)left + (short)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (short)left + (short)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (short)left - (short)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (short)left - (short)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (short)left * (short)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (short)left * (short)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (short)left / (short)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (short)left % (short)right;
                case System.Linq.Expressions.ExpressionType.Or: return (short)left | (short)right;
                case System.Linq.Expressions.ExpressionType.And: return (short)left & (short)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (short)left ^ (short)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (short)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (short)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateByte(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateByte(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (byte)left + (byte)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (byte)left + (byte)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (byte)left - (byte)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (byte)left - (byte)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (byte)left * (byte)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (byte)left * (byte)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (byte)left / (byte)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (byte)left % (byte)right;
                case System.Linq.Expressions.ExpressionType.Or: return (byte)left | (byte)right;
                case System.Linq.Expressions.ExpressionType.And: return (byte)left & (byte)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (byte)left ^ (byte)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (byte)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (byte)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateSByte(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateSByte(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (sbyte)left + (sbyte)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (sbyte)left + (sbyte)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (sbyte)left - (sbyte)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (sbyte)left - (sbyte)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (sbyte)left * (sbyte)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (sbyte)left * (sbyte)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (sbyte)left / (sbyte)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (sbyte)left % (sbyte)right;
                case System.Linq.Expressions.ExpressionType.Or: return (sbyte)left | (sbyte)right;
                case System.Linq.Expressions.ExpressionType.And: return (sbyte)left & (sbyte)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (sbyte)left ^ (sbyte)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (sbyte)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (sbyte)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateChar(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateChar(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (char)left + (char)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (char)left + (char)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (char)left - (char)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (char)left - (char)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (char)left * (char)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (char)left * (char)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (char)left / (char)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (char)left % (char)right;
                case System.Linq.Expressions.ExpressionType.Or: return (char)left | (char)right;
                case System.Linq.Expressions.ExpressionType.And: return (char)left & (char)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (char)left ^ (char)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (char)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (char)left >> (int)right;
                default: return null;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareLong(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((long)left) >= ((long)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((long)left) > ((long)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareUInt(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((uint)left) >= ((uint)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((uint)left) > ((uint)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareInt(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((int)left) >= ((int)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((int)left) > ((int)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareUShort(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((ushort)left) >= ((ushort)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((ushort)left) > ((ushort)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareShort(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((short)left) >= ((short)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((short)left) > ((short)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareByte(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((byte)left) >= ((byte)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((byte)left) > ((byte)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareSByte(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((sbyte)left) >= ((sbyte)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((sbyte)left) > ((sbyte)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareDouble(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((double)left) >= ((double)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((double)left) > ((double)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareFloat(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((float)left) >= ((float)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((float)left) > ((float)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareDecimal(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((decimal)left) >= ((decimal)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((decimal)left) > ((decimal)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareDateTime(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((DateTime)left) >= ((DateTime)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((DateTime)left) > ((DateTime)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static AutoCSer.ORM.ConditionExpression.LogicTypeEnum compareTimeSpan(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((TimeSpan)left) >= ((TimeSpan)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((TimeSpan)left) > ((TimeSpan)right) ? AutoCSer.ORM.ConditionExpression.LogicTypeEnum.True : AutoCSer.ORM.ConditionExpression.LogicTypeEnum.False;
                default: return AutoCSer.ORM.ConditionExpression.LogicTypeEnum.Unknown;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateFloat(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateFloat(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (float)left + (float)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (float)left + (float)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (float)left - (float)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (float)left - (float)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (float)left * (float)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (float)left * (float)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (float)left / (float)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (float)left % (float)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        private static object? calculateDecimal(System.Linq.Expressions.ExpressionType type, object left, object right)
#else
        private static object calculateDecimal(System.Linq.Expressions.ExpressionType type, object left, object right)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (decimal)left + (decimal)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (decimal)left + (decimal)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (decimal)left - (decimal)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (decimal)left - (decimal)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (decimal)left * (decimal)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (decimal)left * (decimal)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (decimal)left / (decimal)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (decimal)left % (decimal)right;
                default: return null;
            }
        }
    }
}

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
        private static object? calculateUInt(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateUInt(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(uint)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(uint)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateInt(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateInt(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(int)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(int)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateUShort(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateUShort(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(ushort)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(ushort)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateShort(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateShort(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(short)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(short)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateByte(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateByte(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(byte)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(byte)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateSByte(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateSByte(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(sbyte)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(sbyte)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateChar(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateChar(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(char)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(char)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateDouble(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateDouble(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(double)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(double)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateFloat(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateFloat(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(float)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(float)value; }
            }
            return null;
        }
    }
}

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
        private static object? calculateDecimal(System.Linq.Expressions.ExpressionType type, object value)
#else
        private static object calculateDecimal(System.Linq.Expressions.ExpressionType type, object value)
#endif
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(decimal)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(decimal)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateNotLong(object value)
        {
            return ~(long)value;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateNotUInt(object value)
        {
            return ~(uint)value;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateNotInt(object value)
        {
            return ~(int)value;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateNotUShort(object value)
        {
            return ~(ushort)value;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateNotShort(object value)
        {
            return ~(short)value;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateNotByte(object value)
        {
            return ~(byte)value;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateNotSByte(object value)
        {
            return ~(sbyte)value;
        }
    }
}

namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int readInt(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetInt32(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int? readIntNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetInt32(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int readIntObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(int)) return reader.GetInt32(index);
            if (reader.IsDBNull(index)) return default(int);
            return int.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int? readIntNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(int)) return reader.GetInt32(index);
            return int.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static int readInt(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadInt();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static int? readIntNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadIntNullable();
        }
    }
}
namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static short readShort(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetInt16(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static short? readShortNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetInt16(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static short readShortObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(short)) return reader.GetInt16(index);
            if (reader.IsDBNull(index)) return default(short);
            return short.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static short? readShortNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(short)) return reader.GetInt16(index);
            return short.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static short readShort(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadShort();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static short? readShortNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadShortNullable();
        }
    }
}
namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static byte readByte(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetByte(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static byte? readByteNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetByte(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static byte readByteObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(byte)) return reader.GetByte(index);
            if (reader.IsDBNull(index)) return default(byte);
            return byte.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static byte? readByteNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(byte)) return reader.GetByte(index);
            return byte.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static byte readByte(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadByte();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static byte? readByteNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadByteNullable();
        }
    }
}
namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool readBool(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetBoolean(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool? readBoolNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetBoolean(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool readBoolObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(bool)) return reader.GetBoolean(index);
            if (reader.IsDBNull(index)) return default(bool);
            return bool.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool? readBoolNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(bool)) return reader.GetBoolean(index);
            return bool.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool readBool(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadBool();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool? readBoolNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadBoolNullable();
        }
    }
}
namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static DateTime readDateTime(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetDateTime(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static DateTime? readDateTimeNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetDateTime(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static DateTime readDateTimeObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(DateTime)) return reader.GetDateTime(index);
            if (reader.IsDBNull(index)) return default(DateTime);
            return DateTime.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static DateTime? readDateTimeNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(DateTime)) return reader.GetDateTime(index);
            return DateTime.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DateTime readDateTime(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadDateTime();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DateTime? readDateTimeNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadDateTimeNullable();
        }
    }
}
namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static decimal readDecimal(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetDecimal(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static decimal? readDecimalNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetDecimal(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static decimal readDecimalObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(decimal)) return reader.GetDecimal(index);
            if (reader.IsDBNull(index)) return default(decimal);
            return decimal.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static decimal? readDecimalNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(decimal)) return reader.GetDecimal(index);
            return decimal.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static decimal readDecimal(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadDecimal();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static decimal? readDecimalNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadDecimalNullable();
        }
    }
}
namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static Guid readGuid(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetGuid(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static Guid? readGuidNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetGuid(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static Guid readGuidObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(Guid)) return reader.GetGuid(index);
            if (reader.IsDBNull(index)) return default(Guid);
            return Guid.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static Guid? readGuidNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(Guid)) return reader.GetGuid(index);
            return Guid.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Guid readGuid(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadGuid();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Guid? readGuidNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadGuidNullable();
        }
    }
}
namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static double readDouble(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetDouble(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static double? readDoubleNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetDouble(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static double readDoubleObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(double)) return reader.GetDouble(index);
            if (reader.IsDBNull(index)) return default(double);
            return double.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static double? readDoubleNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(double)) return reader.GetDouble(index);
            return double.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static double readDouble(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadDouble();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static double? readDoubleNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadDoubleNullable();
        }
    }
}
namespace AutoCSer.ORM
{
    /// <summary>
    /// 字段成员
    /// </summary>
    internal sealed partial class Member
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static float readFloat(System.Data.Common.DbDataReader reader, int index)
        {
            return reader.GetFloat(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static float? readFloatNullable(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetFloat(index);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static float readFloatObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.GetFieldType(index) == typeof(float)) return reader.GetFloat(index);
            if (reader.IsDBNull(index)) return default(float);
            return float.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static float? readFloatNullableObject(System.Data.Common.DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            if (reader.GetFieldType(index) == typeof(float)) return reader.GetFloat(index);
            return float.Parse(AutoCSer.Extensions.NullableReferenceExtension.notNull(reader[index].ToString()));
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static float readFloat(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadFloat();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static float? readFloatNullable(AutoCSer.ORM.RemoteProxy.DataValue[] row, int index)
        {
            return row[index].ReadFloatNullable();
        }
    }
}
namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int ReadInt()
        {
            return !isNull ? readInt() : default(int);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int? ReadIntNullable()
        {
            if (isNull) return null;
            return readInt();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal short ReadShort()
        {
            return !isNull ? readShort() : default(short);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal short? ReadShortNullable()
        {
            if (isNull) return null;
            return readShort();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte ReadByte()
        {
            return !isNull ? readByte() : default(byte);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte? ReadByteNullable()
        {
            if (isNull) return null;
            return readByte();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool ReadBool()
        {
            return !isNull ? readBool() : default(bool);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool? ReadBoolNullable()
        {
            if (isNull) return null;
            return readBool();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal DateTime ReadDateTime()
        {
            return !isNull ? readDateTime() : default(DateTime);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal DateTime? ReadDateTimeNullable()
        {
            if (isNull) return null;
            return readDateTime();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal DateTimeOffset ReadDateTimeOffset()
        {
            return !isNull ? readDateTimeOffset() : default(DateTimeOffset);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal DateTimeOffset? ReadDateTimeOffsetNullable()
        {
            if (isNull) return null;
            return readDateTimeOffset();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal TimeSpan ReadTimeSpan()
        {
            return !isNull ? readTimeSpan() : default(TimeSpan);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal TimeSpan? ReadTimeSpanNullable()
        {
            if (isNull) return null;
            return readTimeSpan();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal decimal ReadDecimal()
        {
            return !isNull ? readDecimal() : default(decimal);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal decimal? ReadDecimalNullable()
        {
            if (isNull) return null;
            return readDecimal();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Guid ReadGuid()
        {
            return !isNull ? readGuid() : default(Guid);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Guid? ReadGuidNullable()
        {
            if (isNull) return null;
            return readGuid();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal double ReadDouble()
        {
            return !isNull ? readDouble() : default(double);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal double? ReadDoubleNullable()
        {
            if (isNull) return null;
            return readDouble();
        }
    }
}

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal float ReadFloat()
        {
            return !isNull ? readFloat() : default(float);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal float? ReadFloatNullable()
        {
            if (isNull) return null;
            return readFloat();
        }
    }
}

#endif