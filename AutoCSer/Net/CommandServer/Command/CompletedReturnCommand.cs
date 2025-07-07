using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// The completed return value command
    /// 已完成的返回值命令
    /// </summary>
    public sealed class CompletedReturnCommand : ReturnCommand
    {
        /// <summary>
        /// The completed return value command
        /// 已完成的返回值命令
        /// </summary>
        /// <param name="returnType">返回值</param>
        private CompletedReturnCommand(CommandClientReturnTypeEnum returnType) : base(returnType) { }

        /// <summary>
        /// 未知返回值
        /// </summary>
        public static readonly CompletedReturnCommand Unknown = new CompletedReturnCommand(CommandClientReturnTypeEnum.Unknown);
    }
    /// <summary>
    /// The completed return value command
    /// 已完成的返回值命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CompletedReturnCommand<T> : ReturnCommand<T>
    {
        /// <summary>
        /// Default empty command
        /// </summary>
        /// <param name="controller"></param>
        internal CompletedReturnCommand(CommandClientController controller) : base(controller) { }
        /// <summary>
        /// The completed return value command
        /// 已完成的返回值命令
        /// </summary>
        /// <param name="returnValue">Return value</param>
        public CompletedReturnCommand(T returnValue) : base(ref returnValue) { }
        /// <summary>
        /// The completed return value command
        /// 已完成的返回值命令
        /// </summary>
        /// <param name="returnValue">Return value</param>
        public CompletedReturnCommand(ref T returnValue) : base(ref returnValue) { }

        /// <summary>
        /// 默认值已完成返回值命令
        /// </summary>
#pragma warning disable CS8604
        public static readonly CompletedReturnCommand<T> Default = new CompletedReturnCommand<T>(default(T));
#pragma warning restore CS8604
    }
}
