using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图自定义属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewAttribute : Attribute
    {
        /// <summary>
        /// 默认为 true 表示生成帮助文档视图数据信息
        /// </summary>
        public bool IsHelp = true;
        /// <summary>
        /// 默认为 false 表示每一个请求使用不同实例，设置为 true 则所有请求使用同一个单例实例（单例实例仅适合无参静态数据视图）
        /// </summary>
        public bool IsSingleton;
        /// <summary>
        /// 默认为 false 表示不缓存返回数据，设置为 true 则根据静态版本信息检查结果缓存返回数据
        /// </summary>
        public bool IsStaticVersion;

        /// <summary>
        /// 默认数据视图自定义属性
        /// </summary>
        internal static readonly ViewAttribute Default = new ViewAttribute();
    }
}
