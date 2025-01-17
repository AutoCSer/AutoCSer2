﻿using System;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// 命令服务端口
    /// </summary>
    public enum CommandServerPortEnum : ushort
    {
        /// <summary>
        /// 示例端口
        /// </summary>
        Example = 12900,
        /// <summary>
        /// 文档测试端口
        /// </summary>
        Document,
        /// <summary>
        /// 测试用例端口
        /// </summary>
        TestCase,
        /// <summary>
        /// 命令服务性能测试端口
        /// </summary>
        Performance,
        /// <summary>
        /// 服务注册测试端口
        /// </summary>
        ServiceRegistry,
        /// <summary>
        /// 服务端口注册测试端口
        /// </summary>
        PortRegistry,
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
        /// 数据库备份端口
        /// </summary>
        DatabaseBackup,
        /// <summary>
        /// 反向日志收集端口
        /// </summary>
        ReverseLogCollection,
        /// <summary>
        /// ORM 业务数据服务测试端口
        /// </summary>
        ORM,
        /// <summary>
        /// 日志流持久化内存缓存测试端口
        /// </summary>
        StreamPersistenceMemoryDatabase,
        /// <summary>
        /// 磁盘块服务测试端口
        /// </summary>
        DiskBlock,
        /// <summary>
        /// 文件同步测试端口
        /// </summary>
        FileSynchronous,

        /// <summary>
        /// 注册服务测试端口
        /// </summary>
        ServiceRegistryPort = 20000,
    }
}
