using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Reflection;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式方法编号
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MethodIndex
    {
        /// <summary>
        /// 方法编号
        /// </summary>
        private readonly int index;
        /// <summary>
        /// 默认头部标记
        /// </summary>
        internal readonly int NodeHeader;
        /// <summary>
        /// 方法信息
        /// </summary>
#if NetStandard21
        internal readonly MethodInfo? Method;
#else
        internal readonly MethodInfo Method;
#endif
        /// <summary>
        /// 远程表达式方法编号
        /// </summary>
        /// <param name="index"></param>
        internal MethodIndex(int index)
        {
            this.index = index;
            Method = null;
            NodeHeader = (int)NodeHeaderEnum.MethodIndex;
        }
        /// <summary>
        /// 远程表达式方法编号
        /// </summary>
        /// <param name="method"></param>
#if NetStandard21
        internal MethodIndex(MethodInfo? method)
#else
        internal MethodIndex(MethodInfo method)
#endif
        {
            index = 0;
            this.Method = method;
            NodeHeader = method == null ? 0 : (int)NodeHeaderEnum.Method;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        internal bool Serialize(ClientMetadata metadata)
        {
            if (NodeHeader == 0) return true;
            if (Method == null) return metadata.Stream.Write(index);
            BindingFlags bindingFlags = Method.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
            UnmanagedStream stream = metadata.Stream;
            bindingFlags |= Method.IsPublic ? BindingFlags.Public : (BindingFlags.Public | BindingFlags.NonPublic);
            if(stream.Write(AutoCSer.BinarySerializer.NullValue) && metadata.Serialize(Method.ReflectedType.notNull()) 
                && metadata.Serialize(Method.Name) && stream.Write((int)bindingFlags))
            {
                Type[] types = Method.IsGenericMethod ? Method.GetGenericArguments() : EmptyArray<Type>.Array;
                if (types.Length < 256)
                {
                    ParameterInfo[] parameters = Method.GetParameters();
                    if (parameters.Length < 256)
                    {
                        if (stream.Write(types.Length | (parameters.Length << 8)))
                        {
                            foreach (Type type in types)
                            {
                                if(!metadata.Serialize(type)) return false;
                            }
                            foreach (ParameterInfo parameter in parameters)
                            {
                                if (!metadata.Serialize(parameter.ParameterType)) return false;
                            }
                            return true;
                        }
                    }
                    metadata.State = RemoteExpressionSerializeStateEnum.TooManyParameters;
                }
                else metadata.State = RemoteExpressionSerializeStateEnum.TooManyGenericArguments;
            }
            return false;
        }
    }
}
