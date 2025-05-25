//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 创建节点索引信息
        /// </summary>
    public partial class CreateNodeIndex
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex value)
            {
                if (serializer.WriteMemberCountVerify(4, 1073741825)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.Simple(Index);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex value)
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
                __deserializer__.Simple(ref this.Index);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 1, 1073741825);
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex>();
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 创建节点索引信息
        /// </summary>
    public partial class CreateNodeIndex
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex DefaultConstructor()
            {
                return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex>();
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 自增 ID 分段
        /// </summary>
    public partial struct IdentityFragment
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment value)
            {
                serializer.Simple(value);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment value)
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
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment>();
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图数据
        /// </summary>
    public partial struct ManyHashBitMap
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap value)
            {
                if (serializer.WriteMemberCountVerify(4, 1073741826)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(Map);
                __serializer__.Simple(SizeDivision);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap value)
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
                __deserializer__.BinaryDeserialize(ref this.Map);
                __deserializer__.Simple(ref this.SizeDivision);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 2, 1073741826);
                typeInfo.Add(typeof(ulong[]));
                typeInfo.Add(typeof(AutoCSer.Algorithm.IntegerDivision));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap>();
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 正在处理的消息标识
        /// </summary>
    public partial struct MessageIdeneity
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value)
            {
                serializer.Simple(value);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value)
            {
                deserializer.Simple(ref value);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                return new AutoCSer.BinarySerialize.TypeInfo(true, 0, 1073741827);
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity>();
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 节点索引信息
        /// </summary>
    public partial struct NodeIndex
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex value)
            {
                serializer.Simple(value);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex value)
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
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>();
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务端节点信息
        /// </summary>
    public partial class NodeInfo
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo value)
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
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo> memberMap, JsonSerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo value, AutoCSer.Memory.CharStream stream)
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
                __stream__.SimpleWrite(@"""RemoteType"":");
                __serializer__.JsonSerializeType(RemoteType);
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void jsonSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo> __memberMap__, JsonSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                bool isNext = false;
                if (__memberMap__.IsMember(0))
                {
                    if (isNext) __stream__.Write(',');
                    else isNext = true;
                    __stream__.SimpleWrite(@"""RemoteType"":");
                    __serializer__.JsonSerializeType(RemoteType);
                }
            }
            /// <summary>
            /// 获取 JSON 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> JsonSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(AutoCSer.Reflection.RemoteType));
                return types;
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo value, ref AutoCSer.Memory.Pointer names)
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
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo> memberMap)
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
                    __deserializer__.JsonDeserialize(ref this.RemoteType);
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
            private void jsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.Memory.Pointer __names__, AutoCSer.Metadata.MemberMap<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo> __memberMap__)
            {
                if (__deserializer__.IsName(ref __names__))
                {
                    __deserializer__.JsonDeserialize(ref this.RemoteType);
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
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.JsonDeserialize(ref __value__.RemoteType);
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
                names.Add(nameof(RemoteType));
                indexs.Add(0);
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                JsonSerializeMemberTypes();
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo));
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 重建持久化文件调用结果
        /// </summary>
    public partial struct RebuildResult
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult value)
            {
                if (serializer.WriteMemberCountVerify(8, 1073741828)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.Stream.Write((byte)this.CallState);
                __serializer__.Stream.Write((byte)this.LoadExceptionNodeState);
                __serializer__.FixedFillSize(2);
                __serializer__.BinarySerialize(LoadExceptionNodeKey);
                __serializer__.Simple(LoadExceptionNodeType);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                this.CallState = (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum)__deserializer__.FixedEnumByte();
                this.LoadExceptionNodeState = (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum)__deserializer__.FixedEnumByte();
                __deserializer__.FixedFillSize(2);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.LoadExceptionNodeKey);
                __deserializer__.Simple(ref this.LoadExceptionNodeType);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 3, 1073741828);
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum));
                typeInfo.Add(typeof(string));
                typeInfo.Add(typeof(AutoCSer.Reflection.RemoteType));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult>();
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 正在处理的消息标识
        /// </summary>
    public partial struct MessageIdeneity
    {
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void JsonSerialize(AutoCSer.JsonSerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value)
            {
            }
            /// <summary>
            /// JSON 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void JsonSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity> memberMap, JsonSerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value, AutoCSer.Memory.CharStream stream)
            {
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value, ref AutoCSer.Memory.Pointer names)
            {
            }
            /// <summary>
            /// JSON 反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            /// <param name="names"></param>
            /// <param name="memberMap"></param>
            internal static void JsonDeserializeMemberMap(AutoCSer.JsonDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value, ref AutoCSer.Memory.Pointer names, AutoCSer.Metadata.MemberMap<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity> memberMap)
            {
            }
            /// <summary>
            /// 成员 JSON 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void JsonDeserialize(AutoCSer.JsonDeserializer __deserializer__, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity __value__, int __memberIndex__)
            {
            }
            /// <summary>
            /// 获取 JSON 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> JsonDeserializeMemberNames()
            {
                return default(AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void JsonSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity);
                JsonSerialize(null, value);
                JsonSerializeMemberMap(null, null, value, null);
                AutoCSer.Memory.Pointer names = default(AutoCSer.Memory.Pointer);
                JsonDeserialize(null, ref value, ref names);
                JsonDeserializeMemberMap(null, ref value, ref names, null);
                JsonDeserialize(null, ref value, 0);
                JsonDeserializeMemberNames();
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity));
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity));
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口 本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNode), typeof(BitmapNodeLocalClient))]
        public partial interface IBitmapNodeLocalClientNode
        {
            /// <summary>
            /// 清除位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ClearBit(uint index);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ClearMap();
            /// <summary>
            /// 读取位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBit(uint index);
            /// <summary>
            /// 清除位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitClearBit(uint index);
            /// <summary>
            /// 状态取反并返回操作之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitInvertBit(uint index);
            /// <summary>
            /// 设置位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitSetBit(uint index);
            /// <summary>
            /// 获取二进制位数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<uint>> GetCapacity();
            /// <summary>
            /// 状态取反
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> InvertBit(uint index);
            /// <summary>
            /// 设置位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> SetBit(uint index);
        }
        /// <summary>
        /// 位图节点接口 本地客户端节点
        /// </summary>
        internal unsafe partial class BitmapNodeLocalClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientNode<IBitmapNodeLocalClientNode>, IBitmapNodeLocalClientNode
        {
            /// <summary>
            /// 本地客户端节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <param name="creator">创建节点操作对象委托</param>
            /// <param name="client">日志流持久化内存数据库客户端</param>
            /// <param name="index">节点索引信息</param>
            /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
            private BitmapNodeLocalClient(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
                : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            internal static IBitmapNodeLocalClientNode LocalClientNodeConstructor(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new BitmapNodeLocalClient(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip0__
            {
                internal uint index;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(4))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.index);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ value, byte* end)
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
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.index);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip9__
            {
                internal byte[] map;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(4))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.map);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__ value, byte* end)
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
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.map, __end__);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__));
            }
            }
            /// <summary>
            /// 清除位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> IBitmapNodeLocalClientNode/**/.ClearBit(uint index)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<bool, __ip0__>(this, 0
                    , new __ip0__
                    {
                        index = index,
                    }
                    );
            }

            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> IBitmapNodeLocalClientNode/**/.ClearMap()
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallNode/**/.Create(this, 1
                    , true
                    );
            }

            /// <summary>
            /// 读取位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> IBitmapNodeLocalClientNode/**/.GetBit(uint index)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>, __ip0__>(this, 2
                    , new __ip0__
                    {
                        index = index,
                    }
                    );
            }

            /// <summary>
            /// 清除位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> IBitmapNodeLocalClientNode/**/.GetBitClearBit(uint index)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>, __ip0__>(this, 3
                    , new __ip0__
                    {
                        index = index,
                    }
                    );
            }

            /// <summary>
            /// 状态取反并返回操作之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> IBitmapNodeLocalClientNode/**/.GetBitInvertBit(uint index)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>, __ip0__>(this, 4
                    , new __ip0__
                    {
                        index = index,
                    }
                    );
            }

            /// <summary>
            /// 设置位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> IBitmapNodeLocalClientNode/**/.GetBitSetBit(uint index)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>, __ip0__>(this, 5
                    , new __ip0__
                    {
                        index = index,
                    }
                    );
            }

            /// <summary>
            /// 获取二进制位数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<uint>> IBitmapNodeLocalClientNode/**/.GetCapacity()
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallOutputNode<uint>/**/.Create(this, 6
                    , true
                    );
            }

            /// <summary>
            /// 状态取反
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> IBitmapNodeLocalClientNode/**/.InvertBit(uint index)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<bool, __ip0__>(this, 7
                    , new __ip0__
                    {
                        index = index,
                    }
                    );
            }

            /// <summary>
            /// 设置位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> IBitmapNodeLocalClientNode/**/.SetBit(uint index)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<bool, __ip0__>(this, 8
                    , new __ip0__
                    {
                        index = index,
                    }
                    );
            }

            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void LocalClientNode()
            {
                LocalClientNodeConstructor(null, null, null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex), false);
                AutoCSer.AotReflection.NonPublicFields(typeof(IBitmapNodeMethodEnum));
                AutoCSer.AotReflection.NonPublicMethods(typeof(BitmapNodeLocalClient));
                AutoCSer.AotReflection.Interfaces(typeof(BitmapNodeLocalClient));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 64 位自增ID 节点接口 本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNode), typeof(IdentityGeneratorNodeLocalClient))]
        public partial interface IIdentityGeneratorNodeLocalClientNode
        {
            /// <summary>
            /// 获取下一个自增ID
            /// </summary>
            /// <returns>下一个自增ID，失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<long>> Next();
            /// <summary>
            /// 获取自增 ID 分段
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>自增 ID 分段</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment>> NextFragment(int count);
        }
        /// <summary>
        /// 64 位自增ID 节点接口 本地客户端节点
        /// </summary>
        internal unsafe partial class IdentityGeneratorNodeLocalClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientNode<IIdentityGeneratorNodeLocalClientNode>, IIdentityGeneratorNodeLocalClientNode
        {
            /// <summary>
            /// 本地客户端节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <param name="creator">创建节点操作对象委托</param>
            /// <param name="client">日志流持久化内存数据库客户端</param>
            /// <param name="index">节点索引信息</param>
            /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
            private IdentityGeneratorNodeLocalClient(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
                : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            internal static IIdentityGeneratorNodeLocalClientNode LocalClientNodeConstructor(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new IdentityGeneratorNodeLocalClient(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip1__
            {
                internal int count;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(4))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.count);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__ value, byte* end)
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
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.count);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip2__
            {
                internal long identity;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__ value)
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
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.identity);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__ value, byte* end)
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
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.identity);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__));
            }
            }
            /// <summary>
            /// 获取下一个自增ID
            /// </summary>
            /// <returns>下一个自增ID，失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<long>> IIdentityGeneratorNodeLocalClientNode/**/.Next()
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallOutputNode<long>/**/.Create(this, 0
                    , true
                    );
            }

            /// <summary>
            /// 获取自增 ID 分段
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>自增 ID 分段</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment>> IIdentityGeneratorNodeLocalClientNode/**/.NextFragment(int count)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment, __ip1__>(this, 1
                    , new __ip1__
                    {
                        count = count,
                    }
                    );
            }

            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void LocalClientNode()
            {
                LocalClientNodeConstructor(null, null, null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex), false);
                AutoCSer.AotReflection.NonPublicFields(typeof(IIdentityGeneratorNodeMethodEnum));
                AutoCSer.AotReflection.NonPublicMethods(typeof(IdentityGeneratorNodeLocalClient));
                AutoCSer.AotReflection.Interfaces(typeof(IdentityGeneratorNodeLocalClient));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器） 本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IManyHashBitMapClientFilterNode), typeof(ManyHashBitMapClientFilterNodeLocalClient))]
        public partial interface IManyHashBitMapClientFilterNodeLocalClientNode
        {
            /// <summary>
            /// 获取设置新位操作
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<System.IDisposable> GetBit(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> __callback__);
            /// <summary>
            /// 获取当前位图数据
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap>> GetData();
            /// <summary>
            /// 获取位图大小（位数量）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetSize();
            /// <summary>
            /// 设置位
            /// </summary>
            /// <param name="bit">位置</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SetBit(int bit);
        }
        /// <summary>
        /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器） 本地客户端节点
        /// </summary>
        internal unsafe partial class ManyHashBitMapClientFilterNodeLocalClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientNode<IManyHashBitMapClientFilterNodeLocalClientNode>, IManyHashBitMapClientFilterNodeLocalClientNode
        {
            /// <summary>
            /// 本地客户端节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <param name="creator">创建节点操作对象委托</param>
            /// <param name="client">日志流持久化内存数据库客户端</param>
            /// <param name="index">节点索引信息</param>
            /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
            private ManyHashBitMapClientFilterNodeLocalClient(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
                : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            internal static IManyHashBitMapClientFilterNodeLocalClientNode LocalClientNodeConstructor(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new ManyHashBitMapClientFilterNodeLocalClient(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip3__
            {
                internal int bit;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(4))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.bit);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__ value, byte* end)
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
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.bit);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip5__
            {
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap map;
                
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__ value)
            {
                if (serializer.WriteMemberCountVerify(4, 1073741825)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(map);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__ value)
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
                __deserializer__.BinaryDeserialize(ref this.map);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 1, 1073741825);
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__>();
            }
            }
            /// <summary>
            /// 获取设置新位操作
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<System.IDisposable> IManyHashBitMapClientFilterNodeLocalClientNode/**/.GetBit(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> __callback__)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceKeepCallbackNode<int>/**/.Create(this, 0
                    , __callback__
                    , false
                    );
            }

            /// <summary>
            /// 获取当前位图数据
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap>> IManyHashBitMapClientFilterNodeLocalClientNode/**/.GetData()
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallOutputNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap>/**/.Create(this, 1
                    , true
                    );
            }

            /// <summary>
            /// 获取位图大小（位数量）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IManyHashBitMapClientFilterNodeLocalClientNode/**/.GetSize()
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallOutputNode<int>/**/.Create(this, 2
                    , true
                    );
            }

            /// <summary>
            /// 设置位
            /// </summary>
            /// <param name="bit">位置</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> IManyHashBitMapClientFilterNodeLocalClientNode/**/.SetBit(int bit)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputNode/**/.Create(this, 3
                    , new __ip3__
                    {
                        bit = bit,
                    }
                    );
            }

            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void LocalClientNode()
            {
                LocalClientNodeConstructor(null, null, null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex), false);
                AutoCSer.AotReflection.NonPublicFields(typeof(IManyHashBitMapClientFilterNodeMethodEnum));
                AutoCSer.AotReflection.NonPublicMethods(typeof(ManyHashBitMapClientFilterNodeLocalClient));
                AutoCSer.AotReflection.Interfaces(typeof(ManyHashBitMapClientFilterNodeLocalClient));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图过滤节点接口（类似布隆过滤器） 本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IManyHashBitMapFilterNode), typeof(ManyHashBitMapFilterNodeLocalClient))]
        public partial interface IManyHashBitMapFilterNodeLocalClientNode
        {
            /// <summary>
            /// 检查位图数据
            /// </summary>
            /// <param name="size">位图大小（位数量）</param>
            /// <param name="bits">位置集合</param>
            /// <returns>返回 false 表示数据不存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.NullableBoolEnum>> CheckBits(int size, uint[] bits);
            /// <summary>
            /// 获取位图大小（位数量）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetSize();
            /// <summary>
            /// 设置位
            /// </summary>
            /// <param name="size">位图大小（位数量）</param>
            /// <param name="bits">位置集合</param>
            /// <returns>返回 false 表示位图大小不匹配</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> SetBits(int size, uint[] bits);
        }
        /// <summary>
        /// 多哈希位图过滤节点接口（类似布隆过滤器） 本地客户端节点
        /// </summary>
        internal unsafe partial class ManyHashBitMapFilterNodeLocalClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientNode<IManyHashBitMapFilterNodeLocalClientNode>, IManyHashBitMapFilterNodeLocalClientNode
        {
            /// <summary>
            /// 本地客户端节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <param name="creator">创建节点操作对象委托</param>
            /// <param name="client">日志流持久化内存数据库客户端</param>
            /// <param name="index">节点索引信息</param>
            /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
            private ManyHashBitMapFilterNodeLocalClient(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
                : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            internal static IManyHashBitMapFilterNodeLocalClientNode LocalClientNodeConstructor(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new ManyHashBitMapFilterNodeLocalClient(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip0__
            {
                internal int size;
                internal uint[] bits;
                
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__ value)
            {
                if (serializer.WriteMemberCountVerify(8, 1073741826)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(size);
                __serializer__.BinarySerialize(bits);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__ value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.size);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.bits);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 2, 1073741826);
                typeInfo.Add(typeof(int));
                typeInfo.Add(typeof(uint[]));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>();
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip4__
            {
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap map;
                
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__ value)
            {
                if (serializer.WriteMemberCountVerify(4, 1073741825)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(map);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__ value)
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
                __deserializer__.BinaryDeserialize(ref this.map);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 1, 1073741825);
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__>();
            }
            }
            /// <summary>
            /// 检查位图数据
            /// </summary>
            /// <param name="size">位图大小（位数量）</param>
            /// <param name="bits">位置集合</param>
            /// <returns>返回 false 表示数据不存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.NullableBoolEnum>> IManyHashBitMapFilterNodeLocalClientNode/**/.CheckBits(int size, uint[] bits)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.NullableBoolEnum, __ip0__>(this, 0
                    , new __ip0__
                    {
                        size = size,
                        bits = bits,
                    }
                    );
            }

            /// <summary>
            /// 获取位图大小（位数量）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IManyHashBitMapFilterNodeLocalClientNode/**/.GetSize()
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallOutputNode<int>/**/.Create(this, 1
                    , true
                    );
            }

            /// <summary>
            /// 设置位
            /// </summary>
            /// <param name="size">位图大小（位数量）</param>
            /// <param name="bits">位置集合</param>
            /// <returns>返回 false 表示位图大小不匹配</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> IManyHashBitMapFilterNodeLocalClientNode/**/.SetBits(int size, uint[] bits)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<bool, __ip0__>(this, 2
                    , new __ip0__
                    {
                        size = size,
                        bits = bits,
                    }
                    );
            }

            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void LocalClientNode()
            {
                LocalClientNodeConstructor(null, null, null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex), false);
                AutoCSer.AotReflection.NonPublicFields(typeof(IManyHashBitMapFilterNodeMethodEnum));
                AutoCSer.AotReflection.NonPublicMethods(typeof(ManyHashBitMapFilterNodeLocalClient));
                AutoCSer.AotReflection.Interfaces(typeof(ManyHashBitMapFilterNodeLocalClient));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举 本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode), typeof(ServiceNodeLocalClient))]
        public partial interface IServiceNodeLocalClientNode
        {
            /// <summary>
            /// 创建位图节点 BitmapNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">二进制位数量</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateBitmapNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, uint capacity);
            /// <summary>
            /// 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="identity">起始分配 ID</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateIdentityGeneratorNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, long identity);
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> RemoveNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index);
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> RemoveNodeByKey(string key);
            /// <summary>
            /// 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="size">位图大小（位数量）</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateManyHashBitMapClientFilterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int size);
            /// <summary>
            /// 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="size">位图大小（位数量）</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateManyHashBitMapFilterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int size);
        }
        /// <summary>
        /// 服务基础操作接口方法映射枚举 本地客户端节点
        /// </summary>
        internal unsafe partial class ServiceNodeLocalClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientNode<IServiceNodeLocalClientNode>, IServiceNodeLocalClientNode
        {
            /// <summary>
            /// 本地客户端节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <param name="creator">创建节点操作对象委托</param>
            /// <param name="client">日志流持久化内存数据库客户端</param>
            /// <param name="index">节点索引信息</param>
            /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
            private ServiceNodeLocalClient(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
                : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            internal static IServiceNodeLocalClientNode LocalClientNodeConstructor(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new ServiceNodeLocalClient(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip0__
            {
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index;
                internal string key;
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo;
                internal uint capacity;
                
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__ value)
            {
                if (serializer.WriteMemberCountVerify(8, 1073741828)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(capacity);
                __serializer__.Simple(index);
                __serializer__.BinarySerialize(key);
                __serializer__.Json(nodeInfo);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__ value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.capacity);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.Simple(ref this.index);
                __deserializer__.BinaryDeserialize(ref this.key);
                __deserializer__.Json(ref this.nodeInfo);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 4, 1073741828);
                typeInfo.Add(typeof(uint));
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
                typeInfo.Add(typeof(string));
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__>();
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip1__
            {
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index;
                internal string key;
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo;
                internal long identity;
                
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__ value)
            {
                if (serializer.WriteMemberCountVerify(12, 1073741828)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(identity);
                __serializer__.Simple(index);
                __serializer__.BinarySerialize(key);
                __serializer__.Json(nodeInfo);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__ value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.identity);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.Simple(ref this.index);
                __deserializer__.BinaryDeserialize(ref this.key);
                __deserializer__.Json(ref this.nodeInfo);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 4, 1073741828);
                typeInfo.Add(typeof(long));
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
                typeInfo.Add(typeof(string));
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__>();
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip2__
            {
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index;
                
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__ value)
            {
                if (serializer.WriteMemberCountVerify(4, 1073741825)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.Simple(index);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__ value)
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
                __deserializer__.Simple(ref this.index);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 1, 1073741825);
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__>();
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip3__
            {
                internal string key;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(4))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.key);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__ value, byte* end)
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
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.key, __end__);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip4__
            {
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index;
                internal string key;
                internal AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo;
                internal int size;
                
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__ value)
            {
                if (serializer.WriteMemberCountVerify(8, 1073741828)) value.binarySerialize(serializer);
            }
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void binarySerialize(AutoCSer.BinarySerializer __serializer__)
            {
                __serializer__.BinarySerialize(size);
                __serializer__.Simple(index);
                __serializer__.BinarySerialize(key);
                __serializer__.Json(nodeInfo);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__ value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.size);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.Simple(ref this.index);
                __deserializer__.BinaryDeserialize(ref this.key);
                __deserializer__.Json(ref this.nodeInfo);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 4, 1073741828);
                typeInfo.Add(typeof(int));
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
                typeInfo.Add(typeof(string));
                typeInfo.Add(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo));
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__ value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__>();
            }
            }
            /// <summary>
            /// 创建位图节点 BitmapNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">二进制位数量</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> IServiceNodeLocalClientNode/**/.CreateBitmapNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, uint capacity)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, __ip0__>(this, 0
                    , new __ip0__
                    {
                        index = index,
                        key = key,
                        nodeInfo = nodeInfo,
                        capacity = capacity,
                    }
                    );
            }

            /// <summary>
            /// 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="identity">起始分配 ID</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> IServiceNodeLocalClientNode/**/.CreateIdentityGeneratorNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, long identity)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, __ip1__>(this, 1
                    , new __ip1__
                    {
                        index = index,
                        key = key,
                        nodeInfo = nodeInfo,
                        identity = identity,
                    }
                    );
            }

            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> IServiceNodeLocalClientNode/**/.RemoveNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<bool, __ip2__>(this, 2
                    , new __ip2__
                    {
                        index = index,
                    }
                    );
            }

            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> IServiceNodeLocalClientNode/**/.RemoveNodeByKey(string key)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<bool, __ip3__>(this, 3
                    , new __ip3__
                    {
                        key = key,
                    }
                    );
            }

            /// <summary>
            /// 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="size">位图大小（位数量）</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> IServiceNodeLocalClientNode/**/.CreateManyHashBitMapClientFilterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int size)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, __ip4__>(this, 4
                    , new __ip4__
                    {
                        index = index,
                        key = key,
                        nodeInfo = nodeInfo,
                        size = size,
                    }
                    );
            }

            /// <summary>
            /// 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="size">位图大小（位数量）</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> IServiceNodeLocalClientNode/**/.CreateManyHashBitMapFilterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int size)
            {
                
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceCallInputOutputNode/**/.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, __ip4__>(this, 5
                    , new __ip4__
                    {
                        index = index,
                        key = key,
                        nodeInfo = nodeInfo,
                        size = size,
                    }
                    );
            }

            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void LocalClientNode()
            {
                LocalClientNodeConstructor(null, null, null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex), false);
                AutoCSer.AotReflection.NonPublicFields(typeof(IServiceNodeMethodEnum));
                AutoCSer.AotReflection.NonPublicMethods(typeof(ServiceNodeLocalClient));
                AutoCSer.AotReflection.Interfaces(typeof(ServiceNodeLocalClient));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(IBitmapNodeMethodEnum), typeof(BitmapNodeMethodParameterCreator))]
        public partial interface IBitmapNode { }
        /// <summary>
        /// 位图节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IBitmapNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除位状态
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            ClearBit = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            ClearMap = 1,
            /// <summary>
            /// [2] 读取位状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 非 0 表示二进制位为已设置状态，索引超出则无返回值
            /// </summary>
            GetBit = 2,
            /// <summary>
            /// [3] 清除位状态并返回设置之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitClearBit = 3,
            /// <summary>
            /// [4] 状态取反并返回操作之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitInvertBit = 4,
            /// <summary>
            /// [5] 设置位状态并返回设置之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitSetBit = 5,
            /// <summary>
            /// [6] 获取二进制位数量
            /// 返回值 uint 
            /// </summary>
            GetCapacity = 6,
            /// <summary>
            /// [7] 状态取反
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            InvertBit = 7,
            /// <summary>
            /// [8] 设置位状态
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            SetBit = 8,
            /// <summary>
            /// [9] 快照添加数据
            /// byte[] map 
            /// </summary>
            SnapshotSet = 9,
        }
        /// <summary>
        /// 清除位状态 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_ClearBit_0 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>
        {
            internal IBitmapNode_ClearBit_0() : base(0, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)31) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IBitmapNode>(methodParameter).ClearBit(parameter.index));
            }
        }
        /// <summary>
        /// 清除所有数据 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_ClearMap_1 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallMethod
        {
            internal IBitmapNode_ClearMap_1() : base(1, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)3) { }
            public override void Call(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IBitmapNode>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IBitmapNode>)node).ClearMap();
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallMethod.Callback(ref callback);
            }
        }
        /// <summary>
        /// 读取位状态 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_GetBit_2 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>
        {
            internal IBitmapNode_GetBit_2() : base(2, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)10) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IBitmapNode>(methodParameter).GetBit(parameter.index));
            }
        }
        /// <summary>
        /// 清除位状态并返回设置之前的状态 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_GetBitClearBit_3 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>
        {
            internal IBitmapNode_GetBitClearBit_3() : base(3, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)27) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IBitmapNode>(methodParameter).GetBitClearBit(parameter.index));
            }
        }
        /// <summary>
        /// 状态取反并返回操作之前的状态 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_GetBitInvertBit_4 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>
        {
            internal IBitmapNode_GetBitInvertBit_4() : base(4, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)27) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IBitmapNode>(methodParameter).GetBitInvertBit(parameter.index));
            }
        }
        /// <summary>
        /// 设置位状态并返回设置之前的状态 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_GetBitSetBit_5 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>
        {
            internal IBitmapNode_GetBitSetBit_5() : base(5, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)27) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IBitmapNode>(methodParameter).GetBitSetBit(parameter.index));
            }
        }
        /// <summary>
        /// 获取二进制位数量 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_GetCapacity_6 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod
        {
            internal IBitmapNode_GetCapacity_6() : base(6, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)7) { }
            public override void CallOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.Callback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IBitmapNode>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IBitmapNode>)node).GetCapacity(), ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)7);
            }
        }
        /// <summary>
        /// 状态取反 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_InvertBit_7 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>
        {
            internal IBitmapNode_InvertBit_7() : base(7, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)31) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IBitmapNode>(methodParameter).InvertBit(parameter.index));
            }
        }
        /// <summary>
        /// 设置位状态 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_SetBit_8 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>
        {
            internal IBitmapNode_SetBit_8() : base(8, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)31) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IBitmapNode>(methodParameter).SetBit(parameter.index));
            }
        }
        /// <summary>
        /// 快照添加数据 服务端节点方法
        /// </summary>
        internal sealed class IBitmapNode_SnapshotSet_9 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__>
        {
            internal IBitmapNode_SnapshotSet_9() : base(9, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)9) { }
            public override void CallInput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IBitmapNode>(methodParameter).SnapshotSet(parameter.map);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter.Callback(methodParameter);
            }
        }
        /// <summary>
        /// 位图节点接口 创建调用方法与参数信息
        /// </summary>
        internal sealed partial class BitmapNodeMethodParameterCreator
        {
            private static void SnapshotSet_SnapshotSerialize(AutoCSer.BinarySerializer serializer, byte[] value)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__ snapshotMethodParameter = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__ { map = value };
                serializer.SimpleSerialize(ref snapshotMethodParameter);
            }
            /// <summary>
            /// 获取生成服务端节点方法信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod GetServerNodeCreatorMethod()
            {
                return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod(new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Method[]
                    {
                        new IBitmapNode_ClearBit_0(),
                        new IBitmapNode_ClearMap_1(),
                        new IBitmapNode_GetBit_2(),
                        new IBitmapNode_GetBitClearBit_3(),
                        new IBitmapNode_GetBitInvertBit_4(),
                        new IBitmapNode_GetBitSetBit_5(),
                        new IBitmapNode_GetCapacity_6(),
                        new IBitmapNode_InvertBit_7(),
                        new IBitmapNode_SetBit_8(),
                        new IBitmapNode_SnapshotSet_9(),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo(9, typeof(byte[]), SnapshotSet_SnapshotSerialize),
                    });
            }
            internal static void MethodParameterCreator()
            {
                GetServerNodeCreatorMethod();
                AutoCSer.AotReflection.NonPublicMethods(typeof(BitmapNodeMethodParameterCreator));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 64 位自增ID 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(IIdentityGeneratorNodeMethodEnum), typeof(IdentityGeneratorNodeMethodParameterCreator))]
        public partial interface IIdentityGeneratorNode { }
        /// <summary>
        /// 64 位自增ID 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IIdentityGeneratorNodeMethodEnum
        {
            /// <summary>
            /// [0] 获取下一个自增ID
            /// 返回值 long 下一个自增ID，失败返回负数
            /// </summary>
            Next = 0,
            /// <summary>
            /// [1] 获取自增 ID 分段
            /// int count 获取数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment 自增 ID 分段
            /// </summary>
            NextFragment = 1,
            /// <summary>
            /// [2] 快照添加数据
            /// long identity 
            /// </summary>
            SnapshotSet = 2,
        }
        /// <summary>
        /// 获取下一个自增ID 服务端节点方法
        /// </summary>
        internal sealed class IIdentityGeneratorNode_Next_0 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod
        {
            internal IIdentityGeneratorNode_Next_0() : base(0, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)7) { }
            public override void CallOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.Callback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IIdentityGeneratorNode>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IIdentityGeneratorNode>)node).Next(), ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)7);
            }
        }
        /// <summary>
        /// 获取自增 ID 分段 服务端节点方法
        /// </summary>
        internal sealed class IIdentityGeneratorNode_NextFragment_1 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__>
        {
            internal IIdentityGeneratorNode_NextFragment_1() : base(1, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)11) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IIdentityGeneratorNode>(methodParameter).NextFragment(parameter.count));
            }
        }
        /// <summary>
        /// 快照添加数据 服务端节点方法
        /// </summary>
        internal sealed class IIdentityGeneratorNode_SnapshotSet_2 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__>
        {
            internal IIdentityGeneratorNode_SnapshotSet_2() : base(2, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)9) { }
            public override void CallInput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IIdentityGeneratorNode>(methodParameter).SnapshotSet(parameter.identity);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter.Callback(methodParameter);
            }
        }
        /// <summary>
        /// 64 位自增ID 节点接口 创建调用方法与参数信息
        /// </summary>
        internal sealed partial class IdentityGeneratorNodeMethodParameterCreator
        {
            private static void SnapshotSet_SnapshotSerialize(AutoCSer.BinarySerializer serializer, long value)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__ snapshotMethodParameter = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__ { identity = value };
                serializer.SimpleSerialize(ref snapshotMethodParameter);
            }
            /// <summary>
            /// 获取生成服务端节点方法信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod GetServerNodeCreatorMethod()
            {
                return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod(new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Method[]
                    {
                        new IIdentityGeneratorNode_Next_0(),
                        new IIdentityGeneratorNode_NextFragment_1(),
                        new IIdentityGeneratorNode_SnapshotSet_2(),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo(2, typeof(long), SnapshotSet_SnapshotSerialize),
                    });
            }
            internal static void MethodParameterCreator()
            {
                GetServerNodeCreatorMethod();
                AutoCSer.AotReflection.NonPublicMethods(typeof(IdentityGeneratorNodeMethodParameterCreator));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(IManyHashBitMapClientFilterNodeMethodEnum), typeof(ManyHashBitMapClientFilterNodeMethodParameterCreator))]
        public partial interface IManyHashBitMapClientFilterNode { }
        /// <summary>
        /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器） 节点方法序号映射枚举类型
        /// </summary>
        public enum IManyHashBitMapClientFilterNodeMethodEnum
        {
            /// <summary>
            /// [0] 获取设置新位操作
            /// </summary>
            GetBit = 0,
            /// <summary>
            /// [1] 获取当前位图数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap 
            /// </summary>
            GetData = 1,
            /// <summary>
            /// [2] 获取位图大小（位数量）
            /// 返回值 int 
            /// </summary>
            GetSize = 2,
            /// <summary>
            /// [3] 设置位
            /// int bit 位置
            /// </summary>
            SetBit = 3,
            /// <summary>
            /// [4] 设置位 持久化前检查
            /// int bit 位置
            /// 返回值 bool 是否继续持久化操作
            /// </summary>
            SetBitBeforePersistence = 4,
            /// <summary>
            /// [5] 快照添加数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap map 多哈希位图数据
            /// </summary>
            SnapshotSet = 5,
        }
        /// <summary>
        /// 获取设置新位操作 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapClientFilterNode_GetBit_0 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackMethod
        {
            internal IManyHashBitMapClientFilterNode_GetBit_0() : base(0, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)134) { }
            public override void KeepCallback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerKeepCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IManyHashBitMapClientFilterNode>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IManyHashBitMapClientFilterNode>)node).GetBit(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodKeepCallback<int>.Create(ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)134));
            }
        }
        /// <summary>
        /// 获取当前位图数据 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapClientFilterNode_GetData_1 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod
        {
            internal IManyHashBitMapClientFilterNode_GetData_1() : base(1, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)2) { }
            public override void CallOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.Callback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IManyHashBitMapClientFilterNode>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IManyHashBitMapClientFilterNode>)node).GetData(), ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)2);
            }
        }
        /// <summary>
        /// 获取位图大小（位数量） 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapClientFilterNode_GetSize_2 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod
        {
            internal IManyHashBitMapClientFilterNode_GetSize_2() : base(2, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)6) { }
            public override void CallOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.Callback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IManyHashBitMapClientFilterNode>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IManyHashBitMapClientFilterNode>)node).GetSize(), ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)6);
            }
        }
        /// <summary>
        /// 设置位 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapClientFilterNode_SetBit_3 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__>
        {
            internal IManyHashBitMapClientFilterNode_SetBit_3() : base(3, 4, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)27) { }
            public override void CallInput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IManyHashBitMapClientFilterNode>(methodParameter).SetBit(parameter.bit);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter.Callback(methodParameter);
            }
        }
        /// <summary>
        /// 设置位 持久化前检查 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapClientFilterNode_SetBitBeforePersistence_4 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__>
        {
            internal IManyHashBitMapClientFilterNode_SetBitBeforePersistence_4() : base(4, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)24) { }
            public override bool CallBeforePersistence(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__>)methodParameter);
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IManyHashBitMapClientFilterNode>(methodParameter).SetBitBeforePersistence(parameter.bit);
            }
        }
        /// <summary>
        /// 快照添加数据 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapClientFilterNode_SnapshotSet_5 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__>
        {
            internal IManyHashBitMapClientFilterNode_SnapshotSet_5() : base(5, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)1) { }
            public override void CallInput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IManyHashBitMapClientFilterNode>(methodParameter).SnapshotSet(parameter.map);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter.Callback(methodParameter);
            }
        }
        /// <summary>
        /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器） 创建调用方法与参数信息
        /// </summary>
        internal sealed partial class ManyHashBitMapClientFilterNodeMethodParameterCreator
        {
            private static void SnapshotSet_SnapshotSerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap value)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__ snapshotMethodParameter = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__ { map = value };
                serializer.InternalIndependentSerializeNotNull(ref snapshotMethodParameter);
            }
            /// <summary>
            /// 获取生成服务端节点方法信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod GetServerNodeCreatorMethod()
            {
                return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod(new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Method[]
                    {
                        new IManyHashBitMapClientFilterNode_GetBit_0(),
                        new IManyHashBitMapClientFilterNode_GetData_1(),
                        new IManyHashBitMapClientFilterNode_GetSize_2(),
                        new IManyHashBitMapClientFilterNode_SetBit_3(),
                        new IManyHashBitMapClientFilterNode_SetBitBeforePersistence_4(),
                        new IManyHashBitMapClientFilterNode_SnapshotSet_5(),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo(5, typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap), SnapshotSet_SnapshotSerialize),
                    });
            }
            internal static void MethodParameterCreator()
            {
                GetServerNodeCreatorMethod();
                AutoCSer.AotReflection.NonPublicMethods(typeof(ManyHashBitMapClientFilterNodeMethodParameterCreator));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图过滤节点接口（类似布隆过滤器）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(IManyHashBitMapFilterNodeMethodEnum), typeof(ManyHashBitMapFilterNodeMethodParameterCreator))]
        public partial interface IManyHashBitMapFilterNode { }
        /// <summary>
        /// 多哈希位图过滤节点接口（类似布隆过滤器） 节点方法序号映射枚举类型
        /// </summary>
        public enum IManyHashBitMapFilterNodeMethodEnum
        {
            /// <summary>
            /// [0] 检查位图数据
            /// int size 位图大小（位数量）
            /// uint[] bits 位置集合
            /// 返回值 AutoCSer.NullableBoolEnum 返回 false 表示数据不存在
            /// </summary>
            CheckBits = 0,
            /// <summary>
            /// [1] 获取位图大小（位数量）
            /// 返回值 int 
            /// </summary>
            GetSize = 1,
            /// <summary>
            /// [2] 设置位
            /// int size 位图大小（位数量）
            /// uint[] bits 位置集合
            /// 返回值 bool 返回 false 表示位图大小不匹配
            /// </summary>
            SetBits = 2,
            /// <summary>
            /// [3] 设置位 持久化前检查
            /// int size 位图大小（位数量）
            /// uint[] bits 位置集合
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 返回 false 表示位图大小不匹配
            /// </summary>
            SetBitsBeforePersistence = 3,
            /// <summary>
            /// [4] 快照添加数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap map 多哈希位图数据
            /// </summary>
            SnapshotSet = 4,
        }
        /// <summary>
        /// 检查位图数据 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapFilterNode_CheckBits_0 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>
        {
            internal IManyHashBitMapFilterNode_CheckBits_0() : base(0, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)6) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IManyHashBitMapFilterNode>(methodParameter).CheckBits(parameter.size, parameter.bits));
            }
        }
        /// <summary>
        /// 获取位图大小（位数量） 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapFilterNode_GetSize_1 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod
        {
            internal IManyHashBitMapFilterNode_GetSize_1() : base(1, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)6) { }
            public override void CallOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.Callback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IManyHashBitMapFilterNode>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<IManyHashBitMapFilterNode>)node).GetSize(), ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)6);
            }
        }
        /// <summary>
        /// 设置位 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapFilterNode_SetBits_2 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>
        {
            internal IManyHashBitMapFilterNode_SetBits_2() : base(2, 3, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)23) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IManyHashBitMapFilterNode>(methodParameter).SetBits(parameter.size, parameter.bits));
            }
        }
        /// <summary>
        /// 设置位 持久化前检查 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapFilterNode_SetBitsBeforePersistence_3 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>
        {
            internal IManyHashBitMapFilterNode_SetBitsBeforePersistence_3() : base(3, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)20) { }
            public override AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> CallOutputBeforePersistence(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__>)methodParameter);
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.GetBeforePersistenceResponseParameter(methodParameter,AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IManyHashBitMapFilterNode>(methodParameter).SetBitsBeforePersistence(parameter.size, parameter.bits));

            }
        }
        /// <summary>
        /// 快照添加数据 服务端节点方法
        /// </summary>
        internal sealed class IManyHashBitMapFilterNode_SnapshotSet_4 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__>
        {
            internal IManyHashBitMapFilterNode_SnapshotSet_4() : base(4, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)1) { }
            public override void CallInput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IManyHashBitMapFilterNode>(methodParameter).SnapshotSet(parameter.map);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter.Callback(methodParameter);
            }
        }
        /// <summary>
        /// 多哈希位图过滤节点接口（类似布隆过滤器） 创建调用方法与参数信息
        /// </summary>
        internal sealed partial class ManyHashBitMapFilterNodeMethodParameterCreator
        {
            private static void SnapshotSet_SnapshotSerialize(AutoCSer.BinarySerializer serializer, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap value)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__ snapshotMethodParameter = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__ { map = value };
                serializer.InternalIndependentSerializeNotNull(ref snapshotMethodParameter);
            }
            /// <summary>
            /// 获取生成服务端节点方法信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod GetServerNodeCreatorMethod()
            {
                return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod(new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Method[]
                    {
                        new IManyHashBitMapFilterNode_CheckBits_0(),
                        new IManyHashBitMapFilterNode_GetSize_1(),
                        new IManyHashBitMapFilterNode_SetBits_2(),
                        new IManyHashBitMapFilterNode_SetBitsBeforePersistence_3(),
                        new IManyHashBitMapFilterNode_SnapshotSet_4(),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo(4, typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap), SnapshotSet_SnapshotSerialize),
                    });
            }
            internal static void MethodParameterCreator()
            {
                GetServerNodeCreatorMethod();
                AutoCSer.AotReflection.NonPublicMethods(typeof(ManyHashBitMapFilterNodeMethodParameterCreator));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(IServiceNodeMethodEnum), typeof(ServiceNodeMethodParameterCreator))]
        public partial interface IServiceNode { }
        /// <summary>
        /// 服务基础操作接口方法映射枚举 节点方法序号映射枚举类型
        /// </summary>
        public enum IServiceNodeMethodEnum
        {
            /// <summary>
            /// [0] 创建位图节点 BitmapNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// uint capacity 二进制位数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateBitmapNode = 0,
            /// <summary>
            /// [1] 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// long identity 起始分配 ID
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateIdentityGeneratorNode = 1,
            /// <summary>
            /// [2] 删除节点
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNode = 2,
            /// <summary>
            /// [3] 删除节点
            /// string key 节点全局关键字
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNodeByKey = 3,
            /// <summary>
            /// [4] 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapClientFilterNode = 4,
            /// <summary>
            /// [5] 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapFilterNode = 5,
        }
        /// <summary>
        /// 创建位图节点 BitmapNode 服务端节点方法
        /// </summary>
        internal sealed class IServiceNode_CreateBitmapNode_0 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__>
        {
            internal IServiceNode_CreateBitmapNode_0() : base(0, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)3) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IServiceNode>(methodParameter).CreateBitmapNode(parameter.index, parameter.key, parameter.nodeInfo, parameter.capacity));
            }
        }
        /// <summary>
        /// 创建 64 位自增ID 节点 IdentityGeneratorNode 服务端节点方法
        /// </summary>
        internal sealed class IServiceNode_CreateIdentityGeneratorNode_1 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__>
        {
            internal IServiceNode_CreateIdentityGeneratorNode_1() : base(1, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)3) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IServiceNode>(methodParameter).CreateIdentityGeneratorNode(parameter.index, parameter.key, parameter.nodeInfo, parameter.identity));
            }
        }
        /// <summary>
        /// 删除节点 服务端节点方法
        /// </summary>
        internal sealed class IServiceNode_RemoveNode_2 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__>
        {
            internal IServiceNode_RemoveNode_2() : base(2, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)7) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IServiceNode>(methodParameter).RemoveNode(parameter.index));
            }
        }
        /// <summary>
        /// 删除节点 服务端节点方法
        /// </summary>
        internal sealed class IServiceNode_RemoveNodeByKey_3 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__>
        {
            internal IServiceNode_RemoveNodeByKey_3() : base(3, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)15) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IServiceNode>(methodParameter).RemoveNodeByKey(parameter.key));
            }
        }
        /// <summary>
        /// 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode 服务端节点方法
        /// </summary>
        internal sealed class IServiceNode_CreateManyHashBitMapClientFilterNode_4 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__>
        {
            internal IServiceNode_CreateManyHashBitMapClientFilterNode_4() : base(4, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)3) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IServiceNode>(methodParameter).CreateManyHashBitMapClientFilterNode(parameter.index, parameter.key, parameter.nodeInfo, parameter.size));
            }
        }
        /// <summary>
        /// 创建多哈希位图过滤节点 IManyHashBitMapFilterNode 服务端节点方法
        /// </summary>
        internal sealed class IServiceNode_CreateManyHashBitMapFilterNode_5 : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__>
        {
            internal IServiceNode_CreateManyHashBitMapFilterNode_5() : base(5, -2147483648, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)3) { }
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__ parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<IServiceNode>(methodParameter).CreateManyHashBitMapFilterNode(parameter.index, parameter.key, parameter.nodeInfo, parameter.size));
            }
        }
        /// <summary>
        /// 服务基础操作接口方法映射枚举 创建调用方法与参数信息
        /// </summary>
        internal sealed partial class ServiceNodeMethodParameterCreator
        {
            /// <summary>
            /// 获取生成服务端节点方法信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod GetServerNodeCreatorMethod()
            {
                return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod(new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Method[]
                    {
                        new IServiceNode_CreateBitmapNode_0(),
                        new IServiceNode_CreateIdentityGeneratorNode_1(),
                        new IServiceNode_RemoveNode_2(),
                        new IServiceNode_RemoveNodeByKey_3(),
                        new IServiceNode_CreateManyHashBitMapClientFilterNode_4(),
                        new IServiceNode_CreateManyHashBitMapFilterNode_5(),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo[]
                    {
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(-2147483648),
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo[]
                    {
                    });
            }
            internal static void MethodParameterCreator()
            {
                GetServerNodeCreatorMethod();
                AutoCSer.AotReflection.NonPublicMethods(typeof(ServiceNodeMethodParameterCreator));
            }
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 自增 ID 分段
        /// </summary>
    public partial struct IdentityFragment
    {
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(12))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.identity);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.count);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment value, byte* end)
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
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.identity);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.count);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment));
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 正在处理的消息标识
        /// </summary>
    public partial struct MessageIdeneity
    {
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(16))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.Identity);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.ArrayIndex);
                    __stream__.Write((byte)this.Flags);
                    __stream__.TryMoveSize(3);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value, byte* end)
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
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.ArrayIndex);
                byte Flags = 0;
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref Flags);
                this.Flags = (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageFlagsEnum)Flags;
                __start__ += 3;
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity));
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 节点索引信息
        /// </summary>
    public partial struct NodeIndex
    {
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex value)
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
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.Identity);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.Index);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex value, byte* end)
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
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex value = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
            }
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
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
                    AutoCSer.AotMethod.Call();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.AotMethod.Call();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex/**/.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CreateNodeIndex/**/.DefaultConstructorReflection();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment/**/.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap/**/.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity/**/.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex/**/.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo/**/.JsonSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult/**/.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity/**/.JsonSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip0__.SimpleSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.__ip9__.SimpleSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeLocalClient.LocalClientNode();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip1__.SimpleSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.__ip2__.SimpleSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeLocalClient.LocalClientNode();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip3__.SimpleSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.__ip5__.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeLocalClient.LocalClientNode();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip0__.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.__ip4__.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeLocalClient.LocalClientNode();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip0__.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip1__.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip2__.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip3__.SimpleSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.__ip4__.BinarySerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeLocalClient.LocalClientNode();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BitmapNodeMethodParameterCreator.MethodParameterCreator();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityGeneratorNodeMethodParameterCreator.MethodParameterCreator();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilterNodeMethodParameterCreator.MethodParameterCreator();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilterNodeMethodParameterCreator.MethodParameterCreator();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNodeMethodParameterCreator.MethodParameterCreator();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment/**/.SimpleSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity/**/.SimpleSerialize();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex/**/.SimpleSerialize();

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>));
                    AutoCSer.BinarySerializer.Simple<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>(null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter>));
                    AutoCSer.BinarySerializer.ICustom<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter>(null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<ulong[]>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.Algorithm.IntegerDivision>));
                    AutoCSer.BinarySerializer.Simple<AutoCSer.Algorithm.IntegerDivision>(null, default(AutoCSer.Algorithm.IntegerDivision));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo>));
                    AutoCSer.BinarySerializer.Json<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo>(null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum>));
                    AutoCSer.BinarySerializer.EnumByte<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum>(null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<string>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.Reflection.RemoteType>));
                    AutoCSer.BinarySerializer.Simple<AutoCSer.Reflection.RemoteType>(null, default(AutoCSer.Reflection.RemoteType));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter>));
                    AutoCSer.BinarySerializer.ICustom<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter>(null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter>));
                    AutoCSer.BinarySerializer.ICustom<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter>(null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<int>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<uint[]>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<uint>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<long>));
                    binaryDeserializeMemberTypes();

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Json.TypeSerializer<AutoCSer.Reflection.RemoteType>));


                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotNode.Create<byte[]>(null);
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.EnumerableSnapshotNode.Create<byte[]>(null);
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotNode.Create<long>(null);
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.EnumerableSnapshotNode.Create<long>(null);
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotNode.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap>(null);
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabase.EnumerableSnapshotNode.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap>(null);
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 二进制反序列化成员类型代码生成调用激活 AOT 反射
            /// </summary>
            private static void binaryDeserializeMemberTypes()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex t1 = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex);
                AutoCSer.BinaryDeserializer.Simple<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>(null, ref t1);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter t2 = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter);
                AutoCSer.BinaryDeserializer.ICustom<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter>(null, ref t2);
                AutoCSer.Algorithm.IntegerDivision t3 = default(AutoCSer.Algorithm.IntegerDivision);
                AutoCSer.BinaryDeserializer.Simple<AutoCSer.Algorithm.IntegerDivision>(null, ref t3);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo t4 = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo);
                AutoCSer.BinaryDeserializer.Json<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo>(null, ref t4);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum t5 = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum);
                AutoCSer.BinaryDeserializer.EnumByte<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum>(null, ref t5);
                AutoCSer.Reflection.RemoteType t6 = default(AutoCSer.Reflection.RemoteType);
                AutoCSer.BinaryDeserializer.Simple<AutoCSer.Reflection.RemoteType>(null, ref t6);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter t7 = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter);
                AutoCSer.BinaryDeserializer.ICustom<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter>(null, ref t7);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter t8 = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter);
                AutoCSer.BinaryDeserializer.ICustom<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter>(null, ref t8);
            }
    }
}
#endif