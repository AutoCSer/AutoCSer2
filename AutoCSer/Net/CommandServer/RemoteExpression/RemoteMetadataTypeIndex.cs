using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程元数据类型编号信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    internal struct RemoteMetadataTypeIndex
    {
        /// <summary>
        /// Assembly name
        /// 程序集名称
        /// </summary>
        public string AssemblyName;
        /// <summary>
        /// Type name
        /// </summary>
        public string TypeName;
        /// <summary>
        /// 类型编号
        /// </summary>
        public int Index;
        /// <summary>
        /// 设置远程元数据类型编号信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(AutoCSer.Reflection.RemoteType type, int typeIndex)
        {
            AssemblyName = type.AssemblyName;
            TypeName = type.Name;
            Index = typeIndex;
        }
        /// <summary>
        /// 获取远程类型信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal new Type? GetType()
#else
        internal new Type GetType()
#endif
        {
            var type = default(Type);
            return new AutoCSer.Reflection.RemoteType(AssemblyName, TypeName).TryGet(out type, false) ? type : null;
        }
    }
}
