using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 接口方法信息
    /// </summary>
    internal abstract class InterfaceMethodBase
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        public readonly Type Type;
        /// <summary>
        /// 接口方法信息
        /// </summary>
        public readonly MethodInfo Method;
        /// <summary>
        /// Return value type
        /// 返回值类型
        /// </summary>
        internal Type ReturnValueType;
        /// <summary>
        /// 自定义命令序号，不能重复，默认小于 0  表示不指定。存在自定义需求时不要使用巨大的数据，建议从 0 开始，因为它会是某个数组的大小。
        /// </summary>
        internal int MethodIndex;
        /// <summary>
        /// 服务端客户端比较附加参数数量
        /// </summary>
        protected int equalsParameterCount;
        /// <summary>
        /// 有效参数数量
        /// </summary>
        internal int EqualsParameterCount { get { return ParameterCount + equalsParameterCount; } }
        /// <summary>
        /// 方法参数集合
        /// </summary>
        internal ParameterInfo[] Parameters;
        /// <summary>
        /// 有效参数起始位置
        /// </summary>
        internal int ParameterStartIndex;
        /// <summary>
        /// 有效参数结束位置
        /// </summary>
        internal int ParameterEndIndex;
        /// <summary>
        /// 有效参数数量
        /// </summary>
        internal int ParameterCount { get { return ParameterEndIndex - ParameterStartIndex; } }
        /// <summary>
        /// 输入参数集合
        /// </summary>
        internal IEnumerable<ParameterInfo> InputParameters
        {
            get
            {
                for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
                {
                    ParameterInfo parameter = Parameters[parameterIndex];
                    if (!parameter.IsOut) yield return parameter;
                }
            }
        }
        /// <summary>
        /// 输入参数类型
        /// </summary>
#if NetStandard21
        internal ServerMethodParameter? InputParameterType;
#else
        internal ServerMethodParameter InputParameterType;
#endif
        /// <summary>
        /// 输入参数字段集合
        /// </summary>
        internal FieldInfo[] InputParameterFields;
        /// <summary>
        /// 是否简单序列化输出数据
        /// </summary>
        internal bool IsSimpleSerializeParamter;
        /// <summary>
        /// Whether to simply deserialize the input data
        /// 是否简单反序列化输入数据
        /// </summary>
        internal bool IsSimpleDeserializeParamter;
        /// <summary>
        /// 返回值类型是否一致
        /// </summary>
        internal bool IsReturnType;
        /// <summary>
        /// 是否自定义基础方法
        /// </summary>
        protected bool isCustomBaseMethod;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        internal string? Error;
#else
        internal string Error;
#endif
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        internal InterfaceMethodBase()
        {
            Type = typeof(ServerInterface);
            Method = AutoCSer.Common.EmptyAction.Method;// AutoCSer.Reflection.Emit.StringWriter.UnmanagedStreamBasePrepSizeMethod;
            ReturnValueType = typeof(void);
            Parameters = EmptyArray<ParameterInfo>.Array;
            InputParameterFields = EmptyArray<FieldInfo>.Array;
        }
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        internal InterfaceMethodBase(Type type, MethodInfo method)
        {
            this.Type = type;
            this.Method = method;
            ReturnValueType = typeof(void);
            Parameters = EmptyArray<ParameterInfo>.Array;
            InputParameterFields = EmptyArray<FieldInfo>.Array;
        }
