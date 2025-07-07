using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// The controller instance binds the command service controller
    /// 控制器实例绑定命令服务控制器
    /// </summary>
    public interface ICommandServerBindController
    {
        /// <summary>
        /// Bind the command service controller
        /// 绑定命令服务控制器
        /// </summary>
        /// <param name="controller"></param>
        void Bind(CommandServerController controller);
    }
}
