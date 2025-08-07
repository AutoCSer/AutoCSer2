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
    /// Remote expression delegate Action (It relies on the state of in-memory data and does not support persistence)
    /// 远程表达式委托（依赖内存数据状态，不支持持久化）
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionAction : AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction>
    {
        /// <summary>
        /// Server side delegate
        /// 服务端委托
        /// </summary>
#if NetStandard21
        private CallParameterAction? action;
#else
        private CallParameterAction action;
#endif
        /// <summary>
        /// Server deserialization state
        /// 服务端反序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// Whether the server deserialization was successful
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return state == RemoteExpressionSerializeStateEnum.Success; } }
        /// <summary>
        /// Client-side Lambda expression
        /// 客户端 Lambda 表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression<Action> expression;
        /// <summary>
        /// Remote expression delegate
        /// 远程表达式委托
        /// </summary>
        /// <param name="expression">Client-side Lambda expression
        /// 客户端 Lambda 表达式</param>
        public RemoteExpressionAction(System.Linq.Expressions.Expression<Action> expression)
        {
            this.expression = expression;
            action = null;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
        ///// <summary>
        ///// Implicit conversion
        ///// </summary>
        ///// <param name="expression"></param>
        //public static implicit operator RemoteExpressionAction(System.Linq.Expressions.Expression<Action> expression) { return new RemoteExpressionAction(expression); }
        /// <summary>
        /// Get the deserialization state of the server
        /// 获取服务端反序列化状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum GetState()
        {
            return state;
        }
        /// <summary>
        /// Call the service delegate
        /// 调用服务端委托
        /// </summary>
        public void Call()
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) action.notNull().Call();
            else throw new MissingMethodException(state.ToString());
        }
        /// <summary>
        /// When deserialization is successful, the server delegate is called
        /// 反序列化成功时调用服务端委托
        /// </summary>
        /// <returns>Server deserialization state
        /// 服务端反序列化状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum CallState()
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) action.notNull().Call();
            return state;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction>.Serialize(BinarySerializer serializer)
        {
            ClientMetadata.Serialize(serializer, EmptyArray<Type>.Array, expression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction>.Deserialize(BinaryDeserializer deserializer)
        {
            DelegateDeserializer delegateDeserializer = DelegateDeserializer.Deserialize(deserializer, EmptyArray<Type>.Array, ref this.state);
            if (delegateDeserializer.State == RemoteExpressionSerializeStateEnum.Success)
            {
                var expression = default(CallDelegate);
                var expressions = default(Dictionary<HashBuffer, CallDelegate>);
                Dictionary<HashBuffer, CallDelegate>[] expressionArray = delegateDeserializer.LockExpressionArray();
                try
                {
                    expression = delegateDeserializer.GetExpression(expressionArray, (byte)DelegateTypeEnum.Action, out expressions);
                    if (expression == null)
                    {
                        if (delegateDeserializer.SerializeInfo.ConstantParameterCount != 0)
                        {
                            ServerMethodParameter constantParameterType = delegateDeserializer.GetConstantParameterType();
                            expression = (constantParameterType.IsSimpleSerialize ? typeof(CallSimpleConstantParameterAction<>): typeof(CallConstantParameterAction<>)).MakeGenericType(constantParameterType.Type)
                                .GetMethod(nameof(CallAction.CreateExpression), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).notNull()
                                .Invoke(null, delegateDeserializer.Socket.GetRemoteExpressionParameters()).notNullCastType<CallDelegate>();
                        }
                        else expression = CallAction.CreateExpression(delegateDeserializer.GetFormatDeserialize());
                        expressions.Add(new HashBuffer(ref delegateDeserializer.SerializeInfo.Key), expression);
                    }
                }
                finally { Monitor.Exit(expressionArray); }
                deserializer.Current = delegateDeserializer.SerializeInfo.End;
                action = (CallParameterAction)expression.Create(deserializer);
                if (deserializer.State == BinarySerialize.DeserializeStateEnum.Success)
                {
                    this.state = RemoteExpressionSerializeStateEnum.Success;
                    return;
                }
            }
        }

        ///// <summary>
        ///// 获取远程表达式委托
        ///// </summary>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static RemoteExpressionAction Get(System.Linq.Expressions.Expression<Action> expression)
        //{
        //    return new RemoteExpressionAction(expression);
        //}
        ///// <summary>
        ///// 获取远程表达式委托
        ///// </summary>
        ///// <typeparam name="T">参数类型</typeparam>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static RemoteExpressionAction<T> Get<T>(System.Linq.Expressions.Expression<Action<T>> expression)
        //{
        //    return new RemoteExpressionAction<T>(expression);
        //}
        ///// <summary>
        ///// 获取远程表达式委托
        ///// </summary>
        ///// <typeparam name="T1">参数类型</typeparam>
        ///// <typeparam name="T2">参数类型</typeparam>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static RemoteExpressionAction<T1, T2> Get<T1, T2>(System.Linq.Expressions.Expression<Action<T1, T2>> expression)
        //{
        //    return new RemoteExpressionAction<T1, T2>(expression);
        //}
        ///// <summary>
        ///// 获取远程表达式委托
        ///// </summary>
        ///// <typeparam name="T1">参数类型</typeparam>
        ///// <typeparam name="T2">参数类型</typeparam>
        ///// <typeparam name="T3">参数类型</typeparam>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static RemoteExpressionAction<T1, T2, T3> Get<T1, T2, T3>(System.Linq.Expressions.Expression<Action<T1, T2, T3>> expression)
        //{
        //    return new RemoteExpressionAction<T1, T2, T3>(expression);
        //}
    }
    /// <summary>
    /// Remote expression delegate Action{T} (It relies on the state of in-memory data and does not support persistence)
    /// 远程表达式委托（依赖内存数据状态，不支持持久化）
    /// </summary>
    /// <typeparam name="T">Parameter type
    /// 参数类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionAction<T> : AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T>>
    {
        /// <summary>
        /// Server side delegate
        /// 服务端委托
        /// </summary>
#if NetStandard21
        private CallParameterAction<T>? action;
#else
        private CallParameterAction<T> action;
#endif
        /// <summary>
        /// Server deserialization state
        /// 服务端反序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// Whether the server deserialization was successful
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return state == RemoteExpressionSerializeStateEnum.Success; } }
        /// <summary>
        /// Client-side Lambda expression
        /// 客户端 Lambda 表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression<Action<T>> expression;
        /// <summary>
        /// Remote expression delegate
        /// 远程表达式委托
        /// </summary>
        /// <param name="expression">Client-side Lambda expression
        /// 客户端 Lambda 表达式</param>
        public RemoteExpressionAction(System.Linq.Expressions.Expression<Action<T>> expression)
        {
            this.expression = expression;
            action = null;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
        ///// <summary>
        ///// Implicit conversion
        ///// </summary>
        ///// <param name="expression"></param>
        //public static implicit operator RemoteExpressionAction<T>(System.Linq.Expressions.Expression<Action<T>> expression) { return new RemoteExpressionAction<T>(expression); }
        /// <summary>
        /// Get the deserialization state of the server
        /// 获取服务端反序列化状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum GetState()
        {
            return state;
        }
        /// <summary>
        /// Call the service delegate
        /// 调用服务端委托
        /// </summary>
        /// <param name="parameter"></param>
        public void Call(T parameter)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) action.notNull().Call(parameter);
            else throw new MissingMethodException(state.ToString());
        }
        /// <summary>
        /// When deserialization is successful, the server delegate is called
        /// 反序列化成功时调用服务端委托
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Server deserialization state
        /// 服务端反序列化状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum CallState(T parameter)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) action.notNull().Call(parameter);
            return state;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T>>.Serialize(BinarySerializer serializer)
        {
            ClientMetadata.Serialize(serializer, parameterTypes, expression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T>>.Deserialize(BinaryDeserializer deserializer)
        {
            DelegateDeserializer delegateDeserializer = DelegateDeserializer.Deserialize(deserializer, parameterTypes, ref this.state);
            if (delegateDeserializer.State == RemoteExpressionSerializeStateEnum.Success)
            {
                var expression = default(CallDelegate);
                var expressions = default(Dictionary<HashBuffer, CallDelegate>);
                Dictionary<HashBuffer, CallDelegate>[] expressionArray = delegateDeserializer.LockExpressionArray();
                try
                {
                    expression = delegateDeserializer.GetExpression(expressionArray, (byte)DelegateTypeEnum.Action1, out expressions);
                    if (expression == null)
                    {
                        if (delegateDeserializer.SerializeInfo.ConstantParameterCount != 0)
                        {
                            ServerMethodParameter constantParameterType = delegateDeserializer.GetConstantParameterType();
                            expression = (constantParameterType.IsSimpleSerialize? typeof(CallSimpleConstantParameterAction<,>): typeof(CallConstantParameterAction<,>)).MakeGenericType(typeof(T), constantParameterType.Type)
                                .GetMethod(nameof(CallAction<T>.CreateExpression), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).notNull()
                                .Invoke(null, delegateDeserializer.Socket.GetRemoteExpressionParameters()).notNullCastType<CallDelegate>();
                        }
                        else expression = CallAction<T>.CreateExpression(delegateDeserializer.GetFormatDeserialize());
                        expressions.Add(new HashBuffer(ref delegateDeserializer.SerializeInfo.Key), expression);
                    }
                }
                finally { Monitor.Exit(expressionArray); }
                deserializer.Current = delegateDeserializer.SerializeInfo.End;
                if (deserializer.State == BinarySerialize.DeserializeStateEnum.Success)
                {
                    action = (CallParameterAction<T>)expression.Create(deserializer);
                    this.state = RemoteExpressionSerializeStateEnum.Success;
                    return;
                }
            }
        }

        /// <summary>
        /// A collection of generic parameter types
        /// 泛型参数类型集合
        /// </summary>
        private static readonly Type[] parameterTypes = new Type[] { typeof(T) };
    }
    /// <summary>
    /// Remote expression delegate Action{T1, T2} (It relies on the state of in-memory data and does not support persistence)
    /// 远程表达式委托（依赖内存数据状态，不支持持久化）
    /// </summary>
    /// <typeparam name="T1">Parameter type
    /// 参数类型</typeparam>
    /// <typeparam name="T2">Parameter type
    /// 参数类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionAction<T1, T2> : AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T1, T2>>
    {
        /// <summary>
        /// Server side delegate
        /// 服务端委托
        /// </summary>
#if NetStandard21
        private CallParameterAction<T1, T2>? action;
#else
        private CallParameterAction<T1, T2> action;
#endif
        /// <summary>
        /// Server deserialization state
        /// 服务端反序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// Whether the server deserialization was successful
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return state == RemoteExpressionSerializeStateEnum.Success; } }
        /// <summary>
        /// Client-side Lambda expression
        /// 客户端 Lambda 表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression<Action<T1, T2>> expression;
        /// <summary>
        /// Remote expression delegate
        /// 远程表达式委托
        /// </summary>
        /// <param name="expression">Client-side Lambda expression
        /// 客户端 Lambda 表达式</param>
        public RemoteExpressionAction(System.Linq.Expressions.Expression<Action<T1, T2>> expression)
        {
            this.expression = expression;
            action = null;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
        ///// <summary>
        ///// Implicit conversion
        ///// </summary>
        ///// <param name="expression"></param>
        //public static implicit operator RemoteExpressionAction<T1, T2>(System.Linq.Expressions.Expression<Action<T1, T2>> expression) { return new RemoteExpressionAction<T1, T2>(expression); }
        /// <summary>
        /// Get the deserialization state of the server
        /// 获取服务端反序列化状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum GetState()
        {
            return state;
        }
        /// <summary>
        /// Call the service delegate
        /// 调用服务端委托
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        public void Call(T1 parameter1, T2 parameter2)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) action.notNull().Call(parameter1, parameter2);
            else throw new MissingMethodException(state.ToString());
        }
        /// <summary>
        /// When deserialization is successful, the server delegate is called
        /// 反序列化成功时调用服务端委托
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns>Server deserialization state
        /// 服务端反序列化状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum CallState(T1 parameter1, T2 parameter2)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) action.notNull().Call(parameter1, parameter2);
            return state;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T1, T2>>.Serialize(BinarySerializer serializer)
        {
            ClientMetadata.Serialize(serializer, parameterTypes, expression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T1, T2>>.Deserialize(BinaryDeserializer deserializer)
        {
            DelegateDeserializer delegateDeserializer = DelegateDeserializer.Deserialize(deserializer, parameterTypes, ref this.state);
            if (delegateDeserializer.State == RemoteExpressionSerializeStateEnum.Success)
            {
                var expression = default(CallDelegate);
                var expressions = default(Dictionary<HashBuffer, CallDelegate>);
                Dictionary<HashBuffer, CallDelegate>[] expressionArray = delegateDeserializer.LockExpressionArray();
                try
                {
                    expression = delegateDeserializer.GetExpression(expressionArray, (byte)DelegateTypeEnum.Action2, out expressions);
                    if (expression == null)
                    {
                        if (delegateDeserializer.SerializeInfo.ConstantParameterCount != 0)
                        {
                            ServerMethodParameter constantParameterType = delegateDeserializer.GetConstantParameterType();
                            expression = (constantParameterType.IsSimpleSerialize? typeof(CallSimpleConstantParameterAction<,,>): typeof(CallConstantParameterAction<,,>)).MakeGenericType(typeof(T1), typeof(T2), constantParameterType.Type)
                                .GetMethod(nameof(CallAction<T1, T2>.CreateExpression), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).notNull()
                                .Invoke(null, delegateDeserializer.Socket.GetRemoteExpressionParameters()).notNullCastType<CallDelegate>();
                        }
                        else expression = CallAction<T1, T2>.CreateExpression(delegateDeserializer.GetFormatDeserialize());
                        expressions.Add(new HashBuffer(ref delegateDeserializer.SerializeInfo.Key), expression);
                    }
                }
                finally { Monitor.Exit(expressionArray); }
                deserializer.Current = delegateDeserializer.SerializeInfo.End;
                if (deserializer.State == BinarySerialize.DeserializeStateEnum.Success)
                {
                    action = (CallParameterAction<T1, T2>)expression.Create(deserializer);
                    this.state = RemoteExpressionSerializeStateEnum.Success;
                    return;
                }
            }
        }

        /// <summary>
        /// A collection of generic parameter types
        /// 泛型参数类型集合
        /// </summary>
        private static readonly Type[] parameterTypes = new Type[] { typeof(T1), typeof(T2) };
    }
    /// <summary>
    /// Remote expression delegate Action{T1, T2, T3} (It relies on the state of in-memory data and does not support persistence)
    /// 远程表达式委托（依赖内存数据状态，不支持持久化）
    /// </summary>
    /// <typeparam name="T1">Parameter type
    /// 参数类型</typeparam>
    /// <typeparam name="T2">Parameter type
    /// 参数类型</typeparam>
    /// <typeparam name="T3">Parameter type
    /// 参数类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteExpressionAction<T1, T2, T3> : AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T1, T2, T3>>
    {
        /// <summary>
        /// Server side delegate
        /// 服务端委托
        /// </summary>
#if NetStandard21
        private CallParameterAction<T1, T2, T3>? action;
#else
        private CallParameterAction<T1, T2, T3> action;
#endif
        /// <summary>
        /// Server deserialization state
        /// 服务端反序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// Whether the server deserialization was successful
        /// 服务端反序列化是否成功
        /// </summary>
        public bool IsSuccess { get { return state == RemoteExpressionSerializeStateEnum.Success; } }
        /// <summary>
        /// Client-side Lambda expression
        /// 客户端 Lambda 表达式
        /// </summary>
        private readonly System.Linq.Expressions.Expression<Action<T1, T2, T3>> expression;
        /// <summary>
        /// Remote expression delegate
        /// 远程表达式委托
        /// </summary>
        /// <param name="expression">Client-side Lambda expression
        /// 客户端 Lambda 表达式</param>
        public RemoteExpressionAction(System.Linq.Expressions.Expression<Action<T1, T2, T3>> expression)
        {
            this.expression = expression;
            action = null;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
        ///// <summary>
        ///// Implicit conversion
        ///// </summary>
        ///// <param name="expression"></param>
        //public static implicit operator RemoteExpressionAction<T1, T2, T3>(System.Linq.Expressions.Expression<Action<T1, T2, T3>> expression) { return new RemoteExpressionAction<T1, T2, T3>(expression); }
        /// <summary>
        /// Get the deserialization state of the server
        /// 获取服务端反序列化状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum GetState()
        {
            return state;
        }
        /// <summary>
        /// Call the service delegate
        /// 调用服务端委托
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        public void Call(T1 parameter1, T2 parameter2, T3 parameter3)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) action.notNull().Call(parameter1, parameter2, parameter3);
            else throw new MissingMethodException(state.ToString());
        }
        /// <summary>
        /// When deserialization is successful, the server delegate is called
        /// 反序列化成功时调用服务端委托
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <returns>Server deserialization state
        /// 服务端反序列化状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public RemoteExpressionSerializeStateEnum CallState(T1 parameter1, T2 parameter2, T3 parameter3)
        {
            if (state == RemoteExpressionSerializeStateEnum.Success) action.notNull().Call(parameter1, parameter2, parameter3);
            return state;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T1, T2, T3>>.Serialize(BinarySerializer serializer)
        {
            ClientMetadata.Serialize(serializer, parameterTypes, expression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RemoteExpressionAction<T1, T2, T3>>.Deserialize(BinaryDeserializer deserializer)
        {
            DelegateDeserializer delegateDeserializer = DelegateDeserializer.Deserialize(deserializer, parameterTypes, ref this.state);
            if (delegateDeserializer.State == RemoteExpressionSerializeStateEnum.Success)
            {
                var expression = default(CallDelegate);
                var expressions = default(Dictionary<HashBuffer, CallDelegate>);
                Dictionary<HashBuffer, CallDelegate>[] expressionArray = delegateDeserializer.LockExpressionArray();
                try
                {
                    expression = delegateDeserializer.GetExpression(expressionArray, (byte)DelegateTypeEnum.Action3, out expressions);
                    if (expression == null)
                    {
                        if (delegateDeserializer.SerializeInfo.ConstantParameterCount != 0)
                        {
                            ServerMethodParameter constantParameterType = delegateDeserializer.GetConstantParameterType();
                            expression = (constantParameterType.IsSimpleSerialize? typeof(CallSimpleConstantParameterAction<,,,>): typeof(CallConstantParameterAction<,,,>)).MakeGenericType(typeof(T1), typeof(T2), typeof(T3), constantParameterType.Type)
                                .GetMethod(nameof(CallAction<T1, T2, T3>.CreateExpression), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).notNull()
                                .Invoke(null, delegateDeserializer.Socket.GetRemoteExpressionParameters()).notNullCastType<CallDelegate>();
                        }
                        else expression = CallAction<T1, T2, T3>.CreateExpression(delegateDeserializer.GetFormatDeserialize());
                        expressions.Add(new HashBuffer(ref delegateDeserializer.SerializeInfo.Key), expression);
                    }
                }
                finally { Monitor.Exit(expressionArray); }
                deserializer.Current = delegateDeserializer.SerializeInfo.End;
                if (deserializer.State == BinarySerialize.DeserializeStateEnum.Success)
                {
                    action = (CallParameterAction<T1, T2, T3>)expression.Create(deserializer);
                    this.state = RemoteExpressionSerializeStateEnum.Success;
                    return;
                }
            }
        }

        /// <summary>
        /// A collection of generic parameter types
        /// 泛型参数类型集合
        /// </summary>
        private static readonly Type[] parameterTypes = new Type[] { typeof(T1), typeof(T2), typeof(T3) };
    }
}
