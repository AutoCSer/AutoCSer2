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
        private ParameterInfo[] parameters;
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
        internal ServerMethodParameterKey(ParameterInfo[] parameters, Type? returnType)
#else
        internal ServerMethodParameterKey(ParameterInfo[] parameters, Type returnType)
#endif
        {
            this.parameters = parameters.sort(compare);
            this.returnType = returnType;
            hashCode = returnType == null ? 0 : returnType.GetHashCode();
            foreach (ParameterInfo parameter in parameters)
            {
                hashCode ^= parameter.elementType().GetHashCode() ^ parameter.Name.notNull().GetHashCode();
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
                if (parameters.Length == other.parameters.Length)
                {
                    int index = 0;
                    foreach (ParameterInfo otherParameter in other.parameters)
                    {
                        ParameterInfo parameter = parameters[index++];
                        if (parameter.ParameterType != otherParameter.ParameterType || parameter.Name != otherParameter.Name) return false;
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
        private static int compare(ParameterInfo left, ParameterInfo right)
        {
            return string.CompareOrdinal(left.Name, right.Name);
        }
    }
}
