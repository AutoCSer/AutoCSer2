﻿using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Synchronous waiting command
    /// 同步等待命令
    /// </summary>
    internal class SynchronousCommand : Command
    {
        /// <summary>
        /// 命令等待锁
        /// </summary>
        internal readonly System.Threading.ManualResetEvent WaitLock;
        //internal AutoCSer.Threading.OnceAutoWaitHandle WaitLock;
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal CommandClientReturnTypeEnum ReturnType;
        //{
        //    get { return (CommandClientReturnTypeEnum)(byte)WaitLock.Reserved; }
        //    set { WaitLock.Reserved = (byte)value; }
        //}
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        private string? errorMessage;
#else
        private string errorMessage;
#endif
        /// <summary>
        /// Return value
        /// </summary>
        internal CommandClientReturnValue ReturnValue { get { return new CommandClientReturnValue(ReturnType, errorMessage); } }
        /// <summary>
        /// Synchronous waiting command
        /// 同步等待命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal SynchronousCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex)
        {
            WaitLock = new System.Threading.ManualResetEvent(false);
            //WaitLock.Set(this);
        }
        /// <summary>
        /// Generate the input data of the request command
        /// 生成请求命令输入数据
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <returns>The next request command
        /// 下一个请求命令</returns>
#if NetStandard21
        internal unsafe override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal unsafe override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            UnmanagedStream stream = Controller.Socket.OutputSerializer.Stream;
            if (stream.Data.Pointer.FreeSize >= sizeof(uint) + sizeof(CallbackIdentity) || buildInfo.Count == 0)
            {
                uint methodIndex = Controller.GetMethodIndex(Method.MethodIndex);
                var nextCommand = LinkNext;
                if (methodIndex != 0)
                {
                    SetTimeoutSeconds();
                    uint identity;
                    int callbackIndex = Controller.Socket.CommandPool.Push(this, out identity);
                    if (callbackIndex != 0)
                    {
                        byte* data = stream.GetBeforeMove(sizeof(uint) + sizeof(CallbackIdentity));
                        *(uint*)data = methodIndex | (uint)CommandFlagsEnum.Callback;
                        *(CallbackIdentity*)(data + sizeof(uint)) = new CallbackIdentity((uint)callbackIndex, identity);
                        buildInfo.SetIsCallback();
                        LinkNext = null;
                        return nextCommand;
                    }
                    ReturnType = CommandClientReturnTypeEnum.ClientBuildError;
                }
                else ReturnType = CommandClientReturnTypeEnum.ControllerMethodIndexError;
                ++buildInfo.FreeCount;
                WaitLock.setDispose();
                LinkNext = null;
                return nextCommand;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
        /// <summary>
        /// Error handling for generating the input data of the request command
        /// 生成请求命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            ReturnType = returnType;
            WaitLock.setDispose();
        }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            ReturnType = (CommandClientReturnTypeEnum)(byte)data.Start;
            this.errorMessage = Controller.Socket.ReceiveErrorMessage;
            WaitLock.setDispose();
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// 等待返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CommandClientReturnValue Wait()
        {
            if (Controller.Socket.TryPush(this) != CommandPushStateEnum.Closed)
            {
                WaitLock.WaitOne();
                return ReturnValue;
            }
            return CommandClientReturnTypeEnum.SocketClosed;
        }
        /// <summary>
        /// The command waiting for idle output attempts to be added to the output queue again
        /// 等待空闲输出的命令再次尝试添加到输出队列
        /// </summary>
        /// <returns>Is it necessary to keep waiting
        /// 是否需要继续等待</returns>
        internal override bool CheckWaitPush()
        {
            switch (Controller.Socket.TryPush(this))
            {
                case CommandPushStateEnum.WaitCount: return true;
                case CommandPushStateEnum.Closed:
                    ReturnType = CommandClientReturnTypeEnum.SocketClosed;
                    WaitLock.setDispose();
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal ClientReceiveErrorTypeEnum OnReceive<T>(ref SubArray<byte> data, ref T outputParameter)
            where T : struct
        {
            if (data.Length == int.MinValue)
            {
                ReturnType = (CommandClientReturnTypeEnum)(byte)data.Start;
                this.errorMessage = Controller.Socket.ReceiveErrorMessage;
                WaitLock.setDispose();
                return ClientReceiveErrorTypeEnum.Success;
            }
            try
            {
                if (Controller.Socket.Deserialize(ref data, ref outputParameter, Method.IsSimpleDeserializeParamter))
                {
                    ReturnType = CommandClientReturnTypeEnum.Success;
                    return ClientReceiveErrorTypeEnum.Success;
                }
                Method.DeserializeError(Controller);
                return ClientReceiveErrorTypeEnum.Success;
            }
            finally
            {
                if (ReturnType != CommandClientReturnTypeEnum.Success)
                {
                    ReturnType = CommandClientReturnTypeEnum.ClientDeserializeError;
                    this.errorMessage = Controller.Socket.ReceiveErrorMessage;
                }
                WaitLock.setDispose();
            }
        }
    }
    /// <summary>
    /// Synchronous waiting command
    /// 同步等待命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SynchronousCommand<T> : SynchronousCommand
        where T : struct
    {
        /// <summary>
        /// Input parameters
        /// </summary>
        private T inputParameter;
        /// <summary>
        /// Synchronous waiting command
        /// 同步等待命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        internal SynchronousCommand(CommandClientController controller, int methodIndex, ref T inputParameter) : base(controller, methodIndex)
        {
            this.inputParameter = inputParameter;
        }
        ///// <summary>
        ///// 同步等待命令
        ///// </summary>
        ///// <param name="controller"></param>
        ///// <param name="methodIndex"></param>
        ///// <param name="inputParameter"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void Set(CommandClientController controller, int methodIndex, ref T inputParameter)
        //{
        //    Controller = controller;
        //    MethodIndex = methodIndex;
        //    this.inputParameter = inputParameter;
        //}
        /// <summary>
        /// Generate the input data of the request command
        /// 生成请求命令输入数据
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <returns>The next request command
        /// 下一个请求命令</returns>
#if NetStandard21
        internal override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            return Build(ref buildInfo, ref inputParameter);
        }
        /// <summary>
        /// 等待返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal new CommandClientReturnValue Wait()
        {
            if (Controller.Socket.TryPush(this) != CommandPushStateEnum.Closed)
            {
                WaitLock.WaitOne();
                return ReturnValue;
            }
            return CommandClientReturnTypeEnum.SocketClosed;
        }
    }
}
