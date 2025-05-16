using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// Task 函数配置创建
    /// </summary>
    internal sealed class MethodObjectCreator : Creator
    {
        /// <summary>
        /// 目标函数
        /// </summary>
        private readonly MethodInfo method;
        /// <summary>
        /// 配置数据类型
        /// </summary>
        private readonly Type type;
        /// <summary>
        /// Task 函配置创建
        /// </summary>
        /// <param name="method">目标函数</param>
        /// <param name="type">配置数据类型</param>
        internal MethodObjectCreator(MethodInfo method, Type type)
        {
            this.method = method;
            this.type = type;
        }
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override ConfigObject? Create()
#else
        internal override ConfigObject Create()
#endif
        {
            return CreateAsync().getResult();
        }
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Task<ConfigObject?> CreateAsync()
#else
        internal override Task<ConfigObject> CreateAsync()
#endif
        {
            var task = method.Invoke(null, null);
            if (task != null)
            {
#if AOT
                return ConfigObject.CreateTaskMethod.MakeGenericMethod(type).Invoke(null, new object[] { task }).notNullCastType<Task<ConfigObject?>>();
#else
                return AutoCSer.Metadata.ClassGenericType.Get(type).CreateConfigObjectTask(task);
#endif
            }
            return CompletedTask<ConfigObject>.Default;
        }
    }
}
