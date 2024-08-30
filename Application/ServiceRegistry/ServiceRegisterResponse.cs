using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册结果
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ServiceRegisterResponse
    {
        /// <summary>
        /// 服务标识ID
        /// </summary>
        public long ServiceID;
        /// <summary>
        /// 服务注册状态
        /// </summary>
        public ServiceRegisterStateEnum State;
        /// <summary>
        /// 服务注册结果
        /// </summary>
        /// <param name="serviceID">服务标识ID</param>
        public ServiceRegisterResponse(long serviceID)
        {
            ServiceID = serviceID;
            State = ServiceRegisterStateEnum.Success;
        }
        /// <summary>
        /// 错误服务注册结果
        /// </summary>
        /// <param name="state"></param>
        public ServiceRegisterResponse(ServiceRegisterStateEnum state)
        {
            ServiceID = 0;
            State = state;
        }
    }
}
