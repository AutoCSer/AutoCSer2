using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 控制器接口信息
    /// </summary>
    internal static class ServerInterfaceController
    {
        /// <summary>
        /// ref SubArray{byte}
        /// </summary>
        internal static readonly Type ByteSubArrayRefType = typeof(SubArray<byte>).MakeByRefType();
        /// <summary>
        /// 命令服务控制器构造函数信息
        /// </summary>
        internal static readonly ConstructorInfo CommandControllerConstructorInfo = typeof(CommandServerController).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(CommandListener), typeof(string), typeof(CommandServerControllerInterfaceAttribute), typeof(ServerInterfaceMethod[]), typeof(int), typeof(byte), typeof(bool), typeof(bool) }, null).notNull();
        /// <summary>
        /// 执行命令参数类型
        /// </summary>
        internal static readonly Type[] DoCommandParameterTypes = new Type[] { typeof(CommandServerSocket), ByteSubArrayRefType };
        /// <summary>
        /// 获取当前命令方法序号
        /// </summary>
        internal static readonly Func<CommandServerSocket, int> CommandServerSocketGetCommandMethodIndex = CommandServerSocket.GetCommandMethodIndex;
        ///// <summary>
        ///// 同步发送数据
        ///// </summary>
        //internal static readonly Func<CommandServerSocket, CommandClientReturnType, bool> CommandServerSocketSendReturnType = CommandServerSocket.Send;
        /// <summary>
        /// 同步发送成功返回值类型
        /// </summary>
        internal static readonly Func<CommandServerSocket, bool> CommandServerSocketSendSuccess = CommandServerSocket.Send;
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Action<CommandServerCallQueue, CommandServerCallQueueNode> ServerCallQueueAdd = CommandServerCallQueue.Add;
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Action<CommandServerCallReadWriteQueue, ReadWriteQueueNode> ServerCallReadWriteQueueAppendRead = CommandServerCallReadWriteQueue.AppendRead;
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Action<CommandServerCallReadWriteQueue, ReadWriteQueueNode> ServerCallReadWriteQueueAppendWrite = CommandServerCallReadWriteQueue.AppendWrite;
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Func<CommandServerCallQueue, CommandServerCallQueueNode, CommandClientReturnTypeEnum> ServerCallQueueAddIsDeserialize = CommandServerCallQueue.AddIsDeserialize;
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Func<CommandServerCallReadWriteQueue, ReadWriteQueueNode, CommandClientReturnTypeEnum> ServerCallReadWriteQueueAppendReadIsDeserialize = CommandServerCallReadQueue.AppendReadIsDeserialize;
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Func<CommandServerCallReadWriteQueue, ReadWriteQueueNode, CommandClientReturnTypeEnum> ServerCallReadWriteQueueAppendWriteIsDeserialize = CommandServerCallReadQueue.AppendWriteIsDeserialize;
        /// <summary>
        /// Determine whether the socket has been closed
        /// 判断套接字是否已经关闭
        /// </summary>
        internal static readonly Func<CommandServerCallQueueNode, bool> CommandServerCallQueueNodeSocketIsClose = CommandServerCallQueueNode.SocketIsClose;
        /// <summary>
        /// Determine whether the socket has been closed
        /// 判断套接字是否已经关闭
        /// </summary>
        internal static readonly Func<CommandServerCallReadWriteQueueNode, bool> CommandServerCallReadWriteQueueNodeSocketIsClose = CommandServerCallReadWriteQueueNode.SocketIsClose;
        /// <summary>
        /// Determine whether the socket has been closed
        /// 判断套接字是否已经关闭
        /// </summary>
        internal static readonly Func<CommandServerCallConcurrencyReadQueueNode, bool> CommandServerCallConcurrencyReadQueueNodeSocketIsClose = CommandServerCallConcurrencyReadQueueNode.SocketIsClose;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerCallQueueNode, bool> CommandServerCallQueueNodeSetIsDeserialize = CommandServerCallQueueNode.SetIsDeserialize;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerCallReadWriteQueueNode, bool> CommandServerCallReadWriteQueueNodeSetIsDeserialize = CommandServerCallReadWriteQueueNode.SetIsDeserialize;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerCallConcurrencyReadQueueNode, bool> CommandServerCallConcurrencyReadQueueNodeSetIsDeserialize = CommandServerCallConcurrencyReadQueueNode.SetIsDeserialize;
        /// <summary>
        /// Offline counting processing
        /// 下线计数处理
        /// </summary>
        internal static readonly Action<CommandServerCallQueueNode> CommandServerCallQueueNodeCheckOfflineCount = CommandServerCallQueueNode.CheckOfflineCount;
        /// <summary>
        /// Offline counting processing
        /// 下线计数处理
        /// </summary>
        internal static readonly Action<CommandServerCallReadWriteQueueNode> CommandServerCallReadWriteQueueNodeCheckOfflineCount = CommandServerCallReadWriteQueueNode.CheckOfflineCount;
        /// <summary>
        /// Offline counting processing
        /// 下线计数处理
        /// </summary>
        internal static readonly Action<CommandServerCallConcurrencyReadQueueNode> CommandServerCallConcurrencyReadQueueNodeCheckOfflineCount = CommandServerCallConcurrencyReadQueueNode.CheckOfflineCount;
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Action<CommandServerCallLowPriorityQueue, CommandServerCallQueueNode> ServerCallQueueLowPriorityLinkAdd = CommandServerCallLowPriorityQueue.Add;
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Func<CommandServerCallLowPriorityQueue, CommandServerCallQueueNode, CommandClientReturnTypeEnum> ServerCallQueueLowPriorityLinkAddIsDeserialize = CommandServerCallLowPriorityQueue.AddIsDeserialize;
        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        internal static readonly Func<CommandServerCallQueueNode, CommandServerSocket> ServerCallQueueNodeGetSocket = CommandServerCallQueueNode.GetSocket;
        /// <summary>
        /// Close the short connection
        /// 关闭短连接
        /// </summary>
        internal static readonly Action<CommandServerCallQueueNode> ServerCallQueueNodeCloseShortLink = CommandServerCallQueueNode.CloseShortLink;
        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        internal static readonly Func<CommandServerCallReadWriteQueueNode, CommandServerSocket> ServerCallReadWriteQueueNodeGetSocket = CommandServerCallReadWriteQueueNode.GetSocket;
        /// <summary>
        /// Close the short connection
        /// 关闭短连接
        /// </summary>
        internal static readonly Action<CommandServerCallReadWriteQueueNode> ServerCallReadWriteQueueNodeCloseShortLink = CommandServerCallReadWriteQueueNode.CloseShortLink;
        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        internal static readonly Func<CommandServerCallConcurrencyReadQueueNode, CommandServerSocket> ServerCallConcurrencyReadQueueNodeGetSocket = CommandServerCallConcurrencyReadQueueNode.GetSocket;
        /// <summary>
        /// Close the short connection
        /// 关闭短连接
        /// </summary>
        internal static readonly Action<CommandServerCallConcurrencyReadQueueNode> ServerCallConcurrencyReadQueueNodeCloseShortLink = CommandServerCallConcurrencyReadQueueNode.CloseShortLink;
        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="task"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        internal delegate CommandServerSocket CommandServerCallTaskQueueTaskNodeGetSocketDelegate(CommandServerCallTaskQueueNode task, out CommandServerCallTaskQueue queue);
        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        internal static readonly CommandServerCallTaskQueueTaskNodeGetSocketDelegate CommandServerCallTaskQueueTaskNodeGetSocket = CommandServerCallTaskQueueNode.GetSocket;
        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="task"></param>
        /// <param name="keepCallback"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        internal delegate CommandServerSocket CommandServerCommandServerKeepCallbackQueueTaskGetSocketDelegate(CommandServerKeepCallbackQueueTask task, CommandServerKeepCallback keepCallback, out CommandServerCallTaskQueue queue);
        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        internal static readonly CommandServerCommandServerKeepCallbackQueueTaskGetSocketDelegate CommandServerKeepCallbackQueueTaskGetSocket = CommandServerKeepCallbackQueueTask.GetSocket;
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal static readonly Func<CommandServerCallTaskQueueTask, Task, bool> CommandServerCallTaskQueueTaskCheckCallTask = CommandServerCallTaskQueueTask.CheckCallTask;
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal static readonly Func<CommandServerCallbackTaskQueueTask, Task, bool> CommandServerCallbackTaskQueueTaskCheckCallTask = CommandServerCallbackTaskQueueTask.CheckCallTask;
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal static readonly Func<CommandServerCallTaskQueueVerifyStateTask, Task<CommandServerVerifyStateEnum>, bool> CommandServerCallTaskQueueVerifyStateTaskCheckCallTask = CommandServerCallTaskQueueVerifyStateTask.CheckCallTask;
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal static readonly Func<CommandServerKeepCallbackQueueTask, Task, bool> CommandServerKeepCallbackQueueTaskCheckCallTask = CommandServerKeepCallbackQueueTask.CheckCallTask;
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal static readonly Func<CommandServerCallTaskQueueSendOnlyTask, Task<CommandServerSendOnly>, bool> CommandServerCallTaskQueueSendOnlyTaskCheckCallTask = CommandServerCallTaskQueueSendOnlyTask.CheckCallTask;
        /// <summary>
        /// 获取服务端执行队列
        /// </summary>
        internal static readonly Func<CommandListener, int, CommandServerCallQueue> CommandListenerGetServerCallQueue = CommandListener.GetServerCallQueue;
        /// <summary>
        /// 获取服务端执行低优先级队列
        /// </summary>
        internal static readonly Func<CommandListener, int, CommandServerCallLowPriorityQueue> CommandListenerGetServerCallQueueLowPriority = CommandListener.GetServerCallQueueLowPriority;
        /// <summary>
        /// 获取服务端读写队列
        /// </summary>
        internal static readonly Func<CommandListener, CommandServerCallReadQueue> CommandListenerGetServerCallReadWriteQueue = CommandListener.GetServerCallReadWriteQueue;
        /// <summary>
        /// 获取服务端读写队列
        /// </summary>
        internal static readonly Func<CommandListener, CommandServerCallConcurrencyReadQueue> CommandListenerGetServerCallConcurrencyReadQueue = CommandListener.GetServerCallConcurrencyReadQueue;
        /// <summary>
        /// 否则使用 IO 线程同步调用 Task
        /// </summary>
        internal static readonly Func<ServerInterfaceMethod, CommandServerSocket, bool> ServerInterfaceMethodIsSynchronousCallTask = ServerInterfaceMethod.IsSynchronousCallTask;
        /// <summary>
        /// 否则使用 IO 线程同步调用 Task
        /// </summary>
        internal static readonly Action<ServerInterfaceMethod, long> ServerInterfaceMethodCheckGetTaskTimestamp = ServerInterfaceMethod.CheckGetTaskTimestamp;
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        internal static readonly Func<long> StopwatchGetTimestamp = Stopwatch.GetTimestamp;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerRunTask, bool> CommandServerRunTaskSetIsDeserialize = CommandServerRunTask.SetIsDeserialize;
        /// <summary>
        /// 任务调用
        /// </summary>
        internal static readonly Func<CommandServerRunTask, CommandClientReturnTypeEnum> CommandServerRunTaskIsDeserialize = CommandServerRunTask.RunTaskIsDeserialize;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerRunTask<CommandServerVerifyStateEnum>, bool> CommandServerVerifyStateRunTaskSetIsDeserialize = CommandServerRunTask<CommandServerVerifyStateEnum>.SetIsDeserialize;
        /// <summary>
        /// 任务调用
        /// </summary>
        internal static readonly Func<CommandServerVerifyStateRunTask, CommandClientReturnTypeEnum> CommandServerVerifyStateRunTaskIsDeserialize = CommandServerVerifyStateRunTask.RunTaskIsDeserialize;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerSendOnlyRunTask, bool> CommandServerSendOnlyRunTaskSetIsDeserialize = CommandServerSendOnlyRunTask.SetIsDeserialize;
        /// <summary>
        /// 任务调用
        /// </summary>
        internal static readonly Func<CommandServerSendOnlyRunTask, CommandClientReturnTypeEnum> CommandServerSendOnlyRunTaskIsDeserialize = CommandServerSendOnlyRunTask.RunTaskIsDeserialize;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerCallbackRunTask, bool> CommandServerCallbackRunTaskSetIsDeserialize = CommandServerCallbackRunTask.SetIsDeserialize;
        /// <summary>
        /// 任务调用
        /// </summary>
        internal static readonly Func<CommandServerCallbackRunTask, CommandClientReturnTypeEnum> CommandServerCallbackRunTaskIsDeserialize = CommandServerCallbackRunTask.RunTaskIsDeserialize;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerKeepCallbackRunTask, bool> CommandServerKeepCallbackRunTaskSetIsDeserialize = CommandServerKeepCallbackRunTask.SetIsDeserialize;
        /// <summary>
        /// 任务调用
        /// </summary>
        internal static readonly Func<CommandServerKeepCallbackRunTask, CommandClientReturnTypeEnum> CommandServerKeepCallbackRunTaskIsDeserialize = CommandServerKeepCallbackRunTask.RunTaskIsDeserialize;
        /// <summary>
        /// 任务调用
        /// </summary>
        internal static readonly Func<CommandServerKeepCallbackRunTask, CommandClientReturnTypeEnum> CommandServerKeepCallbackRunTaskAutoCancelKeepIsDeserialize = CommandServerKeepCallbackRunTask.RunTaskAutoCancelKeepIsDeserialize;
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        internal static readonly Action<CommandServerKeepCallbackCountRunTask, bool> CommandServerKeepCallbackCountRunTaskSetIsDeserialize = CommandServerKeepCallbackCountRunTask.SetIsDeserialize;
        /// <summary>
        /// 任务调用
        /// </summary>
        internal static readonly Func<CommandServerKeepCallbackCountRunTask, CommandClientReturnTypeEnum> CommandServerKeepCallbackCountRunTaskIsDeserialize = CommandServerKeepCallbackCountRunTask.RunTaskIsDeserialize;
        /// <summary>
        /// 任务调用
        /// </summary>
        internal static readonly Func<CommandServerKeepCallbackCountRunTask, CommandClientReturnTypeEnum> CommandServerKeepCallbackCountRunTaskAutoCancelKeepIsDeserialize = CommandServerKeepCallbackCountRunTask.RunTaskAutoCancelKeepIsDeserialize;

        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        internal static readonly Func<CommandServerCall, CommandServerSocket> ServerCallGetSocket = CommandServerCall.GetSocket;
        /// <summary>
        /// Send the return type successfully
        /// 发送成功返回类型
        /// </summary>
        internal static readonly Func<CommandServerCallQueueNode, CommandServerCallQueue, bool> ServerCallQueueNodeSendSuccess = CommandServerCallQueueNode.Send;
        /// <summary>
        /// Send the return type successfully
        /// 发送成功返回类型
        /// </summary>
        internal static readonly Func<CommandServerCallReadWriteQueueNode, bool> ServerCallReadWriteQueueNodeSendSuccess = CommandServerCallReadWriteQueueNode.Send;
        /// <summary>
        /// Send the return type successfully
        /// 发送成功返回类型
        /// </summary>
        internal static readonly Func<CommandServerCallConcurrencyReadQueueNode, bool> ServerCallConcurrencyReadQueueNodeSendSuccess = CommandServerCallConcurrencyReadQueueNode.Send;
        /// <summary>
        /// Send data
        /// </summary>
        internal static readonly Func<CommandServerCall, bool> ServerCallSend = CommandServerCall.Send;
        /// <summary>
        /// The verification method sends data
        /// 验证方法发送数据
        /// </summary>
        internal static readonly Func<CommandServerCall, ServerInterfaceMethod, CommandServerVerifyStateEnum, bool> ServerCallSendVerifyState = CommandServerCall.Send;
        /// <summary>
        /// Set the status of the verification result of the command service
        /// 设置命令服务验证结果状态
        /// </summary>
        internal static readonly Action<CommandServerCallQueueNode, CommandServerVerifyStateEnum> ServerCallQueueNodeSetVerifyState = CommandServerCallQueueNode.SetVerifyState;
        /// <summary>
        /// Set the status of the verification result of the command service
        /// 设置命令服务验证结果状态
        /// </summary>
        internal static readonly Action<CommandServerCallReadWriteQueueNode, CommandServerVerifyStateEnum> ServerCallReadWriteQueueNodeSetVerifyState = CommandServerCallReadWriteQueueNode.SetVerifyState;
        /// <summary>
        /// Set the status of the verification result of the command service
        /// 设置命令服务验证结果状态
        /// </summary>
        internal static readonly Action<CommandServerCallConcurrencyReadQueueNode, CommandServerVerifyStateEnum> ServerCallConcurrencyReadQueueNodeSetVerifyState = CommandServerCallConcurrencyReadQueueNode.SetVerifyState;
        /// <summary>
        /// Set the status of the verification result of the command service
        /// 设置命令服务验证结果状态
        /// </summary>
        internal static readonly Action<CommandServerSocket, CommandServerVerifyStateEnum> CommandServerSocketSetVerifyState = CommandServerSocket.SetVerifyState;
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        internal static readonly Action<CommandServerSocket, Task> CommandServerSocketCheckTask = CommandServerSocket.CheckTask;
        /// <summary>
        /// 检查认证接口任务完成状态
        /// </summary>
        internal static readonly Action<CommandServerSocket, ServerInterfaceMethod, Task<CommandServerVerifyStateEnum>> CommandServerSocketCheckVerifyStateTask = CommandServerSocket.CheckTask;
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        internal static readonly Action<CommandServerSocket, Task<CommandServerSendOnly>> CommandServerSocketCheckSendOnlyTask = CommandServerSocket.CheckTask;
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        internal static readonly Action<CommandServerCallbackTask, Task> CommandServerCallbackTaskCheckTask = CommandServerCallbackTask.CheckTask;
        /// <summary>
        /// 取消保持回调命令
        /// </summary>
        internal static readonly Action<CommandServerKeepCallback> CommandServerKeepCallbackCancelKeep = (Action<CommandServerKeepCallback>)CommandServerKeepCallback.CancelKeep;
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        internal static readonly Action<CommandServerController, CommandServerCallTaskQueueNode> CommandServerControllerAddTaskQueue = CommandServerController.AddTaskQueue;
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        internal static readonly Action<CommandServerController, CommandServerCallTaskQueueNode> CommandServerControllerAddTaskQueueLowPriority = CommandServerController.AddTaskQueueLowPriority;

        /// <summary>
        /// 反序列化方法
        /// </summary>
        internal static readonly MethodInfo CommandServerSocketDeserializeMethod;
        /// <summary>
        /// Send data
        /// </summary>
        internal static readonly MethodInfo CommandServerSocketSendOutputMethod;
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        internal static readonly MethodInfo CommandServerSocketCallTaskQueueAppendQueueMethod;
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        internal static readonly MethodInfo CommandServerSocketCallTaskQueueAppendLowPriorityMethod;

        /// <summary>
        /// 异步回调类型序号
        /// </summary>
        private static int asynchronousTypeIndex;
        /// <summary>
        /// 获取异步回调类型名称
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static string GetAsynchronousTypeName()
        {
            return AutoCSer.Common.NamePrefix + ".Net.CommandServer.InterfaceController.Asynchronous" + Interlocked.Increment(ref asynchronousTypeIndex).toString();
        }
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] CommandServerSocketInterfaceMethodParameterTypes = new Type[] { typeof(CommandServerSocket), typeof(ServerInterfaceMethod) };
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] CommandServerSocketParameterTypes = new Type[] { typeof(CommandServerSocket) };
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] ServerCallQueueNodeParameterTypes = new Type[] { typeof(CommandServerCallQueueNode) };
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] ServerCallReadWriteQueueNodeParameterTypes = new Type[] { typeof(CommandServerCallReadWriteQueueNode) };
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] ServerCallConcurrencyReadQueueNodeParameterTypes = new Type[] { typeof(CommandServerCallConcurrencyReadQueueNode) };
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] ServerCallQueueParameterTypes = new Type[] { typeof(CommandServerCallQueueNode), typeof(CommandServerCallQueue) };
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] ServerCallReadWriteQueueParameterTypes = new Type[] { typeof(CommandServerCallReadWriteQueueNode), typeof(CommandServerCallReadQueue) };
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] ServerCallConcurrencyReadQueueParameterTypes = new Type[] { typeof(CommandServerCallConcurrencyReadQueueNode), typeof(CommandServerCallConcurrencyReadQueue) };
        /// <summary>
        /// 异步回调类型构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] ServerCallTaskQueueParameterTypes = new Type[] { typeof(CommandServerCallTaskQueueNode) };
        ///// <summary>
        ///// 清除下线计数对象
        ///// </summary>
        //internal static readonly Action<CommandServerCallQueueNode> CommandServerCallQueueNodeSetOfflineCountNull = CommandServerCallQueueNode.SetOfflineCountNull;
        /// <summary>
        /// 异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerSocket, CommandServerCallback> CreateServerCallbackDelegate = CommandServerCallback.CreateServerCallback;
        /// <summary>
        /// 异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerSocket, CommandServerCallbackTask> CreateServerCallbackTaskDelegate = CommandServerCallbackTask.CreateServerCallbackTask;
        /// <summary>
        /// 异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallQueueNode, CommandServerCallback> CreateServerCallbackCallQueueNodeDelegate = CommandServerCallback.CreateServerCallback;
        /// <summary>
        /// 异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallReadWriteQueueNode, CommandServerCallback> CreateServerCallbackCallReadWriteQueueNodeDelegate = CommandServerCallback.CreateServerCallback;
        /// <summary>
        /// 异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallConcurrencyReadQueueNode, CommandServerCallback> CreateServerCallbackCallConcurrencyReadQueueNodeDelegate = CommandServerCallback.CreateServerCallback;
        /// <summary>
        /// 异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallTaskQueueNode, CommandServerCallback> CreateServerCallbackCallTaskQueueNodeDelegate = CommandServerCallback.CreateServerCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerSocket, CommandServerKeepCallback> CreateCommandServerKeepCallbackDelegate = CommandServerKeepCallback.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerSocket, int, CommandServerKeepCallbackCount> CreateCommandServerKeepCallbackCountDelegate = CommandServerKeepCallbackCount.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallQueueNode, CommandServerKeepCallback> CreateCommandServerKeepCallbackQueueNodeDelegate = CommandServerKeepCallback.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallReadWriteQueueNode, CommandServerKeepCallback> CreateCommandServerKeepCallbackReadWriteQueueNodeDelegate = CommandServerKeepCallback.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallConcurrencyReadQueueNode, CommandServerKeepCallback> CreateCommandServerKeepCallbackConcurrencyReadQueueNodeDelegate = CommandServerKeepCallback.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallQueueNode, int, CommandServerKeepCallbackCount> CreateCommandServerKeepCallbackCountQueueNodeDelegate = CommandServerKeepCallbackCount.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallReadWriteQueueNode, int, CommandServerKeepCallbackCount> CreateCommandServerKeepCallbackCountReadWriteQueueNodeDelegate = CommandServerKeepCallbackCount.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallConcurrencyReadQueueNode, int, CommandServerKeepCallbackCount> CreateCommandServerKeepCallbackCountConcurrencyReadQueueNodeDelegate = CommandServerKeepCallbackCount.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerSocket, CommandServerKeepCallbackTask> CreateServerKeepCallbackTaskDelegate = CommandServerKeepCallbackTask.CreateServerKeepCallbackTask;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerSocket, int, CommandServerKeepCallbackCountTask> CreateServerKeepCallbackCountTaskDelegate = CommandServerKeepCallbackCountTask.CreateServerKeepCallbackTask;
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        internal static readonly Action<CommandServerKeepCallbackTask, Task> CommandServerKeepCallbackTaskCheckTaskDelegate = CommandServerKeepCallbackTask.CheckTask;
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        internal static readonly Action<CommandServerKeepCallbackTask, Task> CommandServerKeepCallbackTaskCheckTaskAutoCancelKeepDelegate = CommandServerKeepCallbackTask.CheckTaskAutoCancelKeep;
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        internal static readonly Action<CommandServerKeepCallbackCountTask, Task> CommandServerKeepCallbackTaskCheckCountTaskDelegate = CommandServerKeepCallbackCountTask.CheckTask;
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        internal static readonly Action<CommandServerKeepCallbackCountTask, Task> CommandServerKeepCallbackTaskCheckCountTaskAutoCancelKeepDelegate = CommandServerKeepCallbackCountTask.CheckTaskAutoCancelKeep;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallTaskQueueNode, CommandServerKeepCallback> CreateCommandServerKeepCallbackTaskQueueDelegate = CommandServerKeepCallback.CreateServerKeepCallback;
        /// <summary>
        /// 保持异步回调类型构造函数
        /// </summary>
        internal static readonly Func<CommandServerCallTaskQueueNode, int, CommandServerKeepCallbackCount> CreateCommandServerKeepCallbackCountTaskQueueDelegate = CommandServerKeepCallbackCount.CreateServerKeepCallback;
        /// <summary>
        /// 服务端执行队列任务构造函数
        /// </summary>
        internal static readonly ConstructorInfo ServerCallQueueNodeConstructor = typeof(CommandServerCallQueueNode).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(CommandServerSocket), typeof(ServerMethodTypeEnum) }, null).notNull();
        /// <summary>
        /// 服务端读写队列任务构造函数
        /// </summary>
        internal static readonly ConstructorInfo ServerCallReadWriteQueueNodeConstructor = typeof(CommandServerCallReadWriteQueueNode).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(CommandServerSocket), typeof(ServerMethodTypeEnum) }, null).notNull();
        /// <summary>
        /// 服务端支持并发读队列任务构造函数
        /// </summary>
        internal static readonly ConstructorInfo ServerCallConcurrencyReadQueueNodeConstructor = typeof(CommandServerCallConcurrencyReadQueueNode).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(CommandServerSocket), typeof(ServerMethodTypeEnum) }, null).notNull();
        /// <summary>
        /// 服务端异步调用构造函数
        /// </summary>
        internal static readonly ConstructorInfo CommandServerCallTaskQueueTaskConstructor = typeof(CommandServerCallTaskQueueTask).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CommandServerSocketParameterTypes, null).notNull();
        /// <summary>
        /// 服务端异步调用构造函数
        /// </summary>
        internal static readonly ConstructorInfo CommandServerCallbackTaskQueueTaskConstructor = typeof(CommandServerCallbackTaskQueueTask).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CommandServerSocketParameterTypes, null).notNull();
        /// <summary>
        /// 服务端异步调用构造函数
        /// </summary>
        internal static readonly ConstructorInfo CommandServerCallTaskQueueSendOnlyTaskConstructor = typeof(CommandServerCallTaskQueueSendOnlyTask).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CommandServerSocketParameterTypes, null).notNull();
        /// <summary>
        /// 服务端异步调用构造函数
        /// </summary>
        internal static readonly ConstructorInfo CommandServerCallTaskQueueVerifyStateTaskConstructor = typeof(CommandServerCallTaskQueueVerifyStateTask).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CommandServerSocketInterfaceMethodParameterTypes, null).notNull();
        /// <summary>
        /// 服务端异步调用构造函数
        /// </summary>
        internal static readonly ConstructorInfo ServerKeepCallbackQueueTaskConstructor = typeof(CommandServerKeepCallbackQueueTask).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(CommandServerSocket), typeof(ServerMethodTypeEnum), typeof(bool) }, null).notNull();

        /// <summary>
        /// 控制器同步调用队列字段
        /// </summary>
        internal static readonly FieldInfo CommandServerControllerCallQueueField;
        /// <summary>
        /// 控制器同步调用低优先级队列字段
        /// </summary>
        internal static readonly FieldInfo CommandServerControllerCallQueueLowPriorityField;
        /// <summary>
        /// 控制器读写队列字段
        /// </summary>
        internal static readonly FieldInfo CommandServerControllerCallReadWriteQueueField;
        /// <summary>
        /// 控制器读写队列字段
        /// </summary>
        internal static readonly FieldInfo CommandServerControllerCallConcurrencyReadQueueField;

