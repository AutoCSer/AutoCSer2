using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallConstantParameterFunc<RT, CT> : CallParameterFunc<RT>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Func<CallConstantParameterFunc<RT, CT>, RT> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallConstantParameterFunc(Func<CallConstantParameterFunc<RT, CT>, RT> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallConstantParameterFunc(Func<CallConstantParameterFunc<RT, CT>, RT> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.InternalIndependentDeserializeNotReference(ref constantParameter);
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
            return new CallConstantParameterFunc<RT, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterFunc<RT> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallConstantParameterFunc<RT, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallConstantParameterFunc<RT, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallConstantParameterFunc<RT, CT>(System.Linq.Expressions.Expression.Lambda<Func<CallConstantParameterFunc<RT, CT>, RT>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallConstantParameterFunc<T, RT, CT> : CallParameterFunc<T, RT>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Func<CallConstantParameterFunc<T, RT, CT>, RT> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallConstantParameterFunc(Func<CallConstantParameterFunc<T, RT, CT>, RT> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallConstantParameterFunc(Func<CallConstantParameterFunc<T, RT, CT>, RT> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.InternalIndependentDeserializeNotReference(ref constantParameter);
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
            return new CallConstantParameterFunc<T, RT, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterFunc<T, RT> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallConstantParameterFunc<T, RT, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallConstantParameterFunc<T, RT, CT>.parameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterFunc<T, RT, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallConstantParameterFunc<T, RT, CT>(System.Linq.Expressions.Expression.Lambda<Func<CallConstantParameterFunc<T, RT, CT>, RT>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallConstantParameterFunc<T1, T2, RT, CT> : CallParameterFunc<T1, T2, RT>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Func<CallConstantParameterFunc<T1, T2, RT, CT>, RT> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallConstantParameterFunc(Func<CallConstantParameterFunc<T1, T2, RT, CT>, RT> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallConstantParameterFunc(Func<CallConstantParameterFunc<T1, T2, RT, CT>, RT> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.InternalIndependentDeserializeNotReference(ref constantParameter);
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
            return new CallConstantParameterFunc<T1, T2, RT, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterFunc<T1, T2, RT> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallConstantParameterFunc<T1, T2, RT, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallConstantParameterFunc<T1, T2, RT, CT>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterFunc<T1, T2, RT, CT>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterFunc<T1, T2, RT, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallConstantParameterFunc<T1, T2, RT, CT>(System.Linq.Expressions.Expression.Lambda<Func<CallConstantParameterFunc<T1, T2, RT, CT>, RT>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallConstantParameterFunc<T1, T2, T3, RT, CT> : CallParameterFunc<T1, T2, T3, RT>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Func<CallConstantParameterFunc<T1, T2, T3, RT, CT>, RT> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallConstantParameterFunc(Func<CallConstantParameterFunc<T1, T2, T3, RT, CT>, RT> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallConstantParameterFunc(Func<CallConstantParameterFunc<T1, T2, T3, RT, CT>, RT> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.InternalIndependentDeserializeNotReference(ref constantParameter);
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
            return new CallConstantParameterFunc<T1, T2, T3, RT, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterFunc<T1, T2, T3, RT> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallConstantParameterFunc<T1, T2, T3, RT, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallConstantParameterFunc<T1, T2, T3, RT, CT>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterFunc<T1, T2, T3, RT, CT>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterFunc<T1, T2, T3, RT, CT>.parameter3), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterFunc<T1, T2, T3, RT, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallConstantParameterFunc<T1, T2, T3, RT, CT>(System.Linq.Expressions.Expression.Lambda<Func<CallConstantParameterFunc<T1, T2, T3, RT, CT>, RT>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
}
