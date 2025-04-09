using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class JsonSerialize : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @JsonSerializeMethodName(AutoCSer.JsonSerializer serializer, @CurrentType.FullName value)
            {
                #region IF Members.Length
                value.jsonSerialize(serializer);
                #endregion IF Members.Length
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @JsonSerializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap, JsonSerializer serializer, @CurrentType.FullName value, AutoCSer.Memory.CharStream stream)
            {
                #region IF Members.Length
                value.jsonSerialize(memberMap, serializer, stream);
                #endregion IF Members.Length
            }
            #region IF Members.Length
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                #region LOOP Members
                #region NOT IsFirstMember
                __stream__.Write(',');
                #endregion NOT IsFirstMember
                __stream__.SimpleWrite(@"@SerializeMemberName");
                #region IF MemberType.Type.IsValueType
                #region IF MemberType.Type.IsEnum
                __serializer__.@EnumJsonSerializeMethodName(@MemberName);
                #endregion IF MemberType.Type.IsEnum
                #region NOT MemberType.Type.IsEnum
                __serializer__.@SerializeMethodName(@MemberName);
                #endregion NOT MemberType.Type.IsEnum
                #endregion IF MemberType.Type.IsValueType
                #region NOT MemberType.Type.IsValueType
                if (@MemberName == null) __stream__.WriteJsonNull();
                else __serializer__.@SerializeMethodName(@MemberName);
                #endregion NOT MemberType.Type.IsValueType
                #endregion LOOP Members
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                #region LOOP Members
                if (__memberMap__.IsMember(@MemberIndex))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"@SerializeMemberName");
                    #region IF MemberType.Type.IsValueType
                    #region IF MemberType.Type.IsEnum
                    __serializer__.@EnumJsonSerializeMethodName(@MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __serializer__.@SerializeMethodName(@MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    #endregion IF MemberType.Type.IsValueType
                    #region NOT MemberType.Type.IsValueType
                    if (@MemberName == null) __stream__.WriteJsonNull();
                    else __serializer__.@SerializeMethodName(@MemberName);
                    #endregion NOT MemberType.Type.IsValueType
                }
                #endregion LOOP Members
            }
            #endregion IF Members.Length
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @JsonDeserializeMethodName(AutoCSer.JsonDeserializer deserializer, ref @CurrentType.FullName value, ref AutoCSer.Memory.Pointer names)
            {
                #region IF DeserializeMembers.Length
                value.jsonDeserialize(deserializer, ref names);
                #endregion IF DeserializeMembers.Length
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @JsonDeserializeMemberMapMethodName(AutoCSer.JsonDeserializer deserializer, ref @CurrentType.FullName value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap)
            {
                #region IF DeserializeMembers.Length
                value.jsonDeserialize(deserializer, ref names, memberMap);
                #endregion IF DeserializeMembers.Length
            }
            #region IF DeserializeMembers.Length
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                #region LOOP DeserializeMembers
                if (__deserializer__.IsName(ref __names__))
                {
                    #region NOT IsField
                    var @MemberName = this.@MemberName;
                    #region IF MemberType.Type.IsEnum
                    __deserializer__.@EnumJsonDeserializeMethodName(ref @MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __deserializer__.JsonDeserialize(ref @MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    this.@MemberName = @MemberName;
                    #endregion NOT IsField
                    #region IF IsField
                    #region IF MemberType.Type.IsEnum
                    __deserializer__.@EnumJsonDeserializeMethodName(ref this.@MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __deserializer__.JsonDeserialize(ref this.@MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    #endregion IF IsField
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                #endregion LOOP DeserializeMembers
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__)
            {
                #region LOOP DeserializeMembers
                if (__deserializer__.IsName(ref __names__))
                {
                    #region NOT IsField
                    var @MemberName = this.@MemberName;
                    #region IF MemberType.Type.IsEnum
                    __deserializer__.@EnumJsonDeserializeMethodName(ref @MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __deserializer__.JsonDeserialize(ref @MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    this.@MemberName = @MemberName;
                    #endregion NOT IsField
                    #region IF IsField
                    #region IF MemberType.Type.IsEnum
                    __deserializer__.@EnumJsonDeserializeMethodName(ref this.@MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __deserializer__.JsonDeserialize(ref this.@MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    #endregion IF IsField
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(@MemberIndex);
                    else return;
                }
                else return;
                #endregion LOOP DeserializeMembers
            }
            #region LOOP DeserializeMembers
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @MemberJsonDeserializeMethodName(AutoCSer.JsonDeserializer __deserializer__, ref @CurrentType.FullName __value__)
            {
                #region NOT IsField
                var @MemberName = __value__.@MemberName;
                #region IF MemberType.Type.IsEnum
                __deserializer__.@EnumJsonDeserializeMethodName(ref @MemberName);
                #endregion IF MemberType.Type.IsEnum
                #region NOT MemberType.Type.IsEnum
                __deserializer__.JsonDeserialize(ref @MemberName);
                #endregion NOT MemberType.Type.IsEnum
                __value__.@MemberName = @MemberName;
                #endregion NOT IsField
                #region IF IsField
                #region IF MemberType.Type.IsEnum
                __deserializer__.@EnumJsonDeserializeMethodName(ref __value__.@MemberName);
                #endregion IF MemberType.Type.IsEnum
                #region NOT MemberType.Type.IsEnum
                __deserializer__.JsonDeserialize(ref __value__.@MemberName);
                #endregion NOT MemberType.Type.IsEnum
                #endregion IF IsField
            }
            #endregion LOOP DeserializeMembers
            #endregion IF DeserializeMembers.Length
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
            public string MemberName;
            public void jsonSerialize(params object[] value) { }
            public void jsonDeserialize(object value, ref AutoCSer.Memory.Pointer names, object memberMap = null) { }
        }
    }
    #endregion NOTE
}
