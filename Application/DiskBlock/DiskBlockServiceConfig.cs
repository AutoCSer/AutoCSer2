using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 磁盘块服务配置
    /// </summary>
    public class DiskBlockServiceConfig
    {
        /// <summary>
        /// 磁盘块服务唯一编号
        /// </summary>
        public uint Identity;
        /// <summary>
        /// 自动写入字节大小，默认为 1MB，最小值为 4KB
        /// </summary>
        public int AutoFlushSize = 1 << 20;
        /// <summary>
        /// 自动写入间隔毫秒数，默认为最小值 1ms
        /// </summary>
        public int AutoFlushMilliseconds = 1;
        /// <summary>
        /// 缓存最大总字节数，默认为 1GB
        /// </summary>
        public long MaxCacheTotalSize = 1 << 30;
        /// <summary>
        /// 单个数据缓存最大字节数，超出大小不缓存，默认为 1MB
        /// </summary>
        public int MaxCacheSize = 1 << 20;
        /// <summary>
        /// 切换磁盘块限制最小字节数，默认为 1GB，最小值为 1B，低于该字节数不允许切换
        /// </summary>
        public long MinSwitchSize = 1 << 30;
    }
}
