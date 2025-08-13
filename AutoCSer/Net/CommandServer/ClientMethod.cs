using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Client interface method information
    /// 客户端接口方法信息
    /// </summary>
    public sealed class ClientMethod
    {
        /// <summary>
        /// Client interface type
        /// 客户端接口类型
        /// </summary>
        private readonly Type type;
        /// <summary>
        /// Customize the command sequence number
        /// 自定义命令序号
        /// </summary>
        internal readonly int MethodIndex;
        /// <summary>
        /// Whether the output data is simply serialized
        /// 输出数据是否简单序列化
        /// </summary>
        internal readonly bool IsSimpleSerializeParamter;
        /// <summary>
        /// Whether the input data is simply deserialized
        /// 输入数据是否简单反序列化
        /// </summary>
        internal readonly bool IsSimpleDeserializeParamter;
        /// <summary>
        /// The client's await awaits the return value callback thread mode
        /// 客户端 await 等待返回值回调线程模式
        /// </summary>
        internal readonly ClientCallbackTypeEnum CallbackType;
        /// <summary>
        /// Call back the queue number
        /// 回调队列序号
        /// </summary>
        internal readonly byte QueueIndex;
        /// <summary>
        /// Match the method name
        /// 匹配方法名称
        /// </summary>
        internal readonly string MatchMethodName;
        /// <summary>
        /// Timeout seconds
        /// 超时秒数
        /// </summary>
        internal readonly ushort TimeoutSeconds;
        /// <summary>
        /// Whether it is a low-priority queue
        /// 是否低优先级队列
        /// </summary>
        internal readonly bool IsLowPriorityQueue;
        /// <summary>
        /// Whether to simply serialize the return value of the first stage of the two-stage callback
        /// 是否简单序列化二阶段回调的第一阶段的返回值
        /// </summary>
        internal readonly bool IsSimpleSerializeTwoStage‌ReturnValue;
        /// <summary>
        /// Whether deserialization failed
        /// 是否反序列化失败
        /// </summary>
        private bool isDeserializeError;
        /// <summary>
        /// Empty method information
        /// 空方法信息
        /// </summary>
        internal ClientMethod()
        {
            MatchMethodName = string.Empty;
            type = typeof(int);
        }
        /// <summary>
        /// Client interface method information
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
        /// <param name="isSimpleSerializeTwoStage‌ReturnValue"></param>
        public ClientMethod(Type type, string name, int methodIndex, byte isSimpleSerialize, byte isSimpleDeserialize, ClientCallbackTypeEnum callbackType, byte queueIndex, byte isLowPriorityQueue, ushort timeoutSeconds, byte isSimpleSerializeTwoStage‌ReturnValue = 0)
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
            IsSimpleSerializeTwoStageReturnValue = isSimpleSerializeTwoStage‌ReturnValue != 0;
        }
        /// <summary>
        /// Get the collection of server-side method numbers
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
        /// Get the collection of server-side method numbers
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
        /// Output the deserialization error log
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
