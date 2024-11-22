using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 接口任务队列节点匹配方法
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct TaskQueueInterfaceControllerMatchMethod : IEquatable<TaskQueueInterfaceControllerMatchMethod>
    {
        /// <summary>
        /// 接口任务队列节点方法
        /// </summary>
        internal readonly MethodInfo Method;
        /// <summary>
        /// 接口任务队列节点方法参数集合
        /// </summary>
        internal readonly ParameterInfo[] Parameters;
        /// <summary>
        /// 接口任务队列节点匹配方法
        /// </summary>
        /// <param name="method"></param>
        internal TaskQueueInterfaceControllerMatchMethod(MethodInfo method)
        {
            this.Method = method;
            Parameters = method.GetParameters();
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TaskQueueInterfaceControllerMatchMethod other)
        {
            if (Parameters.Length == other.Parameters.Length && Method.Name == other.Method.Name)
            {
                int index = 0;
                foreach (ParameterInfo parameter in other.Parameters)
                {
                    if (parameter.ParameterType != Parameters[index++].ParameterType) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
            public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<TaskQueueInterfaceControllerMatchMethod>());
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = Method.Name.GetHashCode() ^ Parameters.Length;
            foreach (ParameterInfo parameter in Parameters) hashCode ^= parameter.ParameterType.GetHashCode();
            return hashCode;
        }
    }
}
