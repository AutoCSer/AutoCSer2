using System;
using System.Collections.Generic;

namespace AutoCSer.Example.CommandServer
{
    /// <summary>
    /// 项目配置
    /// </summary>
    internal sealed class Config :  AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 公共配置类型集合
        /// </summary>
        public override IEnumerable<Type> PublicTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// 二进制序列化默认支持属性（匿名字段）配置
        /// </summary>
        [AutoCSer.Configuration.Member]
        internal static AutoCSer.BinarySerializeAttribute BinarySerializeAttribute
        {
            get { return new AutoCSer.BinarySerializeAttribute { IsAnonymousFields = true, IsBaseType = false, IsMemberMap = false }; }
        }
    }
}
