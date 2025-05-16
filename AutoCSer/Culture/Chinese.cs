using AutoCSer.Extensions;
using System;

namespace AutoCSer.Culture
{
    /// <summary>
    /// 中文配置
    /// </summary>
    public class Chinese : Configuration
    {
        /// <summary>
        /// 拓扑排序循环错误信息
        /// </summary>
        public override string TopologySortLoopError { get { return "拓扑排序循环错误。"; } }
        /// <summary>
        /// 该实例正在序列化对象操作，不允许释放资源
        /// </summary>
        public override string NotAllowDisposeSerializer { get { return "该实例正在序列化对象操作，不允许释放资源。"; } }
        /// <summary>
        /// RPC 客户端命令控制器名称重复冲突
        /// </summary>
        /// <param name="controllerName">重复的控制器名称</param>
        /// <returns></returns>
        public override string GetCommandClientControllerNameRepeatedly(string controllerName)
        {
            return $"命令控制器名称 {controllerName} 冲突。";
        }
        /// <summary>
        /// RPC 客户端命令控制器数量超出上限
        /// </summary>
        /// <param name="controllerCount">控制器数量</param>
        /// <param name="maxControllerCount">控制器数量最大限制</param>
        /// <returns></returns>
        public override string GetCommandClientControllerCountLimit(int controllerCount, int maxControllerCount)
        {
            return $"命令控制器数量 {controllerCount.toString()} 超出上限 {maxControllerCount.toString()}";
        }
        /// <summary>
        /// 不支持非接口类型
        /// </summary>
        /// <param name="type">不支持的类型</param>
        /// <returns></returns>
        public override string GetNotInterfaceType(Type type)
        {
            return $"不支持非接口类型 {type.fullName()}";
        }
        /// <summary>
        /// RPC 客户端缺少控制器名称
        /// </summary>
        public override string CommandClientControllerEmptyName { get { return "缺少控制器名称。"; } }
        /// <summary>
        /// RPC 客户端控制器创建器创建失败
        /// </summary>
        /// <param name="clientInterfaceType">客户端控制器接口类型</param>
        /// <param name="serverInterfaceType">服务端控制器接口类型</param>
        /// <returns></returns>
        public override string GetCommandClientControllerCreateFailed(Type clientInterfaceType, Type serverInterfaceType)
        {
            return $"客户端控制器创建器创建失败 {clientInterfaceType.fullName()} + {serverInterfaceType.fullName()}";
        }
        /// <summary>
        /// RPC 命令控制器最大命令序号超出限制
        /// </summary>
        /// <param name="methodIndex">控制器最大命令序号</param>
        /// <param name="maxCommandCount">最大命令数量</param>
        /// <returns></returns>
        public override string GetCommandServerControllerMethodCountLimit(int methodIndex, int maxCommandCount)
        {
            return $"命令控制器最大命令序号 {methodIndex.toString()} 超出限制 {maxCommandCount.toString()}，不允许扩展命令控制器。";
        }
        /// <summary>
        /// RPC 命令控制器命令数量超出限制
        /// </summary>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="methodCount">控制器命令数量</param>
        /// <param name="maxCommandCount">最大命令数量</param>
        /// <returns></returns>
        public override string GetCommandServerControllerMethodCountLimit(string controllerName, int methodCount, int maxCommandCount)
        {
            return $"命令控制器 {controllerName} 命令数量 {methodCount.toString()} 超出限制 {maxCommandCount.toString()}";
        }
        /// <summary>
        /// RPC 命令控制器数量已经达到上限
        /// </summary>
        /// <param name="maxControllerCount">最大控制器数量</param>
        /// <returns></returns>
        public override string GetCommandServerControllerCountLimit(int maxControllerCount)
        {
            return $"命令控制器数量已经达到上限 {maxControllerCount.toString()}";
        }
        /// <summary>
        /// RPC 服务端命令控制器名称重复冲突
        /// </summary>
        /// <param name="controllerName">重复的控制器名称</param>
        /// <returns></returns>
        public override string GetCommandServerControllerNameRepeatedly(string controllerName)
        {
            return $"命令控制器名称 {controllerName} 重复冲突。";
        }
        /// <summary>
        /// RPC 服务端缺少控制器名称
        /// </summary>
        public override string CommandServerControllerEmptyName { get { return "缺少控制器名称。"; } }
        /// <summary>
        /// RPC 服务端 Task 队列服务控制器关键字类型重复冲突
        /// </summary>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="otherControllerName">冲突的控制器名称</param>
        /// <param name="keyTye">冲突的关键字类型</param>
        /// <returns></returns>
        public override string GetCommandServerTaskQueueKeyTypeRepeatedly(string controllerName, string otherControllerName, Type keyTye)
        {
            return $"Task 队列服务控制器 {controllerName} 与 {otherControllerName} 关键字类型 {keyTye.fullName()} 重复冲突。";
        }
        /// <summary>
        /// RPC 服务端控制器实例已经绑定套接字上下文
        /// </summary>
        public override string CommandServerControllerBound
        {
            get { return "控制器实例已经绑定套接字上下文，请传入构造委托重新生成控制器实例。"; }
        }
        /// <summary>
        /// RPC 服务端套接字上下文绑定控制器实例必须传入构造委托
        /// </summary>
        public override string CommandServerControllerNotFoundConstructDelegate
        {
            get { return "套接字上下文绑定控制器实例必须传入构造委托，每个套接字都需要新建实例。"; }
        }
        /// <summary>
        /// RPC 服务端控制器服务不匹配
        /// </summary>
        public override string CommandServerControllerServerNotMatch { get { return "控制器服务不匹配。"; } }
        /// <summary>
        /// RPC 服务端缺少控制器参数
        /// </summary>
        public override string CommandServerMissingControllerParameter { get { return "缺少控制器参数。"; } }
        /// <summary>
        /// RPC 命令控制器命令序号超出限制
        /// </summary>
        /// <param name="methodIndex">控制器命令序号</param>
        /// <param name="methodCount">命令数量</param>
        /// <returns></returns>
        public override string GetCommandServerControllerMethodIndexLimit(int methodIndex, int methodCount)
        {
            return $"命令序号 {methodIndex.toString()} 超出控制器命令范围 {methodCount.toString()}";
        }
        /// <summary>
        /// RPC 服务缺少监听端口号
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
#if NetStandard21
        public override string GetCommandServerNotFoundPort(string? serverName)
#else
        public override string GetCommandServerNotFoundPort(string serverName)
#endif
        {
            return $"服务 {serverName} 缺少监听端口号。";
        }
        /// <summary>
        /// 反向 RPC 服务缺少控制器信息
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
#if NetStandard21
        public override string GetReverseCommandServerNotFoundController(string? serverName)
#else
        public override string GetReverseCommandServerNotFoundController(string serverName)
#endif
        {
            return $"反向服务 {serverName} 缺少控制器信息。";
        }
        /// <summary>
        /// 状态机查找数据不能为空
        /// </summary>
        public override string GetStateSearcherEmptyState { get { return "查找数据不能为空。"; } }
        /// <summary>
        /// 状态机查找数据重复
        /// </summary>
        /// <param name="errorValue">重复数据</param>
        public override string GetStateSearcherStateRepetition(string errorValue)
        {
            return $"查找数据 {errorValue} 重复。";
        }

        /// <summary>
        /// 默认中文配置
        /// </summary>
        public static readonly Chinese Default = new Chinese();
    }
}
