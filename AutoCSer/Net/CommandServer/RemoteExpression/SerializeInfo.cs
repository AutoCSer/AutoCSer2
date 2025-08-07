using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式序列化信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct SerializeInfo
    {
        /// <summary>
        /// 常量参数数量
        /// </summary>
        internal readonly int ConstantParameterCount;
        /// <summary>
        /// 序列化数据哈希值
        /// </summary>
        internal uint HashCode;
        /// <summary>
        /// 远程表达式关键字
        /// </summary>
        internal HashBuffer Key;
        /// <summary>
        /// 数据结束位置
        /// </summary>
        internal readonly byte* End;
        /// <summary>
        /// 远程表达式序列化信息
        /// </summary>
        /// <param name="deserializer"></param>
        internal SerializeInfo(BinaryDeserializer deserializer)
        {
            byte* current = deserializer.Current, end = deserializer.End, start = current;
            uint expressionSize = *(uint*)current;
            if ((expressionSize & 0x80000003) == 0 && (current += ((long)expressionSize + sizeof(int))) < end)
            {
                ConstantParameterCount = *(int*)current;
                if ((uint)ConstantParameterCount < 0x10000 && (current += sizeof(int)) < end)
                {
                    if (ConstantParameterCount != 0)
                    {
                        uint constantParameterSize = *(uint*)current;
                        if ((constantParameterSize & 0x80000003) == 0 && (current += ((long)constantParameterSize + sizeof(int))) < end)
                        {
                            HashCode = *(uint*)current;
                            if ((current += sizeof(int)) <= end)
                            {
                                Key = new HashBuffer(start, (int)(current - start) - sizeof(int), (int)HashCode);
                                End = current;
                                return;
                            }
                        }
                    }
                    else
                    {
                        HashCode = *(uint*)current;
                        if ((current += sizeof(int)) <= end)
                        {
                            Key = new HashBuffer(start, (int)(current - start) - sizeof(int) * 2, (int)HashCode);
                            End = current;
                            return;
                        }
                    }
                }
            }
            ConstantParameterCount = -1;
            HashCode = 0;
            Key = default(HashBuffer);
            End = null;
            deserializer.SetIndexOutOfRange();
        }
        /// <summary>
        /// 设置关键字数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        internal void Set(byte* start, int size)
        {
            ulong hashCode64 = Key.Buffer.GetHashCode64(start, size);
            uint hashCode = (uint)(hashCode64 ^ (hashCode64 >> 32));
            HashCode = hashCode | hashCode.logicalInversion();
            Key.Buffer.CurrentIndex = (int)HashCode;
        }
    }
}
