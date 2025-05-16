using System;
using System.Threading.Tasks;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 配置创建
    /// </summary>
    internal abstract class Creator
    {
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal abstract ConfigObject? Create();
#else
        internal abstract ConfigObject Create();
#endif
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal virtual Task<ConfigObject?> CreateAsync()
#else
        internal virtual Task<ConfigObject> CreateAsync()
#endif
        {
            return AutoCSer.Common.GetCompletedTask(Create());
        }
    }
}
