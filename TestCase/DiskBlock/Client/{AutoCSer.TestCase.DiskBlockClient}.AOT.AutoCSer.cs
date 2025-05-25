//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial class IntData
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.TestCase.DiskBlockClient.IntData value)
            {
                if (serializer.WriteMemberCountVerify(8, 1073741825)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(A);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.TestCase.DiskBlockClient.IntData value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.A);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 1, 1073741825);
                typeInfo.Add(typeof(int));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.TestCase.DiskBlockClient.IntData value = default(AutoCSer.TestCase.DiskBlockClient.IntData);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.DiskBlockClient.IntData));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.DiskBlockClient.IntData));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.DiskBlockClient.IntData>();
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial class IntData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.DiskBlockClient.IntData DefaultConstructor()
            {
                return new AutoCSer.TestCase.DiskBlockClient.IntData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.DiskBlockClient.IntData>();
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial class StringData
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.TestCase.DiskBlockClient.StringData value)
            {
                if (serializer.WriteMemberCountVerify(4, 1073741825)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(A);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.TestCase.DiskBlockClient.StringData value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.A);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 1, 1073741825);
                typeInfo.Add(typeof(string));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.TestCase.DiskBlockClient.StringData value = default(AutoCSer.TestCase.DiskBlockClient.StringData);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.DiskBlockClient.StringData));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.DiskBlockClient.StringData));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.DiskBlockClient.StringData>();
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial class StringData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.DiskBlockClient.StringData DefaultConstructor()
            {
                return new AutoCSer.TestCase.DiskBlockClient.StringData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.DiskBlockClient.StringData>();
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial class IntData
    {
            /// <summary>
            /// 对象对比
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            internal static bool FieldEquals(AutoCSer.TestCase.DiskBlockClient.IntData left, AutoCSer.TestCase.DiskBlockClient.IntData right)
            {
                return left.fieldEquals(right);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="__value__"></param>
            private bool fieldEquals(AutoCSer.TestCase.DiskBlockClient.IntData __value__)
            {
                if(!AutoCSer.FieldEquals.Comparor.EquatableEquals(A, __value__.A)) return false;
                return true;
            }
            /// <summary>
            /// 对象对比
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="memberMap"></param>
            /// <returns></returns>
            internal static bool MemberMapFieldEquals(AutoCSer.TestCase.DiskBlockClient.IntData left, AutoCSer.TestCase.DiskBlockClient.IntData right, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.IntData> memberMap)
            {
                return left.fieldEquals(right, memberMap);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="__value__"></param>
            /// <param name="__memberMap__"></param>
            private bool fieldEquals(AutoCSer.TestCase.DiskBlockClient.IntData __value__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.IntData> __memberMap__)
            {
                if (__memberMap__.IsMember(0) && !AutoCSer.FieldEquals.Comparor.EquatableEquals(A, __value__.A)) return false;
                return true;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void FieldEquals()
            {
                AutoCSer.TestCase.DiskBlockClient.IntData left = default(AutoCSer.TestCase.DiskBlockClient.IntData), right = default(AutoCSer.TestCase.DiskBlockClient.IntData);
                FieldEquals(left, right);
                MemberMapFieldEquals(left, right, null);
                AutoCSer.FieldEquals.Comparor.CallEquals<AutoCSer.TestCase.DiskBlockClient.IntData>(left, right);
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial class StringData
    {
            /// <summary>
            /// 对象对比
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            internal static bool FieldEquals(AutoCSer.TestCase.DiskBlockClient.StringData left, AutoCSer.TestCase.DiskBlockClient.StringData right)
            {
                return left.fieldEquals(right);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="__value__"></param>
            private bool fieldEquals(AutoCSer.TestCase.DiskBlockClient.StringData __value__)
            {
                if(!AutoCSer.FieldEquals.Comparor.ReferenceEquals(A, __value__.A)) return false;
                return true;
            }
            /// <summary>
            /// 对象对比
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="memberMap"></param>
            /// <returns></returns>
            internal static bool MemberMapFieldEquals(AutoCSer.TestCase.DiskBlockClient.StringData left, AutoCSer.TestCase.DiskBlockClient.StringData right, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.StringData> memberMap)
            {
                return left.fieldEquals(right, memberMap);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="__value__"></param>
            /// <param name="__memberMap__"></param>
            private bool fieldEquals(AutoCSer.TestCase.DiskBlockClient.StringData __value__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.StringData> __memberMap__)
            {
                if (__memberMap__.IsMember(0) && !AutoCSer.FieldEquals.Comparor.ReferenceEquals(A, __value__.A)) return false;
                return true;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void FieldEquals()
            {
                AutoCSer.TestCase.DiskBlockClient.StringData left = default(AutoCSer.TestCase.DiskBlockClient.StringData), right = default(AutoCSer.TestCase.DiskBlockClient.StringData);
                FieldEquals(left, right);
                MemberMapFieldEquals(left, right, null);
                AutoCSer.FieldEquals.Comparor.CallEquals<AutoCSer.TestCase.DiskBlockClient.StringData>(left, right);
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial struct ValueEquals
    {
            /// <summary>
            /// 对象对比
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            internal static bool FieldEquals(AutoCSer.TestCase.DiskBlockClient.ValueEquals left, AutoCSer.TestCase.DiskBlockClient.ValueEquals right)
            {
                return left.fieldEquals(right);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="__value__"></param>
            private bool fieldEquals(AutoCSer.TestCase.DiskBlockClient.ValueEquals __value__)
            {
                if(!AutoCSer.FieldEquals.Comparor.ArrayEquals<byte>(array, __value__.array)) return false;
                if(!AutoCSer.FieldEquals.Comparor.EquatableEquals(blockIndex, __value__.blockIndex)) return false;
                return true;
            }
            /// <summary>
            /// 对象对比
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="memberMap"></param>
            /// <returns></returns>
            internal static bool MemberMapFieldEquals(AutoCSer.TestCase.DiskBlockClient.ValueEquals left, AutoCSer.TestCase.DiskBlockClient.ValueEquals right, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.ValueEquals> memberMap)
            {
                return left.fieldEquals(right, memberMap);
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="__value__"></param>
            /// <param name="__memberMap__"></param>
            private bool fieldEquals(AutoCSer.TestCase.DiskBlockClient.ValueEquals __value__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.ValueEquals> __memberMap__)
            {
                if (__memberMap__.IsMember(0) && !AutoCSer.FieldEquals.Comparor.ArrayEquals<byte>(array, __value__.array)) return false;
                if (__memberMap__.IsMember(1) && !AutoCSer.FieldEquals.Comparor.EquatableEquals(blockIndex, __value__.blockIndex)) return false;
                return true;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void FieldEquals()
            {
                AutoCSer.TestCase.DiskBlockClient.ValueEquals left = default(AutoCSer.TestCase.DiskBlockClient.ValueEquals), right = default(AutoCSer.TestCase.DiskBlockClient.ValueEquals);
                FieldEquals(left, right);
                MemberMapFieldEquals(left, right, null);
                AutoCSer.FieldEquals.Comparor.CallEquals<AutoCSer.TestCase.DiskBlockClient.ValueEquals>(left, right);
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial class IntData
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.TestCase.DiskBlockClient.IntData value)
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
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.IntData> memberMap, JsonSerializer serializer, AutoCSer.TestCase.DiskBlockClient.IntData value, AutoCSer.Memory.CharStream stream)
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
                __stream__.SimpleWrite(@"""A"":");
                __serializer__.JsonSerialize(A);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.IntData> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""A"":");
                    __serializer__.JsonSerialize(A);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(int));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.DiskBlockClient.IntData value, ref AutoCSer.Memory.Pointer names)
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
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.DiskBlockClient.IntData value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.IntData> memberMap)
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
                    __deserializer__.JsonDeserialize(ref this.A);
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
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.IntData> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.A);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
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
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.TestCase.DiskBlockClient.IntData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.A);
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
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(1);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(1);
                names.Add(nameof(A));
                indexs.Add(0);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.TestCase.DiskBlockClient.IntData value = default(AutoCSer.TestCase.DiskBlockClient.IntData);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.DiskBlockClient.IntData));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.DiskBlockClient.IntData));
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
{
    internal partial class StringData
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.TestCase.DiskBlockClient.StringData value)
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
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.StringData> memberMap, JsonSerializer serializer, AutoCSer.TestCase.DiskBlockClient.StringData value, AutoCSer.Memory.CharStream stream)
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
                __stream__.SimpleWrite(@"""A"":");
                if (A == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerialize(A);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.StringData> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""A"":");
                    if (A == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerialize(A);
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
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.DiskBlockClient.StringData value, ref AutoCSer.Memory.Pointer names)
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
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.DiskBlockClient.StringData value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.StringData> memberMap)
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
                    __deserializer__.JsonDeserialize(ref this.A);
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
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.DiskBlockClient.StringData> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.A);
                    if (AutoCSer.JsonDeserializer.NextNameIndex(__deserializer__, ref __names__)) __memberMap__.SetMember(0);
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
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.TestCase.DiskBlockClient.StringData __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.A);
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
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(1);
                AutoCSer.LeftArray<int> indexs = new AutoCSer.LeftArray<int>(1);
                names.Add(nameof(A));
                indexs.Add(0);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.TestCase.DiskBlockClient.StringData value = default(AutoCSer.TestCase.DiskBlockClient.StringData);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.DiskBlockClient.StringData));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.DiskBlockClient.StringData));
            }
    }
}namespace AutoCSer.TestCase.DiskBlockClient
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
                    AutoCSer.TestCase.DiskBlockClient.IntData/**/.BinarySerialize();
                    AutoCSer.TestCase.DiskBlockClient.IntData/**/.DefaultConstructorReflection();
                    AutoCSer.TestCase.DiskBlockClient.StringData/**/.BinarySerialize();
                    AutoCSer.TestCase.DiskBlockClient.StringData/**/.DefaultConstructorReflection();
                    AutoCSer.TestCase.DiskBlockClient.IntData/**/.FieldEquals();
                    AutoCSer.TestCase.DiskBlockClient.StringData/**/.FieldEquals();
                    AutoCSer.TestCase.DiskBlockClient.ValueEquals/**/.FieldEquals();
                    AutoCSer.TestCase.DiskBlockClient.IntData/**/.JsonSerialize();
                    AutoCSer.TestCase.DiskBlockClient.StringData/**/.JsonSerialize();
                    AutoCSer.FieldEquals.Comparor.EquatableEquals<byte>(default(byte), default(byte));

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<int>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<string>));
                    binaryDeserializeMemberTypes();

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<int>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<string>));


                    return true;
                }
                return false;
            }
            /// <summary>
            /// 二进制反序列化成员类型代码生成调用激活 AOT 反射
            /// </summary>
            private static void binaryDeserializeMemberTypes()
            {
            }
    }
}
#endif