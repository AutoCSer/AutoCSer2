using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 接口方法信息
    /// </summary>
    internal abstract class InterfaceMethod : InterfaceMethodBase, IEquatable<InterfaceMethod>
    {
        /// <summary>
        /// 命令服务配置
        /// </summary>
        protected readonly CommandServerControllerInterfaceAttribute controllerAttribute;
        /// <summary>
        /// 匹配方法名称
        /// </summary>
        internal virtual string MatchMethodName { get { return Method.Name; } }
        /// <summary>
        /// 有效输入参数数量
        /// </summary>
        internal int InputParameterCount;
        /// <summary>
        /// 有效输出参数数量
        /// </summary>
        internal int OutputParameterCount;
        /// <summary>
        /// 输入参数集合
        /// </summary>
        internal IEnumerable<ParameterInfo> OutputParameters
        {
            get
            {
                for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
                {
                    ParameterInfo parameter = Parameters[parameterIndex];
                    if (parameter.IsOut || parameter.ParameterType.IsByRef) yield return parameter;
                }
            }
        }
        /// <summary>
        /// 输出参数类型
        /// </summary>
#if NetStandard21
        internal ServerMethodParameter? OutputParameterType;
#else
        internal ServerMethodParameter OutputParameterType;
#endif
        /// <summary>
        /// 输出参数字段集合
        /// </summary>
        internal FieldInfo[] OutputParameterFields;
        /// <summary>
        /// 服务端接口方法信息
        /// </summary>
        internal InterfaceMethod()
        {
            controllerAttribute = CommandServerController.DefaultAttribute;
            OutputParameterFields = EmptyArray<FieldInfo>.Array;
        }
        /// <summary>
        /// 服务端接口方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="controllerAttribute"></param>
        internal InterfaceMethod(Type type, MethodInfo method, CommandServerControllerInterfaceAttribute controllerAttribute) : base(type, method)
        {
            this.controllerAttribute = controllerAttribute;
            OutputParameterFields = EmptyArray<FieldInfo>.Array;
        }
        /// <summary>
        /// 检查同步方法有效参数名称
        /// </summary>
        /// <returns></returns>
        protected bool checkSynchronousParameter()
        {
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                ParameterInfo parameter = Parameters[parameterIndex];
                if (parameter.ParameterType.IsByRef && ReturnValueType != typeof(void) && parameter.Name == nameof(ServerReturnValue<int>.ReturnValue))
                {
                    Error = $"{Type.fullName()}.{Method.Name} 参数名称 {parameter.Name} 和返回值名称冲突";
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 设置参数数量
        /// </summary>
        protected void setParameterCount()
        {
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                ParameterInfo parameter = Parameters[parameterIndex];
                if (parameter.ParameterType.IsByRef)
                {
                    if (!parameter.IsOut) ++InputParameterCount;
                    ++OutputParameterCount;
                }
                else ++InputParameterCount;
            }
        }

        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
#if NetStandard21
        public bool Equals(InterfaceMethod? other)
#else
        public bool Equals(InterfaceMethod other)
#endif
        {
            return other != null && InputParameterType == other.InputParameterType && OutputParameterType == other.OutputParameterType
                  && ReturnValueType == other.ReturnValueType && MatchMethodName == other.MatchMethodName
                  && EqualsParameterCount == other.EqualsParameterCount;
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
            return Equals(obj.castClass<InterfaceMethod>());
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = EqualsParameterCount ^ ReturnValueType.GetHashCode() ^ MatchMethodName.GetHashCode();
            if (InputParameterType != null) hashCode ^= InputParameterType.GetHashCode();
            if (OutputParameterType != null) hashCode ^= OutputParameterType.GetHashCode();
            return hashCode;
        }
    }
}
