using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 修复方法目录信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct RepairNodeMethodDirectory : IEquatable<RepairNodeMethodDirectory>
    {
        /// <summary>
        /// 节点类型哈希值
        /// </summary>
        internal ulong NodeTypeHashCode;
        /// <summary>
        /// 修复方法生效的持久化重建绝对位置
        /// </summary>
        internal ulong Position;
        /// <summary>
        /// 修复方法目录创建时间
        /// </summary>
        internal ulong RepairTime;
        /// <summary>
        /// 方法编号
        /// </summary>
        internal uint MethodIndex;
        /// <summary>
        /// 修复方法目录信息
        /// </summary>
        /// <param name="nodeTypeFullName">节点类型名称</param>
        internal RepairNodeMethodDirectory(string nodeTypeFullName)
        {
            NodeTypeHashCode = ((SubString)nodeTypeFullName).GetHashCode64();
            Position = RepairTime = 0;
            MethodIndex = 0;
        }
        /// <summary>
        /// 修复方法目录信息
        /// </summary>
        /// <param name="nodeTypeFullName">节点类型名称</param>
        /// <param name="methodIndex">方法编号</param>
        internal RepairNodeMethodDirectory(string nodeTypeFullName, int methodIndex)
        {
            NodeTypeHashCode = ((SubString)nodeTypeFullName).GetHashCode64();
            Position = 0;
            MethodIndex = (uint)methodIndex;
            DateTime repairTime = AutoCSer.Threading.SecondTimer.Now;
            RepairTime = (ulong)repairTime.Year * 10000000000UL + (ulong)(repairTime.Month * 100000000 + repairTime.Day * 1000000 + repairTime.Hour * 10000 + repairTime.Minute * 100 + repairTime.Second);
        }
        /// <summary>
        /// 判断是否一致
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RepairNodeMethodDirectory other)
        {
            return Position == other.Position && NodeTypeHashCode == other.NodeTypeHashCode  && MethodIndex == other.MethodIndex && RepairTime == other.RepairTime;
        }
        /// <summary>
        /// 判断是否一致
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((RepairNodeMethodDirectory)obj);
        }
        /// <summary>
        /// 哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)((uint)Position ^ (uint)(Position >> 32) ^ (uint)RepairTime ^ (uint)(RepairTime >> 32) ^ (uint)NodeTypeHashCode ^ (uint)(NodeTypeHashCode >> 32) ^ MethodIndex);
        }
    }
}
