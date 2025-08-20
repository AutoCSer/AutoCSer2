using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer.RemoteExpression;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Persistent remote Lambda expressions
    /// 可持久化的远程 Lambda 表达式
    /// </summary>
    /// <typeparam name="T">Delegate type
    /// 委托类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RemoteLambdaExpression<T> : AutoCSer.BinarySerialize.ICustomSerialize<RemoteLambdaExpression<T>>
        where T : class
    {
        /// <summary>
        /// Client-side Lambda expression
        /// 客户端 Lambda 表达式
        /// </summary>
#if NetStandard21
        public System.Linq.Expressions.Expression<T>? Expression;
#else
        public System.Linq.Expressions.Expression<T> Expression;
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
        /// Remote Lambda expression
        /// 远程 Lambda 表达式
        /// </summary>
        /// <param name="expression">Client-side Lambda expression
        /// 客户端 Lambda 表达式</param>
        public RemoteLambdaExpression(System.Linq.Expressions.Expression<T> expression)
        {
            this.Expression = expression;
            state = RemoteExpressionSerializeStateEnum.Unknown;
        }
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
        /// Compile delegate
        /// 编译委托
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? Compile()
#else
        public T Compile()
#endif
        {
            return Expression != null ? Expression.Compile() : null;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteLambdaExpression<T>>.Serialize(BinarySerializer serializer)
        {
            if (Expression != null) new LambdaExpressionSerializer(serializer, Expression.Parameters).Serialize(Expression.Body);
            else serializer.Stream.Write((int)(byte)RemoteExpressionSerializeStateEnum.NullExpression);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RemoteLambdaExpression<T>>.Deserialize(BinaryDeserializer deserializer)
        {
            state = RemoteExpressionSerializeStateEnum.DeserializeFailed;
            int stateValue;
            if (deserializer.Read(out stateValue))
            {
                if (stateValue == (byte)RemoteExpressionSerializeStateEnum.Success)
                {
                    var metadata = default(ServerMetadata);
                    var socket = deserializer.Context.castType<CommandServerSocket>();
                    if (socket != null)
                    {
                        metadata = socket.Server.RemoteMetadata;
                        if (metadata == null)
                        {
                            state = RemoteExpressionSerializeStateEnum.NotFoundRemoteMetadata;
                            return;
                        }
                    }
                    else metadata = ServerMetadata.Default;
                    LambdaExpressionDeserializer expressionDeserializer = new LambdaExpressionDeserializer(deserializer, metadata);
                    var expression = expressionDeserializer.Deserialize();
                    if (expression != null)
                    {
                        this.Expression = System.Linq.Expressions.Expression.Lambda<T>(expression.notNull(), expressionDeserializer.Parameters);
                        state = RemoteExpressionSerializeStateEnum.Success;
                    }
                    else state = expressionDeserializer.State;
                }
                else state = (RemoteExpressionSerializeStateEnum)(byte)stateValue;
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="expression"></param>
        public static implicit operator RemoteLambdaExpression<T>(Expression<T> expression) { return new RemoteLambdaExpression<T>(expression); }
        /// <summary>
        /// 客户端传参类型重定向
        /// </summary>
        /// <param name="expression"></param>
        public static RemoteLambdaExpression<T> AutoCSerCommandParameterCastType(Expression<T> expression) { return (RemoteLambdaExpression<T>)expression; }
    }
}
