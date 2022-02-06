using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 项目配置
    /// </summary>
    internal sealed class Config : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 主配置类型集合（可以指向别的类型，这里方便演示就指向当前类型了）
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// JSON 序列化配置参数
        /// </summary>
        [AutoCSer.Configuration.Member]
        public static AutoCSer.JsonSerializeConfig JsonSerializeConfig { get { return new JsonSerializeConfig { CheckLoop = false }; } }
    }
}
