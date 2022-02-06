using System;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// 命令服务端口
    /// </summary>
    public enum CommandServerPort : ushort
    {
        /// <summary>
        /// 示例端口
        /// </summary>
        Example = 12900,
        /// <summary>
        /// 测试用例端口
        /// </summary>
        TestCase,
        /// <summary>
        /// 性能测试端口
        /// </summary>
        Performance,
        /// <summary>
        /// 服务注册测试端口
        /// </summary>
        ServiceRegistry,
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证测试端口
        /// </summary>
        TimestampVerify,
        /// <summary>
        /// 分布式锁测试端口
        /// </summary>
        DistributedLock,
        /// <summary>
        /// 守护进程测试端口
        /// </summary>
        ProcessGuard,
        /// <summary>
        /// 数据库备份
        /// </summary>
        DatabaseBackup,
    }
}
