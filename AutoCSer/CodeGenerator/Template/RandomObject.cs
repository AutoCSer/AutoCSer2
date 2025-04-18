using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class RandomObject : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="value"></param>
            /// <param name="config"></param>
            internal static void @CreateRandomObjectMethodName(ref @CurrentType.FullName value, AutoCSer.RandomObject.Config config)
            {
                value.createRandomObject(config);
                #region IF BaseType
                @BaseType.FullName baseValue = value;
                AutoCSer.RandomObject.Creator.CreateBase<@BaseType.FullName>(ref baseValue, config);
                #endregion IF BaseType
            }
            /// <summary>
            /// 随机对象生成
            /// </summary>
            /// <param name="config"></param>
            private void createRandomObject(AutoCSer.RandomObject.Config __config__)
            {
                #region LOOP Fields
                @MemberName = AutoCSer.RandomObject.Creator.Create<@MemberType.FullName>(__config__, @IsNullable);
                #endregion LOOP Fields
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            #region IF CurrentType.Type.IsGenericType
            public static void @CreateRandomObjectMethodName(/*NOTE*/object value/*NOTE*/)/*NOTE*/ { }/*NOTE*/
            #endregion IF CurrentType.Type.IsGenericType
            #region NOT CurrentType.Type.IsGenericType
            internal static void @CreateRandomObjectMethodName()
            #endregion NOT CurrentType.Type.IsGenericType
            {
                var value = AutoCSer.RandomObject.Creator.CallCreate<@CurrentType.FullName>();
                @CreateRandomObjectMethodName(ref value, null);
            }
            #endregion PART CLASS
            private string MemberName;
            public bool IsNullable = false;
        }
    }
}
