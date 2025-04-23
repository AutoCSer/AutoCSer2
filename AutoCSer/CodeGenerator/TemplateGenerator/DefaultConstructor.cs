using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 默认构造函数
    /// </summary>
    [Generator(Name = "默认构造函数", IsAuto = true)]
    internal partial class DefaultConstructor : AttributeGenerator<AutoCSer.CodeGenerator.DefaultConstructorAttribute>
    {
        /// <summary>
        /// 默认构造函数方法名称
        /// </summary>
        public string DefaultConstructorMethodName { get { return DefaultConstructorAttribute.DefaultConstructorMethodName; } }
        /// <summary>
        /// 默认构造函数方法名称
        /// </summary>
        public string DefaultConstructorReflectionMethodName { get { return DefaultConstructorAttribute.DefaultConstructorMethodName + "Reflection"; } }

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type;
            if (type.IsValueType || type.IsAbstract || type.IsGenericTypeDefinition) return AutoCSer.Common.CompletedTask;
            if (type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyArray<Type>.Array, null) == null) return AutoCSer.Common.CompletedTask;
            create(true);
            AotMethod.Append(CurrentType, DefaultConstructorReflectionMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
