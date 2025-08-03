using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// Remote expression server metadata information
    /// 远程表达式服务端元数据信息
    /// </summary>
    internal sealed class ServerMetadata
    {
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
        private readonly CommandListener server;
        /// <summary>
        /// 命令服务套接字集合
        /// </summary>
        private LeftArray<CommandServerSocket> sockets;
        /// <summary>
        /// 命令服务套接字集合访问锁
        /// </summary>
        private readonly object socketLock;
        /// <summary>
        /// 远程表达式集合
        /// </summary>
        internal readonly Dictionary<SerializeDataKey, CallDelegate>[] ExpressionArray;
        /// <summary>
        /// 远程类型编号集合
        /// </summary>
        internal readonly Dictionary<HashObject<Type>, int> TypeIndexs;
        /// <summary>
        /// 远程类型集合
        /// </summary>
        internal LeftArray<Type> Types;
        /// <summary>
        /// 远程方法编号集合
        /// </summary>
        private readonly Dictionary<HashObject<MethodInfo>, int> methodIndexs;
        /// <summary>
        /// 远程方法集合
        /// </summary>
        internal LeftArray<KeyValue<MethodInfo, RemoteMetadataMethodIndex>> Methods;
        /// <summary>
        /// 远程属性编号集合
        /// </summary>
        private readonly Dictionary<HashObject<PropertyInfo>, int> propertyIndexs;
        /// <summary>
        /// 远程属性集合 + 类型编号
        /// </summary>
        internal LeftArray<KeyValue<PropertyInfo, RemoteMetadataMemberIndex>> Properties;
        /// <summary>
        /// 远程字段编号集合
        /// </summary>
        private readonly Dictionary<HashObject<FieldInfo>, int> fieldIndexs;
        /// <summary>
        /// 远程字段集合 + 类型编号
        /// </summary>
        internal LeftArray<KeyValue<FieldInfo, RemoteMetadataMemberIndex>> Fields;
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
        /// <param name="server"></param>
        internal ServerMetadata(CommandListener server)
        {
            this.server = server;
            ExpressionArray = new Dictionary<SerializeDataKey, CallDelegate>[(byte)DelegateTypeEnum.Count];
            TypeIndexs = DictionaryCreator.CreateHashObject<Type, int>();
            methodIndexs = DictionaryCreator.CreateHashObject<MethodInfo, int>();
            propertyIndexs = DictionaryCreator.CreateHashObject<PropertyInfo, int>();
            fieldIndexs = DictionaryCreator.CreateHashObject<FieldInfo, int>();
            Types = new LeftArray<Type>(0);
            Methods = new LeftArray<KeyValue<MethodInfo, RemoteMetadataMethodIndex>>(0);
            Properties = new LeftArray<KeyValue<PropertyInfo, RemoteMetadataMemberIndex>>(0);
            Fields = new LeftArray<KeyValue<FieldInfo, RemoteMetadataMemberIndex>>(0);
            sockets = new LeftArray<CommandServerSocket>(0);
            socketLock = new object();
        }
        /// <summary>
        /// 添加命令服务套接字
        /// </summary>
        /// <param name="socket"></param>
        internal void Append(CommandServerSocket socket)
        {
            Monitor.Enter(socketLock);
            try
            {
                sockets.Add(socket);
            }
            finally
            {
                Monitor.Exit(socketLock);
                if ((Types.Length | Methods.Length | Properties.Length | Fields.Length) != 0) socket.Push(new ServerOutputRemoteMetadata(this));
            }
        }
        /// <summary>
        /// 添加新方法信息
        /// </summary>
        /// <param name="method"></param>
        /// <param name="methodIndex"></param>
        /// <param name="newMethods"></param>
        /// <returns></returns>
        internal int Append(HashObject<MethodInfo> method, ref RemoteMetadataMethodIndex methodIndex, ref LeftArray<int> newMethods)
        {
            int index;
            bool isNew = false;
            Monitor.Enter(methodIndexs);
            try
            {
                if (!methodIndexs.TryGetValue(method, out index))
                {
                    index = methodIndexs.Count;
                    Methods.PrepLength(1);
                    methodIndex.Index = index;
                    methodIndexs.Add(method, index);
                    Methods.UnsafeAdd(new KeyValue<MethodInfo, RemoteMetadataMethodIndex>(method.Value, methodIndex));
                    isNew = true;
                }
            }
            finally { Monitor.Exit(methodIndexs); }
            if (isNew) newMethods.Add(index);
            return index;
        }
        /// <summary>
        /// 添加新属性信息
        /// </summary>
        /// <param name="property"></param>
        /// <param name="typeIndex"></param>
        /// <param name="newProperties"></param>
        /// <returns></returns>
        internal int Append(HashObject<PropertyInfo> property, int typeIndex, ref LeftArray<int> newProperties)
        {
            int index;
            bool isNew = false;
            Monitor.Enter(propertyIndexs);
            try
            {
                if (!propertyIndexs.TryGetValue(property, out index))
                {
                    Properties.PrepLength(1);
                    propertyIndexs.Add(property, index = propertyIndexs.Count);
                    Properties.UnsafeAdd(new KeyValue<PropertyInfo, RemoteMetadataMemberIndex>(property.Value, new RemoteMetadataMemberIndex(property.Value.Name, index, typeIndex, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)));
                    isNew = true;
                }
            }
            finally { Monitor.Exit(propertyIndexs); }
            if (isNew) newProperties.Add(index);
            return index;
        }
        /// <summary>
        /// 添加新字段信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="typeIndex"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="newFields"></param>
        /// <returns></returns>
        internal int Append(HashObject<FieldInfo> field, int typeIndex, BindingFlags bindingFlags, ref LeftArray<int> newFields)
        {
            int index;
            bool isNew = false;
            Monitor.Enter(fieldIndexs);
            try
            {
                if (!fieldIndexs.TryGetValue(field, out index))
                {
                    Fields.PrepLength(1);
                    fieldIndexs.Add(field, index = fieldIndexs.Count);
                    Fields.UnsafeAdd(new KeyValue<FieldInfo, RemoteMetadataMemberIndex>(field.Value, new RemoteMetadataMemberIndex(field.Value.Name, index, typeIndex, bindingFlags)));
                    isNew = true;
                }
            }
            finally { Monitor.Exit(fieldIndexs); }
            if (isNew) newFields.Add(index);
            return index;
        }
        /// <summary>
        /// 输出远程元数据
        /// </summary>
        /// <param name="formatDeserialize"></param>
        internal void Output(FormatDeserialize formatDeserialize)
        {
            if (sockets.Length != 0)
            {
                ServerOutputRemoteMetadata output = new ServerOutputRemoteMetadata(this, formatDeserialize);
                bool isClone = false;
                Monitor.Enter(socketLock);
                try
                {
                    for (int index = sockets.Length; index != 0;)
                    {
                        CommandServerSocket socket = sockets.Array[--index];
                        if (!socket.IsClose)
                        {
                            if (isClone) output = output.Clone();
                            else isClone = true;
                            socket.AppendOutput(output);
                        }
                        else sockets.RemoveToEnd(index);
                    }
                }
                finally { Monitor.Exit(socketLock); }
            }
        }
        /// <summary>
        /// 获取常量参数类型信息
        /// </summary>
        /// <param name="serializeInfo"></param>
        /// <returns></returns>
        internal unsafe ServerMethodParameter GetConstantParameterType(ref SerializeInfo serializeInfo)
        {
            int count = serializeInfo.ConstantParameterCount, index = 0;
            byte* start = (byte*)serializeInfo.Key.DeserializeData.Data;
            byte[] typeIndexs = AutoCSer.Common.GetUninitializedArray<byte>(count * sizeof(int));
            AutoCSer.Common.CopyTo(start + (*(int*)start + sizeof(int) * 3), typeIndexs);
            var constantParameterType = default(ServerMethodParameter);
            HashBytes typeKey = typeIndexs;
            bool isNew = false;
            Monitor.Enter(constantParameterTypes);
            try
            {
                if (!constantParameterTypes.TryGetValue(typeKey, out constantParameterType))
                {
                    TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".Net.CommandServer.RemoteExpressionConstantParameterType" + (++constantParameterTypeIndex).toString(), TypeAttributes.AutoLayout | TypeAttributes.Public, typeof(ValueType), null);
                    fixed (byte* typeIndexFixed = typeIndexs)
                    {
                        do
                        {
                            typeBuilder.DefineField(index.toString(), Types.Array[*(int*)(typeIndexFixed + (index << 2)) - 1], FieldAttributes.Public);
                        }
                        while (++index != count);
                    }
                    constantParameterTypes.Add(typeKey, constantParameterType = new ServerMethodParameter(typeBuilder.CreateType()));
                    isNew = true;
                }
            }
            finally { Monitor.Exit(constantParameterTypes); }
            if(isNew) ServerMethodParameter.AppendType(constantParameterType.Type);
            return constantParameterType;
        }

        /// <summary>
        /// 远程表达式常量参数类型集合
        /// </summary>
        private static readonly Dictionary<HashBytes, ServerMethodParameter> constantParameterTypes = DictionaryCreator<HashBytes>.Create<ServerMethodParameter>();
        /// <summary>
        /// 远程表达式常量参数类型编号
        /// </summary>
        private static int constantParameterTypeIndex;
    }
}
