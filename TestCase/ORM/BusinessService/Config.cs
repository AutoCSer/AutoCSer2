using System;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// AutoCSer 项目配置
    /// </summary>
    public class Config : AutoCSer.TestCase.Common.Config
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        [AutoCSer.Configuration.Member]
        internal static AutoCSer.Metadata.DefaultConstructor DefaultConstructor
        {
            get { return BusinessService.DefaultConstructor.Default; }
        }
    }
}