#pragma warning disable CS8618
        static ServerInterfaceController()
#pragma warning restore CS8618
        {
            foreach (MethodInfo method in typeof(CommandServerSocket).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                switch (method.Name.Length)
                {
                    case 10:
                        if (method.Name == nameof(CommandServerSocket.SendOutput)) CommandServerSocketSendOutputMethod = method;
                        break;
                    case 11:
                        if (method.Name == nameof(CommandServerSocket.Deserialize)) CommandServerSocketDeserializeMethod = method;
                        break;
                    case 24:
                        if (method.Name == nameof(CommandServerSocket.CallTaskQueueAppendQueue)) CommandServerSocketCallTaskQueueAppendQueueMethod = method;
                        break;
                    case 30:
                        if (method.Name == nameof(CommandServerSocket.CallTaskQueueAppendLowPriority)) CommandServerSocketCallTaskQueueAppendLowPriorityMethod = method;
                        break;
                }
            }
            foreach (FieldInfo field in typeof(CommandServerController).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                switch (field.Name.Length)
                {
                    case 9:
                        if (field.Name == nameof(CommandServerController.CallQueue)) CommandServerControllerCallQueueField = field;
                        break;
                    case 18:
                        if (field.Name == nameof(CommandServerController.CallReadWriteQueue)) CommandServerControllerCallReadWriteQueueField = field;
                        break;
                    case 20:
                        if (field.Name == nameof(CommandServerController.CallQueueLowPriority)) CommandServerControllerCallQueueLowPriorityField = field;
                        break;
                    case 24:
                        if (field.Name == nameof(CommandServerController.CallConcurrencyReadQueue)) CommandServerControllerCallConcurrencyReadQueueField = field;
                        break;
                }
            }
        }
    }
    /// <summary>
    /// 控制器接口信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class ServerInterfaceController<T>
    {
        /// <summary>
        /// 创建命令服务控制器
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controllerName"></param>
        /// <param name="controller"></param>
        /// <param name="getBindController"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static CommandServerController Create(CommandListener server, string controllerName, T controller, Func<CommandServerController, CommandServerSocket, CommandServerBindContextController>? getBindController)
#else
        internal static CommandServerController Create(CommandListener server, string controllerName, T controller, Func<CommandServerController, CommandServerSocket, CommandServerBindContextController> getBindController)
#endif
        {
            if (controllerConstructorException == null)
            {
#if NetStandard21
                CommandServerController commandServerController = (CommandServerController)controllerConstructorInfo.Invoke(new object?[] { server, controllerName, controller, getBindController });
#else
                CommandServerController commandServerController = (CommandServerController)controllerConstructorInfo.Invoke(new object[] { server, controllerName, controller, getBindController });
#endif
                if (controllerConstructorMessages == null) return commandServerController; 
                server.Config.OnControllerConstructorMessage(typeof(T), controllerConstructorMessages);
                return commandServerController;
            }
            throw controllerConstructorException;
        }
        /// <summary>
        /// 命令控制器配置
        /// </summary>
        internal static CommandServerControllerInterfaceAttribute ControllerAttribute;
        /// <summary>
        /// 服务端接口方法信息集合
        /// </summary>
#if NetStandard21
        internal static readonly ServerInterfaceMethod?[] Methods;
#else
        internal static readonly ServerInterfaceMethod[] Methods;
#endif
        /// <summary>
        /// 获取服务端接口方法信息集合
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        internal static ServerInterfaceMethod GetMethod(int methodIndex)
        {
            return Methods[methodIndex].notNull();
        }
        /// <summary>
        /// 控制器构造函数
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private static readonly ConstructorInfo controllerConstructorInfo;
        /// <summary>
        /// 控制器构造错误
        /// </summary>
#if NetStandard21
        private static readonly Exception? controllerConstructorException;
#else
        private static readonly Exception controllerConstructorException;
#endif
        /// <summary>
        /// 控制器构造提示信息
        /// </summary>
#if NetStandard21
        private static readonly string[]? controllerConstructorMessages;
#else
        private static readonly string[] controllerConstructorMessages;
#endif
        /// <summary>
        /// 检查服务控制器相关错误信息
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<string> Check()
        {
            if (controllerConstructorException != null) yield return controllerConstructorException.Message;
            if (controllerConstructorMessages != null)
            {
                foreach (string message in controllerConstructorMessages) yield return message;
            }
        }

        static ServerInterfaceController()
        {
            Type type = typeof(T);
            var currentMethod = default(ServerInterfaceMethod);
            Methods = EmptyArray<ServerInterfaceMethod>.Array;
            ControllerAttribute = CommandServerController.DefaultAttribute;
            try
            {
                ServerInterface serverInterface = new ServerInterface(type, null);
                if (serverInterface.Error != null)
                {
                    controllerConstructorException = new Exception($"{type.fullName()} 服务端控制器生成失败 {serverInterface.Error}");
                    return;
                }
                if (serverInterface.Messages.Length != 0) controllerConstructorMessages = serverInterface.Messages.ToArray();
                Methods = serverInterface.Methods.notNull();
                //if (AutoCSer.Common.IsCodeGenerator) return;
                ControllerAttribute = serverInterface.ControllerAttribute;

                Type[] constructorParameterTypes = new Type[] { typeof(CommandListener), typeof(string), type, typeof(Func<CommandServerController, CommandServerSocket, CommandServerBindContextController>) };
                TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".Net.CommandServer.InterfaceController." + type.FullName, TypeAttributes.Class | TypeAttributes.Sealed, typeof(CommandServerController<T>));
                #region 静态构造函数
                ConstructorBuilder staticConstructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null);
                ILGenerator staticConstructorGenerator = staticConstructorBuilder.GetILGenerator();
                #region MethodX = InterfaceController<T>.GetMethod(X);
                int methodIndex = 0;
                MethodInfo getServerInterfaceMethod = ((Func<int, ServerInterfaceMethod>)GetMethod).Method;
                foreach (var method in Methods)
                {
                    if (method != null)
                    {
                        bool isMethod = method.IsOutputInfo;
                        if (!isMethod)
                        {
                            switch (method.MethodType)
                            {
                                case ServerMethodTypeEnum.Task:
                                case ServerMethodTypeEnum.SendOnlyTask:
                                case ServerMethodTypeEnum.CallbackTask:
                                case ServerMethodTypeEnum.KeepCallbackTask:
                                case ServerMethodTypeEnum.KeepCallbackCountTask:
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                                    isMethod = true;
                                    break;
                            }
                        }
                        if (isMethod)
                        {
                            method.MethodFieldBuilder = typeBuilder.DefineField($"Method{methodIndex.toString()}", typeof(ServerInterfaceMethod), FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly);
                            staticConstructorGenerator.int32(methodIndex);
                            staticConstructorGenerator.call(getServerInterfaceMethod);
                            staticConstructorGenerator.Emit(OpCodes.Stsfld, method.MethodFieldBuilder);
                        }
                    }
                    ++methodIndex;
                }
                #endregion
                staticConstructorGenerator.Emit(OpCodes.Ret);
                #endregion
                #region 构造函数
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                #region base(server, controllerName, controller, getBindController, verifyMethodIndex, controllerQueue)
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.Emit(OpCodes.Ldarg_3);
                constructorGenerator.Emit(OpCodes.Ldarg_S, 4);
                constructorGenerator.int32(serverInterface.VerifyMethodIndex);
                constructorGenerator.int32(serverInterface.ControllerQueue);
                constructorGenerator.int32(serverInterface.ControllerConcurrencyReadQueue ? 1 : 0);
                constructorGenerator.int32(serverInterface.ControllerReadWriteQueue ? 1 : 0);
                constructorGenerator.Emit(OpCodes.Call, typeof(CommandServerController<T>).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(CommandListener), typeof(string), type, typeof(Func<CommandServerController, CommandServerSocket, CommandServerBindContextController>), typeof(int), typeof(byte), typeof(bool), typeof(bool) }, null).notNull());
                #endregion
                //#region Controller = controller;
                //FieldBuilder controllerFieldBuilder = typeBuilder.DefineField("Controller", type, FieldAttributes.Public | FieldAttributes.InitOnly);
                //constructorGenerator.Emit(OpCodes.Ldarg_0);
                //constructorGenerator.Emit(OpCodes.Ldarg_3);
                //constructorGenerator.Emit(OpCodes.Stfld, controllerFieldBuilder);
                //#endregion
                var queueFieldBuilders = default(KeyValue<FieldInfo, FieldInfo>[]);
                if (serverInterface.QueueCount != 0)
                {
                    queueFieldBuilders = new KeyValue<FieldInfo, FieldInfo>[serverInterface.Queues.Length];
                    for (int queueIndex = serverInterface.Queues.Length; queueIndex != 0;)
                    {//从大到小，避免频繁重组
                        if (serverInterface.Queues[--queueIndex] != 0)
                        {
                            #region QueueX = CommandListener.GetServerCallQueue(server, X);
                            queueFieldBuilders[queueIndex].Key = typeBuilder.DefineField($"Queue{queueIndex.toString()}", typeof(CommandServerCallQueue), FieldAttributes.Public | FieldAttributes.InitOnly);
                            constructorGenerator.Emit(OpCodes.Ldarg_0);
                            constructorGenerator.Emit(OpCodes.Ldarg_1);
                            constructorGenerator.int32(queueIndex);
                            constructorGenerator.call(ServerInterfaceController.CommandListenerGetServerCallQueue.Method);
                            constructorGenerator.Emit(OpCodes.Stfld, queueFieldBuilders[queueIndex].Key);
                            #endregion
                            #region queueLowPriorityX = CommandListener.GetServerCallQueueLowPriority(server, X);
                            if ((serverInterface.Queues[queueIndex] & 2) != 0)
                            {
                                queueFieldBuilders[queueIndex].Value = typeBuilder.DefineField($"queueLowPriority{queueIndex.toString()}", typeof(CommandServerCallLowPriorityQueue), FieldAttributes.Public | FieldAttributes.InitOnly);
                                constructorGenerator.Emit(OpCodes.Ldarg_0);
                                constructorGenerator.Emit(OpCodes.Ldarg_1);
                                constructorGenerator.int32(queueIndex);
                                constructorGenerator.call(ServerInterfaceController.CommandListenerGetServerCallQueueLowPriority.Method);
                                constructorGenerator.Emit(OpCodes.Stfld, queueFieldBuilders[queueIndex].Value);
                            }
                            #endregion
                        }
                    }
                }
                if (serverInterface.ControllerQueue != 0)
                {
                    if (queueFieldBuilders == null) queueFieldBuilders = new KeyValue<FieldInfo, FieldInfo>[1];
                    queueFieldBuilders[0].Set(ServerInterfaceController.CommandServerControllerCallQueueField, ServerInterfaceController.CommandServerControllerCallQueueLowPriorityField);
                }
                var concurrencyReadQueueFieldBuilder = default(FieldBuilder);
                if (serverInterface.IsConcurrencyReadQueue)
                {
                    #region ConcurrencyReadQueue = CommandListener.GetServerCallConcurrencyReadQueue(server);
                    concurrencyReadQueueFieldBuilder = typeBuilder.DefineField("ConcurrencyReadQueue", typeof(CommandServerCallReadQueue), FieldAttributes.Public | FieldAttributes.InitOnly);
                    constructorGenerator.Emit(OpCodes.Ldarg_0);
                    constructorGenerator.Emit(OpCodes.Ldarg_1);
                    constructorGenerator.call(ServerInterfaceController.CommandListenerGetServerCallConcurrencyReadQueue.Method);
                    constructorGenerator.Emit(OpCodes.Stfld, concurrencyReadQueueFieldBuilder);
                    #endregion
                }
                var concurrencyReadQueueField = serverInterface.ControllerConcurrencyReadQueue ? ServerInterfaceController.CommandServerControllerCallConcurrencyReadQueueField : default(FieldInfo);
                var readWriteQueueFieldBuilder = default(FieldBuilder);
                if (serverInterface.IsReadWriteQueue)
                {
                    #region ReadWriteQueue = CommandListener.GetServerCallReadWriteQueue(server);
                    readWriteQueueFieldBuilder = typeBuilder.DefineField("ReadWriteQueue", typeof(CommandServerCallReadQueue), FieldAttributes.Public | FieldAttributes.InitOnly);
                    constructorGenerator.Emit(OpCodes.Ldarg_0);
                    constructorGenerator.Emit(OpCodes.Ldarg_1);
                    constructorGenerator.call(ServerInterfaceController.CommandListenerGetServerCallReadWriteQueue.Method);
                    constructorGenerator.Emit(OpCodes.Stfld, readWriteQueueFieldBuilder);
                    #endregion
                }
                var readWriteQueueField = serverInterface.ControllerReadWriteQueue ? ServerInterfaceController.CommandServerControllerCallReadWriteQueueField : default(FieldInfo);
                #region taskQueueSetX = (ServerCallTaskQueueSet<X>)CommandListener.GetServerCallTaskQueueSet<X>(server);
                if (serverInterface.TaskQueueFieldBuilders != null)
                {
                    int queueTypeIndex = 0;
                    foreach (HashObject<Type> taskQueueType in serverInterface.TaskQueueFieldBuilders.Keys.getArray())
                    {
                        EquatableGenericType equatableGenericType = EquatableGenericType.Get(taskQueueType);
                        FieldBuilder taskQueueSetFieldBuilder = typeBuilder.DefineField($"taskQueueSet{queueTypeIndex.toString()}", equatableGenericType.ServerCallTaskQueueSetType, FieldAttributes.Public | FieldAttributes.InitOnly);
                        constructorGenerator.Emit(OpCodes.Ldarg_0);
                        constructorGenerator.Emit(OpCodes.Ldarg_1);
                        constructorGenerator.call(equatableGenericType.CommandListenerGetServerCallTaskQueueSetDelegate.Method);
                        constructorGenerator.Emit(OpCodes.Stfld, taskQueueSetFieldBuilder);
                        serverInterface.TaskQueueFieldBuilders[taskQueueType] = taskQueueSetFieldBuilder;
                        ++queueTypeIndex;
                    }
                }
                #endregion
                constructorGenerator.Emit(OpCodes.Ret);
                #endregion
                #region public override void DoCommand(CommandServerSocket socket, ref SubArray<byte> data)
                MethodBuilder doCommandMethodBuilder = typeBuilder.DefineMethod(nameof(CommandServerController.DoCommand), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(CommandClientReturnTypeEnum), ServerInterfaceController.DoCommandParameterTypes);
                ILGenerator doCommandGenerator = doCommandMethodBuilder.GetILGenerator();
                #region switch (CommandServerSocket.GetCommandMethodIndex(socket))
                Label doCommandReturnDeserializeErrorLabel;
                Label[] doCommandLabels = ServerInterfaceMethod.DoCommandSwitchMethodIndex(doCommandGenerator, Methods, out doCommandReturnDeserializeErrorLabel);
                LocalBuilder controllerLocalBuilder = doCommandGenerator.DeclareLocal(type);
                var timestampLocalBuilder = default(LocalBuilder);
                MethodInfo getControllerMethod = ((Func<CommandServerController<T>, CommandServerSocket, T>)CommandServerController<T>.GetController).Method;
                MethodInfo getQueueNodeControllerMethod = ((Func<CommandServerController<T>, CommandServerCallQueueNode, T>)CommandServerController<T>.GetQueueNodeController).Method;
                MethodInfo getReadWriteQueueNodeControllerMethod = ((Func<CommandServerController<T>, CommandServerCallReadWriteQueueNode, T>)CommandServerController<T>.GetReadWriteQueueNodeController).Method;
                MethodInfo getConcurrencyReadQueueNodeControllerMethod = ((Func<CommandServerController<T>, CommandServerCallConcurrencyReadQueueNode, T>)CommandServerController<T>.GetConcurrencyReadQueueNodeController).Method;
                #endregion
                methodIndex = 0;
                foreach (var method in Methods)
                {
                    if (method != null)
                    {
                        currentMethod = method;
                        doCommandGenerator.MarkLabel(doCommandLabels[methodIndex]);
                        if (method.MethodType == ServerMethodTypeEnum.VersionExpired)
                        {
                            #region return CommandServerReturnType.VersionExpired;
                            doCommandGenerator.int32((byte)CommandClientReturnTypeEnum.VersionExpired);
                            doCommandGenerator.Emit(OpCodes.Ret);
                            #endregion
                        }
                        else
                        {
                            Label runTaskLabel = default(Label);
                            var inputParameterLocalBuilder = method.InputParameterDeserialize(doCommandGenerator, ref doCommandReturnDeserializeErrorLabel, ref runTaskLabel);
                            var asynchronousConstructorBuilder = default(ConstructorBuilder);
                            bool returnSuccess = true;
                            switch (method.MethodType)
                            {
                                case ServerMethodTypeEnum.Synchronous:
                                case ServerMethodTypeEnum.SendOnly:
                                    #region SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
                                    var outputParameterLocalBuilder = method.GetOutputParameterLocalBuilder(doCommandGenerator);
                                    #endregion
                                    #region outputParameter.__Return__ = GetController(this, socket).X(socket, inputParameter.X, out outputParameter.X);
                                    if (method.OutputParameterType != null && method.ReturnValueType != typeof(void))
                                    {
                                        doCommandGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                                    }
                                    method.CallMethodParameter(doCommandGenerator, getControllerMethod, controllerLocalBuilder, inputParameterLocalBuilder, outputParameterLocalBuilder);
                                    method.CallMethod(doCommandGenerator, type, null, ref outputParameterLocalBuilder);
                                    #endregion
                                    #region outputParameter.Ref = inputParameter.Ref;
                                    if (method.OutputParameterType != null && method.InputParameterType != null)
                                    {
                                        int parameterIndex = 0;
                                        foreach (ParameterInfo parameter in method.InputParameters)
                                        {
                                            if (parameter.ParameterType.IsByRef)
                                            {
                                                doCommandGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                                                doCommandGenerator.Emit(OpCodes.Ldloc, inputParameterLocalBuilder.notNull());
                                                doCommandGenerator.Emit(OpCodes.Ldfld, method.InputParameterFields[parameterIndex]);
                                                doCommandGenerator.Emit(OpCodes.Stfld, method.GetOutputParameterField(parameter.Name.notNull()).notNull());
                                            }
                                            ++parameterIndex;
                                        }
                                    }
                                    #endregion
                                    #region CommandServerSocket.SetVerifyState(socket, outputParameter.__Return__);
                                    if (methodIndex == serverInterface.VerifyMethodIndex)
                                    {
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        if (method.OutputParameterType == null) doCommandGenerator.Emit(OpCodes.Ldloc, outputParameterLocalBuilder.notNull());
                                        else
                                        {
                                            doCommandGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                                            doCommandGenerator.Emit(OpCodes.Ldfld, method.OutputParameterFields[method.OutputParameterCount]);
                                        }
                                        doCommandGenerator.call(ServerInterfaceController.CommandServerSocketSetVerifyState.Method);
                                    }
                                    #endregion
                                    #region CommandServerSocket.Send(socket, OutputInfo0, ref outputParameter);
                                    if (method.MethodType != ServerMethodTypeEnum.SendOnly)
                                    {
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        if (method.OutputParameterType == null)
                                        {
                                            if (method.ReturnValueType == typeof(void)) doCommandGenerator.call(ServerInterfaceController.CommandServerSocketSendSuccess.Method);
                                            else
                                            {
                                                doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                                doCommandGenerator.Emit(OpCodes.Ldloc, outputParameterLocalBuilder.notNull());
                                                doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CommandServerSocketSendReturnValueDelegate.Method);
                                            }
                                        }
                                        else
                                        {
                                            doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                            doCommandGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                                            doCommandGenerator.call(ServerInterfaceController.CommandServerSocketSendOutputMethod.MakeGenericMethod(method.OutputParameterType.Type));
                                        }
                                    }
                                    #endregion
                                    doCommandGenerator.Emit(OpCodes.Pop);
                                    break;
                                case ServerMethodTypeEnum.Callback:
                                case ServerMethodTypeEnum.CallbackTask:
                                case ServerMethodTypeEnum.CallbackQueue:
                                case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                                case ServerMethodTypeEnum.CallbackReadWriteQueue:
                                case ServerMethodTypeEnum.CallbackTaskQueue:
                                    var returnGenericType = default(GenericType);
                                    if (method.ReturnValueType == typeof(void))
                                    {
                                        asynchronousConstructorBuilder = null;
                                        goto CALLMETHOD;
                                    }
                                    #region public sealed class AsynchronousCallback : ServerCallback<X>
                                    returnGenericType = GenericType.Get(method.ReturnValueType);
                                    Type serverCallbackType = returnGenericType.GetCommandServerCallbackType(method);
                                    TypeBuilder asynchronousTypeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(ServerInterfaceController.GetAsynchronousTypeName(), TypeAttributes.Class | TypeAttributes.Sealed, serverCallbackType);
                                    #endregion
                                    var asynchronousQueueFieldBuilder = default(FieldBuilder);
                                    if (methodIndex != serverInterface.VerifyMethodIndex)
                                    {
                                        switch (method.MethodType)
                                        {
                                            case ServerMethodTypeEnum.CallbackQueue:
                                                asynchronousQueueFieldBuilder = asynchronousTypeBuilder.DefineField("queue", typeof(CommandServerCallQueue), FieldAttributes.Public | FieldAttributes.InitOnly);
                                                break;
                                            case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                                                asynchronousQueueFieldBuilder = asynchronousTypeBuilder.DefineField("queue", typeof(CommandServerCallConcurrencyReadQueue), FieldAttributes.Public | FieldAttributes.InitOnly);
                                                break;
                                            case ServerMethodTypeEnum.CallbackReadWriteQueue:
                                                asynchronousQueueFieldBuilder = asynchronousTypeBuilder.DefineField("queue", typeof(CommandServerCallReadQueue), FieldAttributes.Public | FieldAttributes.InitOnly);
                                                break;
                                        }
                                    }
                                    #region public AsynchronousCallback(CommandServerSocket socket) : base(socket) { }
                                    Type[] asynchronousConstructorParameterTypeArray, serverCallbackConstructorParameterTypeArray;
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.CallbackQueue:
                                            asynchronousConstructorParameterTypeArray = ServerInterfaceController.ServerCallQueueParameterTypes;
                                            serverCallbackConstructorParameterTypeArray = ServerInterfaceController.ServerCallQueueNodeParameterTypes;
                                            break;
                                        case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                                            asynchronousConstructorParameterTypeArray = ServerInterfaceController.ServerCallConcurrencyReadQueueParameterTypes;
                                            serverCallbackConstructorParameterTypeArray = ServerInterfaceController.ServerCallConcurrencyReadQueueNodeParameterTypes;
                                            break;
                                        case ServerMethodTypeEnum.CallbackReadWriteQueue:
                                            asynchronousConstructorParameterTypeArray = ServerInterfaceController.ServerCallReadWriteQueueParameterTypes;
                                            serverCallbackConstructorParameterTypeArray = ServerInterfaceController.ServerCallReadWriteQueueNodeParameterTypes;
                                            break;
                                        case ServerMethodTypeEnum.CallbackTaskQueue:
                                            serverCallbackConstructorParameterTypeArray = asynchronousConstructorParameterTypeArray = ServerInterfaceController.ServerCallTaskQueueParameterTypes;
                                            break;
                                        default:
                                            serverCallbackConstructorParameterTypeArray = asynchronousConstructorParameterTypeArray = ServerInterfaceController.CommandServerSocketParameterTypes;
                                            break;
                                    }
                                    asynchronousConstructorBuilder = asynchronousTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, asynchronousConstructorParameterTypeArray);
                                    ILGenerator asynchronousConstructorGenerator = asynchronousConstructorBuilder.GetILGenerator();
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_1);
                                    if (asynchronousQueueFieldBuilder != null)
                                    {
                                        
                                        asynchronousConstructorGenerator.Emit(OpCodes.Call, serverCallbackType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, serverCallbackConstructorParameterTypeArray, null).notNull());
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_2);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Stfld, asynchronousQueueFieldBuilder);
                                    }
                                    else
                                    {
                                        if (asynchronousConstructorParameterTypeArray.Length == 2) asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_2);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Call, serverCallbackType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, serverCallbackConstructorParameterTypeArray, null).notNull());
                                    }
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                    MethodBuilder asynchronousMethodBuilder;
                                    ILGenerator asynchronousMethodGenerator;
                                    if (returnGenericType != null)
                                    {
                                        #region public override bool Callback(X returnValue)
                                        asynchronousMethodBuilder = asynchronousTypeBuilder.DefineMethod(nameof(CommandServerCallback.Callback), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(bool), new Type[] { method.ReturnValueType });
                                        asynchronousMethodGenerator = asynchronousMethodBuilder.GetILGenerator();
                                        #endregion
                                        #region return CommandServerSocket.Callback<X>(this, InterfaceControllerIL<T>.OutputInfo0, returnValue);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_1);
                                        asynchronousMethodGenerator.call(methodIndex == serverInterface.VerifyMethodIndex ? ServerInterfaceController.ServerCallSendVerifyState.Method : returnGenericType.CommandServerCallbackDelegate.Method);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ret);
                                        #endregion
                                    }
                                    if (asynchronousQueueFieldBuilder != null && method.MethodType == ServerMethodTypeEnum.CallbackQueue)
                                    {
                                        #region internal override bool SynchronousCallback(X returnValue)
                                        asynchronousMethodBuilder = asynchronousTypeBuilder.DefineMethod(nameof(CommandServerCallback<string>.SynchronousCallback), MethodAttributes.Assembly | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(bool), new Type[] { method.ReturnValueType });
                                        asynchronousMethodGenerator = asynchronousMethodBuilder.GetILGenerator();
                                        #endregion
                                        #region return CommandServerSocket.SynchronousCallback<X>(this.queue, this, InterfaceControllerIL<T>.OutputInfo0, returnValue);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldfld, asynchronousQueueFieldBuilder);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_1);
                                        asynchronousMethodGenerator.call(returnGenericType.notNull().CommandServerSynchronousCallbackDelegate.Method);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ret);
                                        #endregion
                                    }
                                    method.AsynchronousType = asynchronousTypeBuilder.CreateType();
                                    CALLMETHOD:
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.CallbackTask: goto TASK;
                                        case ServerMethodTypeEnum.CallbackTaskQueue: goto TASKQUEUE;
                                        case ServerMethodTypeEnum.CallbackQueue: goto QUEUE;
                                        case ServerMethodTypeEnum.CallbackConcurrencyReadQueue: goto LONGREADQUEUE;
                                        case ServerMethodTypeEnum.CallbackReadWriteQueue: goto READWRITEQUEUE;
                                    }
                                    #region GetController(this, socket).X(socket, inputParameter.X, new AsynchronousCallback(socket));
                                    method.CallMethodParameter(doCommandGenerator, getControllerMethod, controllerLocalBuilder, inputParameterLocalBuilder);
                                    method.CreateServerCallback(doCommandGenerator, asynchronousConstructorBuilder);
                                    doCommandGenerator.Emit(OpCodes.Constrained, type);
                                    doCommandGenerator.call(method.Method);
                                    #endregion
                                    break;
                                case ServerMethodTypeEnum.KeepCallback:
                                    #region CommandServerKeepCallback keepCallback = CommandServerKeepCallback<X>.CreateServerKeepCallback(socket, Method0);
                                    var keepCallbackLocalBuilder = default(LocalBuilder);
                                    if (method.MethodAttribute.AutoCancelKeep)
                                    {
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        if (method.ReturnValueType == typeof(void)) doCommandGenerator.call(ServerInterfaceController.CreateCommandServerKeepCallbackDelegate.Method);
                                        else
                                        {
                                            doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                            doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackDelegate.Method);
                                        }
                                        doCommandGenerator.Emit(OpCodes.Stloc, keepCallbackLocalBuilder = doCommandGenerator.DeclareLocal(typeof(CommandServerKeepCallback)));
                                    }
                                    #endregion
                                    #region GetController(this, socket).X(socket, inputParameter.X, new ServerKeepCallback<X>(socket, OutputInfo0));
                                    method.CallMethodParameter(doCommandGenerator, getControllerMethod, controllerLocalBuilder, inputParameterLocalBuilder);
                                    if (keepCallbackLocalBuilder == null)
                                    {
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        if (method.ReturnValueType == typeof(void)) doCommandGenerator.call(ServerInterfaceController.CreateCommandServerKeepCallbackDelegate.Method);
                                        else
                                        {
                                            doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                            doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackDelegate.Method);
                                        }
                                    }
                                    else doCommandGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder);
                                    doCommandGenerator.Emit(OpCodes.Constrained, type);
                                    doCommandGenerator.call(method.Method);
                                    #region CommandServerKeepCallback.CancelKeep(keepCallback);
                                    if (keepCallbackLocalBuilder != null)
                                    {
                                        doCommandGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder);
                                        doCommandGenerator.call(ServerInterfaceController.CommandServerKeepCallbackCancelKeep.Method);
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                case ServerMethodTypeEnum.KeepCallbackCount:
                                    #region GetController(this, socket).X(socket, inputParameter.X, new ServerKeepCallbackCount<X>(socket, OutputInfo0));
                                    method.CallMethodParameter(doCommandGenerator, getControllerMethod, controllerLocalBuilder, inputParameterLocalBuilder);
                                    doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                    if (method.ReturnValueType == typeof(void))
                                    {
                                        doCommandGenerator.int32(Math.Max(method.MethodAttribute.KeepCallbackOutputCount, 1));
                                        doCommandGenerator.call(ServerInterfaceController.CreateCommandServerKeepCallbackCountDelegate.Method);
                                    }
                                    else
                                    {
                                        doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                        doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackCountDelegate.Method);
                                    }
                                    doCommandGenerator.Emit(OpCodes.Constrained, type);
                                    doCommandGenerator.call(method.Method);
                                    #endregion
                                    break;
                                case ServerMethodTypeEnum.Queue:
                                case ServerMethodTypeEnum.SendOnlyQueue:
                                case ServerMethodTypeEnum.KeepCallbackQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountQueue:
                                QUEUE:
                                    Type queueNodeType = typeof(CommandServerCallQueueNode);
                                    ConstructorInfo queueNodeConstructor = ServerInterfaceController.ServerCallQueueNodeConstructor;
                                    MethodInfo setDeserializeMethod = ServerInterfaceController.CommandServerCallQueueNodeSetIsDeserialize.Method;
                                    MethodInfo socketIsCloseMethod = ServerInterfaceController.CommandServerCallQueueNodeSocketIsClose.Method;
                                    MethodInfo queueGetSocketMethod = ServerInterfaceController.ServerCallQueueNodeGetSocket.Method;
                                    MethodInfo closeShortLinkMethod = ServerInterfaceController.ServerCallQueueNodeCloseShortLink.Method;
                                    MethodInfo queueNodeControllerMethod = getQueueNodeControllerMethod;
                                    FieldInfo queueField = queueFieldBuilders.notNull()[method.MethodAttribute.QueueIndex].Key;
                                    FieldInfo queueParameterField = method.IsLowPriorityQueue ? queueFieldBuilders.notNull()[method.MethodAttribute.QueueIndex].Value : queueField;
                                    MethodInfo checkOfflineCountMethod = ServerInterfaceController.CommandServerCallQueueNodeCheckOfflineCount.Method;
                                    MethodInfo keepCallbackQueueNodeMethod = method.ReturnValueType == typeof(void) ? ServerInterfaceController.CreateCommandServerKeepCallbackQueueNodeDelegate.Method : GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackQueueNodeDelegate.Method;
                                    MethodInfo createServerCallbackMethod = ServerInterfaceController.CreateServerCallbackCallQueueNodeDelegate.Method;
                                    MethodInfo createServerCallbackCountMethod = method.ReturnValueType == typeof(void) ? ServerInterfaceController.CreateCommandServerKeepCallbackCountQueueNodeDelegate.Method : GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackCountQueueNodeDelegate.Method;
                                    MethodInfo setVerifyStateMethod = ServerInterfaceController.ServerCallQueueNodeSetVerifyState.Method;
                                    MethodInfo queueSendMethod;
                                    if (method.OutputParameterType == null)
                                    {
                                        if (method.ReturnValueType == typeof(void)) queueSendMethod = ServerInterfaceController.ServerCallQueueNodeSendSuccess.Method;
                                        else queueSendMethod = GenericType.Get(method.ReturnValueType).CommandServerCallQueueSendReturnValueDelegate.Method;
                                    }
                                    else queueSendMethod = StructGenericType.Get(method.OutputParameterType.Type).CommandServerCallQueueSend.Method;
                                    MethodInfo addQueueNodeMethod;
                                    if (method.InputParameterType != null)
                                    {
                                        addQueueNodeMethod = method.IsLowPriorityQueue ? ServerInterfaceController.ServerCallQueueLowPriorityLinkAddIsDeserialize.Method : ServerInterfaceController.ServerCallQueueAddIsDeserialize.Method;
                                    }
                                    else
                                    {
                                        addQueueNodeMethod = method.IsLowPriorityQueue ? ServerInterfaceController.ServerCallQueueLowPriorityLinkAdd.Method : ServerInterfaceController.ServerCallQueueAdd.Method;
                                    }
                                ALLQUEUE:
                                    #region public sealed class SynchronousQueue : ServerCallQueueNode
                                    asynchronousTypeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(ServerInterfaceController.GetAsynchronousTypeName(), TypeAttributes.Class | TypeAttributes.Sealed, queueNodeType);
                                    #endregion
                                    #region public SynchronousQueue(CommandServerSocket socket, InterfaceControllerIL<T> controller, ref SubArray<byte> data) : base(socket)
                                    var callbackQueueAsynchronousConstructorBuilder = asynchronousConstructorBuilder;
                                    Type[] constructorParameterTypeArray;
                                    if(method.InputParameterType != null)
                                    {
                                        constructorParameterTypeArray = new Type[] { typeof(CommandServerSocket), typeof(object), ServerInterfaceController.ByteSubArrayRefType };
                                    }
                                    else constructorParameterTypeArray = new Type[] { typeof(CommandServerSocket), typeof(object) };
                                    asynchronousConstructorBuilder = asynchronousTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypeArray);
                                    asynchronousConstructorGenerator = asynchronousConstructorBuilder.GetILGenerator();
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_1);
                                    //asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_2);
                                    asynchronousConstructorGenerator.int32((byte)method.MethodType);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Call, queueNodeConstructor);
                                    #endregion
                                    #region this.controller = controller;
                                    FieldBuilder asynchronousControllerFieldBuilder = asynchronousTypeBuilder.DefineField("controller", typeof(object), FieldAttributes.Public | FieldAttributes.InitOnly);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_2);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Stfld, asynchronousControllerFieldBuilder);
                                    #endregion
                                    #region CommandServerCallQueueNode.SetIsDeserialize(this, CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true));
                                    var inputParameterFieldBuilder = default(FieldBuilder);
                                    if (method.InputParameterType != null)
                                    {
                                        inputParameterFieldBuilder = asynchronousTypeBuilder.DefineField("inputParameter", method.InputParameterType.Type, FieldAttributes.Public);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_1);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_3);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldflda, inputParameterFieldBuilder);
                                        asynchronousConstructorGenerator.int32(method.IsSimpleDeserializeParamter);
                                        asynchronousConstructorGenerator.call(ServerInterfaceController.CommandServerSocketDeserializeMethod.MakeGenericMethod(method.InputParameterType.Type));
                                        asynchronousConstructorGenerator.call(setDeserializeMethod);
                                    }
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                    #region public override void RunTask()
                                    asynchronousMethodBuilder = asynchronousTypeBuilder.DefineMethod(nameof(QueueTaskNode.RunTask), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(void), EmptyArray<Type>.Array);
                                    asynchronousMethodGenerator = asynchronousMethodBuilder.GetILGenerator();
                                    #endregion
                                    #region if (!CommandServerCallQueueNode.SocketIsClose(this))
                                    Label returnLabel = asynchronousMethodGenerator.DefineLabel();
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousMethodGenerator.call(socketIsCloseMethod);
                                    asynchronousMethodGenerator.Emit(OpCodes.Brtrue, returnLabel);
                                    #endregion
                                    keepCallbackLocalBuilder = null;
                                    LocalBuilder asynchronousControllerLocalBuilder = asynchronousMethodGenerator.DeclareLocal(type);
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.SendOnlyQueue:
                                        case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                                        case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                                            #region AutoCSer.Net.CommandServerCallQueueNode.CloseShortLink(this);
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                            asynchronousMethodGenerator.call(closeShortLinkMethod);
                                            #endregion
                                            #region GetQueueNodeController(controller, this).X(ServerCallQueueNode.GetSocket(this), controller.Queue0, inputParameter.X);
                                            method.CallMethodParameter(asynchronousMethodGenerator, asynchronousControllerFieldBuilder, queueNodeControllerMethod, asynchronousControllerLocalBuilder
                                                , queueGetSocketMethod, null, queueField, null, inputParameterFieldBuilder);
                                            asynchronousMethodGenerator.Emit(OpCodes.Constrained, type);
                                            asynchronousMethodGenerator.call(method.Method);
                                            asynchronousMethodGenerator.Emit(OpCodes.Pop);
                                            #endregion
                                            #region CommandServerCallQueueNode.CheckOfflineCount(this);
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                            asynchronousMethodGenerator.call(checkOfflineCountMethod);
                                            #endregion
                                            break;
                                        case ServerMethodTypeEnum.KeepCallbackQueue:
                                        case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                                        case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                                            #region CommandServerKeepCallback keepCallback = CommandServerKeepCallback<X>.CreateServerKeepCallback(socket, Method0);
                                            if (method.MethodAttribute.AutoCancelKeep)
                                            {
                                                asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                if (method.ReturnValueType != typeof(void)) asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                                asynchronousMethodGenerator.call(keepCallbackQueueNodeMethod);
                                                asynchronousMethodGenerator.Emit(OpCodes.Stloc, keepCallbackLocalBuilder = asynchronousMethodGenerator.DeclareLocal(typeof(CommandServerKeepCallback)));
                                            }
                                            #endregion
                                            goto QUEUECALL;
                                        default:
                                        QUEUECALL:
                                            #region SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
                                            outputParameterLocalBuilder = method.GetOutputParameterLocalBuilder(asynchronousMethodGenerator);
                                            #endregion
                                            #region outputParameter.__Return__ = GetQueueNodeController(controller, this).X(ServerCallQueueNode.GetSocket(this), controller.Queue0, inputParameter.X, out outputParameter.X);
                                            if (method.OutputParameterType != null && method.ReturnValueType != typeof(void))
                                            {
                                                asynchronousMethodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                                            }
                                            method.CallMethodParameter(asynchronousMethodGenerator, asynchronousControllerFieldBuilder, queueNodeControllerMethod, asynchronousControllerLocalBuilder
                                                , queueGetSocketMethod, null, queueField, null, inputParameterFieldBuilder, outputParameterLocalBuilder);
                                            switch (method.MethodType)
                                            {
                                                case ServerMethodTypeEnum.CallbackQueue:
                                                case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                                                case ServerMethodTypeEnum.CallbackReadWriteQueue:
                                                    if (method.ReturnValueType == typeof(void))
                                                    {
                                                        #region CommandServerCallback.CreateServerCallback(this)
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                        asynchronousMethodGenerator.call(createServerCallbackMethod);
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region new AsynchronousCallback(this, controller.Queue0)
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldfld, asynchronousControllerFieldBuilder);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldfld, queueField);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Newobj, callbackQueueAsynchronousConstructorBuilder.notNull());
                                                        #endregion
                                                    }
                                                    break;
                                                case ServerMethodTypeEnum.KeepCallbackQueue:
                                                case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                                                case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                                                    #region new ServerKeepCallback<X>(this, InterfaceControllerIL<T>.OutputInfo0)
                                                    if (keepCallbackLocalBuilder == null)
                                                    {
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                        if (method.ReturnValueType != typeof(void)) asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                                        asynchronousMethodGenerator.call(keepCallbackQueueNodeMethod);
                                                    }
                                                    else asynchronousMethodGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder);
                                                    #endregion
                                                    break;
                                                case ServerMethodTypeEnum.KeepCallbackCountQueue:
                                                case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                                                case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                                                    #region new ServerKeepCallbackCount<X>(this, InterfaceControllerIL<T>.OutputInfo0)
                                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                    if (method.ReturnValueType == typeof(void)) asynchronousMethodGenerator.int32(Math.Max(method.MethodAttribute.KeepCallbackOutputCount, 1));
                                                    else asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                                    asynchronousMethodGenerator.call(createServerCallbackCountMethod);
                                                    #endregion
                                                    break;
                                            }
                                            method.CallMethod(asynchronousMethodGenerator, type, null, ref outputParameterLocalBuilder);
                                            #endregion
                                            #region CommandServerKeepCallback.CancelKeep(keepCallback);
                                            if (keepCallbackLocalBuilder != null)
                                            {
                                                asynchronousMethodGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder);
                                                asynchronousMethodGenerator.call(ServerInterfaceController.CommandServerKeepCallbackCancelKeep.Method);
                                            }
                                            #endregion
                                            #region outputParameter.Ref = inputParameter.Ref;
                                            if (method.OutputParameterType != null && method.InputParameterType != null)
                                            {
                                                int parameterIndex = 0;
                                                foreach (ParameterInfo parameter in method.InputParameters)
                                                {
                                                    if (parameter.ParameterType.IsByRef)
                                                    {
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldflda, inputParameterFieldBuilder.notNull());
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldfld, method.InputParameterFields[parameterIndex]);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Stfld, method.GetOutputParameterField(parameter.Name.notNull()).notNull());
                                                    }
                                                    ++parameterIndex;
                                                }
                                            }
                                            #endregion
                                            switch (method.MethodType)
                                            {
                                                case ServerMethodTypeEnum.Queue:
                                                case ServerMethodTypeEnum.ConcurrencyReadQueue:
                                                case ServerMethodTypeEnum.ReadWriteQueue:
                                                    #region ServerCallQueueNode.SetVerifyState(this, outputParameter.__Return__);
                                                    if (methodIndex == serverInterface.VerifyMethodIndex)
                                                    {
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                        if (method.OutputParameterType == null) asynchronousMethodGenerator.Emit(OpCodes.Ldloc, outputParameterLocalBuilder.notNull());
                                                        else
                                                        {
                                                            asynchronousMethodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                                                            asynchronousMethodGenerator.Emit(OpCodes.Ldfld, method.OutputParameterFields[method.OutputParameterCount]);
                                                        }
                                                        asynchronousMethodGenerator.call(setVerifyStateMethod);
                                                    }
                                                    #endregion
                                                    #region CommandServerSocket.CallQueueSend(this, controller.Queue0, InterfaceControllerIL<T>.OutputInfo0, ref outputParameter);
                                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                    if (method.MethodType == ServerMethodTypeEnum.Queue)
                                                    {
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldfld, asynchronousControllerFieldBuilder);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldfld, queueField);
                                                    }
                                                    if (method.OutputParameterType == null)
                                                    {
                                                        if (method.ReturnValueType != typeof(void))
                                                        {
                                                            asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                                            asynchronousMethodGenerator.Emit(OpCodes.Ldloc, outputParameterLocalBuilder.notNull());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                                        asynchronousMethodGenerator.Emit(OpCodes.Ldloca, outputParameterLocalBuilder.notNull());
                                                    }
                                                    asynchronousMethodGenerator.call(queueSendMethod);
                                                    asynchronousMethodGenerator.Emit(OpCodes.Pop);
                                                    #endregion
                                                    break;
                                            }
                                            #region CommandServerCallQueueNode.CheckOfflineCount(this);
                                            switch (method.MethodType)
                                            {
                                                case ServerMethodTypeEnum.CallbackQueue:
                                                case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                                                case ServerMethodTypeEnum.CallbackReadWriteQueue:
                                                    break;
                                                default:
                                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                                    asynchronousMethodGenerator.call(checkOfflineCountMethod);
                                                    break;
                                            }
                                            #endregion
                                            break;
                                    }
                                    asynchronousMethodGenerator.MarkLabel(returnLabel);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ret);
                                    method.AsynchronousType = asynchronousTypeBuilder.CreateType();
                                    if (method.InputParameterType != null)
                                    {
                                        #region return CommandServerCallQueue.AddIsDeserialize(Queue0, new SynchronousParameterQueue(socket, this, ref data));
                                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                        doCommandGenerator.Emit(OpCodes.Ldfld, queueParameterField);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_2);
                                        returnSuccess = false;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region ServerCallQueue.Add(Queue0, new SynchronousQueue(socket, this, ref inputParameter));
                                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                        doCommandGenerator.Emit(OpCodes.Ldfld, queueParameterField);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                        #endregion
                                    }
                                    doCommandGenerator.Emit(OpCodes.Newobj, asynchronousConstructorBuilder);
                                    doCommandGenerator.call(addQueueNodeMethod);
                                    break;
                                case ServerMethodTypeEnum.ConcurrencyReadQueue:
                                case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                                case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                                LONGREADQUEUE:
                                    queueNodeType = typeof(CommandServerCallConcurrencyReadQueueNode);
                                    queueNodeConstructor = ServerInterfaceController.ServerCallConcurrencyReadQueueNodeConstructor;
                                    setDeserializeMethod = ServerInterfaceController.CommandServerCallConcurrencyReadQueueNodeSetIsDeserialize.Method;
                                    socketIsCloseMethod = ServerInterfaceController.CommandServerCallConcurrencyReadQueueNodeSocketIsClose.Method;
                                    queueGetSocketMethod = ServerInterfaceController.ServerCallConcurrencyReadQueueNodeGetSocket.Method;
                                    closeShortLinkMethod = ServerInterfaceController.ServerCallConcurrencyReadQueueNodeCloseShortLink.Method;
                                    queueNodeControllerMethod = getConcurrencyReadQueueNodeControllerMethod;
                                    queueParameterField = queueField = method.MethodAttribute.IsControllerConcurrencyReadQueue ? concurrencyReadQueueField.notNull() : concurrencyReadQueueFieldBuilder.notNull();
                                    checkOfflineCountMethod = ServerInterfaceController.CommandServerCallConcurrencyReadQueueNodeCheckOfflineCount.Method;
                                    keepCallbackQueueNodeMethod = method.ReturnValueType == typeof(void) ? ServerInterfaceController.CreateCommandServerKeepCallbackConcurrencyReadQueueNodeDelegate.Method : GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackConcurrencyReadQueueNodeDelegate.Method;
                                    createServerCallbackMethod = ServerInterfaceController.CreateServerCallbackCallConcurrencyReadQueueNodeDelegate.Method;
                                    createServerCallbackCountMethod = method.ReturnValueType == typeof(void) ? ServerInterfaceController.CreateCommandServerKeepCallbackCountConcurrencyReadQueueNodeDelegate.Method : GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackCountConcurrencyReadQueueNodeDelegate.Method;
                                    setVerifyStateMethod = ServerInterfaceController.ServerCallConcurrencyReadQueueNodeSetVerifyState.Method;
                                    if (method.OutputParameterType == null)
                                    {
                                        if (method.ReturnValueType == typeof(void)) queueSendMethod = ServerInterfaceController.ServerCallConcurrencyReadQueueNodeSendSuccess.Method;
                                        else queueSendMethod = GenericType.Get(method.ReturnValueType).CommandServerCallConcurrencyReadQueueSendReturnValueDelegate.Method;
                                    }
                                    else queueSendMethod = StructGenericType.Get(method.OutputParameterType.Type).CommandServerCallConcurrencyReadQueueSend.Method;
                                    if (method.InputParameterType != null)
                                    {
                                        addQueueNodeMethod = method.IsWriteQueue ? ServerInterfaceController.ServerCallReadWriteQueueAppendWriteIsDeserialize.Method : ServerInterfaceController.ServerCallReadWriteQueueAppendReadIsDeserialize.Method;
                                    }
                                    else
                                    {
                                        addQueueNodeMethod = method.IsWriteQueue ? ServerInterfaceController.ServerCallReadWriteQueueAppendWrite.Method : ServerInterfaceController.ServerCallReadWriteQueueAppendRead.Method;
                                    }
                                    goto ALLQUEUE;
                                case ServerMethodTypeEnum.ReadWriteQueue:
                                case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                                case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                                READWRITEQUEUE:
                                    queueNodeType = typeof(CommandServerCallReadWriteQueueNode);
                                    queueNodeConstructor = ServerInterfaceController.ServerCallReadWriteQueueNodeConstructor;
                                    setDeserializeMethod = ServerInterfaceController.CommandServerCallReadWriteQueueNodeSetIsDeserialize.Method;
                                    socketIsCloseMethod = ServerInterfaceController.CommandServerCallReadWriteQueueNodeSocketIsClose.Method;
                                    queueGetSocketMethod = ServerInterfaceController.ServerCallReadWriteQueueNodeGetSocket.Method;
                                    closeShortLinkMethod = ServerInterfaceController.ServerCallReadWriteQueueNodeCloseShortLink.Method;
                                    queueNodeControllerMethod = getReadWriteQueueNodeControllerMethod;
                                    queueParameterField = queueField = method.MethodAttribute.IsControllerReadWriteQueue ? readWriteQueueField.notNull() : readWriteQueueFieldBuilder.notNull();
                                    checkOfflineCountMethod = ServerInterfaceController.CommandServerCallReadWriteQueueNodeCheckOfflineCount.Method;
                                    keepCallbackQueueNodeMethod = method.ReturnValueType == typeof(void) ? ServerInterfaceController.CreateCommandServerKeepCallbackReadWriteQueueNodeDelegate.Method : GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackReadWriteQueueNodeDelegate.Method;
                                    createServerCallbackMethod = ServerInterfaceController.CreateServerCallbackCallReadWriteQueueNodeDelegate.Method;
                                    createServerCallbackCountMethod = method.ReturnValueType == typeof(void) ? ServerInterfaceController.CreateCommandServerKeepCallbackCountReadWriteQueueNodeDelegate.Method : GenericType.Get(method.ReturnValueType).CreateCommandServerKeepCallbackCountReadWriteQueueNodeDelegate.Method;
                                    setVerifyStateMethod = ServerInterfaceController.ServerCallReadWriteQueueNodeSetVerifyState.Method;
                                    if (method.OutputParameterType == null)
                                    {
                                        if (method.ReturnValueType == typeof(void)) queueSendMethod = ServerInterfaceController.ServerCallReadWriteQueueNodeSendSuccess.Method;
                                        else queueSendMethod = GenericType.Get(method.ReturnValueType).CommandServerCallReadWriteQueueSendReturnValueDelegate.Method;
                                    }
                                    else queueSendMethod = StructGenericType.Get(method.OutputParameterType.Type).CommandServerCallReadWriteQueueSend.Method;
                                    if (method.InputParameterType != null)
                                    {
                                        addQueueNodeMethod = method.IsWriteQueue ? ServerInterfaceController.ServerCallReadWriteQueueAppendWriteIsDeserialize.Method : ServerInterfaceController.ServerCallReadWriteQueueAppendReadIsDeserialize.Method;
                                    }
                                    else
                                    {
                                        addQueueNodeMethod = method.IsWriteQueue ? ServerInterfaceController.ServerCallReadWriteQueueAppendWrite.Method : ServerInterfaceController.ServerCallReadWriteQueueAppendRead.Method;
                                    }
                                    goto ALLQUEUE;
                                case ServerMethodTypeEnum.Task:
                                case ServerMethodTypeEnum.SendOnlyTask:
                                TASK:
                                    #region CommandServerSocket.CheckTask(socket, GetController(this, socket).AsynchronousTask(socket, inputParameter.Value, inputParameter.Ref));
                                    var callbackLocalBuilder = default(LocalBuilder);
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.CallbackTask:
                                            #region AsynchronousCallbackTask callback = new AsynchronousCallbackTask(socket);
                                            callbackLocalBuilder = doCommandGenerator.DeclareLocal(method.ReturnValueType == typeof(void) ? typeof(CommandServerCallbackTask) : method.AsynchronousType);
                                            method.CreateServerCallback(doCommandGenerator, asynchronousConstructorBuilder);
                                            doCommandGenerator.Emit(OpCodes.Stloc, callbackLocalBuilder);
                                            doCommandGenerator.Emit(OpCodes.Ldloc, callbackLocalBuilder);
                                            #endregion
                                            break;
                                        default:
                                            doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                            if (method.ReturnValueType != typeof(void)) doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                            break;
                                    }
                                    method.CallMethodParameter(doCommandGenerator, getControllerMethod, controllerLocalBuilder, inputParameterLocalBuilder);
                                    if (method.MethodType == ServerMethodTypeEnum.CallbackTask) doCommandGenerator.Emit(OpCodes.Ldloc, callbackLocalBuilder.notNull());
                                    #region long timestamp = Stopwatch.GetTimestamp();
                                    if (timestampLocalBuilder == null) timestampLocalBuilder = doCommandGenerator.DeclareLocal(typeof(long));
                                    doCommandGenerator.call(ServerInterfaceController.StopwatchGetTimestamp.Method);
                                    doCommandGenerator.Emit(OpCodes.Stloc, timestampLocalBuilder);
                                    #endregion
                                    doCommandGenerator.Emit(OpCodes.Constrained, type);
                                    doCommandGenerator.call(method.Method);
                                    #region ServerInterfaceMethod.CheckGetTaskTimestamp(Method0, timestamp);
                                    doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                    doCommandGenerator.Emit(OpCodes.Ldloc, timestampLocalBuilder.notNull());
                                    doCommandGenerator.call(ServerInterfaceController.ServerInterfaceMethodCheckGetTaskTimestamp.Method);
                                    #endregion
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.CallbackTask:
                                            if (method.ReturnValueType == typeof(void)) doCommandGenerator.call(ServerInterfaceController.CommandServerCallbackTaskCheckTask.Method);
                                            else doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CommandServerCallbackTaskCheckTaskDelegate.Method);
                                            break;
                                        case ServerMethodTypeEnum.SendOnlyTask:
                                            doCommandGenerator.call(ServerInterfaceController.CommandServerSocketCheckSendOnlyTask.Method);
                                            break;
                                        default:
                                            if (method.ReturnValueType == typeof(void)) doCommandGenerator.call(ServerInterfaceController.CommandServerSocketCheckTask.Method);
                                            else if (methodIndex == serverInterface.VerifyMethodIndex) doCommandGenerator.call(ServerInterfaceController.CommandServerSocketCheckVerifyStateTask.Method);
                                            else doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CommandServerSocketCheckTaskDelegate.Method);
                                            break;
                                    }
                                    #endregion
                                    break;
                                case ServerMethodTypeEnum.KeepCallbackTask:
                                case ServerMethodTypeEnum.KeepCallbackCountTask:
                                    #region CommandServerKeepCallbackTask<string> keepCallback = CommandServerKeepCallbackTask<string>.CreateServerKeepCallbackTask(socket, ServerInterfaceControllerIL<T>.Method0);
                                    if (method.MethodType == ServerMethodTypeEnum.KeepCallbackTask)
                                    {
                                        keepCallbackLocalBuilder = doCommandGenerator.DeclareLocal(typeof(CommandServerKeepCallback));
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        if (method.ReturnValueType == typeof(void)) doCommandGenerator.call(ServerInterfaceController.CreateServerKeepCallbackTaskDelegate.Method);
                                        else
                                        {
                                            doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                            doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CreateServerKeepCallbackTaskDelegate.Method);
                                        }
                                        doCommandGenerator.Emit(OpCodes.Stloc, keepCallbackLocalBuilder);
                                    }
                                    else
                                    {
                                        keepCallbackLocalBuilder = doCommandGenerator.DeclareLocal(typeof(CommandServerKeepCallbackCount));
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        if (method.ReturnValueType == typeof(void))
                                        {
                                            doCommandGenerator.int32(Math.Max(method.MethodAttribute.KeepCallbackOutputCount, 1));
                                            doCommandGenerator.call(ServerInterfaceController.CreateServerKeepCallbackCountTaskDelegate.Method);
                                        }
                                        else
                                        {
                                            doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                            doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CreateServerKeepCallbackCountTaskDelegate.Method);
                                        }
                                        doCommandGenerator.Emit(OpCodes.Stloc, keepCallbackLocalBuilder);
                                    }
                                    #endregion
                                    #region CommandServerKeepCallbackTask<string>.CheckTask(keepCallback, GetController(this, socket).KeepCallbackTask(socket, inputParameter.Value, inputParameter.Ref, keepCallback));
                                    doCommandGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder);
                                    method.CallMethodParameter(doCommandGenerator, getControllerMethod, controllerLocalBuilder, inputParameterLocalBuilder);
                                    doCommandGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder);
                                    doCommandGenerator.Emit(OpCodes.Constrained, type);
                                    doCommandGenerator.call(method.Method);
                                    //doCommandGenerator.Emit(OpCodes.Ldarg_3);
                                    if (method.MethodType == ServerMethodTypeEnum.KeepCallbackTask)
                                    {
                                        if (method.ReturnValueType == typeof(void))
                                        {
                                            if (method.MethodAttribute.AutoCancelKeep) doCommandGenerator.call(ServerInterfaceController.CommandServerKeepCallbackTaskCheckTaskAutoCancelKeepDelegate.Method);
                                            else doCommandGenerator.call(ServerInterfaceController.CommandServerKeepCallbackTaskCheckTaskDelegate.Method);
                                        }
                                        else
                                        {
                                            if (method.MethodAttribute.AutoCancelKeep) doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CommandServerKeepCallbackTaskCheckTaskAutoCancelKeepDelegate.Method);
                                            else doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CommandServerKeepCallbackTaskCheckTaskDelegate.Method);
                                        }
                                    }
                                    else
                                    {
                                        if (method.ReturnValueType == typeof(void))
                                        {
                                            if (method.MethodAttribute.AutoCancelKeep) doCommandGenerator.call(ServerInterfaceController.CommandServerKeepCallbackTaskCheckCountTaskAutoCancelKeepDelegate.Method);
                                            else doCommandGenerator.call(ServerInterfaceController.CommandServerKeepCallbackTaskCheckCountTaskDelegate.Method);
                                        }
                                        else
                                        {
                                            if (method.MethodAttribute.AutoCancelKeep) doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CommandServerKeepCallbackTaskCheckCountTaskAutoCancelKeepDelegate.Method);
                                            else doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CommandServerKeepCallbackTaskCheckCountTaskDelegate.Method);
                                        }
                                    }
                                    #endregion
                                    break;
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
#if NetStandard21
                                case ServerMethodTypeEnum.AsyncEnumerableTask:
