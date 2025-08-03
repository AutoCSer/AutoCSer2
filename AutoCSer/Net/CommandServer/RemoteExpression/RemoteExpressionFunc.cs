using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer.RemoteExpression;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程表达式委托
    /// </summary>
    public static class RemoteExpressionFunc
    {
        /// <summary>
        /// 获取远程表达式委托
        /// </summary>
        /// <typeparam name="RT">返回值类型</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static RemoteExpressionFunc<RT> Get<RT>(System.Linq.Expressions.Expression<Func<RT>> expression)
        {
            return new RemoteExpressionFunc<RT>(expression);
        }
        ///// <summary>
        ///// 获取远程表达式委托
        ///// </summary>
        ///// <typeparam name="T">参数类型</typeparam>
        ///// <typeparam name="RT">返回值类型</typeparam>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static RemoteExpressionFunc<T, RT> Get<T, RT>(System.Linq.Expressions.Expression<Func<T, RT>> expression)
        //{
        //    return new RemoteExpressionFunc<T, RT>(expression);
        //}
        ///// <summary>
        ///// 获取远程表达式委托
        ///// </summary>
        ///// <typeparam name="T1">参数类型</typeparam>
        ///// <typeparam name="T2">参数类型</typeparam>
        ///// <typeparam name="RT">返回值类型</typeparam>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static RemoteExpressionFunc<T1, T2, RT> Get<T1, T2, RT>(System.Linq.Expressions.Expression<Func<T1, T2, RT>> expression)
        //{
        //    return new RemoteExpressionFunc<T1, T2, RT>(expression);
        //}
        ///// <summary>
        ///// 获取远程表达式委托
        ///// </summary>
        ///// <typeparam name="T1">参数类型</typeparam>
        ///// <typeparam name="T2">参数类型</typeparam>
        ///// <typeparam name="T3">参数类型</typeparam>
        ///// <typeparam name="RT">返回值类型</typeparam>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static RemoteExpressionFunc<T1, T2, T3, RT> Get<T1, T2, T3, RT>(System.Linq.Expressions.Expression<Func<T1, T2, T3, RT>> expression)
        //{
        //    return new RemoteExpressionFunc<T1, T2, T3, RT>(expression);
        //}
    }
    /// <summary>
    /// 远程表达式委托（依赖内存数据状态，不支持持久化）
    /// </summary>
    /// <typeparam name="RT">返回值类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionFunc<RT> : AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<RT>>
    {
        /// <summary>
        /// 服务端委托
        /// </summary>
#if NetStandard21
        private CallParameterFunc<RT>? function;
#else
        private CallParameterFunc<RT> function;
#endif
        /// <summary>
        /// 服务端反序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return state == RemoteExpressionSerializeStateEnum.Success; } }
        /// <summary>
        /// 客户端 Lambda 表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression<Func<RT>> expression;
        /// <summary>
        /// 远程表达式委托
        /// </summary>
        /// <param name="expression">客户端 Lambda 表达式</param>
        public RemoteExpressionFunc(System.Linq.Expressions.Expression<Func<RT>> expression)
        {
            this.expression = expression;
            function = null;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
        ///// <summary>
        ///// Implicit conversion
        ///// </summary>
        ///// <param name="expression"></param>
        //public static implicit operator RemoteExpressionFunc<RT>(System.Linq.Expressions.Expression<Func<RT>> expression) { return new RemoteExpressionFunc<RT>(expression); }
        /// <summary>
        /// 获取服务端反序列化状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum GetState()
        {
            return state;
        }
        /// <summary>
        /// 委托调用
        /// </summary>
        /// <returns></returns>
        public RT Call()
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) return function.notNull().Call();
            throw new MissingMethodException(state.ToString());
        }

        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<RT>>.Serialize(BinarySerializer serializer)
        {
            ClientMetadata.Serialize(serializer, parameterTypes, expression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<RT>>.Deserialize(BinaryDeserializer deserializer)
        {
            DelegateDeserializer delegateDeserializer = DelegateDeserializer.Deserialize(deserializer, parameterTypes, ref this.state);
            if (delegateDeserializer.State == RemoteExpressionSerializeStateEnum.Success)
            {
                var expression = default(CallDelegate);
                var expressions = default(Dictionary<SerializeDataKey, CallDelegate>);
                Dictionary<SerializeDataKey, CallDelegate>[] expressionArray = delegateDeserializer.LockExpressionArray();
                try
                {
                    expression = delegateDeserializer.GetExpression(expressionArray, (byte)DelegateTypeEnum.Func, out expressions);
                    if (expression == null)
                    {
                        if (delegateDeserializer.SerializeInfo.ConstantParameterCount != 0)
                        {
                            ServerMethodParameter constantParameterType = delegateDeserializer.GetConstantParameterType();
                            expression = (constantParameterType.IsSimpleSerialize ? typeof(CallSimpleConstantParameterFunc<,>) : typeof(CallConstantParameterFunc<,>)).MakeGenericType(typeof(RT), constantParameterType.Type)
                                .GetMethod(nameof(CallFunc<RT>.CreateExpression), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).notNull()
                                .Invoke(null, delegateDeserializer.Socket.GetRemoteExpressionParameters()).notNullCastType<CallDelegate>();
                        }
                        else expression = CallFunc<RT>.CreateExpression(delegateDeserializer.GetFormatDeserialize());
                        expressions.Add(new SerializeDataKey(ref delegateDeserializer.SerializeInfo.Key), expression);
                    }
                }
                finally { Monitor.Exit(expressionArray); }
                deserializer.Current = delegateDeserializer.SerializeInfo.End;
                function = (CallParameterFunc<RT>)expression.Create(deserializer);
                if (deserializer.State == BinarySerialize.DeserializeStateEnum.Success)
                {
                    this.state = RemoteExpressionSerializeStateEnum.Success;
                    return;
                }
            }
        }

        /// <summary>
        /// 泛型参数类型集合
        /// </summary>
        private static readonly Type[] parameterTypes = new Type[] { typeof(RT) };
    }
    /// <summary>
    /// 远程表达式委托（依赖内存数据状态，不支持持久化）
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    /// <typeparam name="RT">返回值类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionFunc<T, RT> : AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T, RT>>
    {
        /// <summary>
        /// 服务端委托
        /// </summary>
#if NetStandard21
        private CallParameterFunc<T, RT>? function;
#else
        private CallParameterFunc<T, RT> function;
#endif
        /// <summary>
        /// 服务端反序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return state == RemoteExpressionSerializeStateEnum.Success; } }
        /// <summary>
        /// 客户端 Lambda 表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression<Func<T, RT>> expression;
        /// <summary>
        /// 远程表达式委托
        /// </summary>
        /// <param name="expression">客户端 Lambda 表达式</param>
        public RemoteExpressionFunc(System.Linq.Expressions.Expression<Func<T, RT>> expression)
        {
            this.expression = expression;
            function = null;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
        ///// <summary>
        ///// Implicit conversion
        ///// </summary>
        ///// <param name="expression"></param>
        //public static implicit operator RemoteExpressionFunc<T, RT>(System.Linq.Expressions.Expression<Func<T, RT>> expression) { return new RemoteExpressionFunc<T, RT>(expression); }
        /// <summary>
        /// 获取服务端反序列化状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum GetState()
        {
            return state;
        }
        /// <summary>
        /// 委托调用
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public RT Call(T parameter)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) return function.notNull().Call(parameter);
            throw new MissingMethodException(state.ToString());
        }

        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T, RT>>.Serialize(BinarySerializer serializer)
        {
            ClientMetadata.Serialize(serializer, parameterTypes, expression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T, RT>>.Deserialize(BinaryDeserializer deserializer)
        {
            DelegateDeserializer delegateDeserializer = DelegateDeserializer.Deserialize(deserializer, parameterTypes, ref this.state);
            if (delegateDeserializer.State == RemoteExpressionSerializeStateEnum.Success)
            {
                var expression = default(CallDelegate);
                var expressions = default(Dictionary<SerializeDataKey, CallDelegate>);
                Dictionary<SerializeDataKey, CallDelegate>[] expressionArray = delegateDeserializer.LockExpressionArray();
                try
                {
                    expression = delegateDeserializer.GetExpression(expressionArray, (byte)DelegateTypeEnum.Func1, out expressions);
                    if (expression == null)
                    {
                        if (delegateDeserializer.SerializeInfo.ConstantParameterCount != 0)
                        {
                            ServerMethodParameter constantParameterType = delegateDeserializer.GetConstantParameterType();
                            expression = (constantParameterType.IsSimpleSerialize? typeof(CallSimpleConstantParameterFunc<,,>) : typeof(CallConstantParameterFunc<,,>)) .MakeGenericType(typeof(T), typeof(RT), constantParameterType.Type)
                                .GetMethod(nameof(CallFunc<T, RT>.CreateExpression), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).notNull()
                                .Invoke(null, delegateDeserializer.Socket.GetRemoteExpressionParameters()).notNullCastType<CallDelegate>();
                        }
                        else expression = CallFunc<T, RT>.CreateExpression(delegateDeserializer.GetFormatDeserialize());
                        expressions.Add(new SerializeDataKey(ref delegateDeserializer.SerializeInfo.Key), expression);
                    }
                }
                finally { Monitor.Exit(expressionArray); }
                deserializer.Current = delegateDeserializer.SerializeInfo.End;
                function = (CallParameterFunc<T, RT>)expression.Create(deserializer);
                if (deserializer.State == BinarySerialize.DeserializeStateEnum.Success)
                {
                    this.state = RemoteExpressionSerializeStateEnum.Success;
                    return;
                }
            }
        }

        /// <summary>
        /// 泛型参数类型集合
        /// </summary>
        private static readonly Type[] parameterTypes = new Type[] { typeof(T), typeof(RT) };
    }
    /// <summary>
    /// 远程表达式委托（依赖内存数据状态，不支持持久化）
    /// </summary>
    /// <typeparam name="T1">参数类型</typeparam>
    /// <typeparam name="T2">参数类型</typeparam>
    /// <typeparam name="RT">返回值类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionFunc<T1, T2, RT> : AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T1, T2, RT>>
    {
        /// <summary>
        /// 服务端委托
        /// </summary>
#if NetStandard21
        private CallParameterFunc<T1, T2, RT>? function;
#else
        private CallParameterFunc<T1, T2, RT> function;
#endif
        /// <summary>
        /// 服务端反序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return state == RemoteExpressionSerializeStateEnum.Success; } }
        /// <summary>
        /// 客户端 Lambda 表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression<Func<T1, T2, RT>> expression;
        /// <summary>
        /// 远程表达式委托
        /// </summary>
        /// <param name="expression">客户端 Lambda 表达式</param>
        public RemoteExpressionFunc(System.Linq.Expressions.Expression<Func<T1, T2, RT>> expression)
        {
            this.expression = expression;
            function = null;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
        ///// <summary>
        ///// Implicit conversion
        ///// </summary>
        ///// <param name="expression"></param>
        //public static implicit operator RemoteExpressionFunc<T1, T2, RT>(System.Linq.Expressions.Expression<Func<T1, T2, RT>> expression) { return new RemoteExpressionFunc<T1, T2, RT>(expression); }
        /// <summary>
        /// 获取服务端反序列化状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum GetState()
        {
            return state;
        }
        /// <summary>
        /// 委托调用
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns></returns>
        public RT Call(T1 parameter1, T2 parameter2)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) return function.notNull().Call(parameter1, parameter2);
            throw new MissingMethodException(state.ToString());
        }

        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T1, T2, RT>>.Serialize(BinarySerializer serializer)
        {
            ClientMetadata.Serialize(serializer, parameterTypes, expression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T1, T2, RT>>.Deserialize(BinaryDeserializer deserializer)
        {
            DelegateDeserializer delegateDeserializer = DelegateDeserializer.Deserialize(deserializer, parameterTypes, ref this.state);
            if (delegateDeserializer.State == RemoteExpressionSerializeStateEnum.Success)
            {
                var expression = default(CallDelegate);
                var expressions = default(Dictionary<SerializeDataKey, CallDelegate>);
                Dictionary<SerializeDataKey, CallDelegate>[] expressionArray = delegateDeserializer.LockExpressionArray();
                try
                {
                    expression = delegateDeserializer.GetExpression(expressionArray, (byte)DelegateTypeEnum.Func2, out expressions);
                    if (expression == null)
                    {
                        if (delegateDeserializer.SerializeInfo.ConstantParameterCount != 0)
                        {
                            ServerMethodParameter constantParameterType = delegateDeserializer.GetConstantParameterType();
                            expression = (constantParameterType.IsSimpleSerialize ? typeof(CallSimpleConstantParameterFunc<,,,>) : typeof(CallConstantParameterFunc<,,,>)).MakeGenericType(typeof(T1), typeof(T2), typeof(RT), constantParameterType.Type)
                                .GetMethod(nameof(CallFunc<T1, T2, RT>.CreateExpression), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).notNull()
                                .Invoke(null, delegateDeserializer.Socket.GetRemoteExpressionParameters()).notNullCastType<CallDelegate>();
                        }
                        else expression = CallFunc<T1, T2, RT>.CreateExpression(delegateDeserializer.GetFormatDeserialize());
                        expressions.Add(new SerializeDataKey(ref delegateDeserializer.SerializeInfo.Key), expression);
                    }
                }
                finally { Monitor.Exit(expressionArray); }
                deserializer.Current = delegateDeserializer.SerializeInfo.End;
                function = (CallParameterFunc<T1, T2, RT>)expression.Create(deserializer);
                if (deserializer.State == BinarySerialize.DeserializeStateEnum.Success)
                {
                    this.state = RemoteExpressionSerializeStateEnum.Success;
                    return;
                }
            }
        }

        /// <summary>
        /// 泛型参数类型集合
        /// </summary>
        private static readonly Type[] parameterTypes = new Type[] { typeof(T1), typeof(T2), typeof(RT) };
    }
    /// <summary>
    /// 远程表达式委托（依赖内存数据状态，不支持持久化）
    /// </summary>
    /// <typeparam name="T1">参数类型</typeparam>
    /// <typeparam name="T2">参数类型</typeparam>
    /// <typeparam name="T3">参数类型</typeparam>
    /// <typeparam name="RT">返回值类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionFunc<T1, T2, T3, RT> : AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T1, T2, T3, RT>>
    {
        /// <summary>
        /// 服务端委托
        /// </summary>
#if NetStandard21
        private CallParameterFunc<T1, T2, T3, RT>? function;
#else
        private CallParameterFunc<T1, T2, T3, RT> function;
#endif
        /// <summary>
        /// 服务端反序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return state == RemoteExpressionSerializeStateEnum.Success; } }
        /// <summary>
        /// 客户端 Lambda 表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression<Func<T1, T2, T3, RT>> expression;
        /// <summary>
        /// 远程表达式委托
        /// </summary>
        /// <param name="expression">客户端 Lambda 表达式</param>
        public RemoteExpressionFunc(System.Linq.Expressions.Expression<Func<T1, T2, T3, RT>> expression)
        {
            this.expression = expression;
            function = null;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
        ///// <summary>
        ///// Implicit conversion
        ///// </summary>
        ///// <param name="expression"></param>
        //public static implicit operator RemoteExpressionFunc<T1, T2, T3, RT>(System.Linq.Expressions.Expression<Func<T1, T2, T3, RT>> expression) { return new RemoteExpressionFunc<T1, T2, T3, RT>(expression); }
        /// <summary>
        /// 获取服务端反序列化状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum GetState()
        {
            return state;
        }
        /// <summary>
        /// 委托调用
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <returns></returns>
        public RT Call(T1 parameter1, T2 parameter2, T3 parameter3)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) return function.notNull().Call(parameter1, parameter2, parameter3);
            throw new MissingMethodException(state.ToString());
        }

        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T1, T2, T3, RT>>.Serialize(BinarySerializer serializer)
        {
            ClientMetadata.Serialize(serializer, parameterTypes, expression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionFunc<T1, T2, T3, RT>>.Deserialize(BinaryDeserializer deserializer)
        {
            DelegateDeserializer delegateDeserializer = DelegateDeserializer.Deserialize(deserializer, parameterTypes, ref this.state);
            if (delegateDeserializer.State == RemoteExpressionSerializeStateEnum.Success)
            {
                var expression = default(CallDelegate);
                var expressions = default(Dictionary<SerializeDataKey, CallDelegate>);
                Dictionary<SerializeDataKey, CallDelegate>[] expressionArray = delegateDeserializer.LockExpressionArray();
                try
                {
                    expression = delegateDeserializer.GetExpression(expressionArray, (byte)DelegateTypeEnum.Func3, out expressions);
                    if (expression == null)
                    {
                        if (delegateDeserializer.SerializeInfo.ConstantParameterCount != 0)
                        {
                            ServerMethodParameter constantParameterType = delegateDeserializer.GetConstantParameterType();
                            expression = (constantParameterType.IsSimpleSerialize ? typeof(CallSimpleConstantParameterFunc<,,,,>) : typeof(CallConstantParameterFunc<,,,,>)).MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(RT), constantParameterType.Type)
                                .GetMethod(nameof(CallFunc<T1, T2, T3, RT>.CreateExpression), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).notNull()
                                .Invoke(null, delegateDeserializer.Socket.GetRemoteExpressionParameters()).notNullCastType<CallDelegate>();
                        }
                        else expression = CallFunc<T1, T2, T3, RT>.CreateExpression(delegateDeserializer.GetFormatDeserialize());
                        expressions.Add(new SerializeDataKey(ref delegateDeserializer.SerializeInfo.Key), expression);
                    }
                }
                finally { Monitor.Exit(expressionArray); }
                deserializer.Current = delegateDeserializer.SerializeInfo.End;
                function = (CallParameterFunc<T1, T2, T3, RT>)expression.Create(deserializer);
                if (deserializer.State == BinarySerialize.DeserializeStateEnum.Success)
                {
                    this.state = RemoteExpressionSerializeStateEnum.Success;
                    return;
                }
            }
        }

        /// <summary>
        /// 泛型参数类型集合
        /// </summary>
        private static readonly Type[] parameterTypes = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(RT) };
    }
}
