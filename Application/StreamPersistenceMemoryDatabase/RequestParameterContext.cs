using AutoCSer.Extensions;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 请求参数反序列化上下文
    /// </summary>
    internal sealed class RequestParameterContext
    {
        /// <summary>
        /// Command server socket
        /// 命令服务套接字
        /// </summary>
        private readonly CommandServerSocket socket;
        /// <summary>
        /// Command service controller
        /// 命令服务控制器
        /// </summary>
        private readonly CommandServerController controller;
        /// <summary>
        /// Log stream persistence memory database service
        /// 日志流持久化内存数据库服务
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseService service;
        /// <summary>
        /// 请求参数反序列化上下文
        /// </summary>
        /// <param name="socket"></param>
        private RequestParameterContext(CommandServerSocket socket)
        {
            this.socket = socket;
            controller = socket.CurrentController;
            service = (StreamPersistenceMemoryDatabaseService)controller.GetControllerObject();
        }
        /// <summary>
        /// 获取日志流持久化内存数据库服务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
#if NetStandard21
        internal StreamPersistenceMemoryDatabaseService? GetService(object? context)
#else
        internal StreamPersistenceMemoryDatabaseService GetService(object context)
#endif
        {
            return object.ReferenceEquals(context, socket) && object.ReferenceEquals(socket.CurrentController, controller) ? service : null;
        }
        /// <summary>
        /// 获取日志流持久化内存数据库服务
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
#if NetStandard21
        internal static StreamPersistenceMemoryDatabaseService? GetService(AutoCSer.BinaryDeserializer deserializer)
#else
        internal static StreamPersistenceMemoryDatabaseService GetService(AutoCSer.BinaryDeserializer deserializer)
#endif
        {
            CommandServerSocket socket = deserializer.Context.castType<CommandServerSocket>().notNull();
            if(!object.ReferenceEquals(socket, CommandServerSocket.CommandServerSocketContext))
            {
                RequestParameterContext context = new RequestParameterContext(socket);
                deserializer.StreamPersistenceMemoryDatabaseServiceRequestParameterContext = context;
                return context.service;
            }
            return null;
        }
    }
}
