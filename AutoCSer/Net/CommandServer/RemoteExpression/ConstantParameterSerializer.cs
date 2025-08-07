using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式常量参数序列化
    /// </summary>
    internal sealed class ConstantParameterSerializer : ServerMethodParameter
    {
        /// <summary>
        /// 常量参数序列化
        /// </summary>
        internal readonly Action<ClientMetadata> Serializer;
        /// <summary>
        /// 远程表达式常量参数序列化
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="parameters"></param>
        internal ConstantParameterSerializer(Type type, ref LeftArray<ConstantParameter> parameters) : base(type)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(nameof(Serializer), null, new Type[] { typeof(ClientMetadata) }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            LocalBuilder parameter = generator.DeclareLocal(type);
            generator.Emit(OpCodes.Ldloca_S, parameter);
            generator.Emit(OpCodes.Initobj, type);
            foreach (FieldInfo field in Fields)
            {
                int index = int.Parse(field.Name);
                generator.Emit(OpCodes.Ldloca_S, parameter);
                generator.ldarg(0);
                generator.int32(index);
                generator.call(GetConstantParameterValueMethod.MakeGenericMethod(parameters.Array[index].Type));
                generator.Emit(OpCodes.Stfld, field);
            }
            generator.ldarg(0);
            generator.Emit(OpCodes.Ldloca_S, parameter);
            if (IsSimpleSerialize) generator.call(SimpleSerializeConstantParameterMethod.MakeGenericMethod(type));
            else generator.call(SerializeConstantParameterMethod.MakeGenericMethod(type));
            generator.Emit(OpCodes.Ret);
            Serializer = (Action<ClientMetadata>)dynamicMethod.CreateDelegate(typeof(Action<ClientMetadata>));
        }

        /// <summary>
        /// 远程表达式常量参数序列化集合
        /// </summary>
        private static readonly Dictionary<ConstantParameterKey, ConstantParameterSerializer> serializers = DictionaryCreator<ConstantParameterKey>.Create<ConstantParameterSerializer>();
        /// <summary>
        /// 远程表达式常量参数序列化编号
        /// </summary>
        private static int typeIndex;
        /// <summary>
        /// 获取远程表达式常量参数序列化
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static ConstantParameterSerializer Get(ConstantParameterKey key)
        {
            var type = default(ConstantParameterSerializer);
            bool isNew = false;
            Monitor.Enter(serializers);
            try
            {
                if (!serializers.TryGetValue(key, out type))
                {
                    TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".Net.CommandServer.RemoteExpressionConstantParameterSerializer" + (++typeIndex).toString(), TypeAttributes.AutoLayout | TypeAttributes.Public, typeof(ValueType), null);
                    int count = key.Parameters.Length, index = 0;
                    foreach (ConstantParameter parameter in key.Parameters.Array)
                    {
                        typeBuilder.DefineField(index.toString(), parameter.Type, FieldAttributes.Public);
                        if (--count == 0) break;
                        ++index;
                    }
                    serializers.Add(new ConstantParameterKey(key), type = new ConstantParameterSerializer(typeBuilder.CreateType(), ref key.Parameters));
                    isNew = true;
                }
            }
            finally { Monitor.Exit(serializers); }
            if (isNew) ServerMethodParameter.AppendType(type.Type);
            return type;
        }

        /// <summary>
        /// 获取常量参数
        /// </summary>
        internal static readonly MethodInfo GetConstantParameterValueMethod = typeof(ClientMetadata).GetMethod(nameof(ClientMetadata.GetConstantParameterValue), BindingFlags.Static | BindingFlags.NonPublic).notNull();
        /// <summary>
        /// 常量参数序列化
        /// </summary>
        internal static readonly MethodInfo SimpleSerializeConstantParameterMethod = typeof(ClientMetadata).GetMethod(nameof(ClientMetadata.SimpleSerializeConstantParameter), BindingFlags.Static | BindingFlags.NonPublic).notNull();
        /// <summary>
        /// 常量参数序列化
        /// </summary>
        internal static readonly MethodInfo SerializeConstantParameterMethod = typeof(ClientMetadata).GetMethod(nameof(ClientMetadata.SerializeConstantParameter), BindingFlags.Static | BindingFlags.NonPublic).notNull();
    }
#if DEBUG
    internal static class RemoteExpressionConstantParameterSerializerIL
    {
        struct Parameter
        {
            public int Int;
            public string String;
        }
        internal static void Serialize(ClientMetadata metadata)
        {
            Parameter parameter = new Parameter
            {
                Int = ClientMetadata.GetConstantParameter<int>(metadata, 0),
                String = ClientMetadata.GetConstantParameter<string>(metadata, 1).notNull()
            };
            ClientMetadata.SerializeConstantParameter(metadata, ref parameter);
        }
    }
#endif
}
