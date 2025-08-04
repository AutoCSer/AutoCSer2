using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Remote expression serialization status
    /// 远程表达式序列化状态
    /// </summary>
    public enum RemoteExpressionSerializeStateEnum : byte
    {
        /// <summary>
        /// Unknown state
        /// 未知状态
        /// </summary>
        Unknown,
        /// <summary>
        /// Serialization successful
        /// 序列化成功
        /// </summary>
        Success,
        /// <summary>
        /// The client does not support remote expressions
        /// 客户端不支持远程表达式
        /// </summary>
        NotSupportClient,
        /// <summary>
        /// Unknown node type
        /// 未知节点类型
        /// </summary>
        UnknownNodeType,
        /// <summary>
        /// The new operation on nodes is not supported
        /// 不支持 new 操作节点 
        /// </summary>
        NotSupportNew,
        /// <summary>
        /// Lambda nodes are not supported
        /// 不支持 Lambda 节点
        /// </summary>
        NotSupportLambda,
        /// <summary>
        /// Code block nodes are not supported
        /// 不支持代码块节点 Block
        /// </summary>
        NotSupportBlock,
        /// <summary>
        /// Dynamic type nodes are not supported
        /// 不支持动态类型节点 Dynamic
        /// </summary>
        NotSupportDynamic,
        /// <summary>
        /// goto tag nodes are not supported
        /// 不支持 goto 标签节点 Label
        /// </summary>
        NotSupportLabel,
        /// <summary>
        /// Expansion nodes are not supported
        /// 不支持扩展节点 Extension
        /// </summary>
        NotSupportExtension,
        /// <summary>
        /// Debugging information nodes are not supported
        /// 不支持调试信息节点 DebugInfo
        /// </summary>
        NotSupportDebugInfo,
        ///// <summary>
        ///// Conditional expression nodes are not supported
        ///// 不支持条件表达式节点 Conditional
        ///// </summary>
        //NotSupportConditional,
        /// <summary>
        /// Multidimensional array indexing is not supported
        /// 不支持多维数组索引
        /// </summary>
        NotSupportArrayIndex,
        /// <summary>
        /// A null reference exception occurred in the calculation of constant values
        /// 常量值计算发生空引用异常
        /// </summary>
        ConstantNullReference,
        /// <summary>
        /// The number of constant parameters exceeds the maximum value of 65535
        /// 常量参数数量超过最大值 65535
        /// </summary>
        TooManyConstant,
        ///// <summary>
        ///// 不支持的常量值类型
        ///// </summary>
        //NotSupportConstantType,
        /// <summary>
        /// The number of parameters of the generic type exceeds the maximum value of 255
        /// 泛型类型参数数量超过最大值 255
        /// </summary>
        TooManyGenericArguments,
        /// <summary>
        /// The number of method parameters exceeds the maximum value of 255
        /// 方法参数数量超过最大值 255
        /// </summary>
        TooManyParameters,
        /// <summary>
        /// No matching parameter expression was found
        /// 没有找到匹配参数表达式
        /// </summary>
        NotFoundParameter,

        /// <summary>
        /// Server deserialization failed
        /// 服务端反序列化失败
        /// </summary>
        DeserializeFailed,
        /// <summary>
        /// The server deserialization did not find the socket context
        /// 服务端反序列化没有找到套接字上下文
        /// </summary>
        NotFoundCommandServerSocket,
        /// <summary>
        /// The server deserialization did not find the remote metadata information, indicating that the server is missing configuration AutoCSer.Net.CommandServerConfig.IsRemoteExpression
        /// 服务端反序列化没有找到远程元数据信息，说明服务端缺少配置 AutoCSer.Net.CommandServerConfig.IsRemoteExpression
        /// </summary>
        NotFoundRemoteMetadata,
        /// <summary>
        /// The server is unaware of the serialization information
        /// 服务端未知序列化信息
        /// </summary>
        UnknownSerializeInfo,
        /// <summary>
        /// The server formatted data write address is out of range
        /// 服务端格式化数据写入地址超出范围
        /// </summary>
        FormatWriteIndexOutOfRange,
        /// <summary>
        /// The server formatted the data reading address is out of range
        /// 服务端格式化数据读取地址超出范围
        /// </summary>
        FormatReadIndexOutOfRange,
        /// <summary>
        /// The server did not find the remote type information
        /// 服务端没有找到远程类型信息
        /// </summary>
        NotFoundType,
        /// <summary>
        /// The server did not find the remote method information
        /// 服务端没有找到远程方法信息
        /// </summary>
        NotFoundMethod,
        /// <summary>
        /// The server did not find the remote property information
        /// 服务端没有找到远程属性信息
        /// </summary>
        NotFoundProperty,
        /// <summary>
        /// The server did not find the remote field information
        /// 服务端没有找到远程字段信息
        /// </summary>
        NotFoundField,
    }
}
