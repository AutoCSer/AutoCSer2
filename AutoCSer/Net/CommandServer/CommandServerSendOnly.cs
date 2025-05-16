using System;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 不返回数据方法返回值类型定义
    /// </summary>
    public sealed class CommandServerSendOnly
    {
        /// <summary>
        /// 默认空数据
        /// </summary>
        public static readonly CommandServerSendOnly Null = new CommandServerSendOnly();
        /// <summary>
        /// 默认空任务
        /// </summary>
        public static readonly Task<CommandServerSendOnly> NullTask = Task.FromResult(Null);
    }
}
