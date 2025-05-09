using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
#if AOT
    /// <summary>
    /// 创建快照克隆接口节点
    /// </summary>
    public static class SnapshotCloneNode
    {
        /// <summary>
        /// 创建快照克隆接口节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static object Create<T>(object target) where T : SnapshotCloneObject<T>
        {
            return new SnapshotCloneNode<T>((ISnapshot<T>)target);
        }
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal static void ReflectionMethodName<T>(object value) { }
        /// <summary>
        /// 创建快照克隆接口节点方法信息
        /// </summary>
        internal static readonly MethodInfo CreateMethod = typeof(SnapshotCloneNode).GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.Public).notNull();
    }
#endif
    /// <summary>
    /// 快照克隆接口节点
    /// </summary>
    /// <typeparam name="T">快照数据类型</typeparam>
    internal sealed class SnapshotCloneNode<T> : SnapshotNode<T>
        where T : SnapshotCloneObject<T>
    {
        /// <summary>
        /// 快照克隆接口节点
        /// </summary>
        /// <param name="target">操作节点对象</param>
        internal SnapshotCloneNode(ISnapshot<T> target) : base(target) { }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        internal override void GetResult()
        {
            base.GetResult();
            int count = result.Count;
            if (count != 0)
            {
                foreach (T value in array)
                {
                    value.SnapshotValue = value;
                    if (--count == 0) break;
                }
            }
            count = result.Array.Length;
            if (count != 0)
            {
                foreach (T value in result.Array.Array)
                {
                    value.SnapshotValue = value;
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 持久化重建
        /// </summary>
        /// <param name="rebuilder"></param>
        /// <returns></returns>
        internal override bool Rebuild(PersistenceRebuilder rebuilder)
        {
            LeftArray<T> array, newArray;
            rebuild(out array, out newArray);
            return rebuilder.RebuildSnapshotClone(ref array, ref newArray);
        }
    }
}
