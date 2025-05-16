using System;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 申明为配置项
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class MemberAttribute : Attribute
    {
        /// <summary>
        /// 配置名称，默认为 null 表示默认名称，空字符串表示使用定义名称
        /// </summary>
#if NetStandard21
        public string? Name;
#else
        public string Name;
#endif
        /// <summary>
        /// 申明为配置项
        /// </summary>
        /// <param name="name">配置名称，默认为 null 表示默认名称，空字符串表示使用定义名称</param>
#if NetStandard21
        public MemberAttribute(string? name = null)
#else
        public MemberAttribute(string name = null)
#endif
        {
            Name = name;
        }
        /// <summary>
        /// 获取配置缓存名称
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        internal string GetCacheName(string memberName)
        {
            if (Name == null) return string.Empty;
            if (Name.Length == 0) return memberName;
            return Name;
        }
    }
}
