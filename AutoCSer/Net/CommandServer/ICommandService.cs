using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令服务接口
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// 设置命令服务
        /// </summary>
        /// <param name="listener"></param>
        void Set(CommandListener listener);
    }
}
