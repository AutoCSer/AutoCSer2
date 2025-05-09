using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照接口节点
    /// </summary>
    public abstract class SnapshotNode
    {
        /// <summary>
        /// 自定义对象，用于预生成辅助数据
        /// </summary>
        protected object customObject;
        /// <summary>
        /// 快照接口节点
        /// </summary>
        protected SnapshotNode()
        {
            customObject = this;
        }
        /// <summary>
        /// 关闭重建
        /// </summary>
        internal abstract void Close();
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        internal abstract void GetArray();
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        internal abstract void GetResult();
        /// <summary>
        /// 持久化重建
        /// </summary>
        /// <param name="rebuilder"></param>
        /// <returns></returns>
        internal abstract bool Rebuild(PersistenceRebuilder rebuilder);

        /// <summary>
        /// 检查类型是否存在快照功能接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool Check(Type type)
        {
            foreach (Type checkType in type.GetInterfaces())
            {
                if (checkType.IsGenericType)
                {
                    Type snapshotType = checkType.GetGenericTypeDefinition();
                    if (snapshotType == typeof(ISnapshot<>) || snapshotType == typeof(IEnumerableSnapshot<>)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static object Create<T>(object target)
        {
            return new SnapshotNode<T>((ISnapshot<T>)target);
        }
        /// <summary>
        /// 创建快照接口节点方法信息
        /// </summary>
        internal static readonly MethodInfo CreateMethod = typeof(SnapshotNode).GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.Public).notNull();
    }
    /// <summary>
    /// 快照接口节点
    /// </summary>
    /// <typeparam name="T">快照数据类型</typeparam>
    internal class SnapshotNode<T> : SnapshotNode
    {
        /// <summary>
        /// 操作节点对象
        /// </summary>
        private readonly ISnapshot<T> target;
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        protected T[] array;
        /// <summary>
        /// 快照数据信息
        /// </summary>
        protected SnapshotResult<T> result;
        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target">操作节点对象</param>
        internal SnapshotNode(ISnapshot<T> target)
        {
            this.target = target;
            array = EmptyArray<T>.Array;
        }
        /// <summary>
        /// 关闭重建
        /// </summary>
        internal override void Close()
        {
            customObject = this;
            array = EmptyArray<T>.Array;
            result.Array.SetEmpty();
        }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        internal override void GetArray()
        {
            int capacity = target.GetSnapshotCapacity(ref customObject);
            if (capacity > array.Length) array = new T[capacity];
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        internal override void GetResult()
        {
            result = target.GetSnapshotResult(array, customObject);
            customObject = this;
        }
        /// <summary>
        /// 获取持久化重建数据
        /// </summary>
        /// <param name="array"></param>
        /// <param name="newArray"></param>
        protected void rebuild(out LeftArray<T> array, out LeftArray<T> newArray)
        {
            array = new LeftArray<T>(result.Count, this.array);
            newArray = result.Array;
            this.array = EmptyArray<T>.Array;
            result.Array.SetEmpty();
            target.SetSnapshotResult(ref array, ref newArray);
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
            return rebuilder.Rebuild(ref array, ref newArray);
        }
    }
}
