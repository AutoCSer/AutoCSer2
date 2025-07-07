using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// Member bitmap
    /// 成员位图
    /// </summary>
    public abstract class MemberMap
    {
        /// <summary>
        /// Binary deserialization
        /// 二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal abstract bool Deserialize(BinaryDeserializer deserializer);
        /// <summary>
        /// Get the member name
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
    /// Member bitmap
    /// 成员位图
    /// </summary>
    /// <typeparam name="T">Data type
    /// 数据类型</typeparam>
    public sealed partial class MemberMap<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicFields | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicFields | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties)]
#endif
    T> : MemberMap
    {
        /// <summary>
        /// Member bitmap data
        /// 成员位图数据
        /// </summary>
        internal MemberMapData<T> MemberMapData;
        /// <summary>
        /// Is all members are valid by default
        /// 是否默认全部成员有效
        /// </summary>
        public bool IsDefault { get { return MemberMapData.IsDefault; } }
        /// <summary>
        /// Member bitmap
        /// </summary>
        public MemberMap() { }
        /// <summary>
        /// Member bitmap
        /// </summary>
        /// <param name="memberMap">Member bitmap</param>
        internal MemberMap(MemberMapData<T> memberMap)
        {
            MemberMapData = memberMap;
        }
        /// <summary>
        /// Member bitmap
        /// </summary>
        /// <param name="memberMap">Member bitmap</param>
        internal MemberMap(ref MemberMapData<T> memberMap)
        {
            MemberMapData = memberMap;
        }
        /// <summary>
        /// Copy the member bitmap
        /// 复制成员位图
        /// </summary>
        /// <param name="memberMap"></param>
        public MemberMap(MemberMap<T> memberMap)
        {
            MemberMapData = memberMap.MemberMapData.Copy();
        }
        /// <summary>
        /// Get member bitmap index
        /// 获取成员位图索引
        /// </summary>
        /// <param name="memberName">Member name</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public MemberMapIndex<T> GetMemberIndex(string memberName)
        {
            return new MemberMapIndex<T>(memberName);
        }
        /// <summary>
        /// Get member bitmap index
        /// 获取成员位图索引
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public MemberMapIndex<T> GetMemberIndex<VT>(Expression<Func<T, VT>> member)
        {
            return new MemberMapIndex<T>(GetMemberName(member));
        }
        /// <summary>
        /// Clear the member index and ignore the default members
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">Member index
        /// 成员索引</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ClearMember(MemberMapIndex<T> memberIndex)
        {
            if (memberIndex.MemberIndex >= 0) MemberMapData.ClearMember(memberIndex.MemberIndex);
        }
        /// <summary>
        /// Clear the member index and ignore the default members
        /// 清除成员索引,忽略默认成员
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ClearMember<VT>(Expression<Func<T, VT>> member)
        {
            MemberMapData.ClearMember(GetMemberName(member));
        }
        /// <summary>
        /// Clear the member index and ignore the default members
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">Member index
        /// 成员索引</param>
        /// <returns>Whether the setting is successful
        /// 设置是否成功</returns>
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
        /// Clear the member index and ignore the default members
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="member"></param>
        /// <returns>Whether the setting is successful
        /// 设置是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool SetMember<VT>(Expression<Func<T, VT>> member)
        {
            return MemberMapData.SetMember(GetMemberName(member));
        }
        /// <summary>
        /// Determine whether the member index is valid
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberIndex">Member index
        /// 成员索引</param>
        /// <returns>Whether the member index is valid
        /// 成员索引是否有效</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsMember(MemberMapIndex<T> memberIndex)
        {
            return memberIndex.MemberIndex >= 0 && MemberMapData.IsMember(memberIndex.MemberIndex);
        }
#if AOT
        /// <summary>
        /// Determine whether the member index is valid
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberIndex">Member index
        /// 成员索引</param>
        /// <returns>Whether the member index is valid
        /// 成员索引是否有效</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsMember(int memberIndex)
        {
            return (uint)memberIndex < (uint)MemberMapData<T>.MemberCount && MemberMapData.IsMember(memberIndex);
        }
        /// <summary>
        /// Clear the member index and ignore the default members
        /// 设置成员索引,忽略默认成员
        /// </summary>
        /// <param name="memberIndex">Member index
        /// 成员索引</param>
        /// <returns>Whether the setting is successful
        /// 设置是否成功</returns>
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
        /// Determine whether the member index is valid
        /// 判断成员索引是否有效
        /// </summary>
        /// <param name="memberMap">Member bitmap</param>
        /// <param name="memberIndex">Member index
        /// 成员索引</param>
        /// <returns>Whether the member index is valid
        /// 成员索引是否有效</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsMember(MemberMap<T> memberMap, int memberIndex)
        {
            return memberMap.MemberMapData.IsMember(memberIndex);
        }
        /// <summary>
        /// Clear the member index and ignore the default members
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
        /// Binary deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal override bool Deserialize(BinaryDeserializer deserializer)
        {
            return MemberMapData.Deserialize(deserializer);
        }

        /// <summary>
        /// Create a bitmap of all members
        /// 创建所有成员的位图
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
        /// Create a bitmap without members
        /// 创建没有成员的位图
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
        /// Default public default deserialization member bitmap
        /// 默认公共缺省反序列化成员位图
        /// </summary>
        internal static readonly MemberMap<T> Default = new MemberMap<T>();
    }
}
