using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 节点方法序号映射枚举类型配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class ServerNodeMethodIndexAttribute : Attribute
    {
        /// <summary>
        /// 方法序号映射枚举类型
        /// </summary>
#if NetStandard21
        public readonly Type? MethodIndexEnumType;
#else
        public readonly Type MethodIndexEnumType;
#endif
        /// <summary>
        /// 节点方法序号映射枚举类型配置
        /// </summary>
        /// <param name="methodIndexEnumType">方法序号映射枚举类型</param>
#if NetStandard21
        public ServerNodeMethodIndexAttribute(Type? methodIndexEnumType = null)
#else
        public ServerNodeMethodIndexAttribute(Type methodIndexEnumType = null)
#endif
        {
            MethodIndexEnumType = methodIndexEnumType;
        }
    }
}