#endif
                                    #region CommandServerEnumerableKeepCallbackCountTask<string>.CreateServerKeepCallbackTask(socket, ServerInterfaceControllerIL<T>.Method0, GetController(this, socket).EnumerableKeepCallbackCountTask(socket, inputParameter.Value, inputParameter.Ref));
                                    doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                    doCommandGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                    method.CallMethodParameter(doCommandGenerator, getControllerMethod, controllerLocalBuilder, inputParameterLocalBuilder);
                                    doCommandGenerator.Emit(OpCodes.Constrained, type);
                                    doCommandGenerator.call(method.Method);
                                    //doCommandGenerator.Emit(OpCodes.Ldarg_3);
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                                            doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CreateServerEnumerableKeepCallbackCountTaskDelegate.Method);
                                            break;
#if NetStandard21
                                        case ServerMethodTypeEnum.AsyncEnumerableTask:
                                            doCommandGenerator.call(GenericType.Get(method.ReturnValueType).CreateServerAsyncEnumerableTaskDelegate.Method);
                                            break;
#endif
                                    }
                                    #endregion
                                    break;
                                case ServerMethodTypeEnum.TaskQueue:
                                case ServerMethodTypeEnum.SendOnlyTaskQueue:
                                case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                                case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                                    TASKQUEUE:
                                    #region public sealed class CallTaskQueue : ServerCallTaskQueueTask
                                    Type commandServerCallTaskQueueTaskType = method.GetCommandServerCallTaskQueueTaskType(methodIndex == serverInterface.VerifyMethodIndex, out returnGenericType);
                                    asynchronousTypeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(ServerInterfaceController.GetAsynchronousTypeName(), TypeAttributes.Class | TypeAttributes.Sealed, commandServerCallTaskQueueTaskType);
                                    #endregion
                                    #region public CallTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter) : base(socket, false)
                                    var callbackAsynchronousConstructorBuilder = asynchronousConstructorBuilder;
                                    if (inputParameterLocalBuilder == null)
                                    {
                                        asynchronousConstructorBuilder = asynchronousTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(CommandServerSocket), type });
                                    }
                                    else
                                    {
                                        asynchronousConstructorBuilder = asynchronousTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(CommandServerSocket), type, method.InputParameterType.notNull().Type.MakeByRefType() });
                                    }
                                    asynchronousConstructorGenerator = method.TaskQueueAsynchronousConstructorBase(asynchronousConstructorBuilder, returnGenericType, commandServerCallTaskQueueTaskType, methodIndex == serverInterface.VerifyMethodIndex);
                                    #endregion
                                    #region this.controller = controller;
                                    asynchronousControllerFieldBuilder = method.GetTaskQueueControllerFieldBuilder(asynchronousTypeBuilder, asynchronousConstructorGenerator, type);
                                    #endregion
                                    #region this.inputParameter = inputParameter;
                                    inputParameterFieldBuilder = method.GetTaskQueueInputParameterFieldBuilder(asynchronousTypeBuilder, asynchronousConstructorGenerator);
                                    #endregion
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ret);
                                    #region public override bool RunTask()
                                    asynchronousMethodBuilder = asynchronousTypeBuilder.DefineMethod(nameof(CommandServerCallTaskQueueNode.RunTask), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(bool), EmptyArray<Type>.Array);
                                    asynchronousMethodGenerator = asynchronousMethodBuilder.GetILGenerator();
                                    #endregion
                                    #region CommandServerKeepCallback<string> keepCallback = CommandServerKeepCallback<string>.CreateServerKeepCallback(this, ServerInterfaceControllerIL<T>.Method0);
                                    keepCallbackLocalBuilder = method.GetTaskQueueKeepCallbackLocalBuilder(asynchronousMethodGenerator, returnGenericType);
                                    #endregion
                                    #region CommandServerSocket socket = CommandServerCallTaskQueueTaskNode.GetSocket(this, keepCallback, out CommandServerCallTaskQueue queue);
                                    LocalBuilder commandServerSocketLocalBuilder = asynchronousMethodGenerator.DeclareLocal(typeof(CommandServerSocket));
                                    LocalBuilder commandServerCallTaskQueueLocalBuilder = asynchronousMethodGenerator.DeclareLocal(typeof(CommandServerCallTaskQueue));
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder.notNull());
                                            break;
                                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                                        case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                            break;
                                    }
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldloca_S, commandServerCallTaskQueueLocalBuilder);
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                            asynchronousMethodGenerator.call(ServerInterfaceController.CommandServerKeepCallbackQueueTaskGetSocket.Method);
                                            break;
                                        case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
                                            asynchronousMethodGenerator.call(returnGenericType.notNull().CommandServerKeepCallbackQueueTaskGetSocketDelegate.Method);
                                            break;
