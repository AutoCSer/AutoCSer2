using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式常量参数关键字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ConstantParameterKey : IEquatable<ConstantParameterKey>
    {
        /// <summary>
        /// 参数类型集合
        /// </summary>
        private readonly Type[] types;
        /// <summary>
        /// 参数集合
        /// </summary>
        internal LeftArray<ConstantParameter> Parameters;
        /// <summary>
        /// 远程表达式常量参数关键字
        /// </summary>
        /// <param name="parameters"></param>
        internal ConstantParameterKey(ref LeftArray<ConstantParameter> parameters)
        {
            types = EmptyArray<Type>.Array;
            this.Parameters = parameters;
            uint hashCode = 0;
            int count = parameters.Length;
            foreach (ConstantParameter parameter in parameters.Array)
            {
                hashCode ^= (uint)parameter.Type.GetHashCode();
                if (--count == 0) break;
                hashCode = AutoCSer.Memory.Common.HashCodeShift(hashCode);
            }
            parameters.Reserve = (int)hashCode;
        }
        /// <summary>
        /// 远程表达式常量参数关键字
        /// </summary>
        /// <param name="key"></param>
        internal ConstantParameterKey(ConstantParameterKey key)
        {
            int index = 0, count = key.Parameters.Length;
            Parameters = new LeftArray<ConstantParameter>(0);
            types = new Type[count];
            foreach (ConstantParameter parameter in key.Parameters.Array)
            {
                types[index] = parameter.Type;
                if (--count == 0) break;
                ++index;
            }
            Parameters.Reserve = key.Parameters.Reserve;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ConstantParameterKey other)
        {
            if (types.Length != 0)
            {
                int index = 0;
                if (other.types.Length != 0)
                {
                    if (types.Length == other.types.Length)
                    {
                        foreach (Type type in other.types)
                        {
                            if (types[index] != type) return false;
                            ++index;
                        }
                        return true;
                    }
                    return false;
                }
                int parameterCount = other.Parameters.Length;
                if (types.Length == parameterCount)
                {
                    foreach (ConstantParameter parameter in other.Parameters.Array)
                    {
                        if (types[index] != parameter.Type) return false;
                        if (--parameterCount == 0) return true;
                        ++index;
                    }
                }
                return false;
            }
            return other.types.Length != 0 && other.Equals(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<ConstantParameterKey>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Parameters.Reserve;
        }
    }
}
