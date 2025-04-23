//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Document.SymmetryService
{
        /// <summary>
        /// Interface symmetry service definition 
///            接口对称服务定义 客户端接口
        /// </summary>
        public partial interface ISymmetryServiceClientController
        {
            /// <summary>
            /// Synchronization API definition (It is not recommended to define synchronization apis in interface symmetric services because the client synchronization blocking mode may cause performance bottlenecks) 
///            同步 API 定义（不建议在接口对称服务中定义同步 API，因为客户端同步阻塞模式可能造成性能瓶颈）
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> Add(int left, int right);
            /// <summary>
            /// Asynchronous API definition 
///            异步 API 定义
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AddAsync(int left, int right);
        }
}
#endif