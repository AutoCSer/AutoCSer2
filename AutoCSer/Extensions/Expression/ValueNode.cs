using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 取值表达式节点
    /// </summary>
    internal class ValueNode
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        private readonly NodeTypeEnum nodeType;
        /// <summary>
        /// 取值类型
        /// </summary>
        internal readonly ValueTypeEnum ValueType;
        /// <summary>
        /// 成员回溯深度
        /// </summary>
        internal readonly byte MemberDepth;
        /// <summary>
        /// The next node
        /// </summary>
#if NetStandard21
        internal ValueNode? Next;
#else
        internal ValueNode Next;
#endif
        /// <summary>
        /// 空表达式节点
        /// </summary>
        internal ValueNode() { }
        /// <summary>
        /// 取值表达式节点
        /// </summary>
        /// <param name="valueType">取值类型</param>
        /// <param name="next">下一个节点</param>
        /// <param name="memberDepth">成员回溯深度</param>
#if NetStandard21
        internal ValueNode(ValueTypeEnum valueType, ValueNode? next = null, byte memberDepth = 0)
#else
        internal ValueNode(ValueTypeEnum valueType, ValueNode next = null, byte memberDepth = 0)
#endif
        {
            this.ValueType = valueType;
            this.Next = next;
            MemberDepth = memberDepth;
        }
        /// <summary>
        /// 取值表达式节点
        /// </summary>
        /// <param name="nodeType">节点类型</param>
        /// <param name="valueType">取值类型</param>
        /// <param name="next">下一个节点</param>
        /// <param name="memberDepth">成员回溯深度</param>
#if NetStandard21
        internal ValueNode(NodeTypeEnum nodeType, ValueTypeEnum valueType, ValueNode? next = null, byte memberDepth = 0)
#else
        internal ValueNode(NodeTypeEnum nodeType, ValueTypeEnum valueType, ValueNode next = null, byte memberDepth = 0)
#endif
        {
            this.nodeType = nodeType;
            this.ValueType = valueType;
            MemberDepth = memberDepth;
            this.Next = next;
        }
        /// <summary>
        /// 强制检查是否存在下一个节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ValueNode? CheckNext()
#else
        internal ValueNode CheckNext()
#endif
        {
            return Next != null ? this : null;
        }
        /// <summary>
        /// 检查下一个节点是否合法
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckNextNull()
        {
            if (Next != null)
            {
                if (object.ReferenceEquals(Next, Null)) Next = null;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置下一个节点
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal bool SetNext(ValueNode? next)
#else
        internal bool SetNext(ValueNode next)
#endif
        {
            if (next != null)
            {
                if (!object.ReferenceEquals(next, Null)) Next = next;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 空表达式节点
        /// </summary>
        internal static readonly ValueNode Null = new ValueNode();
    }
}
