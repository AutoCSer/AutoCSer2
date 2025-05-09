using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 修复方法文件信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public struct RepairNodeMethodFile : IEquatable<RepairNodeMethodFile>
    {
        /// <summary>
        /// 程序集文件长度
        /// </summary>
        internal long AssemblyFileLength;
        /// <summary>
        /// 方法名称信息 JSON 文件长度
        /// </summary>
        internal long MethodNameFileLength;
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        internal DateTime LastWriteTime;
        /// <summary>
        /// 修复方法文件信息
        /// </summary>
        /// <param name="assemblyFile">程序集文件</param>
        /// <param name="methodNameFile">方法名称信息 JSON 文件</param>
        internal RepairNodeMethodFile(FileInfo assemblyFile, FileInfo methodNameFile)
        {
            AssemblyFileLength = assemblyFile.Length;
            LastWriteTime = assemblyFile.LastWriteTimeUtc;
            MethodNameFileLength = methodNameFile.Length;
        }
        /// <summary>
        /// 设置程序集文件信息
        /// </summary>
        /// <param name="assemblyFile">程序集文件</param>
        /// <param name="methodNameFile">方法名称信息 JSON 文件</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(FileInfo assemblyFile, FileInfo methodNameFile)
        {
            AssemblyFileLength = assemblyFile.Length;
            LastWriteTime = assemblyFile.LastWriteTimeUtc;
            MethodNameFileLength = methodNameFile.Length;
        }
        /// <summary>
        /// 判断是否一致
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RepairNodeMethodFile other)
        {
            return LastWriteTime == other.LastWriteTime && ((AssemblyFileLength ^ other.AssemblyFileLength) | (MethodNameFileLength ^ other.MethodNameFileLength)) == 0;
        }
    }
}
