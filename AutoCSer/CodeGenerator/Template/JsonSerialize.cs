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
            internal static void @JsonSerializeMethodName(AutoCSer.JsonSerializer serializer, @CurrentType.FullName value)
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
            internal static void @JsonSerializeMemberMapMethodName(AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap, JsonSerializer serializer, @CurrentType.FullName value, AutoCSer.Memory.CharStream stream)
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
            internal static void @JsonDeserializeMethodName(AutoCSer.JsonDeserializer deserializer, ref @CurrentType.FullName value, ref AutoCSer.Memory.Pointer names)
            {
                #region IF DeserializeMemberCount
                value.jsonDeserialize(deserializer, ref names);
                #endregion IF DeserializeMemberCount
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void @JsonDeserializeMemberMapMethodName(AutoCSer.JsonDeserializer deserializer, ref @CurrentType.FullName value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap)
            {
                #region IF DeserializeMemberCount
                value.jsonDeserialize(deserializer, ref names, memberMap);
                #endregion IF DeserializeMemberCount
            }
            #region IF DeserializeMemberCount
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
                    __deserializer__.@DeserializeMethodName(ref @MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    this.@MemberName = @MemberName;
                    #endregion NOT IsField
                    #region IF IsField
                    #region IF MemberType.Type.IsEnum
                    __deserializer__.@EnumJsonDeserializeMethodName(ref this.@MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __deserializer__.@DeserializeMethodName(ref this.@MemberName);
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
                    __deserializer__.@DeserializeMethodName(ref @MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    this.@MemberName = @MemberName;
                    #endregion NOT IsField
                    #region IF IsField
                    #region IF MemberType.Type.IsEnum
                    __deserializer__.@EnumJsonDeserializeMethodName(ref this.@MemberName);
                    #endregion IF MemberType.Type.IsEnum
                    #region NOT MemberType.Type.IsEnum
                    __deserializer__.@DeserializeMethodName(ref this.@MemberName);
                    #endregion NOT MemberType.Type.IsEnum
                    #endregion IF IsField
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(@MemberIndex);
                    else return;
                }
                else return;
                #endregion LOOP DeserializeMembers
            }
            #endregion IF DeserializeMemberCount
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void @JsonDeserializeMethodName(AutoCSer.JsonDeserializer __deserializer__, ref @CurrentType.FullName __value__, int __memberIndex__)
            {
                #region IF DeserializeMemberCount
                switch (__memberIndex__)
                {
                    #region LOOP DeserializeMembers
                    case @IntMemberIndex:
                        #region NOT IsField
                        var @MemberName = __value__.@MemberName;
                        #region IF MemberType.Type.IsEnum
                        __deserializer__.@EnumJsonDeserializeMethodName(ref @MemberName);
                        #endregion IF MemberType.Type.IsEnum
                        #region NOT MemberType.Type.IsEnum
                        __deserializer__.@DeserializeMethodName(ref @MemberName);
                        #endregion NOT MemberType.Type.IsEnum
                        __value__.@MemberName = @MemberName;
                        #endregion NOT IsField
                        #region IF IsField
                        #region IF MemberType.Type.IsEnum
                        __deserializer__.@EnumJsonDeserializeMethodName(ref __value__.@MemberName);
                        #endregion IF MemberType.Type.IsEnum
                        #region NOT MemberType.Type.IsEnum
                        __deserializer__.@DeserializeMethodName(ref __value__.@MemberName);
                        #endregion NOT MemberType.Type.IsEnum
                        #endregion IF IsField
                        return;
                        #endregion LOOP DeserializeMembers
                }
                #endregion IF DeserializeMemberCount
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> @JsonDeserializeMemberNameMethodName()
            {
                #region IF DeserializeMemberCount
                return jsonDeserializeMemberName();
                #endregion IF DeserializeMemberCount
                #region NOT DeserializeMemberCount
                return default(AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>);
                #endregion NOT DeserializeMemberCount
            }
            #region IF DeserializeMemberCount
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(@DeserializeMemberCount);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(@DeserializeMemberCount);
                #region LOOP DeserializeMembers
                names.Add(nameof(@MemberName));
                indexs.Add(@IntMemberIndex);
                #endregion LOOP DeserializeMembers
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            #endregion IF DeserializeMemberCount
            #region IF MemberTypeCount
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> @JsonSerializeMemberTypeMethodName()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(@MemberTypeCount);
                #region LOOP MemberTypes
                types.Add(typeof(@MemberType.FullName));
                #endregion LOOP MemberTypes
                return types;
            }
            #endregion IF MemberTypeCount
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            #region IF CurrentType.Type.IsGenericType
            public static void @JsonSerializeMethodName(/*NOTE*/object value/*NOTE*/)/*NOTE*/ { }/*NOTE*/
            #endregion IF CurrentType.Type.IsGenericType
            #region NOT CurrentType.Type.IsGenericType
            internal static void @JsonSerializeMethodName()
            #endregion NOT CurrentType.Type.IsGenericType
            {
                @CurrentType.FullName value = default(@CurrentType.FullName);
                @JsonSerializeMethodName(null, value);
                @JsonSerializeMemberMapMethodName(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                @JsonDeserializeMethodName(null, ref value, ref names);
                @JsonDeserializeMemberMapMethodName(null, ref value, ref names, null);
                @JsonDeserializeMethodName(null, ref value, 0);
                @JsonDeserializeMemberNameMethodName();
                AutoCSer.JsonSerializer.JsonSerialize<@CurrentType.FullName>();
                AutoCSer.JsonDeserializer.JsonDeserialize<@CurrentType.FullName>();
                #region IF MemberTypeCount
                @JsonSerializeMemberTypeMethodName();
                #endregion IF MemberTypeCount
            }
            #endregion PART CLASS
            private string MemberName;
            private const int IntMemberIndex = 0;
            private AutoCSer.Metadata.MemberMapIndex<CurrentType.FullName> MemberIndex = new AutoCSer.Metadata.MemberMapIndex<FullName>(null);
            private const int DeserializeMemberCount = 0;
            private const int MemberTypeCount = 0;
        }
    }
}
