using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 项目配置
    /// </summary>
    [AutoCSer.CodeGenerator.Configuration]
    internal sealed partial class Config : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 主配置类型集合（可以指向别的类型，这里方便演示就指向当前类型了）
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// 二进制序列化自定义全局配置
        /// </summary>
        [AutoCSer.Configuration.Member]
        public static AutoCSer.BinarySerialize.CustomConfig BinarySerializeCustomConfig { get { return new BinarySerializeCustomConfig(); } }
    }
}
