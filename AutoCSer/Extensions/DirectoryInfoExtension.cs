using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 目录相关操作
    /// </summary>
    internal static partial class DirectoryInfoExtension
    {
        /// <summary>
        /// 目录分隔符
        /// </summary>
        internal static readonly string Separator = Path.DirectorySeparatorChar.ToString();
        /// <summary>
        /// 取以\结尾的路径全名
        /// </summary>
        /// <param name="path">目录</param>
        /// <returns>\结尾的路径全名</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static string fullName(this DirectoryInfo path)
        {
            string name = path.FullName;
            return name[name.Length - 1] == Path.DirectorySeparatorChar ? name : (name + Separator);
        }
        ///// <summary>
        ///// 目录分隔符\替换
        ///// </summary>
        ///// <param name="path">路径</param>
        ///// <returns>替换\后的路径</returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static string pathSeparator(this string path)
        //{
        //    if (Path.DirectorySeparatorChar != '\\') path.replaceNotNull('\\', Path.DirectorySeparatorChar);
        //    return path;
        //}
    }
}
