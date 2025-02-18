using AutoCSer.Extensions;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 实时调用标识
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CallIdentity : IEquatable<CallIdentity>
    {
        /// <summary>
        /// 实时调用监视标识
        /// </summary>
        public readonly long MonitorIdentity;
        /// <summary>
        /// 调用标识
        /// </summary>
        public readonly long Identity;
        /// <summary>
        /// 实时调用标识
        /// </summary>
        /// <param name="monitorIdentity">实时调用监视标识</param>
        /// <param name="callIdentity">调用标识</param>
        internal CallIdentity(long monitorIdentity, long callIdentity)
        {
            MonitorIdentity = monitorIdentity;
            Identity = callIdentity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CallIdentity other)
        {
            return Identity == other.Identity && MonitorIdentity == other.MonitorIdentity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<CallIdentity>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (MonitorIdentity ^ Identity).GetHashCode();
        }
    }
}
