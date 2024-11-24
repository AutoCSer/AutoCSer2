using System;

namespace AutoCSer.Document.ServiceDataSerialize
{
    /// <summary>
    /// 强制 API 交互数据采用 JSON 序列化操作
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(IsSimpleSerializeInputParameter = false, IsSimpleSerializeOutputParameter = false, IsJsonSerialize = true)]
    public interface IJsonSerializeService
    {
        /// <summary>
        /// 示例 API 定义
        /// </summary>
        /// <param name="left">输入参数</param>
        /// <param name="right">输入参数 + 输出参数</param>
        /// <param name="sum">输出参数</param>
        /// <returns>输出参数</returns>
        bool Add(int left, ref int right, out int sum);
    }
}
