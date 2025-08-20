using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 命令控制器查询输出数据
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize(IsSerialize = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct CommandControllerOutputData
    {
        /// <summary>
        /// 命令控制器名称
        /// </summary>
        public string ControllerName;
        /// <summary>
        /// 命令控制器序号
        /// </summary>
        public int ControllerIndex;
        ///// <summary>
        ///// 命令数量
        ///// </summary>
        //public int MethodCount;
        /// <summary>
        /// 方法名称集合
        /// </summary>
#if NetStandard21
        public string?[]? MethodNames;
#else
        public string[] MethodNames;
#endif
#if !AOT
        /// <summary>
        /// 命令控制器查询输出数据
        /// </summary>
        /// <param name="controllerIndex"></param>
        /// <param name="controller"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int controllerIndex, CommandServerController controller)
        {
            ControllerName = controller.ControllerName;
            ControllerIndex = controllerIndex;
            //MethodCount = controller.Methods.Length;
            if (controller.Methods.Length != 0 && controller.Server.Config.IsOutputControllerMethodName)
            {
                MethodNames = controller.Methods.getArray(p => p?.Method.Name);
            }
        }
#endif
    }
}
