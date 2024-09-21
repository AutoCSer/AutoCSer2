using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct KeepCallbackResponseParameter : AutoCSer.BinarySerialize.ICustomSerialize<KeepCallbackResponseParameter>
    {
        /// <summary>
        /// 调用状态
        /// </summary>
        internal CallStateEnum State;
        /// <summary>
        /// 是否简单序列化输出数据
        /// </summary>
        internal readonly bool IsSimpleSerialize;
        /// <summary>
        /// 返回参数序列化
        /// </summary>
        internal readonly ResponseParameterSerializer Serializer;
        /// <summary>
        /// 反序列化操作对象
        /// </summary>
        internal object DeserializeValue;
        /// <summary>
        /// 返回参数序列化
        /// </summary>
        /// <param name="state">调用状态</param>
        internal KeepCallbackResponseParameter(CallStateEnum state)
        {
            this.State = state;
            IsSimpleSerialize = false;
            Serializer = null;
            DeserializeValue = null;
        }
        /// <summary>
        /// 返回参数序列化
        /// </summary>
        /// <param name="serializer">返回参数序列化</param>
        /// <param name="isSimpleSerialize">是否简单序列化输出数据</param>
        internal KeepCallbackResponseParameter(ResponseParameterSerializer serializer, bool isSimpleSerialize)
        {
            State = CallStateEnum.Success;
            IsSimpleSerialize = isSimpleSerialize;
            this.Serializer = serializer;
            DeserializeValue = null;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<KeepCallbackResponseParameter>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.Stream.Write((int)(byte)State);
            if (State == CallStateEnum.Success) this.Serializer.Serialize(serializer);
        }
        /// <summary>
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
        /// 创建返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="isSimpleSerialize">是否简单序列化输出数据</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static KeepCallbackResponseParameter Create<T>(T value, bool isSimpleSerialize)
        {
            if (isSimpleSerialize) return new KeepCallbackResponseParameter(new ResponseParameterSimpleSerializer<T>(value), isSimpleSerialize);
            return new KeepCallbackResponseParameter(new ResponseParameterBinarySerializer<T>(value), isSimpleSerialize);
        }
        /// <summary>
        /// 创建返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="isSimpleSerialize">是否简单序列化输出数据</param>
        /// <returns></returns>
        internal static IEnumerable<KeepCallbackResponseParameter> CreateValues<T>(IEnumerable<T> values, bool isSimpleSerialize)
        {
            if (isSimpleSerialize)
            {
                foreach (T value in values)
                {
                    yield return new KeepCallbackResponseParameter(new ResponseParameterSimpleSerializer<T>(value), true);
                }
            }
            else
            {
                foreach (T value in values)
                {
                    yield return new KeepCallbackResponseParameter(new ResponseParameterBinarySerializer<T>(value), false);
                }
            }
        }

        /// <summary>
        /// 空回调
        /// </summary>
        internal static readonly CommandServerKeepCallback<KeepCallbackResponseParameter> EmptyKeepCallback = new CommandServerKeepCallback<KeepCallbackResponseParameter>();
    }
}
