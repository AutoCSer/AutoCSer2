using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 控制器实例绑定命令服务控制器
    /// </summary>
    public interface ICommandServerBindController
    {
        /// <summary>
        /// 绑定命令服务控制器
        /// </summary>
        /// <param name="controller"></param>
        void Bind(CommandServerController controller);
    }
}
