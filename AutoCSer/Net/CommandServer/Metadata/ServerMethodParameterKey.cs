using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 命令服务参数缓存关键字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ServerMethodParameterKey : IEquatable<ServerMethodParameterKey>
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        internal MethodParameter[] Parameters;
        /// <summary>
        /// 类型
        /// </summary>
#if NetStandard21
        private Type? returnType;
#else
        private Type returnType;
#endif
        /// <summary>
        /// 
        /// </summary>
        private int hashCode;
        /// <summary>
        /// TCP 参数类型
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <param name="returnType">类型</param>
#if NetStandard21
        internal ServerMethodParameterKey(MethodParameter[] parameters, Type? returnType)
#else
        internal ServerMethodParameterKey(MethodParameter[] parameters, Type returnType)
#endif
        {
            this.Parameters = parameters.sort(compare);
            this.returnType = returnType;
            hashCode = returnType == null ? 0 : returnType.GetHashCode();
            foreach (MethodParameter parameter in parameters)
            {
                hashCode ^= parameter.ElementType.GetHashCode() ^ parameter.Parameter.Name.notNull().GetHashCode();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ServerMethodParameterKey other)
        {
            if (returnType == other.returnType)
            {
                if (Parameters.Length == other.Parameters.Length)
                {
                    int index = 0;
                    foreach (MethodParameter otherParameter in other.Parameters)
                    {
                        MethodParameter parameter = Parameters[index++];
                        if (parameter.ParameterType != otherParameter.ParameterType || parameter.Parameter.Name != otherParameter.Parameter.Name) return false;
                    }
                    return true;
                }
            }
            return false;
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
            return Equals(obj.castValue<ServerMethodParameterKey>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashCode;
        }


        /// <summary>
        /// 参数排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(MethodParameter left, MethodParameter right)
        {
            return string.CompareOrdinal(left.Parameter.Name, right.Parameter.Name);
        }
    }
}
