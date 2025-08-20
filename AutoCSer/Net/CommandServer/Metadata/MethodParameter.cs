using AutoCSer.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 调用参数信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MethodParameter
    {
        /// <summary>
        /// 调用参数类型重定向方法名称
        /// </summary>
        internal const string CommandParameterCastTypeMethodName = "AutoCSerCommandParameterCastType";

        /// <summary>
        /// 参数信息
        /// </summary>
        internal readonly ParameterInfo Parameter;
        /// <summary>
        /// 重定向类型
        /// </summary>
#if NetStandard21
        private readonly Type? redirectType;
#else
        private readonly Type redirectType;
#endif
        /// <summary>
        /// 参数类型
        /// </summary>
        internal Type ParameterType { get { return redirectType ?? Parameter.ParameterType; } }
        /// <summary>
        /// 获取参数真实类型
        /// </summary>
        internal Type ElementType { get { return redirectType ?? Parameter.elementType(); } }
        /// <summary>
        /// 调用参数信息
        /// </summary>
        /// <param name="parameter">参数信息</param>
        /// <param name="isCheckRedirectType"></param>
        internal MethodParameter(ParameterInfo parameter, bool isCheckRedirectType)
        {
            Parameter = parameter;
            var castMethod = default(MethodInfo);
            redirectType = isCheckRedirectType ? GetRedirectType(parameter, out castMethod) : null;
        }
        /// <summary>
        /// 客户端获取重定向类型
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="castMethod"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? GetRedirectType(ParameterInfo parameter, out MethodInfo? castMethod)
#else
        internal static Type GetRedirectType(ParameterInfo parameter, out MethodInfo castMethod)
#endif
        {
            Type type = parameter.ParameterType;
            if (!type.IsByRef && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Expression<>))
            {
                var redirectType = parameter.GetCustomAttribute(typeof(MethodParameterTypeAttribute), false).castClass<MethodParameterTypeAttribute>()?.RedirectType;
                if (redirectType != null)
                {
                    castMethod = redirectType.GetMethod(CommandParameterCastTypeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { type }, null);
                    if (castMethod?.ReturnType == redirectType && !castMethod.IsGenericMethodDefinition) return redirectType;
                }
            }
            castMethod = null;
            return null;
        }
        /// <summary>
        /// 服务端获取客户端重定向类型
        /// </summary>
        /// <param name="redirectType"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? GetParameterType(Type redirectType)
#else
        internal static Type GetParameterType(Type redirectType)
#endif
        {
            var castMethod = redirectType.GetMethod(CommandParameterCastTypeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (castMethod?.ReturnType == redirectType && !castMethod.IsGenericMethodDefinition)
            {
                ParameterInfo[] parameters = castMethod.GetParameters();
                if(parameters.Length == 1)
                {
                    Type type = parameters[0].ParameterType;
                    if (!type.IsByRef && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Expression<>)) return type;
                }
            }
            return null;
        }
    }
}
