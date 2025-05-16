using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 序列化配置参数
    /// </summary>
    public abstract class SerializeConfig
    {
        /// <summary>
        /// 默认最大节点检测深度
        /// </summary>
        public const int DefaultCheckDepth = 64;

        /// <summary>
        /// 成员位图
        /// </summary>
#if NetStandard21
        public MemberMap? MemberMap;
#else
        public MemberMap MemberMap;
#endif
        /// <summary>
        /// 最大节点检测深度，默认为 64（过大的深度会造成堆栈溢出，所以该序列化组件不适合序列化链表结构，如果存在该类似需求请自定义序列化转换为数组处理）
        /// </summary>
        public int CheckDepth = DefaultCheckDepth;
        /// <summary>
        /// 是否检查循环引用，默认为 true
        /// </summary>
        public bool CheckLoop = true;
        /// <summary>
        /// 成员位图类型不匹配时是否使用默认输出，默认为 true
        /// </summary>
        public bool IsMemberMapErrorToDefault = true;
        /// <summary>
        /// 默认为 true 表示将 Infinity / -Infinity 转换为 NaN 输出
        /// </summary>
        public bool IsInfinityToNaN = true;
        /// <summary>
        /// 是否将 object 转换成真实类型输出
        /// </summary>
        public bool IsObject;
        /// <summary>
        /// 枚举类型是否输出字符串，否则输出数字
        /// </summary>
        public bool IsEnumToString;

        /// <summary>
        /// 获取并设置自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">序列化成员位图</param>
        /// <returns>序列化成员位图</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public MemberMap? SetCustomMemberMap(MemberMap? memberMap)
#else
        public MemberMap SetCustomMemberMap(MemberMap memberMap)
#endif
        {
            var oldMemberMap = MemberMap;
            MemberMap = memberMap;
            return oldMemberMap;
        }
    }
}
