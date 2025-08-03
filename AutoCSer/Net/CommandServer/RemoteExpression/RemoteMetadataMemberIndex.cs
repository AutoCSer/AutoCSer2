using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer.RemoteExpression;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程元数据成员编号信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    internal struct RemoteMetadataMemberIndex
    {
        /// <summary>
        /// 成员编号
        /// </summary>
        public int Index;
        /// <summary>
        /// 类型编号
        /// </summary>
        public int TypeIndex;
        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName;
        /// <summary>
        /// 成员选择标记
        /// </summary>
        public BindingFlags BindingFlags;
        /// <summary>
        /// 远程元数据成员编号信息
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="index"></param>
        /// <param name="typeIndex"></param>
        /// <param name="bindingFlags"></param>
        internal RemoteMetadataMemberIndex(string memberName, int index, int typeIndex, BindingFlags bindingFlags)
        {
            Index = index;
            TypeIndex = typeIndex;
            MemberName = memberName;
            BindingFlags = bindingFlags;
        }
        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="types"></param>
        /// <returns></returns>
#if NetStandard21
        internal PropertyInfo? GetProperty(ClientMetadata metadata, ref LeftArray<KeyValue<Type, int>> types)
#else
        internal PropertyInfo GetProperty(ClientMetadata metadata, ref LeftArray<KeyValue<Type, int>> types)
#endif
        {
            return metadata.GetType(TypeIndex, ref types)?.GetProperty(MemberName, BindingFlags);
        }
        /// <summary>
        /// 获取字段信息
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="types"></param>
        /// <returns></returns>
#if NetStandard21
        internal FieldInfo? GetField(ClientMetadata metadata, ref LeftArray<KeyValue<Type, int>> types)
#else
        internal FieldInfo GetField(ClientMetadata metadata, ref LeftArray<KeyValue<Type, int>> types)
#endif
        {
            return metadata.GetType(TypeIndex, ref types)?.GetField(MemberName, BindingFlags);
        }
    }
}
