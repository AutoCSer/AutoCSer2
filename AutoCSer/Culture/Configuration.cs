using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AutoCSer.Culture
{
    /// <summary>
    /// 系统语言文化配置
    /// </summary>
    public abstract class Configuration
    {
        /// <summary>
        /// 拓扑排序循环错误信息
        /// </summary>
        public abstract string TopologySortLoopError { get; }
        /// <summary>
        /// 该实例正在序列化对象操作，不允许释放资源
        /// </summary>
        public abstract string NotAllowDisposeSerializer { get; }
        /// <summary>
        /// RPC 客户端命令控制器名称重复冲突
        /// </summary>
        /// <param name="controllerName">重复的控制器名称</param>
        /// <returns></returns>
        public abstract string GetCommandClientControllerNameRepeatedly(string controllerName);
        /// <summary>
        /// RPC 客户端命令控制器数量超出上限
        /// </summary>
        /// <param name="controllerCount">控制器数量</param>
        /// <param name="maxControllerCount">控制器数量最大限制</param>
        /// <returns></returns>
        public abstract string GetCommandClientControllerCountLimit(int controllerCount, int maxControllerCount);
        /// <summary>
        /// 不支持非接口类型
        /// </summary>
        /// <param name="type">不支持的类型</param>
        /// <returns></returns>
        public abstract string GetNotInterfaceType(Type type);
        /// <summary>
        /// RPC 客户端缺少控制器名称
        /// </summary>
        public abstract string CommandClientControllerEmptyName { get; }
        /// <summary>
        /// RPC 客户端控制器创建器创建失败
        /// </summary>
        /// <param name="clientInterfaceType">客户端控制器接口类型</param>
        /// <param name="serverInterfaceType">服务端控制器接口类型</param>
        /// <returns></returns>
        public abstract string GetCommandClientControllerCreateFailed(Type clientInterfaceType, Type serverInterfaceType);
        /// <summary>
        /// RPC 命令控制器最大命令序号超出限制
        /// </summary>
        /// <param name="methodIndex">控制器最大命令序号</param>
        /// <param name="maxCommandCount">最大命令数量</param>
        /// <returns></returns>
        public abstract string GetCommandServerControllerMethodCountLimit(int methodIndex, int maxCommandCount);
        /// <summary>
        /// RPC 命令控制器命令数量超出限制
        /// </summary>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="methodCount">控制器命令数量</param>
        /// <param name="maxCommandCount">最大命令数量</param>
        /// <returns></returns>
        public abstract string GetCommandServerControllerMethodCountLimit(string controllerName, int methodCount, int maxCommandCount);
        /// <summary>
        /// RPC 命令控制器数量已经达到上限
        /// </summary>
        /// <param name="maxControllerCount">最大控制器数量</param>
        /// <returns></returns>
        public abstract string GetCommandServerControllerCountLimit(int maxControllerCount);
        /// <summary>
        /// RPC 服务端命令控制器名称重复冲突
        /// </summary>
        /// <param name="controllerName">重复的控制器名称</param>
        /// <returns></returns>
        public abstract string GetCommandServerControllerNameRepeatedly(string controllerName);
        /// <summary>
        /// RPC 服务端缺少控制器名称
        /// </summary>
        public abstract string CommandServerControllerEmptyName { get; }
        /// <summary>
        /// RPC 服务端 Task 队列服务控制器关键字类型重复冲突
        /// </summary>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="otherControllerName">冲突的控制器名称</param>
        /// <param name="keyTye">冲突的关键字类型</param>
        /// <returns></returns>
        public abstract string GetCommandServerTaskQueueKeyTypeRepeatedly(string controllerName, string otherControllerName, Type keyTye);
        /// <summary>
        /// RPC 服务端控制器实例已经绑定套接字上下文
        /// </summary>
        public abstract string CommandServerControllerBound { get; }
        /// <summary>
        /// RPC 服务端套接字上下文绑定控制器实例必须传入构造委托
        /// </summary>
        public abstract string CommandServerControllerNotFoundConstructDelegate { get; }
        /// <summary>
        /// RPC 服务端控制器服务不匹配
        /// </summary>
        public abstract string CommandServerControllerServerNotMatch { get; }
        /// <summary>
        /// RPC 服务端缺少控制器参数
        /// </summary>
        public abstract string CommandServerMissingControllerParameter { get; }
        /// <summary>
        /// RPC 命令控制器命令序号超出限制
        /// </summary>
        /// <param name="methodIndex">控制器命令序号</param>
        /// <param name="methodCount">命令数量</param>
        /// <returns></returns>
        public abstract string GetCommandServerControllerMethodIndexLimit(int methodIndex, int methodCount);
        /// <summary>
        /// RPC 服务缺少监听端口号
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
#if NetStandard21
        public abstract string GetCommandServerNotFoundPort(string? serverName);
#else
        public abstract string GetCommandServerNotFoundPort(string serverName);
#endif
        /// <summary>
        /// 反向 RPC 服务缺少控制器信息
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
#if NetStandard21
        public abstract string GetReverseCommandServerNotFoundController(string? serverName);
#else
        public abstract string GetReverseCommandServerNotFoundController(string serverName);
#endif
        /// <summary>
        /// 状态机查找数据不能为空
        /// </summary>
        public abstract string GetStateSearcherEmptyState { get; }
        /// <summary>
        /// 状态机查找数据重复
        /// </summary>
        /// <param name="errorValue">重复数据</param>
        public abstract string GetStateSearcherStateRepetition(string errorValue);

        /// <summary>
        /// 获取默认系统语言文化配置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Configuration GetDefault()
        {
            return IsChinese ? (Configuration)Chinese.Default : English.Default;
        }
        /// <summary>
        /// 是否中文环境
        /// </summary>
        /// <returns></returns>
        public static readonly bool IsChinese;
        static Configuration()
        {
            string cultureName = CultureInfo.CurrentCulture.Name;
            switch (cultureName.Length - 5)
            {
                case 0:
                    if ((cultureName[0] | 0x20) == 'z' && (cultureName[1] | 0x20) == 'h' && cultureName[2] == '-')
                    {
                        switch (cultureName[3] | 0x20)
                        {
                            case 'c':
                                if ((cultureName[4] | 0x20) == 'n') IsChinese = true;
                                break;
                            case 'h':
                                if ((cultureName[4] | 0x20) == 'k') IsChinese = true;
                                break;
                            case 't':
                                if ((cultureName[4] | 0x20) == 'w') IsChinese = true;
                                break;
                            case 'm':
                                if ((cultureName[4] | 0x20) == 'o') IsChinese = true;
                                break;
                            case 's':
                                if ((cultureName[4] | 0x20) == 'g') IsChinese = true;
                                break;
                        }
                    }
                    break;
                case 1:
                    if ((cultureName[0] | 0x20) == 'z' && (cultureName[1] | 0x20) == 'h' && cultureName[2] == '-'
                        && (cultureName[3] | 0x20) == 'c' && (cultureName[4] | 0x20) == 'h')
                    {
                        switch (cultureName[5] & 1)
                        {
                            case 's':
                                if ((cultureName[5] | 0x20) == 's') IsChinese = true;
                                break;
                            case 't':
                                if ((cultureName[5] | 0x20) == 't') IsChinese = true;
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
