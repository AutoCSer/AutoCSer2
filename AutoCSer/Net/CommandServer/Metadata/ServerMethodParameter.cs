using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 命令服务参数类型
    /// </summary>
    internal class ServerMethodParameter
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// 参数字段集合
        /// </summary>
        internal readonly FieldInfo[] Fields;
        /// <summary>
        /// 是否已经检查参数字段集合
        /// </summary>
        private bool isCheckFields;
        /// <summary>
        /// 是否支持简单序列化
        /// </summary>
        private bool isSimpleSerialize;
        /// <summary>
        /// 是否支持简单序列化
        /// </summary>
        internal bool IsSimpleSerialize
        {
            get
            {
                if (!isCheckFields) checkFields();
                return isSimpleSerialize;
            }
        }
        /// <summary>
        /// 是否需要初始化对象
        /// </summary>
        private bool isInitobj;
        /// <summary>
        /// 是否需要初始化对象
        /// </summary>
        internal bool IsInitobj
        {
            get
            {
                if (!isCheckFields) checkFields();
                return isInitobj;
            }
        }
        /// <summary>
        /// 命令服务参数类型
        /// </summary>
        /// <param name="type">参数类型</param>
        internal ServerMethodParameter(Type type)
        {
            Type = type;
            Fields = type.GetFields();
        }
        /// <summary>
        /// 检查参数字段集合
        /// </summary>
        private void checkFields()
        {
            isSimpleSerialize = true;
            foreach (FieldInfo field in Fields)
            {
                if (isSimpleSerialize && !SimpleSerialize.Serializer.IsType(field.FieldType)) isSimpleSerialize = false;
                if (!isInitobj && DynamicArray.IsClearArray(field.FieldType)) isInitobj = true;
            }
            isCheckFields = true;
        }
        /// <summary>
        /// 根据参数获取字段数据
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="returnValueName"></param>
        /// <returns></returns>
#if NetStandard21
        internal FieldInfo[] GetFields(IEnumerable<ParameterInfo> parameters, string? returnValueName = null)
#else
        internal FieldInfo[] GetFields(IEnumerable<ParameterInfo> parameters, string returnValueName = null)
#endif
        {
            int fieldIndex = 0;
            var parameterFields = default(FieldInfo[]);
            foreach (ParameterInfo parameter in parameters)
            {
                string matchName = parameter.Name.notNull();
                if (matchName == returnValueName) matchName = nameof(ServerReturnValue<int>.ReturnValue);
                if (parameterFields == null && Fields[fieldIndex].Name != matchName)
                {
                    AutoCSer.Common.CopyTo(Fields, parameterFields = new FieldInfo[Fields.Length], 0, fieldIndex);
                }
                if (parameterFields != null)
                {
                    foreach (FieldInfo field in Fields)
                    {
                        if (field.Name == matchName)
                        {
                            parameterFields[fieldIndex] = field;
                            break;
                        }
                    }
                }
                ++fieldIndex;
            }
            if (parameterFields != null && fieldIndex != Fields.Length)
            {
                foreach (FieldInfo field in Fields)
                {
                    if (field.Name == nameof(ServerReturnValue<int>.ReturnValue))
                    {
                        parameterFields[fieldIndex] = field;
                        break;
                    }
                }
            }
            return parameterFields ?? Fields;
        }
        /// <summary>
        /// 获取参数字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
#if NetStandard21
        internal FieldInfo? GetField(string name)
#else
        internal FieldInfo GetField(string name)
