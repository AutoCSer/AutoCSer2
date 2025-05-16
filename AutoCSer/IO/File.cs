using System;
using System.Threading.Tasks;

namespace AutoCSer.IO
{
    /// <summary>
    /// 文件扩展操作
    /// </summary>
    public static class File
    {
        /// <summary>
        /// 临时文件前缀
        /// </summary>
        public const string BakPrefix = "%";
        /// <summary>
        /// 修改文件名成为默认备份文件 %yyyyMMdd-HHmmss_HEX_fileName
        /// </summary>
        /// <param name="fileName">源文件名</param>
        /// <returns>备份文件名称,失败返回 null</returns>
#if NetStandard21
        internal static async Task<string?> MoveBak(string fileName)
#else
        internal static async Task<string> MoveBak(string fileName)
#endif
        {
            if (await AutoCSer.Common.FileExists(fileName))
            {
                string newFileName = await MoveBakFileName(fileName);
                await AutoCSer.Common.FileMove(fileName, newFileName);
                return newFileName;
            }
            return null;
        }
        /// <summary>
        /// 获取备份文件名称 %yyyyMMdd-HHmmss_HEX_fileName
        /// </summary>
        /// <param name="fileName">源文件名</param>
        /// <returns>备份文件名称</returns>
        internal static async Task<string> MoveBakFileName(string fileName)
        {
            string newFileName;
            do
            {
                newFileName = AutoCSer.Common.GetMoveBakFileName(fileName);
            }
            while (await AutoCSer.Common.FileExists(newFileName));
            return newFileName;
        }
    }
}
