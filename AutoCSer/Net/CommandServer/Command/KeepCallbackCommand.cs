using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// The keep callback command (await AutoCSer.Net.CommandKeepCallback, return the keep callback object of the command)
    /// 保持回调命令（await AutoCSer.Net.CommandKeepCallback，返回命令保持回调对象）
    /// </summary>
    public abstract class KeepCallbackCommand : KeepCommand, IDisposable
    {
        /// <summary>
        /// The keep callback command (await AutoCSer.Net.CommandKeepCallback, return the keep callback object of the command)
        /// 保持回调命令（await AutoCSer.Net.CommandKeepCallback，返回命令保持回调对象）
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal KeepCallbackCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                keepCallback.Cancel(false);
            }
        }
        /// <summary>
        /// Wait for the command to add the output queue
        /// 等待命令添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<CommandKeepCallback?> Wait()
#else
        public async Task<CommandKeepCallback> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Get the command to keep callback object
        /// 获取命令保持回调对象
        /// </summary>
        /// <returns>The operation of adding to the output queue failed and returned null
        /// 添加到输出队列操作失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public CommandKeepCallback? GetResult()
#else
        public CommandKeepCallback GetResult()
#endif
        {
            return PushState == CommandPushStateEnum.Success ? keepCallback : null;
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeepCallbackCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Default empty callback
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="command"></param>
        private static void nullCallback(CommandClientReturnValue returnValue, KeepCallbackCommand command) { }
        /// <summary>
        /// Default empty callback
        /// </summary>
        public static readonly Action<CommandClientReturnValue, KeepCallbackCommand> NullCallback = nullCallback;
    }
}
