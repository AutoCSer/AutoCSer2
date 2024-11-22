using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 接口任务队列
    /// </summary>
    public sealed class InterfaceControllerTaskQueue : TaskQueueBase
    {
        /// <summary>
        /// 任务队列
        /// </summary>
        private Link<InterfaceControllerTaskQueueNodeBase>.YieldQueue queue;
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected override void run()
        {
            do
            {
                waitHandle.Wait();
                if (isDisposed) return;
                var value = queue.GetClear();
                var currentTask = value;
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            currentTask = value;
                            value.RunTask(ref value);
                            if (isDisposed) return;
                        }
                        break;
                    }
                    catch (Exception exception)
                    {
                        try
                        {
                            currentTask.notNull().SetReturnType(CommandClientReturnTypeEnum.ServerException);
                        }
                        catch(Exception serverException)
                        {
                            AutoCSer.LogHelper.ExceptionIgnoreException(serverException, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                        }
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                }
                while (value != null);
            }
            while (!isDisposed);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddOnly(InterfaceControllerTaskQueueNodeBase node)
        {
            if (queue.IsPushHead(node)) waitHandle.Set();
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Add(InterfaceControllerTaskQueue queue, InterfaceControllerTaskQueueNodeBase node)
        {
            queue.AddOnly(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        public void Add(InterfaceControllerTaskQueueCustomNode node)
        {
            if (node.CheckQueue()) AddOnly(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        /// <typeparam name="T">返回值类型</typeparam>
        public void Add<T>(InterfaceControllerTaskQueueCustomNode<T> node)
        {
            if (node.CheckQueue()) AddOnly(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
        /// 创建接口任务队列调用接口实例
        /// </summary>
        /// <typeparam name="T">调用接口类型</typeparam>
        /// <typeparam name="ST">服务实现实例</typeparam>
        /// <param name="service">服务实现实例类型</param>
        /// <returns>调用接口实例</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T CreateController<T, ST>(ST service) where ST : class
        {
            return TaskQueueInterfaceController<T, ST>.Create(this, service);
        }
    }
}
