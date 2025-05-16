using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 设置客户端控制器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SetClientControllerDynamicMethod
    {
        /// <summary>
        /// 动态函数
        /// </summary>
        private readonly DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private readonly ILGenerator generator;
        /// <summary>
        /// 设置客户端控制器
        /// </summary>
        /// <param name="type"></param>
        internal SetClientControllerDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SetClientController", null, new Type[] { typeof(CommandClientSocketEvent) }, type, true);
            generator = dynamicMethod.GetILGenerator();
        }
        /// <summary>
        /// 添加客户端控制器属性 this.ClientController = this.GetController("controllerName");
        /// </summary>
        /// <param name="property"></param>
        /// <param name="controllerName"></param>
        internal void Push(PropertyInfo property, string controllerName)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_0);
            generator.ldstr(controllerName);
            generator.call(commandClientSocketEventGetControllerMethodInfo.Method);
            Type propertyType = property.PropertyType;
            if (propertyType.IsInterface) generator.Emit(OpCodes.Isinst, propertyType);
            generator.call(property.GetSetMethod(true).notNull());
        }
        /// <summary>
        /// 创建web表单委托
        /// </summary>
        /// <param name="type">委托类型</param>
        /// <returns>web表单委托</returns>
        internal Delegate Create(Type type)
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(type);
        }

        /// <summary>
        /// 获取命令客户端控制器方法信息
        /// </summary>
#if NetStandard21
        private static readonly Func<CommandClientSocketEvent, string, CommandClientController?> commandClientSocketEventGetControllerMethodInfo = CommandClientSocketEvent.GetController;
#else
        private static readonly Func<CommandClientSocketEvent, string, CommandClientController> commandClientSocketEventGetControllerMethodInfo = CommandClientSocketEvent.GetController;
#endif
    }
}