#if !AOT
        /// <summary>
        /// 设置输入数据
        /// </summary>
        /// <param name="methodGenerator"></param>
        /// <param name="newInputParameterLocalBuilder"></param>
        /// <param name="isCheckRedirectType"></param>
        internal void SetInputParameter(ILGenerator methodGenerator, LocalBuilder newInputParameterLocalBuilder, bool isCheckRedirectType = false)
        {
            int parameterIndex = 0;
            foreach (ParameterInfo parameter in InputParameters)
            {
                if (!parameter.IsOut)
                {
                    methodGenerator.Emit(OpCodes.Ldloca, newInputParameterLocalBuilder);
                    methodGenerator.ldarg(parameterIndex + ParameterStartIndex + 1);
                    if (parameter.ParameterType.IsByRef)
                    {
                        Type parameterType = parameter.elementType();
                        if (parameterType.IsValueType)
                        {
                            if (parameterType.IsEnum) parameterType = System.Enum.GetUnderlyingType(parameterType);
                            if (parameterType == typeof(int)) methodGenerator.Emit(OpCodes.Ldind_I4);
                            else if (parameterType == typeof(uint)) methodGenerator.Emit(OpCodes.Ldind_U4);
                            else if (parameterType == typeof(long) || parameterType == typeof(ulong)) methodGenerator.Emit(OpCodes.Ldind_I8);
                            else if (parameterType == typeof(byte)) methodGenerator.Emit(OpCodes.Ldind_U1);
                            else if (parameterType == typeof(sbyte)) methodGenerator.Emit(OpCodes.Ldind_I1);
                            else if (parameterType == typeof(short)) methodGenerator.Emit(OpCodes.Ldind_I2);
                            else if (parameterType == typeof(char) || parameterType == typeof(ushort)) methodGenerator.Emit(OpCodes.Ldind_U2);
                            else if (parameterType == typeof(float)) methodGenerator.Emit(OpCodes.Ldind_R4);
                            else if (parameterType == typeof(double)) methodGenerator.Emit(OpCodes.Ldind_R8);
                            else methodGenerator.Emit(OpCodes.Ldobj, parameterType);
                        }
                        else methodGenerator.Emit(OpCodes.Ldind_Ref);
                    }
                    else if(isCheckRedirectType)
                    {
                        var castMethod = default(MethodInfo);
                        var redirectType = MethodParameter.GetRedirectType(parameter, out castMethod);
                        if (redirectType != null) methodGenerator.call(castMethod.notNull());
                    }
                    methodGenerator.Emit(OpCodes.Stfld, InputParameterFields[parameterIndex]);
                }
                ++parameterIndex;
            }
        }
#endif
        /// <summary>
        /// 检查方法编号
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="methodIndexAttribute"></param>
        /// <param name="methodIndexEnumType"></param>
        /// <param name="methodArray"></param>
        /// <param name="messages"></param>
        /// <param name="methods"></param>
        /// <param name="minCustomMethodIndex"></param>
        /// <returns></returns>
#if NetStandard21
        internal static string? CheckMethodIndexs<T>(Type type, InterfaceMethodIndexAttribute methodIndexAttribute, Type? methodIndexEnumType, ref LeftArray<T> methodArray, ref LeftArray<string> messages, out T?[] methods, int minCustomMethodIndex = 0)
#else
        internal static string CheckMethodIndexs<T>(Type type, InterfaceMethodIndexAttribute methodIndexAttribute, Type methodIndexEnumType, ref LeftArray<T> methodArray, ref LeftArray<string> messages, out T[] methods, int minCustomMethodIndex = 0)
