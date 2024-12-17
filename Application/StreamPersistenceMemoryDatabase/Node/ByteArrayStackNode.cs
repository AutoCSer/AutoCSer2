using AutoCSer.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 栈节点（后进先出）
    /// </summary>
    public class ByteArrayStackNode : IByteArrayStackNode
#if NetStandard21
        , ISnapshot<byte[]?>
#else
        , ISnapshot<byte[]>
#endif
    {
        /// <summary>
        /// 栈
        /// </summary>
#if NetStandard21
        private Stack<byte[]?> stack;
#else
        private Stack<byte[]> stack;
#endif
        /// <summary>
        /// 栈节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public ByteArrayStackNode(int capacity)
        {
#if NetStandard21
            stack = new Stack<byte[]?>(capacity);
#else
            stack = new Stack<byte[]>(capacity);
#endif
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
#if NetStandard21
        public LeftArray<byte[]?> GetSnapshotArray()
#else
        public LeftArray<byte[]> GetSnapshotArray()
#endif
        {
#if NetStandard21
            return new LeftArray<byte[]?>(stack.ToArray());
#else
            return new LeftArray<byte[]>(stack.ToArray());
#endif
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(byte[] value)
        {
            stack.Push(value);
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return stack.Count;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            stack.Clear();
        }
        /// <summary>
        /// 将数据添加到栈
        /// </summary>
        /// <param name="value"></param>
        public void Push(ServerByteArray value)
        {
            stack.Push(value);
        }
        /// <summary>
        /// 从栈中弹出一个数据 持久化参数检查
        /// </summary>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<byte[]>> TryPopBeforePersistence()
        {
            if (stack.Count != 0) return default(ValueResult<ValueResult<byte[]>>);
            return default(ValueResult<byte[]>);
        }
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public ValueResult<byte[]?> TryPop()
#else
        public ValueResult<byte[]> TryPop()
#endif
        {
            var value = default(byte[]);
            if (stack.TryPop(out value)) return value;
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// 从栈中弹出一个数据 持久化参数检查
        /// </summary>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ResponseParameter> TryPopResponseParameterBeforePersistence()
        {
            if (stack.Count != 0) return default(ValueResult<ResponseParameter>);
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        public ResponseParameter TryPopResponseParameter()
        {
            var value = default(byte[]);
            if (stack.TryPop(out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public ValueResult<byte[]?> TryPeek()
#else
        public ValueResult<byte[]> TryPeek()
#endif
        {
            var value = default(byte[]);
            if (stack.TryPeek(out value)) return value;
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        public ResponseParameter TryPeekResponseParameter()
        {
            var value = default(byte[]);
            if (stack.TryPeek(out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
    }
}
