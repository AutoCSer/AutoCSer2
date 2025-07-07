using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Return parameter
    /// 返回参数
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public class ResponseParameter : AutoCSer.BinarySerialize.ICustomSerialize<ResponseParameter>
    {
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        internal CallStateEnum State;
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// Return parameter
        /// 返回参数
        /// </summary>
        internal ResponseParameter()
        {
            State = CallStateEnum.Success;
        }
        /// <summary>
        /// Return parameter
        /// 返回参数
        /// </summary>
        /// <param name="state">Call status
        /// 调用状态</param>
        internal ResponseParameter(CallStateEnum state)
        {
            State = state;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        protected virtual void serialize(AutoCSer.BinarySerializer serializer) { }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected virtual void deserialize(AutoCSer.BinaryDeserializer deserializer) { }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ResponseParameter>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.Stream.Write((int)(byte)State);
            if (State == CallStateEnum.Success) serialize(serializer);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ResponseParameter>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            int state;
            if (deserializer.Read(out state))
            {
                this.State = (CallStateEnum)(byte)state;
                if (this.State == CallStateEnum.Success) deserialize(deserializer);
            }
        }
        /// <summary>
        /// Create a keep callback return parameter
        /// 创建保持回调返回参数
        /// </summary>
        /// <returns></returns>
        internal virtual KeepCallbackResponseParameter CreateKeepCallback() { throw new InvalidCastException(); }
        /// <summary>
        /// Create the return parameters
        /// 创建返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ResponseParameter Create<T>(T value, MethodFlagsEnum flags)
        {
            if ((flags & MethodFlagsEnum.IsSimpleSerializeParamter) != 0) return new SimpleSerializeResponseParameter<T>(value);
            return new BinarySerializeResponseParameter<T>(value); 
        }

        /// <summary>
        /// A collection of call state return parameter
        /// 调用状态返回参数集合
        /// </summary>
        internal static readonly ResponseParameter[] CallStates;
        static ResponseParameter()
        {
            CallStates = new ResponseParameter[(byte)CallStateEnum.Callbacked];
            for (byte state = 0; state != CallStates.Length; ++state) CallStates[state] = new ResponseParameter((CallStateEnum)state);
        }
    }
    /// <summary>
    /// Return parameter
    /// 返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ResponseParameter<T> : ResponseParameter
    {
        /// <summary>
        /// Return data
        /// </summary>
        internal ServerReturnValue<T> Value;
        /// <summary>
        /// Return parameter
        /// 返回参数
        /// </summary>
        internal ResponseParameter() : base(CallStateEnum.Unknown) { }
        /// <summary>
        /// Return parameter
        /// 返回参数
        /// </summary>
        /// <param name="value"></param>
        internal ResponseParameter(T value)
        {
            Value.ReturnValue = value;
        }
    }
}
