using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 返回值命令 await CommandClientReturnValue
    /// </summary>
    public abstract class ReturnCommand : BaseReturnCommand
    {
        /// <summary>
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
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
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<CommandClientReturnValue> Wait()
        {
            return await this;
        }
        /// <summary>
        /// 获取命令调用结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue GetResult()
        {
            return new CommandClientReturnValue(ReturnType, ErrorMessage);
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// 获取 Task
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
    /// 返回值命令 await CommandClientReturnValue{T}
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReturnCommand<T> : BaseReturnCommand
    {
        /// <summary>
        /// 返回值
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal T ReturnValue;
        /// <summary>
        /// 默认空命令
        /// </summary>
        /// <param name="controller"></param>
        internal ReturnCommand(CommandClientController controller) : base(controller) { }
        /// <summary>
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
        /// 返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<CommandClientReturnValue<T>> Wait()
        {
            return await this;
        }
        /// <summary>
        /// 获取命令调用结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<T> GetResult()
        {
            if (ReturnType == CommandClientReturnTypeEnum.Success) return ReturnValue;
            return new CommandClientReturnValue<T>(ReturnType, ErrorMessage);
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<T> GetAwaiter()
        {
            return this;
        }

        /// <summary>
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
        /// 获取 Task
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
        /// 代码生成模板
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
