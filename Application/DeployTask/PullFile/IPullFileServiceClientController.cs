﻿using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 拉取文件客户端接口
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IPullFileService))]
#endif
    public partial interface IPullFileServiceClientController
    {
        /// <summary>
        /// 获取指定文件数据
        /// </summary>
        /// <param name="returnValue">The interface returns the initial value, which is used for custom deserialization operations of the return value. The parameter name must be ReturnValue, case-insensitive, and must be placed before the first data parameter. The type must be consistent with the type of the return value
        /// 接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>文件数据</returns>
#if NetStandard21
        EnumeratorCommand<PullFileBuffer?> GetFileData(PullFileBuffer returnValue, SynchronousFileInfo fileInfo);
#else
        EnumeratorCommand<PullFileBuffer> GetFileData(PullFileBuffer returnValue, SynchronousFileInfo fileInfo);
#endif
    }
}
