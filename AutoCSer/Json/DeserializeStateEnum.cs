using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析状态
    /// </summary>
    public enum DeserializeStateEnum : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// JSON 字符串参数为空
        /// </summary>
        NullJson,
        /// <summary>
        /// 非正常意外结束
        /// </summary>
        CrashEnd,
        /// <summary>
        /// 未能识别的注释
        /// </summary>
        UnknownNote,
        /// <summary>
        /// /**/ 注释缺少回合
        /// </summary>
        NoteNotRound,
        /// <summary>
        /// null 值解析失败
        /// </summary>
        NotNull,
        /// <summary>
        /// 逻辑值解析错误
        /// </summary>
        NotBool,
        /// <summary>
        /// 非数字解析错误
        /// </summary>
        NotNumber,
        /// <summary>
        /// 数字解析超出范围
        /// </summary>
        NumberOutOfRange,
        /// <summary>
        /// 16 进制数字解析错误
        /// </summary>
        NotHex,
        /// <summary>
        /// 时间解析错误
        /// </summary>
        NotDateTime,
        /// <summary>
        /// 时间解析错误
        /// </summary>
        NotTimeSpan,
        /// <summary>
        /// 字符解析错误
        /// </summary>
        NotChar,
        /// <summary>
        /// Guid解析错误
        /// </summary>
        NotGuid,
        /// <summary>
        /// 字符串解析失败
        /// </summary>
        NotString,
        /// <summary>
        /// 字符串被换行截断
        /// </summary>
        StringEnter,
        /// <summary>
        /// 类型解析错误
        /// </summary>
        ErrorType,
        /// <summary>
        /// 数组数据解析错误
        /// </summary>
        NotArrayValue,
        /// <summary>
        /// 数组长度错误
        /// </summary>
        ArraySizeError,
        /// <summary>
        /// 非枚举字符
        /// </summary>
        NotEnumChar,
        /// <summary>
        /// 没有找到匹配的枚举值
        /// </summary>
        NoFoundEnumValue,
        /// <summary>
        /// 没有找到成员名称
        /// </summary>
        NotFoundName,
        /// <summary>
        /// 没有找到冒号
        /// </summary>
        NotFoundColon,
        /// <summary>
        /// 对象解析错误
        /// </summary>
        NotObject,
        /// <summary>
        /// 忽略值解析错误
        /// </summary>
        UnknownValue,
        /// <summary>
        /// 不支持的类型解析错误
        /// </summary>
        NotSupport,
        /// <summary>
        /// 构造函数返回 null 值
        /// </summary>
        ConstructorNull,
        /// <summary>
        /// 成员位图类型错误
        /// </summary>
        MemberMap,
        /// <summary>
        /// System.Numerics.Complex 混杂解析失败
        /// </summary>
        NotComplex,
        /// <summary>
        /// System.Numerics.Vector2 混杂解析失败
        /// </summary>
        NotVector2,
        /// <summary>
        /// System.Numerics.Vector3 混杂解析失败
        /// </summary>
        NotVector3,
        /// <summary>
        /// System.Numerics.Vector4 混杂解析失败
        /// </summary>
        NotVector4,
        /// <summary>
        /// System.Numerics.Plane 混杂解析失败
        /// </summary>
        NotPlane,
        /// <summary>
        /// System.Numerics.Quaternion 混杂解析失败
        /// </summary>
        NotQuaternion,
        /// <summary>
        /// System.Numerics.Matrix3x2 混杂解析失败
        /// </summary>
        NotMatrix3x2,
        /// <summary>
        /// System.Numerics.Matrix4x4 混杂解析失败
        /// </summary>
        NotMatrix4x4,
        /// <summary>
        /// 自定义反序列化失败
        /// </summary>
        CustomError,
    }
}
