using AutoCSer.Metadata;
using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command service method configuration
    /// 命令服务方法配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandServerMethodAttribute : CommandMethodAttribute
    {
        /// <summary>
        /// Maintain the callback output count, a hold callback API setting for waiting counts, with a default of 1. It cannot batch output data. For internal service high-frequency call scenarios, it is recommended to set appropriate values based on specific business data and the size of the output cache to improve throughput and control memory usage
        /// 保持回调输出计数，用于等待计数的保持回调 API 设置，默认为 1 无法批量输出数据，内部服务高频调用场景建议根据具体业务数据以及输出缓存区大小设置合适值以提高吞吐量并控制内存占用
        /// </summary>
        public int KeepCallbackOutputCount = 1;
        /// <summary>
        /// By default, false indicates that the framework schedules tasks based on concurrency; otherwise, tasks are synchronously called using IO threads
        /// 默认为 false 表示框架根据并发情况调度 Task，否则使用 IO 线程同步调用 Task
        /// </summary>
        public bool IsSynchronousCallTask;
        /// <summary>
        /// By default, false indicates that the output object cache pool is not adopted. For frequent calls to the output interface, it is recommended to set it to true
        /// 默认为 false 表示不采用输出对象缓存池，频繁调用输出接口建议设置为 true
        /// </summary>
        public bool IsOutputPool;
        /// <summary>
        /// The serial number of the synchronous queue. By default, 0 indicates an independent queue of the controller; otherwise, it is a shared queue of the current service
        /// 同步队列序号，默认为 0 表示控制器独立队列，否则为当前服务的共享队列
        /// </summary>
        public byte QueueIndex;
        /// <summary>
        /// By default, true indicates that the controller independently supports the synchronous queue for parallel reading, while setting false represents the shared synchronous queue of the current service
        /// 默认为 true 表示控制器独立支持并行读的同步队列，设置为 false 则为当前服务的共享同步队列
        /// </summary>
        public bool IsControllerConcurrencyReadQueue = true;
        /// <summary>
        /// By default, true indicates the independent read and write queue of the controller, while setting false represents the shared read and write queue of the current service
        /// 默认为 true 表示控制器独立读写队列，设置为 false 则为当前服务的共享读写队列
        /// </summary>
        public bool IsControllerReadWriteQueue = true;
        /// <summary>
        /// The default is true, indicating the use of the controller asynchronous task queue; otherwise, the queue keyword parameter needs to be prepared
        /// 默认为 true 表示采用控制器异步任务队列，否则需要准备队列关键字参数
        /// </summary>
        public bool IsControllerTaskQueue = true;
        /// <summary>
        /// The default is false, indicating that the Task queue controller mode does not use the low-priority API
        /// 默认为 false 表示 Task 队列控制器模式不使用低优先级 API
        /// </summary>
        public bool IsLowPriorityTaskQueue;
        /// <summary>
        /// The default value of true indicates that the hold callback is automatically cancelled when the API call is completed, so all data output operations must be completed within the API. Otherwise, CancelKeep needs to be manually called to cancel for asynchronous call scenarios
        /// 默认为 true 表示在 API 调用完成时自动取消保持回调，所以要求数据输出操作都在 API 中完成；否则需要手动调用 CancelKeep 取消用于异步调用场景
        /// </summary>
        public bool AutoCancelKeep = true;
        /// <summary>
        /// Whether to enable the service offline notification counting logic, which is used for singleton server registration to wait for all tasks to be completed before going offline and notifying the new service to go online, and to keep the calling-related interfaces from waiting for asynchronous callbacks to complete
        /// 是否启用服务下线通知计数逻辑，用于单例服务注册等待所有任务完成以后下线并通知新服务上线，保持回调相关接口不等待异步回调完成
        /// </summary>
        public bool IsOfflineCount;
        /// <summary>
        /// Is expired
        /// </summary>
        public bool IsExpired;

        /// <summary>
        /// Default command service method configuration
        /// 默认命令服务方法配置
        /// </summary>
        internal static readonly CommandServerMethodAttribute Default = new CommandServerMethodAttribute();
    }
}
