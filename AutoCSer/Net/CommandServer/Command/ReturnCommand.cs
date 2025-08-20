using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// The return value command (await AutoCSer.Net.CommandClientReturnValue)
    /// 返回值命令（await AutoCSer.Net.CommandClientReturnValue）
    /// </summary>
    public abstract class ReturnCommand : BaseReturnCommand
    {
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// The return value command has been completed
        /// 已完成返回值命令
        /// </summary>
        /// <param name="returnType"></param>
        internal ReturnCommand(CommandClientReturnTypeEnum returnType)
        {
            ReturnType = returnType;
            IsCompleted = true;
            continuation = Common.EmptyAction;
        }
        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<CommandClientReturnValue> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the command call, return an error message before the result is returned (Only for supporting await)
        /// 获取命令调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue GetResult()
        {
            return new CommandClientReturnValue(ReturnType, ErrorMessage);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Get the command return value
        /// 获取命令返回值
        /// </summary>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandReturnValue GetValue(bool isIgnoreError = false)
        {
            return new CommandReturnValue(this, isIgnoreError);
        }

        /// <summary>
        /// Convert to a Task object
        /// 转换为 Task 对象
        /// </summary>
        /// <param name="returnCommand"></param>
        /// <returns></returns>
        public static async Task GetTask(ReturnCommand returnCommand)
        {
            CommandClientReturnValue value = await returnCommand;
            if (value.IsSuccess) return;
            if (string.IsNullOrEmpty(value.ErrorMessage)) throw new Exception(value.ReturnType.ToString());
            throw new Exception($"{value.ReturnType} {value.ErrorMessage}");
        }
    }
    /// <summary>
    /// The return value command (await AutoCSer.Net.CommandClientReturnValue{T})
    /// 返回值命令（await AutoCSer.Net.CommandClientReturnValue{T}）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReturnCommand<T> : BaseReturnCommand
    {
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal T ReturnValue;
        /// <summary>
        /// Default empty command
        /// </summary>
        /// <param name="controller"></param>
        internal ReturnCommand(CommandClientController controller) : base(controller) { }
        /// <summary>
        /// The return value command has been completed
        /// 已完成返回值命令
        /// </summary>
        /// <param name="returnValue"></param>
        internal ReturnCommand(ref T returnValue)
        {
            ReturnValue = returnValue;
            ReturnType = CommandClientReturnTypeEnum.Success;
            IsCompleted = true;
            continuation = Common.EmptyAction;
        }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        internal ReturnCommand(CommandClientDefaultController controller) : base(controller) { }
        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<CommandClientReturnValue<T>> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the command call, return an error message before the result is returned (Only for supporting await)
        /// 获取命令调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<T> GetResult()
        {
            if (ReturnType == CommandClientReturnTypeEnum.Success) return ReturnValue;
            return new CommandClientReturnValue<T>(ReturnType, ErrorMessage);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Get the command return value
        /// 获取命令返回值
        /// </summary>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandReturnValue<T> GetValue(bool isIgnoreError = false)
        {
            return new CommandReturnValue<T>(this, isIgnoreError);
        }

        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        /// <param name="returnValue"></param>
#if NetStandard21
        internal void SetReturn(T? returnValue)
#else
        internal void SetReturn(T returnValue)
#endif
        {
            ReturnType = CommandClientReturnTypeEnum.Success;
            this.ReturnValue = returnValue;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                Callback(continuation);
            }
        }
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        /// <param name="returnValue"></param>
#if NetStandard21
        internal void SetReturnQueue(T? returnValue)
#else
        internal void SetReturnQueue(T returnValue)
#endif
        {
            ReturnType = CommandClientReturnTypeEnum.Success;
            this.ReturnValue = returnValue;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                Controller.AppendQueue(Method, continuation);
            }
        }

        /// <summary>
        /// Convert to a Task object
        /// 转换为 Task 对象
        /// </summary>
        /// <param name="returnCommand"></param>
        /// <returns></returns>
        public static async Task<T> GetTask(ReturnCommand<T> returnCommand)
        {
            CommandClientReturnValue<T> value = await returnCommand;
            if (value.IsSuccess) return value.Value;
            if (string.IsNullOrEmpty(value.ErrorMessage)) throw new Exception(value.ReturnType.ToString());
            throw new Exception($"{value.ReturnType} {value.ErrorMessage}");
        }
#if AOT
        /// <summary>
        /// AOT code generation template
        /// AOT 代码生成模板
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        internal static CommandClientReturnValue GetTask(CommandClientReturnValue returnValue)
        {
            return returnValue;
        }
#endif
    }
}
