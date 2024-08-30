using AutoCSer.CommandService.FileSynchronous;
using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.FileSynchronous
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
        /// 文件上传服务配置
        /// </summary>
        [AutoCSer.Configuration.Member]
        public static UploadFileConfig UploadFileConfig { get { return new UploadFileConfig { IsSingleService = true }; } }
    }
}
