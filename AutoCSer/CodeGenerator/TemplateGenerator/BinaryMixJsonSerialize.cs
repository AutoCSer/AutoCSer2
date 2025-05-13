using AutoCSer.BinarySerialize;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 二进制混杂 JSON 序列化
    /// </summary>
    [Generator(Name = "二进制混杂 JSON 序列化", IsAuto = true, IsTemplate = false)]
    internal partial class BinaryMixJsonSerialize : AttributeGenerator<AutoCSer.BinarySerializeAttribute>
    {
        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            if (CurrentAttribute.IsCodeGenerator)
            {
                BinarySerialize.Create(CurrentType.Type);
                if (CurrentAttribute.IsMixJsonSerialize) JsonSerialize.Create(CurrentType.Type);
            }
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
