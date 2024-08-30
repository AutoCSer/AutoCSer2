using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// XML 解析结果
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
        public AutoCSer.Metadata.MemberMap MemberMap;
        /// <summary>
        /// XML 字符串
        /// </summary>
        public SubString Xml;
        /// <summary>
        /// 自定义错误
        /// </summary>
        public string CustomError;
        /// <summary>
        /// XML 反序列化状态结果
        /// </summary>
        /// <param name="memberMap"></param>
        internal DeserializeResult(AutoCSer.Metadata.MemberMap memberMap)
        {
            State = DeserializeStateEnum.Success;
            MemberMap = memberMap;
            Index = 0;
            CustomError = string.Empty;
            Xml = string.Empty;
        }
        /// <summary>
        /// XML 反序列化状态结果
        /// </summary>
        /// <param name="state"></param>
        /// <param name="index"></param>
        /// <param name="xml"></param>
        /// <param name="customError"></param>
        internal DeserializeResult(DeserializeStateEnum state, int index = 0, string xml = null, string customError = null)
        {
            State = state;
            MemberMap = null;
            Index = index;
            Xml = xml ?? string.Empty;
            CustomError = customError ?? string.Empty;
        }
        /// <summary>
        /// XML 反序列化状态结果
        /// </summary>
        /// <param name="state"></param>
        /// <param name="xml"></param>
        /// <param name="index"></param>
        /// <param name="customError"></param>
        internal DeserializeResult(DeserializeStateEnum state, ref SubString xml, int index, string customError = null)
        {
            State = state;
            MemberMap = null;
            Index = index;
            Xml = xml;
            CustomError = customError ?? string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator bool(DeserializeResult value) { return value.State == DeserializeStateEnum.Success; }
    }
}
