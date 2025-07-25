﻿using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using AutoCSer.Reflection;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 修复节点方法信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    internal sealed class RepairNodeMethod : AutoCSer.Threading.Link<RepairNodeMethod>
    {
        /// <summary>
        /// 服务端节点接口类型
        /// </summary>
        internal AutoCSer.Reflection.RemoteType RemoteType;
        /// <summary>
        /// 修复节点方法类型保存目录名称
        /// </summary>
        internal string TypeDirectoryName;
        /// <summary>
        /// 修复节点方法保存目录名称
        /// </summary>
        internal string MethodDirectoryName;
        /// <summary>
        /// 程序集文件数据
        /// </summary>
        internal SubArray<byte> RawAssembly;
        /// <summary>
        /// 修复方法名称
        /// </summary>
        internal RepairNodeMethodName MethodName;
        /// <summary>
        /// 修复方法目录信息
        /// </summary>
        internal RepairNodeMethodDirectory RepairNodeMethodDirectory;
        /// <summary>
        /// 修复方法文件信息
        /// </summary>
        internal RepairNodeMethodFile RepairNodeMethodFile;
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        internal CallStateEnum CallState;
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        internal RepairNodeMethod()
        {
#if NetStandard21
            TypeDirectoryName = MethodDirectoryName = string.Empty;
#endif
        }
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        /// <param name="callState">调用状态</param>
        internal RepairNodeMethod(CallStateEnum callState)
        {
            CallState = callState;
#if NetStandard21
            TypeDirectoryName = MethodDirectoryName = string.Empty;
#endif
        }
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        /// <param name="type">节点类型</param>
        /// <param name="typeDirectoryName">修复节点方法类型保存目录名称</param>
        /// <param name="methodDirectoryName">修复节点方法保存目录名称</param>
        /// <param name="rawAssembly">Assembly file data
        /// 程序集文件数据</param>
        /// <param name="method">修复方法信息</param>
        /// <param name="assemblyFile">程序集文件信息</param>
        /// <param name="methodNameFile">修复方法名称文件信息</param>
        internal RepairNodeMethod(Type type, string typeDirectoryName, string methodDirectoryName, byte[] rawAssembly, MethodInfo method, FileInfo assemblyFile, FileInfo methodNameFile)
        {
            RemoteType = new AutoCSer.Reflection.RemoteType(type);
            TypeDirectoryName = typeDirectoryName;
            MethodDirectoryName = methodDirectoryName;
            RawAssembly = rawAssembly;
            MethodName = new RepairNodeMethodName(method);
            RepairNodeMethodFile.Set(assemblyFile, methodNameFile);
            CallState = CallStateEnum.Success;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal unsafe void Serialize(AutoCSer.BinarySerializer serializer)
        {
            UnmanagedStream stream = serializer.Stream;
            stream.Write((int)CallState);
            if (CallState == CallStateEnum.Success)
            {
                int index = serializer.SerializeBufferStart();
                if (index >= 0)
                {
                    serializer.SerializeOnly(RemoteType.AssemblyName);
                    serializer.SerializeOnly(RemoteType.Name);
                    serializer.SerializeOnly(TypeDirectoryName);
                    serializer.SerializeOnly(MethodDirectoryName);
                    serializer.SerializeOnly(MethodName.DeclaringTypeFullName);
                    serializer.SerializeOnly(MethodName.Name);
                    serializer.SerializeBuffer(ref RawAssembly);
                    byte* data = serializer.Stream.GetBeforeMove(sizeof(DateTime) + sizeof(ulong) * 3 + sizeof(uint));
                    if (data != null)
                    {
                        *(DateTime*)data = RepairNodeMethodFile.LastWriteTime;
                        *(ulong*)(data + sizeof(DateTime)) = RepairNodeMethodDirectory.NodeTypeHashCode;
                        *(ulong*)(data + (sizeof(DateTime) + sizeof(ulong))) = RepairNodeMethodDirectory.Position;
                        *(ulong*)(data + (sizeof(DateTime) + sizeof(ulong) * 2)) = RepairNodeMethodDirectory.RepairTime;
                        *(uint*)(data + (sizeof(DateTime) + sizeof(ulong) * 3)) = RepairNodeMethodDirectory.MethodIndex;
                        serializer.SerializeBufferEnd(index);
                    }
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="state"></param>
        internal unsafe void Deserialize(AutoCSer.BinaryDeserializer deserializer, int state)
        {
            CallState = (CallStateEnum)(byte)state;
            if (CallState == CallStateEnum.Success)
            {
                byte* end = deserializer.DeserializeBufferStart();
                if (end != null)
                {
                    var assemblyName = default(string);
                    var typeName = default(string);
                    var typeDirectoryName = default(string);
                    var methodDirectoryName = default(string);
                    var methodTypeName = default(string);
                    var methodName = default(string);
                    if (deserializer.DeserializeOnly(ref assemblyName) && deserializer.DeserializeOnly(ref typeName) &&
                        deserializer.DeserializeOnly(ref typeDirectoryName) && deserializer.DeserializeOnly(ref methodDirectoryName)
                        && deserializer.DeserializeOnly(ref methodTypeName) && deserializer.DeserializeOnly(ref methodName))
                    {
                        if (assemblyName != null && typeName != null && typeDirectoryName != null && methodDirectoryName != null && methodTypeName != null && methodName != null
                            && deserializer.DeserializeBuffer(ref RawAssembly, true))
                        {
                            byte* data = deserializer.GetBeforeMove(sizeof(DateTime) + sizeof(ulong) * 3 + sizeof(uint));
                            if (data != null && deserializer.DeserializeBufferEnd(end))
                            {
                                RepairNodeMethodFile.LastWriteTime = *(DateTime*)data;
                                RepairNodeMethodDirectory.NodeTypeHashCode = *(ulong*)(data + sizeof(DateTime));
                                RepairNodeMethodDirectory.Position = *(ulong*)(data + (sizeof(DateTime) + sizeof(ulong)));
                                RepairNodeMethodDirectory.RepairTime = *(ulong*)(data + (sizeof(DateTime) + sizeof(ulong) * 2));
                                RepairNodeMethodDirectory.MethodIndex = *(uint*)(data + (sizeof(DateTime) + sizeof(ulong) * 3));

                                RemoteType.Set(assemblyName, typeName);
                                TypeDirectoryName = typeDirectoryName;
                                MethodDirectoryName = methodDirectoryName;
                                MethodName.Set(methodTypeName, methodName);

                                StreamPersistenceMemoryDatabaseServiceBase service = (StreamPersistenceMemoryDatabaseService)deserializer.Context.castType<CommandServerSocket>().notNull().CurrentController.GetControllerObject();
                                MethodName.NodeTypeFullName = RemoteType.Name;
                                service.AppendRepairNodeMethod(this).Wait();
                                return;
                            }
                        }
                    }
                    CallState = CallStateEnum.CustomDeserializeError;
                    deserializer.SetCustomError(CallState.ToString());
                }
            }
        }
    }
}
