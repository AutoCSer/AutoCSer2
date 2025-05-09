using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点类型配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class ServerNodeTypeAttribute : Attribute
    {
        /// <summary>
        /// 创建调用方法信息
        /// </summary>
        internal const string MethodParameterCreatorMethodName = "MethodParameterCreator";
        /// <summary>
        /// 获取生成服务端节点方法信息方法名称
        /// </summary>
        internal const string GetServerNodeCreatorMethodName = "GetServerNodeCreatorMethod";

        /// <summary>
        /// 方法序号映射枚举类型
        /// </summary>
#if NetStandard21
        public readonly Type? MethodIndexEnumType;
#else
        public readonly Type MethodIndexEnumType;
#endif
#if AOT
        /// <summary>
        /// 创建调用方法与参数信息类型
        /// </summary>
        public readonly Type? MethodParameterCreatorType;
        /// <summary>
        /// 是否创建调用方法与参数信息
        /// </summary>
        public readonly bool IsMethodParameterCreator;
        /// <summary>
        /// 服务端节点类型配置
        /// </summary>
        /// <param name="methodIndexEnumType">方法序号映射枚举类型</param>
        /// <param name="methodParameterCreatorType">创建调用方法与参数信息类型</param>
        /// <param name="isMethodParameterCreator">是否创建调用方法与参数信息</param>
        public ServerNodeTypeAttribute(Type? methodIndexEnumType = null, Type? methodParameterCreatorType = null, bool isMethodParameterCreator = false)
        {
            MethodIndexEnumType = methodIndexEnumType;
            MethodParameterCreatorType = methodParameterCreatorType;
            IsMethodParameterCreator = isMethodParameterCreator;
        }
#else
        /// <summary>
        /// 服务端节点类型配置
        /// </summary>
        /// <param name="methodIndexEnumType">方法序号映射枚举类型</param>
#if NetStandard21
        public ServerNodeTypeAttribute(Type? methodIndexEnumType = null)
#else
        public ServerNodeTypeAttribute(Type methodIndexEnumType = null)
#endif
        {
            MethodIndexEnumType = methodIndexEnumType;
        }
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <param name="methodIndexEnumType"></param>
        /// <param name="methodParameterCreatorType"></param>
        /// <param name="isMethodParameterCreator"></param>
        internal ServerNodeTypeAttribute(Type methodIndexEnumType, Type methodParameterCreatorType, bool isMethodParameterCreator) { }
#endif
    }
}
