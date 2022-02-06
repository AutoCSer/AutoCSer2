using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册结果
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct ServiceRegisterResponse
    {
        /// <summary>
        /// 服务标识ID
        /// </summary>
        public long ServiceID;
        /// <summary>
        /// 服务注册状态
        /// </summary>
        public ServiceRegisterState State;
        /// <summary>
        /// 服务注册结果
        /// </summary>
        /// <param name="serviceID">服务标识ID</param>
        public ServiceRegisterResponse(long serviceID)
        {
            ServiceID = serviceID;
            State = ServiceRegisterState.Success;
        }
        /// <summary>
        /// 错误服务注册结果
        /// </summary>
        /// <param name="state"></param>
        public ServiceRegisterResponse(ServiceRegisterState state)
        {
            ServiceID = 0;
            State = state;
        }
    }
}
