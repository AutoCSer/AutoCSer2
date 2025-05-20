//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.Common
{
    public partial class JsonFileConfig
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.TestCase.Common.JsonFileConfig value)
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
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.Common.JsonFileConfig> memberMap, JsonSerializer serializer, AutoCSer.TestCase.Common.JsonFileConfig value, AutoCSer.Memory.CharStream stream)
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
                __stream__.SimpleWrite(@"""ServerHost"":");
                if (ServerHost == null) __stream__.WriteJsonNull();
                else __serializer__.JsonSerialize(ServerHost);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.Common.JsonFileConfig> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""ServerHost"":");
                    if (ServerHost == null) __stream__.WriteJsonNull();
                    else __serializer__.JsonSerialize(ServerHost);
                }
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.Common.JsonFileConfig value, ref AutoCSer.Memory.Pointer names)
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
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.TestCase.Common.JsonFileConfig value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.Common.JsonFileConfig> memberMap)
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
                    __deserializer__.JsonDeserialize(ref this.ServerHost);
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
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.Common.JsonFileConfig> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.ServerHost);
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
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.TestCase.Common.JsonFileConfig __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.ServerHost);
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
                names.Add(nameof(ServerHost));
                indexs.Add(0);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
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
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.TestCase.Common.JsonFileConfig value = default(AutoCSer.TestCase.Common.JsonFileConfig);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.Common.JsonFileConfig));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.TestCase.Common.JsonFileConfig));
                JsonSerializeMemberTypes();
            }
    }
}namespace AutoCSer.TestCase.Common
{
    public partial class JsonFileConfig
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.TestCase.Common.JsonFileConfig DefaultConstructor()
            {
                return new AutoCSer.TestCase.Common.JsonFileConfig();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.TestCase.Common.JsonFileConfig>();
            }
    }
}namespace AutoCSer.TestCase.Common
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
                    AutoCSer.TestCase.Common.JsonFileConfig/**/.JsonSerialize();
                    AutoCSer.TestCase.Common.JsonFileConfig/**/.DefaultConstructorReflection();


                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<string>));


                    return true;
                }
                return false;
            }
    }
}
#endif