using System;

namespace AutoCSer.TestCase.DiskBlockClient
{
    /// <summary>
    /// 比较数据代码生成
    /// </summary>
    [AutoCSer.CodeGenerator.FieldEquals]
    internal partial struct ValueEquals
    {
        /// <summary>
        /// 字节数组
        /// </summary>
        private byte[] array;
        /// <summary>
        /// 磁盘块索引信息
        /// </summary>
        private AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex;
    }
}