#endif
            where T : InterfaceMethodBase
        {
            methods = EmptyArray<T>.Array;
            int methodIndex = methodArray.Length - 1, customMethodCount = 0;
#if NetStandard21
            Dictionary<int, T?> methodIndexs = DictionaryCreator.CreateInt<T?>();
#else
            Dictionary<int, T> methodIndexs = DictionaryCreator.CreateInt<T>();
#endif
            foreach (T method in methodArray)
            {
                if (method.MethodIndex >= 0)
                {
                    var indexMethod = default(T);
                    if (methodIndexs.TryGetValue(method.MethodIndex, out indexMethod))
                    {
                        return $"{type.fullName()} 方法序号 {method.MethodIndex.toString()} 冲突 {method.Method.Name} + {indexMethod?.Method.Name}";
                    }
                    methodIndexs.Add(method.MethodIndex, method);
                    if (method.MethodIndex > methodIndex) methodIndex = method.MethodIndex;
                }
                if (method.isCustomBaseMethod) ++customMethodCount;
            }
            int unknownEnumCount = 0;
            if (methodIndexEnumType != null)
            {
                if (!methodIndexEnumType.IsEnum)
                {
                    return $"方法序号映射类型 {methodIndexEnumType.fullName()} 必须为 enum 类型";
                }
                Array enums = System.Enum.GetValues(methodIndexEnumType);
                Dictionary<string, object> enumNames = DictionaryCreator<string>.Create<object>(enums.Length);
                foreach (object value in enums) enumNames.Add(value.ToString().notNull(), value);
                foreach (T method in methodArray)
                {
                    if (method.MethodIndex < 0)
                    {
                        var value = default(object);
                        string hashKey = method.Method.Name;
                        if (enumNames.TryGetValue(hashKey, out value))
                        {
                            method.MethodIndex = ((IConvertible)value).ToInt32(null);
                            if (method.MethodIndex >= minCustomMethodIndex || !method.isCustomBaseMethod)
                            {
                                var indexMethod = default(T);
                                if (methodIndexs.TryGetValue(method.MethodIndex, out indexMethod))
                                {
                                    return $"{type.fullName()} 命令序号 {method.MethodIndex.toString()} 冲突 {method.Method.Name} + {indexMethod?.Method.Name}";
                                }
                                methodIndexs.Add(method.MethodIndex, method);
                                if (method.MethodIndex > methodIndex) methodIndex = method.MethodIndex;
                                enumNames.Remove(hashKey);
                            }
                            else
                            {
                                messages.Add($"自定义基础方法 {type.fullName()}.{method.Method.Name} 命令序号 {method.MethodIndex} 必须不允许小于 {minCustomMethodIndex}");
                                method.MethodIndex = -1;
                            }
                        }
                        else messages.Add($"{type.fullName()}.{method.Method.Name} 没有找到枚举命令序号");
                    }
                }
                if (enumNames.Count != 0)
                {
                    foreach (object value in enumNames.Values)
                    {
                        if (methodIndexs.TryAdd(((IConvertible)value).ToInt32(null), null)) ++unknownEnumCount;
                    }
                    messages.Add($"{type.fullName()} 没有找到枚举命令 {string.Join(", ", enumNames.Keys)}");
                }
            }
            ++methodIndex;
            if (customMethodCount != 0 && methodIndex < (minCustomMethodIndex + customMethodCount)) methodIndex = minCustomMethodIndex + customMethodCount;
            methods = new T[methodIndex + unknownEnumCount];
            foreach (var method in methodIndexs) methods[method.Key] = method.Value;
            methodIndex = 0;
            foreach (T method in methodArray)
            {
                if (method.MethodIndex < 0)
                {
                    if (AutoCSer.Common.IsCodeGenerator || methodIndexAttribute.IsAutoMethodIndex)
                    {
                        if (method.isCustomBaseMethod)
                        {
                            while (methods[minCustomMethodIndex] != null || methodIndexs.ContainsKey(minCustomMethodIndex)) ++minCustomMethodIndex;
                            methods[method.MethodIndex = minCustomMethodIndex] = method;
                            ++minCustomMethodIndex;
                        }
                        else
                        {
                            while (methods[methodIndex] != null || methodIndexs.ContainsKey(methodIndex)) ++methodIndex;
                            methods[method.MethodIndex = methodIndex] = method;
                            ++methodIndex;
                        }
                    }
                    else return $"{type.fullName()}.{method.Method.Name} 缺少命令序号";
                }
            }
            return null;
        }
        /// <summary>
        /// 服务端接口方法排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int Compare(InterfaceMethodBase left, InterfaceMethodBase right)
        {
            if (left.ReturnValueType == typeof(CommandServerVerifyStateEnum))
            {
                if (right.ReturnValueType != typeof(CommandServerVerifyStateEnum)) return -1;
            }
            else if (right.ReturnValueType == typeof(CommandServerVerifyStateEnum)) return 1;
            return MethodCompare(left, right);
        }
        /// <summary>
        /// 服务端接口方法排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int MethodCompare(InterfaceMethodBase left, InterfaceMethodBase right)
        {
            if (left.Type == right.Type)
            {
                int value = string.CompareOrdinal(left.Method.Name, right.Method.Name);
                if (value == 0)
                {
                    value = left.ParameterCount - right.ParameterCount;
                    if (value == 0)
                    {
                        if (left.ReturnValueType != right.ReturnValueType)
                        {
                            value = string.CompareOrdinal(left.ReturnValueType.FullName, right.ReturnValueType.FullName);
                        }
                        if (value == 0)
                        {
                            for (int parameterIndex = 0; parameterIndex != left.ParameterCount; ++parameterIndex)
                            {
                                value = compare(left.Parameters[left.ParameterStartIndex + parameterIndex], right.Parameters[right.ParameterStartIndex + parameterIndex]);
                                if (value != 0) return value;
                            }
                        }
                    }
                }
                return value;
            }
            return string.CompareOrdinal(left.Type.FullName, right.Type.FullName);
        }
        /// <summary>
        /// 参数排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(ParameterInfo left, ParameterInfo right)
        {
            int value = string.CompareOrdinal(left.Name, right.Name);
            if (value == 0 && left.ParameterType != right.ParameterType)
            {
                value = string.CompareOrdinal(left.ParameterType.FullName, right.ParameterType.FullName);
            }
            return value;
        }
    }
}
