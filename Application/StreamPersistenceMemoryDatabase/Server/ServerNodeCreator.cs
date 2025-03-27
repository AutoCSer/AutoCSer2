using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 生成服务端节点
    /// </summary>
    public sealed class ServerNodeCreator
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        internal readonly StreamPersistenceMemoryDatabaseServiceBase Service;
        /// <summary>
        /// 节点接口类型
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// 节点方法集合访问锁
        /// </summary>
        private readonly object methodLock;
        /// <summary>
        /// 节点方法集合
        /// </summary>
#if NetStandard21
        internal Method?[] Methods;
#else
        internal Method[] Methods;
#endif
        /// <summary>
        /// 节点方法信息集合
        /// </summary>
#if NetStandard21
        internal ServerNodeMethod?[] NodeMethods;
#else
        internal ServerNodeMethod[] NodeMethods;
#endif
        /// <summary>
        /// 快照方法集合
        /// </summary>
        internal SnapshotMethod[] SnapshotMethods;
        /// <summary>
        /// 节点状态
        /// </summary>
        internal CallStateEnum State;
        /// <summary>
        /// 生成服务端节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="type">节点接口类型</param>
        /// <param name="methods">节点方法集合</param>
        /// <param name="nodeMethods">节点方法信息集合</param>
        /// <param name="snapshotMethods">快照方法集合</param>
#if NetStandard21
        unsafe internal ServerNodeCreator(StreamPersistenceMemoryDatabaseServiceBase service, Type type, Method?[] methods, ServerNodeMethod?[] nodeMethods, SnapshotMethod[] snapshotMethods)
#else
        unsafe internal ServerNodeCreator(StreamPersistenceMemoryDatabaseServiceBase service, Type type, Method[] methods, ServerNodeMethod[] nodeMethods, SnapshotMethod[] snapshotMethods)
