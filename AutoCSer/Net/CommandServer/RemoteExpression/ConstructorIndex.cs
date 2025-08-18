using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Reflection;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式构造函数编号
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ConstructorIndex
    {
        /// <summary>
        /// 构造函数编号
        /// </summary>
        private readonly int index;
        /// <summary>
        /// 默认头部标记
        /// </summary>
        internal readonly int NodeHeader;
        /// <summary>
        /// 构造函数信息
        /// </summary>
#if NetStandard21
        internal readonly ConstructorInfo? Constructor;
#else
        internal readonly ConstructorInfo Constructor;
#endif
        /// <summary>
        /// 远程表达式构造函数编号
        /// </summary>
        /// <param name="index"></param>
        internal ConstructorIndex(int index)
        {
            this.index = index;
            Constructor = null;
            NodeHeader = (int)NodeHeaderEnum.ConstructorIndex;
        }
        /// <summary>
        /// 远程表达式构造函数编号
        /// </summary>
        /// <param name="constructor"></param>
#if NetStandard21
        internal ConstructorIndex(ConstructorInfo? constructor)
#else
        internal ConstructorIndex(ConstructorInfo constructor)
#endif
        {
            index = 0;
            this.Constructor = constructor;
            NodeHeader = constructor == null ? 0 : (int)NodeHeaderEnum.Constructor;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal bool Serialize(ClientMetadata metadata, Type type)
        {
            if (NodeHeader == 0) return true;
            if (Constructor == null) return metadata.Stream.Write(index);
            UnmanagedStream stream = metadata.Stream;
            if (stream.Write(AutoCSer.BinarySerializer.NullValue) && metadata.Serialize(type))
            {
                ParameterInfo[] parameters = Constructor.GetParameters();
                if (parameters.Length < 256 && stream.Write(parameters.Length))
                {
                    foreach (ParameterInfo parameter in parameters)
                    {
                        if (!metadata.Serialize(parameter.ParameterType)) return false;
                    }
                    return true;
                }
                metadata.State = RemoteExpressionSerializeStateEnum.TooManyParameters;
            }
            return false;
        }
    }
}
