using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 控制器接口信息
    /// </summary>
    internal static class InterfaceController
    {
        /// <summary>
        /// 检查接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isGenericTypeDefinition"></param>
        /// <param name="error"></param>
        /// <returns></returns>
#if NetStandard21
        internal static bool CheckType(Type type, bool isGenericTypeDefinition, [MaybeNullWhen(true)] out string error)
#else
        internal static bool CheckType(Type type, bool isGenericTypeDefinition, out string error)
#endif
        {
            if (!type.IsInterface)
            {
                error = $"不支持非接口类型 {type.fullName()}";
                return false;
            }
            if (!isGenericTypeDefinition && type.IsGenericTypeDefinition)// && !AutoCSer.Common.IsCodeGenerator
            {
                error = $"不支持泛型接口类型 {type.fullName()}";
                return false;
            }
            foreach (PropertyInfo property in type.GetProperties())
            {
                error = $"不支持属性 {type.fullName()}";
                return false;
            }
            error = null;
            return true;
        }
        /// <summary>
        /// 获取命令控制器配置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isGenericTypeDefinition"></param>
        /// <param name="error"></param>
        /// <returns></returns>
#if NetStandard21
        internal static CommandServerControllerInterfaceAttribute GetCommandControllerAttribute(Type type, bool isGenericTypeDefinition, out string? error)
#else
        internal static CommandServerControllerInterfaceAttribute GetCommandControllerAttribute(Type type, bool isGenericTypeDefinition, out string error)
#endif
        {
            if (CheckType(type, isGenericTypeDefinition, out error))
            {
                return type.GetCustomAttribute<CommandServerControllerInterfaceAttribute>(false) ?? CommandServerController.DefaultAttribute;
            }
            return CommandServerController.DefaultAttribute;
        }
        /// <summary>
        /// 检查方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns>错误信息</returns>
#if NetStandard21
        internal static string? CheckMethod(Type type, MethodInfo method)
#else
        internal static string CheckMethod(Type type, MethodInfo method)
#endif
        {
            return method.IsGenericMethod ? $"不支持泛型函数 {type.fullName()}.{method.Name}" : null;
        }
    }
}
