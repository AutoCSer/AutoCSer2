using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口注册服务
    /// </summary>
    public class PortRegistryService : IPortRegistryService
    {
        /// <summary>
        /// 端口信息集合
        /// </summary>
        private readonly PortRegistryData[] portArray;
        /// <summary>
        /// 当前分配端口信息位置
        /// </summary>
        private int portIndex;
        /// <summary>
        /// 起始端口号
        /// </summary>
        private readonly ushort startPort;
        /// <summary>
        /// 端口注册服务
        /// </summary>
        /// <param name="startPort">起始端口号</param>
        /// <param name="endPort">0 表示不限制</param>
        public PortRegistryService(ushort startPort, ushort endPort)
        {
            this.startPort = startPort;
            portArray = new PortRegistryData[(endPort == 0 ? (int)ushort.MaxValue + 1 : endPort) - startPort];
            portIndex = AutoCSer.Random.Default.NextUShort() % portArray.Length;
        }
        /// <summary>
        /// 获取一个空闲端口标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public PortIdentity GetPort(CommandServerSocket socket, CommandServerCallQueue queue)
        {
            int startIndex = portIndex;
            PortIdentity portIdentity = default(PortIdentity);
            do
            {
                if (get(ref portIdentity)) return portIdentity;
            }
            while (portIndex != 0);
            while (portIndex != startIndex)
            {
                if (get(ref portIdentity)) return portIdentity;
            }
            return portIdentity;
        }
        /// <summary>
        /// 获取一个空闲端口标识
        /// </summary>
        /// <param name="portIdentity"></param>
        /// <returns></returns>
        private bool get(ref PortIdentity portIdentity)
        {
            int port = portIndex + startPort;
            uint identity = portArray[portIndex].GetIdentity();
            if (++portIndex == portArray.Length) portIndex = 0;
            if (identity == 0) return false;
            portIdentity.Set(identity, port);
            return true;
        }
        /// <summary>
        /// 断线重连设置端口标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="portIdentity"></param>
        /// <returns></returns>
        public PortIdentity SetPort(CommandServerSocket socket, CommandServerCallQueue queue, PortIdentity portIdentity)
        {
            if (portIdentity.Ticks == AutoCSer.Date.StartTime.Ticks)
            {
                if (portArray[portIdentity.Port - startPort].Check(portIdentity.Identity)) return portIdentity;
            }
            else if (portArray[portIdentity.Port - startPort].Set(portIdentity.Identity))
            {
                portIdentity.Ticks = AutoCSer.Date.StartTime.Ticks;
                return portIdentity;
            }
            return default(PortIdentity);
        }
        /// <summary>
        /// 设置端口标识在线检查回调委托
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="portIdentity"></param>
        /// <param name="callback">端口标识在线检查回调委托</param>
        public void SetCallback(CommandServerSocket socket, CommandServerCallQueue queue, PortIdentity portIdentity, CommandServerKeepCallback callback)
        {
            try
            {
                if (portIdentity.Ticks == AutoCSer.Date.StartTime.Ticks) portArray[portIdentity.Port - startPort].Set(portIdentity.Identity, ref callback);
            }
            finally
            {
                callback?.CancelKeep();
            }
        }
        /// <summary>
        /// 释放端口标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="portIdentity"></param>
        /// <returns></returns>
        public CommandServerSendOnly FreePort(CommandServerSocket socket, CommandServerCallQueue queue, PortIdentity portIdentity)
        {
            if (portIdentity.Ticks == AutoCSer.Date.StartTime.Ticks) portArray[portIdentity.Port - startPort].Free(portIdentity.Identity);
            return null;
        }
    }
}
