using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer.RemoteExpression;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程元数据方法编号信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    internal struct RemoteMetadataMethodIndex
    {
        /// <summary>
        /// 方法编号
        /// </summary>
        public int Index;
        /// <summary>
        /// 类型编号
        /// </summary>
        public int TypeIndex;
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName;
        /// <summary>
        /// 参数类型集合
        /// </summary>
        public int[] ParameterTypes;
        /// <summary>
        /// 泛型参数类型集合
        /// </summary>
        public int[] GenericTypes;
        /// <summary>
        /// 方法选择标记
        /// </summary>
        public BindingFlags BindingFlags;
        /// <summary>
        /// 远程元数据方法编号信息
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="typeIndex"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="genericTypes"></param>
        internal RemoteMetadataMethodIndex(string methodName, int typeIndex, BindingFlags bindingFlags, int[] parameterTypes, int[] genericTypes)
        {
            Index = 0;
            TypeIndex = typeIndex;
            MethodName = methodName;
            ParameterTypes = parameterTypes;
            GenericTypes = genericTypes;
            BindingFlags = bindingFlags;
        }
        /// <summary>
        /// 获取方法信息
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="types"></param>
        /// <returns></returns>
#if NetStandard21
        internal MethodInfo? GetMethod(ClientMetadata metadata, ref LeftArray<KeyValue<Type, int>> types)
#else
        internal MethodInfo GetMethod(ClientMetadata metadata, ref LeftArray<KeyValue<Type, int>> types)
#endif
        {
            var type = metadata.GetType(TypeIndex, ref types);
            if (type != null)
            {
                var parameterTypes = metadata.GetTypeArray(ParameterTypes, ref types);
                if (parameterTypes != null)
                {
                    if (GenericTypes.Length == 0) return type.GetMethod(MethodName, BindingFlags, null, parameterTypes, null);
                    var genericTypes = metadata.GetTypeArray(GenericTypes, ref types);
                    if (genericTypes != null) return GetMethod(type, MethodName, BindingFlags, genericTypes, parameterTypes);
                }
            }
            return null;
        }
        /// <summary>
        /// 获取方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
#if NetStandard21
        internal static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, Type[] types, Type[] parameterTypes)
#else
        internal static MethodInfo GetMethod(Type type, string methodName, BindingFlags bindingFlags, Type[] types, Type[] parameterTypes)
#endif
        {
            foreach (MethodInfo method in type.GetMethods(bindingFlags))
            {
                if (method.IsGenericMethodDefinition && method.Name == methodName)
                {
                    Type[] genericTypes = method.GetGenericArguments();
                    if (genericTypes.Length == types.Length)
                    {
                        int index = 0;
                        foreach (Type genericType in genericTypes)
                        {
                            if (types[index] != genericType) break;
                            ++index;
                        }
                        if (index == parameterTypes.Length)
                        {
                            ParameterInfo[] parameters = method.GetParameters();
                            if (parameters.Length == parameterTypes.Length)
                            {
                                index = 0;
                                foreach (ParameterInfo parameter in parameters)
                                {
                                    if (parameterTypes[index] != parameter.ParameterType) break;
                                    ++index;
                                }
                                if (index == parameterTypes.Length) return method;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
