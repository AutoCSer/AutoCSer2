using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式类型编号
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct TypeIndex
    {
        /// <summary>
        /// 类型编号
        /// </summary>
        private readonly int index;
        /// <summary>
        /// 默认头部标记
        /// </summary>
        internal readonly int NodeHeader;
        /// <summary>
        /// 类型信息
        /// </summary>
#if NetStandard21
        private readonly Type? type;
#else
        private readonly Type type;
#endif
        /// <summary>
        /// 远程表达式类型编号
        /// </summary>
        /// <param name="index"></param>
        internal TypeIndex(int index)
        {
            this.index = index;
            type = null;
            NodeHeader = (int)NodeHeaderEnum.TypeIndex;
        }
        /// <summary>
        /// 远程表达式类型编号
        /// </summary>
        /// <param name="type"></param>
#if NetStandard21
        internal TypeIndex(Type? type)
#else
        internal TypeIndex(Type type)
#endif
        {
            index = 0;
            this.type = type;
            NodeHeader = type != null ? (int)NodeHeaderEnum.Type : 0;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        internal bool Serialize(ClientMetadata metadata)
        {
            if (NodeHeader != 0)
            {
                if (type == null) return metadata.Stream.Write(index);
                AutoCSer.Reflection.RemoteType remoteType = type;
                if (metadata.Stream.Write(AutoCSer.BinarySerializer.NullValue) && metadata.Serialize(remoteType.AssemblyName) && metadata.Serialize(remoteType.Name))
                {
                    metadata.AppendTypeIndex(type);
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
