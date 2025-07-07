using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制反序列化状态结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeserializeResult
    {
        /// <summary>
        /// 解析状态
        /// </summary>
        public DeserializeStateEnum State;
        /// <summary>
        /// JSON 解析状态
        /// </summary>
        public Json.DeserializeStateEnum JsonState;
        /// <summary>
        /// 成员位图
        /// </summary>
#if NetStandard21
        public AutoCSer.Metadata.MemberMap? MemberMap;
#else
        public AutoCSer.Metadata.MemberMap MemberMap;
#endif
        /// <summary>
        /// 自定义错误
        /// </summary>
#if NetStandard21
        public string? CustomError;
#else
        public string CustomError;
#endif
        /// <summary>
        /// 二进制反序列化状态结果
        /// </summary>
        /// <param name="memberMap"></param>
#if NetStandard21
        internal DeserializeResult(AutoCSer.Metadata.MemberMap? memberMap)
#else
        internal DeserializeResult(AutoCSer.Metadata.MemberMap memberMap)
#endif
        {
            State = DeserializeStateEnum.Success;
            JsonState = Json.DeserializeStateEnum.Success;
            MemberMap = memberMap;
            CustomError = null;
        }
        /// <summary>
        /// 二进制反序列化状态结果
        /// </summary>
        /// <param name="state"></param>
        /// <param name="jsonState"></param>
        /// <param name="customError"></param>
#if NetStandard21
        internal DeserializeResult(DeserializeStateEnum state, Json.DeserializeStateEnum jsonState = Json.DeserializeStateEnum.Success, string? customError = null)
#else
        internal DeserializeResult(DeserializeStateEnum state, Json.DeserializeStateEnum jsonState = Json.DeserializeStateEnum.Success, string customError = null)
#endif
        {
            State = state;
            JsonState = jsonState;
            MemberMap = null;
            CustomError = customError;
        }

        /// <summary>
        /// Implicit conversion是否成功
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator bool(DeserializeResult value) { return value.State == DeserializeStateEnum.Success; }
    }
}
