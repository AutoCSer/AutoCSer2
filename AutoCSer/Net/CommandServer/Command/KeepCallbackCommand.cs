using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 保持回调命令 await CommandKeepCallback
    /// </summary>
    public abstract class KeepCallbackCommand : KeepCommand, IDisposable
    {
        /// <summary>
        /// 添加输出命令通知
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal KeepCallbackCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// 释放资源
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
        /// 等待添加输出队列
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
        /// 是否成功添加输出队列
        /// </summary>
        /// <returns></returns>
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
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeepCallbackCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// 默认空回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="command"></param>
        private static void nullCallback(CommandClientReturnValue returnValue, KeepCallbackCommand command) { }
        /// <summary>
        /// 默认空回调
        /// </summary>
        public static readonly Action<CommandClientReturnValue, KeepCallbackCommand> NullCallback = nullCallback;
    }
}
