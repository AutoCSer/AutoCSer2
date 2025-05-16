using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令控制器接口配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandServerControllerAttribute : Attribute
    {
        /// <summary>
        /// 服务接口类型
        /// </summary>
#if NetStandard21
        public Type? InterfaceType;
#else
        public Type InterfaceType;
#endif
    }
}
