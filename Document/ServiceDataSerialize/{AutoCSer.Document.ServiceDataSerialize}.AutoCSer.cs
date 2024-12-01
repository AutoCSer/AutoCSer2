//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Document.ServiceDataSerialize
{
        /// <summary>
        /// 强制 API 交互数据采用 JSON 序列化操作 客户端接口
        /// </summary>
        public partial interface IJsonSerializeServiceClientController
        {
            /// <summary>
            /// 示例 API 定义
            /// </summary>
            /// <param name="left">输入参数</param>
            /// <param name="right">输入参数 + 输出参数</param>
            /// <param name="sum">输出参数</param>
            /// <returns>输出参数</returns>
            AutoCSer.Net.CommandClientReturnValue<bool> Add(int left, ref int right, out int sum);
        }
}
#endif