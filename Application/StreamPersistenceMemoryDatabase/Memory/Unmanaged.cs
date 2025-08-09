using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Memory
{
    /// <summary>
    /// 非托管内存
    /// </summary>
    internal unsafe static class Unmanaged
    {
        /// <summary>
        /// AutoCSer 使用静态内存段，防止碎片化
        /// </summary>
        private static AutoCSer.Memory.Pointer AutoCSerStatic;
        /// <summary>
        /// 基于均匀概率的总量统计的二进制位数量的倒数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Memory.Pointer GetPowerReciprocal()
        {
            return AutoCSerStatic.Slice(0, sizeof(double) * (65 - UniformProbabilityTotalStatisticsNode.MinIndexBits));
        }
        static Unmanaged()
        {
            AutoCSerStatic = AutoCSer.Memory.Unmanaged.GetStaticPointer(sizeof(double) * (65 - UniformProbabilityTotalStatisticsNode.MinIndexBits), false);
        }
    }
}
