using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 接口任务队列节点方法
    /// </summary>
    internal sealed class TaskQueueInterfaceControllerMethod : InterfaceMethodBase
    {
        /// <summary>
        /// 接口任务队列节点方法配置
        /// </summary>
        internal readonly InterfaceControllerTaskQueueMethodAttribute MethodAttribute;
        /// <summary>
        /// 服务实例方法
        /// </summary>
        internal readonly MethodInfo ServiceMethod;
        /// <summary>
        /// 接口任务队列节点方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="serviceMethod"></param>
        internal unsafe TaskQueueInterfaceControllerMethod(Type type, TaskQueueInterfaceControllerMatchMethod method, MethodInfo serviceMethod) : base(type, method.Method)
        {
            MethodAttribute = Method.GetCustomAttribute<InterfaceControllerTaskQueueMethodAttribute>(false) ?? InterfaceControllerTaskQueueMethodAttribute.Default;
            Parameters = method.Parameters;
            ParameterEndIndex = Parameters.Length;
            ReturnValueType = Method.ReturnType;
            ServiceMethod = serviceMethod;
            if (serviceMethod.ReturnType == typeof(void))
            {
                if (ReturnValueType != typeof(InterfaceControllerTaskQueueNode))
                {
                    Error = $"接口 {type.fullName()}.{Method.Name} 返回值类型必须为 {typeof(InterfaceControllerTaskQueueNode).fullName()}";
                    return;
                }
            }
            else
            {
                Type returnType = typeof(InterfaceControllerTaskQueueNode<>).MakeGenericType(ServiceMethod.ReturnType);
                if (ReturnValueType != returnType)
                {
                    Error = $"接口 {type.fullName()}.{Method.Name} 返回值类型必须为 {returnType.fullName()}";
                    return;
                }
            }
            for (int parameterIndex = ParameterStartIndex; parameterIndex != ParameterEndIndex; ++parameterIndex)
            {
                if (Parameters[parameterIndex].ParameterType.IsByRef)
                {
                    Error = $"接口 {type.fullName()}.{Method.Name} 不允许下 ref / out 参数 {Parameters[parameterIndex].Name}";
                    return;
                }
            }
        }
        /// <summary>
        /// 获取接口方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serviceMethods"></param>
        /// <param name="methods"></param>
        /// <returns>错误信息</returns>
#if NetStandard21
        internal static string? GetMethod(Type type, Dictionary<TaskQueueInterfaceControllerMatchMethod, MethodInfo> serviceMethods, ref LeftArray<TaskQueueInterfaceControllerMethod> methods)
#else
        internal static string GetMethod(Type type, Dictionary<TaskQueueInterfaceControllerMatchMethod, MethodInfo> serviceMethods, ref LeftArray<TaskQueueInterfaceControllerMethod> methods)
#endif
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                var error = InterfaceController.CheckMethod(type, method);
                if (error != null) return error;
                var serviceMethod = default(MethodInfo);
                TaskQueueInterfaceControllerMatchMethod matchMethod = new TaskQueueInterfaceControllerMatchMethod(method);
                if(!serviceMethods.TryGetValue(matchMethod, out serviceMethod)) return $"{type.fullName()}.{method.Name} 没有找到匹配的目标实例方法";
                TaskQueueInterfaceControllerMethod controllerMethod = new TaskQueueInterfaceControllerMethod(type, matchMethod, serviceMethod);
                if (controllerMethod.Error != null) return controllerMethod.Error;
                methods.Add(controllerMethod);
            }
            return null;
        }
    }
}
