using AutoCSer.Extensions;
using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class Configuration : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// AOT code generation call activation reflection
            /// AOT 代码生成调用激活反射
            /// </summary>
            internal static void @ConfigurationMethodName()
            {
                #region LOOP CreateTypes
                AutoCSer.ConfigObject.Create<@FullName>(null);
                #endregion LOOP CreateTypes
                #region LOOP CreateTaskTypes
                AutoCSer.Extensions.TaskExtension.NotWait(AutoCSer.ConfigObject.CreateTask<@FullName>(null));
                #endregion LOOP CreateTaskTypes
                #region LOOP GetTaskTypes
                AutoCSer.Extensions.TaskExtension.NotWait(AutoCSer.ConfigObject.GetTask<@FullName>(null));
                #endregion LOOP GetTaskTypes
                AutoCSer.AotReflection.All(typeof(@CurrentType.FullName));
            }
            #endregion PART CLASS
        }
    }
}
