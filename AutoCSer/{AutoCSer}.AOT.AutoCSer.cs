//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Algorithm
{
    internal partial struct IntegerDivision
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.Algorithm.IntegerDivision value)
            {
                serializer.Simple(value);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.Algorithm.IntegerDivision value)
            {
                deserializer.Simple(ref value);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                return new AutoCSer.BinarySerialize.TypeInfo(true, 0, 1073741829);
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.Algorithm.IntegerDivision value = default(AutoCSer.Algorithm.IntegerDivision);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Algorithm.IntegerDivision));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Algorithm.IntegerDivision));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.Algorithm.IntegerDivision>();
            }
    }
}namespace AutoCSer.Net.CommandServer
{
    internal partial struct CancelKeepCallbackData
    {
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.Net.CommandServer.CancelKeepCallbackData value)
            {
                deserializer.Simple(ref value);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                return new AutoCSer.BinarySerialize.TypeInfo(true, 0, 1073741828);
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.Net.CommandServer.CancelKeepCallbackData value = default(AutoCSer.Net.CommandServer.CancelKeepCallbackData);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Net.CommandServer.CancelKeepCallbackData));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Net.CommandServer.CancelKeepCallbackData));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.Net.CommandServer.CancelKeepCallbackData>();
            }
    }
}namespace AutoCSer.Reflection
{
    public partial struct RemoteType
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.Reflection.RemoteType value)
            {
                serializer.Simple(value);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.Reflection.RemoteType value)
            {
                deserializer.Simple(ref value);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                return new AutoCSer.BinarySerialize.TypeInfo(true, 0, 1073741826);
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.Reflection.RemoteType value = default(AutoCSer.Reflection.RemoteType);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Reflection.RemoteType));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Reflection.RemoteType));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.Reflection.RemoteType>();
            }
    }
}namespace AutoCSer.Net.CommandServer
{
    internal partial struct CommandControllerOutputData
    {
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.Net.CommandServer.CommandControllerOutputData value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.ControllerIndex);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.ControllerName);
                __deserializer__.BinaryDeserialize(ref this.MethodNames);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 0, 1073741827);
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.Net.CommandServer.CommandControllerOutputData value = default(AutoCSer.Net.CommandServer.CommandControllerOutputData);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Net.CommandServer.CommandControllerOutputData));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Net.CommandServer.CommandControllerOutputData));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.Net.CommandServer.CommandControllerOutputData>();
            }
    }
}namespace AutoCSer
{
    public partial struct KeyValue<KT,VT>
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.KeyValue<KT,VT> value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.KeyValue<KT,VT>> memberMap, JsonSerializer serializer, AutoCSer.KeyValue<KT,VT> value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""Key"":");
                if (Key == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerializeType(Key);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Value"":");
                if (Value == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerializeType(Value);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.KeyValue<KT,VT>> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Key"":");
                    if (Key == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerializeType(Key);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Value"":");
                    if (Value == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerializeType(Value);
                }
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.KeyValue<KT,VT> value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.KeyValue<KT,VT> value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.KeyValue<KT,VT>> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Key);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Value);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.KeyValue<KT,VT>> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Key);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Value);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.KeyValue<KT,VT> __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.Key);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.Value);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(2);
                names.Add(nameof(Key));
                indexs.Add(0);
                names.Add(nameof(Value));
                indexs.Add(1);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            public static void JsonSerialize()
            {
                AutoCSer.KeyValue<KT,VT> value = default(AutoCSer.KeyValue<KT,VT>);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.KeyValue<KT,VT>));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.KeyValue<KT,VT>));
            }
    }
}namespace AutoCSer.Reflection
{
    public partial struct RemoteType
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.Reflection.RemoteType value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Reflection.RemoteType> memberMap, JsonSerializer serializer, AutoCSer.Reflection.RemoteType value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""AssemblyName"":");
                if (AssemblyName == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerialize(AssemblyName);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Name"":");
                if (Name == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerialize(Name);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Reflection.RemoteType> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""AssemblyName"":");
                    if (AssemblyName == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerialize(AssemblyName);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Name"":");
                    if (Name == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerialize(Name);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(string));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.Reflection.RemoteType value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.Reflection.RemoteType value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.Reflection.RemoteType> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.AssemblyName);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Name);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.Reflection.RemoteType> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.AssemblyName);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Name);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Reflection.RemoteType __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.AssemblyName);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.Name);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(2);
                names.Add(nameof(AssemblyName));
                indexs.Add(0);
                names.Add(nameof(Name));
                indexs.Add(1);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.Reflection.RemoteType value = default(AutoCSer.Reflection.RemoteType);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Reflection.RemoteType));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Reflection.RemoteType));
            }
    }
}namespace AutoCSer
{
    internal partial struct SerializeComplex
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.SerializeComplex value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeComplex> memberMap, JsonSerializer serializer, AutoCSer.SerializeComplex value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""Imaginary"":");
                __serializer__.JsonSerialize(Imaginary);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Real"":");
                __serializer__.JsonSerialize(Real);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeComplex> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Imaginary"":");
                    __serializer__.JsonSerialize(Imaginary);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Real"":");
                    __serializer__.JsonSerialize(Real);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(double));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeComplex value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeComplex value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeComplex> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Imaginary);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Real);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeComplex> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Imaginary);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Real);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.SerializeComplex __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.Imaginary);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.Real);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(2);
                names.Add(nameof(Imaginary));
                indexs.Add(0);
                names.Add(nameof(Real));
                indexs.Add(1);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.SerializeComplex value = default(AutoCSer.SerializeComplex);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.SerializeComplex));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.SerializeComplex));
            }
    }
}namespace AutoCSer
{
    internal partial struct SerializeMatrix3x2
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.SerializeMatrix3x2 value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeMatrix3x2> memberMap, JsonSerializer serializer, AutoCSer.SerializeMatrix3x2 value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""M11"":");
                __serializer__.JsonSerialize(M11);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M12"":");
                __serializer__.JsonSerialize(M12);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M21"":");
                __serializer__.JsonSerialize(M21);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M22"":");
                __serializer__.JsonSerialize(M22);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M31"":");
                __serializer__.JsonSerialize(M31);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M32"":");
                __serializer__.JsonSerialize(M32);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeMatrix3x2> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M11"":");
                    __serializer__.JsonSerialize(M11);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M12"":");
                    __serializer__.JsonSerialize(M12);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M21"":");
                    __serializer__.JsonSerialize(M21);
                }
                if (__memberMap__.IsMember(3))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M22"":");
                    __serializer__.JsonSerialize(M22);
                }
                if (__memberMap__.IsMember(4))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M31"":");
                    __serializer__.JsonSerialize(M31);
                }
                if (__memberMap__.IsMember(5))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M32"":");
                    __serializer__.JsonSerialize(M32);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeMatrix3x2 value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeMatrix3x2 value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeMatrix3x2> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M11);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M12);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M21);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M22);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M31);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M32);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeMatrix3x2> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M11);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M12);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M21);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M22);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(3);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M31);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(4);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M32);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(5);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.SerializeMatrix3x2 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.M11);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.M12);
                        return;
                    case 2:
                        __deserializer__.JsonDeserialize(ref __value__.M21);
                        return;
                    case 3:
                        __deserializer__.JsonDeserialize(ref __value__.M22);
                        return;
                    case 4:
                        __deserializer__.JsonDeserialize(ref __value__.M31);
                        return;
                    case 5:
                        __deserializer__.JsonDeserialize(ref __value__.M32);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(6);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(6);
                names.Add(nameof(M11));
                indexs.Add(0);
                names.Add(nameof(M12));
                indexs.Add(1);
                names.Add(nameof(M21));
                indexs.Add(2);
                names.Add(nameof(M22));
                indexs.Add(3);
                names.Add(nameof(M31));
                indexs.Add(4);
                names.Add(nameof(M32));
                indexs.Add(5);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.SerializeMatrix3x2 value = default(AutoCSer.SerializeMatrix3x2);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.SerializeMatrix3x2));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.SerializeMatrix3x2));
            }
    }
}namespace AutoCSer
{
    internal partial struct SerializeMatrix4x4
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.SerializeMatrix4x4 value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeMatrix4x4> memberMap, JsonSerializer serializer, AutoCSer.SerializeMatrix4x4 value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""M11"":");
                __serializer__.JsonSerialize(M11);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M12"":");
                __serializer__.JsonSerialize(M12);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M13"":");
                __serializer__.JsonSerialize(M13);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M14"":");
                __serializer__.JsonSerialize(M14);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M21"":");
                __serializer__.JsonSerialize(M21);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M22"":");
                __serializer__.JsonSerialize(M22);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M23"":");
                __serializer__.JsonSerialize(M23);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M24"":");
                __serializer__.JsonSerialize(M24);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M31"":");
                __serializer__.JsonSerialize(M31);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M32"":");
                __serializer__.JsonSerialize(M32);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M33"":");
                __serializer__.JsonSerialize(M33);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M34"":");
                __serializer__.JsonSerialize(M34);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M41"":");
                __serializer__.JsonSerialize(M41);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M42"":");
                __serializer__.JsonSerialize(M42);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M43"":");
                __serializer__.JsonSerialize(M43);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""M44"":");
                __serializer__.JsonSerialize(M44);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeMatrix4x4> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M11"":");
                    __serializer__.JsonSerialize(M11);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M12"":");
                    __serializer__.JsonSerialize(M12);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M13"":");
                    __serializer__.JsonSerialize(M13);
                }
                if (__memberMap__.IsMember(3))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M14"":");
                    __serializer__.JsonSerialize(M14);
                }
                if (__memberMap__.IsMember(4))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M21"":");
                    __serializer__.JsonSerialize(M21);
                }
                if (__memberMap__.IsMember(5))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M22"":");
                    __serializer__.JsonSerialize(M22);
                }
                if (__memberMap__.IsMember(6))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M23"":");
                    __serializer__.JsonSerialize(M23);
                }
                if (__memberMap__.IsMember(7))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M24"":");
                    __serializer__.JsonSerialize(M24);
                }
                if (__memberMap__.IsMember(8))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M31"":");
                    __serializer__.JsonSerialize(M31);
                }
                if (__memberMap__.IsMember(9))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M32"":");
                    __serializer__.JsonSerialize(M32);
                }
                if (__memberMap__.IsMember(10))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M33"":");
                    __serializer__.JsonSerialize(M33);
                }
                if (__memberMap__.IsMember(11))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M34"":");
                    __serializer__.JsonSerialize(M34);
                }
                if (__memberMap__.IsMember(12))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M41"":");
                    __serializer__.JsonSerialize(M41);
                }
                if (__memberMap__.IsMember(13))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M42"":");
                    __serializer__.JsonSerialize(M42);
                }
                if (__memberMap__.IsMember(14))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M43"":");
                    __serializer__.JsonSerialize(M43);
                }
                if (__memberMap__.IsMember(15))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""M44"":");
                    __serializer__.JsonSerialize(M44);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeMatrix4x4 value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeMatrix4x4 value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeMatrix4x4> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M11);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M12);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M13);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M14);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M21);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M22);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M23);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M24);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M31);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M32);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M33);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M34);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M41);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M42);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M43);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M44);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeMatrix4x4> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M11);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M12);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M13);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M14);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(3);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M21);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(4);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M22);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(5);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M23);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(6);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M24);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(7);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M31);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(8);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M32);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(9);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M33);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(10);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M34);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(11);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M41);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(12);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M42);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(13);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M43);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(14);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.M44);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(15);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.SerializeMatrix4x4 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.M11);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.M12);
                        return;
                    case 2:
                        __deserializer__.JsonDeserialize(ref __value__.M13);
                        return;
                    case 3:
                        __deserializer__.JsonDeserialize(ref __value__.M14);
                        return;
                    case 4:
                        __deserializer__.JsonDeserialize(ref __value__.M21);
                        return;
                    case 5:
                        __deserializer__.JsonDeserialize(ref __value__.M22);
                        return;
                    case 6:
                        __deserializer__.JsonDeserialize(ref __value__.M23);
                        return;
                    case 7:
                        __deserializer__.JsonDeserialize(ref __value__.M24);
                        return;
                    case 8:
                        __deserializer__.JsonDeserialize(ref __value__.M31);
                        return;
                    case 9:
                        __deserializer__.JsonDeserialize(ref __value__.M32);
                        return;
                    case 10:
                        __deserializer__.JsonDeserialize(ref __value__.M33);
                        return;
                    case 11:
                        __deserializer__.JsonDeserialize(ref __value__.M34);
                        return;
                    case 12:
                        __deserializer__.JsonDeserialize(ref __value__.M41);
                        return;
                    case 13:
                        __deserializer__.JsonDeserialize(ref __value__.M42);
                        return;
                    case 14:
                        __deserializer__.JsonDeserialize(ref __value__.M43);
                        return;
                    case 15:
                        __deserializer__.JsonDeserialize(ref __value__.M44);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(16);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(16);
                names.Add(nameof(M11));
                indexs.Add(0);
                names.Add(nameof(M12));
                indexs.Add(1);
                names.Add(nameof(M13));
                indexs.Add(2);
                names.Add(nameof(M14));
                indexs.Add(3);
                names.Add(nameof(M21));
                indexs.Add(4);
                names.Add(nameof(M22));
                indexs.Add(5);
                names.Add(nameof(M23));
                indexs.Add(6);
                names.Add(nameof(M24));
                indexs.Add(7);
                names.Add(nameof(M31));
                indexs.Add(8);
                names.Add(nameof(M32));
                indexs.Add(9);
                names.Add(nameof(M33));
                indexs.Add(10);
                names.Add(nameof(M34));
                indexs.Add(11);
                names.Add(nameof(M41));
                indexs.Add(12);
                names.Add(nameof(M42));
                indexs.Add(13);
                names.Add(nameof(M43));
                indexs.Add(14);
                names.Add(nameof(M44));
                indexs.Add(15);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.SerializeMatrix4x4 value = default(AutoCSer.SerializeMatrix4x4);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.SerializeMatrix4x4));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.SerializeMatrix4x4));
            }
    }
}namespace AutoCSer
{
    internal partial struct SerializePlane
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.SerializePlane value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.SerializePlane> memberMap, JsonSerializer serializer, AutoCSer.SerializePlane value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""D"":");
                __serializer__.JsonSerialize(D);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Normal"":");
                __serializer__.JsonSerializeType(Normal);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.SerializePlane> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""D"":");
                    __serializer__.JsonSerialize(D);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Normal"":");
                    __serializer__.JsonSerializeType(Normal);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(2);
                types.Add(typeof(float));
                types.Add(typeof(AutoCSer.SerializeVector3));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializePlane value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializePlane value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.SerializePlane> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.D);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Normal);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.SerializePlane> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.D);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Normal);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.SerializePlane __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.D);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.Normal);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(2);
                names.Add(nameof(D));
                indexs.Add(0);
                names.Add(nameof(Normal));
                indexs.Add(1);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.SerializePlane value = default(AutoCSer.SerializePlane);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.SerializePlane));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.SerializePlane));
            }
    }
}namespace AutoCSer
{
    internal partial struct SerializeQuaternion
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.SerializeQuaternion value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeQuaternion> memberMap, JsonSerializer serializer, AutoCSer.SerializeQuaternion value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""W"":");
                __serializer__.JsonSerialize(W);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""X"":");
                __serializer__.JsonSerialize(X);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Y"":");
                __serializer__.JsonSerialize(Y);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Z"":");
                __serializer__.JsonSerialize(Z);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeQuaternion> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""W"":");
                    __serializer__.JsonSerialize(W);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""X"":");
                    __serializer__.JsonSerialize(X);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Y"":");
                    __serializer__.JsonSerialize(Y);
                }
                if (__memberMap__.IsMember(3))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Z"":");
                    __serializer__.JsonSerialize(Z);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeQuaternion value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeQuaternion value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeQuaternion> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.W);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.X);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Y);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Z);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeQuaternion> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.W);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.X);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Y);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Z);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(3);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.SerializeQuaternion __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.W);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.X);
                        return;
                    case 2:
                        __deserializer__.JsonDeserialize(ref __value__.Y);
                        return;
                    case 3:
                        __deserializer__.JsonDeserialize(ref __value__.Z);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(4);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(4);
                names.Add(nameof(W));
                indexs.Add(0);
                names.Add(nameof(X));
                indexs.Add(1);
                names.Add(nameof(Y));
                indexs.Add(2);
                names.Add(nameof(Z));
                indexs.Add(3);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.SerializeQuaternion value = default(AutoCSer.SerializeQuaternion);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.SerializeQuaternion));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.SerializeQuaternion));
            }
    }
}namespace AutoCSer
{
    internal partial struct SerializeVector2
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.SerializeVector2 value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector2> memberMap, JsonSerializer serializer, AutoCSer.SerializeVector2 value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""X"":");
                __serializer__.JsonSerialize(X);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Y"":");
                __serializer__.JsonSerialize(Y);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector2> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""X"":");
                    __serializer__.JsonSerialize(X);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Y"":");
                    __serializer__.JsonSerialize(Y);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeVector2 value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeVector2 value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector2> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.X);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Y);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector2> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.X);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Y);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.SerializeVector2 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.X);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.Y);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(2);
                names.Add(nameof(X));
                indexs.Add(0);
                names.Add(nameof(Y));
                indexs.Add(1);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.SerializeVector2 value = default(AutoCSer.SerializeVector2);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.SerializeVector2));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.SerializeVector2));
            }
    }
}namespace AutoCSer
{
    internal partial struct SerializeVector3
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.SerializeVector3 value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector3> memberMap, JsonSerializer serializer, AutoCSer.SerializeVector3 value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""X"":");
                __serializer__.JsonSerialize(X);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Y"":");
                __serializer__.JsonSerialize(Y);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Z"":");
                __serializer__.JsonSerialize(Z);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector3> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""X"":");
                    __serializer__.JsonSerialize(X);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Y"":");
                    __serializer__.JsonSerialize(Y);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Z"":");
                    __serializer__.JsonSerialize(Z);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeVector3 value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeVector3 value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector3> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.X);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Y);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Z);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector3> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.X);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Y);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Z);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.SerializeVector3 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.X);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.Y);
                        return;
                    case 2:
                        __deserializer__.JsonDeserialize(ref __value__.Z);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(3);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(3);
                names.Add(nameof(X));
                indexs.Add(0);
                names.Add(nameof(Y));
                indexs.Add(1);
                names.Add(nameof(Z));
                indexs.Add(2);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.SerializeVector3 value = default(AutoCSer.SerializeVector3);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.SerializeVector3));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.SerializeVector3));
            }
    }
}namespace AutoCSer
{
    internal partial struct SerializeVector4
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.SerializeVector4 value)
            {
                value.jsonSerialize(serializer);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector4> memberMap, JsonSerializer serializer, AutoCSer.SerializeVector4 value, AutoCSer.Memory.CharStream stream)
            {
                value.jsonSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void jsonSerialize(AutoCSer.JsonSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                __stream__.SimpleWrite(@"""W"":");
                __serializer__.JsonSerialize(W);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""X"":");
                __serializer__.JsonSerialize(X);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Y"":");
                __serializer__.JsonSerialize(Y);
                __stream__.Write(',');
                __stream__.SimpleWrite(@"""Z"":");
                __serializer__.JsonSerialize(Z);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector4> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""W"":");
                    __serializer__.JsonSerialize(W);
                }
                if (__memberMap__.IsMember(1))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""X"":");
                    __serializer__.JsonSerialize(X);
                }
                if (__memberMap__.IsMember(2))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Y"":");
                    __serializer__.JsonSerialize(Y);
                }
                if (__memberMap__.IsMember(3))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""Z"":");
                    __serializer__.JsonSerialize(Z);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeVector4 value, ref AutoCSer.Memory.Pointer names)
            {
                value.jsonDeserialize(deserializer, ref names);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.SerializeVector4 value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector4> memberMap)
            {
                value.jsonDeserialize(deserializer, ref names, memberMap);
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.W);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.X);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Y);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Z);
                    if (!AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) return;
                }
                else return;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__names__"></param>
            /// <param name="__memberMap__"></param>
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.SerializeVector4> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.W);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.X);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(1);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Y);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(2);
                    else return;
                }
                else return;
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.Z);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(3);
                    else return;
                }
                else return;
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.SerializeVector4 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.W);
                        return;
                    case 1:
                        __deserializer__.JsonDeserialize(ref __value__.X);
                        return;
                    case 2:
                        __deserializer__.JsonDeserialize(ref __value__.Y);
                        return;
                    case 3:
                        __deserializer__.JsonDeserialize(ref __value__.Z);
                        return;
                }
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return jsonDeserializeMemberName();
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> jsonDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(4);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(4);
                names.Add(nameof(W));
                indexs.Add(0);
                names.Add(nameof(X));
                indexs.Add(1);
                names.Add(nameof(Y));
                indexs.Add(2);
                names.Add(nameof(Z));
                indexs.Add(3);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.SerializeVector4 value = default(AutoCSer.SerializeVector4);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.SerializeVector4));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.SerializeVector4));
            }
    }
}namespace AutoCSer.Algorithm
{
    internal partial struct IntegerDivision
    {
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.Algorithm.IntegerDivision value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(24))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.Multiplier);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.Divisor);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.HighBitQuotient);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.HighBitMod);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.ShiftBit);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.Algorithm.IntegerDivision value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private unsafe byte* simpleDeserialize(byte* __start__, byte* __end__)
            {
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.Multiplier);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.Divisor);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.HighBitQuotient);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.HighBitMod);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.ShiftBit);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.Algorithm.IntegerDivision value = default(AutoCSer.Algorithm.IntegerDivision);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Algorithm.IntegerDivision));
            }
    }
}namespace AutoCSer.Net.CommandServer
{
    internal partial struct CancelKeepCallbackData
    {
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.Net.CommandServer.CancelKeepCallbackData value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private unsafe byte* simpleDeserialize(byte* __start__, byte* __end__)
            {
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.Identity);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.Index);
                byte ReturnType = 0;
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref ReturnType);
                this.ReturnType = (AutoCSer.Net.CommandClientReturnTypeEnum)ReturnType;
                __start__ += 3;
                if (__start__ == null || __start__ > __end__) return null;
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.ErrorMessage, __end__);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.Net.CommandServer.CancelKeepCallbackData value = default(AutoCSer.Net.CommandServer.CancelKeepCallbackData);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Net.CommandServer.CancelKeepCallbackData));
            }
    }
}namespace AutoCSer.Reflection
{
    public partial struct RemoteType
    {
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.Reflection.RemoteType value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(8))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.AssemblyName);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.Name);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.Reflection.RemoteType value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private unsafe byte* simpleDeserialize(byte* __start__, byte* __end__)
            {
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.AssemblyName, __end__);
                if (__start__ == null || __start__ > __end__) return null;
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.Name, __end__);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.Reflection.RemoteType value = default(AutoCSer.Reflection.RemoteType);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Reflection.RemoteType));
            }
    }
}namespace AutoCSer
{
    /// <summary>
    /// 触发 AOT 编译
    /// </summary>
    public static class AotMethod
    {
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            /// <returns></returns>
            public static bool Call()
            {
                if (AutoCSer.Date.StartTimestamp == long.MinValue)
                {
                    AutoCSer.Algorithm.IntegerDivision/**/.BinarySerialize();
                    AutoCSer.Net.CommandServer.CancelKeepCallbackData/**/.BinarySerialize();
                    AutoCSer.Reflection.RemoteType/**/.BinarySerialize();
                    AutoCSer.Net.CommandServer.CommandControllerOutputData/**/.BinarySerialize();
                    AutoCSer.Reflection.RemoteType/**/.JsonSerialize();
                    AutoCSer.SerializeComplex/**/.JsonSerialize();
                    AutoCSer.SerializeMatrix3x2/**/.JsonSerialize();
                    AutoCSer.SerializeMatrix4x4/**/.JsonSerialize();
                    AutoCSer.SerializePlane/**/.JsonSerialize();
                    AutoCSer.SerializeQuaternion/**/.JsonSerialize();
                    AutoCSer.SerializeVector2/**/.JsonSerialize();
                    AutoCSer.SerializeVector3/**/.JsonSerialize();
                    AutoCSer.SerializeVector4/**/.JsonSerialize();
                    AutoCSer.Algorithm.IntegerDivision/**/.SimpleSerialize();
                    AutoCSer.Net.CommandServer.CancelKeepCallbackData/**/.SimpleSerialize();
                    AutoCSer.Reflection.RemoteType/**/.SimpleSerialize();

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<int>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<string>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<string[]>));
                    AutoCSer.BinarySerializer.Array<string>(null, default(string[]));
                    binaryDeserializeMemberTypes();

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<string>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<double>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<float>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<AutoCSer.SerializeVector3>));


                    return true;
                }
                return false;
            }
            /// <summary>
            /// 二进制反序列化成员类型代码生成调用激活 AOT 反射
            /// </summary>
            private static void binaryDeserializeMemberTypes()
            {
                string[] t1 = default(string[]);
                AutoCSer.BinaryDeserializer.Array<string>(null, ref t1);
            }
    }
}
#endif