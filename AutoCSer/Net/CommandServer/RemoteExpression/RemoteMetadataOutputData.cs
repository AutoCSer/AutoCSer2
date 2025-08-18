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
        /// 远程构造函数编号集合
        /// </summary>
        public RemoteMetadataConstructorIndex[] ConstructorIndexs;
        /// <summary>
        /// 远程元数据输出数据
        /// </summary>
        /// <param name="metadata"></param>
        internal RemoteMetadataOutputData(ServerMetadata metadata)
        {
            int count = metadata.TypeArray.Length;
            if (count != 0)
            {
                TypeIndexs = new RemoteMetadataTypeIndex[count];
                int index = 0;
                foreach (Type type in metadata.TypeArray.Array)
                {
                    TypeIndexs[index].Set(type, index + 1);
                    if (--count == 0) break;
                    ++index;
                }
            }
            else TypeIndexs = EmptyArray<RemoteMetadataTypeIndex>.Array;

            count = metadata.PropertyArray.Length;
            if (count != 0)
            {
                PropertyIndexs = new RemoteMetadataMemberIndex[count];
                int index = 0;
                foreach (KeyValue<PropertyInfo, RemoteMetadataMemberIndex> property in metadata.PropertyArray.Array)
                {
                    PropertyIndexs[index] = property.Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else PropertyIndexs = EmptyArray<RemoteMetadataMemberIndex>.Array;

            count = metadata.FieldArray.Length;
            if (count != 0)
            {
                FieldIndexs = new RemoteMetadataMemberIndex[count];
                int index = 0;
                foreach (KeyValue<FieldInfo, RemoteMetadataMemberIndex> field in metadata.FieldArray.Array)
                {
                    FieldIndexs[index] = field.Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else FieldIndexs = EmptyArray<RemoteMetadataMemberIndex>.Array;

            count = metadata.MethodArray.Length;
            if (count != 0)
            {
                MethodIndexs = new RemoteMetadataMethodIndex[count];
                int index = 0;
                foreach (KeyValue<MethodInfo, RemoteMetadataMethodIndex> method in metadata.MethodArray.Array)
                {
                    MethodIndexs[index] = method.Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else MethodIndexs = EmptyArray<RemoteMetadataMethodIndex>.Array;

            count = metadata.ConstructorArray.Length;
            if (count != 0)
            {
                ConstructorIndexs = new RemoteMetadataConstructorIndex[count];
                int index = 0;
                foreach (KeyValue<ConstructorInfo, RemoteMetadataConstructorIndex> constructor in metadata.ConstructorArray.Array)
                {
                    ConstructorIndexs[index] = constructor.Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else ConstructorIndexs = EmptyArray<RemoteMetadataConstructorIndex>.Array;
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
                Type[] types = metadata.TypeArray.Array;
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
                KeyValue<PropertyInfo, RemoteMetadataMemberIndex>[] properties = metadata.PropertyArray.Array;
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
                KeyValue<FieldInfo, RemoteMetadataMemberIndex>[] fields = metadata.FieldArray.Array;
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
                KeyValue<MethodInfo, RemoteMetadataMethodIndex>[] methods = metadata.MethodArray.Array;
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

            count = formatDeserialize.NewConstructors.Length;
            if (count != 0)
            {
                KeyValue<ConstructorInfo, RemoteMetadataConstructorIndex>[] constructors = metadata.ConstructorArray.Array;
                ConstructorIndexs = new RemoteMetadataConstructorIndex[count];
                int index = 0;
                foreach (int constructorIndex in formatDeserialize.NewConstructors.Array)
                {
                    ConstructorIndexs[index] = constructors[constructorIndex].Value;
                    if (--count == 0) break;
                    ++index;
                }
            }
            else ConstructorIndexs = EmptyArray<RemoteMetadataConstructorIndex>.Array;
        }
    }
}
