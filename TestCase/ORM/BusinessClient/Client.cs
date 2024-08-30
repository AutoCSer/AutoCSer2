using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// 命令客户端
    /// </summary>
    public sealed class Client
    {
        /// <summary>
        /// 命令客户端
        /// </summary>
        private readonly CommandClient commandClient;
        /// <summary>
        /// 命令客户端
        /// </summary>
        public static CommandClient CommandClient { get { return Instance.commandClient; } }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        private readonly CommandClientSocketEvent socketEvent;
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        public static CommandClientSocketEvent SocketEvent { get { return Instance.socketEvent; } }
        /// <summary>
        /// 分布式锁客户端
        /// </summary>
        private readonly AutoCSer.CommandService.DistributedLockClient<string> distributedLockClient;
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        public static AutoCSer.CommandService.DistributedLockClient<string> DistributedLockClient { get { return Instance.distributedLockClient; } }
        /// <summary>
        /// 命令客户端
        /// </summary>
        /// <param name="commandClient"></param>
        /// <param name="socketEvent"></param>
        /// <param name="distributedLockClient"></param>
        private Client(CommandClient commandClient, CommandClientSocketEvent socketEvent, AutoCSer.CommandService.DistributedLockClient<string> distributedLockClient)
        {
            this.commandClient = commandClient;
            this.socketEvent = socketEvent;
            this.distributedLockClient = distributedLockClient;
        }
        /// <summary>
        /// 获取分布式业务锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ReleaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="KeepSeconds">心跳间隔秒数</param>
        /// <returns></returns>
        public static async Task<CommandClientReturnValue<AutoCSer.CommandService.DistributedLockKeepRequest<string>>> EnterDistributedLockAsync(string key, int ReleaseSeconds = 60, int KeepSeconds = 15)
        {
            CommandClientSocketEvent socketEvent = SocketEvent;
            if (socketEvent.DistributedLockClient == null)
            {
                await CommandClient.GetSocketAsync();
                if (socketEvent.DistributedLockClient == null) return new CommandClientReturnValue { ReturnType = CommandClientReturnTypeEnum.ClientUnknown, ErrorMessage = "业务数据服务连接中，请稍后重试！" };
            }
            return await DistributedLockClient.Enter(key, ReleaseSeconds, KeepSeconds);
        }

        /// <summary>
        /// 自增ID与其它混合测试模型业务数据服务客户端接口
        /// </summary>
        public static IAutoIdentityModelClient AutoIdentityModelClient { get { return SocketEvent.AutoIdentityModelClient; } }
        /// <summary>
        /// 字段测试模型业务数据服务客户端接口
        /// </summary>
        public static IFieldModelClient FieldModelClient { get { return SocketEvent.FieldModelClient; } }
        /// <summary>
        /// 属性测试模型业务数据服务客户端接口
        /// </summary>
        public static IPropertyModelClient PropertyModelClient { get { return SocketEvent.PropertyModelClient; } }
        /// <summary>
        /// 自定义字段列测试模型业务数据服务客户端接口
        /// </summary>
        public static ICustomColumnFieldModelClient CustomColumnFieldModelClient { get { return SocketEvent.CustomColumnFieldModelClient; } }
        /// <summary>
        /// 自定义属性列测试模型业务数据服务客户端接口
        /// </summary>
        public static ICustomColumnPropertyModelClient CustomColumnPropertyModelClient { get { return SocketEvent.CustomColumnPropertyModelClient; } }

        /// <summary>
        /// 命令客户端初始化访问锁
        /// </summary>
        private static AutoCSer.Threading.SemaphoreSlimLock instanceLock = new AutoCSer.Threading.SemaphoreSlimLock(1);
        /// <summary>
        /// 命令客户端
        /// </summary>
        private static Client instance;
        /// <summary>
        /// 命令客户端
        /// </summary>
        public static Client Instance
        {
            get
            {
                if (instance != null) return instance;
                LogHelper.ErrorIgnoreException("请在 Main 函数中初始化调用 await Initialize() 避免产生同步阻塞");
                return Initialize().Result;
            }
        }
        /// <summary>
        /// 获取命令客户端
        /// </summary>
        /// <returns></returns>
        public static async Task<Client> GetInstance()
        {
            if (instance != null) return instance;
            return await Initialize();
        }
        /// <summary>
        /// 初始化命令客户端
        /// </summary>
        /// <returns></returns>
        public static async Task<Client> Initialize()
        {
            await instanceLock.EnterAsync();
            try
            {
                if (instance == null)
                {
                    CommandClientConfig commandClientConfig = new CommandClientConfig
                    {
                        MinCompressSize = 1024, Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ORM),
                        GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
                    };
                    CommandClient commandClient = new CommandClient(commandClientConfig);
                    CommandClientSocket commandClientSocket = await commandClient.GetSocketAsync();
                    if (commandClientSocket == null)
                    {
                        await LogHelper.Error("业务数据服务连接失败");
                        return null;
                    }
                    CommandClientSocketEvent socketEvent = (CommandClientSocketEvent)commandClient.SocketEvent;
                    instance = new Client(commandClient, socketEvent, new AutoCSer.CommandService.DistributedLockClient<string>(socketEvent));
                }
            }
            finally { instanceLock.Exit(); }
            return instance;
        }
    }
}
