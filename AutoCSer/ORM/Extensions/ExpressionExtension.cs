using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.Extensions
{
    /// <summary>
    /// 表达式扩展
    /// </summary>
    internal static class ExpressionExtension
    {
        /// <summary>
        /// 是否简单表达式（不需要括号）
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="isAddCalculateRight">是否加减运算右侧表达式</param>
        /// <returns></returns>
        internal static bool isSimple(this System.Linq.Expressions.Expression expression, bool isAddCalculateRight = false)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                case ExpressionType.MemberAccess:
                case ExpressionType.Coalesce:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return true;
                case ExpressionType.Unbox:
                case ExpressionType.UnaryPlus:
                    return isSimple(((UnaryExpression)expression).Operand);
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return !isAddCalculateRight;
                case ExpressionType.Call:
                    MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
                    System.Reflection.MethodInfo method = methodCallExpression.Method;
                    if (method.ReflectedType == typeof(SQLExpression))
                    {
                        switch (method.Name.Length)
                        {
                            case 2:
                                if (method.Name == nameof(SQLExpression.In)) return false;
                                break;
                            case 4:
                                if (method.Name == nameof(SQLExpression.Case) || method.Name == nameof(SQLExpression.Like)) return false;
                                break;
                            case 5:
                                if (method.Name == nameof(SQLExpression.NotIn)) return false;
                                break;
                            case 6:
                                if (method.Name == nameof(SQLExpression.LikeOr)) return false;
                                break;
                            case 7:
                                if (method.Name == nameof(SQLExpression.NotLike)) return false;
                                break;
                            case 8:
                                if (method.Name == nameof(SQLExpression.EndsWith)) return false;
                                break;
                            case 9:
                                if (method.Name == nameof(SQLExpression.CompareTo)) return false;
                                break;
                            case 10:
                                if (method.Name == nameof(SQLExpression.StartsWith)) return false;
                                break;
                            case 11:
                                if (method.Name == nameof(SQLExpression.NotEndsWith)) return false;
                                break;
                            case 13:
                                if (method.Name == nameof(SQLExpression.NotStartsWith)) return false;
                                break;
                        }
                    }
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 是否非逻辑的简单表达式（不需要括号）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal static bool isSimpleNotLogic(this System.Linq.Expressions.Expression expression)
        {
            if (!isSimple(expression)) return false;
            if (expression.NodeType != ExpressionType.Call) return true;

            System.Reflection.MethodInfo method = ((MethodCallExpression)expression).Method;
            return method.ReflectedType != typeof(SQLExpression) || method.ReturnType != typeof(bool);
        }
        /// <summary>
        /// 获取常量表达式常量值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static object? getConstantValue(this System.Linq.Expressions.Expression expression)
#else
        internal static object getConstantValue(this System.Linq.Expressions.Expression expression)
#endif
        {
            return ((ConstantExpression)expression).Value;
        }
        /// <summary>
        /// 判断是否常量表达式 null 值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool isConstantNull(this System.Linq.Expressions.Expression expression)
        {
            return expression.NodeType == ExpressionType.Constant && getConstantValue(expression) == null;
        }
        ///// <summary>
        ///// 判断是否 DateTime 常量表达式
        ///// </summary>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static bool isConstantDateTime(this System.Linq.Expressions.Expression expression)
        //{
        //    if (expression.NodeType == ExpressionType.Constant) 
        //    {
        //        object value = getConstantValue(expression);
        //        if(value != null)
        //        {
        //            Type type = value.GetType();
        //            return type == typeof(DateTime) || type == typeof(DateTime?);
        //        }
        //    }
        //    return false;
        //}
    }
}
