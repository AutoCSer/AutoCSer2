using System.Linq.Expressions;
using AutoCSer.ORM.ConditionExpression;
using AutoCSer.ORM.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct ConditionExpressionConverter
    {
        /// <summary>
        /// 条件表达式
        /// </summary>
        public System.Linq.Expressions.Expression Expression { get; private set; }
        /// <summary>
        /// 表达式类型
        /// </summary>
        internal ConvertTypeEnum Type;
        /// <summary>
        /// 不支持的表达式类型
        /// </summary>
        internal ExpressionType NotSupportType;
        /// <summary>
        /// 空引用类型
        /// </summary>
        internal ExceptionTypeEnum ExceptionType;
        ///// <summary>
        ///// 表达式转换是否错误
        ///// </summary>
        //internal bool ExpressionConverterError;
        ///// <summary>
        ///// 条件表达式
        ///// </summary>
        //internal Expression NullExpression
        //{
        //    get
        //    {
        //        return IsWhereTrue ? null : Expression;
        //    }
        //}
        ///// <summary>
        ///// 条件表达式是否为真
        ///// </summary>
        //internal bool IsWhereTrue
        //{
        //    get
        //    {
        //        if (Expression == null || object.ReferenceEquals(Expression, constantTrue)) return true;
        //        return Expression.NodeType == ExpressionType.Constant && (bool)Expression.GetConstantValue();
        //    }
        //}
        ///// <summary>
        ///// 条件表达式是否为假
        ///// </summary>
        //internal bool IsWhereFalse
        //{
        //    get
        //    {
        //        if (Expression == null) return false;
        //        if (object.ReferenceEquals(Expression, constantFalse)) return true;
        //        return Expression.NodeType == ExpressionType.Constant && !(bool)Expression.GetConstantValue();
        //    }
        //}
        ///// <summary>
        ///// 判断是否常量 null
        ///// </summary>
        //internal bool IsConstantNull
        //{
        //    get
        //    {
        //        return Expression.NodeType == ExpressionType.Constant && Expression.GetConstantValue() == null && Type != ConvertTypeEnum.Unknown;
        //    }
        //}
        ///// <summary>
        ///// 正常表达式
        ///// </summary>
        //public Expression NormalExpression
        //{
        //    get { return Type != ConvertTypeEnum.Unknown ? Expression : null; }
        //}
        /// <summary>
        /// 逻辑值类型
        /// </summary>
        public LogicTypeEnum LogicType
        {
            get
            {
                if (Type == ConvertTypeEnum.NotSupport) return LogicTypeEnum.NotSupport;
                if (object.ReferenceEquals(Expression, constantFalse)) return LogicTypeEnum.False;
                if (object.ReferenceEquals(Expression, constantTrue) || Expression == null) return LogicTypeEnum.True;
                if (Expression.NodeType == ExpressionType.Constant)
                {
                    object value = Expression.getConstantValue().notNull();
                    if (value.GetType() == typeof(bool)) return (bool)value ? LogicTypeEnum.True : LogicTypeEnum.False;
                }
                return LogicTypeEnum.Unknown;
            }
        }
        /// <summary>
        /// 条件表达式重组
        /// </summary>
        /// <param name="expression"></param>
        public ConditionExpressionConverter(System.Linq.Expressions.Expression expression)
        {
            Expression = expression;
            Type = ConvertTypeEnum.Expression;
            NotSupportType = 0;
            ExceptionType = 0;
            if (expression != null) Convert();
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TryConvert(System.Linq.Expressions.Expression expression)
        {
            if (expression != null)
            {
                Expression = expression;
                Convert();
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Convert(System.Linq.Expressions.Expression expression)
        {
            Expression = expression;
            Type = ConvertTypeEnum.Expression;
            Convert();
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        internal void Convert()
        {
            switch (Expression.NodeType)
            {
                case ExpressionType.OrElse: convertOrElse(); return;
                case ExpressionType.AndAlso: convertAndAlso(); return;
                case ExpressionType.Not: convertNot(); return;
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.Or:
                case ExpressionType.And:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                    convertBinaryExpression();
                    return;
                case ExpressionType.MemberAccess: convertMemberAccess(); return;
                case ExpressionType.ArrayLength: convertArrayLength(); return;
                case ExpressionType.ArrayIndex: convertArrayIndex(); return;
                case ExpressionType.Coalesce: convertCoalesce(); return;
                case ExpressionType.Unbox: convertUnbox(); return;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    convertUnaryCalculator();
                    return;
                case ExpressionType.IsTrue: convertIsTrue(); return;
                case ExpressionType.IsFalse: convertIsFalse(); return;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    convertConvert((UnaryExpression)Expression);
                    return;
                case ExpressionType.Conditional: convertConditional(); return;
                case ExpressionType.Call: convertCall(); return;
                //case ExpressionType.Lambda: convertLambda(); return;

                case ExpressionType.UnaryPlus:
                case ExpressionType.Constant: Type = ConvertTypeEnum.Expression; return;
                default: Type = ConvertTypeEnum.NotSupport; NotSupportType = Expression.NodeType; return;
            }
        }

        /// <summary>
        /// 获取逻辑值类型
        /// </summary>
        /// <returns></returns>
        private LogicTypeEnum getLogicType()
        {
            if (Expression.NodeType == ExpressionType.Constant)
            {
                return (bool)Expression.getConstantValue().notNull() ? LogicTypeEnum.True : LogicTypeEnum.False;
            }
            return LogicTypeEnum.Unknown;
        }
        /// <summary>
        /// || 表达式
        /// </summary>
        private void convertOrElse()
        {
            BinaryExpression binaryExpression = (BinaryExpression)Expression;
            Expression = binaryExpression.Left;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                switch (getLogicType())
                {
                    case LogicTypeEnum.False:
                        Expression = binaryExpression.Right;
                        Convert();
                        if (Type != ConvertTypeEnum.NotSupport) Type = ConvertTypeEnum.ConvertExpression;
                        return;
                    case LogicTypeEnum.True:
                        Expression = constantTrue;
                        Type = ConvertTypeEnum.ConvertExpression;
                        return;
                    default:
                        ConditionExpressionConverter left = this;
                        Expression = binaryExpression.Right;
                        Convert();
                        if (Type != ConvertTypeEnum.NotSupport)
                        {
                            switch (getLogicType())
                            {
                                case LogicTypeEnum.False: Expression = left.Expression; break;
                                case LogicTypeEnum.True: Expression = constantTrue; break;
                                default:
                                    if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression)
                                    {
                                        Expression = binaryExpression;
                                        return;
                                    }
                                    Expression = System.Linq.Expressions.Expression.OrElse(left.Expression, Expression);
                                    break;
                            }
                            Type = ConvertTypeEnum.ConvertExpression;
                        }
                        return;
                }
            }
        }
        /// <summary>
        /// AND 表达式
        /// </summary>
        private void convertAndAlso()
        {
            BinaryExpression binaryExpression = (BinaryExpression)Expression;
            Expression = binaryExpression.Left;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                switch (getLogicType())
                {
                    case LogicTypeEnum.False:
                        Expression = constantFalse;
                        Type = ConvertTypeEnum.ConvertExpression;
                        return;
                    case LogicTypeEnum.True:
                        Expression = binaryExpression.Right;
                        Convert();
                        if (Type != ConvertTypeEnum.NotSupport) Type = ConvertTypeEnum.ConvertExpression;
                        return;
                    default:
                        ConditionExpressionConverter left = this;
                        Expression = binaryExpression.Right;
                        Convert();
                        if (Type != ConvertTypeEnum.NotSupport)
                        {
                            switch (getLogicType())
                            {
                                case LogicTypeEnum.False: Expression = constantFalse; break;
                                case LogicTypeEnum.True: Expression = left.Expression; break;
                                default:
                                    if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression)
                                    {
                                        Expression = binaryExpression;
                                        return;
                                    }
                                    Expression = System.Linq.Expressions.Expression.AndAlso(left.Expression, Expression);
                                    break;
                            }
                            Type = ConvertTypeEnum.ConvertExpression;
                        }
                        return;
                }
            }
        }
        /// <summary>
        /// ! 表达式
        /// </summary>
        private void convertNot()
        {
            UnaryExpression unaryExpression = (UnaryExpression)Expression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                if (Expression.NodeType == ExpressionType.Constant)
                {
                    var value = Expression.getConstantValue();
                    if (value != null)
                    {
                        System.Type systemType = value.GetType();
                        if (systemType == typeof(bool))
                        {
                            Expression = (bool)value ? constantFalse : constantTrue;
                            Type = ConvertTypeEnum.ConvertExpression;
                            return;
                        }
                        var calculator = default(Func<object, object>);
                        if (notCalculators.TryGetValue(systemType, out calculator))
                        {
                            Expression = System.Linq.Expressions.Expression.Constant(calculator(value));
                            Type = ConvertTypeEnum.ConvertExpression;
                            return;
                        }
                    }
                }
                if (Type == ConvertTypeEnum.Expression) Expression = unaryExpression;
                else
                {
                    Expression = System.Linq.Expressions.Expression.Not(Expression);
                    Type = ConvertTypeEnum.ConvertExpression;
                }
            }
        }
        /// <summary>
        /// 二元表达式
        /// </summary>
        private void convertBinaryExpression()
        {
            BinaryExpression binaryExpression = (BinaryExpression)Expression;
            Expression = binaryExpression.Left;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                ConditionExpressionConverter left = this;
                Expression = binaryExpression.Right;
                Convert();
                if (Type != ConvertTypeEnum.NotSupport)
                {
                    switch (binaryExpression.NodeType)
                    {
                        case ExpressionType.Equal:
                            if (convertEqual(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.NotEqual:
                            if (convertNotEqual(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.GreaterThanOrEqual:
                            if (convertGreaterThanOrEqual(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.GreaterThan:
                            if (convertGreaterThan(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.LessThan:
                            if (convertLessThan(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.LessThanOrEqual:
                            if (convertLessThanOrEqual(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.Add:
                        case ExpressionType.AddChecked:
                        case ExpressionType.Subtract:
                        case ExpressionType.SubtractChecked:
                        case ExpressionType.Multiply:
                        case ExpressionType.MultiplyChecked:
                        case ExpressionType.Divide:
                        case ExpressionType.Modulo:
                        case ExpressionType.Or:
                        case ExpressionType.And:
                        case ExpressionType.ExclusiveOr:
                            var value = getConstantCalculator(left.Expression, binaryExpression.NodeType);
                            if (value != null) Expression = System.Linq.Expressions.Expression.Constant(value);
                            else if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression)
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            else
                            {
                                switch (binaryExpression.NodeType)
                                {
                                    case ExpressionType.Add: Expression = System.Linq.Expressions.Expression.Add(left.Expression, Expression); break;
                                    case ExpressionType.AddChecked: Expression = System.Linq.Expressions.Expression.AddChecked(left.Expression, Expression); break;
                                    case ExpressionType.Subtract: Expression = System.Linq.Expressions.Expression.Subtract(left.Expression, Expression); break;
                                    case ExpressionType.SubtractChecked: Expression = System.Linq.Expressions.Expression.SubtractChecked(left.Expression, Expression); break;
                                    case ExpressionType.Multiply: Expression = System.Linq.Expressions.Expression.Multiply(left.Expression, Expression); break;
                                    case ExpressionType.MultiplyChecked: Expression = System.Linq.Expressions.Expression.MultiplyChecked(left.Expression, Expression); break;
                                    case ExpressionType.Divide: Expression = System.Linq.Expressions.Expression.Divide(left.Expression, Expression); break;
                                    case ExpressionType.Modulo: Expression = System.Linq.Expressions.Expression.Modulo(left.Expression, Expression); break;
                                    case ExpressionType.Or: Expression = System.Linq.Expressions.Expression.Or(left.Expression, Expression); break;
                                    case ExpressionType.And: Expression = System.Linq.Expressions.Expression.And(left.Expression, Expression); break;
                                    case ExpressionType.ExclusiveOr: Expression = System.Linq.Expressions.Expression.ExclusiveOr(left.Expression, Expression); break;
                                }
                            }
                            break;
                        case ExpressionType.LeftShift:
                            if (convertLeftShift(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.RightShift:
                            if (convertRightShift(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                    }
                    Type = ConvertTypeEnum.ConvertExpression;
                }
            }
        }
        /// <summary>
        /// 获取常量 == 比较结果类型
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <returns></returns>
        private LogicTypeEnum getConstantEqualType(System.Linq.Expressions.Expression leftExpression)
        {
            if (Expression.NodeType == ExpressionType.Constant && leftExpression.NodeType == ExpressionType.Constant)
            {
                var left = leftExpression.getConstantValue();
                var right = Expression.getConstantValue();
                if (left != null) return left.Equals(right) ? LogicTypeEnum.True : LogicTypeEnum.False;
                return right == null || right.Equals(left) ? LogicTypeEnum.True : LogicTypeEnum.False;
            }
            return LogicTypeEnum.Unknown;
        }
        /// <summary>
        /// == 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertEqual(ref ConditionExpressionConverter left)
        {
            switch (getConstantEqualType(left.Expression))
            {
                case LogicTypeEnum.True: Expression = constantTrue; return false;
                case LogicTypeEnum.False: Expression = constantFalse; return false;
                default:
                    System.Linq.Expressions.Expression right;
                    System.Linq.Expressions.Expression leftExpression = checkMemberAccess(left.Expression, Expression, out right);
                    if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression && object.ReferenceEquals(right, Expression)) return true;
                    Expression = System.Linq.Expressions.Expression.Equal(leftExpression, right);
                    return false;
            }
        }
        /// <summary>
        /// != 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertNotEqual(ref ConditionExpressionConverter left)
        {
            switch (getConstantEqualType(left.Expression))
            {
                case LogicTypeEnum.True: Expression = constantFalse; return false;
                case LogicTypeEnum.False: Expression = constantTrue; return false;
                default:
                    System.Linq.Expressions.Expression right;
                    System.Linq.Expressions.Expression leftExpression = checkMemberAccess(left.Expression, Expression, out right);
                    if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression && object.ReferenceEquals(right, Expression)) return true;
                    Expression = System.Linq.Expressions.Expression.NotEqual(leftExpression, right);
                    return false;
            }
        }
        /// <summary>
        /// 获取常量比较结果类型
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        private LogicTypeEnum getConstantComparatorType(System.Linq.Expressions.Expression leftExpression, ExpressionType expressionType)
        {
            if (Expression.NodeType == ExpressionType.Constant && leftExpression.NodeType == ExpressionType.Constant)
            {
                var left = leftExpression.getConstantValue();
                if (left != null)
                {
                    var right = Expression.getConstantValue();
                    if (right != null)
                    {
                        System.Type type = left.GetType();
                        if (type == right.GetType())
                        {
                            var comparator = default(Func<ExpressionType, object, object, LogicTypeEnum>);
                            if (comparators.TryGetValue(type, out comparator)) return comparator(expressionType, left, right);
                        }
                    }
                }
            }
            return LogicTypeEnum.Unknown;
        }
        /// <summary>
        /// 大于等于 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertGreaterThanOrEqual(ref ConditionExpressionConverter left)
        {
            switch (getConstantComparatorType(left.Expression, ExpressionType.GreaterThanOrEqual))
            {
                case LogicTypeEnum.True: Expression = constantTrue; return false;
                case LogicTypeEnum.False: Expression = constantFalse; return false;
                default:
                    System.Linq.Expressions.Expression right;
                    System.Linq.Expressions.Expression leftExpression = checkMemberAccess(left.Expression, Expression, out right);
                    if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression && object.ReferenceEquals(right, Expression)) return true;
                    if (object.ReferenceEquals(leftExpression, left.Expression)) Expression = System.Linq.Expressions.Expression.GreaterThanOrEqual(leftExpression, right);
                    else Expression = System.Linq.Expressions.Expression.LessThan(leftExpression, right);
                    return false;
            }
        }
        /// <summary>
        /// 大于 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertGreaterThan(ref ConditionExpressionConverter left)
        {
            switch (getConstantComparatorType(left.Expression, ExpressionType.GreaterThan))
            {
                case LogicTypeEnum.True: Expression = constantTrue; return false;
                case LogicTypeEnum.False: Expression = constantFalse; return false;
                default:
                    System.Linq.Expressions.Expression right;
                    System.Linq.Expressions.Expression leftExpression = checkMemberAccess(left.Expression, Expression, out right);
                    if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression && object.ReferenceEquals(right, Expression)) return true;
                    if (object.ReferenceEquals(leftExpression, left.Expression)) Expression = System.Linq.Expressions.Expression.GreaterThan(leftExpression, right);
                    else Expression = System.Linq.Expressions.Expression.LessThanOrEqual(leftExpression, right);
                    return false;
            }
        }
        /// <summary>
        /// 小于 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertLessThan(ref ConditionExpressionConverter left)
        {
            switch (getConstantComparatorType(left.Expression, ExpressionType.GreaterThanOrEqual))
            {
                case LogicTypeEnum.True: Expression = constantFalse; return false;
                case LogicTypeEnum.False: Expression = constantTrue; return false;
                default:
                    System.Linq.Expressions.Expression right;
                    System.Linq.Expressions.Expression leftExpression = checkMemberAccess(left.Expression, Expression, out right);
                    if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression && object.ReferenceEquals(right, Expression)) return true;
                    if (object.ReferenceEquals(leftExpression, left.Expression)) Expression = System.Linq.Expressions.Expression.LessThan(leftExpression, right);
                    else Expression = System.Linq.Expressions.Expression.GreaterThanOrEqual(leftExpression, right);
                    return false;
            }
        }
        /// <summary>
        /// 小于等于 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertLessThanOrEqual(ref ConditionExpressionConverter left)
        {
            switch (getConstantComparatorType(left.Expression, ExpressionType.GreaterThan))
            {
                case LogicTypeEnum.True: Expression = constantFalse; return false;
                case LogicTypeEnum.False: Expression = constantTrue; return false;
                default:
                    System.Linq.Expressions.Expression right;
                    System.Linq.Expressions.Expression leftExpression = checkMemberAccess(left.Expression, Expression, out right);
                    if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression && object.ReferenceEquals(right, Expression)) return true;
                    if (object.ReferenceEquals(leftExpression, left.Expression)) Expression = System.Linq.Expressions.Expression.LessThanOrEqual(leftExpression, right);
                    else Expression = System.Linq.Expressions.Expression.GreaterThan(leftExpression, right);
                    return false;
            }
        }
        /// <summary>
        /// 获取常量计算结果
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
#if NetStandard21
        private object? getConstantCalculator(System.Linq.Expressions.Expression leftExpression, ExpressionType expressionType)
#else
        private object getConstantCalculator(System.Linq.Expressions.Expression leftExpression, ExpressionType expressionType)
#endif
        {
            if (Expression.NodeType == ExpressionType.Constant && leftExpression.NodeType == ExpressionType.Constant)
            {
                var left = leftExpression.getConstantValue();
                if (left != null)
                {
                    var right = Expression.getConstantValue();
                    if (right != null)
                    {
                        System.Type type = left.GetType();
                        if (type == right.GetType())
                        {
#if NetStandard21
                            var calculator = default(Func<ExpressionType, object, object, object?>);
#else
                            Func<ExpressionType, object, object, object> calculator;
#endif
                            if (calculators.TryGetValue(type, out calculator)) return calculator(expressionType, left, right);
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取常量计算结果
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
#if NetStandard21
        private object? getConstantCalculatorShift(System.Linq.Expressions.Expression leftExpression, ExpressionType expressionType)
#else
        private object getConstantCalculatorShift(System.Linq.Expressions.Expression leftExpression, ExpressionType expressionType)
#endif
        {
            if (Expression.NodeType == ExpressionType.Constant && leftExpression.NodeType == ExpressionType.Constant)
            {
                var left = leftExpression.getConstantValue();
                if (left != null)
                {
                    var right = Expression.getConstantValue();
                    if (right != null && right.GetType() == typeof(int))
                    {
#if NetStandard21
                        var calculator = default(Func<ExpressionType, object, object, object?>);
#else
                        Func<ExpressionType, object, object, object> calculator;
#endif
                        if (calculators.TryGetValue(left.GetType(), out calculator)) return calculator(expressionType, left, right);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 左移 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertLeftShift(ref ConditionExpressionConverter left)
        {
            var value = getConstantCalculatorShift(left.Expression, ExpressionType.LeftShift);
            if (value != null)
            {
                Expression = System.Linq.Expressions.Expression.Constant(value);
                return false;
            }
            if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression) return true;
            Expression = System.Linq.Expressions.Expression.LeftShift(left.Expression, Expression);
            return false;
        }
        /// <summary>
        /// 右移 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertRightShift(ref ConditionExpressionConverter left)
        {
            var value = getConstantCalculatorShift(left.Expression, ExpressionType.RightShift);
            if (value != null)
            {
                Expression = System.Linq.Expressions.Expression.Constant(value);
                return false;
            }
            if (Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression) return true;
            Expression = System.Linq.Expressions.Expression.RightShift(left.Expression, Expression);
            return false;
        }
        /// <summary>
        /// 检查成员表达式
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static System.Linq.Expressions.Expression checkMemberAccess(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, out System.Linq.Expressions.Expression expression)
        {
            if (left.NodeType == ExpressionType.MemberAccess || right.NodeType != ExpressionType.MemberAccess)
            {
                expression = right;
                return left;
            }
            expression = left;
            return right;
        }
        /// <summary>
        /// 判断成员表达式是否参数成员
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        private static bool checkMemberAccessParameter(MemberExpression memberExpression)
        {
            System.Linq.Expressions.Expression expression = memberExpression.Expression.notNull();
            while(expression.NodeType == ExpressionType.MemberAccess) expression = ((MemberExpression)expression).Expression.notNull();
            return expression.NodeType == ExpressionType.Parameter;
        }
        /// <summary>
        /// 成员表达式
        /// </summary>
        private void convertMemberAccess()
        {
            MemberExpression memberExpression = (MemberExpression)Expression;
            var target = default(object);
            if (memberExpression.Expression != null)
            {
                if (checkMemberAccessParameter(memberExpression))
                {
                    Type = ConvertTypeEnum.Expression;
                    return;
                }
                Expression = memberExpression.Expression;
                Convert();
                if (Type == ConvertTypeEnum.NotSupport) return;
                if (Expression.NodeType != ExpressionType.Constant)
                {
                    unknown(memberExpression, ExpressionType.MemberAccess);
                    return;
                }
                target = Expression.getConstantValue();
                if (target == null)
                {
                    ExceptionType = ExceptionTypeEnum.TargetIsNull;
                    unknown(memberExpression, ExpressionType.MemberAccess);
                    return;
                }
            }
            var fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo != null)
            {
                Expression = System.Linq.Expressions.Expression.Constant(fieldInfo.GetValue(target));
                Type = ConvertTypeEnum.ConvertExpression;
                return;
            }
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
            {
                Expression = System.Linq.Expressions.Expression.Constant(propertyInfo.GetValue(target, null));
                Type = ConvertTypeEnum.ConvertExpression;
                return;
            }
            unknown(memberExpression, ExpressionType.MemberAccess);
        }
        /// <summary>
        /// 未知表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="notSupportType"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void unknown(System.Linq.Expressions.Expression expression, ExpressionType notSupportType)
        {
            Type = ConvertTypeEnum.NotSupport;
            NotSupportType = ExpressionType.MemberAccess;
            Expression = expression;
        }
        /// <summary>
        /// 数组长度表达式
        /// </summary>
        private void convertArrayLength()
        {
            UnaryExpression unaryExpression = (UnaryExpression)Expression;
            Expression = unaryExpression.Operand;
            var array = convertArray(ExpressionType.ArrayLength);
            if (array != null)
            {
                Expression = System.Linq.Expressions.Expression.Constant(array.Length);
                Type = ConvertTypeEnum.ConvertExpression;
            }
        }
        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        private Array? convertArray(ExpressionType type)
#else
        private Array convertArray(ExpressionType type)
#endif
        {
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                if (Expression.NodeType == ExpressionType.Constant)
                {
                    var array = Expression.getConstantValue();
                    if (array != null)
                    {
                        if (array.GetType().IsArray) return (Array)array;
                        ExceptionType = ExceptionTypeEnum.TargetNotArray;
                    }
                    else ExceptionType = ExceptionTypeEnum.TargetIsNull;
                }
                else ExceptionType = ExceptionTypeEnum.ArrayNotConstant;
                Type = ConvertTypeEnum.NotSupport;
                NotSupportType = type;
            }
            return null;
        }
        /// <summary>
        /// 数组索引表达式
        /// </summary>
        private void convertArrayIndex()
        {
            BinaryExpression binaryExpression = (BinaryExpression)Expression;
            Expression = binaryExpression.Left;
            var array = convertArray(ExpressionType.ArrayIndex);
            if (array != null)
            {
                Expression = binaryExpression.Right;
                if (Type != ConvertTypeEnum.NotSupport)
                {
                    if (Expression.NodeType == ExpressionType.Constant)
                    {
                        var indexObject = Expression.getConstantValue();
                        if (indexObject != null && indexObject.GetType() == typeof(int))
                        {
                            int index = (int)indexObject;
                            if ((uint)index < array.Length)
                            {
                                Expression = System.Linq.Expressions.Expression.Constant(array.GetValue(index));
                                Type = ConvertTypeEnum.ConvertExpression;
                                return;
                            }
                            ExceptionType = ExceptionTypeEnum.ArrayIndexOutOfRange;
                        }
                        else ExceptionType = ExceptionTypeEnum.ArrayIndexNotInt;
                    }
                    else ExceptionType = ExceptionTypeEnum.ArrayIndexNotInt;
                    Type = ConvertTypeEnum.NotSupport;
                    NotSupportType = ExpressionType.ArrayIndex;
                }
            }
        }
        /// <summary>
        /// ?? 表达式（isnull）
        /// </summary>
        private void convertCoalesce()
        {
            BinaryExpression binaryExpression = (BinaryExpression)Expression;
            Expression = binaryExpression.Left;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                if (Expression.NodeType == ExpressionType.Constant)
                {
                    if (Expression.getConstantValue() == null)
                    {
                        Expression = binaryExpression.Right;
                        Convert();
                    }
                    Type = ConvertTypeEnum.ConvertExpression;
                    return;
                }
                if (Type == ConvertTypeEnum.Expression)
                {
                    Expression = binaryExpression;
                    return;
                }
                System.Linq.Expressions.Expression left = Expression;
                Expression = binaryExpression.Right;
                Convert();
                if (Type != ConvertTypeEnum.NotSupport)
                {
                    Expression = System.Linq.Expressions.Expression.Coalesce(left, Expression);
                    Type = ConvertTypeEnum.ConvertExpression;
                }
            }
        }
        /// <summary>
        /// 拆箱表达式
        /// </summary>
        private void convertUnbox()
        {
            UnaryExpression unaryExpression = (UnaryExpression)Expression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type == ConvertTypeEnum.Expression) Expression = unaryExpression;
        }
        /// <summary>
        /// 单值计算表达式
        /// </summary>
        private void convertUnaryCalculator()
        {
            UnaryExpression unaryExpression = (UnaryExpression)Expression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                if (Expression.NodeType == ExpressionType.Constant)
                {
                    var value = Expression.getConstantValue();
                    if (value != null)
                    {
#if NetStandard21
                        var calculator = default(Func<ExpressionType, object, object?>);
#else
                        Func<ExpressionType, object, object> calculator;
#endif
                        if (unaryCalculators.TryGetValue(value.GetType(), out calculator))
                        {
                            value = calculator(unaryExpression.NodeType, value);
                            if (value != null)
                            {
                                Expression = System.Linq.Expressions.Expression.Constant(value);
                                Type = ConvertTypeEnum.ConvertExpression;
                                return;
                            }
                        }
                    }
                }
                if (Type == ConvertTypeEnum.Expression)
                {
                    Expression = unaryExpression;
                    return;
                }
                switch (unaryExpression.NodeType)
                {
                    case ExpressionType.Negate: Expression = System.Linq.Expressions.Expression.Negate(Expression); break;
                    case ExpressionType.NegateChecked: Expression = System.Linq.Expressions.Expression.NegateChecked(Expression); break;
                }
                Type = ConvertTypeEnum.ConvertExpression;
            }
        }
        /// <summary>
        /// 真值判断 表达式
        /// </summary>
        private void convertIsTrue()
        {
            UnaryExpression unaryExpression = (UnaryExpression)Expression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport && getLogicType() == LogicTypeEnum.Unknown)
            {
                if (Type == ConvertTypeEnum.Expression) Expression = unaryExpression;
                else
                {
                    Expression = System.Linq.Expressions.Expression.IsTrue(Expression);
                    Type = ConvertTypeEnum.ConvertExpression;
                }
            }
        }
        /// <summary>
        /// 假值判断 表达式
        /// </summary>
        private void convertIsFalse()
        {
            UnaryExpression unaryExpression = (UnaryExpression)Expression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                switch (getLogicType())
                {
                    case LogicTypeEnum.True: Expression = constantFalse; break;
                    case LogicTypeEnum.False: Expression = constantTrue; break;
                    default:
                        if (Type == ConvertTypeEnum.Expression)
                        {
                            Expression = unaryExpression;
                            return;
                        }
                        Expression = System.Linq.Expressions.Expression.IsFalse(Expression);
                        break;
                }
                Type = ConvertTypeEnum.ConvertExpression;
            }
        }
        /// <summary>
        /// 类型转换表达式
        /// </summary>
        /// <param name="unaryExpression"></param>
        private void convertConvert(UnaryExpression unaryExpression)
        {
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                switch (Expression.NodeType)
                {
                    case ExpressionType.Constant:
                        var value = Expression.getConstantValue();
                        if (value != null)
                        {
                            System.Type valueType = value.GetType(), convertType = unaryExpression.Type;
                            if (valueType == convertType)
                            {
                                Type = ConvertTypeEnum.ConvertExpression;
                                return;
                            }
                            if (convertType.IsValueType && convertType.IsGenericType
                                && convertType.GetGenericTypeDefinition() == typeof(Nullable<>) && valueType == convertType.GetGenericArguments()[0])
                            {
                                Type = ConvertTypeEnum.ConvertExpression;
                                return;
                            }
                            if (valueType.IsEnum && System.Enum.GetUnderlyingType(valueType) == convertType)
                            {
                                Expression = System.Linq.Expressions.Expression.Constant(convertEnum(value, convertType));
                                Type = ConvertTypeEnum.ConvertExpression;
                                return;
                            }
                        }
                        else if (unaryExpression.Type.IsClass)
                        {
                            Expression = System.Linq.Expressions.Expression.Constant(null, unaryExpression.Type);
                            Type = ConvertTypeEnum.ConvertExpression;
                            return;
                        }
                        break;
                    //case ExpressionType.MemberAccess: Type = ConvertTypeEnum.ConvertExpression; return;
                }
                if (Type == ConvertTypeEnum.Expression)
                {
                    Expression = unaryExpression;
                    return;
                }
                Expression = System.Linq.Expressions.Expression.Convert(Expression, unaryExpression.Type);
                Type = ConvertTypeEnum.ConvertExpression;
            }
        }
        /// <summary>
        /// 枚举转换整数
        /// </summary>
#if NetStandard21
        private static object? convertEnum(object value, System.Type convertType)
#else
        private static object convertEnum(object value, System.Type convertType)
#endif
        {
            if (convertType == typeof(int)) return (int)value;
            if (convertType == typeof(long)) return (long)value;
            if (convertType == typeof(uint)) return (uint)value;
            if (convertType == typeof(ulong)) return (ulong)value;
            if (convertType == typeof(byte)) return (byte)value;
            if (convertType == typeof(sbyte)) return (sbyte)value;
            if (convertType == typeof(ushort)) return (ushort)value;
            if (convertType == typeof(short)) return (short)value;
            return null;
        }
        /// <summary>
        /// 三元表达式
        /// </summary>
        private void convertConditional()
        {
            ConditionalExpression conditionalExpression = (ConditionalExpression)Expression;
            Expression = conditionalExpression.Test;
            Convert();
            if (Type != ConvertTypeEnum.NotSupport)
            {
                switch (getLogicType())
                {
                    case LogicTypeEnum.True:
                        Expression = conditionalExpression.IfTrue;
                        Convert();
                        if (Type != ConvertTypeEnum.NotSupport) Type = ConvertTypeEnum.ConvertExpression;
                        return;
                    case LogicTypeEnum.False:
                        Expression = conditionalExpression.IfFalse;
                        Convert();
                        if (Type != ConvertTypeEnum.NotSupport) Type = ConvertTypeEnum.ConvertExpression;
                        return;
                    default:
                        ConditionExpressionConverter test = this;
                        Expression = conditionalExpression.IfTrue;
                        Convert();
                        if (Type != ConvertTypeEnum.NotSupport)
                        {
                            ConditionExpressionConverter left = this;
                            Expression = conditionalExpression.IfFalse;
                            Convert();
                            if (Type != ConvertTypeEnum.NotSupport)
                            {
                                if (Type == ConvertTypeEnum.Expression && test.Type == ConvertTypeEnum.Expression && left.Type == ConvertTypeEnum.Expression)
                                {
                                    Expression = conditionalExpression;
                                    return;
                                }
                                Expression = System.Linq.Expressions.Expression.Condition(test.Expression, left.Expression, Expression);
                                Type = ConvertTypeEnum.ConvertExpression;
                            }
                        }
                        return;
                }
            }
        }
        /// <summary>
        /// 函数表达式
        /// </summary>
        private void convertCall()
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)Expression;
            MethodInfo method = methodCallExpression.Method;
            if (method.ReflectedType == typeof(SQLExpression))
            {
                switch (method.Name.Length)
                {
                    case 2:
                        switch (method.Name)
                        {
                            case nameof(SQLExpression.In):
                                Expression = methodCallExpression.Arguments[1];
                                if (convertCallIn(false)) return;
                                break;
                        }
                        break;
                    case 5:
                        switch (method.Name)
                        {
                            case nameof(SQLExpression.NotIn):
                                Expression = methodCallExpression.Arguments[1];
                                if (convertCallIn(true)) return;
                                break;
                        }
                        break;
                    case 6:
                        switch (method.Name)
                        {
                            case nameof(SQLExpression.Exists):
                                Expression = methodCallExpression.Arguments[0];
                                if (convertCallExists(false)) return;
                                break;
                        }
                        break;
                    case 9:
                        switch (method.Name)
                        {
                            case nameof(SQLExpression.NotExists):
                                Expression = methodCallExpression.Arguments[0];
                                if (convertCallExists(true)) return;
                                break;
                            case nameof(SQLExpression.CompareTo):
                                Expression = methodCallExpression.Arguments[0];
                                Convert();
                                if (Expression.NodeType == ExpressionType.Constant)
                                {
                                    var left = Expression.getConstantValue();
                                    if (left != null)
                                    {
                                        Expression = methodCallExpression.Arguments[1];
                                        Convert();
                                        if (Expression.NodeType == ExpressionType.Constant)
                                        {
                                            var right = Expression.getConstantValue();
                                            if (left != null && right != null)
                                            {
                                                Type type = right.GetType();
                                                if (typeof(IQueryBuilder).IsAssignableFrom(type))
                                                {
                                                    if (!((IQueryBuilder)right).IsQuery)
                                                    {
                                                        Expression = constantFalse;
                                                        Type = ConvertTypeEnum.ConvertExpression;
                                                        return;
                                                    }
                                                }
                                                else if (type == left.GetType())
                                                {
                                                    var comparator = default(Func<ExpressionType, object, object, LogicTypeEnum>);
                                                    if (comparators.TryGetValue(type, out comparator))
                                                    {
                                                        LogicTypeEnum logicType = comparator(methodCallExpression.Arguments[2].getConstantValue().castType<ExpressionType>(), left, right);
                                                        switch (logicType)
                                                        {
                                                            case LogicTypeEnum.False:
                                                                Expression = constantFalse;
                                                                Type = ConvertTypeEnum.ConvertExpression;
                                                                return;
                                                            case LogicTypeEnum.True:
                                                                Expression = constantTrue;
                                                                Type = ConvertTypeEnum.ConvertExpression;
                                                                return;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                }
                Expression = methodCallExpression;
                Type = ConvertTypeEnum.Expression;
                return;
            }
            ConvertCall(methodCallExpression, method);
        }
        /// <summary>
        /// IN 表达式
        /// </summary>
        /// <param name="isNot"></param>
        /// <returns></returns>
        private bool convertCallIn(bool isNot)
        {
            Convert();
            if (Type == ConvertTypeEnum.NotSupport) return true;
            if (Expression.NodeType == ExpressionType.Constant)
            {
                var values = Expression.getConstantValue();
                bool isEmpty = values == null;
                if (values != null)
                {
                    Type type = values.GetType();
                    if (type.IsArray) isEmpty = ((Array)values).Length == 0;
                    else if (typeof(IQueryBuilder).IsAssignableFrom(type)) isEmpty = !((IQueryBuilder)values).IsQuery;
                }
                if (isEmpty)
                {
                    Expression = isNot ? constantTrue : constantFalse;
                    Type = ConvertTypeEnum.ConvertExpression;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// EXISTS 表达式
        /// </summary>
        /// <param name="isNot"></param>
        /// <returns></returns>
        private bool convertCallExists(bool isNot)
        {
            var query = Expression.getConstantValue().castType<IQueryBuilder>();
            if (query == null || !query.IsQuery)
            {
                Expression = isNot ? constantTrue : constantFalse;
                Type = ConvertTypeEnum.ConvertExpression;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 函数表达式
        /// </summary>
        /// <param name="methodCallExpression"></param>
        /// <param name="method"></param>
        internal void ConvertCall(MethodCallExpression methodCallExpression, MethodInfo method)
        {
            var target = default(object);
            if (methodCallExpression.Object != null)
            {
                Expression = methodCallExpression.Object;
                Convert();
                if (Type == ConvertTypeEnum.NotSupport) return;
                if (Expression.NodeType != ExpressionType.Constant)
                {
                    unknown(methodCallExpression, ExpressionType.Call);
                    return;
                }
                target = Expression.getConstantValue();
                if (target == null)
                {
                    ExceptionType = ExceptionTypeEnum.TargetIsNull;
                    unknown(methodCallExpression, ExpressionType.Call);
                    return;
                }
            }
#if NetStandard21
            object?[] arguments = EmptyArray<object>.Array;
#else
            object[] arguments = EmptyArray<object>.Array;
#endif
            if (methodCallExpression.Arguments != null && methodCallExpression.Arguments.Count != 0)
            {
                arguments = new object[methodCallExpression.Arguments.Count];
                int argumentIndex = 0;
                foreach (System.Linq.Expressions.Expression agrumentExpression in methodCallExpression.Arguments)
                {
                    Expression = agrumentExpression;
                    Convert();
                    if (Type == ConvertTypeEnum.NotSupport) return;
                    if (Expression.NodeType != ExpressionType.Constant)
                    {
                        unknown(methodCallExpression, ExpressionType.Call);
                        return;
                    }
                    arguments[argumentIndex++] = Expression.getConstantValue();
                }
            }
            Expression = System.Linq.Expressions.Expression.Constant(method.Invoke(target, arguments));
            Type = ConvertTypeEnum.ConvertExpression;
        }
        ///// <summary>
        ///// Lambda 表达式
        ///// </summary>
        //private void convertLambda()
        //{
        //    try
        //    {
        //        //需要缓存，否则会内存泄漏
        //        MethodInfo compileMethod = Expression.GetType().GetMethod("Compile", BindingFlags.Instance | BindingFlags.Public, null, EmptyArray<Type>.Array, null);
        //        object delegateObject = compileMethod.Invoke(Expression, null);
        //        Expression = Expression.Constant(delegateObject);
        //        Type = ConvertTypeEnum.ConvertExpression;
        //    }
        //    catch
        //    {
        //        Type = ConvertTypeEnum.NotSupport;
        //        NotSupportType = ExpressionType.Lambda;
        //    }
        //}

        /// <summary>
        /// 常量真值
        /// </summary>
        private static readonly ConstantExpression constantTrue = System.Linq.Expressions.Expression.Constant(true);
        /// <summary>
        /// 常量假值
        /// </summary>
        private static readonly ConstantExpression constantFalse = System.Linq.Expressions.Expression.Constant(false);

        /// <summary>
        /// 常量比较器集合
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, Func<ExpressionType, object, object, LogicTypeEnum>> comparators;
        /// <summary>
        /// 常量计算器集合
        /// </summary>
#if NetStandard21
        private static readonly Dictionary<HashObject<System.Type>, Func<ExpressionType, object, object, object?>> calculators;
#else
        private static readonly Dictionary<HashObject<System.Type>, Func<ExpressionType, object, object, object>> calculators;
#endif
        /// <summary>
        /// 常量计算器集合
        /// </summary>
#if NetStandard21
        private static readonly Dictionary<HashObject<System.Type>, Func<ExpressionType, object, object?>> unaryCalculators;
#else
        private static readonly Dictionary<HashObject<System.Type>, Func<ExpressionType, object, object>> unaryCalculators;
#endif
        /// <summary>
        /// 常量计算器集合
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, Func<object, object>> notCalculators;
        static ConditionExpressionConverter()
        {
            comparators = DictionaryCreator.CreateHashObject<System.Type, Func<ExpressionType, object, object, LogicTypeEnum>>();
            comparators.Add(typeof(ulong), compareULong);
            comparators.Add(typeof(long), compareLong);
            comparators.Add(typeof(uint), compareUInt);
            comparators.Add(typeof(int), compareInt);
            comparators.Add(typeof(ushort), compareUShort);
            comparators.Add(typeof(short), compareShort);
            comparators.Add(typeof(byte), compareByte);
            comparators.Add(typeof(sbyte), compareSByte);
            comparators.Add(typeof(double), compareDouble);
            comparators.Add(typeof(float), compareFloat);
            comparators.Add(typeof(decimal), compareDecimal);
            comparators.Add(typeof(DateTime), compareDateTime);
            comparators.Add(typeof(TimeSpan), compareTimeSpan);

#if NetStandard21
            calculators = DictionaryCreator.CreateHashObject<System.Type, Func<ExpressionType, object, object, object?>>();
#else
            calculators = DictionaryCreator.CreateHashObject<System.Type, Func<ExpressionType, object, object, object>>();
#endif
            calculators.Add(typeof(ulong), calculateULong);
            calculators.Add(typeof(long), calculateLong);
            calculators.Add(typeof(uint), calculateUInt);
            calculators.Add(typeof(int), calculateInt);
            calculators.Add(typeof(ushort), calculateUShort);
            calculators.Add(typeof(short), calculateShort);
            calculators.Add(typeof(byte), calculateByte);
            calculators.Add(typeof(sbyte), calculateSByte);
            calculators.Add(typeof(char), calculateChar);
            calculators.Add(typeof(double), calculateDouble);
            calculators.Add(typeof(float), calculateFloat);
            calculators.Add(typeof(decimal), calculateDecimal);

#if NetStandard21
            unaryCalculators = DictionaryCreator.CreateHashObject<System.Type, Func<ExpressionType, object, object?>>();
#else
            unaryCalculators = DictionaryCreator.CreateHashObject<System.Type, Func<ExpressionType, object, object>>();
#endif
            unaryCalculators.Add(typeof(long), calculateLong);
            unaryCalculators.Add(typeof(uint), calculateUInt);
            unaryCalculators.Add(typeof(int), calculateInt);
            unaryCalculators.Add(typeof(ushort), calculateUShort);
            unaryCalculators.Add(typeof(short), calculateShort);
            unaryCalculators.Add(typeof(byte), calculateByte);
            unaryCalculators.Add(typeof(sbyte), calculateSByte);
            unaryCalculators.Add(typeof(char), calculateChar);
            unaryCalculators.Add(typeof(double), calculateDouble);
            unaryCalculators.Add(typeof(float), calculateFloat);
            unaryCalculators.Add(typeof(decimal), calculateDecimal);

            notCalculators = DictionaryCreator.CreateHashObject<System.Type, Func<object, object>>();
            notCalculators.Add(typeof(ulong), calculateNotULong);
            notCalculators.Add(typeof(long), calculateNotLong);
            notCalculators.Add(typeof(uint), calculateNotUInt);
            notCalculators.Add(typeof(int), calculateNotInt);
            notCalculators.Add(typeof(ushort), calculateNotUShort);
            notCalculators.Add(typeof(short), calculateNotShort);
            notCalculators.Add(typeof(byte), calculateNotByte);
            notCalculators.Add(typeof(sbyte), calculateNotSByte);
        }
    }
}
