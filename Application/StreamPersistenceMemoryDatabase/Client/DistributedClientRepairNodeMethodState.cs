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
        /// Call status
        /// 调用状态
        /// </summary>
        public readonly CallStateEnum State;
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess
        {
            get { return ReturnType == CommandClientReturnTypeEnum.Success && State == CallStateEnum.Success; }
        }
        /// <summary>
        /// 调用错误的客户端
        /// </summary>
#if NetStandard21
        public readonly StreamPersistenceMemoryDatabaseClient? ErrorClient;
#else
        public readonly StreamPersistenceMemoryDatabaseClient ErrorClient;
#endif
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
#if NetStandard21
        internal DistributedClientRepairNodeMethodState(CallStateEnum state, StreamPersistenceMemoryDatabaseClient? client)
#else
        internal DistributedClientRepairNodeMethodState(CallStateEnum state, StreamPersistenceMemoryDatabaseClient client)
#endif
        {
            ReturnType = CommandClientReturnTypeEnum.Success;
            State = state;
            ErrorClient = client;
        }
    }
}