#endif
        {
            Service = service;
            Type = type;
            Methods = methods;
            NodeMethods = nodeMethods;
            SnapshotMethods = snapshotMethods;
            //SnapshotType = snapshotType;
            State = CallStateEnum.Success;
            methodLock = new object();
        }
        /// <summary>
        /// 获取快照方法索引位置
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private int getSnapshotMethodIndex(Method method)
        {
            int index = 0;
            foreach (SnapshotMethod snapshotMethod in SnapshotMethods)
            {
                if (object.ReferenceEquals(snapshotMethod.Method, method)) return index;
                ++index;
            }
            return -1;
        }
        /// <summary>
        /// 获取快照方法索引
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal Method GetSnapshotMethod(Type type)
        {
            foreach (SnapshotMethod snapshotMethod in SnapshotMethods)
            {
                if (snapshotMethod.Type == type) return snapshotMethod.Method;
            }
            throw new MissingMethodException(Type.fullName() + @"
" + type.fullName());
        }
        /// <summary>
        /// 快照节点检查
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal CallStateEnum CheckSnapshotNode(Type type)
        {
            bool isSnapshot = false;
            foreach (Type checkType in type.GetInterfaces())
            {
                if (checkType.IsGenericType && checkType.GetGenericTypeDefinition() == typeof(ISnapshot<>))
                {
                    Type snapshotType = checkType.GetGenericArguments()[0];
                    if (GetSnapshotMethod(snapshotType) == null) return CallStateEnum.NotFoundSnapshotMethod;
                    isSnapshot = true;
                }
            }
            return isSnapshot ? CallStateEnum.Success : CallStateEnum.NotFoundSnapshotNode;
        }
        /// <summary>
        /// 初始化节点加载修复方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal void LoadRepairNodeMethod<T>()
        {
            string nodeTypeFullName = Type.fullName().notNull();
            RepairNodeMethodDirectory repairNodeMethodDirectory = new RepairNodeMethodDirectory(nodeTypeFullName);
            DirectoryInfo typeDirectory = new DirectoryInfo(Path.Combine(Service.PersistenceDirectory.FullName, Service.Config.RepairNodeMethodDirectoryName, repairNodeMethodDirectory.NodeTypeHashCode.toHex()));
            if (typeDirectory.Exists)
            {
                HashSet<int> methodIndexs = HashSetCreator.CreateInt();
                var positionMethodIndexs = default(HashSet<HashKey<ulong, uint>>);
                foreach (DirectoryInfo methodDirectory in typeDirectory.GetDirectories().Where(p => p.Name.Length == 16 + 14 + 8).OrderByDescending(p => p.Name))
                {
                    if (GetMethodDirectory(methodDirectory, ref repairNodeMethodDirectory))
                    {
                        if (Service.IsLoaded || repairNodeMethodDirectory.Position <= Service.RebuildPosition)
                        {
                            var repairNodeMethod = loadRepairNodeMethod<T>(methodDirectory, nodeTypeFullName, methodIndexs, !Service.IsLoaded);
                            if (repairNodeMethod != null)
                            {
                                repairNodeMethod.RepairNodeMethodDirectory = repairNodeMethodDirectory;
                                Service.AppendLoadedRepairNodeMethod(repairNodeMethod);
                            }
                        }
                        else
                        {
                            FileInfo methodNameFile = new FileInfo(Path.Combine(methodDirectory.FullName, Service.Config.RepairNodeMethodNameFileName));
                            if (methodNameFile.Exists)
                            {
                                RepairNodeMethodName methodName = AutoCSer.JsonDeserializer.Deserialize<RepairNodeMethodName>(File.ReadAllText(methodNameFile.FullName, Encoding.UTF8));
                                if (methodName.NodeTypeFullName == nodeTypeFullName)
                                {
                                    if (positionMethodIndexs == null) positionMethodIndexs = HashSetCreator<HashKey<ulong, uint>>.Create();
                                    if (positionMethodIndexs.Add(new HashKey<ulong, uint>(repairNodeMethodDirectory.Position, repairNodeMethodDirectory.MethodIndex)))
                                    {
                                        Service.AppendRepairNodeMethodLoader(repairNodeMethodDirectory.Position, new RepairNodeMethodLoader<T>(this, methodDirectory, ref repairNodeMethodDirectory));
                                    }
                                    else Directory.Move(methodDirectory.FullName, methodDirectory.FullName + ".bak");
                                }
                            }
                            else throw new FileNotFoundException(methodNameFile.FullName);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 初始化节点加载修复方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodDirectory"></param>
        /// <param name="nodeTypeFullName"></param>
        /// <param name="methodIndexs"></param>
        /// <param name="isMoveHistory"></param>
        /// <returns></returns>
#if NetStandard21
        internal RepairNodeMethod? loadRepairNodeMethod<T>(DirectoryInfo methodDirectory, string nodeTypeFullName, HashSet<int>? methodIndexs, bool isMoveHistory)
#else
        internal RepairNodeMethod loadRepairNodeMethod<T>(DirectoryInfo methodDirectory, string nodeTypeFullName, HashSet<int> methodIndexs, bool isMoveHistory)
#endif
        {
            FileInfo assemblyFile = new FileInfo(Path.Combine(methodDirectory.FullName, Service.Config.RepairNodeMethodAssemblyFileName));
            FileInfo methodNameFile = new FileInfo(Path.Combine(methodDirectory.FullName, Service.Config.RepairNodeMethodNameFileName));
            if (assemblyFile.Exists)
            {
                if (methodNameFile.Exists)
                {
                    RepairNodeMethodName methodName = AutoCSer.JsonDeserializer.Deserialize<RepairNodeMethodName>(File.ReadAllText(methodNameFile.FullName, Encoding.UTF8));
                    if (methodName.NodeTypeFullName == nodeTypeFullName)
                    {
                        var methodInfo = default(MethodInfo);
                        byte[] rawAssembly = File.ReadAllBytes(assemblyFile.FullName);
                        CallStateEnum state = GetRepairMethod(ref rawAssembly, ref methodName, out methodInfo);
                        if (state == CallStateEnum.Success)
                        {
                            methodInfo = methodInfo.notNull();
                            ServerMethodAttribute methodAttribute = methodInfo.GetCustomAttribute<ServerMethodAttribute>(false).notNull();
                            if (methodIndexs == null || methodIndexs.Add(methodAttribute.MethodIndex))
                            {
                                if ((uint)methodAttribute.MethodIndex >= (uint)Methods.Length) Methods = AutoCSer.Common.GetCopyArray(Methods, methodAttribute.MethodIndex + 1);
                                if ((uint)methodAttribute.MethodIndex >= (uint)NodeMethods.Length) NodeMethods = AutoCSer.Common.GetCopyArray(NodeMethods, methodAttribute.MethodIndex + 1);
                                var method = Methods[methodAttribute.MethodIndex];
                                var nodeMethod = default(ServerNodeMethod);
                                if (method != null)
                                {
                                    nodeMethod = NodeMethods[methodAttribute.MethodIndex].notNull();
                                    state = nodeMethod.CheckRepair(Type, methodInfo);
                                    if (state == CallStateEnum.Success)
                                    {
                                        var repairNodeMethod = Service.CanCreateSlave ? new RepairNodeMethod(Type, methodDirectory.Parent.notNull().Name, methodDirectory.Name, rawAssembly, methodInfo, assemblyFile, methodNameFile) : null;
                                        Method newMethod = nodeMethod.CreateMethod<T>(methodInfo);
                                        Methods[methodAttribute.MethodIndex] = newMethod;
                                        nodeMethod.RepairNodeMethod = methodInfo;
                                        int snapshotMethodIndex = getSnapshotMethodIndex(method);
                                        if (snapshotMethodIndex >= 0) SnapshotMethods[snapshotMethodIndex].Method = newMethod;
                                        return repairNodeMethod;
                                    }
                                    throw new InvalidCastException(state.ToString());
                                }
                                nodeMethod = new ServerNodeMethod(Type, methodInfo, methodAttribute);
                                if (nodeMethod.CallType != CallTypeEnum.Unknown)
                                {
                                    var repairNodeMethod = Service.CanCreateSlave ? new RepairNodeMethod(Type, methodDirectory.Parent.notNull().Name, methodDirectory.Name, rawAssembly, methodInfo, assemblyFile, methodNameFile) : null;
                                    method = nodeMethod.CreateMethod<T>(methodInfo);
                                    Methods[methodAttribute.MethodIndex] = method;
                                    NodeMethods[methodAttribute.MethodIndex] = nodeMethod;
                                    nodeMethod.RepairNodeMethod = methodInfo;
                                    return repairNodeMethod;
                                }
                                throw new InvalidCastException(nodeMethod.CallState.ToString());
                            }
                            if (isMoveHistory) Directory.Move(methodDirectory.FullName, methodDirectory.FullName + ".bak");
                            return null;
                        }
                        throw new InvalidCastException(state.ToString());
                    }
                    return null;
                }
                throw new FileNotFoundException(methodNameFile.FullName);
            }
            throw new FileNotFoundException(assemblyFile.FullName);
        }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rawAssembly"></param>
        /// <param name="methodInfo">必须是静态方法，第一个参数必须是操作节点接口类型</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        internal async Task Repair<T>(byte[] rawAssembly, MethodInfo methodInfo, ServerMethodAttribute methodAttribute, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.MethodIndexOutOfRange;
            try
            {
                if ((uint)methodAttribute.MethodIndex < (uint)Methods.Length)
                {
                    var method = Methods[methodAttribute.MethodIndex];
                    if (method != null)
                    {
                        ServerNodeMethod nodeMethod = NodeMethods[methodAttribute.MethodIndex].notNull();
                        if (nodeMethod.RepairNodeMethod != methodInfo)
                        {
                            state = nodeMethod.CheckRepair(Type, methodInfo);
                            if (state == CallStateEnum.Success)
                            {
                                Service.CommandServerCallQueue.AppendWriteOnly(new RepairNodeMethodCallback(this, await writeRepairNodeMethodFile(rawAssembly, methodInfo, methodAttribute), null, nodeMethod.CreateMethod<T>(methodInfo), methodInfo, methodAttribute, callback));
                                state = CallStateEnum.Callbacked;
                            }
                        }
                        else state = CallStateEnum.Success;
                    }
                    else state = CallStateEnum.NotFoundMethod;
                }
            }
            finally
            {
                if (state != CallStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 写入修复节点方法问价
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="methodInfo"></param>
        /// <param name="methodAttribute"></param>
        /// <returns></returns>
        private async Task<RepairNodeMethod> writeRepairNodeMethodFile(byte[] rawAssembly, MethodInfo methodInfo, ServerMethodAttribute methodAttribute)
        {
            RepairNodeMethodDirectory repairNodeMethodDirectory = new RepairNodeMethodDirectory(Type.fullName().notNull(), methodAttribute.MethodIndex);
            DirectoryInfo typeDirectory = new DirectoryInfo(Path.Combine(Service.PersistenceDirectory.FullName, Service.Config.RepairNodeMethodDirectoryName, repairNodeMethodDirectory.NodeTypeHashCode.toHex()));
            await AutoCSer.Common.TryCreateDirectory(typeDirectory);
            DirectoryInfo methodDirectory = new DirectoryInfo(Path.Combine(typeDirectory.FullName, repairNodeMethodDirectory.RepairTime.toString() + repairNodeMethodDirectory.MethodIndex.toHex()));
            await AutoCSer.Common.TryCreateDirectory(methodDirectory);
            FileInfo assemblyFile = new FileInfo(Path.Combine(methodDirectory.FullName, Service.Config.RepairNodeMethodAssemblyFileName));
#if NetStandard21
            await using (FileStream assemblyStream = await AutoCSer.Common.CreateFileStream(assemblyFile.FullName, FileMode.Create, FileAccess.Write))
#else
            using (FileStream assemblyStream = await AutoCSer.Common.CreateFileStream(assemblyFile.FullName, FileMode.Create, FileAccess.Write))
#endif
            {
                await assemblyStream.WriteAsync(rawAssembly, 0, rawAssembly.Length);
            }
            FileInfo methodNameFile = new FileInfo(Path.Combine(methodDirectory.FullName, Service.Config.RepairNodeMethodNameFileName));
            await AutoCSer.Common.WriteFileAllText(methodNameFile.FullName, AutoCSer.JsonSerializer.Serialize(new RepairNodeMethodName(methodInfo, Type)), Encoding.UTF8);
            methodNameFile.LastWriteTimeUtc = assemblyFile.RefreshLastWriteTimeUtc();
            RepairNodeMethod repairNodeMethod = new RepairNodeMethod(Type, methodDirectory.Parent.notNull().Name, methodDirectory.FullName, rawAssembly, methodInfo, assemblyFile, methodNameFile);
            repairNodeMethod.RepairNodeMethodDirectory = repairNodeMethodDirectory;
            return repairNodeMethod;
        }
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rawAssembly"></param>
        /// <param name="methodInfo">必须是静态方法，第一个参数必须是操作节点接口类型</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal async Task Bind<T>(byte[] rawAssembly, MethodInfo methodInfo, ServerMethodAttribute methodAttribute, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.MethodIndexOutOfRange;
            try
            {
                if ((uint)methodAttribute.MethodIndex >= (uint)Methods.Length)
                {
                    Monitor.Enter(methodLock);
                    try
                    {
                        if ((uint)methodAttribute.MethodIndex >= (uint)Methods.Length)
                        {
                            var methods = AutoCSer.Common.GetCopyArray(Methods, methodAttribute.MethodIndex + 1);
                            NodeMethods = AutoCSer.Common.GetCopyArray(NodeMethods, methods.Length);
                            Methods = methods;
                        }
                    }
                    finally { Monitor.Exit(methodLock); }
                }
                var nodeMethod = NodeMethods[methodAttribute.MethodIndex];
                if(nodeMethod == null)
                {
                    nodeMethod = new ServerNodeMethod(Type, methodInfo, methodAttribute);
                    if (nodeMethod.CallType != CallTypeEnum.Unknown)
                    {
                        Service.CommandServerCallQueue.AppendWriteOnly(new RepairNodeMethodCallback(this, await writeRepairNodeMethodFile(rawAssembly, methodInfo, methodAttribute), nodeMethod, nodeMethod.CreateMethod<T>(methodInfo), methodInfo, methodAttribute, callback));
                        state = CallStateEnum.Callbacked;
                    }
                    else
                    {
                        AutoCSer.LogHelper.ErrorIgnoreException(nodeMethod.Error ?? $"{methodInfo.DeclaringType.notNull().fullName()}.{methodInfo.Name} 未知节点方法调用类型 {nodeMethod.CallState}");
                        state = nodeMethod.CallState;
                    }
                }
                else if (nodeMethod.Method.IsStatic)
                {
                    if (nodeMethod.RepairNodeMethod != methodInfo)
                    {
                        Service.CommandServerCallQueue.AppendWriteOnly(new RepairNodeMethodCallback(this, await writeRepairNodeMethodFile(rawAssembly, methodInfo, methodAttribute), nodeMethod, nodeMethod.CreateMethod<T>(methodInfo), methodInfo, methodAttribute, callback));
                        state = CallStateEnum.Callbacked;
                    }
                    else state = CallStateEnum.Success;
                }
                else state = CallStateEnum.BindMethodIndexUsed;
            }
            finally
            {
                if (state != CallStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 修复接口方法错误
        /// </summary>
        /// <param name="repairNodeMethod"></param>
        /// <param name="nodeMethod"></param>
        /// <param name="method"></param>
        /// <param name="methodInfo"></param>
        /// <param name="methodAttribute"></param>
        /// <param name="methodDirectory"></param>
        /// <param name="callback"></param>
#if NetStandard21
        internal void Repair(RepairNodeMethod repairNodeMethod, ServerNodeMethod? nodeMethod, Method method, MethodInfo methodInfo, ServerMethodAttribute methodAttribute, DirectoryInfo methodDirectory, CommandServerCallback<CallStateEnum> callback)
#else
        internal void Repair(RepairNodeMethod repairNodeMethod, ServerNodeMethod nodeMethod, Method method, MethodInfo methodInfo, ServerMethodAttribute methodAttribute, DirectoryInfo methodDirectory, CommandServerCallback<CallStateEnum> callback)
#endif
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                repairNodeMethod.RepairNodeMethodDirectory.Position = Service.RebuildPersistenceEndPosition;
                Directory.Move(methodDirectory.FullName, Path.Combine(methodDirectory.Parent.notNull().FullName, repairNodeMethod.MethodDirectoryName = repairNodeMethod.RepairNodeMethodDirectory.Position.toHex() + methodDirectory.Name));
                int methodIndex = methodAttribute.MethodIndex;
                var historyMethod = Methods[methodIndex];
                if (nodeMethod == null) nodeMethod = NodeMethods[methodIndex].notNull();
                else NodeMethods[methodIndex] = nodeMethod;
                Methods[methodIndex] = method;
                nodeMethod.RepairNodeMethod = methodInfo;
                if (historyMethod != null)
                {
                    int snapshotMethodIndex = getSnapshotMethodIndex(historyMethod);
                    if (snapshotMethodIndex >= 0) SnapshotMethods[snapshotMethodIndex].Method = method;
                }
                state = CallStateEnum.Success;
                Service.AppendLoadedRepairNodeMethod(repairNodeMethod);
            }
            finally { callback.Callback(state); }
        }
        /// <summary>
        /// 根据修复方法节点类型目录获取对应节点类型哈希值
        /// </summary>
        /// <param name="typeDirectory"></param>
        /// <param name="nodeTypeHashCode"></param>
        /// <returns></returns>
        internal unsafe static bool GetNodeTypeHashCode(DirectoryInfo typeDirectory, out ulong nodeTypeHashCode)
        {
            fixed (char* nameFixed = typeDirectory.Name)
            {
                if (NumberExtension.FromHex(nameFixed, out nodeTypeHashCode)) return true;
            }
            return false;
        }
        /// <summary>
        /// 根据修复方法目录获取对应持久化位置
        /// </summary>
        /// <param name="methodDirectory"></param>
        /// <returns></returns>
        internal unsafe static ulong GetMethodDirectoryPosition(DirectoryInfo methodDirectory)
        {
            ulong position;
            fixed (char* nameFixed = methodDirectory.Name)
            {
                if (NumberExtension.FromHex(nameFixed, out position)) return position;
            }
            return ulong.MaxValue;
        }
        /// <summary>
        /// 根据修复方法目录获取相关对应信息
        /// </summary>
        /// <param name="methodDirectory"></param>
        /// <param name="repairNodeMethodDirectoryKey"></param>
        /// <returns></returns>
        internal unsafe static bool GetMethodDirectory(DirectoryInfo methodDirectory, ref RepairNodeMethodDirectory repairNodeMethodDirectoryKey)
        {
            fixed (char* nameFixed = methodDirectory.Name)
            {
                if (NumberExtension.FromHex(nameFixed, out repairNodeMethodDirectoryKey.Position))
                {
                    char* start = nameFixed + 16, end = start + 14;
                    ulong repairTime = 0;
                    do
                    {
                        ulong code = (ulong)(*start - '0');
                        if (code < 10) repairTime = repairTime * 10 + code;
                        else return false;
                    }
                    while (++start != end);
                    repairNodeMethodDirectoryKey.RepairTime = repairTime;
                    return NumberExtension.FromHex(end, out repairNodeMethodDirectoryKey.MethodIndex);
                }
            }
            return false;
        }
        /// <summary>
        /// 获取修复方法信息
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="methodName"></param>
        /// <param name="method"></param>
        /// <returns></returns>
#if NetStandard21
        internal static CallStateEnum GetRepairMethod(ref byte[] rawAssembly, ref RepairNodeMethodName methodName, out MethodInfo? method)
#else
        internal static CallStateEnum GetRepairMethod(ref byte[] rawAssembly, ref RepairNodeMethodName methodName, out MethodInfo method)
#endif
        {
            Assembly assembly = LoadAssemblyCache.Load(ref rawAssembly);
            var type = assembly.GetType(methodName.DeclaringTypeFullName);
            if (type != null)
            {
                method = type.GetMethod(methodName.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                if (method != null) return CallStateEnum.Success;
                return CallStateEnum.NotFoundRepairMethod;
            }
            method = null;
            return CallStateEnum.NotFoundRepairMethodDeclaringType;
        }
        /// <summary>
        /// 获取修复方法信息
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="methodName"></param>
        /// <param name="method"></param>
        /// <param name="methodAttribute"></param>
        /// <returns></returns>
#if NetStandard21
        internal static CallStateEnum GetRepairMethod(byte[] rawAssembly, ref RepairNodeMethodName methodName, out MethodInfo? method, out ServerMethodAttribute? methodAttribute)
#else
        internal static CallStateEnum GetRepairMethod(byte[] rawAssembly, ref RepairNodeMethodName methodName, out MethodInfo method, out ServerMethodAttribute methodAttribute)
#endif
        {
            CallStateEnum state = GetRepairMethod(ref rawAssembly, ref methodName, out method);
            if (state == CallStateEnum.Success)
            {
                method = method.notNull();
                if (method.IsStatic)
                {
                    if (!method.IsGenericMethodDefinition)
                    {
                        methodAttribute = method.GetCustomAttribute<ServerMethodAttribute>(false) ?? ServerMethodAttribute.Default;
                        return methodAttribute.MethodIndex >= 0 ? CallStateEnum.Success : CallStateEnum.MethodIndexOutOfRange;
                    }
                    state = CallStateEnum.RepairMethodIsGenericMethodDefinition;
                }
                else state = CallStateEnum.RepairMethodNotStatic;
            }
            methodAttribute = null;
            return state;
        }
        /// <summary>
        /// 快照方法排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int snapshotMethodSort(SnapshotMethod left, SnapshotMethod right)
        {
            return left.NodeMethod.MethodAttribute.SnapshotMethodSort - right.NodeMethod.MethodAttribute.SnapshotMethodSort;
        }
        /// <summary>
        /// 快照方法排序
        /// </summary>
        internal static readonly Func<SnapshotMethod, SnapshotMethod, int> SnapshotMethodSort = snapshotMethodSort;

        /// <summary>
        /// 服务端节点方法构造函数参数
        /// </summary>
        internal static readonly Type[] MethodConstructorParameterTypes = new Type[] { typeof(int), typeof(int), typeof(MethodFlagsEnum) };
        /// <summary>
        /// 服务端节点方法构造函数参数
        /// </summary>
        internal static readonly Type[] CallTypeMethodConstructorParameterTypes = new Type[] { typeof(int), typeof(int), typeof(CallTypeEnum), typeof(MethodFlagsEnum) };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo CallMethod = typeof(CallMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallMethod.Call), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法参数
        /// </summary>
        internal static readonly Type[] CallMethodParameterTypes = new Type[] { typeof(ServerNode), typeof(CommandServerCallback<CallStateEnum>).MakeByRefType() };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo CallOutputMethod = typeof(CallOutputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.CallOutput), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法参数
        /// </summary>
        internal static readonly Type[] CallOutputMethodParameterTypes = new Type[] { typeof(ServerNode), typeof(CommandServerCallback<ResponseParameter>).MakeByRefType() };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo CallOutputCallBeforePersistenceMethod = typeof(CallOutputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.CallBeforePersistence), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo CallOutputCallOutputBeforePersistenceMethod = typeof(CallOutputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.CallOutputBeforePersistence), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法参数
        /// </summary>
        internal static readonly Type[] CallOutputBeforePersistenceMethodParameterTypes = new Type[] { typeof(ServerNode) };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo CallInputMethod = typeof(CallInputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethod.CallInput), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法参数
        /// </summary>
        internal static readonly Type[] CallInputMethodParameterTypes = new Type[] { typeof(CallInputMethodParameter) };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo CallInputOutputMethod = typeof(CallInputOutputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod.CallInputOutput), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法参数
        /// </summary>
        internal static readonly Type[] CallInputOutputMethodParameterTypes = new Type[] { typeof(CallInputOutputMethodParameter) };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo CallInputOutputCallOutputBeforePersistenceMethod = typeof(CallInputOutputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod.CallOutputBeforePersistence), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo CallInputOutputCallBeforePersistenceMethod = typeof(CallInputOutputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod.CallBeforePersistence), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo SendOnlyMethod = typeof(SendOnlyMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SendOnlyMethod.SendOnly), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法参数
        /// </summary>
        internal static readonly Type[] SendOnlyMethodParameterTypes = new Type[] { typeof(SendOnlyMethodParameter) };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo KeepCallbackMethod = typeof(KeepCallbackMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackMethod.KeepCallback), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法参数
        /// </summary>
        internal static readonly Type[] KeepCallbackMethodParameterTypes = new Type[] { typeof(ServerNode), typeof(CommandServerKeepCallback<KeepCallbackResponseParameter>).MakeByRefType() };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo InputKeepCallbackMethod = typeof(InputKeepCallbackMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.InputKeepCallbackMethod.InputKeepCallback), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用节点方法参数
        /// </summary>
        internal static readonly Type[] InputKeepCallbackMethodParameterTypes = new Type[] { typeof(InputKeepCallbackMethodParameter) };
        ///// <summary>
        ///// 获取服务端节点
        ///// </summary>
        //internal static readonly Func<MethodParameter, ServerNode> MethodParameterGetNode = MethodParameter.GetNode;
        /// <summary>
        /// 调用回调
        /// </summary>
        internal static readonly CallMethod.CallbackDelegate CallMethodCallback = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallMethod.Callback;
        /// <summary>
        /// 调用回调
        /// </summary>
        internal static readonly MethodInfo CallOutputMethodCallbackMethod = typeof(CallOutputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.Callback), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <param name="value"></param>
        /// <param name="callback"></param>
#if NetStandard21
        internal delegate void CallOutputMethodCallbackResponseParameterDelegate(ResponseParameter value, ref CommandServerCallback<ResponseParameter>? callback);
#else
        internal delegate void CallOutputMethodCallbackResponseParameterDelegate(ResponseParameter value, ref CommandServerCallback<ResponseParameter> callback);
#endif
        /// <summary>
        /// 调用回调
        /// </summary>
        internal static readonly CallOutputMethodCallbackResponseParameterDelegate CallOutputMethodCallbackResponseParameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.CallbackResponseParameter;
        /// <summary>
        /// 获取持久化检查方法返回值
        /// </summary>
        internal static readonly MethodInfo CallOutputMethodGetBeforePersistenceResponseParameterMethod = typeof(CallOutputMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.GetBeforePersistenceResponseParameter), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用回调
        /// </summary>
        internal static readonly Action<CallInputMethodParameter> CallInputMethodParameterCallback = CallInputMethodParameter.Callback;
        /// <summary>
        /// 调用回调
        /// </summary>
        internal static readonly MethodInfo CallInputOutputMethodParameterCallbackMethod = typeof(CallInputOutputMethodParameter).GetMethod(nameof(CallInputOutputMethodParameter.Callback), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 调用回调
        /// </summary>
        internal static readonly Action<CallInputOutputMethodParameter, ResponseParameter> CallInputOutputMethodParameterCallbackResponseParameter = CallInputOutputMethodParameter.CallbackResponseParameter;
        /// <summary>
        /// 获取持久化检查方法返回值
        /// </summary>
        internal static readonly MethodInfo CallInputOutputMethodParameterGetBeforePersistenceResponseParameterMethod = typeof(CallInputOutputMethodParameter).GetMethod(nameof(CallInputOutputMethodParameter.GetBeforePersistenceResponseParameter), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 枚举回调
        /// </summary>
        internal static readonly MethodInfo KeepCallbackMethodEnumerableCallbackMethod = typeof(KeepCallbackMethod).GetMethod(nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackMethod.EnumerableCallback), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();
        /// <summary>
        /// 枚举回调
        /// </summary>
        internal static readonly MethodInfo InputKeepCallbackMethodParameterEnumerableCallbackMethod = typeof(InputKeepCallbackMethodParameter).GetMethod(nameof(InputKeepCallbackMethodParameter.EnumerableCallback), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).notNull();

        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal static readonly Action<MethodParameterCreator, int> MethodParameterCreatorCreateCallMethodParameter = MethodParameterCreator.CreateCallMethodParameter;
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal static readonly Action<MethodParameterCreator, int> MethodParameterCreatorCreateCallOutputMethodParameter = MethodParameterCreator.CreateCallOutputMethodParameter;
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
#if NetStandard21
        internal static readonly Action<MethodParameterCreator, int, CommandServerCallback<ResponseParameter>?> MethodParameterCreatorCreateCallOutputCallbackMethodParameter = MethodParameterCreator.CreateCallOutputCallbackMethodParameter;
#else
        internal static readonly Action<MethodParameterCreator, int, CommandServerCallback<ResponseParameter>> MethodParameterCreatorCreateCallOutputCallbackMethodParameter = MethodParameterCreator.CreateCallOutputCallbackMethodParameter;
#endif
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal static readonly Action<MethodParameterCreator, int> MethodParameterCreatorCreateKeepCallbackMethodParameter = MethodParameterCreator.CreateKeepCallbackMethodParameter;
    }
    /// <summary>
    /// 生成服务端节点
    /// </summary>
    /// <typeparam name="T">节点接口类型</typeparam>
    internal static class ServerNodeCreator<T>
    {
        /// <summary>
        /// 节点生成错误
        /// </summary>
#if NetStandard21
        private static Exception? creatorException;
#else
        private static Exception creatorException;
#endif
        /// <summary>
        /// 节点生成提示信息
        /// </summary>
#if NetStandard21
        private static string[]? creatorMessages;
#else
        private static string[] creatorMessages;
#endif
        /// <summary>
        /// 节点方法集合
        /// </summary>
#if NetStandard21
        private static Method?[] methods;
#else
        private static Method[] methods;
#endif
        /// <summary>
        /// 节点方法信息集合
        /// </summary>
#if NetStandard21
        private static ServerNodeMethod?[] nodeMethods;
#else
        private static ServerNodeMethod[] nodeMethods;
#endif
        /// <summary>
        /// 快照方法集合
        /// </summary>
        private static SnapshotMethod[] snapshotMethods;
        /// <summary>
        /// 生成服务端节点
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        internal static ServerNodeCreator Create(StreamPersistenceMemoryDatabaseServiceBase service)
        {
            if (creatorException == null)
            {
                if (creatorMessages != null && creatorMessages.Length != 0)
                {
                    AutoCSer.LogHelper.DebugIgnoreException($"{typeof(T).fullName()} 节点服务端生成警告\r\n{string.Join("\r\n", creatorMessages)}");
                    creatorMessages = null;
                }
                ServerNodeCreator nodeCreator = new ServerNodeCreator(service, typeof(T), methods.copy(), nodeMethods.copy(), snapshotMethods);
                nodeCreator.LoadRepairNodeMethod<T>();
                return nodeCreator;
            }
            throw creatorException;
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal static readonly Func<ServerNode<T>, T> MethodParameterCreator;

        static ServerNodeCreator()
        {
            Type type = typeof(T);
            NodeType nodeType = default(NodeType);
            methods = EmptyArray<Method>.Array;
            nodeMethods = EmptyArray<ServerNodeMethod>.Array;
            snapshotMethods = EmptyArray<SnapshotMethod>.Array;
            try
            {
                nodeType = new NodeType(type);
                if (nodeType.Error != null)
                {
                    creatorException = new Exception($"{type.fullName()} 节点服务端生成失败 {nodeType.Error}");
                    return;
                }
                var nodeMethods = nodeType.Methods;
#if NetStandard21
                Method?[] methods = new Method[nodeMethods.Length];
#else
                Method[] methods = new Method[nodeMethods.Length];
#endif
                LeftArray<SnapshotMethod> snapshotMethods = new LeftArray<SnapshotMethod>(0);
                int methodIndex = 0;
                foreach (var nodeMethod in nodeMethods)
                {
                    if (nodeMethod != null)
                    {
                        Method method = nodeMethod.CreateMethod<T>(null);
                        methods[methodIndex] = method;
                        if (nodeMethod.MethodAttribute.SnapshotMethodSort != 0)
                        {
                            if (nodeMethod.ParameterCount == 1)
                            {
                                var snapshotType = nodeMethod.Parameters[nodeMethod.ParameterStartIndex].ParameterType;
                                int snapshotMethodIndex = 0;
                                foreach (SnapshotMethod snapshotMethod in snapshotMethods)
                                {
                                    if (snapshotMethod.Type == snapshotType)
                                    {
                                        snapshotType = null;
                                        nodeType.Messages.Add($"{type.fullName()} 节点快照方法 {nodeMethod.Method.Name} 冲突 {snapshotMethod.NodeMethod.Method.Name}");
                                        break;
                                    }
                                    ++snapshotMethodIndex;
                                }
                                if (snapshotType != null) snapshotMethods.Add(new SnapshotMethod(snapshotType, method, nodeMethod));
                            }
                            else nodeType.Messages.Add($"{type.fullName()} 节点快照方法 {nodeMethod.Method.Name} 有效输入参数数量 {nodeMethod.ParameterCount} 必须为 1");
                        }
                    }
                    ++methodIndex;
                }
                if (nodeType.NodeAttribute.IsMethodParameterCreator)
                {
                    Type methodParameterCreatorType = typeof(MethodParameterCreator<>).MakeGenericType(type);
                    Type[] methodParameterCreatorConstructorParameterTypes = new Type[] { typeof(ServerNode<>).MakeGenericType(type) };
                    TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator." + type.FullName, TypeAttributes.Class | TypeAttributes.Sealed, methodParameterCreatorType, new Type[] { type });
                    #region 构造函数
                    ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, methodParameterCreatorConstructorParameterTypes);
                    ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                    #region base(node)
                    constructorGenerator.Emit(OpCodes.Ldarg_0);
                    constructorGenerator.Emit(OpCodes.Ldarg_1);
                    constructorGenerator.Emit(OpCodes.Call, methodParameterCreatorType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, methodParameterCreatorConstructorParameterTypes, null).notNull());
                    constructorGenerator.Emit(OpCodes.Ret);
                    #endregion
                    #endregion
                    methodIndex = 0;
                    foreach (var nodeMethod in nodeMethods)
                    {
                        if (nodeMethod != null)
                        {
                            MethodBuilder methodBuilder = typeBuilder.DefineMethod(nodeMethod.Method.Name, MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, nodeMethod.Method.ReturnType, nodeMethod.Parameters.getArray(parameter => parameter.ParameterType));
                            typeBuilder.DefineMethodOverride(methodBuilder, nodeMethod.Method);
                            ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                            #region MethodParameterCreator.CreateCallInputOutputMethodParameter(this, 0, new parameter { x = x });
                            methodGenerator.Emit(OpCodes.Ldarg_0);
                            methodGenerator.int32(methodIndex);
                            if (nodeMethod.InputParameterType != null)
                            {
                                LocalBuilder inputParameterLocalBuilder = methodGenerator.DeclareLocal(nodeMethod.InputParameterType.Type);
                                if (nodeMethod.InputParameterType.IsInitobj)
                                {
                                    methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Initobj, nodeMethod.InputParameterType.Type);
                                }
                                nodeMethod.SetInputParameter(methodGenerator, inputParameterLocalBuilder);
                                methodGenerator.Emit(OpCodes.Ldloc_S, inputParameterLocalBuilder);
                            }
                            switch (nodeMethod.CallType)
                            {
                                case CallTypeEnum.CallInput:
                                    methodGenerator.call(StructGenericType.Get(nodeMethod.InputParameterType.notNull().Type).MethodParameterCreatorCreateCallInputMethodParameterDelegate.Method);
                                    break;
                                case CallTypeEnum.CallInputOutput:
                                    methodGenerator.call(StructGenericType.Get(nodeMethod.InputParameterType.notNull().Type).MethodParameterCreatorCreateCallInputOutputMethodParameterDelegate.Method);
                                    break;
                                case CallTypeEnum.InputCallback:
                                    methodGenerator.ldarg(nodeMethod.ParameterEndIndex + 1);
                                    methodGenerator.call(GenericType.Get(nodeMethod.ReturnValueType).MethodCallbackGetCallbackDelegate.Method);
                                    methodGenerator.call(StructGenericType.Get(nodeMethod.InputParameterType.notNull().Type).MethodParameterCreatorCreateCallInputOutputCallbackMethodParameterDelegate.Method);
                                    break;
                                case CallTypeEnum.SendOnly:
                                    methodGenerator.call(StructGenericType.Get(nodeMethod.InputParameterType.notNull().Type).MethodParameterCreatorCreateSendOnlyMethodParameterDelegate.Method);
                                    break;
                                case CallTypeEnum.InputKeepCallback:
                                case CallTypeEnum.InputEnumerable:
                                    methodGenerator.call(StructGenericType.Get(nodeMethod.InputParameterType.notNull().Type).MethodParameterCreatorCreateInputKeepCallbackMethodParameterDelegate.Method);
                                    break;

                                case CallTypeEnum.Call:
                                    methodGenerator.call(ServerNodeCreator.MethodParameterCreatorCreateCallMethodParameter.Method);
                                    break;
                                case CallTypeEnum.CallOutput:
                                    methodGenerator.call(ServerNodeCreator.MethodParameterCreatorCreateCallOutputMethodParameter.Method);
                                    break;
                                case CallTypeEnum.Callback:
                                    methodGenerator.ldarg(nodeMethod.ParameterEndIndex + 1);
                                    methodGenerator.call(GenericType.Get(nodeMethod.ReturnValueType).MethodCallbackGetCallbackDelegate.Method);
                                    methodGenerator.call(ServerNodeCreator.MethodParameterCreatorCreateCallOutputCallbackMethodParameter.Method);
                                    break;
                                case CallTypeEnum.KeepCallback:
                                case CallTypeEnum.Enumerable:
                                    methodGenerator.call(ServerNodeCreator.MethodParameterCreatorCreateKeepCallbackMethodParameter.Method);
                                    break;
                            }
                            #endregion
                            #region return default
                            if (nodeMethod.Method.ReturnType != typeof(void))
                            {
                                LocalBuilder returnLocalBuilder = methodGenerator.DeclareLocal(nodeMethod.Method.ReturnType);
                                if (DynamicArray.IsClearArray(nodeMethod.Method.ReturnType))
                                {
                                    methodGenerator.Emit(OpCodes.Ldloca_S, returnLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Initobj, nodeMethod.Method.ReturnType);
                                }
                                methodGenerator.Emit(OpCodes.Ldloc_S, returnLocalBuilder);
                            }
                            methodGenerator.Emit(OpCodes.Ret);
                            #endregion
                        }
                        ++methodIndex;
                    }
                    Type creatorType = typeBuilder.CreateType();
                    DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "CallConstructor", type, methodParameterCreatorConstructorParameterTypes, creatorType, true);
                    ILGenerator callConstructorGenerator = dynamicMethod.GetILGenerator();
                    callConstructorGenerator.Emit(OpCodes.Ldarg_0);
                    callConstructorGenerator.Emit(OpCodes.Newobj, creatorType.GetConstructor(methodParameterCreatorConstructorParameterTypes).notNull());
                    callConstructorGenerator.Emit(OpCodes.Ret);
                    MethodParameterCreator = (Func<ServerNode<T>, T>)dynamicMethod.CreateDelegate(typeof(Func<ServerNode<T>, T>));
                }
                ServerNodeCreator<T>.methods = methods;
                ServerNodeCreator<T>.nodeMethods = nodeMethods;
                snapshotMethods.Sort(ServerNodeCreator.SnapshotMethodSort);
                ServerNodeCreator<T>.snapshotMethods = snapshotMethods.ToArray();
                //ServerNodeCreator<T>.snapshotType = snapshotType;
            }
            catch (Exception exception)
            {
                creatorException = new Exception($"{type.fullName()} 节点服务端生成失败", exception);
                if (nodeType.Messages.Length != 0) creatorMessages = nodeType.Messages.ToArray();
            }
        }
#if DEBUG && NetStandard21
        public interface IDictionary<KT, VT>
            where KT : IEquatable<KT>
        {
            void Call();
            void Add(KT key, VT value);
            void Set(KT key, VT value);
            VT? Get(KT key);
            VT? CallOutput();
            void Callback(MethodCallback<VT?> callback);
            void GetCallback(KT key, MethodCallback<VT?> callback);
            void KeepCallback(MethodKeepCallback<VT> callback);
            void GetKeepCallback(KT key, MethodKeepCallback<VT> callback);
        }
        public sealed class DictionaryMethodParameter<KT, VT> : MethodParameterCreator<IDictionary<KT, VT>>, IDictionary<KT, VT>
            where KT : IEquatable<KT>
        {
            public DictionaryMethodParameter(ServerNode<IDictionary<KT, VT>> node) : base(node) { }
            public void Call()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateCallMethodParameter(this, 0);
            }
            public void Add(KT key, VT value)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateCallInputMethodParameter(this, 0, new Dictionary<KT, VT>.p0 { key = key, value = value });
            }
            public void Set(KT key, VT value)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateCallInputMethodParameter(this, 0, new Dictionary<KT, VT>.p0 { key = key, value = value });
            }
            public VT? Get(KT key)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateCallInputOutputMethodParameter(this, 0, new Dictionary<KT, VT>.p0 { key = key });
                return default(VT);
            }
            public VT? CallOutput()
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateCallOutputMethodParameter(this, 0);
                return default(VT);
            }
            public void Callback(MethodCallback<VT?> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateCallOutputMethodParameter(this, 0);
            }
            public void GetCallback(KT key, MethodCallback<VT?> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateCallInputOutputMethodParameter(this, 0, new Dictionary<KT, VT>.p0 { key = key });
            }
            public void KeepCallback(MethodKeepCallback<VT> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateKeepCallbackMethodParameter(this, 0);
            }
            public void GetKeepCallback(KT key, MethodKeepCallback<VT> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.CreateInputKeepCallbackMethodParameter(this, 0, new Dictionary<KT, VT>.p0 { key = key });
            }
        }
        public sealed class Dictionary<KT, VT> : IDictionary<KT, VT>
            where KT : IEquatable<KT>
        {
            public void Call() { }
            public void Add(KT key, VT value) { }
            public void Set(KT key, VT value) { }
            public VT? Get(KT key) { return default(VT); }
            public VT? CallOutput() { return default(VT); }
            public void Callback(MethodCallback<VT?> callback) { callback.Callback(default(VT)); }
            public void GetCallback(KT key, MethodCallback<VT?> callback) { callback.Callback(default(VT)); }
            public void KeepCallback(MethodKeepCallback<VT> callback) { callback.CallbackCancelKeep(CallStateEnum.Unknown); }
            public void GetKeepCallback(KT key, MethodKeepCallback<VT> callback) { callback.CallbackCancelKeep(CallStateEnum.Unknown); }

            public struct p0
            {
                public KT key;
                public VT value;
            }
            public struct p1
            {
                public KT key;
            }
            public sealed class m0 : CallInputMethod<p0>
            {
                public m0() : base(0, int.MinValue, 0) { }
                public override void CallInput(CallInputMethodParameter methodParameter)
                {
                    p0 parameter = CallInputMethodParameter<p0>.GetParameter(((CallInputMethodParameter<p0>)methodParameter));
                    MethodParameter.GetNodeTarget<IDictionary<KT, VT>>(methodParameter).Add(parameter.key, parameter.value);
                    CallInputMethodParameter.Callback(methodParameter);
                }
            }
            public sealed class m1 : CallInputOutputMethod<p1>
            {
                public m1() : base(0, int.MinValue, 0) { }
                public override void CallInputOutput(CallInputOutputMethodParameter methodParameter)
                {
                    p1 parameter = CallInputOutputMethodParameter<p1>.GetParameter(((CallInputOutputMethodParameter<p1>)methodParameter));
                    CallInputOutputMethodParameter.Callback(methodParameter, MethodParameter.GetNodeTarget<IDictionary<KT, VT>>(methodParameter).Get(parameter.key));
                }
                public override ValueResult<ResponseParameter> CallOutputBeforePersistence(CallInputOutputMethodParameter methodParameter)
                {
                    p1 parameter = CallInputOutputMethodParameter<p1>.GetParameter(((CallInputOutputMethodParameter<p1>)methodParameter));
                    return CallInputOutputMethodParameter.GetBeforePersistenceResponseParameter(methodParameter, (ValueResult<VT?>)MethodParameter.GetNodeTarget<IDictionary<KT, VT>>(methodParameter).Get(parameter.key));
                }
                public override bool CallBeforePersistence(CallInputOutputMethodParameter methodParameter)
                {
                    p1 parameter = CallInputOutputMethodParameter<p1>.GetParameter(((CallInputOutputMethodParameter<p1>)methodParameter));
#pragma warning disable CS8605
                    return (bool)(object?)MethodParameter.GetNodeTarget<IDictionary<KT, VT>>(methodParameter).Get(parameter.key);
#pragma warning restore CS8605
                }
            }
            public sealed class m2 : CallMethod
            {
                public m2() : base(0, int.MinValue, 0) { }
                public override void Call(ServerNode node, ref CommandServerCallback<CallStateEnum>? callback)
                {
                    ServerNode<IDictionary<KT, VT>>.GetTarget(((ServerNode<IDictionary<KT, VT>>)node)).Call();
                    CallMethod.Callback(ref callback);
                }
            }
            public sealed class m3 : CallOutputMethod
            {
                public m3() : base(0, int.MinValue, 0) { }
                public override void CallOutput(ServerNode node, ref CommandServerCallback<ResponseParameter>? callback)
                {
                    CallOutputMethod.Callback(ServerNode<IDictionary<KT, VT>>.GetTarget(((ServerNode<IDictionary<KT, VT>>)node)).CallOutput(), ref callback, 0);
                }
                public override ValueResult<ResponseParameter> CallOutputBeforePersistence(ServerNode node)
                {
                    return CallOutputMethod.GetBeforePersistenceResponseParameter((ValueResult<VT?>)ServerNode<IDictionary<KT, VT>>.GetTarget(((ServerNode<IDictionary<KT, VT>>)node)).CallOutput(), 0);
                }
                public override bool CallBeforePersistence(ServerNode node)
                {
#pragma warning disable CS8605
                    return (bool)(object?)ServerNode<IDictionary<KT, VT>>.GetTarget(((ServerNode<IDictionary<KT, VT>>)node)).CallOutput();
#pragma warning restore CS8605
                }
            }
            public sealed class m4 : CallOutputMethod
            {
                public m4() : base(0, int.MinValue, CallTypeEnum.Callback, 0) { }
                public override void CallOutput(ServerNode node, ref CommandServerCallback<ResponseParameter>? callback)
                {
                    ServerNode<IDictionary<KT, VT>>.GetTarget(((ServerNode<IDictionary<KT, VT>>)node)).Callback(MethodCallback<VT?>.Create(ref callback, 0));
                }
            }
            public sealed class m5 : CallInputOutputMethod<p1>
            {
                public m5() : base(0, int.MinValue, CallTypeEnum.InputCallback, 0) { }
                public override void CallInputOutput(CallInputOutputMethodParameter methodParameter)
                {
                    p1 parameter = CallInputOutputMethodParameter<p1>.GetParameter(((CallInputOutputMethodParameter<p1>)methodParameter));
                    MethodParameter.GetNodeTarget<IDictionary<KT, VT>>(methodParameter).GetCallback(parameter.key, MethodCallback<VT?>.Create(methodParameter));
                }
            }
            public sealed class m6 : SendOnlyMethod<p0>
            {
                public m6() : base(0, int.MinValue, 0) { }
                public override void SendOnly(SendOnlyMethodParameter methodParameter)
                {
                    p0 parameter = SendOnlyMethodParameter<p0>.GetParameter(((SendOnlyMethodParameter<p0>)methodParameter));
                    MethodParameter.GetNodeTarget<IDictionary<KT, VT>>(methodParameter).Add(parameter.key, parameter.value);
                }
            }
            public sealed class m7 : KeepCallbackMethod
            {
                public m7() : base(0, int.MinValue, 0) { }
                public override void KeepCallback(ServerNode node, ref CommandServerKeepCallback<KeepCallbackResponseParameter>? callback)
                {
                    ServerNode<IDictionary<KT, VT>>.GetTarget(((ServerNode<IDictionary<KT, VT>>)node)).KeepCallback(MethodKeepCallback<VT>.Create(ref callback, 0));
                }
            }
            public sealed class m8 : InputKeepCallbackMethod<p1>
            {
                public m8() : base(0, int.MinValue, 0) { }
                public override void InputKeepCallback(InputKeepCallbackMethodParameter methodParameter)
                {
                    p1 parameter = InputKeepCallbackMethodParameter<p1>.GetParameter(((InputKeepCallbackMethodParameter<p1>)methodParameter));
                    MethodParameter.GetNodeTarget<IDictionary<KT, VT>>(methodParameter).GetKeepCallback(parameter.key, MethodKeepCallback<VT>.Create(methodParameter));
                }
            }
        }
#endif
    }
}
