using System;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 多哈希位图容器参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ManyHashBitMapCapacity
    {
        /// <summary>
        /// 哈希值数量
        /// </summary>
        private readonly byte hashCount;
        /// <summary>
        /// 位图容器大小
        /// </summary>
        private readonly long capacity;
        /// <summary>
        /// 多哈希位图容器参数
        /// </summary>
        /// <param name="estimatedCount">预期数据数量</param>
        /// <param name="hashCount">哈希值数量</param>
        /// <param name="misjudgmentProbability">误判概率=(estimatedCount*hashCount/capacity)^hashCount</param>
        public ManyHashBitMapCapacity(uint estimatedCount, byte hashCount = 4, double misjudgmentProbability = 0.01)
        {
            this.hashCount = hashCount != 0 ? hashCount : (byte)1;
            if (misjudgmentProbability <= 0) misjudgmentProbability = 0.01;
            double capacity = ((ulong)Math.Max(estimatedCount, 1) * this.hashCount) / Math.Pow(Math.Min(misjudgmentProbability, 1), (1 / (double)this.hashCount));
            if (capacity <= long.MaxValue)
            {
                this.capacity = Math.Max((long)capacity, 0);
                if (this.capacity != long.MaxValue) ++this.capacity;
            }
            else this.capacity = long.MaxValue;
        }
        /// <summary>
        /// 获取位图容器大小
        /// </summary>
        /// <returns></returns>
        public int GetHashCapacity()
        {
            if (capacity <= AutoCSer.ReusableDictionary.MaxPrime) return AutoCSer.ReusableDictionary.GetCapacity((int)capacity);
            throw new IndexOutOfRangeException();
        }
    }
}
