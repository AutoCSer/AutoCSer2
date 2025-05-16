using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 反序列化状态
    /// </summary>
    public enum DeserializeStateEnum : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 数据不可识别
        /// </summary>
        UnknownData,
        /// <summary>
        /// 头部数据不匹配
        /// </summary>
        HeaderError,
        /// <summary>
        /// 对象引用数据不匹配
        /// </summary>
        ObjectReferenceError,
        /// <summary>
        /// 结束验证错误
        /// </summary>
        EndVerify,
        /// <summary>
        /// 成员位图检测失败
        /// </summary>
        MemberMap,
        /// <summary>
        /// 成员位图类型错误
        /// </summary>
        MemberMapType,
        /// <summary>
        /// 成员位图数量验证失败
        /// </summary>
        MemberMapVerify,
        /// <summary>
        /// JSON反序列化失败
        /// </summary>
        JsonError,
        /// <summary>
        /// 没有命中历史对象
        /// </summary>
        NoPoint,
        /// <summary>
        /// 数据长度不足
        /// </summary>
        IndexOutOfRange,
        /// <summary>
        /// 数组大小超出范围
        /// </summary>
        ArraySizeOutOfRange,
        /// <summary>
        /// 不可识别的数据类型
        /// </summary>
        ErrorDataType,
        /// <summary>
        /// 类型解析错误，或者泛型类型不满足 AutoCSer.Common.Config.CheckRemoteType 合法性检查条件
        /// </summary>
        ErrorType,
        /// <summary>
        /// 不支持的类型
        /// </summary>
        NotSupport,
        /// <summary>
        /// 自定义缓冲区创建失败
        /// </summary>
        CustomBufferError,
        /// <summary>
        /// 自定义反序列化失败
        /// </summary>
        CustomError,
        /// <summary>
        /// 构造函数返回 null 值
        /// </summary>
        ConstructorNull,
        /// <summary>
        /// 非 object 值
        /// </summary>
        NotObject,
    }
}