#if NetStandard21
                                        case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
                                            asynchronousMethodGenerator.call(returnGenericType.notNull().CommandServerAsyncEnumerableQueueTaskGetSocketDelegate.Method);
                                            break;
#endif
                                        default:
                                            asynchronousMethodGenerator.call(ServerInterfaceController.CommandServerCallTaskQueueTaskNodeGetSocket.Method);
                                            break;
                                    }
                                    asynchronousMethodGenerator.Emit(OpCodes.Stloc, commandServerSocketLocalBuilder);
                                    #endregion
                                    #region return CommandServerCallTaskQueueTask<string>.CheckCallTask(this, controller.TaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref));
                                    //asynchronousControllerLocalBuilder = asynchronousMethodGenerator.DeclareLocal(type);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                    method.CallMethodParameter(asynchronousMethodGenerator, asynchronousControllerFieldBuilder
                                            , commandServerSocketLocalBuilder, commandServerCallTaskQueueLocalBuilder, inputParameterFieldBuilder);
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                                        case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldloc, keepCallbackLocalBuilder.notNull());
                                            break;
                                        case ServerMethodTypeEnum.CallbackTaskQueue:
                                            #region new AsynchronousCallback(this)
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                            if (method.ReturnValueType == typeof(void)) asynchronousMethodGenerator.call(ServerInterfaceController.CreateServerCallbackCallTaskQueueNodeDelegate.Method);
                                            else asynchronousMethodGenerator.Emit(OpCodes.Newobj, callbackAsynchronousConstructorBuilder.notNull());
                                            #endregion
                                            break;
                                    }
                                    //asynchronousMethodGenerator.Emit(OpCodes.Constrained, type);
                                    asynchronousMethodGenerator.call(method.Method);
                                    method.CheckCallTaskQueue(asynchronousMethodGenerator, returnGenericType, methodIndex == serverInterface.VerifyMethodIndex);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                    method.AsynchronousType = asynchronousTypeBuilder.CreateType();

                                    if (ControllerAttribute.TaskQueueMaxConcurrent <= 0 || !method.MethodAttribute.IsControllerTaskQueue)
                                    {
                                        #region CommandServerSocket.CallTaskQueueAppendQueue(taskQueueSet0, inputParameter.Value, new CallTaskQueue(socket, GetController(this, socket), ref inputParameter));
                                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                        doCommandGenerator.Emit(OpCodes.Ldfld, serverInterface.TaskQueueFieldBuilders.notNull()[method.TaskQueueKeyType.notNull()].notNull());
                                        doCommandGenerator.Emit(OpCodes.Ldloc, inputParameterLocalBuilder.notNull());
                                        doCommandGenerator.Emit(OpCodes.Ldfld, method.TaskQueueKeyField);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        //doCommandGenerator.Emit(OpCodes.Ldarg_3);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        doCommandGenerator.call(getControllerMethod);
                                        doCommandGenerator.Emit(OpCodes.Ldloca, inputParameterLocalBuilder.notNull());
                                        doCommandGenerator.Emit(OpCodes.Newobj, asynchronousConstructorBuilder);
                                        doCommandGenerator.call((method.IsLowPriorityQueue ? ServerInterfaceController.CommandServerSocketCallTaskQueueAppendLowPriorityMethod : ServerInterfaceController.CommandServerSocketCallTaskQueueAppendQueueMethod).MakeGenericMethod(method.TaskQueueKeyType.notNull()));
                                        returnSuccess = false;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region CommandServerController.AddTaskQueue(this, new CallTaskQueue(socket, GetController(this, socket), ref inputParameter));
                                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        //doCommandGenerator.Emit(OpCodes.Ldarg_3);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                        doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                        doCommandGenerator.call(getControllerMethod);
                                        if (inputParameterLocalBuilder != null) doCommandGenerator.Emit(OpCodes.Ldloca, inputParameterLocalBuilder);
                                        doCommandGenerator.Emit(OpCodes.Newobj, asynchronousConstructorBuilder);
                                        doCommandGenerator.call(method.IsLowPriorityQueue ? ServerInterfaceController.CommandServerControllerAddTaskQueueLowPriority.Method : ServerInterfaceController.CommandServerControllerAddTaskQueue.Method);
                                        #endregion
                                    }
                                    break;
                            }
                            if (returnSuccess) doCommandGenerator.int32((byte)CommandClientReturnTypeEnum.Success);
                            doCommandGenerator.Emit(OpCodes.Ret);
                            var callbackDelegate = default(Delegate);
                            switch (method.MethodType)
                            {
                                case ServerMethodTypeEnum.SendOnlyTask:
                                    Type runTaskType = typeof(CommandServerSendOnlyRunTask);
                                    Delegate setIsDeserializeDelegate = ServerInterfaceController.CommandServerSendOnlyRunTaskSetIsDeserialize;
                                    Delegate runTaskDelegate = ServerInterfaceController.CommandServerSendOnlyRunTaskIsDeserialize;
                                    goto RUNTASK;
                                case ServerMethodTypeEnum.CallbackTask:
                                    if (method.ReturnValueType == typeof(void))
                                    {
                                        runTaskType = typeof(CommandServerCallbackRunTask);
                                        setIsDeserializeDelegate = ServerInterfaceController.CommandServerCallbackRunTaskSetIsDeserialize;
                                        runTaskDelegate = ServerInterfaceController.CommandServerCallbackRunTaskIsDeserialize;
                                    }
                                    else
                                    {
                                        GenericType returnGenericType = GenericType.Get(method.ReturnValueType);
                                        runTaskType = returnGenericType.CommandServerCallbackRunTaskType;
                                        setIsDeserializeDelegate = returnGenericType.CommandServerCallbackRunTaskSetIsDeserializeDelegate;
                                        runTaskDelegate = returnGenericType.CommandServerCallbackRunTaskIsDeserializeDelegate;
                                        callbackDelegate = returnGenericType.CommandServerCallbackDelegate;
                                    }
                                    goto RUNTASK;
                                case ServerMethodTypeEnum.KeepCallbackTask:
                                    if (method.ReturnValueType == typeof(void))
                                    {
                                        runTaskType = typeof(CommandServerKeepCallbackRunTask);
                                        setIsDeserializeDelegate = ServerInterfaceController.CommandServerKeepCallbackRunTaskSetIsDeserialize;
                                        if (method.MethodAttribute.AutoCancelKeep) runTaskDelegate = ServerInterfaceController.CommandServerKeepCallbackRunTaskAutoCancelKeepIsDeserialize;
                                        else runTaskDelegate = ServerInterfaceController.CommandServerKeepCallbackRunTaskIsDeserialize;
                                    }
                                    else
                                    {
                                        GenericType returnGenericType = GenericType.Get(method.ReturnValueType);
                                        runTaskType = returnGenericType.CommandServerKeepCallbackRunTaskType;
                                        setIsDeserializeDelegate = returnGenericType.CommandServerKeepCallbackRunTaskSetIsDeserializeDelegate;
                                        if (method.MethodAttribute.AutoCancelKeep) runTaskDelegate = returnGenericType.CommandServerKeepCallbackRunTaskAutoCancelKeepIsDeserializeDelegate;
                                        else runTaskDelegate = returnGenericType.CommandServerKeepCallbackRunTaskIsDeserializeDelegate;
                                    }
                                    goto RUNTASK;
                                case ServerMethodTypeEnum.KeepCallbackCountTask:
                                    if (method.ReturnValueType == typeof(void))
                                    {
                                        runTaskType = typeof(CommandServerKeepCallbackCountRunTask);
                                        setIsDeserializeDelegate = ServerInterfaceController.CommandServerKeepCallbackCountRunTaskSetIsDeserialize;
                                        if (method.MethodAttribute.AutoCancelKeep) runTaskDelegate = ServerInterfaceController.CommandServerKeepCallbackCountRunTaskAutoCancelKeepIsDeserialize;
                                        else runTaskDelegate = ServerInterfaceController.CommandServerKeepCallbackCountRunTaskIsDeserialize;
                                    }
                                    else
                                    {
                                        GenericType returnGenericType = GenericType.Get(method.ReturnValueType);
                                        runTaskType = returnGenericType.CommandServerKeepCallbackCountRunTaskType;
                                        setIsDeserializeDelegate = returnGenericType.CommandServerKeepCallbackCountRunTaskSetIsDeserializeDelegate;
                                        if (method.MethodAttribute.AutoCancelKeep) runTaskDelegate = returnGenericType.CommandServerKeepCallbackCountRunTaskAutoCancelKeepIsDeserializeDelegate;
                                        else runTaskDelegate = returnGenericType.CommandServerKeepCallbackCountRunTaskIsDeserializeDelegate;
                                    }
                                    goto RUNTASK;
                                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                                    {
                                        GenericType returnGenericType = GenericType.Get(method.ReturnValueType);
                                        runTaskType = returnGenericType.CommandServerEnumerableKeepCallbackCountRunTaskType;
                                        setIsDeserializeDelegate = returnGenericType.CommandServerEnumerableKeepCallbackCountRunTaskSetIsDeserializeDelegate;
                                        runTaskDelegate = returnGenericType.CommandServerEnumerableKeepCallbackCountRunTaskIsDeserializeDelegate;
                                    }
                                    goto RUNTASK;
                                case ServerMethodTypeEnum.Task:
                                    if (method.ReturnValueType == typeof(void))
                                    {
                                        runTaskType = typeof(CommandServerRunTask);
                                        setIsDeserializeDelegate = ServerInterfaceController.CommandServerRunTaskSetIsDeserialize;
                                        runTaskDelegate = ServerInterfaceController.CommandServerRunTaskIsDeserialize;
                                    }
                                    else if (methodIndex == serverInterface.VerifyMethodIndex)
                                    {
                                        runTaskType = typeof(CommandServerVerifyStateRunTask);
                                        setIsDeserializeDelegate = ServerInterfaceController.CommandServerVerifyStateRunTaskSetIsDeserialize;
                                        runTaskDelegate = ServerInterfaceController.CommandServerVerifyStateRunTaskIsDeserialize;
                                    }
                                    else
                                    {
                                        GenericType returnGenericType = GenericType.Get(method.ReturnValueType);
                                        runTaskType = returnGenericType.CommandServerRunTaskType;
                                        setIsDeserializeDelegate = returnGenericType.CommandServerRunTaskSetIsDeserializeDelegate;
                                        runTaskDelegate = returnGenericType.CommandServerRunTaskIsDeserializeDelegate;
                                    }
                                RUNTASK:
                                    #region public sealed class RunTask : CommandServerRunTask
                                    TypeBuilder asynchronousTypeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(ServerInterfaceController.GetAsynchronousTypeName(), TypeAttributes.Class | TypeAttributes.Sealed, runTaskType);
                                    #endregion
                                    #region public RunTask(CommandServerSocket socket, T controller, ref SubArray<byte> data) : base(socket, Method0)
                                    Type[] constructorParameterTypeArray;
                                    if (method.InputParameterType != null)
                                    {
                                        constructorParameterTypeArray = new Type[] { typeof(CommandServerSocket), type, ServerInterfaceController.ByteSubArrayRefType };
                                    }
                                    else constructorParameterTypeArray = new Type[] { typeof(CommandServerSocket), type };
                                    asynchronousConstructorBuilder = asynchronousTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypeArray);
                                    ILGenerator asynchronousConstructorGenerator = asynchronousConstructorBuilder.GetILGenerator();
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_1);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                    asynchronousConstructorGenerator.Emit(OpCodes.Call, runTaskType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ServerInterfaceController.CommandServerSocketInterfaceMethodParameterTypes, null).notNull());
                                    #endregion
                                    #region this.controller = controller;
                                    FieldBuilder asynchronousControllerFieldBuilder = method.GetTaskQueueControllerFieldBuilder(asynchronousTypeBuilder, asynchronousConstructorGenerator, type);
                                    #endregion
                                    #region CommandServerRunTask.SetIsDeserialize(this, CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true));
                                    var inputParameterFieldBuilder = default(FieldBuilder);
                                    if (method.InputParameterType != null)
                                    {
                                        inputParameterFieldBuilder = asynchronousTypeBuilder.DefineField("inputParameter", method.InputParameterType.Type, FieldAttributes.Public);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_1);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_3);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousConstructorGenerator.Emit(OpCodes.Ldflda, inputParameterFieldBuilder);
                                        asynchronousConstructorGenerator.int32(method.IsSimpleDeserializeParamter);
                                        asynchronousConstructorGenerator.call(ServerInterfaceController.CommandServerSocketDeserializeMethod.MakeGenericMethod(method.InputParameterType.Type));
                                        asynchronousConstructorGenerator.call(setIsDeserializeDelegate.Method);
                                    }
                                    #endregion
                                    asynchronousConstructorGenerator.Emit(OpCodes.Ret);
                                    #region public override Task GetTask()
                                    MethodBuilder asynchronousMethodBuilder = asynchronousTypeBuilder.DefineMethod(nameof(CommandServerRunTask.GetTask), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, method.Method.ReturnType, EmptyArray<Type>.Array);
                                    ILGenerator asynchronousMethodGenerator = asynchronousMethodBuilder.GetILGenerator();
                                    #endregion
                                    #region return controller.AsynchronousTask(CommandServerCall.GetSocket(this), inputParameter.Value, inputParameter.Ref);
                                    method.CallMethodParameter(asynchronousMethodGenerator, asynchronousControllerFieldBuilder, ServerInterfaceController.ServerCallGetSocket.Method, inputParameterFieldBuilder);
                                    switch (method.MethodType)
                                    {
                                        case ServerMethodTypeEnum.CallbackTask:
                                        case ServerMethodTypeEnum.KeepCallbackTask:
                                        case ServerMethodTypeEnum.KeepCallbackCountTask:
                                            asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                            break;
                                    }
                                    asynchronousMethodGenerator.call(method.Method);
                                    asynchronousMethodGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                    if (method.MethodType == ServerMethodTypeEnum.CallbackTask && method.ReturnValueType != typeof(void))
                                    {
                                        #region public override bool Callback(X returnValue)
                                        asynchronousMethodBuilder = asynchronousTypeBuilder.DefineMethod(nameof(CommandServerCallback.Callback), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(bool), new Type[] { method.ReturnValueType });
                                        asynchronousMethodGenerator = asynchronousMethodBuilder.GetILGenerator();
                                        #endregion
                                        #region return CommandServerCallback.Callback<X>(this, ServerInterfaceControllerIL<T>.Method0, returnValue);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_0);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldsfld, method.MethodFieldBuilder);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ldarg_1);
                                        asynchronousMethodGenerator.call(callbackDelegate.notNull().Method);
                                        asynchronousMethodGenerator.Emit(OpCodes.Ret);
                                        #endregion

                                    }
                                    method.AsynchronousType = asynchronousTypeBuilder.CreateType();

                                    #region return CommandServerRunTask.RunTask(new RunTask(socket, GetController(this, socket), ref data));
                                    doCommandGenerator.MarkLabel(runTaskLabel);
                                    doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                    doCommandGenerator.Emit(OpCodes.Ldarg_0);
                                    doCommandGenerator.Emit(OpCodes.Ldarg_1);
                                    doCommandGenerator.call(getControllerMethod);
                                    if (method.InputParameterType != null) doCommandGenerator.Emit(OpCodes.Ldarg_2);
                                    doCommandGenerator.Emit(OpCodes.Newobj, asynchronousConstructorBuilder.notNull());
                                    doCommandGenerator.call(runTaskDelegate.Method);
                                    doCommandGenerator.Emit(OpCodes.Ret);
                                    #endregion
                                    break;
                            }
                        }
                    }
                    ++methodIndex;
                }
                #endregion
                controllerConstructorInfo = typeBuilder.CreateType().GetConstructor(constructorParameterTypes).notNull();
            }
            catch (Exception exception)
            {
                controllerConstructorException = new Exception($"{type.fullName()} 服务端控制器生成失败", exception);
            }
        }
    }
