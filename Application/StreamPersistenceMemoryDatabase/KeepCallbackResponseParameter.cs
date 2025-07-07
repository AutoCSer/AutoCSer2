using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The return parameters of the keep callback
    /// 保持回调的返回参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct KeepCallbackResponseParameter : AutoCSer.BinarySerialize.ICustomSerialize<KeepCallbackResponseParameter>
    {
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        internal CallStateEnum State;
        /// <summary>
        /// Server-side node method flags
        /// 服务端节点方法标记
        /// </summary>
        internal readonly MethodFlagsEnum flag;
        /// <summary>
        /// Return parameter serialization
        /// 返回参数序列化
        /// </summary>
        internal readonly ResponseParameterSerializer Serializer;
        /// <summary>
        /// Deserialization operation object
        /// 反序列化操作对象
        /// </summary>
#if NetStandard21
        internal object? DeserializeValue;
#else
        internal object DeserializeValue;
#endif
        /// <summary>
        /// The return parameters of the keep callback
        /// 保持回调的返回参数
        /// </summary>
        /// <param name="state">Call status
        /// 调用状态</param>
        internal KeepCallbackResponseParameter(CallStateEnum state)
        {
            this.State = state;
            flag = 0;
#if NetStandard21
            Serializer = NullResponseParameterSerializer.Null;
#else
            Serializer = null;
#endif
            DeserializeValue = null;
        }
        ///// <summary>
        ///// 返回参数序列化
        ///// </summary>
        ///// <param name="serializer">返回参数序列化</param>
        //internal KeepCallbackResponseParameter(ResponseParameterSerializer serializer)
        //{
        //    State = CallStateEnum.Success;
        //    flag = 0;
        //    this.Serializer = serializer;
        //    DeserializeValue = null;
        //}
        /// <summary>
        /// The return parameters of the keep callback
        /// 保持回调的返回参数
        /// </summary>
        /// <param name="serializer">Return parameter serialization
        /// 返回参数序列化</param>
        /// <param name="flag">Server-side node method flags
        /// 服务端节点方法标记</param>
        internal KeepCallbackResponseParameter(ResponseParameterSerializer serializer, MethodFlagsEnum flag)
        {
            State = CallStateEnum.Success;
            this.flag = flag;
            this.Serializer = serializer;
            DeserializeValue = null;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<KeepCallbackResponseParameter>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.Stream.Write((int)(byte)State);
            if (State == CallStateEnum.Success) this.Serializer.Serialize(serializer);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<KeepCallbackResponseParameter>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            int state;
            if (deserializer.Read(out state))
            {
                this.State = (CallStateEnum)(byte)state;
                if (this.State == CallStateEnum.Success) DeserializeValue = this.Serializer.Deserialize(deserializer);
            }
        }
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
        internal static KeepCallbackResponseParameter Create<T>(T value, MethodFlagsEnum flags)
        {
            if ((flags & MethodFlagsEnum.IsSimpleSerializeParamter) != 0) return new KeepCallbackResponseParameter(new ResponseParameterSimpleSerializer<T>(value), flags);
            return new KeepCallbackResponseParameter(new ResponseParameterBinarySerializer<T>(value), flags);
        }
        /// <summary>
        /// Create the return parameters
        /// 创建返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <returns></returns>
        internal static IEnumerable<KeepCallbackResponseParameter> CreateValues<T>(IEnumerable<T> values, MethodFlagsEnum flags)
        {
            if ((flags & MethodFlagsEnum.IsSimpleSerializeParamter) != 0)
            {
                foreach (T value in values)
                {
                    yield return new KeepCallbackResponseParameter(new ResponseParameterSimpleSerializer<T>(value), flags);
                }
            }
            else
            {
                foreach (T value in values)
                {
                    yield return new KeepCallbackResponseParameter(new ResponseParameterBinarySerializer<T>(value), flags);
                }
            }
        }

        /// <summary>
        /// Empty callback
        /// </summary>
        internal static readonly CommandServerKeepCallback<KeepCallbackResponseParameter> EmptyKeepCallback = new CommandServerKeepCallback<KeepCallbackResponseParameter>();
    }
}
