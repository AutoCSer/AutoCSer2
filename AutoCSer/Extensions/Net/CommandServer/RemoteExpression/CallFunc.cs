using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    internal sealed class CallFunc<RT> : CallParameterFunc<RT>
    {
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Func<RT> expression;
        /// <summary>
        /// 封装表达式
        /// </summary>
        /// <param name="expression"></param>
        private CallFunc(Func<RT> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        /// <returns></returns>
        internal override RT Call()
        {
            return expression();
        }
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override object Create(BinaryDeserializer deserializer)
        {
            return this;
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterFunc<RT> CreateExpression(FormatDeserialize formatDeserialize)
        {
            return new CallFunc<RT>(System.Linq.Expressions.Expression.Lambda<Func<RT>>(formatDeserialize.CreateExpression(null, EmptyArray<FieldInfo>.Array)).Compile());
        }
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    internal sealed class CallFunc<T, RT> : CallParameterFunc<T, RT>
    {
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Func<CallFunc<T, RT>, RT> expression;
        /// <summary>
        /// 封装表达式
        /// </summary>
        /// <param name="expression"></param>
        private CallFunc(Func<CallFunc<T, RT>, RT> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        /// <returns></returns>
        internal override RT Call()
        {
            return expression(this);
        }
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override object Create(BinaryDeserializer deserializer)
        {
            return new CallFunc<T, RT>(expression);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterFunc<T, RT> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallFunc<T, RT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallFunc<T, RT>.parameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
            };
            return new CallFunc<T, RT>(System.Linq.Expressions.Expression.Lambda<Func<CallFunc<T, RT>, RT>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="RT"></typeparam>
    internal sealed class CallFunc<T1, T2, RT> : CallParameterFunc<T1, T2, RT>
    {
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Func<CallFunc<T1, T2, RT>, RT> expression;
        /// <summary>
        /// 封装表达式
        /// </summary>
        /// <param name="expression"></param>
        private CallFunc(Func<CallFunc<T1, T2, RT>, RT> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        /// <returns></returns>
        internal override RT Call()
        {
            return expression(this);
        }
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override object Create(BinaryDeserializer deserializer)
        {
            return new CallFunc<T1, T2, RT>(expression);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterFunc<T1, T2, RT> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallFunc<T1, T2, RT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallFunc<T1, T2, RT>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallFunc<T1, T2, RT>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
            };
            return new CallFunc<T1, T2, RT>(System.Linq.Expressions.Expression.Lambda<Func<CallFunc<T1, T2, RT>, RT>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="RT"></typeparam>
    internal sealed class CallFunc<T1, T2, T3, RT> : CallParameterFunc<T1, T2, T3, RT>
    {
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Func<CallFunc<T1, T2, T3, RT>, RT> expression;
        /// <summary>
        /// 封装表达式
        /// </summary>
        /// <param name="expression"></param>
        private CallFunc(Func<CallFunc<T1, T2, T3, RT>, RT> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        /// <returns></returns>
        internal override RT Call()
        {
            return expression(this);
        }
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override object Create(BinaryDeserializer deserializer)
        {
            return new CallFunc<T1, T2, T3, RT>(expression);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterFunc<T1, T2, T3, RT> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallFunc<T1, T2, T3, RT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallFunc<T1, T2, T3, RT>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallFunc<T1, T2, T3, RT>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallFunc<T1, T2, T3, RT>.parameter3), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
            };
            return new CallFunc<T1, T2, T3, RT>(System.Linq.Expressions.Expression.Lambda<Func<CallFunc<T1, T2, T3, RT>, RT>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
}
