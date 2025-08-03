using AutoCSer.Net.CommandServer.RemoteExpression;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程元数据输出数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    internal struct RemoteMetadataOutputData
    {
        /// <summary>
        /// 远程类型编号集合
        /// </summary>
        public RemoteMetadataTypeIndex[] TypeIndexs;
        /// <summary>
        /// 远程方法编号集合
        /// </summary>
        public RemoteMetadataMethodIndex[] MethodIndexs;
        /// <summary>
        /// 远程属性编号集合
        /// </summary>
        public RemoteMetadataMemberIndex[] PropertyIndexs;
        /// <summary>
        /// 远程字段编号集合
        /// </summary>
        public RemoteMetadataMemberIndex[] FieldIndexs;
        /// <summary>
        /// 远程元数据输出数据
        /// </summary>
        /// <param name="metadata"></param>
        internal RemoteMetadataOutputData(ServerMetadata metadata)
        {
            int count = metadata.Types.Length;
            if (count != 0)
            {
                TypeIndexs = new RemoteMetadataTypeIndex[count];
                int index = 0;
                foreach (Type type in metadata.Types.Array)
                {
                    TypeIndexs[index].Set(type, index + 1);
                    if (--count == 0) break;
                    ++index;
                }
            }
            else TypeIndexs = EmptyArray<RemoteMetadataTypeIndex>.Array;

            count = metadata.Properties.Length;
            if (count != 0)
            {
                PropertyIndexs = new RemoteMetadataMemberIndex[count];
                int index = 0;
                foreach (KeyValue<PropertyInfo, RemoteMetadataMemberIndex> property in metadata.Properties.Array)
                {
                    PropertyIndexs[index] = property.Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else PropertyIndexs = EmptyArray<RemoteMetadataMemberIndex>.Array;

            count = metadata.Fields.Length;
            if (count != 0)
            {
                FieldIndexs = new RemoteMetadataMemberIndex[count];
                int index = 0;
                foreach (KeyValue<FieldInfo, RemoteMetadataMemberIndex> field in metadata.Fields.Array)
                {
                    FieldIndexs[index] = field.Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else FieldIndexs = EmptyArray<RemoteMetadataMemberIndex>.Array;

            count = metadata.Methods.Length;
            if (count != 0)
            {
                MethodIndexs = new RemoteMetadataMethodIndex[count];
                int index = 0;
                foreach (KeyValue<MethodInfo, RemoteMetadataMethodIndex> method in metadata.Methods.Array)
                {
                    MethodIndexs[index] = method.Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else MethodIndexs = EmptyArray<RemoteMetadataMethodIndex>.Array;
        }
        /// <summary>
        /// 远程元数据输出数据
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="formatDeserialize"></param>
        internal RemoteMetadataOutputData(ServerMetadata metadata, FormatDeserialize formatDeserialize)
        {
            int count = formatDeserialize.NewTypes.Length;
            if (count != 0)
            {
                Type[] types = metadata.Types.Array;
                TypeIndexs = new RemoteMetadataTypeIndex[count];
                int index = 0;
                foreach (int typeIndex in formatDeserialize.NewTypes.Array)
                {
                    TypeIndexs[index].Set(types[typeIndex - 1], typeIndex);
                    if (--count == 0) break;
                    ++index;
                }
            }
            else TypeIndexs = EmptyArray<RemoteMetadataTypeIndex>.Array;

            count = formatDeserialize.NewProperties.Length;
            if (count != 0)
            {
                KeyValue<PropertyInfo, RemoteMetadataMemberIndex>[] properties = metadata.Properties.Array;
                PropertyIndexs = new RemoteMetadataMemberIndex[count];
                int index = 0;
                foreach (int propertyIndex in formatDeserialize.NewProperties.Array)
                {
                    PropertyIndexs[index] = properties[propertyIndex].Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else PropertyIndexs = EmptyArray<RemoteMetadataMemberIndex>.Array;

            count = formatDeserialize.NewFields.Length;
            if (count != 0)
            {
                KeyValue<FieldInfo, RemoteMetadataMemberIndex>[] fields = metadata.Fields.Array;
                FieldIndexs = new RemoteMetadataMemberIndex[count];
                int index = 0;
                foreach (int fieldIndex in formatDeserialize.NewFields.Array)
                {
                    FieldIndexs[index] = fields[fieldIndex].Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else FieldIndexs = EmptyArray<RemoteMetadataMemberIndex>.Array;

            count = formatDeserialize.NewMethods.Length;
            if (count != 0)
            {
                KeyValue<MethodInfo, RemoteMetadataMethodIndex>[] methods = metadata.Methods.Array;
                MethodIndexs = new RemoteMetadataMethodIndex[count];
                int index = 0;
                foreach (int methodIndex in formatDeserialize.NewMethods.Array)
                {
                    MethodIndexs[index] = methods[methodIndex].Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else MethodIndexs = EmptyArray<RemoteMetadataMethodIndex>.Array;
        }
    }
}
