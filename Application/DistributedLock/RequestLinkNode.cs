using System;

namespace AutoCSer.CommandService.DistributedLock
{
    /// <summary>
    /// 锁请求链表节点
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct RequestLinkNode
    {
        /// <summary>
        /// 上一个请求节点
        /// </summary>
        internal IDistributedLockRequest PreviousRequest;
        /// <summary>
        /// 下一个请求节点
        /// </summary>
        internal IDistributedLockRequest NextRequest;
        /// <summary>
        /// 从链表中删除当前尾节点并返回下一个尾节点
        /// </summary>
        /// <returns>下一个尾节点</returns>
        internal IDistributedLockRequest RemoveEnd()
        {
            if (PreviousRequest == null) return null;
            IDistributedLockRequest linkEnd = PreviousRequest;
            linkEnd.NextRequest = null;
            PreviousRequest = null;
            return linkEnd;
        }
        /// <summary>
        /// 从链表中删除当前节点
        /// </summary>
        internal void Remove()
        {
            if (PreviousRequest != null) PreviousRequest.NextRequest = NextRequest;
            if (NextRequest != null)
            {
                NextRequest.PreviousRequest = PreviousRequest;
                NextRequest = null;
            }
            PreviousRequest = null;
        }
    }
}
