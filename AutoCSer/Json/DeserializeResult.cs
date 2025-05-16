using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 反序列化状态结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeserializeResult
    {
        /// <summary>
        /// 解析状态
        /// </summary>
        public DeserializeStateEnum State;
        /// <summary>
        /// 当前解析位置
        /// </summary>
        public int Index;
        /// <summary>
        /// 成员位图
        /// </summary>
#if NetStandard21
        public AutoCSer.Metadata.MemberMap? MemberMap;
#else
        public AutoCSer.Metadata.MemberMap MemberMap;
#endif
        /// <summary>
        /// JSON 字符串
        /// </summary>
        public SubString Json;
        /// <summary>
        /// 自定义错误
        /// </summary>
#if NetStandard21
        public string? CustomError;
#else
        public string CustomError;
#endif
        /// <summary>
        /// JSON 反序列化状态结果
        /// </summary>
        /// <param name="memberMap"></param>
#if NetStandard21
        internal DeserializeResult(AutoCSer.Metadata.MemberMap? memberMap)
#else
        internal DeserializeResult(AutoCSer.Metadata.MemberMap memberMap)
#endif
        {
            State = DeserializeStateEnum.Success;
            MemberMap = memberMap;
            Index = 0;
            CustomError = null;
            Json = string.Empty;
        }
        /// <summary>
        /// JSON 反序列化状态结果
        /// </summary>
        /// <param name="state"></param>
        /// <param name="index"></param>
        /// <param name="json"></param>
        /// <param name="customError"></param>
#if NetStandard21
        internal DeserializeResult(DeserializeStateEnum state, int index = 0, string? json = null, string? customError = null)
#else
        internal DeserializeResult(DeserializeStateEnum state, int index = 0, string json = null, string customError = null)
#endif
        {
            State = state;
            MemberMap = null;
            Index = index;
            Json = json ?? string.Empty;
            CustomError = null;
        }
        /// <summary>
        /// JSON 反序列化状态结果
        /// </summary>
        /// <param name="state"></param>
        /// <param name="json"></param>
        /// <param name="index"></param>
        /// <param name="customError"></param>
#if NetStandard21
        internal DeserializeResult(DeserializeStateEnum state, ref SubString json, int index, string? customError = null)
#else
        internal DeserializeResult(DeserializeStateEnum state, ref SubString json, int index, string customError = null)
#endif
        {
            State = state;
            MemberMap = null;
            Index = index;
            Json = json;
            CustomError = customError;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator bool(DeserializeResult value) { return value.State == DeserializeStateEnum.Success; }
    }
}
