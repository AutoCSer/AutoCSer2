using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// Custom configuration of the data view
    /// 数据视图自定义配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewAttribute : Attribute
    {
        /// <summary>
        /// A default value of true indicates the generation of help document view data information
        /// 默认为 true 表示生成帮助文档视图数据信息
        /// </summary>
        public bool IsHelp = true;
        /// <summary>
        /// By default, false indicates that each request uses a different instance. If set to true, all requests use the same singleton instance (singleton instances are only suitable for static data views without parameters).
        /// 默认为 false 表示每一个请求使用不同实例，设置为 true 则所有请求使用同一个单例实例（单例实例仅适合无参静态数据视图）
        /// </summary>
        public bool IsSingleton;
        /// <summary>
        /// By default, false indicates that the returned data is not cached. If set to true, the returned data will be cached based on the static version information check results
        /// 默认为 false 表示不缓存返回数据，设置为 true 则根据静态版本信息检查结果缓存返回数据
        /// </summary>
        public bool IsStaticVersion;

        /// <summary>
        /// Default data view custom configuration
        /// 默认数据视图自定义配置
        /// </summary>
        internal static readonly ViewAttribute Default = new ViewAttribute();
    }
}
