using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 文件信息相关扩展
    /// </summary>
    public static class FileInfoExtension
    {
        /// <summary>
        /// 刷新文件信息状态并返回文件是否存在状态
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool RefreshExists(this FileInfo file)
        {
            if(file != null)
            {
                file.Refresh();
                return file.Exists;
            }
            return false;
        }
        /// <summary>
        /// 刷新文件信息状态并返回文件最后修改时间
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static DateTime RefreshLastWriteTimeUtc(this FileInfo file)
        {
            if (file != null)
            {
                file.Refresh();
                return file.LastWriteTimeUtc;
            }
            return default(DateTime);
        }
    }
}
