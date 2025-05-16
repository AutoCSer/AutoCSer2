using AutoCSer.Metadata;
using System;
using System.Reflection;

namespace AutoCSer
{
    /// <summary>
    /// 成员复制方法
    /// </summary>
    internal static class MemberCopyMethod
    {
        /// <summary>
        /// 数组复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="writeArray"></param>
        /// <param name="readArray"></param>
        internal static void CopyArray<T>(ref T[] writeArray, T[] readArray)
        {
            MemberCopy<T>.CopyArray(ref writeArray, readArray);
        }
        /// <summary>
        /// 数组复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="writeArray"></param>
        /// <param name="readArray"></param>
        /// <param name="memberMap"></param>
        internal static void CopyArrayMemberMap<T>(ref T[] writeArray, T[] readArray, MemberMap<T[]> memberMap)
        {
            MemberCopy<T>.CopyArray(ref writeArray, readArray);
        }
        /// <summary>
        /// 数组复制
        /// </summary>
        internal static readonly MethodInfo CopyArrayMethod;
        /// <summary>
        /// 数组复制
        /// </summary>
        internal static readonly MethodInfo CopyArrayMemberMapMethod;
        static MemberCopyMethod()
        {
#if NetStandard21
            CopyArrayMethod = CopyArrayMemberMapMethod = AutoCSer.Common.NullMethodInfo;
#endif
            foreach (MethodInfo method in typeof(MemberCopyMethod).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                switch (method.Name.Length)
                {
                    case 9:
                        if (method.Name == nameof(CopyArray)) CopyArrayMethod = method;
                        break;
                    case 18:
                        if (method.Name == nameof(CopyArrayMemberMap)) CopyArrayMemberMapMethod = method;
                        break;
                }
            }
        }
    }
}
