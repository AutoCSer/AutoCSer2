using AutoCSer.Net.CommandServer;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command controller interface configuration
    /// 命令服务控制器接口配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class CommandServerControllerInterfaceAttribute : InterfaceMethodIndexAttribute
    {
        /// <summary>
        /// The default is true, indicating the generation method sequence number mapping enumeration type
        /// 默认为 true 表示生成方法序号映射枚举类型
        /// </summary>
        public bool IsCodeGeneratorMethodEnum = true;
        /// <summary>
        /// Generate method sequence number mapping enumeration type code relative path. The default is null, indicating that the code is generated in the default file {xxx}.AutoCSer.cs
        /// 生成方法序号映射枚举类型代码相对路径，默认为 null 表示代码生成到默认文件 {xxx}.AutoCSer.cs 中
        /// </summary>
#if NetStandard21
        public string? MethodIndexEnumTypeCodeGeneratorPath;
#else
        public string MethodIndexEnumTypeCodeGeneratorPath;
#endif
        /// <summary>
        /// The default true indicates that the code generator generates the client controller interface
        /// 默认为 true 表示代码生成器生成客户端控制器接口
        /// </summary>
        public bool IsCodeGeneratorClientInterface = true;
#if AOT
        /// <summary>
        /// A default value of true indicates that the default client controller configuration is generated
        /// 默认为 true 表示生成默认客户端控制器配置
        /// </summary>
        public bool IsCodeGeneratorControllerAttribute = true;
#else
        /// <summary>
        /// Reserved, only for AOT code generation
        /// 保留，仅用于 AOT 代码生成
        /// </summary>
        public bool IsCodeGeneratorControllerAttribute;
#endif
        /// <summary>
        /// The default value true indicates that input parameters are preferred for simple serialization operations
        /// 默认为 true 表示输入参数优先适配简单序列化操作
        /// </summary>
        public bool IsSimpleSerializeInputParameter = true;
        /// <summary>
        /// The default true indicates that the output parameters are preferred for simple serialization operations
        /// 默认为 true 表示输出参数优先适配简单序列化操作
        /// </summary>
        public bool IsSimpleSerializeOutputParameter = true;

        /// <summary>
        /// The maximum concurrent number of the default read/write queue is set to the number of CPU logical processors minus 1 by default
        /// 默认读写队列最大并发数量，默认为 CPU 逻辑处理器数量 - 1
        /// </summary>
        public int MaxReadWriteQueueConcurrency = AutoCSer.Common.ProcessorCount - 1;
        /// <summary>
        /// The default value is 0, indicating that the controller queue is disabled. If the recommended value of the controller queue is set to 1, the concurrency problem is ignored. If the value is greater than 1, the concurrent throughput can be increased. In addition, write operations can be performed only after all unfinished read operations are complete. Therefore, the number of concurrent read tasks should not be too large to prevent long write operation waiting time
        /// 异步读写队列最大读操作并发任务数量，默认为 0 表示不启用控制器队列，如果启用控制器队列建议值为 1 可以不考虑并发问题，当设置大于 1 时可提高并发吞吐，但是访问共享资源需要增加队列锁操作，而且写操作需要等待所有未完成读取操作结束以后才能执行，所以并发读取任务数量不宜过大避免造成写操作等待时间过长
        /// </summary>
        public int TaskQueueMaxConcurrent;
        /// <summary>
        /// The number of waiting read and write tasks in the asynchronous read and write queue. The default value is 16 and the minimum value is 1. The number of waiting read and write tasks should not be too large to prevent long write wait time
        /// 异步读写队列写操作等待读取操作任务数量，默认为 16，最小值为 1，等待读取操作任务数量不宜过大避免造成写操作等待时间过长
        /// </summary>
        public int TaskQueueWaitCount = 16;

        /// <summary>
        /// Copy the command controller configuration
        /// 复制命令控制器配置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CommandServerControllerInterfaceAttribute Clone()
        {
           return (CommandServerControllerInterfaceAttribute)MemberwiseClone();
        }
        /// <summary>
        /// Gets the method number mapping enumeration type
        /// 获取方法序号映射枚举类型
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        internal static string GetControllerName(Type interfaceType)
        {
            var methodIndexEnumType = interfaceType.GetCustomAttribute<ServerControllerInterfaceAttribute>()?.MethodIndexEnumType;
            if (methodIndexEnumType == null) throw new ArgumentNullException(AutoCSer.Common.Culture.CommandServerControllerEmptyName);
            return methodIndexEnumType.Name;
        }
    }
}
