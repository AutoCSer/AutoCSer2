using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class BinarySerialize : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @FixedBinarySerializeMethodName(AutoCSer.BinarySerializer serializer, @CurrentType.FullName value)
            {
                #region IF FixedFields.Length
                value.fixedBinarySerialize(serializer);
                #endregion IF FixedFields.Length
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @FixedBinaryDeserializeMethodName(AutoCSer.BinaryDeserializer deserializer, ref @CurrentType.FullName value)
            {
                #region IF FixedFields.Length
                value.fixedBinaryDeserialize(deserializer);
                #endregion IF FixedFields.Length
            }
            #region IF FixedFields.Length
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void fixedBinarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                #region LOOP FixedFields
                #region IF MemberType.Type.IsEnum
                __serializer__.Stream.Write((@UnderlyingType.FullName)/*NOTE*/(object)/*NOTE*/this.@MemberName);
                #endregion IF MemberType.Type.IsEnum
                #region NOT MemberType.Type.IsEnum
                __serializer__.BinarySerialize(@MemberName);
                #endregion NOT MemberType.Type.IsEnum
                #endregion LOOP FixedFields
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void fixedBinaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
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
            }
            #endregion IF FixedFields.Length
            #region IF IsMemberMap
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @FixedBinarySerializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap, AutoCSer.BinarySerializer serializer, @CurrentType.FullName value)
            {
                #region IF FixedFields.Length
                value.fixedBinarySerialize(memberMap, serializer);
                #endregion IF FixedFields.Length
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @FixedBinaryDeserializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap, AutoCSer.BinaryDeserializer deserializer, ref @CurrentType.FullName value)
            {
                #region IF FixedFields.Length
                value.fixedBinaryDeserialize(memberMap, deserializer);
                #endregion IF FixedFields.Length
            }
            #region IF FixedFields.Length
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            private void fixedBinarySerialize(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__, AutoCSer.BinarySerializer __serializer__)
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
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__deserializer__"></param>
            private void fixedBinaryDeserialize(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__, AutoCSer.BinaryDeserializer __deserializer__)
            {
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
            }
            #endregion IF FixedFields.Length
            #endregion IF IsMemberMap
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @BinarySerializeMethodName(AutoCSer.BinarySerializer serializer, @CurrentType.FullName value)
            {
                #region IF FieldArray.Length
                value.binarySerialize(serializer);
                #endregion IF FieldArray.Length
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @BinaryDeserializeMethodName(AutoCSer.BinaryDeserializer deserializer, ref @CurrentType.FullName value)
            {
                #region IF FieldArray.Length
                value.binaryDeserialize(deserializer);
                #endregion IF FieldArray.Length
            }
            #region IF FieldArray.Length
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                #region LOOP FieldArray
                __serializer__.@SerializeMethodName/*IF:IsGenericType*/<@GenericTypeName>/*IF:IsGenericType*/(@MemberName);
                #endregion LOOP FieldArray
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
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
            /// 二进制序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @BinarySerializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap, AutoCSer.BinarySerializer serializer, @CurrentType.FullName value)
            {
                #region IF FieldArray.Length
                value.binarySerialize(memberMap, serializer);
                #endregion IF FieldArray.Length
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @BinaryDeserializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap, AutoCSer.BinaryDeserializer deserializer, ref @CurrentType.FullName value)
            {
                #region IF FieldArray.Length
                value.binaryDeserialize(memberMap, deserializer);
                #endregion IF FieldArray.Length
            }
            #region IF FieldArray.Length
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__, AutoCSer.BinarySerializer __serializer__)
            {
                #region LOOP FieldArray
                if (__memberMap__.IsMember(@MemberIndex)) __serializer__.@SerializeMethodName/*IF:IsGenericType*/<@GenericTypeName>/*IF:IsGenericType*/(@MemberName);
                #endregion LOOP FieldArray
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__, AutoCSer.BinaryDeserializer __deserializer__)
            {
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
            #endregion IF FieldArray.Length
            #endregion IF IsMemberMap
            #region NOTE
            private string MemberName;
            private AutoCSer.Metadata.MemberMapIndex<CurrentType.FullName> MemberIndex = new AutoCSer.Metadata.MemberMapIndex<FullName>(null);
            #endregion NOTE
            #endregion PART CLASS
        }
    }
    #region NOTE
    /// <summary>
    /// CSharp 模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        public unsafe partial class FullName
        {
            public void fixedBinarySerialize(params object[] values) { }
            public void binarySerialize(params object[] values) { }
            public void fixedBinaryDeserialize(params object[] values) { }
            public void binaryDeserialize(params object[] values) { }
        }
        public partial class GenericTypeName { }
    }
    #endregion NOTE
}
