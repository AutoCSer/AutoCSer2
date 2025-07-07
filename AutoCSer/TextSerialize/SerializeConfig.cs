using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// Text serialization configuration parameters
    /// 文本序列化配置参数
    /// </summary>
    public abstract class SerializeConfig
    {
        /// <summary>
        /// The default maximum node detection depth value is 64
        /// 默认最大节点检测深度值为 64
        /// </summary>
        public const int DefaultCheckDepth = 64;

        /// <summary>
        /// Member bitmap
        /// 成员位图
        /// </summary>
#if NetStandard21
        public MemberMap? MemberMap;
#else
        public MemberMap MemberMap;
#endif
        /// <summary>
        /// The maximum node detection depth is set to 64 by default. (Excessive depth can cause stack overflow, so this serialization component is not suitable for serializing linked list structures. If there is a similar requirement, please customize the serialization conversion to an array for processing)
        /// 最大节点检测深度，默认为 64（过大的深度会造成堆栈溢出，所以该序列化组件不适合序列化链表结构，如果存在该类似需求请自定义序列化转换为数组处理）
        /// </summary>
        public int CheckDepth = DefaultCheckDepth;
        /// <summary>
        /// The default is true, indicating that the circular reference is checked
        /// 默认为 true 表示检查循环引用
        /// </summary>
        public bool CheckLoop = true;
        /// <summary>
        /// The default is true, indicating that the default output is used when the member bitmap types do not match
        /// 默认为 true 表示成员位图类型不匹配时使用默认输出
        /// </summary>
        public bool IsMemberMapErrorToDefault = true;
        /// <summary>
        /// The default is true, indicating that Infinity / -Infinity is converted to NaN output
        /// 默认为 true 表示将 Infinity / -Infinity 转换为 NaN 输出
        /// </summary>
        public bool IsInfinityToNaN = true;
        /// <summary>
        /// Whether to convert object to a real type for output
        /// 是否将 object 转换成真实类型输出
        /// </summary>
        public bool IsObject;
        /// <summary>
        /// By default, false indicates that the enumeration type outputs a number; otherwise, a string is output
        /// 默认为 false 表示枚举类型输出数字，否则输出字符串
        /// </summary>
        public bool IsEnumToString;

        /// <summary>
        /// Get and set the custom serialization member bitmap
        /// 获取并设置自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">Serialization member bitmap
        /// 序列化成员位图</param>
        /// <returns>Original serialization member bitmap
        /// 原序列化成员位图</returns>
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
