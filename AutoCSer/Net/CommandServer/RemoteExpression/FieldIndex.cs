using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Reflection;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式字段编号
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct FieldIndex
    {
        /// <summary>
        /// 字段编号
        /// </summary>
        private readonly int index;
        /// <summary>
        /// 默认头部标记
        /// </summary>
        internal readonly int NodeHeader;
        /// <summary>
        /// 字段信息
        /// </summary>
#if NetStandard21
        private readonly FieldInfo? field;
#else
        private readonly FieldInfo field;
#endif
        /// <summary>
        /// 远程表达式字段编号
        /// </summary>
        /// <param name="index"></param>
        internal FieldIndex(int index)
        {
            this.index = index;
            field = null;
            NodeHeader = (int)NodeHeaderEnum.FieldIndex;
        }
        /// <summary>
        /// 远程表达式字段编号
        /// </summary>
        /// <param name="field"></param>
        internal FieldIndex(FieldInfo field)
        {
            index = 0;
            this.field = field;
            NodeHeader = (int)NodeHeaderEnum.Field;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        internal bool Serialize(ClientMetadata metadata)
        {
            if (field == null) return metadata.Stream.Write(index);
            BindingFlags bindingFlags = field.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
            UnmanagedStream stream = metadata.Stream;
            bindingFlags |= field.IsPublic ? BindingFlags.Public : (BindingFlags.Public | BindingFlags.NonPublic);
            return stream.Write(AutoCSer.BinarySerializer.NullValue) && metadata.Serialize(field.ReflectedType.notNull())
                && metadata.Serialize(field.Name) && stream.Write((int)bindingFlags);
        }
    }
}
