using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class BinarySerialize : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            #region IF IsSerialize
            /// <summary>
            /// Binary serialization
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void @BinarySerializeMethodName(AutoCSer.BinarySerializer serializer, @TypeFullName value)
            {
                #region IF IsSimpleSerialize
                serializer.Simple(/*NOTE*/(int)/*NOTE*/value);
                #endregion IF IsSimpleSerialize
                #region NOT IsSimpleSerialize
                if (serializer.WriteMemberCountVerify(@FixedSize, @MemberCountVerify)) value.binarySerialize(serializer);
                #endregion NOT IsSimpleSerialize
            }
            #region NOT IsSimpleSerialize
            /// <summary>
            /// Binary serialization
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                #region LOOP FixedFields
                #region IF MemberType.Type.IsEnum
                __serializer__.Stream.Write((@UnderlyingType.FullName)/*NOTE*/(object)/*NOTE*/this.@MemberName);
                #endregion IF MemberType.Type.IsEnum
                #region NOT MemberType.Type.IsEnum
                __serializer__.BinarySerialize(@MemberName);
                #endregion NOT MemberType.Type.IsEnum
                #endregion LOOP FixedFields
                #region IF FixedFillSize
                __serializer__.FixedFillSize(@FixedFillSize);
                #endregion IF FixedFillSize
                #region LOOP FieldArray
                __serializer__.@SerializeMethodName/*IF:IsGenericType*/<@GenericTypeName>/*IF:IsGenericType*/(@MemberName);
                #endregion LOOP FieldArray
            }
            #region IF IsMemberMap
            /// <summary>
            /// Binary serialization of member bitmaps is supported
            /// 支持成员位图的二进制序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void @BinarySerializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@TypeFullName> memberMap, AutoCSer.BinarySerializer serializer, @TypeFullName value)
            {
                #region IF IsMemberMapFixedFillSize
                int startIndex = serializer.Stream.GetPrepSizeCurrentIndex(@FixedSize);
                if (startIndex >= 0) value.binarySerialize(memberMap, serializer, startIndex);
                #endregion IF IsMemberMapFixedFillSize
                #region NOT IsMemberMapFixedFillSize
                value.binarySerialize(memberMap, serializer);
                #endregion NOT IsMemberMapFixedFillSize
            }
            /// <summary>
            /// Binary serialization of member bitmaps is supported
            /// 支持成员位图的二进制序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__startIndex__"></param>
            private void binarySerialize(AutoCSer.Metadata.MemberMap<@TypeFullName> __memberMap__, AutoCSer.BinarySerializer __serializer__/*IF:IsMemberMapFixedFillSize*/, int __startIndex__/*IF:IsMemberMapFixedFillSize*/)
            {
                #region LOOP FixedFields
                if (__memberMap__.IsMember(@MemberIndex))
                {
                    #region IF MemberType.Type.IsEnum
                    __serializer__.Stream.Write((@UnderlyingType.FullName)/*NOTE*/(object)/*NOTE*/this.@MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __serializer__.BinarySerialize(@MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                }
                #endregion LOOP FixedFields
                #region IF IsMemberMapFixedFillSize
                __serializer__.SerializeFill(__startIndex__);
                #endregion IF IsMemberMapFixedFillSize
                #region LOOP FieldArray
                if (__memberMap__.IsMember(@MemberIndex)) __serializer__.@SerializeMethodName/*IF:IsGenericType*/<@GenericTypeName>/*IF:IsGenericType*/(@MemberName);
                #endregion LOOP FieldArray
            }
            #endregion IF IsMemberMap
            #endregion NOT IsSimpleSerialize
            #endregion IF IsSerialize
            #region IF IsDeserialize
            /// <summary>
            /// Binary deserialization
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void @BinaryDeserializeMethodName(AutoCSer.BinaryDeserializer deserializer, ref @TypeFullName value)
            {
                #region IF IsSimpleSerialize
                deserializer.@SimpleMethodName(ref value);
                #endregion IF IsSimpleSerialize
                #region NOT IsSimpleSerialize
                value.binaryDeserialize(deserializer);
                #endregion NOT IsSimpleSerialize
            }
            #region NOT IsSimpleSerialize
            /// <summary>
            /// Binary deserialization
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                #region LOOP FixedFields
                #region IF MemberType.Type.IsEnum
                this.@MemberName = (@MemberType.FullName)__deserializer__.@DeserializeMethodName();
                #endregion IF MemberType.Type.IsEnum
                #region NOT MemberType.Type.IsEnum
                #region IF IsProperty
                var @MemberName = this.@MemberName;
                __deserializer__.@DeserializeMethodName(ref @MemberName);
                this.@MemberName = @MemberName;
                #endregion IF IsProperty
                #region NOT IsProperty
                __deserializer__.@DeserializeMethodName(ref this.@MemberName);
                #endregion NOT IsProperty
                #endregion NOT MemberType.Type.IsEnum
                #endregion LOOP FixedFields
                #region IF FixedFillSize
                __deserializer__.FixedFillSize(@FixedFillSize);
                #endregion IF FixedFillSize
                #region IF FieldArray.Length
                binaryFieldDeserialize(__deserializer__);
                #endregion IF FieldArray.Length
            }
            #region IF FieldArray.Length
            /// <summary>
            /// Binary deserialization
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                #region LOOP FieldArray
                #region IF IsProperty
                var @MemberName = this.@MemberName;
                __deserializer__.@DeserializeMethodName/*IF:IsGenericType*/<@GenericTypeName>/*IF:IsGenericType*/(ref @MemberName);
                this.@MemberName = @MemberName;
                #endregion IF IsProperty
                #region NOT IsProperty
                __deserializer__.@DeserializeMethodName/*IF:IsGenericType*/<@GenericTypeName>/*IF:IsGenericType*/(ref this.@MemberName);
                #endregion NOT IsProperty
                #endregion LOOP FieldArray
            }
            #endregion IF FieldArray.Length
            #region IF IsMemberMap
            /// <summary>
            /// Binary deserialization of member bitmaps is supported
            /// 支持成员位图的二进制反序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void @BinaryDeserializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@TypeFullName> memberMap, AutoCSer.BinaryDeserializer deserializer, ref @TypeFullName value)
            {
                value.binaryDeserialize(memberMap, deserializer);
            }
            /// <summary>
            /// Binary deserialization of member bitmaps is supported
            /// 支持成员位图的二进制反序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.Metadata.MemberMap<@TypeFullName> __memberMap__, AutoCSer.BinaryDeserializer __deserializer__)
            {
                #region IF IsMemberMapFixedFillSize
                __deserializer__.SetFixedCurrent();
                #endregion IF IsMemberMapFixedFillSize
                #region LOOP FixedFields
                if (__memberMap__.IsMember(@MemberIndex))
                {
                    #region IF MemberType.Type.IsEnum
                    this.@MemberName = (@MemberType.FullName)__deserializer__.@DeserializeMethodName();
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    #region IF IsProperty
                    var @MemberName = this.@MemberName;
                    __deserializer__.@DeserializeMethodName(ref @MemberName);
                    this.@MemberName = @MemberName;
                    #endregion IF IsProperty
                    #region NOT IsProperty
                    __deserializer__.@DeserializeMethodName(ref this.@MemberName);
                    #endregion NOT IsProperty
                    #endregion NOT MemberType.Type.IsEnum
                }
                #endregion LOOP FixedFields
                #region IF IsMemberMapFixedFillSize
                __deserializer__.SetFixedCurrentEnd();
                #endregion IF IsMemberMapFixedFillSize
                #region LOOP FieldArray
                if (__memberMap__.IsMember(@MemberIndex))
                {
                    #region IF IsProperty
                    var @MemberName = this.@MemberName;
                    __deserializer__.@DeserializeMethodName/*IF:IsGenericType*/<@GenericTypeName>/*IF:IsGenericType*/(ref @MemberName);
                    this.@MemberName = @MemberName;
                    #endregion IF IsProperty
                    #region NOT IsProperty
                    __deserializer__.@DeserializeMethodName/*IF:IsGenericType*/<@GenericTypeName>/*IF:IsGenericType*/(ref this.@MemberName);
                    #endregion NOT IsProperty
                }
                #endregion LOOP FieldArray
            }
            #endregion IF IsMemberMap
            #endregion NOT IsSimpleSerialize
            #endregion IF IsDeserialize
            /// <summary>
            /// Get the binary serialization type information
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo @BinarySerializeMemberTypeMethodName()
            {
                #region IF IsSimpleSerialize
                return new AutoCSer.BinarySerialize.TypeInfo(true, @MemberTypeCount, @MemberCountVerify);
                #endregion IF IsSimpleSerialize
                #region NOT IsSimpleSerialize
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, @MemberTypeCount, @MemberCountVerify);
                #region IF IsSerialize
                #region LOOP MemberTypes
                typeInfo.Add(typeof(@MemberType.FullName));
                #endregion LOOP MemberTypes
                #endregion IF IsSerialize
                return typeInfo;
                #endregion NOT IsSimpleSerialize
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void @BinarySerializeMethodName()
            {
                @TypeFullName value = default(@TypeFullName);
                #region IF IsSerialize
                @BinarySerializeMethodName(null, value);
                #region NOT IsSimpleSerialize
                #region IF IsMemberMap
                @BinarySerializeMemberMapMethodName(null, null, value);
                #endregion IF IsMemberMap
                #endregion NOT IsSimpleSerialize
                #endregion IF IsSerialize
                #region IF IsDeserialize
                @BinaryDeserializeMethodName(null, ref value);
                #region NOT IsSimpleSerialize
                #region IF IsMemberMap
                @BinaryDeserializeMemberMapMethodName(null, null, ref value);
                #endregion IF IsMemberMap
                #endregion NOT IsSimpleSerialize
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(@TypeFullName));
                #endregion IF IsDeserialize
                @BinarySerializeMemberTypeMethodName();
                AutoCSer.AotReflection.NonPublicMethods(typeof(@TypeFullName));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<@TypeFullName>();
            }
            #endregion PART CLASS
            private string MemberName;
            private AutoCSer.Metadata.MemberMapIndex<TypeFullName> MemberIndex = new AutoCSer.Metadata.MemberMapIndex<TypeFullName>(null);
            private static int FixedSize = 0;
            private int FixedFillSize = 0;
            private static int MemberCountVerify = 0;
            private const int MemberTypeCount = 0;
        }
    }
}
