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
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallConstantParameterAction<CT> : CallParameterAction
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallConstantParameterAction<CT>> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallConstantParameterAction(Action<CallConstantParameterAction<CT>> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallConstantParameterAction(Action<CallConstantParameterAction<CT>> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.InternalIndependentDeserializeNotReference(ref constantParameter);
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        internal override void Call()
        {
            expression(this);
        }
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override object Create(BinaryDeserializer deserializer)
        {
            return new CallConstantParameterAction<CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallConstantParameterAction<CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallConstantParameterAction<CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallConstantParameterAction<CT>(System.Linq.Expressions.Expression.Lambda<Action<CallConstantParameterAction<CT>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallConstantParameterAction<T, CT> : CallParameterAction<T>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallConstantParameterAction<T, CT>> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallConstantParameterAction(Action<CallConstantParameterAction<T, CT>> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallConstantParameterAction(Action<CallConstantParameterAction<T, CT>> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.InternalIndependentDeserializeNotReference(ref constantParameter);
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        internal override void Call()
        {
            expression(this);
        }
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override object Create(BinaryDeserializer deserializer)
        {
            return new CallConstantParameterAction<T, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallConstantParameterAction<T, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallConstantParameterAction<T, CT>.parameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterAction<T, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallConstantParameterAction<T, CT>(System.Linq.Expressions.Expression.Lambda<Action<CallConstantParameterAction<T, CT>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallConstantParameterAction<T1, T2, CT> : CallParameterAction<T1, T2>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallConstantParameterAction<T1, T2, CT>> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallConstantParameterAction(Action<CallConstantParameterAction<T1, T2, CT>> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallConstantParameterAction(Action<CallConstantParameterAction<T1, T2, CT>> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.InternalIndependentDeserializeNotReference(ref constantParameter);
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        internal override void Call()
        {
            expression(this);
        }
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override object Create(BinaryDeserializer deserializer)
        {
            return new CallConstantParameterAction<T1, T2, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T1, T2> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallConstantParameterAction<T1, T2, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallConstantParameterAction<T1, T2, CT>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterAction<T1, T2, CT>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterAction<T1, T2, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallConstantParameterAction<T1, T2, CT>(System.Linq.Expressions.Expression.Lambda<Action<CallConstantParameterAction<T1, T2, CT>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallConstantParameterAction<T1, T2, T3, CT> : CallParameterAction<T1, T2, T3>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallConstantParameterAction<T1, T2, T3, CT>> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallConstantParameterAction(Action<CallConstantParameterAction<T1, T2, T3, CT>> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallConstantParameterAction(Action<CallConstantParameterAction<T1, T2, T3, CT>> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.InternalIndependentDeserializeNotReference(ref constantParameter);
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        internal override void Call()
        {
            expression(this);
        }
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override object Create(BinaryDeserializer deserializer)
        {
            return new CallConstantParameterAction<T1, T2, T3, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T1, T2, T3> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallConstantParameterAction<T1, T2, T3, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallConstantParameterAction<T1, T2, T3, CT>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterAction<T1, T2, T3, CT>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterAction<T1, T2, T3, CT>.parameter3), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallConstantParameterAction<T1, T2, T3, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallConstantParameterAction<T1, T2, T3, CT>(System.Linq.Expressions.Expression.Lambda<Action<CallConstantParameterAction<T1, T2, T3, CT>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
}
