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
    internal sealed class CallSimpleConstantParameterAction<CT> : CallParameterAction
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallSimpleConstantParameterAction<CT>> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallSimpleConstantParameterAction(Action<CallSimpleConstantParameterAction<CT>> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallSimpleConstantParameterAction(Action<CallSimpleConstantParameterAction<CT>> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.SimpleDeserialize(ref constantParameter);
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
            return new CallSimpleConstantParameterAction<CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallSimpleConstantParameterAction<CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallSimpleConstantParameterAction<CT>(System.Linq.Expressions.Expression.Lambda<Action<CallSimpleConstantParameterAction<CT>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallSimpleConstantParameterAction<T, CT> : CallParameterAction<T>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallSimpleConstantParameterAction<T, CT>> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallSimpleConstantParameterAction(Action<CallSimpleConstantParameterAction<T, CT>> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallSimpleConstantParameterAction(Action<CallSimpleConstantParameterAction<T, CT>> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.SimpleDeserialize(ref constantParameter);
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
            return new CallSimpleConstantParameterAction<T, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallSimpleConstantParameterAction<T, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T, CT>.parameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallSimpleConstantParameterAction<T, CT>(System.Linq.Expressions.Expression.Lambda<Action<CallSimpleConstantParameterAction<T, CT>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallSimpleConstantParameterAction<T1, T2, CT> : CallParameterAction<T1, T2>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallSimpleConstantParameterAction<T1, T2, CT>> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallSimpleConstantParameterAction(Action<CallSimpleConstantParameterAction<T1, T2, CT>> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallSimpleConstantParameterAction(Action<CallSimpleConstantParameterAction<T1, T2, CT>> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.SimpleDeserialize(ref constantParameter);
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
            return new CallSimpleConstantParameterAction<T1, T2, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T1, T2> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallSimpleConstantParameterAction<T1, T2, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T1, T2, CT>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T1, T2, CT>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T1, T2, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallSimpleConstantParameterAction<T1, T2, CT>(System.Linq.Expressions.Expression.Lambda<Action<CallSimpleConstantParameterAction<T1, T2, CT>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 常量参数封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="CT"></typeparam>
    internal sealed class CallSimpleConstantParameterAction<T1, T2, T3, CT> : CallParameterAction<T1, T2, T3>
        where CT : struct
    {
        /// <summary>
        /// 常量参数
        /// </summary>
        private CT constantParameter;
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallSimpleConstantParameterAction<T1, T2, T3, CT>> expression;
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        internal CallSimpleConstantParameterAction(Action<CallSimpleConstantParameterAction<T1, T2, T3, CT>> expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 常量参数封装表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="deserializer"></param>
        private CallSimpleConstantParameterAction(Action<CallSimpleConstantParameterAction<T1, T2, T3, CT>> expression, BinaryDeserializer deserializer)
        {
            this.expression = expression;
            deserializer.SimpleDeserialize(ref constantParameter);
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
            return new CallSimpleConstantParameterAction<T1, T2, T3, CT>(expression, deserializer);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T1, T2, T3> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallSimpleConstantParameterAction<T1, T2, T3, CT>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T1, T2, T3, CT>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T1, T2, T3, CT>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T1, T2, T3, CT>.parameter3), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallSimpleConstantParameterAction<T1, T2, T3, CT>.constantParameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull()
            };
            return new CallSimpleConstantParameterAction<T1, T2, T3, CT>(System.Linq.Expressions.Expression.Lambda<Action<CallSimpleConstantParameterAction<T1, T2, T3, CT>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
}
