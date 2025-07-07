using System;

namespace AutoCSer
{
    /// <summary>
    /// AOT code generation call activation reflection
    /// AOT 代码生成调用激活反射
    /// </summary>
    public static class AotReflection
    {
        /// <summary>
        /// Non-public methods support reflection
        /// 非公共方法支持反射
        /// </summary>
        /// <param name="type"></param>
        public static void NonPublicMethods([System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] Type type)
        {
        }
        /// <summary>
        /// Parameterless constructors and non-public methods support reflection
        /// 无参构造方法与非公共方法支持反射
        /// </summary>
        /// <param name="type"></param>
        public static void ConstructorNonPublicMethods([System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] Type type)
        {
        }
        /// <summary>
        /// Non-public fields support reflection
        /// 非公共字段支持反射
        /// </summary>
        /// <param name="type"></param>
        public static void NonPublicFields([System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicFields)] Type type)
        {
        }
        /// <summary>
        /// Public fields support reflection
        /// 公共字段支持反射
        /// </summary>
        /// <param name="type"></param>
        public static void PublicFields([System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicFields)] Type type)
        {
        }
        /// <summary>
        /// All properties support reflection
        /// 所有属性支持反射
        /// </summary>
        /// <param name="type"></param>
        public static void Properties([System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties)] Type type)
        {
        }
        /// <summary>
        /// All of the fields and properties support reflection
        /// 所有字段与属性支持反射
        /// </summary>
        /// <param name="type"></param>
        public static void FieldsAndProperties([System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicFields | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicFields | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties)]Type type)
        {
        }
        /// <summary>
        /// Interfaces methods support reflection
        /// 接口方法支持反射
        /// </summary>
        /// <param name="type"></param>
        public static void Interfaces([System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.Interfaces | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicMethods | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] Type type)
        {
        }
        /// <summary>
        /// All members support reflection
        /// 所有成员支持反射
        /// </summary>
        /// <param name="type"></param>
        public static void All([System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)] Type type)
        {
        }
        static AotReflection()
        {
            NonPublicMethods(typeof(ConfigObject));
        }
    }
}
