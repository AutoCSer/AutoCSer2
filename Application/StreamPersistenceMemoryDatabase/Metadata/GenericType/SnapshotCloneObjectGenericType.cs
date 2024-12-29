using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class SnapshotCloneObjectGenericType : AutoCSer.Metadata.GenericTypeCache<SnapshotCloneObjectGenericType>
    {
        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal abstract SnapshotNode CreateSnapshotCloneNode(object target);

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static SnapshotCloneObjectGenericType create<T>()
            where T : SnapshotCloneObject<T>
        {
            return new SnapshotCloneObjectGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static SnapshotCloneObjectGenericType? lastGenericType;
#else
        protected static SnapshotCloneObjectGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SnapshotCloneObjectGenericType Get(Type type)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SnapshotCloneObjectGenericType<T> : SnapshotCloneObjectGenericType
        where T : SnapshotCloneObject<T>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal override SnapshotNode CreateSnapshotCloneNode(object target)
        {
            return new SnapshotCloneNode<T>((ISnapshot<T>)target);
        }
    }
}
