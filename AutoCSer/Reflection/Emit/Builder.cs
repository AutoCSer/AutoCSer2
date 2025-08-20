using System;
using System.Reflection.Emit;

namespace AutoCSer.Reflection.Emit
{
    /// <summary>
    /// 动态程序集模块
    /// </summary>
    internal static class Module
    {
        /// <summary>
        /// 动态程序集
        /// </summary>
        private static readonly AssemblyBuilder assemblyBuilder;
        /// <summary>
        /// 动态程序集模块
        /// </summary>
        internal static readonly ModuleBuilder Builder;

        static Module()
        {
            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(AutoCSer.Common.NamePrefix + ".DynamicAssembly"), AssemblyBuilderAccess.Run);
            Builder = assemblyBuilder.DefineDynamicModule(AutoCSer.Common.NamePrefix + ".DynamicModule");
        }
    }
}
