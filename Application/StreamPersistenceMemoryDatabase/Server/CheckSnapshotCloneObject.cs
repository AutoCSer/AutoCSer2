using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 获取快照对象之前检查快照对象
    /// </summary>
    /// <typeparam name="T">快照对象类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CheckSnapshotCloneObject<T> where T : SnapshotCloneObject<T>
    {
        /// <summary>
        /// 快照数据对象
        /// </summary>
#if NetStandard21
        private readonly T? value;
#else
        private readonly T value;
#endif
        /// <summary>
        /// 获取快照对象之前检查快照对象
        /// </summary>
        /// <param name="value">快照数据对象</param>
#if NetStandard21
        private CheckSnapshotCloneObject(T? value)
#else
        private CheckSnapshotCloneObject(T value)
#endif
        {
            this.value = value;
        }
        /// <summary>
        /// 获取快照对象之前检查快照对象
        /// </summary>
        /// <returns>快照数据对象</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? Get()
#else
        public T Get()
#endif
        {
            if (value != null)
            {
                value.CheckSnapshotValue();
                return value;
            }
            return null;
        }
        /// <summary>
        /// 获取快照对象之前检查快照对象
        /// </summary>
        /// <returns>快照数据对象</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetNotNull()
        {
            T value = this.value.notNull();
            value.CheckSnapshotValue();
            return value;
        }
        /// <summary>
        /// 获取快照对象不检查快照对象状态，强制获取数据对象，仅用于创建快照对象集合与非持久化 API 逻辑，持久化 API 逻辑中应该使用 Get 方法）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? NoCheck()
#else
        public T NoCheck()
#endif
        {
            return value;
        }
        /// <summary>
        /// 获取快照对象不检查快照对象状态，强制获取数据对象，仅用于创建快照对象集合与非持久化 API 逻辑，持久化 API 逻辑中应该使用 Get 方法）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T NoCheckNotNull()
        {
            return value.notNull();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator CheckSnapshotCloneObject<T>(T? value) { return new CheckSnapshotCloneObject<T>(value); }
#else
        public static implicit operator CheckSnapshotCloneObject<T>(T value) { return new CheckSnapshotCloneObject<T>(value); }
#endif
        /// <summary>
        /// 隐式转换（获取快照对象之前检查快照对象）
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator T?(CheckSnapshotCloneObject<T> value) { return value.Get(); }
#else
        public static implicit operator T(CheckSnapshotCloneObject<T> value) { return value.Get(); }
#endif
    }
}