#if DEBUG && NetStandard21
    #region 控制器接口 IL 模板
    internal interface ServerInterfaceControllerIL
    {
        string Synchronous(CommandServerSocket socket, int Value, ref int Ref, out int Out);
        //CommandServerVerifyState Synchronous(CommandServerSocket socket, int Value);
        CommandServerSendOnly SendOnly(CommandServerSocket socket, int Value, int Ref);
        void Callback(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback);
        void KeepCallback(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackCount(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);

        string Queue(CommandServerSocket socket, CommandServerCallQueue queue, int Value, ref int Ref, out int Out);
        CommandServerSendOnly SendOnly(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref);
        void Callback(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<string> Callback);
        void KeepCallback(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackCount(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);

        Task<string> AsynchronousTask(CommandServerSocket socket, int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTask(CommandServerSocket socket, int Value, int Ref);
        Task CallbackTask(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback);
        Task KeepCallbackTask(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackCountTask(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task<IEnumerable<string>> EnumerableKeepCallbackCountTask(CommandServerSocket socket, int Value, int Ref);

        Task<string> TaskQueue(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTaskQueue(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref);
        Task CallbackTaskQueue(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerCallback<string> Callback);
        Task KeepCallbackTaskQueue(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackCountTaskQueue(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskQueue(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref);

#if NetStandard21
        IAsyncEnumerable<string> AsyncEnumerableTask(CommandServerSocket socket, int Value, int Ref);
        IAsyncEnumerable<string> AsyncEnumerableTaskQueue(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref);
#endif
    }
    internal sealed class ServerInterfaceControllerIL<T> : CommandServerController<T>
        where T : class, ServerInterfaceControllerIL
    {
        public struct SynchronousInputParameter
        {
            public int Value;
            public int Ref;
        }
        public struct SynchronousOutputParameter
        {
            public string __Return__;
            public int Ref;
            public int Out;
        }
        public sealed class AsynchronousCallback : CommandServerCallback<string>
        {
            public AsynchronousCallback(CommandServerSocket socket) : base(socket)
            {
            }
            public AsynchronousCallback(CommandServerCallTaskQueueNode node) : base(node) { }
#if NetStandard21
            [AllowNull]
#endif
            private readonly CommandServerCallQueue queue;
            public AsynchronousCallback(CommandServerCallQueueNode node, CommandServerCallQueue queue) : base(node)
            {
                this.queue = queue;
            }
            public override bool Callback(string returnValue)
            {
                return CommandServerCallback<string>.Callback(this, ServerInterfaceControllerIL<T>.Method0, returnValue);
            }
            internal override bool SynchronousCallback(string returnValue)
            {
                return CommandServerCallback<string>.SynchronousCallback(queue, this, ServerInterfaceControllerIL<T>.Method0, returnValue);
            }
        }
        public sealed class AsynchronousCallbackTask : CommandServerCallbackTask<string>
        {
            public AsynchronousCallbackTask(CommandServerSocket socket) : base(socket)
            {
            }
            public override bool Callback(string returnValue)
            {
                return CommandServerCallback<string>.Callback(this, ServerInterfaceControllerIL<T>.Method0, returnValue);
            }
        }

        public sealed class SynchronousQueue : CommandServerCallQueueNode
        {
            private readonly ServerInterfaceControllerIL<T> controller;
            private SynchronousInputParameter inputParameter;
            public SynchronousQueue(CommandServerSocket socket, ServerInterfaceControllerIL<T> controller, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.Queue)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }
            public override void RunTask()
            {
                if (!CommandServerCallQueueNode.SocketIsClose(this))
                {
                    SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
                    outputParameter.__Return__ = GetQueueNodeController(controller, this).Queue(CommandServerCallQueueNode.GetSocket(this), controller.Queue0, inputParameter.Value, ref inputParameter.Ref, out outputParameter.Out);
                    outputParameter.Ref = inputParameter.Ref;
                    //ServerCallQueueNode.SetVerifyState(this, outputParameter.__Return__);
                    CommandServerCallQueueNode.Send(this, controller.Queue0, ServerInterfaceControllerIL<T>.Method0, ref outputParameter);
                    CommandServerCallQueueNode.CheckOfflineCount(this);
                }
            }
        }
        public sealed class SynchronousParameterQueue : CommandServerCallQueueNode
        {
            private readonly ServerInterfaceControllerIL<T> controller;
            private SynchronousInputParameter inputParameter;
            public SynchronousParameterQueue(CommandServerSocket socket, ServerInterfaceControllerIL<T> controller, ref SubArray<byte> data)
                : base(socket, ServerMethodTypeEnum.Queue)
            {
                this.controller = controller;
                CommandServerCallQueueNode.SetIsDeserialize(this, CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true));
            }
            public override void RunTask()
            {
                if (!CommandServerCallQueueNode.SocketIsClose(this))
                {
                    SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
                    outputParameter.__Return__ = GetQueueNodeController(controller, this).Queue(CommandServerCallQueueNode.GetSocket(this), controller.Queue0, inputParameter.Value, ref inputParameter.Ref, out outputParameter.Out);
                    outputParameter.Ref = inputParameter.Ref;
                    //ServerCallQueueNode.SetVerifyState(this, outputParameter.__Return__);
                    CommandServerCallQueueNode.Send(this, controller.Queue0, ServerInterfaceControllerIL<T>.Method0, ref outputParameter);
                    CommandServerCallQueueNode.CheckOfflineCount(this);
                }
            }
        }

        public sealed class SendOnlyQueue : CommandServerCallQueueNode
        {
            private readonly ServerInterfaceControllerIL<T> controller;
            private SynchronousInputParameter inputParameter;
            public SendOnlyQueue(CommandServerSocket socket, ServerInterfaceControllerIL<T> controller, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.SendOnlyQueue)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }
            public override void RunTask()
            {
                if (!CommandServerCallQueueNode.SocketIsClose(this))
                {
                    GetQueueNodeController(controller, this).SendOnly(CommandServerCallQueueNode.GetSocket(this), controller.Queue0, inputParameter.Value, inputParameter.Ref);
                    CommandServerCallQueueNode.CheckOfflineCount(this);
                }
            }
        }
        public sealed class CallbackQueue : CommandServerCallQueueNode
        {
            private readonly ServerInterfaceControllerIL<T> controller;
            private SynchronousInputParameter inputParameter;
            public CallbackQueue(CommandServerSocket socket, ServerInterfaceControllerIL<T> controller, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.CallbackQueue)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }
            public override void RunTask()
            {
                if (!CommandServerCallQueueNode.SocketIsClose(this))
                {
                    GetQueueNodeController(controller, this).Callback(CommandServerCallQueueNode.GetSocket(this), controller.Queue0, inputParameter.Value, inputParameter.Ref, new AsynchronousCallback(this, controller.Queue0));
                }
            }
        }
        public sealed class KeepCallbackQueue : CommandServerCallQueueNode
        {
            private readonly ServerInterfaceControllerIL<T> controller;
            private SynchronousInputParameter inputParameter;
            public KeepCallbackQueue(CommandServerSocket socket, ServerInterfaceControllerIL<T> controller, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.KeepCallbackQueue)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }
            public override void RunTask()
            {
                if (!CommandServerCallQueueNode.SocketIsClose(this))
                {
                    GetQueueNodeController(controller, this).KeepCallback(CommandServerCallQueueNode.GetSocket(this), controller.Queue0, inputParameter.Value, inputParameter.Ref, CommandServerKeepCallback<string>.CreateServerKeepCallback(this, ServerInterfaceControllerIL<T>.Method0));
                    CommandServerCallQueueNode.CheckOfflineCount(this);
                }
            }
        }
        public sealed class KeepCallbackCountQueue : CommandServerCallQueueNode
        {
            private readonly ServerInterfaceControllerIL<T> controller;
            private SynchronousInputParameter inputParameter;
            public KeepCallbackCountQueue(CommandServerSocket socket, ServerInterfaceControllerIL<T> controller, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.KeepCallbackCountQueue)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }
            public override void RunTask()
            {
                if (!CommandServerCallQueueNode.SocketIsClose(this))
                {
                    GetQueueNodeController(controller, this).KeepCallbackCount(CommandServerCallQueueNode.GetSocket(this), controller.Queue0, inputParameter.Value, inputParameter.Ref, CommandServerKeepCallbackCount<string>.CreateServerKeepCallback(this, ServerInterfaceControllerIL<T>.Method0));
                    CommandServerCallQueueNode.CheckOfflineCount(this);
                }
            }
        }

        public sealed class RunTask : CommandServerRunTask
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public RunTask(CommandServerSocket socket, T controller, ref SubArray<byte> data)
                : base(socket, Method0)
            {
                this.controller = controller;
                CommandServerRunTask.SetIsDeserialize(this, CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true));
            }
            public override Task GetTask()
            {
                return controller.AsynchronousTask(CommandServerCall.GetSocket(this), inputParameter.Value, inputParameter.Ref);
            }
        }
        public sealed class CallbackRunTask : CommandServerCallbackRunTask<string>
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public CallbackRunTask(CommandServerSocket socket, T controller, ref SubArray<byte> data)
                : base(socket, Method0)
            {
                this.controller = controller;
                CommandServerCallbackRunTask<string>.SetIsDeserialize(this, CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true));
            }
            public override bool Callback(string returnValue)
            {
                return CommandServerCallback<string>.Callback(this, ServerInterfaceControllerIL<T>.Method0, returnValue);
            }
            public override Task GetTask()
            {
                return controller.CallbackTask(CommandServerCall.GetSocket(this), inputParameter.Value, inputParameter.Ref, this);
            }
        }

        public new sealed class CallTaskQueue : CommandServerCallTaskQueueTask<string>
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public CallTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerInterfaceControllerIL<T>.Method0)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }
            public override bool RunTask()
            {
                CommandServerCallTaskQueue queue;
                CommandServerSocket socket = CommandServerCallTaskQueueNode.GetSocket(this, out queue);
                return CommandServerCallTaskQueueTask<string>.CheckCallTask(this, controller.TaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref));
            }
        }
        //public sealed class CallParameterTaskQueue : CommandServerCallTaskQueueTask<string>
        //{
        //    private readonly T controller;
        //    private SynchronousInputParameter inputParameter;
        //    public CallParameterTaskQueue(CommandServerSocket socket, OfflineCount offlineCount, T controller, ref SubArray<byte> data)
        //        : base(socket, offlineCount, ServerInterfaceControllerIL<T>.Method0)
        //    {
        //        this.controller = controller;
        //        CommandServerCallTaskQueueNode.SetIsDeserialize(this, CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true));
        //    }
        //    public override bool RunTask()
        //    {
        //        CommandServerCallTaskQueue queue;
        //        CommandServerSocket socket = CommandServerCallTaskQueueNode.GetSocket(this, out queue);
        //        return CommandServerCallTaskQueueTask<string>.CheckCallTask(this, controller.TaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref));
        //    }
        //}
        public sealed class CallbackTaskQueue : CommandServerCallbackTaskQueueTask
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public CallbackTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter)
                : base(socket)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }
            public override bool RunTask()
            {
                CommandServerCallTaskQueue queue;
                CommandServerSocket socket = CommandServerCallTaskQueueNode.GetSocket(this, out queue);
                return CommandServerCallbackTaskQueueTask.CheckCallTask(this, controller.CallbackTaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref, new AsynchronousCallback(this)));
            }
        }
        public sealed class SendOnlyTaskQueue : CommandServerCallTaskQueueSendOnlyTask
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public SendOnlyTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter)
                : base(socket)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }
            public override bool RunTask()
            {
                CommandServerCallTaskQueue queue;
                CommandServerSocket socket = CommandServerCallTaskQueueNode.GetSocket(this, out queue);
                return CommandServerCallTaskQueueSendOnlyTask.CheckCallTask(this, controller.SendOnlyTaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref));
            }
        }
        public sealed class KeepCallbackTaskQueue : CommandServerKeepCallbackQueueTask
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public KeepCallbackTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.KeepCallbackTaskQueue, true)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }

            public override bool RunTask()
            {
                CommandServerKeepCallback<string> keepCallback = CommandServerKeepCallback<string>.CreateServerKeepCallback(this, ServerInterfaceControllerIL<T>.Method0);
                CommandServerCallTaskQueue queue;
                CommandServerSocket socket = CommandServerKeepCallbackQueueTask.GetSocket(this, keepCallback, out queue);
                return CommandServerKeepCallbackQueueTask.CheckCallTask(this, controller.KeepCallbackTaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref, keepCallback));
            }
        }
        public sealed class KeepCallbackCountTaskQueue : CommandServerKeepCallbackQueueTask
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public KeepCallbackCountTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter)
                : base(socket, ServerMethodTypeEnum.KeepCallbackCountTaskQueue, true)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }

            public override bool RunTask()
            {
                CommandServerKeepCallbackCount<string> keepCallback = CommandServerKeepCallbackCount<string>.CreateServerKeepCallback(this, ServerInterfaceControllerIL<T>.Method0);
                CommandServerCallTaskQueue queue;
                CommandServerSocket socket = CommandServerKeepCallbackQueueTask.GetSocket(this, keepCallback, out queue);
                return CommandServerKeepCallbackQueueTask.CheckCallTask(this, controller.KeepCallbackCountTaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref, keepCallback));
            }
        }
        public sealed class EnumerableKeepCallbackCountTaskQueue : CommandServerKeepCallbackQueueTask<string>
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public EnumerableKeepCallbackCountTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter)
                : base(socket)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }

            public override bool RunTask()
            {
                CommandServerCallTaskQueue queue;
                CommandServerSocket socket = CommandServerKeepCallbackQueueTask<string>.GetSocket(this, ServerInterfaceControllerIL<T>.Method0, out queue);
                return CommandServerKeepCallbackQueueTask<string>.CheckCallTask(this, controller.EnumerableKeepCallbackCountTaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref));
            }
        }
