using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node method information
    /// 服务端节点方法信息
    /// </summary>
    internal abstract class NodeMethod : AutoCSer.Net.CommandServer.InterfaceMethodBase, IEquatable<NodeMethod>
    {
        /// <summary>
        /// 生成方法序号映射枚举类型代码相对路径
        /// </summary>
        internal const string MethodIndexEnumTypePath = @"NodeMethodEnum\";

        /// <summary>
        /// Method call type
        /// 方法调用类型
        /// </summary>
        internal CallTypeEnum CallType = CallTypeEnum.Unknown;
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        internal CallStateEnum CallState = CallStateEnum.Success;
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        internal unsafe NodeMethod(Type type, MethodInfo method) : base(type, method) { }
        /// <summary>
        /// 设置方法调用类型
        /// </summary>
        protected void setCallType()
        {
            if (ParameterStartIndex == ParameterEndIndex)
            {
                CallType = ReturnValueType == typeof(void) ? CallTypeEnum.Call : CallTypeEnum.CallOutput;
            }
            else
            {
                CallType = ReturnValueType == typeof(void) ? CallTypeEnum.CallInput : CallTypeEnum.CallInputOutput;
            }
        }
        /// <summary>
        /// 参数检查
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected bool checkParameter(ParameterInfo parameter)
        {
            if (!parameter.ParameterType.IsByRef) return true;
            SetError(CallStateEnum.NodeMethodParameterIsByRef, $"节点方法 {Type.fullName()}.{Method.Name} 不允许下 ref / out 参数 {parameter.Name}");
            return false;
        }
        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="error"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetError(CallStateEnum state, string error)
        {
            CallType = CallTypeEnum.Unknown;
            CallState = state;
            Error = error;
        }

        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
#if NetStandard21
        public bool Equals(NodeMethod? other)
#else
        public bool Equals(NodeMethod other)
#endif
        {
            return other != null && InputParameterType == other.InputParameterType && ReturnValueType == other.ReturnValueType && Method.Name == other.Method.Name;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castType<NodeMethod>());
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = ParameterCount ^ ReturnValueType.GetHashCode() ^ Method.Name.GetHashCode();
            if (InputParameterType != null) hashCode ^= InputParameterType.GetHashCode();
            return hashCode;
        }
    }
}
