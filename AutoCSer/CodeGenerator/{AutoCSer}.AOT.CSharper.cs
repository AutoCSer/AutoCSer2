//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class AotMethod
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
            /// 触发 AOT 编译
            /// </summary>
            /// <returns></returns>
            public static bool Call()
            {
                if (AutoCSer.Date.StartTimestamp == long.MinValue)
                {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.Method[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.Method _value2_ in _value1_)
                        {
            _code_.Add(@"
                    ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.MethodName);
            _code_.Add(@"();");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = EqualsMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.FieldEquals.Comparor.");
            _code_.Add(_value2_.ReflectionMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"), default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = RandomMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.RandomObject.Creator.");
            _code_.Add(_value2_.ReflectionMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(null);");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = BinarySerializeMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.BinarySerializer.TypeSerialize(typeof(AutoCSer.BinarySerialize.TypeSerializer<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">));");
            _if_ = false;
                    if (_value2_.ReflectionMethodName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    AutoCSer.BinarySerializer.");
            _code_.Add(_value2_.ReflectionMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(null, default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"));");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_ = BinarySerializeMemberTypes;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[]))
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
                    binaryDeserializeMemberTypes();");
            }
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = BinarySerializeGenericTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.BinarySerializer.BinarySerializeGenericType<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">();");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = BinarySerializeGenericMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.BinarySerializer.");
            _code_.Add(_value2_.ReflectionMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(null, null);
                    AutoCSer.BinaryDeserializer.");
            _code_.Add(_value2_.ReflectionMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(null);");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = JsonSerializeMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.JsonSerializer.TypeSerialize(typeof(AutoCSer.Json.TypeSerializer<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">));");
            _if_ = false;
                    if (_value2_.ReflectionMethodName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    AutoCSer.JsonSerializer.");
            _code_.Add(_value2_.ReflectionMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(null, default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"));");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_ = JsonDeserializeMemberTypes;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[]))
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
                    jsonDeserializeMemberTypes();");
            }
            _code_.Add(@"
");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = XmlSerializeMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.XmlSerializer.TypeSerialize(typeof(AutoCSer.Xml.TypeSerializer<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">));");
            _if_ = false;
                    if (_value2_.ReflectionMethodName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    AutoCSer.XmlSerializer.");
            _code_.Add(_value2_.ReflectionMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">");
            }
            _code_.Add(@"(null, default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"));");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_ = XmlDeserializeMemberTypes;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[]))
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
                    xmlDeserializeMemberTypes();");
            }
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = XmlSerializeNullableElementTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                    AutoCSer.XmlSerializer.NullableHasValue<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">(null);");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                    return true;
                }
                return false;
            }");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_ = BinarySerializeMemberTypes;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[]))
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
            /// 二进制反序列化成员类型触发 AOT 编译
            /// </summary>
            private static void binaryDeserializeMemberTypes()
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = BinarySerializeMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.ReflectionMethodName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@");
                AutoCSer.BinaryDeserializer.");
            _code_.Add(_value2_.ReflectionMethodName);
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">(null, ref ");
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
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_ = JsonDeserializeMemberTypes;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[]))
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
            /// JSON 反序列化成员类型触发 AOT 编译
            /// </summary>
            private static void jsonDeserializeMemberTypes()
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = JsonDeserializeMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@");
                AutoCSer.JsonDeserializer.");
            _code_.Add(_value2_.ReflectionMethodName);
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">(null, ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_ = XmlDeserializeMemberTypes;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[]))
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
            /// XML 反序列化成员类型触发 AOT 编译
            /// </summary>
            private static void xmlDeserializeMemberTypes()
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = XmlDeserializeMemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@");
                AutoCSer.XmlDeserializer.");
            _code_.Add(_value2_.ReflectionMethodName);
            _code_.Add(@"<");
            _code_.Add(_value2_.GenericTypeName);
            _code_.Add(@">(null, ref ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }");
            }
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class DefaultConstructor
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
            /// 默认构造函数
            /// </summary>
            internal static ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(DefaultConstructorMethodName);
            _code_.Add(@"()
            {
                return new ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void ");
            _code_.Add(DefaultConstructorReflectionMethodName);
            _code_.Add(@"()
            {
                ");
            _code_.Add(DefaultConstructorMethodName);
            _code_.Add(@"();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class FieldEquals
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
            /// 对象对比
            /// </summary>
            /// <param name=""left""></param>
            /// <param name=""right""></param>
            /// <returns></returns>
            internal static bool ");
            _code_.Add(FieldEqualsMethodName);
            _code_.Add(@"(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" left, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" right)
            {
                return left.fieldEquals(right)");
            _if_ = false;
                    if (BaseType != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@" && AutoCSer.FieldEquals.Comparor.CallEquals<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = BaseType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">(left, right)");
            }
            _code_.Add(@";
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name=""__value__""></param>
            private bool fieldEquals(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" __value__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.FieldEquals.EqualsMember[] _value1_;
                    _value1_ = Fields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.FieldEquals.EqualsMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if(!AutoCSer.FieldEquals.Comparor.");
            _code_.Add(_value2_.EqualsMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
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
            _code_.Add(@", __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@")) return false;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                return true;
            }
            /// <summary>
            /// 对象对比
            /// </summary>
            /// <param name=""left""></param>
            /// <param name=""right""></param>
            /// <param name=""memberMap""></param>
            /// <returns></returns>
            internal static bool ");
            _code_.Add(MemberMapFieldEqualsMethodName);
            _code_.Add(@"(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" left, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" right, AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap)
            {
                return left.fieldEquals(right, memberMap);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name=""__value__""></param>
            /// <param name=""__memberMap__""></param>
            private bool fieldEquals(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" __value__, AutoCSer.Metadata.MemberMap<");
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
                    AutoCSer.CodeGenerator.TemplateGenerator.FieldEquals.EqualsMember[] _value1_;
                    _value1_ = MemberMapFields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.FieldEquals.EqualsMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__memberMap__.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@") && !AutoCSer.FieldEquals.Comparor.");
            _code_.Add(_value2_.EqualsMethodName);
            _if_ = false;
                    if (_value2_.GenericTypeName != default(string))
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
            _code_.Add(@", __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@")) return false;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                return true;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void ");
            _code_.Add(FieldEqualsMethodName);
            _code_.Add(@"()
            {
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" left = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"), right = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@");
                ");
            _code_.Add(FieldEqualsMethodName);
            _code_.Add(@"(left, right);
                ");
            _code_.Add(MemberMapFieldEqualsMethodName);
            _code_.Add(@"(left, right, null);
                AutoCSer.FieldEquals.Comparor.CallEquals<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">(left, right);
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class RandomObject
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
            /// 随机对象生成
            /// </summary>
            /// <param name=""value""></param>
            /// <param name=""config""></param>
            internal static void ");
            _code_.Add(CreateRandomObjectMethodName);
            _code_.Add(@"(ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value, AutoCSer.RandomObject.Config config)
            {
                value.createRandomObject(config);");
            _if_ = false;
                    if (BaseType != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = BaseType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" baseValue = value;
                AutoCSer.RandomObject.Creator.CreateBase<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = BaseType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">(ref baseValue, config);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name=""config""></param>
            private void createRandomObject(AutoCSer.RandomObject.Config __config__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.RandomObject.RandomMember[] _value1_;
                    _value1_ = Fields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.RandomObject.RandomMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = AutoCSer.RandomObject.Creator.Create<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@">(__config__, ");
            _code_.Add(_value2_.IsNullable ? "true" : "false");
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != default(System.Type))
                    {
                    if (_value2_.IsGenericType)
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
            public static void ");
            _code_.Add(CreateRandomObjectMethodName);
            _code_.Add(@"()");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != default(System.Type))
                    {
                if (!(bool)_value2_.IsGenericType)
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
            internal static void ");
            _code_.Add(CreateRandomObjectMethodName);
            _code_.Add(@"()");
            }
            _code_.Add(@"
            {
                var value = AutoCSer.RandomObject.Creator.CallCreate<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();
                ");
            _code_.Add(CreateRandomObjectMethodName);
            _code_.Add(@"(ref value, null);
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class MemberCopy
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
            /// 成员复制
            /// </summary>
            /// <param name=""writeValue""></param>
            /// <param name=""readValue""></param>
            internal static void ");
            _code_.Add(MemberCopyMethodName);
            _code_.Add(@"(ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" writeValue, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" readValue)
            {
                writeValue.memberCopyFrom(readValue);
            }
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name=""__value__""></param>
            private void memberCopyFrom(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" __value__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.MemberCopy.CopyMember[] _value1_;
                    _value1_ = Fields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.MemberCopy.CopyMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name=""writeValue""></param>
            /// <param name=""readValue""></param>
            /// <param name=""memberMap""></param>
            internal static void ");
            _code_.Add(MemberMapCopyMethodName);
            _code_.Add(@"(ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" writeValue, ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" readValue, AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap)
            {
                writeValue.memberCopyFrom(readValue, memberMap);
            }
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name=""readValue""></param>
            /// <param name=""memberMap""></param>
            private void memberCopyFrom(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" __value__, AutoCSer.Metadata.MemberMap<");
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
                    AutoCSer.CodeGenerator.TemplateGenerator.MemberCopy.CopyMember[] _value1_;
                    _value1_ = Fields;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.MemberCopy.CopyMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__memberMap__.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@")) ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void ");
            _code_.Add(MemberCopyMethodName);
            _code_.Add(@"()
            {
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" writeValue = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"), readValue = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@");
                ");
            _code_.Add(MemberCopyMethodName);
            _code_.Add(@"(ref writeValue, readValue);
                ");
            _code_.Add(MemberMapCopyMethodName);
            _code_.Add(@"(ref writeValue, readValue, null);
                AutoCSer.MemberCopy<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">.Copy(ref writeValue, readValue);
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
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
            internal static void ");
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
            {
                if (serializer.WriteMemberCountVerify(");
            _code_.Add(FixedSize.ToString());
            _code_.Add(@", ");
            _code_.Add(MemberCountVerify.ToString());
            _code_.Add(@")) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""__serializer__""></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
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
            _if_ = false;
                    if (FixedFillSize != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __serializer__.FixedFillSize(");
            _code_.Add(FixedFillSize.ToString());
            _code_.Add(@");");
            }
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
            /// <param name=""deserializer""></param>
            /// <param name=""value""></param>
            internal static void ");
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
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
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
            _if_ = false;
                    if (FixedFillSize != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.FixedFillSize(");
            _code_.Add(FixedFillSize.ToString());
            _code_.Add(@");");
            }
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
                binaryFieldDeserialize(__deserializer__);");
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
            /// 二进制反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
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
            internal static void ");
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
                    if (IsMemberMapFixedFillSize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                int startIndex = serializer.Stream.GetPrepSizeCurrentIndex(");
            _code_.Add(FixedSize.ToString());
            _code_.Add(@");
                if (startIndex >= 0) value.binarySerialize(memberMap, serializer, startIndex);");
            }
            _if_ = false;
                if (!(bool)IsMemberMapFixedFillSize)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                value.binarySerialize(memberMap, serializer);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name=""__memberMap__""></param>
            /// <param name=""__serializer__""></param>
            /// <param name=""__startIndex__""></param>
            private void binarySerialize(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> __memberMap__, AutoCSer.BinarySerializer __serializer__");
            _if_ = false;
                    if (IsMemberMapFixedFillSize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", int __startIndex__");
            }
            _code_.Add(@")
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
            _if_ = false;
                    if (IsMemberMapFixedFillSize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __serializer__.SerializeFill(__startIndex__);");
            }
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
            /// <param name=""memberMap""></param>
            /// <param name=""deserializer""></param>
            /// <param name=""value""></param>
            internal static void ");
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
            {
                value.binaryDeserialize(memberMap, deserializer);
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
            _if_ = false;
                    if (IsMemberMapFixedFillSize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.SetFixedCurrent();");
            }
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
            _if_ = false;
                    if (IsMemberMapFixedFillSize)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __deserializer__.SetFixedCurrentEnd();");
            }
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
            _if_ = false;
                    if (MemberTypeCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> ");
            _code_.Add(BinarySerializeMemberTypeMethodName);
            _code_.Add(@"()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(");
            _code_.Add(MemberTypeCount.ToString());
            _code_.Add(@");");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = MemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                types.Add(typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                return types;
            }");
            }
            _code_.Add(@"
            /// <summary>
            /// 获取二进制序列化成员数量信息
            /// </summary>
            /// <returns></returns>
            internal static int ");
            _code_.Add(BinarySerializeMemberCountVerifyMethodName);
            _code_.Add(@"()
            {
                return ");
            _code_.Add(MemberCountVerify.ToString());
            _code_.Add(@";
            }
            /// <summary>
            /// 二进制序列化触发 AOT 编译
            /// </summary>
            internal static void ");
            _code_.Add(BinarySerializeMethodName);
            _code_.Add(@"()
            {
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@");
                ");
            _code_.Add(BinarySerializeMethodName);
            _code_.Add(@"(null, value);
                ");
            _code_.Add(BinaryDeserializeMethodName);
            _code_.Add(@"(null, ref value);");
            _if_ = false;
                    if (IsMemberMap)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
            _code_.Add(BinarySerializeMemberMapMethodName);
            _code_.Add(@"(null, null, value);
                ");
            _code_.Add(BinaryDeserializeMemberMapMethodName);
            _code_.Add(@"(null, null, ref value);");
            }
            _code_.Add(@"
                AutoCSer.BinarySerializer.BinarySerialize<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();
                AutoCSer.BinaryDeserializer.BinaryDeserialize<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();");
            _if_ = false;
                    if (MemberTypeCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
            _code_.Add(BinarySerializeMemberTypeMethodName);
            _code_.Add(@"();");
            }
            _code_.Add(@"
                ");
            _code_.Add(BinarySerializeMemberCountVerifyMethodName);
            _code_.Add(@"();
            }");
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
            internal static void ");
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
            internal static void ");
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
            internal static void ");
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
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
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
            internal static void ");
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
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                value.jsonDeserialize(deserializer, ref names, memberMap);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
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
                    __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref ");
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
                    __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref this.");
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
                    __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref ");
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
                    __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref this.");
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
            }
            _code_.Add(@"
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            /// <param name=""__value__""></param>
            /// <param name=""__memberIndex__""></param>
            internal static void ");
            _code_.Add(JsonDeserializeMethodName);
            _code_.Add(@"(AutoCSer.JsonDeserializer __deserializer__, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" __value__, int __memberIndex__)
            {");
            _if_ = false;
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                switch (__memberIndex__)
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
                    case ");
            _code_.Add(_value2_.IntMemberIndex.ToString());
            _code_.Add(@":");
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
                        __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref ");
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
                        __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
                        return;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                }");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> ");
            _code_.Add(JsonDeserializeMemberNameMethodName);
            _code_.Add(@"()
            {");
            _if_ = false;
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                return jsonDeserializeMemberName();");
            }
            _if_ = false;
                if (DeserializeMemberCount == default(int))
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                return default(AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(");
            _code_.Add(DeserializeMemberCount.ToString());
            _code_.Add(@");
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(");
            _code_.Add(DeserializeMemberCount.ToString());
            _code_.Add(@");");
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
                names.Add(nameof(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@"));
                indexs.Add(");
            _code_.Add(_value2_.IntMemberIndex.ToString());
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }");
            }
            _if_ = false;
                    if (MemberTypeCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> ");
            _code_.Add(JsonSerializeMemberTypeMethodName);
            _code_.Add(@"()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(");
            _code_.Add(MemberTypeCount.ToString());
            _code_.Add(@");");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = MemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                types.Add(typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                return types;
            }");
            }
            _code_.Add(@"
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != default(System.Type))
                    {
                    if (_value2_.IsGenericType)
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
            public static void ");
            _code_.Add(JsonSerializeMethodName);
            _code_.Add(@"()");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != default(System.Type))
                    {
                if (!(bool)_value2_.IsGenericType)
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
            internal static void ");
            _code_.Add(JsonSerializeMethodName);
            _code_.Add(@"()");
            }
            _code_.Add(@"
            {
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@");
                ");
            _code_.Add(JsonSerializeMethodName);
            _code_.Add(@"(null, value);
                ");
            _code_.Add(JsonSerializeMemberMapMethodName);
            _code_.Add(@"(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                ");
            _code_.Add(JsonDeserializeMethodName);
            _code_.Add(@"(null, ref value, ref names);
                ");
            _code_.Add(JsonDeserializeMemberMapMethodName);
            _code_.Add(@"(null, ref value, ref names, null);
                ");
            _code_.Add(JsonDeserializeMethodName);
            _code_.Add(@"(null, ref value, 0);
                ");
            _code_.Add(JsonDeserializeMemberNameMethodName);
            _code_.Add(@"();
                AutoCSer.JsonSerializer.JsonSerialize<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();
                AutoCSer.JsonDeserializer.JsonDeserialize<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();");
            _if_ = false;
                    if (MemberTypeCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
            _code_.Add(JsonSerializeMemberTypeMethodName);
            _code_.Add(@"();");
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
            internal static void ");
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
            internal static byte* ");
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
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void ");
            _code_.Add(SimpleSerializeMethodName);
            _code_.Add(@"()
            {
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@");
                ");
            _code_.Add(SimpleSerializeMethodName);
            _code_.Add(@"(null, ref value);
                ");
            _code_.Add(SimpleDeserializeMethodName);
            _code_.Add(@"(null, ref value, null);
                AutoCSer.SimpleSerialize.Serializer.Serialize<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">(null, default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"));
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class XmlSerialize
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
            /// XML 序列化
            /// </summary>
            /// <param name=""serializer""></param>
            /// <param name=""value""></param>
            internal static void ");
            _code_.Add(XmlSerializeMethodName);
            _code_.Add(@"(AutoCSer.XmlSerializer serializer, ");
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
                    AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[] _value1_ = Members;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[]))
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
                value.xmlSerialize(serializer);");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name=""memberMap""></param>
            /// <param name=""serializer""></param>
            /// <param name=""value""></param>
            /// <param name=""stream""></param>
            internal static void ");
            _code_.Add(XmlSerializeMemberMapMethodName);
            _code_.Add(@"(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> memberMap, XmlSerializer serializer, ");
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
                    AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[] _value1_ = Members;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[]))
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
                value.xmlSerialize(memberMap, serializer, stream);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[] _value1_ = Members;
                    if (_value1_ != default(AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[]))
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
            /// XML 序列化
            /// </summary>
            /// <param name=""__serializer__""></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[] _value1_;
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.IsOutputMethodName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (AutoCSer.XmlSerializer.");
            _code_.Add(_value2_.IsOutputMethodName);
            _code_.Add(@"(__serializer__, ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@"))");
            }
            _code_.Add(@"
                {
                    __stream__.SimpleWrite(@""");
            _code_.Add(_value2_.SerializeMemberNameStart);
            _code_.Add(@""");");
            _if_ = false;
                    if (_value2_.MemberItemName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    AutoCSer.XmlSerializer.SetItemName(__serializer__, """);
            _code_.Add(_value2_.MemberItemName);
            _code_.Add(@""");");
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
            _code_.Add(_value2_.EnumXmlSerializeMethodName);
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
            _code_.Add(@" != null) __serializer__.");
            _code_.Add(_value2_.SerializeMethodName);
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                    __stream__.SimpleWrite(@""");
            _code_.Add(_value2_.SerializeMemberNameEnd);
            _code_.Add(@""");
                }");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name=""__memberMap__""></param>
            /// <param name=""__serializer__""></param>
            /// <param name=""__stream__""></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@"> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[] _value1_;
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                if (__memberMap__.IsMember(");
            _code_.Add(_value2_.MemberIndex.ToString());
            _code_.Add(@")");
            _if_ = false;
                    if (_value2_.IsOutputMethodName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@" && AutoCSer.XmlSerializer.");
            _code_.Add(_value2_.IsOutputMethodName);
            _code_.Add(@"(__serializer__, ");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@")");
            }
            _code_.Add(@")
                {
                    __stream__.SimpleWrite(@""");
            _code_.Add(_value2_.SerializeMemberNameStart);
            _code_.Add(@""");");
            _if_ = false;
                    if (_value2_.MemberItemName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                    AutoCSer.XmlSerializer.SetItemName(__serializer__, """);
            _code_.Add(_value2_.MemberItemName);
            _code_.Add(@""");");
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
            _code_.Add(_value2_.EnumXmlSerializeMethodName);
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
            _code_.Add(@" != null) __serializer__.");
            _code_.Add(_value2_.SerializeMethodName);
            _code_.Add(@"(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            _code_.Add(@"
                    __stream__.SimpleWrite(@""");
            _code_.Add(_value2_.SerializeMemberNameEnd);
            _code_.Add(@""");
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
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name=""__deserializer__""></param>
            /// <param name=""__value__""></param>
            /// <param name=""__memberIndex__""></param>
            internal static void ");
            _code_.Add(XmlDeserializeMethodName);
            _code_.Add(@"(AutoCSer.XmlDeserializer __deserializer__, ref ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" __value__, int __memberIndex__)
            {");
            _if_ = false;
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                switch (__memberIndex__)
                {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[] _value1_;
                    _value1_ = DeserializeMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                    case ");
            _code_.Add(_value2_.IntMemberIndex.ToString());
            _code_.Add(@":");
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
            _code_.Add(_value2_.EnumXmlDeserializeMethodName);
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
                        __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref ");
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
            _code_.Add(_value2_.EnumXmlDeserializeMethodName);
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
                        __deserializer__.");
            _code_.Add(_value2_.DeserializeMethodName);
            _code_.Add(@"(ref __value__.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@");");
            }
            }
            _code_.Add(@"
                        return;");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                }");
            }
            _code_.Add(@"
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> ");
            _code_.Add(XmlDeserializeMemberNameMethodName);
            _code_.Add(@"()
            {");
            _if_ = false;
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                return xmlDeserializeMemberName();");
            }
            _if_ = false;
                if (DeserializeMemberCount == default(int))
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                return default(AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>);");
            }
            _code_.Add(@"
            }");
            _if_ = false;
                    if (DeserializeMemberCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(");
            _code_.Add(DeserializeMemberCount.ToString());
            _code_.Add(@");
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(");
            _code_.Add(DeserializeMemberCount.ToString());
            _code_.Add(@");");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember[] _value1_;
                    _value1_ = DeserializeMembers;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.XmlSerialize.SerializeMember _value2_ in _value1_)
                        {
            _code_.Add(@"
                names.Add(nameof(");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@"));");
            _if_ = false;
                    if (_value2_.MemberItemName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                indexs.Add(new KeyValue<int, string>(");
            _code_.Add(_value2_.IntMemberIndex.ToString());
            _code_.Add(@", """);
            _code_.Add(_value2_.MemberItemName);
            _code_.Add(@"""));");
            }
            _if_ = false;
                if (_value2_.MemberItemName == default(string))
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                indexs.Add(new KeyValue<int, string>(");
            _code_.Add(_value2_.IntMemberIndex.ToString());
            _code_.Add(@", null));");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }");
            }
            _if_ = false;
                    if (MemberTypeCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> ");
            _code_.Add(XmlSerializeMemberTypeMethodName);
            _code_.Add(@"()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(");
            _code_.Add(MemberTypeCount.ToString());
            _code_.Add(@");");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType[] _value1_;
                    _value1_ = MemberTypes;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.AotMethod.ReflectionMemberType _value2_ in _value1_)
                        {
            _code_.Add(@"
                types.Add(typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@"));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                return types;
            }");
            }
            _code_.Add(@"
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>");
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != default(System.Type))
                    {
                    if (_value2_.IsGenericType)
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
            public static void ");
            _code_.Add(XmlSerializeMethodName);
            _code_.Add(@"()");
            }
            _if_ = false;
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
                {
                    System.Type _value2_ = _value1_.Type;
                    if (_value2_ != default(System.Type))
                    {
                if (!(bool)_value2_.IsGenericType)
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
            internal static void ");
            _code_.Add(XmlSerializeMethodName);
            _code_.Add(@"()");
            }
            _code_.Add(@"
            {
                ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@");
                ");
            _code_.Add(XmlSerializeMethodName);
            _code_.Add(@"(null, value);
                ");
            _code_.Add(XmlSerializeMemberMapMethodName);
            _code_.Add(@"(null, null, value, null);
                ");
            _code_.Add(XmlDeserializeMethodName);
            _code_.Add(@"(null, ref value, 0);
                ");
            _code_.Add(XmlDeserializeMemberNameMethodName);
            _code_.Add(@"();
                AutoCSer.XmlSerializer.XmlSerialize<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();
                AutoCSer.XmlDeserializer.XmlDeserialize<");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@">();");
            _if_ = false;
                    if (MemberTypeCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                ");
            _code_.Add(XmlSerializeMemberTypeMethodName);
            _code_.Add(@"();");
            }
            _code_.Add(@"
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
#endif