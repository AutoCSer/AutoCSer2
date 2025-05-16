using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图对象
    /// </summary>
    public abstract class MemberMap
    {
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal abstract bool Deserialize(BinaryDeserializer deserializer);
        /// <summary>
        /// 获取成员名称
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
#if NetStandard21
        internal static string? GetMemberName(LambdaExpression memberExpression)
#else
        internal static string GetMemberName(LambdaExpression memberExpression)
#endif
        {
            Expression expression = memberExpression.Body;
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                MemberInfo member = ((MemberExpression)expression).Member;
                var field = member as FieldInfo;
                if (field != null) return field.Name;
                var property = member as PropertyInfo;
                if (property != null) return property.Name;
            }
            return null;
        }
    }
    /// <summary>
    /// 成员位图对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed partial class MemberMap<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicFields | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicFields | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties)]
#endif
    T> : MemberMap
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMapData<T> MemberMapData;
        /// <summary>
        /// 是否默认全部成员有效
        /// </summary>
        public bool IsDefault { get { return MemberMapData.IsDefault; } }
        /// <summary>
        /// 成员位图对象
        /// </summary>
        public MemberMap() { }
        /// <summary>
        /// 成员位图对象
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        internal MemberMap(MemberMapData<T> memberMap)
        {
            MemberMapData = memberMap;
        }
        /// <summary>
        /// 成员位图对象
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        internal MemberMap(ref MemberMapData<T> memberMap)
        {
            MemberMapData = memberMap;
        }
        /// <summary>
        /// 复制成员位图
        /// </summary>
        /// <param name="memberMap"></param>
        public MemberMap(MemberMap<T> memberMap)
        {
            MemberMapData = memberMap.MemberMapData.Copy();
        }
        /// <summary>
        /// 获取成员位图索引
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public MemberMapIndex<T> GetMemberIndex(string memberName)
        {
            return new MemberMapIndex<T>(memberName);
        }
        /// <summary>
        /// 获取成员位图索引
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member">成员</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public MemberMapIndex<T> GetMemberIndex<VT>(Expression<Func<T, VT>> member)
        {
            return new MemberMapIndex<T>(GetMemberName(member));
        }
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ClearMember(MemberMapIndex<T> memberIndex)
        {
            if (memberIndex.MemberIndex >= 0) MemberMapData.ClearMember(memberIndex.MemberIndex);
        }
        /// <summary>
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member">成员</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ClearMember<VT>(Expression<Func<T, VT>> member)
        {
            MemberMapData.ClearMember(GetMemberName(member));
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool SetMember(MemberMapIndex<T> memberIndex)
        {
            if (memberIndex.MemberIndex >= 0)
            {
                MemberMapData.SetMember(memberIndex.MemberIndex);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member">成员</param>
        /// <returns>是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool SetMember<VT>(Expression<Func<T, VT>> member)
        {
            return MemberMapData.SetMember(GetMemberName(member));
        }
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员索引是否有效</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsMember(MemberMapIndex<T> memberIndex)
        {
            return memberIndex.MemberIndex >= 0 && MemberMapData.IsMember(memberIndex.MemberIndex);
        }
#if AOT
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员索引是否有效</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsMember(int memberIndex)
        {
            return (uint)memberIndex < (uint)MemberMapData<T>.MemberCount && MemberMapData.IsMember(memberIndex);
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool SetMember(int memberIndex)
        {
            if ((uint)memberIndex < (uint)MemberMapData<T>.MemberCount)
            {
                MemberMapData.SetMember(memberIndex);
                return true;
            }
            return false;
        }
#endif
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        /// <param name="memberIndex">成员索引</param>
        /// <returns>成员索引是否有效</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsMember(MemberMap<T> memberMap, int memberIndex)
        {
            return memberMap.MemberMapData.IsMember(memberIndex);
        }
        /// <summary>
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="memberIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetMember(MemberMap<T> memberMap, int memberIndex)
        {
            memberMap.MemberMapData.SetMember(memberIndex);
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override bool Deserialize(BinaryDeserializer deserializer)
        {
            return MemberMapData.Deserialize(deserializer);
        }

        /// <summary>
        /// 成员所有成员位图
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static MemberMap<T> NewFull()
        {
            MemberMap<T> memberMap = new MemberMap<T>();
            memberMap.MemberMapData.Full();
            return memberMap;
        }
        /// <summary>
        /// 空成员位图
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static MemberMap<T> NewEmpty()
        {
            MemberMap<T> value = new MemberMap<T>();
            value.MemberMapData.Empty();
            return value;
        }
        /// <summary>
        /// 默认公共缺省反序列化成员位图
        /// </summary>
        internal static readonly MemberMap<T> Default = new MemberMap<T>();
    }
}
