using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 是否单例实例
    /// </summary>
    public enum JsonApiSingletonEnum : byte
    {
        /// <summary>
        /// 由控制器配置 AutoCSer.NetCoreWeb.JsonApiControllerAttribute.IsSingleton 决定
        /// </summary>
        Controller,
        /// <summary>
        /// 每一个请求使用不同实例
        /// </summary>
        New,
        /// <summary>
        /// 单例模式
        /// </summary>
        Singleton,
    }
}
