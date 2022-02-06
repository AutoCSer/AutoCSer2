using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 锁请求对象
    /// </summary>
    public interface IDistributedLockRequest
    {
        /// <summary>
        /// 锁客户端状态是否有效
        /// </summary>
        bool IsLock { get; }
        /// <summary>
        /// 上一个请求节点
        /// </summary>
        IDistributedLockRequest PreviousRequest { set; }
        /// <summary>
        /// 下一个请求节点
        /// </summary>
        IDistributedLockRequest NextRequest { set; }
        /// <summary>
        /// 从链表中删除当前节点
        /// </summary>
        void Remove();
        /// <summary>
        /// 从链表中删除当前尾节点并返回下一个尾节点
        /// </summary>
        /// <returns>下一个尾节点</returns>
        IDistributedLockRequest RemoveEnd();
        /// <summary>
        /// 从链表中删除当前尾节点并返回下一个尾节点
        /// </summary>
        /// <param name="requests">需要短线重连的锁请求集合</param>
        /// <returns>下一个尾节点</returns>
        IDistributedLockRequest RemoveAgain(ref AutoCSer.LeftArray<IDistributedLockRequest> requests);
        /// <summary>
        /// 短线重连
        /// </summary>
        /// <returns></returns>
        Task EnterAgain();
    }
}
