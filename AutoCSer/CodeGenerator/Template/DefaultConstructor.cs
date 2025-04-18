using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class DefaultConstructor : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static @CurrentType.FullName @DefaultConstructorMethodName()
            {
                return new @CurrentType.FullName();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void @DefaultConstructorReflectionMethodName()
            {
                @DefaultConstructorMethodName();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<@CurrentType.FullName>();
            }
            #endregion PART CLASS
        }
    }
}
