using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式客户端修复接口方法状态结果
    /// </summary>
    public struct DistributedClientRepairNodeMethodState
    {
        /// <summary>
        /// RPC 调用状态
        /// </summary>
        public readonly CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// 调用状态
        /// </summary>
        public readonly CallStateEnum State;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get { return ReturnType == CommandClientReturnTypeEnum.Success && State == CallStateEnum.Success; }
        }
        /// <summary>
        /// 调用错误的客户端
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClient ErrorClient;
        /// <summary>
        /// 分布式客户端修复接口方法状态结果
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="client"></param>
        internal DistributedClientRepairNodeMethodState(CommandClientReturnTypeEnum returnType, StreamPersistenceMemoryDatabaseClient client)
        {
            ReturnType = returnType;
            State = CallStateEnum.Unknown;
            ErrorClient = client;
        }
        /// <summary>
        /// 分布式客户端修复接口方法状态结果
        /// </summary>
        /// <param name="state"></param>
        /// <param name="client"></param>
        internal DistributedClientRepairNodeMethodState(CallStateEnum state, StreamPersistenceMemoryDatabaseClient client)
        {
            ReturnType = CommandClientReturnTypeEnum.Success;
            State = state;
            ErrorClient = client;
        }
    }
}
