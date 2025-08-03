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
    internal sealed class CallAction : CallParameterAction
    {
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action expression;
        /// <summary>
        /// 封装表达式
        /// </summary>
        /// <param name="expression"></param>
        private CallAction(Action expression)
        {
            this.expression = expression;
        }
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        internal override void Call()
        {
            expression();
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
        internal static CallParameterAction CreateExpression(FormatDeserialize formatDeserialize)
        {
            return new CallAction(System.Linq.Expressions.Expression.Lambda<Action>(formatDeserialize.CreateExpression(null, EmptyArray<FieldInfo>.Array)).Compile());
        }
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CallAction<T> : CallParameterAction<T>
    {
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallAction<T>> expression;
        /// <summary>
        /// 封装表达式
        /// </summary>
        /// <param name="expression"></param>
        private CallAction(Action<CallAction<T>> expression)
        {
            this.expression = expression;
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
            return new CallAction<T>(expression);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallAction<T>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallAction<T>.parameter), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
            };
            return new CallAction<T>(System.Linq.Expressions.Expression.Lambda<Action<CallAction<T>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    internal sealed class CallAction<T1, T2> : CallParameterAction<T1, T2>
    {
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallAction<T1, T2>> expression;
        /// <summary>
        /// 封装表达式
        /// </summary>
        /// <param name="expression"></param>
        private CallAction(Action<CallAction<T1, T2>> expression)
        {
            this.expression = expression;
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
            return new CallAction<T1, T2>(expression);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T1, T2> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallAction<T1, T2>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallAction<T1, T2>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallAction<T1, T2>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
            };
            return new CallAction<T1, T2>(System.Linq.Expressions.Expression.Lambda<Action<CallAction<T1, T2>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    internal sealed class CallAction<T1, T2, T3> : CallParameterAction<T1, T2, T3>
    {
        /// <summary>
        /// 封装表达式委托
        /// </summary>
        private readonly Action<CallAction<T1, T2, T3>> expression;
        /// <summary>
        /// 封装表达式
        /// </summary>
        /// <param name="expression"></param>
        private CallAction(Action<CallAction<T1, T2, T3>> expression)
        {
            this.expression = expression;
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
            return new CallAction<T1, T2, T3>(expression);
        }
        /// <summary>
        /// 创建常量参数表达式
        /// </summary>
        /// <param name="formatDeserialize"></param>
        /// <returns></returns>
        internal static CallParameterAction<T1, T2, T3> CreateExpression(FormatDeserialize formatDeserialize)
        {
            Type parameterType = typeof(CallAction<T1, T2, T3>);
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(parameterType);
            FieldInfo[] parameterFields = new FieldInfo[]
            {
                parameterType.GetField(nameof(CallAction<T1, T2, T3>.parameter1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallAction<T1, T2, T3>.parameter2), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
                parameterType.GetField(nameof(CallAction<T1, T2, T3>.parameter3), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).notNull(),
            };
            return new CallAction<T1, T2, T3>(System.Linq.Expressions.Expression.Lambda<Action<CallAction<T1, T2, T3>>>(formatDeserialize.CreateExpression(parameter, parameterFields), parameter).Compile());
        }
    }
}
