using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端接口方法信息
    /// </summary>
    public sealed class ClientMethod
    {
        /// <summary>
        /// 客户端接口类型
        /// </summary>
        private readonly Type type;
        /// <summary>
        /// 自定义命令序号
        /// </summary>
        internal readonly int MethodIndex;
        /// <summary>
        /// 是否简单序列化输出数据
        /// </summary>
        internal readonly bool IsSimpleSerializeParamter;
        /// <summary>
        /// 是否简单反序列化输入数据
        /// </summary>
        internal readonly bool IsSimpleDeserializeParamter;
        /// <summary>
        /// 客户端 await 等待返回值回调线程模式
        /// </summary>
        internal readonly ClientCallbackTypeEnum CallbackType;
        /// <summary>
        /// 回调队列序号
        /// </summary>
        internal readonly byte QueueIndex;
        /// <summary>
        /// 匹配方法名称
        /// </summary>
        internal readonly string MatchMethodName;
        /// <summary>
        /// 超时秒数
        /// </summary>
        internal readonly ushort TimeoutSeconds;
        /// <summary>
        /// 是否低优先级队列
        /// </summary>
        internal readonly bool IsLowPriorityQueue;
        /// <summary>
        /// 是否反序列化错误
        /// </summary>
        private bool isDeserializeError;
        /// <summary>
        /// 空方法信息
        /// </summary>
        internal ClientMethod()
        {
            MatchMethodName = string.Empty;
            type = typeof(int);
        }
        /// <summary>
        /// 客户端接口方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="methodIndex"></param>
        /// <param name="isSimpleSerialize"></param>
        /// <param name="isSimpleDeserialize"></param>
        /// <param name="callbackType"></param>
        /// <param name="queueIndex"></param>
        /// <param name="isLowPriorityQueue"></param>
        /// <param name="timeoutSeconds"></param>
        public ClientMethod(Type type, string name, int methodIndex, byte isSimpleSerialize, byte isSimpleDeserialize, ClientCallbackTypeEnum callbackType, byte queueIndex, byte isLowPriorityQueue, ushort timeoutSeconds)
        {
            this.type = type;
            MethodIndex = methodIndex;
            IsSimpleSerializeParamter = isSimpleSerialize != 0;
            IsSimpleDeserializeParamter = isSimpleDeserialize != 0;
            CallbackType = callbackType;
            QueueIndex = queueIndex;
            MatchMethodName = name;
            TimeoutSeconds = timeoutSeconds;
            IsLowPriorityQueue = isLowPriorityQueue != 0;
        }
        /// <summary>
        /// 获取服务端方法编号集合
        /// </summary>
        /// <param name="methodStartIndex"></param>
        /// <param name="methods"></param>
        /// <param name="serverMethodNames"></param>
        /// <returns></returns>
#if NetStandard21
        internal static int[] GetServerMethodIndexs(int methodStartIndex, ClientMethod[] methods, string?[]? serverMethodNames)
#else
        internal static int[] GetServerMethodIndexs(int methodStartIndex, ClientMethod[] methods, string[] serverMethodNames)
#endif
        {
            int maxMethodIndex = -1;
            foreach (var method in methods)
            {
                if (method.MethodIndex > maxMethodIndex) maxMethodIndex = method.MethodIndex;
            }
            int[] serverMethodIndexs = AutoCSer.Common.GetUninitializedArray<int>(maxMethodIndex + 1);
            if (serverMethodNames == null)
            {
                AutoCSer.Common.Fill(serverMethodIndexs, -methodStartIndex);
                foreach (var method in methods)
                {
                    int methodIndex = method.MethodIndex;
                    //if ((uint)methodIndex >= serverMethodIndexs.Length)
                    //{
                    //    Console.WriteLine("ERROR");
                    //}
                    serverMethodIndexs[methodIndex] = methodIndex;
                }
            }
            else GetServerMethodIndexs(methodStartIndex, methods, serverMethodNames, serverMethodIndexs);
            return serverMethodIndexs;
        }
        /// <summary>
        /// 获取服务端方法编号集合
        /// </summary>
        /// <param name="methodStartIndex"></param>
        /// <param name="methods"></param>
        /// <param name="serverMethodNames"></param>
        /// <param name="serverMethodIndexs"></param>
#if NetStandard21
        internal static void GetServerMethodIndexs(int methodStartIndex, ClientMethod[] methods, string?[] serverMethodNames, int[] serverMethodIndexs)
#else
        internal static void GetServerMethodIndexs(int methodStartIndex, ClientMethod[] methods, string[] serverMethodNames, int[] serverMethodIndexs)
#endif
        {
            AutoCSer.Common.Fill(serverMethodIndexs, -methodStartIndex);
            LeftArray<ClientMethod> errorMethods = new LeftArray<ClientMethod>(0);
            foreach (var method in methods)
            {
                int methodIndex = method.MethodIndex;
                if (methodIndex < serverMethodNames.Length && method.MatchMethodName == serverMethodNames[methodIndex])
                {
                    serverMethodIndexs[methodIndex] = methodIndex;
                    serverMethodNames[methodIndex] = null;
                }
                else if (serverMethodIndexs[methodIndex] < 0) errorMethods.Add(method);
            }
            if (errorMethods.Length != 0)
            {
                Dictionary<string, int> serverMethodNameIndexs = DictionaryCreator<string>.Create<int>(serverMethodNames.Length);
                int methodIndex = 0;
                foreach (var name in serverMethodNames)
                {
                    if (name != null) serverMethodNameIndexs[name] = methodIndex;
                    ++methodIndex;
                }
                foreach (var method in errorMethods)
                {
                    if (serverMethodIndexs[methodIndex = method.MethodIndex] < 0)
                    {
                        int serverIndex;
                        if (serverMethodNameIndexs.TryGetValue(method.MatchMethodName, out serverIndex)) serverMethodIndexs[methodIndex] = serverIndex;
                    }
                }
            }
        }
        /// <summary>
        /// 输出反序列化错误日志
        /// </summary>
        /// <param name="controller"></param>
        internal void DeserializeError(CommandClientController controller)
        {
            if (!isDeserializeError)
            {
                isDeserializeError = true;
                controller.Socket.Client.Log.ErrorIgnoreException($"{controller.ControllerName} =>{type.fullName()}.{MatchMethodName} {nameof(DeserializeError)}");
            }
        }
    }
}
