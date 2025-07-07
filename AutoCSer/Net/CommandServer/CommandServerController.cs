using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command service controller
    /// 命令服务控制器
    /// </summary>
    public abstract class CommandServerController
    {
        /// <summary>
        /// 多命令控制器模式最大命令数量有效位
        /// </summary>
        internal const int MaxCommandBits = 16;
        /// <summary>
        /// 多命令控制器模式最大命令数量
        /// </summary>
        internal const int MaxCommandCount = 1 << MaxCommandBits;
        /// <summary>
        /// 默认命令服务控制器配置
        /// </summary>
        internal static readonly CommandServerControllerInterfaceAttribute DefaultAttribute = AutoCSer.Configuration.Common.Get<CommandServerControllerInterfaceAttribute>()?.Value ?? new CommandServerControllerInterfaceAttribute { IsAutoMethodIndex = true };

        /// <summary>
        /// 命令服务
        /// </summary>
        public readonly CommandListener Server;
        /// <summary>
        /// 控制器名称
        /// </summary>
        public readonly string ControllerName;
        /// <summary>
        /// 命令控制器配置
        /// </summary>
        internal readonly CommandServerControllerInterfaceAttribute Attribute;
        /// <summary>
        /// 服务端接口方法信息集合
        /// </summary>
#if NetStandard21
        internal readonly ServerInterfaceMethod?[] Methods;
#else
        internal readonly ServerInterfaceMethod[] Methods;
#endif
        /// <summary>
        /// Verification method
        /// 验证方法
        /// </summary>
#if NetStandard21
        internal readonly ServerInterfaceMethod? VerifyMethod;
#else
        internal readonly ServerInterfaceMethod VerifyMethod;
#endif
        /// <summary>
        /// 验证方法序号
        /// </summary>
        internal readonly int VerifyMethodIndex;
        /// <summary>
        /// 同步调用队列
        /// </summary>
#if NetStandard21
        internal readonly CommandServerCallQueue? CallQueue;
#else
        internal readonly CommandServerCallQueue CallQueue;
#endif
        /// <summary>
        /// 同步调用低优先级队列
        /// </summary>
#if NetStandard21
        internal readonly CommandServerCallLowPriorityQueue? CallQueueLowPriority;
#else
        internal readonly CommandServerCallLowPriorityQueue CallQueueLowPriority;
#endif
        /// <summary>
        /// 支持并发读队列
        /// </summary>
        internal readonly CommandServerCallConcurrencyReadQueue CallConcurrencyReadQueue;
        /// <summary>
        /// 读写队列
        /// </summary>
        internal readonly CommandServerCallReadQueue CallReadWriteQueue;
        /// <summary>
        /// The queue for asynchronous server calls
        /// 服务端异步调用队列
        /// </summary>
#if NetStandard21
        internal CommandServerControllerCallTaskQueue? CallTaskQueue;
#else
        internal CommandServerControllerCallTaskQueue CallTaskQueue;
#endif
        /// <summary>
        /// 起始命令序号
        /// </summary>
        internal int CommandStartIndex;
        /// <summary>
        /// 结束命令序号
        /// </summary>
        internal int CommandEndIndex;
        /// <summary>
        /// 控制器在服务中的索引编号
        /// </summary>
        internal int ControllerIndex;
        /// <summary>
        /// 默认空命令服务控制器
        /// </summary>
        /// <param name="server"></param>
        internal CommandServerController(CommandListener server)
        {
            Server = server;
#if NetStandard21
            ControllerName = string.Empty;
            Attribute = DefaultAttribute;
            Methods = EmptyArray<ServerInterfaceMethod>.Array;
            CallConcurrencyReadQueue = server.CallConcurrencyReadQueue;
            CallReadWriteQueue = server.CallReadWriteQueue;
#endif
        }
        /// <summary>
        /// Command service controller
        /// 命令服务控制器
        /// </summary>
        /// <param name="server">Command server to listen
        /// 命令服务端监听</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="attribute"></param>
        /// <param name="methods"></param>
        /// <param name="verifyMethodIndex"></param>
        /// <param name="controllerQueue">控制器同步队列标记</param>
        /// <param name="isConcurrencyReadQueue">是否存在并发读队列</param>
        /// <param name="isReadWriteQueue">是否存在控制器读写队列</param>
#if NetStandard21
        internal CommandServerController(CommandListener server, string controllerName, CommandServerControllerInterfaceAttribute attribute, ServerInterfaceMethod?[] methods, int verifyMethodIndex, byte controllerQueue, bool isConcurrencyReadQueue, bool isReadWriteQueue)
#else
        internal CommandServerController(CommandListener server, string controllerName, CommandServerControllerInterfaceAttribute attribute, ServerInterfaceMethod[] methods, int verifyMethodIndex, byte controllerQueue, bool isConcurrencyReadQueue, bool isReadWriteQueue)
#endif
        {
            Server = server;
            ControllerName = controllerName;
            Attribute = attribute;
            Methods = methods;
            if (verifyMethodIndex >= 0)
            {
                VerifyMethod = methods[verifyMethodIndex];
                VerifyMethodIndex = verifyMethodIndex + CommandListener.MethodStartIndex;
            }
            else VerifyMethodIndex = 0;
            if (controllerQueue != 0)
            {
                CallQueue = new CommandServerCallQueue(server, this, 0);
                CallQueueLowPriority = CallQueue.CreateLink();
            }
            CallConcurrencyReadQueue = isConcurrencyReadQueue ? new CommandServerCallConcurrencyReadQueue(server, this) : server.CallConcurrencyReadQueue;
            CallReadWriteQueue = isReadWriteQueue ? new CommandServerCallReadQueue(server, this, attribute.MaxReadWriteQueueConcurrency) : server.CallReadWriteQueue;
            if (attribute.TaskQueueMaxConcurrent > 0) CallTaskQueue = new CommandServerControllerCallTaskQueue(this);
            server.Append(this);
        }
        /// <summary>
        /// 关闭控制器
        /// </summary>
        internal virtual void Close()
        {
            CallQueue?.Close();
            CallTaskQueue = null;
            if (!object.ReferenceEquals(Server.CallConcurrencyReadQueue, CallConcurrencyReadQueue)) CallConcurrencyReadQueue.Close();
            if (!object.ReferenceEquals(Server.CallReadWriteQueue, CallReadWriteQueue)) CallReadWriteQueue.Close();
        }
        /// <summary>
        /// 控制器接口实例
        /// </summary>
        /// <returns></returns>
        public virtual object GetControllerObject() { throw new InvalidOperationException(); }
        /// <summary>
        /// 设置起始命令序号
        /// </summary>
        /// <param name="commandStartIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetCommandStartIndex(int commandStartIndex)
        {
            CommandStartIndex = commandStartIndex;
            CommandEndIndex = commandStartIndex + Methods.Length;
        }
        /// <summary>
        /// 获取服务端接口方法信息
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ServerInterfaceMethod? GetMethod(int methodIndex)
#else
        internal ServerInterfaceMethod GetMethod(int methodIndex)
#endif
        {
            return methodIndex < Methods.Length ? Methods[methodIndex] : null;
        }
        /// <summary>
        /// 获取命令位图索引
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetCommandMapIndex(int methodIndex)
        {
            return methodIndex + CommandStartIndex - CommandListener.MethodStartIndex;
        }
        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="socket">Command server socket
        /// 命令服务套接字</param>
        /// <param name="data">命令数据</param>
        /// <returns></returns>
        public abstract CommandClientReturnTypeEnum DoCommand(CommandServerSocket socket, ref SubArray<byte> data);
        /// <summary>
        /// 添加同步调用队列任务
        /// </summary>
        /// <param name="node"></param>
        /// <returns>返回 false 表示当前控制器不支持异步队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool AddQueue(CommandServerCallQueueCustomNode node)
        {
            if(CallQueue != null)
            {
                CallQueue.Add(node);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加同步调用队列低优先级任务
        /// </summary>
        /// <param name="node"></param>
        /// <returns>返回 false 表示当前控制器不支持异步队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool AddQueueLowPriority(CommandServerCallQueueCustomNode node)
        {
            if (CallQueueLowPriority != null)
            {
                CallQueueLowPriority.Add(node);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <returns>返回 false 表示当前控制器不支持异步队列</returns>
        public bool Add(Func<CommandServerCallTaskQueue, Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue != null)
            {
                CallTaskQueue.AddOnly(new CommandServerCallTaskQueueCustomTask(getTask, true));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueCustomTask? AddTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueCustomTask AddTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(getTask, isSynchronous);
            CallTaskQueue.AddOnly(task);
            return task;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueExceptionTask? AddExceptionTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueExceptionTask AddExceptionTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(getTask, isSynchronous);
            CallTaskQueue.AddOnly(task);
            return task;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueCustomTask<T>? AddTask<T>(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueCustomTask<T> AddTask<T>(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueCustomTask<T> task = new CommandServerCallTaskQueueCustomTask<T>(getTask, isSynchronous);
            CallTaskQueue.AddOnly(task);
            return task;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueExceptionTask<T>? AddExceptionTask<T>(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueExceptionTask<T> AddExceptionTask<T>(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueExceptionTask<T> task = new CommandServerCallTaskQueueExceptionTask<T>(getTask, isSynchronous);
            CallTaskQueue.AddOnly(task);
            return task;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <returns>返回 false 表示当前控制器不支持异步队列</returns>
        public bool Add(Func<Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue != null)
            {
                CallTaskQueue.AddOnly(new CommandServerCallTaskQueueCustomTask(getTask, true));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueCustomTask? AddTask(Func<Task> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueCustomTask AddTask(Func<Task> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(getTask, isSynchronous);
            CallTaskQueue.AddOnly(task);
            return task;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueExceptionTask? AddExceptionTask(Func<Task> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueExceptionTask AddExceptionTask(Func<Task> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(getTask, isSynchronous);
            CallTaskQueue.AddOnly(task);
            return task;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueCustomTask<T>? AddTask<T>(Func<Task<T>> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueCustomTask<T> AddTask<T>(Func<Task<T>> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueCustomTask<T> task = new CommandServerCallTaskQueueCustomTask<T>(getTask, isSynchronous);
            CallTaskQueue.AddOnly(task);
            return task;
        }
        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueExceptionTask<T>? AddExceptionTask<T>(Func<Task<T>> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueExceptionTask<T> AddExceptionTask<T>(Func<Task<T>> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueExceptionTask<T> task = new CommandServerCallTaskQueueExceptionTask<T>(getTask, isSynchronous);
            CallTaskQueue.AddOnly(task);
            return task;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <returns>返回 false 表示当前控制器不支持异步队列</returns>
        public bool AddLowPriority(Func<CommandServerCallTaskQueue, Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue != null)
            {
                CallTaskQueue.AddLowPriorityOnly(new CommandServerCallTaskQueueCustomTask(getTask, true));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueCustomTask? AddLowPriorityTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueCustomTask AddLowPriorityTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(getTask, isSynchronous);
            CallTaskQueue.AddLowPriorityOnly(task);
            return task;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueExceptionTask? AddLowPriorityExceptionTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueExceptionTask AddLowPriorityExceptionTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(getTask, isSynchronous);
            CallTaskQueue.AddLowPriorityOnly(task);
            return task;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueCustomTask<T>? AddLowPriorityTask<T>(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueCustomTask<T> AddLowPriorityTask<T>(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueCustomTask<T> task = new CommandServerCallTaskQueueCustomTask<T>(getTask, isSynchronous);
            CallTaskQueue.AddLowPriorityOnly(task);
            return task;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueExceptionTask<T>? AddLowPriorityExceptionTask<T>(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueExceptionTask<T> AddLowPriorityExceptionTask<T>(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueExceptionTask<T> task = new CommandServerCallTaskQueueExceptionTask<T>(getTask, isSynchronous);
            CallTaskQueue.AddLowPriorityOnly(task);
            return task;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <returns>返回 false 表示当前控制器不支持异步队列</returns>
        public bool AddLowPriority(Func<Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue != null)
            {
                CallTaskQueue.AddLowPriorityOnly(new CommandServerCallTaskQueueCustomTask(getTask, true));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueCustomTask? AddLowPriorityTask(Func<Task> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueCustomTask AddLowPriorityTask(Func<Task> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(getTask, isSynchronous);
            CallTaskQueue.AddLowPriorityOnly(task);
            return task;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueExceptionTask? AddLowPriorityExceptionTask(Func<Task> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueExceptionTask AddLowPriorityExceptionTask(Func<Task> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(getTask, isSynchronous);
            CallTaskQueue.AddLowPriorityOnly(task);
            return task;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueCustomTask<T>? AddLowPriorityTask<T>(Func<Task<T>> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueCustomTask<T> AddLowPriorityTask<T>(Func<Task<T>> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueCustomTask<T> task = new CommandServerCallTaskQueueCustomTask<T>(getTask, isSynchronous);
            CallTaskQueue.AddLowPriorityOnly(task);
            return task;
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象，返回 null 表示当前控制器不支持异步队列</returns>
#if NetStandard21
        public CommandServerCallTaskQueueExceptionTask<T>? AddLowPriorityExceptionTask<T>(Func<Task<T>> getTask, bool isSynchronous = false)
#else
        public CommandServerCallTaskQueueExceptionTask<T> AddLowPriorityExceptionTask<T>(Func<Task<T>> getTask, bool isSynchronous = false)
#endif
        {
            if (getTask == null) throw new ArgumentNullException();
            if (CallTaskQueue == null) return null;
            CommandServerCallTaskQueueExceptionTask<T> task = new CommandServerCallTaskQueueExceptionTask<T>(getTask, isSynchronous);
            CallTaskQueue.AddLowPriorityOnly(task);
            return task;
        }

        /// <summary>
        /// Add the queue task
        /// 添加队列任务
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AddTaskQueue(CommandServerController controller, CommandServerCallTaskQueueNode value)
        {
            controller.CallTaskQueue.notNull().AddOnly(value);
        }
        /// <summary>
        /// Add low priority task to the queue
        /// 添加队列低优先级任务
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AddTaskQueueLowPriority(CommandServerController controller, CommandServerCallTaskQueueNode value)
        {
            controller.CallTaskQueue.notNull().AddLowPriorityOnly(value);
        }
    }
    /// <summary>
    /// Command service controller
    /// 命令服务控制器
    /// </summary>
    /// <typeparam name="T">控制器接口类型</typeparam>
    public abstract class CommandServerController<T> : CommandServerController
    {
        /// <summary>
        /// 控制器接口实例
        /// </summary>
        public readonly T Controller;
        /// <summary>
        /// 获取控制器接口实例
        /// </summary>
        private readonly Func<CommandServerController, CommandServerSocket, CommandServerBindContextController> getBindController;
        /// <summary>
        /// Command service controller
        /// 命令服务控制器
        /// </summary>
        /// <param name="server">Command server to listen
        /// 命令服务端监听</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="controller"></param>
        /// <param name="getBindController"></param>
        /// <param name="verifyMethodIndex"></param>
        /// <param name="controllerQueue">控制器同步队列标记</param>
        /// <param name="isConcurrencyReadQueue">是否存在并发读队列</param>
        /// <param name="isReadWriteQueue">是否存在控制器读写队列</param>
        internal CommandServerController(CommandListener server, string controllerName, T controller, Func<CommandServerController, CommandServerSocket, CommandServerBindContextController> getBindController, int verifyMethodIndex, byte controllerQueue, bool isConcurrencyReadQueue, bool isReadWriteQueue)
            : base(server, controllerName, ServerInterfaceController<T>.ControllerAttribute, ServerInterfaceController<T>.Methods, verifyMethodIndex, controllerQueue, isConcurrencyReadQueue, isReadWriteQueue)
        {
            this.Controller = controller;
            this.getBindController = getBindController;
        }
        /// <summary>
        /// 关闭控制器
        /// </summary>
        internal override void Close()
        {
            base.Close();
            (Controller as IDisposable)?.Dispose();
        }
        /// <summary>
        /// 控制器接口实例
        /// </summary>
        /// <returns></returns>
        public override object GetControllerObject() { return Controller.castObject(); }
        /// <summary>
        /// 获取控制器接口实例
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private T getController(CommandServerSocket socket)
        {
            if (getBindController == null) return this.Controller;
            var controller = socket.GetBindController(ControllerIndex);
            if (controller != null) return (T)(object)controller;
            socket.SetBindController(ControllerIndex, controller = getBindController(this, socket));
            return (T)(object)controller;
        }
        /// <summary>
        /// 获取控制器接口实例
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T GetController(CommandServerController<T> controller, CommandServerSocket socket)
        {
            return controller.getController(socket);
        }
        /// <summary>
        /// 获取控制器接口实例
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T GetQueueNodeController(CommandServerController<T> controller, CommandServerCallQueueNode node)
        {
            return controller.getController(node.Socket);
        }
        /// <summary>
        /// 获取控制器接口实例
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T GetReadWriteQueueNodeController(CommandServerController<T> controller, CommandServerCallReadWriteQueueNode node)
        {
            return controller.getController(node.Socket);
        }
        /// <summary>
        /// 获取控制器接口实例
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T GetConcurrencyReadQueueNodeController(CommandServerController<T> controller, CommandServerCallConcurrencyReadQueueNode node)
        {
            return controller.getController(node.Socket);
        }
    }
}