#endif
        {
            foreach (FieldInfo field in Fields)
            {
                if (field.Name == name) return field;
            }
            return null;
        }

        /// <summary>
        /// 命令服务参数类型集合
        /// </summary>
        private static readonly Dictionary<ServerMethodParameterKey, ServerMethodParameter> keyTypes = DictionaryCreator<ServerMethodParameterKey>.Create<ServerMethodParameter>();
        /// <summary>
        /// 命令服务参数类型集合
        /// </summary>
        internal static readonly HashSet<HashObject<Type>> Types = HashSetCreator.CreateHashObject<Type>();
        ///// <summary>
        ///// 判断是否命令服务参数类型
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static bool IsType(Type type)
        //{
        //    return Types.Contains(type);
        //}
        /// <summary>
        /// 命令服务参数类型编号
        /// </summary>
        private static int typeIndex;
        /// <summary>
        /// 获取命令服务参数类型关键字
        /// </summary>
        /// <param name="parameterCount"></param>
        /// <param name="parameters"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
#if NetStandard21
        private static ServerMethodParameterKey getKey(int parameterCount, IEnumerable<ParameterInfo> parameters, Type? returnType)
#else
        private static ServerMethodParameterKey getKey(int parameterCount, IEnumerable<ParameterInfo> parameters, Type returnType)
#endif
        {
            if (parameterCount == 0) return new ServerMethodParameterKey(EmptyArray<ParameterInfo>.Array, returnType);
            int parameterIndex = 0;
            ParameterInfo[] parameterArray = new ParameterInfo[parameterCount];
            foreach (ParameterInfo parameter in parameters) parameterArray[parameterIndex++] = parameter;
            return new ServerMethodParameterKey(parameterArray, returnType == typeof(void) ? null : returnType);
        }
        /// <summary>
        /// 获取命令服务参数类型
        /// </summary>
        /// <param name="parameterCount"></param>
        /// <param name="parameters">参数集合</param>
        /// <param name="returnType">类型</param>
        /// <returns></returns>
#if NetStandard21
        internal static ServerMethodParameter? Get(int parameterCount, IEnumerable<ParameterInfo> parameters, Type? returnType)
#else
        internal static ServerMethodParameter Get(int parameterCount, IEnumerable<ParameterInfo> parameters, Type returnType)
#endif
        {
            var type = default(ServerMethodParameter);
            ServerMethodParameterKey key = getKey(parameterCount, parameters, returnType);
            Monitor.Enter(keyTypes);
            keyTypes.TryGetValue(key, out type);
            Monitor.Exit(keyTypes);
            return type;
        }
        /// <summary>
        /// 获取命令服务参数类型
        /// </summary>
        /// <param name="parameterCount"></param>
        /// <param name="parameters">参数集合</param>
        /// <param name="returnType">类型</param>
        /// <returns></returns>
        internal static ServerMethodParameter GetOrCreate(int parameterCount, IEnumerable<ParameterInfo> parameters, Type returnType)
        {
            var type = default(ServerMethodParameter);
            ServerMethodParameterKey key = getKey(parameterCount, parameters, returnType);
            Monitor.Enter(keyTypes);
            if (keyTypes.TryGetValue(key, out type))
            {
                Monitor.Exit(keyTypes);
                return type;
            }
            try
            {
                TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".Net.CommandServer.ServerMethodParameter" + (++typeIndex).toString(), TypeAttributes.AutoLayout | TypeAttributes.Public, typeof(ValueType), null);
                foreach (ParameterInfo parameter in parameters) typeBuilder.DefineField(parameter.Name.notNull(), parameter.elementType(), FieldAttributes.Public);
                if (returnType != typeof(void))
                {
                    FieldBuilder returnFieldBuilder = typeBuilder.DefineField(nameof(ServerReturnValue<int>.ReturnValue), returnType, FieldAttributes.Public);
                }
                keyTypes.Add(key, type = new ServerMethodParameter(typeBuilder.CreateType()));
                Types.Add(type.Type);
            }
            finally { Monitor.Exit(keyTypes); }
            return type;
        }
        /// <summary>
        /// 添加命令服务参数类型
        /// </summary>
        /// <param name="type"></param>
        internal static void AppendType(Type type)
        {
            Monitor.Enter(keyTypes);
            try
            {
                Types.Add(type);
            }
            finally { Monitor.Exit(keyTypes); }
        }
    }
}
