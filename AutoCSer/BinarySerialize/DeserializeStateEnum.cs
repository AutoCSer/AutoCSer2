using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// Deserialization status
    /// 反序列化状态
    /// </summary>
    public enum DeserializeStateEnum : byte
    {
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// The data is not identifiable
        /// 数据不可识别
        /// </summary>
        UnknownData,
        /// <summary>
        /// The header data does not match
        /// 头部数据不匹配
        /// </summary>
        HeaderError,
        /// <summary>
        /// The object reference data does not match
        /// 对象引用数据不匹配
        /// </summary>
        ObjectReferenceError,
        /// <summary>
        /// End verification error
        /// 结束验证错误
        /// </summary>
        EndVerify,
        /// <summary>
        /// Member bitmap detection failed
        /// 成员位图检测失败
        /// </summary>
        MemberMap,
        /// <summary>
        /// The member bitmap type is incorrect
        /// 成员位图类型错误
        /// </summary>
        MemberMapType,
        /// <summary>
        /// The verification of the number of member bitmaps failed
        /// 成员位图数量验证失败
        /// </summary>
        MemberMapVerify,
        /// <summary>
        /// JSON deserialization failed
        /// JSON 反序列化失败
        /// </summary>
        JsonError,
        /// <summary>
        /// The historical object was not hit
        /// 没有命中历史对象
        /// </summary>
        NoPoint,
        /// <summary>
        /// Insufficient data length
        /// 数据长度不足
        /// </summary>
        IndexOutOfRange,
        /// <summary>
        /// The size of the array is out of range
        /// 数组大小超出范围
        /// </summary>
        ArraySizeOutOfRange,
        /// <summary>
        /// Unidentifiable data type
        /// 不可识别的数据类型
        /// </summary>
        ErrorDataType,
        /// <summary>
        /// Type resolution error, or the generic type does not meet the AutoCSer.Common.Config.CheckRemoteType validity check conditions
        /// 类型解析错误，或者泛型类型不满足 AutoCSer.Common.Config.CheckRemoteType 合法性检查条件
        /// </summary>
        ErrorType,
        /// <summary>
        /// Unsupported types
        /// 不支持的类型
        /// </summary>
        NotSupport,
        /// <summary>
        /// The custom buffer creation failed
        /// 自定义缓冲区创建失败
        /// </summary>
        CustomBufferError,
        /// <summary>
        /// Custom deserialization failed
        /// 自定义反序列化失败
        /// </summary>
        CustomError,
        /// <summary>
        /// The constructor returns a null value
        /// 构造函数返回 null 值
        /// </summary>
        ConstructorNull,
        /// <summary>
        /// Non-object value
        /// 非 object 值
        /// </summary>
        NotObject,
    }
}
