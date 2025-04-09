//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class BinarySerialize
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""serializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(FixedBinarySerializeMethodName);
            _code_.Add(@"(AutoCSer.BinarySerializer serializer, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FixedFields;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.fixedBinarySerialize(serializer);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""deserializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(FixedBinaryDeserializeMethodName);
            _code_.Add(@"(AutoCSer.BinaryDeserializer deserializer, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FixedFields;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.fixedBinaryDeserialize(deserializer);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FixedFields;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""__serializer__""></param>
            private void fixedBinarySerialize(AutoCSer.BinarySerializer __serializer__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_;
                    _value1_ = FixedFields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                __serializer__.Stream.Write((");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.UnderlyingType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                __serializer__.BinarySerialize(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            private void fixedBinaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_;
                    _value1_ = FixedFields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember _value2_ in _value1_)
                        {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = (");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")__deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"();");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.IsProperty)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                var ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");
                this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            }
            _if_ = false;
                if (!(bool)_value2_.IsProperty)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (IsMemberMap)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""memberMap""></param>
            /// <param name=""serializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(FixedBinarySerializeMemberMapMethodName);
            _code_.Add(@"(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap, AutoCSer.BinarySerializer serializer, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FixedFields;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.fixedBinarySerialize(memberMap, serializer);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""memberMap""></param>
            /// <param name=""deserializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(FixedBinaryDeserializeMemberMapMethodName);
            _code_.Add(@"(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap, AutoCSer.BinaryDeserializer deserializer, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FixedFields;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.fixedBinaryDeserialize(memberMap, deserializer);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FixedFields;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""__memberMap__""></param>
            /// <param name=""__serializer__""></param>
            private void fixedBinarySerialize(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> __memberMap__, AutoCSer.BinarySerializer __serializer__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_;
                    _value1_ = FixedFields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__memberMap__.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __serializer__.Stream.Write((");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.UnderlyingType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __serializer__.BinarySerialize(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""__memberMap__""></param>
            /// <param name=""__deserializer__""></param>
            private void fixedBinaryDeserialize(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> __memberMap__, AutoCSer.BinaryDeserializer __deserializer__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_;
                    _value1_ = FixedFields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__memberMap__.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = (");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")__deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"();");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.IsProperty)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    var ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                    __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");
                    this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            }
            _if_ = false;
                if (!(bool)_value2_.IsProperty)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
                }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }");
            }
            }
            _code_.Add(@"
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""serializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(BinarySerializeMethodName);
            _code_.Add(@"(AutoCSer.BinarySerializer serializer, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FieldArray;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.binarySerialize(serializer);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""deserializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(BinaryDeserializeMethodName);
            _code_.Add(@"(AutoCSer.BinaryDeserializer deserializer, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FieldArray;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.binaryDeserialize(deserializer);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FieldArray;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""__serializer__""></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_;
                    _value1_ = FieldArray;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                __serializer__.");
            _code_.Add(_value2_.SerializeMethodName);
            _if_ = false;
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_;
                    _value1_ = FieldArray;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsProperty)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                var ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _if_ = false;
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");
                this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            }
            _if_ = false;
                if (!(bool)_value2_.IsProperty)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _if_ = false;
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(ref this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }");
            }
            _if_ = false;
                    if (IsMemberMap)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""memberMap""></param>
            /// <param name=""serializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(BinarySerializeMemberMapMethodName);
            _code_.Add(@"(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap, AutoCSer.BinarySerializer serializer, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FieldArray;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.binarySerialize(memberMap, serializer);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""memberMap""></param>
            /// <param name=""deserializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(BinaryDeserializeMemberMapMethodName);
            _code_.Add(@"(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap, AutoCSer.BinaryDeserializer deserializer, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FieldArray;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.binaryDeserialize(memberMap, deserializer);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_ = FieldArray;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""__memberMap__""></param>
            /// <param name=""__serializer__""></param>
            private void binarySerialize(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> __memberMap__, AutoCSer.BinarySerializer __serializer__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_;
                    _value1_ = FieldArray;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__memberMap__.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@")) __serializer__.");
            _code_.Add(_value2_.SerializeMethodName);
            _if_ = false;
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""__memberMap__""></param>
            /// <param name=""__deserializer__""></param>
            private void binaryDeserialize(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> __memberMap__, AutoCSer.BinaryDeserializer __deserializer__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember[] _value1_;
                    _value1_ = FieldArray;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.BinarySerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__memberMap__.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                {");
            _if_ = false;
                    if (_value2_.IsProperty)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    var ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";
                    __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _if_ = false;
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");
                    this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            }
            _if_ = false;
                if (!(bool)_value2_.IsProperty)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _if_ = false;
                    if (_value2_.IsGenericType)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(ref this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }");
            }
            }
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class JsonSerialize
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name=""serializer""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(JsonSerializeMethodName);
            _code_.Add(@"(AutoCSer.JsonSerializer serializer, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_ = Members;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.jsonSerialize(serializer);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name=""memberMap""></param>
            /// <param name=""serializer""></param>
            /// <param name=""value""></param>
            /// <param name=""stream""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(JsonSerializeMemberMapMethodName);
            _code_.Add(@"(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap, JsonSerializer serializer, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value, AutoCSer.Memory.CharStream stream)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_ = Members;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.jsonSerialize(memberMap, serializer, stream);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_ = Members;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name=""__serializer__""></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_;
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember _value2_ in _value1_)
                        {
            _if_ = false;
                if (!(bool)IsFirstMember)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __stream__.Write(',');");
            }
            _code_.Add(@"
                __stream__.SimpleWrite(@""");
            _code_.Add(_value2_.SerializeMemberName);
            _code_.Add(@""");");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsValueType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                __serializer__.");
            _code_.Add(_value2_.EnumJsonSerializeMethodName);
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                __serializer__.");
            _code_.Add(_value2_.SerializeMethodName);
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsValueType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                if (");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" == null) __stream__.WriteJsonNull();
                else __serializer__.");
            _code_.Add(_value2_.SerializeMethodName);
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name=""__memberMap__""></param>
            /// <param name=""__serializer__""></param>
            /// <param name=""__stream__""></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_;
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__memberMap__.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@"))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@""");
            _code_.Add(_value2_.SerializeMemberName);
            _code_.Add(@""");");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsValueType)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __serializer__.");
            _code_.Add(_value2_.EnumJsonSerializeMethodName);
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __serializer__.");
            _code_.Add(_value2_.SerializeMethodName);
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsValueType)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    if (");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" == null) __stream__.WriteJsonNull();
                    else __serializer__.");
            _code_.Add(_value2_.SerializeMethodName);
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }");
            }
            _code_.Add(@"
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name=""deserializer""></param>
            /// <param name=""value""></param>
            /// <param name=""names""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(JsonDeserializeMethodName);
            _code_.Add(@"(AutoCSer.JsonDeserializer deserializer, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value, ref AutoCSer.Memory.Pointer names)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_ = DeserializeMembers;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.jsonDeserialize(deserializer, ref names);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name=""deserializer""></param>
            /// <param name=""value""></param>
            /// <param name=""names""></param>
            /// <param name=""memberMap""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(JsonDeserializeMemberMapMethodName);
            _code_.Add(@"(AutoCSer.JsonDeserializer deserializer, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap)
            {");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_ = DeserializeMembers;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                value.jsonDeserialize(deserializer, ref names, memberMap);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_ = DeserializeMembers;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[]))
                    {
                    if (_value1_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            /// <param name=""__names__""></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_;
                    _value1_ = DeserializeMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__deserializer__.IsName(ref __names__))
                {");
            _if_ = false;
                if (!(bool)_value2_.IsField)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    var ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.");
            _code_.Add(_value2_.EnumJsonDeserializeMethodName);
            _code_.Add(@"(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.JsonDeserialize(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                    this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            }
            _if_ = false;
                    if (_value2_.IsField)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.");
            _code_.Add(_value2_.EnumJsonDeserializeMethodName);
            _code_.Add(@"(ref this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.JsonDeserialize(ref this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            /// <param name=""__names__""></param>
            /// <param name=""__memberMap__""></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> __memberMap__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_;
                    _value1_ = DeserializeMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__deserializer__.IsName(ref __names__))
                {");
            _if_ = false;
                if (!(bool)_value2_.IsField)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    var ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.");
            _code_.Add(_value2_.EnumJsonDeserializeMethodName);
            _code_.Add(@"(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.JsonDeserialize(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                    this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            }
            _if_ = false;
                    if (_value2_.IsField)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.");
            _code_.Add(_value2_.EnumJsonDeserializeMethodName);
            _code_.Add(@"(ref this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                    __deserializer__.JsonDeserialize(ref this.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@");
                    else return;
                }
                else return;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember[] _value1_;
                    _value1_ = DeserializeMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.JsonSerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            /// <param name=""__value__""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(_value2_.MemberJsonDeserializeMethodName);
            _code_.Add(@"(AutoCSer.JsonDeserializer __deserializer__, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = CurrentType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" __value__)
            {");
            _if_ = false;
                if (!(bool)_value2_.IsField)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                var ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.");
            _code_.Add(_value2_.EnumJsonDeserializeMethodName);
            _code_.Add(@"(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.JsonDeserialize(ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
            }
            _if_ = false;
                    if (_value2_.IsField)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                    if (_value4_.IsEnum)
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.");
            _code_.Add(_value2_.EnumJsonDeserializeMethodName);
            _code_.Add(@"(ref __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value4_ = _value3_.Type;
                    if (_value4_ != default(System.Type))
                    {
                if (!(bool)_value4_.IsEnum)
                {
                    _if_ = true;
                }
                    }
                }
                    }
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.JsonDeserialize(ref __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
            }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            }
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class SimpleSerialize
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name=""stream""></param>
            /// <param name=""value""></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void ");
            _code_.Add(SimpleSerializeMethodName);
            _code_.Add(@"(AutoCSer.Memory.UnmanagedStream stream, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name=""__stream__""></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(");
            _code_.Add(PrepSize.ToString());
            _code_.Add(@"))
                {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.SimpleSerialize.SerializeField[] _value1_;
                    _value1_ = FixedFields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.SimpleSerialize.SerializeField _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsEnum)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    __stream__.Write((");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.UnderlyingType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")this.");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@");");
            }
            _if_ = false;
                if (!(bool)_value2_.IsEnum)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _if_ = false;
                    if (FixedFillSize != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    __stream__.TryMoveSize(");
            _code_.Add(FixedFillSize.ToString());
            _code_.Add(@");");
            }
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.SimpleSerialize.SerializeField[] _value1_;
                    _value1_ = FieldArray;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.SimpleSerialize.SerializeField _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name=""start""></param>
            /// <param name=""value""></param>
            /// <param name=""end""></param>
            /// <returns></returns>
            public static byte* ");
            _code_.Add(SimpleDeserializeMethodName);
            _code_.Add(@"(byte* start, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name=""__start__""></param>
            /// <param name=""__end__""></param>
            /// <returns></returns>
            private byte* simpleDeserialize(byte* __start__, byte* __end__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.SimpleSerialize.SerializeField[] _value1_;
                    _value1_ = FixedFields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.SimpleSerialize.SerializeField _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsEnum)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.UnderlyingType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@" = 0;
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref ");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@");
                this.");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@" = (");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@")");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@";");
            }
            _if_ = false;
                if (!(bool)_value2_.IsEnum)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                if (__start__ == null || __start__ > __end__) return null;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _if_ = false;
                    if (FixedFillSize != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __start__ += ");
            _code_.Add(FixedFillSize.ToString());
            _code_.Add(@";");
            }
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.SimpleSerialize.SerializeField[] _value1_;
                    _value1_ = FieldArray;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.SimpleSerialize.SerializeField _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@", __end__);");
            }
            _if_ = false;
                if (!(bool)_value2_.IsString)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.");
            _code_.Add(_value2_.FieldName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                if (__start__ == null || __start__ > __end__) return null;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                return __start__;
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class CommandServerMethodIndexEnumType
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.CommandServerMethodIndexEnumType.MethodInfo[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.CommandServerMethodIndexEnumType.MethodInfo _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.EnumName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// [");
            _code_.Add(_value2_.MethodIndex.ToString());
            _code_.Add(@"] ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorXmlDocument);
                    }
                }
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.XmlFullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" ");
            _code_.Add(_value4_.CodeGeneratorXmlDocument);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// 返回值 ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.XmlFullName);
                    }
                }
            _code_.Add(@" ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorReturnXmlDocument);
                    }
                }
            }
            _code_.Add(@"
            /// </summary>");
            }
            _code_.Add(@"
            ");
            _code_.Add(_value2_.EnumName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MethodIndex.ToString());
            _code_.Add(@",");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
                if (_isOut_) outEnd();
            }
        }
    }
}
#if !DotNet45
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class NetCoreWebView
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
            /// <summary>
            /// 获取请求路径，默认为 类型命令空间+类型名称，用于代码生成
            /// </summary>
            protected override string defaultRequestPath { get { return """);
            _code_.Add(RequestPath);
            _code_.Add(@"""; } }");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value1_ = default(AutoCSer.CodeGenerator.Metadata.MethodIndex);
                    _value1_ = Method;
            _if_ = false;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value1_.ParameterCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            ");
            _if_ = false;
                    if (IsQueryName)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 客户端查询传参
            /// </summary>
            public ");
            }
            _code_.Add(@"struct __QUERYPARAMETER__
            {
#pragma warning disable CS0649");
                {
                    System.Collections.Generic.IEnumerable<AutoCSer.CodeGenerator.Metadata.MethodParameter> _value2_;
                    _value2_ = LoadParameters;
                    if (_value2_ != null)
                    {
                        int _loopIndex2_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value3_ in _value2_)
                        {
            _code_.Add(@"
                public ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value4_ = _value3_.ParameterType;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value3_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex2_;
                    }
                }
            _code_.Add(@"
#pragma warning restore CS0649
            }");
            _if_ = false;
                    if (IsQueryName)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
#pragma warning disable CS0649
            /// <summary>
            /// 客户端查询传参
            /// </summary>
            public __QUERYPARAMETER__ ");
            _code_.Add(QueryParameterName);
            _code_.Add(@";
#pragma warning restore CS0649");
            }
            }
            }
                }
            _code_.Add(@"
            /// <summary>
            /// 初始化加载数据（基本操作用代码生成组件处理）
            /// </summary>
            /// <param name=""httpContext"">HTTP 上下文</param>
            /// <param name=""viewRequest"">数据视图信息</param>
            /// <returns></returns>
            protected override async System.Threading.Tasks.Task<AutoCSer.NetCoreWeb.ResponseResult> load(Microsoft.AspNetCore.Http.HttpContext httpContext, AutoCSer.NetCoreWeb.ViewRequest viewRequest)
            {");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value1_ = default(AutoCSer.CodeGenerator.Metadata.MethodIndex);
                    _value1_ = Method;
            _if_ = false;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value1_.ParameterCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                AutoCSer.NetCoreWeb.ResponseResult<__QUERYPARAMETER__> parameter = await getParameter<__QUERYPARAMETER__>(httpContext, viewRequest);
                if (!parameter.IsSuccess) return parameter;
                ");
            _if_ = false;
                if (!(bool)IsQueryName)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __QUERYPARAMETER__ ");
            }
            _code_.Add(QueryParameterName);
            _code_.Add(@" = parameter.Result;");
            _if_ = false;
                    if (CheckParameters != default(AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebView.CheckParameter[]))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                AutoCSer.NetCoreWeb.ParameterChecker checker = default(AutoCSer.NetCoreWeb.ParameterChecker);");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebView.CheckParameter[] _value2_;
                    _value2_ = CheckParameters;
                    if (_value2_ != null)
                    {
                        int _loopIndex2_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebView.CheckParameter _value3_ in _value2_)
                        {
            _if_ = false;
                    if (_value3_.IsCheckNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckNull(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
            _if_ = false;
                    if (_value3_.IsCheckEquatable)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckEquatable(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
            _if_ = false;
                    if (_value3_.IsCheckCollection)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckCollection(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
            _if_ = false;
                    if (_value3_.IsCheckString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckString(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
            _if_ = false;
                    if (_value3_.IsCheckConstraint)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckConstraint(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex2_;
                    }
                }
            }
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value2_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value2_ = AccessTokenParameter;
            _if_ = false;
                    if (_value2_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (viewRequest.IsAccessTokenParameter)
                {
                    AutoCSer.NetCoreWeb.ResponseResult checkResult = await viewRequest.ViewMiddleware.CheckAccessTokenParameter(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.ParameterName);
            _code_.Add(@");
                    if (!checkResult.IsSuccess) return checkResult;
                }");
            }
                }
            }
            _code_.Add(@"
                AutoCSer.NetCoreWeb.ResponseResult loadResult = await LoadView(");
            _if_ = false;
                    if (IsHttpContextParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"httpContext");
            }
            _if_ = false;
                    if (IsHttpContextParameterJoin)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", ");
            }
            _if_ = false;
                    if (IsViewMiddlewareParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"viewRequest.ViewMiddleware");
            }
            _if_ = false;
                    if (IsViewMiddlewareParameterJoin)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", ");
            }
                {
                    System.Collections.Generic.IEnumerable<AutoCSer.CodeGenerator.Metadata.MethodParameter> _value2_;
                    _value2_ = LoadParameters;
                    if (_value2_ != null)
                    {
                        int _loopIndex2_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value3_ in _value2_)
                        {
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value3_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex2_;
                    }
                }
            _code_.Add(@");
                if (!loadResult.IsSuccess) return loadResult;");
            }
                }
            _code_.Add(@"
                AutoCSer.NetCoreWeb.ViewResponse response = getResponse();
                try
                {
                    AutoCSer.Memory.CharStream stream = responseStart(httpContext, viewRequest, ref response);
                    ");
            _code_.Add(ViewCode);
            _code_.Add(@"
                    await responseEnd(httpContext, viewRequest, response);
                    return AutoCSer.NetCoreWeb.ResponseStateEnum.Success;
                }
                finally { response.Free(); }
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
#endif
#if !DotNet45
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class NetCoreWebViewMiddleware
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
            /// <summary>
            /// 用于代码生成
            /// </summary>
            private ");
            _code_.Add(TypeName);
            _code_.Add(@"() { }
            /// <summary>
            /// 数据视图中间件
            /// </summary>
            /// <param name=""nextRequest""></param>
            public ");
            _code_.Add(TypeName);
            _code_.Add(@"(Microsoft.AspNetCore.Http.RequestDelegate nextRequest) : base(nextRequest)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebViewMiddleware.ViewLoadMethod[] _value1_;
                    _value1_ = Views;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebViewMiddleware.ViewLoadMethod _value2_ in _value1_)
                        {
            _code_.Add(@"
                appendView(new AutoCSer.NetCoreWeb.ViewRequest(this, typeof(");
            _code_.Add(_value2_.NetCoreWebViewTypeFullName);
            _code_.Add(@"), () => new ");
            _code_.Add(_value2_.NetCoreWebViewTypeFullName);
            _code_.Add(@"()");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value3_ = _value2_.Parameter;
            _if_ = false;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value4_ = _value3_.ParameterType;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@"), """);
            _code_.Add(_value3_.ParameterName);
            _code_.Add(@"""");
            }
                }
            _code_.Add(@"));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                AutoCSer.Extensions.TaskExtension.Catch(load());
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
#endif
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class CommandServerClientController
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
        /// <summary>
        /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.CodeGeneratorXmlDocument);
                    }
                }
            _code_.Add(@" 客户端接口
        /// </summary>
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
        {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ControllerMethod[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.CommandServerClientController.ControllerMethod _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorXmlDocument);
                    }
                }
            _code_.Add(@"
            /// </summary>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// <param name=""");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""">");
            _code_.Add(_value4_.CodeGeneratorXmlDocument);
            _code_.Add(@"</param>");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <returns>");
            _code_.Add(_value2_.CodeGeneratorReturnXmlDocument);
            _code_.Add(@"</returns>");
            }
            _code_.Add(@"
            ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MethodName);
            _code_.Add(@"(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = default(AutoCSer.CodeGenerator.Metadata.ExtensionType);
                    _value3_ = _value2_.TaskQueueKeyType;
            _if_ = false;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.FullName);
            _code_.Add(@" queueKey");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value5_ = _value4_.Parameters;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]))
                    {
                    if (_value5_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", ");
            }
            }
                }
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(_value4_.RefOutString);
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
        }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class StreamPersistenceMemoryDatabaseNode
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
        /// <summary>
        /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.CodeGeneratorXmlDocument);
                    }
                }
            _code_.Add(@"
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(");
            _code_.Add(MethodIndexEnumTypeName);
            _code_.Add(@"))]
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@" { }
        /// <summary>
        /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.CodeGeneratorXmlDocument);
                    }
                }
            _code_.Add(@" 节点方法序号映射枚举类型
        /// </summary>
        public enum ");
            _code_.Add(MethodIndexEnumTypeName);
            _code_.Add(@"
        {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseNode.NodeMethod[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseNode.NodeMethod _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.EnumName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// [");
            _code_.Add(_value2_.MethodIndex.ToString());
            _code_.Add(@"] ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorXmlDocument);
                    }
                }
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.XmlFullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" ");
            _code_.Add(_value4_.CodeGeneratorXmlDocument);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// 返回值 ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.XmlFullName);
                    }
                }
            _code_.Add(@" ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorReturnXmlDocument);
                    }
                }
            }
            _code_.Add(@"
            /// </summary>");
            }
            _code_.Add(@"
            ");
            _code_.Add(_value2_.EnumName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MethodIndex.ToString());
            _code_.Add(@",");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
        }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class StreamPersistenceMemoryDatabaseLocalClientNode
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
        /// <summary>
        /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.CodeGeneratorXmlDocument);
                    }
                }
            _code_.Add(@" 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.GenericDefinitionFullName);
                    }
                }
            _code_.Add(@"))]
        ");
            _code_.Add(TypeNameDefinition);
            _if_ = false;
                    if (IsCustomServiceNode)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@" : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode");
            }
            _code_.Add(@"
        {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseLocalClientNode.NodeMethod[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseLocalClientNode.NodeMethod _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorXmlDocument);
                    }
                }
            _code_.Add(@"
            /// </summary>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// <param name=""");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""">");
            _code_.Add(_value4_.CodeGeneratorXmlDocument);
            _code_.Add(@"</param>");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <returns>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorReturnXmlDocument);
                    }
                }
            _code_.Add(@"</returns>");
            }
            _code_.Add(@"
            ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MethodName);
            _code_.Add(@"(");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = default(AutoCSer.CodeGenerator.Metadata.ExtensionType);
                    _value3_ = _value2_.CallbackType;
            _if_ = false;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value5_ = _value4_.Parameters;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]))
                    {
                    if (_value5_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", ");
            }
            _code_.Add(_value3_.FullName);
            _code_.Add(@" callback");
            }
                }
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
        }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class StreamPersistenceMemoryDatabaseClientNode
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                
            _code_.Add(@"
        /// <summary>
        /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.CodeGeneratorXmlDocument);
                    }
                }
            _code_.Add(@" 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.GenericDefinitionFullName);
                    }
                }
            _code_.Add(@"))]
        ");
            _code_.Add(TypeNameDefinition);
            _if_ = false;
                    if (IsCustomServiceNode)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@" : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode");
            }
            _code_.Add(@"
        {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseClientNode.NodeMethod[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseClientNode.NodeMethod _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorXmlDocument);
                    }
                }
            _code_.Add(@"
            /// </summary>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// <param name=""");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""">");
            _code_.Add(_value4_.CodeGeneratorXmlDocument);
            _code_.Add(@"</param>");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <returns>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.CodeGeneratorReturnXmlDocument);
                    }
                }
            _code_.Add(@"</returns>");
            }
            _code_.Add(@"
            ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MethodName);
            _code_.Add(@"(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = default(AutoCSer.CodeGenerator.Metadata.ExtensionType);
                    _value3_ = _value2_.ReturnRequestParameterType;
            _if_ = false;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(_value3_.FullName);
            _code_.Add(@" returnValue");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value5_ = _value4_.Parameters;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]))
                    {
                    if (_value5_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", ");
            }
            }
                }
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = default(AutoCSer.CodeGenerator.Metadata.ExtensionType);
                    _value3_ = _value2_.CallbackType;
            _if_ = false;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value5_ = _value4_.Parameters;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]))
                    {
                    if (_value5_.Length != default(int))
                    {
                        _if_ = true;
                    }
                }
                    }
                }
                }
            if (_if_)
            {
            _code_.Add(@", ");
            }
            _code_.Add(_value3_.FullName);
            _code_.Add(@" callback");
            }
                }
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
        }");
                if (_isOut_) outEnd();
            }
        }
    }
}
#endif