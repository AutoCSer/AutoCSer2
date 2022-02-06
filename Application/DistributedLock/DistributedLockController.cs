using AutoCSer.Net;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 分布式锁服务端
    /// </summary>
    public abstract class DistributedLockController : CommandServerBindController
    {
        /// <summary>
        /// 请求标识生成器
        /// </summary>
        public readonly DistributedMillisecondIdentityGenerator IdentityGenerator;
        /// <summary>
        /// 分布式锁服务端
        /// </summary>
        /// <param name="identityGenerator">请求标识生成器</param>
        public DistributedLockController(DistributedMillisecondIdentityGenerator identityGenerator = null)
        {
            IdentityGenerator = identityGenerator
             ?? ((ConfigObject<DistributedMillisecondIdentityGenerator>)AutoCSer.Configuration.Common.Get(typeof(DistributedMillisecondIdentityGenerator)))?.Value
             ?? new DistributedMillisecondIdentityGenerator();
        }
    }
}
