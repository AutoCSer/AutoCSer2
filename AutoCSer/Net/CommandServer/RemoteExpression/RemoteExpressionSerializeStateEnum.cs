using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程表达式序列化状态
    /// </summary>
    public enum RemoteExpressionSerializeStateEnum : byte
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        ///// <summary>
        ///// 不支持的委托类型
        ///// </summary>
        //NotSupportDelegateType,
        /// <summary>
        /// 未知节点类型
        /// </summary>
        UnknownNodeType,
        /// <summary>
        /// 不支持 new 操作节点 
        /// </summary>
        NotSupportNew,
        /// <summary>
        /// 不支持 Lambda 节点
        /// </summary>
        NotSupportLambda,
        /// <summary>
        /// 不支持代码块节点 Block
        /// </summary>
        NotSupportBlock,
        /// <summary>
        /// 不支持动态类型节点 Dynamic
        /// </summary>
        NotSupportDynamic,
        /// <summary>
        /// 不支持 goto 标签节点 Label
        /// </summary>
        NotSupportLabel,
        /// <summary>
        /// 不支持扩展节点 Extension
        /// </summary>
        NotSupportExtension,
        /// <summary>
        /// 不支持调试信息节点 DebugInfo
        /// </summary>
        NotSupportDebugInfo,
        /// <summary>
        /// 不支持条件表达式节点 Conditional
        /// </summary>
        NotSupportConditional,
        /// <summary>
        /// 不支持多维数组索引
        /// </summary>
        NotSupportArrayIndex,
        /// <summary>
        /// 常量值计算发生空引用异常
        /// </summary>
        ConstantNullReference,
        /// <summary>
        /// 常量参数数量超过最大值 65535
        /// </summary>
        TooManyConstant,
        ///// <summary>
        ///// 不支持的常量值类型
        ///// </summary>
        //NotSupportConstantType,
        /// <summary>
        /// 泛型类型参数数量超过最大值 255
        /// </summary>
        TooManyGenericArguments,
        /// <summary>
        /// 方法参数数量最大值 255
        /// </summary>
        TooManyParameters,
        /// <summary>
        /// 没有找到匹配参数表达式
        /// </summary>
        NotFoundParameter,

        /// <summary>
        /// 服务端反序列化失败
        /// </summary>
        DeserializeError,
        /// <summary>
        /// 服务端反序列化没有找到套接字上下文
        /// </summary>
        NotFoundCommandServerSocket,
        /// <summary>
        /// 服务端反序列化没有找到远程元数据信息，说明服务端缺少配置 AutoCSer.Net.CommandServerConfig.IsRemoteExpression
        /// </summary>
        NotFoundRemoteMetadata,
        /// <summary>
        /// 服务端未知序列化信息
        /// </summary>
        UnknownSerializeInfo,
        /// <summary>
        /// 服务端格式化数据写入地址超出范围
        /// </summary>
        FormatWriteIndexOutOfRange,
        /// <summary>
        /// 服务端格式化数据读取地址超出范围
        /// </summary>
        FormatReadIndexOutOfRange,
        /// <summary>
        /// 服务端没有找到远程类型信息
        /// </summary>
        NotFoundType,
        /// <summary>
        /// 服务端没有找到远程方法信息
        /// </summary>
        NotFoundMethod,
        /// <summary>
        /// 服务端没有找到远程属性信息
        /// </summary>
        NotFoundProperty,
        /// <summary>
        /// 服务端没有找到远程字段信息
        /// </summary>
        NotFoundField,
    }
}
