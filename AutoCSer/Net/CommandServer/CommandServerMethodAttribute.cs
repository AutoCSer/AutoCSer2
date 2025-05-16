using AutoCSer.Metadata;
using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令服务方法配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandServerMethodAttribute : CommandMethodAttribute
    {
        /// <summary>
        /// 保持回调输出计数，用于等待计数的保持回调 API 设置，默认为 1 无法批量输出数据，内部服务高频调用场景建议根据具体业务数据以及输出缓存区大小设置合适值以提高吞吐量并控制内存占用
        /// </summary>
        public int KeepCallbackOutputCount = 1;
        /// <summary>
        /// 默认为 false 表示框架根据并发情况调度 Task，否则使用 IO 线程同步调用 Task
        /// </summary>
        public bool IsSynchronousCallTask;
        /// <summary>
        /// 输出对象是否采用缓存池，默认为 false，频繁调用输出接口建议设置为 true
        /// </summary>
        public bool IsOutputPool;
        /// <summary>
        /// 同步队列序号，默认为 0 表示控制器独立队列，否则为当前服务的共享队列
        /// </summary>
        public byte QueueIndex;
        /// <summary>
        /// 默认为 true 表示控制器独立支持并行读的同步队列，设置为 false 则为当前服务的共享同步队列
        /// </summary>
        public bool IsControllerConcurrencyReadQueue = true;
        /// <summary>
        /// 默认为 true 表示控制器独立读写队列，设置为 false 则为当前服务的共享读写队列
        /// </summary>
        public bool IsControllerReadWriteQueue = true;
        /// <summary>
        /// 默认为 true 表示采用控制器异步任务队列，否则需要准备队列关键字参数
        /// </summary>
        public bool IsControllerTaskQueue = true;
        /// <summary>
        /// Task 队列控制器模式是否低优先级 API，默认为 false
        /// </summary>
        public bool IsLowPriorityTaskQueue;
        /// <summary>
        /// 默认为 true 表示在 API 调用完成时自动取消保持回调，否则需要手动调用 CancelKeep 取消用于异步调用场景
        /// </summary>
        public bool AutoCancelKeep = true;
        /// <summary>
        /// 是否启用服务下线通知计数逻辑，用于单例服务注册等待所有任务完成以后下线并通知新服务上线，保持回调相关接口不等待异步回调完成
        /// </summary>
        public bool IsOfflineCount;
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired;

        /// <summary>
        /// 默认命令服务方法配置
        /// </summary>
        internal static readonly CommandServerMethodAttribute Default = new CommandServerMethodAttribute();
    }
}
