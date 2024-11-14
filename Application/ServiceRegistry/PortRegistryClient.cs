using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口注册客户端
    /// </summary>
    public sealed class PortRegistryClient
    {
        /// <summary>
        /// 端口注册客户端套接字事件
        /// </summary>
        private readonly IPortRegistryClientSocketEvent socketEvent;
        /// <summary>
        /// 注册数据访问锁
        /// </summary>
        private readonly object dataLock;
        /// <summary>
        /// 注册数据集合
        /// </summary>
        private LeftArray<PortRegistryClientData> dataArray;
        /// <summary>
        /// 端口注册客户端
        /// </summary>
        /// <param name="socketEvent"></param>
        public PortRegistryClient(IPortRegistryClientSocketEvent socketEvent)
        {
            this.socketEvent = socketEvent;
            dataLock = new object();
            dataArray = new LeftArray<PortRegistryClientData>(0);
        }
        /// <summary>
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <returns></returns>
        public void OnClientMethodVerified()
        {
            if (dataArray.Length != 0) onClientMethodVerified().NotWait();
        }
        /// <summary>
        /// 当前套接字通过验证方法
        /// </summary>
        /// <returns></returns>
        private async Task onClientMethodVerified()
        {
            LeftArray<PortRegistryClientData> dataArray = default(LeftArray<PortRegistryClientData>);
            System.Threading.Monitor.Enter(dataLock);
            int count = dataArray.Length;
            try
            {
                if (count == 0) return;
                dataArray = this.dataArray;
                this.dataArray = new LeftArray<PortRegistryClientData>(dataArray.Length);
            }
            finally { System.Threading.Monitor.Exit(dataLock); }

            foreach (PortRegistryClientData data in dataArray.Array)
            {
                bool isDisposeKeepCallback = true;
                try
                {
                    PortIdentity portIdentity = data.GetPortIdentity();
                    if (portIdentity.Identity != 0)
                    {
                        CommandClientReturnValue<PortIdentity> setPortIdentity = await socketEvent.IPortRegistryClient.SetPort(portIdentity);
                        if (setPortIdentity.IsSuccess)
                        {
                            portIdentity = setPortIdentity.Value;
                            if (portIdentity.Identity != 0) await SetCallback(data.CommandListener, portIdentity);
                        }
                        else
                        {
                            System.Threading.Monitor.Enter(dataLock);
                            if (this.dataArray.IsFree)
                            {
                                this.dataArray.Add(data);
                                System.Threading.Monitor.Exit(dataLock);
                                isDisposeKeepCallback = false;
                            }
                            else
                            {
                                try
                                {
                                    this.dataArray.Add(data);
                                    isDisposeKeepCallback = false;
                                }
                                finally { System.Threading.Monitor.Exit(dataLock); }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    await AutoCSer.LogHelper.Exception(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                finally
                {
                    if (isDisposeKeepCallback) data.DisposeKeepCallback();
                }
                if (--count == 0) return;
            }
        }
        /// <summary>
        /// 设置端口标识在线检查回调委托
        /// </summary>
        /// <param name="commandListener"></param>
        /// <param name="portIdentity"></param>
        /// <returns></returns>
        internal async Task<bool> SetCallback(CommandListenerBase commandListener, PortIdentity portIdentity)
        {
            var commandKeepCallback = await socketEvent.IPortRegistryClient.SetCallback(portIdentity, CommandClientKeepCallback.EmptyCallback);
            if (commandKeepCallback == null) return false;
            PortRegistryClientData data = new PortRegistryClientData(commandListener, portIdentity, commandKeepCallback);
            bool isAdd = false;
            System.Threading.Monitor.Enter(dataLock);
            if (dataArray.IsFree)
            {
                dataArray.Add(data);
                System.Threading.Monitor.Exit(dataLock);
                return true;
            }
            try
            {
                dataArray.Add(data);
                isAdd = true;
            }
            finally
            {
                System.Threading.Monitor.Exit(dataLock);
                if (!isAdd) data.DisposeKeepCallback();
            }
            return true;
        }
        /// <summary>
        /// 获取端口标识
        /// </summary>
        /// <returns></returns>
        internal async Task<CommandClientReturnValue<PortIdentity>> GetPortIdentity()
        {
            return await socketEvent.IPortRegistryClient.GetPort();
        }
        /// <summary>
        /// 释放端口标识
        /// </summary>
        /// <param name="commandListener"></param>
        internal void Free(CommandListenerBase commandListener)
        {
            PortRegistryClientData freeData = default(PortRegistryClientData);
            System.Threading.Monitor.Enter(dataLock);
            int count = dataArray.Length;
            try
            {
                if (count == 0) return;
                foreach (PortRegistryClientData data in dataArray.Array)
                {
                    if (object.ReferenceEquals(data.CommandListener, commandListener))
                    {
                        freeData = data;
                        dataArray.Array[dataArray.Length - count] = dataArray.Array[dataArray.Length - 1];
                        --dataArray.Length;
                        break;
                    }
                    --count;
                }
            }
            finally { System.Threading.Monitor.Exit(dataLock); }
            if (freeData.CommandListener != null) socketEvent.IPortRegistryClient.FreePort(freeData.Free());
        }
    }
}
