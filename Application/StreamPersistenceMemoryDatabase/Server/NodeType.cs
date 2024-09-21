using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点类型信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct NodeType
    {
        /// <summary>
        /// 节点接口配置
        /// </summary>
        internal ServerNodeAttribute NodeAttribute;
        /// <summary>
        /// 服务端接口方法信息集合
        /// </summary>
        internal ServerNodeMethod[] Methods;
        /// <summary>
        /// 提示信息集合
        /// </summary>
        internal LeftArray<string> Messages;
        /// <summary>
        /// 错误信息
        /// </summary>
        internal string Error;
        /// <summary>
        /// 服务端节点类型信息
        /// </summary>
        /// <param name="type"></param>
        internal NodeType(Type type)
        {
            Methods = null;
            Messages = new LeftArray<string>(0);
            Error = CheckType(type, out NodeAttribute);
            if (Error != null) return;

            LeftArray<ServerNodeMethod> methodArray = new LeftArray<ServerNodeMethod>(0);
            Error = ServerNodeMethod.GetMethod(type, ref methodArray, true);
            if (Error != null) return;
            foreach (Type interfaceType in type.GetInterfaces())
            {
                Error = ServerNodeMethod.GetMethod(interfaceType, ref methodArray, false);
                if (Error != null) return;
            }
            if (methodArray.Length == 0)
            {
                Error = $"没有找到接口方法定义 {type.fullName()}";
                return;
            }

            methodArray.Sort(AutoCSer.Net.CommandServer.InterfaceMethodBase.Compare);
            Error = AutoCSer.Net.CommandServer.InterfaceMethodBase.CheckMethodIndexs(type, NodeAttribute, ref methodArray, ref Messages, out Methods);
            if (Error != null) return;
            foreach (ServerNodeMethod beforePersistenceMethod in Methods)
            {
                if (beforePersistenceMethod != null && beforePersistenceMethod.PersistenceMethodName.Length != 0)
                {
                    bool isMethod = false;
                    foreach (ServerNodeMethod nodeMethod in Methods)
                    {
                        if (nodeMethod != null && nodeMethod.CheckBeforePersistence(beforePersistenceMethod, ref Error))
                        {
                            if (Error != null) return;
                            isMethod = true;
                            break;
                        }
                    }
                    if (!isMethod)
                    {
                        Error = $"{type.fullName()} 持久化检查方法 {beforePersistenceMethod.Method.Name} 没有找到匹配的持久化方法 {beforePersistenceMethod.PersistenceMethodName}";
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 获取客户端方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="creatorException"></param>
        /// <param name="creatorMessages"></param>
        /// <param name="methods"></param>
        /// <param name="isLocalClient"></param>
        /// <returns></returns>
        internal bool GetClientMethods(Type type, ref Exception creatorException, ref string[] creatorMessages, out ClientNodeMethod[] methods, bool isLocalClient)
        {
            methods = null;
            LeftArray<ClientNodeMethod> methodArray = new LeftArray<ClientNodeMethod>(0);
            if (Error == null)
            {
                if (Messages.Length != 0) creatorMessages = Messages.ToArray();
                Error = ClientNodeMethod.GetMethod(type, ref methodArray, isLocalClient);
            }
            if (Error != null)
            {
                creatorException = new Exception($"{type.fullName()} 客户端节点生成失败 {Error}");
                return false;
            }
            foreach (Type interfaceType in type.GetInterfaces())
            {
                Error = ClientNodeMethod.GetMethod(interfaceType, ref methodArray, isLocalClient);
                if (Error != null)
                {
                    creatorException = new Exception($"{type.fullName()} 客户端节点生成失败 {Error}");
                    return false;
                }
            }
            if (methodArray.Length == 0)
            {
                creatorException = new Exception($"{type.fullName()} 客户端节点生成失败 没有找到接口方法定义");
                return false;
            }
            bool checkEnum = false;
            methods = new ClientNodeMethod[Methods.Length];
            Dictionary<StreamPersistenceMemoryDatabase.NodeMethod, HeadLeftArray<ServerNodeMethod>> serverMethodGroup = GetMethodGroup();
            foreach (ClientNodeMethod method in methodArray)
            {
                HeadLeftArray<ServerNodeMethod> serverMethodArray;
                if (serverMethodGroup.TryGetValue(method, out serverMethodArray))
                {
                    if (serverMethodArray.Count == 1)
                    {
                        string error = method.Set(serverMethodArray.Head);
                        if (error == null) methods[method.MethodIndex] = method;
                        else
                        {
                            creatorException = new Exception(error);
                            return false;
                        }
                    }
                    else
                    {
                        creatorException = new Exception($"客户端节点方法 {method.Method.Name} 匹配到多个服务端节点方法 {string.Join(",", serverMethodArray.Values.Select(p => p.Method.Name))}");
                        return false;
                    }
                }
                else checkEnum = true;
            }
            if (checkEnum)
            {
                Dictionary<HashString, object> enumNames = null;
                if (NodeAttribute.MethodIndexEnumType != null)
                {
                    Array enums = System.Enum.GetValues(NodeAttribute.MethodIndexEnumType);
                    enumNames = DictionaryCreator.CreateHashString<object>(enums.Length);
                    foreach (object value in enums) enumNames.Add(value.ToString(), value);
                }
                else enumNames = DictionaryCreator.CreateHashString<object>();
                foreach (ClientNodeMethod method in methodArray)
                {
                    if (method.MethodIndex < 0)
                    {
                        object value;
                        HashString hashKey = method.Method.Name;
                        if (enumNames.TryGetValue(hashKey, out value))
                        {
                            method.MethodIndex = ((IConvertible)value).ToInt32(null);
                            if (methods[method.MethodIndex] == null) methods[method.MethodIndex] = method;
                            else
                            {
                                creatorException = new Exception($"{type.fullName()} 客户端节点方法编号 {method.MethodIndex.toString()} 冲突 {method.Method.Name} + {methods[method.MethodIndex].Method.Name}");
                                return false;
                            }
                            enumNames.Remove(hashKey);
                        }
                        else
                        {
                            creatorException = new Exception($"{type.fullName()} 客户端节点生成失败 {method.Method.Name} 没有找到匹配的服务端节点方法");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 获取方法分组
        /// </summary>
        /// <returns></returns>
        internal Dictionary<StreamPersistenceMemoryDatabase.NodeMethod, HeadLeftArray<ServerNodeMethod>> GetMethodGroup()
        {
            if (Methods == null) return null;
            Dictionary<StreamPersistenceMemoryDatabase.NodeMethod, HeadLeftArray<ServerNodeMethod>> methodGroup = DictionaryCreator<StreamPersistenceMemoryDatabase.NodeMethod>.Create<HeadLeftArray<ServerNodeMethod>>(Methods.Length);
            foreach (ServerNodeMethod method in Methods)
            {
                if (method != null && method.IsClientCall)
                {
                    HeadLeftArray<ServerNodeMethod> methodArray;
                    if (methodGroup.TryGetValue(method, out methodArray))
                    {
                        methodArray.Add(method);
                        methodGroup[method] = methodArray;
                    }
                    else methodGroup.Add(method, new HeadLeftArray<ServerNodeMethod>(method));
                }
            }
            return methodGroup;
        }


        /// <summary>
        /// 接口类型检查
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string CheckType(Type type)
        {
            if (!type.IsInterface) return $"不支持非接口类型 {type.fullName()}";
            if (type.IsGenericTypeDefinition && !AutoCSer.Common.IsCodeGenerator) return $"不支持泛型接口类型 {type.fullName()}";
            foreach (PropertyInfo property in type.GetProperties()) return $"不支持属性 {type.fullName()}";
            return null;
        }
        /// <summary>
        /// 检查节点接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nodeAttribute"></param>
        /// <returns>错误信息</returns>
        internal static string CheckType(Type type, out ServerNodeAttribute nodeAttribute)
        {
            string error = CheckType(type);
            if (error == null)
            {
                nodeAttribute = (ServerNodeAttribute)type.GetCustomAttribute(typeof(ServerNodeAttribute), false) ?? ServerNode.DefaultAttribute;
                if (nodeAttribute.MethodIndexEnumType == null) return $"{type.fullName()} 缺少配置方法序号映射枚举类型 {typeof(ServerNodeAttribute).fullName()}.{nameof(ServerNodeAttribute.MethodIndexEnumType)}";
                return null;
            }
            nodeAttribute = null;
            return error;
        }
    }
}
