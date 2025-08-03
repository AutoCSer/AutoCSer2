using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Reflection;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式属性编号
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PropertyIndex
    {
        /// <summary>
        /// 属性编号
        /// </summary>
        private readonly int index;
        /// <summary>
        /// 默认头部标记
        /// </summary>
        internal readonly int NodeHeader;
        /// <summary>
        /// 属性信息
        /// </summary>
#if NetStandard21
        private readonly PropertyInfo? property;
#else
        private readonly PropertyInfo property;
#endif
        /// <summary>
        /// 远程表达式属性编号
        /// </summary>
        /// <param name="index"></param>
        internal PropertyIndex(int index)
        {
            this.index = index;
            property = null;
            NodeHeader = (int)NodeHeaderEnum.PropertyIndex;
        }
        /// <summary>
        /// 远程表达式属性编号
        /// </summary>
        /// <param name="property"></param>
#if NetStandard21
        internal PropertyIndex(PropertyInfo? property)
#else
        internal PropertyIndex(PropertyInfo property)
#endif
        {
            index = 0;
            this.property = property;
            NodeHeader = property == null ? 0 : (int)NodeHeaderEnum.Property;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        internal bool Serialize(ClientMetadata metadata)
        {
            if (NodeHeader == 0) return true;
            if (property == null) return metadata.Stream.Write(index);
            UnmanagedStream stream = metadata.Stream;
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            return stream.Write(AutoCSer.BinarySerializer.NullValue) && metadata.Serialize(property.ReflectedType.notNull())
                && metadata.Serialize(property.Name) && stream.Write((int)bindingFlags);
        }
    }
}