#if NetStandard21
        public sealed class AsyncEnumerableTaskQueue : AsyncEnumerableQueueTask<string>
        {
            private readonly T controller;
            private SynchronousInputParameter inputParameter;
            public AsyncEnumerableTaskQueue(CommandServerSocket socket, T controller, ref SynchronousInputParameter inputParameter)
                : base(socket)
            {
                this.controller = controller;
                this.inputParameter = inputParameter;
            }

            public override bool RunTask()
            {
                CommandServerCallTaskQueue queue;
                CommandServerSocket socket = AsyncEnumerableQueueTask<string>.GetSocket(this, ServerInterfaceControllerIL<T>.Method0, out queue);
                return AsyncEnumerableQueueTask<string>.CheckCallTask(this, controller.AsyncEnumerableTaskQueue(socket, queue, inputParameter.Value, inputParameter.Ref));
            }
        }
#endif

        /// <summary>
        /// 服务端执行队列
        /// </summary>
        public readonly CommandServerCallQueue Queue0;
        /// <summary>
        /// 服务端执行队列（低优先级）
        /// </summary>
        private readonly CommandServerCallLowPriorityQueue queueLowPriority0;
        /// <summary>
        /// The queue for asynchronous server calls
        /// 服务端异步调用队列
        /// </summary>
        private readonly CommandServerCallTaskQueueSet<int> taskQueueSet0;
        /// <summary>
        /// 命令服务控制器
        /// </summary>
        /// <param name="server">Command server to listen
        /// 命令服务端监听</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="controller">控制器接口实例</param>
        /// <param name="getBindController">获取控制器接口实例</param>
        public ServerInterfaceControllerIL(CommandListener server, string controllerName, T controller, Func<CommandServerController, CommandServerSocket, CommandServerBindContextController> getBindController)
            : base(server, controllerName, controller, getBindController, int.MinValue, 0, false, false)
        {
            Queue0 = CommandListener.GetServerCallQueue(server, 0);
            queueLowPriority0 = CommandListener.GetServerCallQueueLowPriority(server, 0);

            taskQueueSet0 = CommandListener.GetServerCallTaskQueueSet<int>(server).notNull();
        }
        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override CommandClientReturnTypeEnum DoCommand(CommandServerSocket socket, ref SubArray<byte> data)
        {
            switch (CommandServerSocket.GetCommandMethodIndex(socket))
            {
                case (int)ServerMethodTypeEnum.Unknown: return CommandClientReturnTypeEnum.Unknown;
                case (int)ServerMethodTypeEnum.VersionExpired: return CommandClientReturnTypeEnum.VersionExpired;

                case (int)ServerMethodTypeEnum.Synchronous:
                    SynchronousInputParameter inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        //CommandServerVerifyState returnValue = Controller.Synchronous(socket, inputParameter.Value);
                        //CommandServerSocket.SetVerifyState(socket, returnValue);
                        //CommandServerSocket.SendReturnValue(socket, Method0, returnValue);
                        SynchronousOutputParameter outputParameter = new SynchronousOutputParameter();
                        outputParameter.__Return__ = GetController(this, socket).Synchronous(socket, inputParameter.Value, ref inputParameter.Ref, out outputParameter.Out);
                        outputParameter.Ref = inputParameter.Ref;
                        CommandServerSocket.SendOutput(socket, Method0, ref outputParameter);
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.SendOnly:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        GetController(this, socket).SendOnly(socket, inputParameter.Value, inputParameter.Ref);
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.Callback:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        GetController(this, socket).Callback(socket, inputParameter.Value, inputParameter.Ref, new AsynchronousCallback(socket));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallback:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerKeepCallback<string> keepCallback = CommandServerKeepCallback<string>.CreateServerKeepCallback(socket, Method0);
                        GetController(this, socket).KeepCallback(socket, inputParameter.Value, inputParameter.Ref, keepCallback);
                        CommandServerKeepCallback.CancelKeep(keepCallback);
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackCount:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        GetController(this, socket).KeepCallbackCount(socket, inputParameter.Value, inputParameter.Ref, CommandServerKeepCallbackCount<string>.CreateServerKeepCallback(socket, Method0));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;

                case (int)ServerMethodTypeEnum.Queue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerCallQueue.Add(Queue0, new SynchronousQueue(socket, this, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    //return CommandClientReturnTypeEnum.ServerDeserializeError;
                    return CommandServerCallQueue.AddIsDeserialize(Queue0, new SynchronousParameterQueue(socket, this, ref data));
                case (int)ServerMethodTypeEnum.SendOnlyQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerCallQueue.Add(Queue0, new SendOnlyQueue(socket, this, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.CallbackQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerCallQueue.Add(Queue0, new CallbackQueue(socket, this, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerCallQueue.Add(Queue0, new KeepCallbackQueue(socket, this, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackCountQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerCallQueue.Add(Queue0, new KeepCallbackCountQueue(socket, this, ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;

                case (int)ServerMethodTypeEnum.Task:
                    if (ServerInterfaceMethod.IsSynchronousCallTask(Method0, socket))
                    {
                        inputParameter = new SynchronousInputParameter();
                        if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                        {
                            long timestamp = Stopwatch.GetTimestamp();
                            Task task = GetController(this, socket).AsynchronousTask(socket, inputParameter.Value, inputParameter.Ref);
                            ServerInterfaceMethod.CheckGetTaskTimestamp(Method0, timestamp);
                            CommandServerSocket.CheckTask(socket, task);
                            return CommandClientReturnTypeEnum.Success;
                        }
                        return CommandClientReturnTypeEnum.ServerDeserializeError;
                    }
                    return CommandServerRunTask.RunTaskIsDeserialize(new RunTask(socket, GetController(this, socket), ref data));
                case (int)ServerMethodTypeEnum.CallbackTask:
                    if (ServerInterfaceMethod.IsSynchronousCallTask(Method0, socket))
                    {
                        inputParameter = new SynchronousInputParameter();
                        if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                        {
                            AsynchronousCallbackTask callback = new AsynchronousCallbackTask(socket);
                            CommandServerCallbackTask<string>.CheckTask(callback, GetController(this, socket).CallbackTask(socket, inputParameter.Value, inputParameter.Ref, callback));
                            return CommandClientReturnTypeEnum.Success;
                        }
                        return CommandClientReturnTypeEnum.ServerDeserializeError;
                    }
                    return CommandServerCallbackRunTask<string>.RunTaskIsDeserialize(new CallbackRunTask(socket, GetController(this, socket), ref data));
                case (int)ServerMethodTypeEnum.SendOnlyTask:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CheckTask(socket, GetController(this, socket).SendOnlyTask(socket, inputParameter.Value, inputParameter.Ref));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackTask:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerKeepCallbackTask<string> keepCallback = CommandServerKeepCallbackTask<string>.CreateServerKeepCallbackTask(socket, ServerInterfaceControllerIL<T>.Method0);
                        CommandServerKeepCallbackTask<string>.CheckTask(keepCallback, GetController(this, socket).KeepCallbackTask(socket, inputParameter.Value, inputParameter.Ref, keepCallback));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackCountTask:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerKeepCallbackCountTask<string> keepCallback = CommandServerKeepCallbackCountTask<string>.CreateServerKeepCallbackTask(socket, ServerInterfaceControllerIL<T>.Method0);
                        CommandServerKeepCallbackCountTask<string>.CheckTask(keepCallback, GetController(this, socket).KeepCallbackCountTask(socket, inputParameter.Value, inputParameter.Ref, keepCallback));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerEnumerableKeepCallbackCountTask<string>.CreateServerKeepCallbackTask(socket, ServerInterfaceControllerIL<T>.Method0, GetController(this, socket).EnumerableKeepCallbackCountTask(socket, inputParameter.Value, inputParameter.Ref));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
#if NetStandard21
                case (int)ServerMethodTypeEnum.AsyncEnumerableTask:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerAsyncEnumerableTask<string>.CreateServerKeepCallbackTask(socket, ServerInterfaceControllerIL<T>.Method0, GetController(this, socket).AsyncEnumerableTask(socket, inputParameter.Value, inputParameter.Ref));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
#endif

                case (int)ServerMethodTypeEnum.TaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(taskQueueSet0, inputParameter.Value, new CallTaskQueue(socket, GetController(this, socket), ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.CallbackTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(taskQueueSet0, inputParameter.Value, new CallbackTaskQueue(socket, GetController(this, socket), ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.SendOnlyTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(taskQueueSet0, inputParameter.Value, new SendOnlyTaskQueue(socket, GetController(this, socket), ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(taskQueueSet0, inputParameter.Value, new KeepCallbackTaskQueue(socket, GetController(this, socket), ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(taskQueueSet0, inputParameter.Value, new KeepCallbackCountTaskQueue(socket, GetController(this, socket), ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
                case (int)ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(taskQueueSet0, inputParameter.Value, new EnumerableKeepCallbackCountTaskQueue(socket, GetController(this, socket), ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
#if NetStandard21
                case (int)ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
                    inputParameter = new SynchronousInputParameter();
                    if (CommandServerSocket.Deserialize(socket, ref data, ref inputParameter, true))
                    {
                        CommandServerSocket.CallTaskQueueAppendQueue(taskQueueSet0, inputParameter.Value, new AsyncEnumerableTaskQueue(socket, GetController(this, socket), ref inputParameter));
                        return CommandClientReturnTypeEnum.Success;
                    }
                    return CommandClientReturnTypeEnum.ServerDeserializeError;
#endif
            }
            return CommandClientReturnTypeEnum.Unknown;
        }

        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        public static readonly ServerInterfaceMethod Method0;
        static ServerInterfaceControllerIL()
        {
            Method0 = ServerInterfaceController<T>.GetMethod(0);
        }
    }
    #endregion
#endif
}
