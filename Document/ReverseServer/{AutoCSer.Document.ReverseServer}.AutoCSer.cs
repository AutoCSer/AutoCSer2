//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Document.ReverseServer
{
        /// <summary>
        /// 接口对称服务定义 客户端接口
        /// </summary>
        public partial interface ISymmetryServiceClientController
        {
            /// <summary>
            /// 同步 API 定义（不建议在接口对称服务中定义同步 API，因为客户端同步阻塞模式可能造成性能瓶颈）
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> Add(int left, int right);
            /// <summary>
            /// 异步 API 定义
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AddAsync(int left, int right);
        }
}
#endif