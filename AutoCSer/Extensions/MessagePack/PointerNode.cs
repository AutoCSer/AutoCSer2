using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using AutoCSer.Extensions;

namespace AutoCSer.MessagePack
{
    /// <summary>
    /// 数据节点 https://github.com/msgpack/msgpack/blob/master/spec.md
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct PointerNode
    {
        /// <summary>
        /// 节点数据起始位置，null 表示解析失败
        /// </summary>
        public readonly byte* Start;
        /// <summary>
        /// 数据结束位置，null 表示解析失败
        /// </summary>
        private readonly byte* End;
        /// <summary>
        /// 节点数据类型
        /// </summary>
        public DataTypeEnum DataType
        {
            get
            {
                if (Start != null)
                {
                    switch (*Start - 0xC0)
                    {
                        case 0xC0 - 0xC0: return DataTypeEnum.Null;
                        case 0xC1 - 0xC0: return DataTypeEnum.Reserved;
                        case 0xC2 - 0xC0:
                        case 0xC3 - 0xC0:
                            return DataTypeEnum.Bool;
                        case 0xC4 - 0xC0:
                        case 0xC5 - 0xC0:
                        case 0xC6 - 0xC0:
                        case 0xD9 - 0xC0:
                        case 0xDA - 0xC0:
                        case 0xDB - 0xC0:
                            return DataTypeEnum.Memory;
                        case 0xC7 - 0xC0:
                        case 0xC8 - 0xC0:
                        case 0xC9 - 0xC0:
                        case 0xD4 - 0xC0:
                        case 0xD5 - 0xC0:
                        case 0xD6 - 0xC0:
                        case 0xD7 - 0xC0:
                        case 0xD8 - 0xC0:
                            return DataTypeEnum.Extension;
                        case 0xCA - 0xC0: return DataTypeEnum.Float;
                        case 0xCB - 0xC0: return DataTypeEnum.Double;
                        case 0xCC - 0xC0:
                        case 0xCD - 0xC0:
                        case 0xCE - 0xC0:
                        case 0xCF - 0xC0:
                        case 0xD0 - 0xC0:
                        case 0xD1 - 0xC0:
                        case 0xD2 - 0xC0:
                        case 0xD3 - 0xC0:
                            return DataTypeEnum.Integer;
                        case 0xDC - 0xC0:
                        case 0xDD - 0xC0:
                            return DataTypeEnum.Array;
                        case 0xDE - 0xC0:
                        case 0xDF - 0xC0:
                            return DataTypeEnum.Map;
                    }
                    switch ((*Start >> 4) - 0x8)
                    {
                        case 0x8 - 0x8: return DataTypeEnum.Map;
                        case 0x9 - 0x8: return DataTypeEnum.Array;
                        case 0xA - 0x8:
                        case 0xB - 0x8:
                            return DataTypeEnum.Memory;
                    }
                    return DataTypeEnum.Integer;
                }
                return DataTypeEnum.Error;
            }
        }
        /// <summary>
        /// 获取元素数量（数组、K-V、内存数据块）
        /// </summary>
        public int ItemCount
        {
            get
            {
                switch (DataType)
                {
                    case DataTypeEnum.Array:
                        if ((*Start & 0xf0) == 0x90) return *Start & 0xf;
                        switch (*Start - 0xC0)
                        {
                            case 0xDC - 0xC0: return (int)AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1);
                            case 0xDD - 0xC0: return (int)AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1);
                        }
                        break;
                    case DataTypeEnum.Map:
                        if ((*Start & 0xf0) == 0x80) return *Start & 0xf;
                        switch (*Start - 0xC0)
                        {
                            case 0xDE - 0xC0: return (int)AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1);
                            case 0xDF - 0xC0: return (int)AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1);
                        }
                        break;
                    case DataTypeEnum.Memory:
                        switch (*Start - 0xC0)
                        {
                            case 0xC4 - 0xC0:
                            case 0xD9 - 0xC0:
                                return Start[1];
                            case 0xC5 - 0xC0:
                            case 0xDA - 0xC0:
                                return (int)AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1);
                            case 0xC6 - 0xC0:
                            case 0xDB - 0xC0:
                                return (int)AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1);
                        }
                        switch ((*Start >> 4) - 0xA)
                        {
                            case 0xA - 0xA:
                            case 0xB - 0xA:
                                return *Start & 0x1f;
                        }
                        break;
                }
                return -1;
            }
        }
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="start">节点数据起始位置</param>
        /// <param name="end">数据最大结束位置</param>
        private PointerNode(byte* start, byte* end)
        {
            Start = start;
            End = end;
        }
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="start">节点数据起始位置</param>
        /// <param name="end">数据最大结束位置</param>
        private PointerNode(ref AutoCSer.Memory.Pointer start, ref AutoCSer.Memory.Pointer end)
        {
            this.Start = start.Byte;
            this.End = end.Byte;
        }
        /// <summary>
        /// 忽略数据并返回数据结束位置
        /// </summary>
        /// <param name="start">节点数据起始位置</param>
        /// <returns>数据结束位置</returns>
        private byte* ignore(byte* start)
        {
            switch (*start - 0xC0)
            {
                case 0xC4 - 0xC0:
                case 0xD9 - 0xC0:
                    return start + (start[1] + (1 + 1));
                case 0xC5 - 0xC0:
                case 0xDA - 0xC0:
                    return start + (AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(start + 1) + (1 + sizeof(ushort)));
                case 0xC6 - 0xC0:
                case 0xDB - 0xC0:
                    return start + AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(start + 1) + (1 + sizeof(uint));
                case 0xC7 - 0xC0: return start + (start[1] + (2 + 1));
                case 0xC8 - 0xC0: return start + (AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(start + 1) + (2 + sizeof(ushort)));
                case 0xC9 - 0xC0: return start + (AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(start + 1) + (2 + sizeof(uint)));
                case 0xCA - 0xC0: return start + (1 + sizeof(float));
                case 0xCB - 0xC0: return start + (1 + sizeof(double));
                case 0xCC - 0xC0: return start + (1 + sizeof(byte));
                case 0xCD - 0xC0: return start + (1 + sizeof(ushort));
                case 0xCE - 0xC0: return start + (1 + sizeof(uint));
                case 0xCF - 0xC0: return start + (1 + sizeof(ulong));
                case 0xD0 - 0xC0: return start + (1 + sizeof(sbyte));
                case 0xD1 - 0xC0: return start + (1 + sizeof(short));
                case 0xD2 - 0xC0: return start + (1 + sizeof(int));
                case 0xD3 - 0xC0: return start + (1 + sizeof(long));
                case 0xD4 - 0xC0: return start + (1 + 1 + 1);
                case 0xD5 - 0xC0: return start + (1 + 1 + 2);
                case 0xD6 - 0xC0: return start + (1 + 1 + 4);
                case 0xD7 - 0xC0: return start + (1 + 1 + 8);
                case 0xD8 - 0xC0: return start + (1 + 1 + 16);
                case 0xDC - 0xC0: return ignoreArray(start + (1 + sizeof(ushort)), AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(start + 1));
                case 0xDD - 0xC0: return ignoreArray(start + (1 + sizeof(uint)), AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(start + 1));
                case 0xDE - 0xC0: return ignoreMap(start + (1 + sizeof(ushort)), AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(start + 1));
                case 0xDF - 0xC0: return ignoreMap(start + (1 + sizeof(uint)), AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(start + 1));
            }
            switch ((*start >> 4) - 0x8)
            {
                case 0x8 - 0x8: return ignoreMap(start + 1, (uint)*start & 0xf);
                case 0x9 - 0x8: return ignoreArray(start + 1, (uint)*start & 0xf);
                case 0xA - 0x8:
                case 0xB - 0x8:
                    return start + (1 + (*start & 0x1f));
            }
            return start + 1;
        }
        /// <summary>
        /// 忽略数组
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte* ignoreArray(byte* start, uint size)
        {
            while (size != 0)
            {
                start = ignore(start);
                if (start <= End) --size;
                else break;
            }
            return start;
        }
        /// <summary>
        /// 忽略键值对
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte* ignoreMap(byte* start, uint size)
        {
            while (size != 0)
            {
                start = ignore(start);
                if (start <= End)
                {
                    start = ignore(start);
                    if (start <= End) --size;
                    else break;
                }
                else break;
            }
            return start;
        }
        /// <summary>
        /// 忽略数据并返回数据结束位置
        /// </summary>
        /// <param name="start">节点数据起始位置</param>
        /// <returns>数据结束位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private AutoCSer.Memory.Pointer ignore(ref AutoCSer.Memory.Pointer start)
        {
            return new AutoCSer.Memory.Pointer(ignore(start.Byte), 0);
        }
        /// <summary>
        /// 获取数组元素集合
        /// </summary>
#if NetStandard21
        public IEnumerable<PointerNode>? Array
#else
        public IEnumerable<PointerNode> Array
#endif
        {
            get
            {
                if ((*Start & 0xf0) == 0x90) return getArray(new AutoCSer.Memory.Pointer(Start + 1, 0), (uint)*Start & 0xf);
                switch (*Start - 0xC0)
                {
                    case 0xC0 - 0xC0: return null;
                    case 0xDC - 0xC0: return getArray(new AutoCSer.Memory.Pointer(Start + (1 + sizeof(ushort)), 0), AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1));
                    case 0xDD - 0xC0: return getArray(new AutoCSer.Memory.Pointer(Start + (1 + sizeof(uint)), 0), AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1));
                }
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 获取数组元素集合
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private IEnumerable<PointerNode> getArray(AutoCSer.Memory.Pointer start, uint size)
        {
            while (size != 0)
            {
                AutoCSer.Memory.Pointer end = ignore(ref start);
                yield return new PointerNode(ref start, ref end);
                start = end;
                --size;
            }
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public PointerNode[]? GetArray()
#else
        public PointerNode[] GetArray()
#endif
        {
            if ((*Start & 0xf0) == 0x90) return getArray(Start + 1, (uint)*Start & 0xf);
            switch (*Start - 0xC0)
            {
                case 0xC0 - 0xC0: return null;
                case 0xDC - 0xC0: return getArray(Start + (1 + sizeof(ushort)), AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1));
                case 0xDD - 0xC0: return getArray(Start + (1 + sizeof(uint)), AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1));
            }
            throw new InvalidCastException();
        }
        /// <summary>
        /// 获取数组元素集合
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private PointerNode[] getArray(byte* start, uint size)
        {
            if (size != 0)
            {
                PointerNode[] array = new PointerNode[size];
                for (int index = 0; index != size; ++index)
                {
                    byte* end = ignore(start);
                    array[index] = new PointerNode(start, end);
                    start = end;
                }
                return array;
            }
            return EmptyArray<PointerNode>.Array;
        }
        /// <summary>
        /// 获取 Map 元素集合
        /// </summary>
#if NetStandard21
        public IEnumerable<KeyValue<PointerNode, PointerNode>>? Map
#else
        public IEnumerable<KeyValue<PointerNode, PointerNode>> Map
#endif
        {
            get
            {
                if ((*Start & 0xf0) == 0x80) return getMap(new AutoCSer.Memory.Pointer(Start + 1, 0), (uint)*Start & 0xf);
                switch (*Start - 0xC0)
                {
                    case 0xC0 - 0xC0: return null;
                    case 0xDE - 0xC0: return getMap(new AutoCSer.Memory.Pointer(Start + (1 + sizeof(ushort)), 0), AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1));
                    case 0xDF - 0xC0: return getMap(new AutoCSer.Memory.Pointer(Start + (1 + sizeof(uint)), 0), AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1));
                }
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 获取 Map 元素集合
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private IEnumerable<KeyValue<PointerNode, PointerNode>> getMap(AutoCSer.Memory.Pointer start, uint size)
        {
            while (size != 0)
            {
                AutoCSer.Memory.Pointer keyEnd = ignore(ref start), valueEnd = ignore(ref keyEnd);
                yield return new KeyValue<PointerNode, PointerNode>(new PointerNode(ref start, ref keyEnd), new PointerNode(ref keyEnd, ref valueEnd));
                start = valueEnd;
                --size;
            }
        }
        /// <summary>
        /// 获取 Map 元素数组
        /// </summary>
#if NetStandard21
        public KeyValue<PointerNode, PointerNode>[]? GetMap()
#else
        public KeyValue<PointerNode, PointerNode>[] GetMap()
#endif
        {
            if ((*Start & 0xf0) == 0x80) return getMap(Start + 1, (uint)*Start & 0xf);
            switch (*Start - 0xC0)
            {
                case 0xC0 - 0xC0: return null;
                case 0xDE - 0xC0: return getMap(Start + (1 + sizeof(ushort)), AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1));
                case 0xDF - 0xC0: return getMap(Start + (1 + sizeof(uint)), AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1));
            }
            throw new InvalidCastException();
        }
        /// <summary>
        /// 获取 Map 元素数组
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private KeyValue<PointerNode, PointerNode>[] getMap(byte* start, uint size)
        {
            if (size != 0)
            {
                KeyValue<PointerNode, PointerNode>[] nodeArray = new KeyValue<PointerNode, PointerNode>[size];
                for (uint index = 0; index != size; ++index)
                {
                    byte* keyEnd = ignore(start), valueEnd = ignore(keyEnd);
                    nodeArray[index].Set(new PointerNode(start, keyEnd), new PointerNode(keyEnd, valueEnd));
                    start = valueEnd;
                }
                return nodeArray;
            }
            return EmptyArray<KeyValue<PointerNode, PointerNode>>.Array;
        }
        /// <summary>
        /// 内存数据块
        /// </summary>
        public AutoCSer.Memory.Pointer Memory
        {
            get
            {
                switch (*Start - 0xC0)
                {
                    case 0xC0 - 0xC0: return default(AutoCSer.Memory.Pointer);
                    case 0xC4 - 0xC0:
                    case 0xD9 - 0xC0:
                        return new AutoCSer.Memory.Pointer(Start + 2, Start[1]);
                    case 0xC5 - 0xC0:
                    case 0xDA - 0xC0:
                        return new AutoCSer.Memory.Pointer(Start + (1 + sizeof(ushort)), (int)AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1));
                    case 0xC6 - 0xC0:
                    case 0xDB - 0xC0:
                        return new AutoCSer.Memory.Pointer(Start + (1 + sizeof(uint)), (int)AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1));
                }
                switch ((*Start >> 4) - 0xA)
                {
                    case 0xA - 0xA:
                    case 0xB - 0xA:
                        return new AutoCSer.Memory.Pointer(Start + 1, *Start & 0x1f);
                }
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 字节数组
        /// </summary>
#if NetStandard21
        public byte[]? ByteArray
#else
        public byte[] ByteArray
#endif
        {
            get
            {
                AutoCSer.Memory.Pointer memory = Memory;
                if (memory.ByteSize > 0)
                {
                    byte[] data = AutoCSer.Common.GetUninitializedArray<byte>(memory.ByteSize);
                    AutoCSer.Common.CopyTo(memory.Data, data, 0, memory.ByteSize);
                    return data;
                }
                return memory.Data != null ? EmptyArray<byte>.Array : null;
            }
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
#if NetStandard21
        public unsafe string? GetString(Encoding encoding)
#else
        public unsafe string GetString(Encoding encoding)
#endif
        {
            AutoCSer.Memory.Pointer memory = Memory;
            if (memory.ByteSize > 0) return encoding.GetString(memory.Byte, memory.ByteSize);
            return memory.Data == null ? null : string.Empty;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>整数</returns>
        public static implicit operator long(PointerNode node) { return node.getLong(); }
        /// <summary>
        /// 整数
        /// </summary>
        private long getLong()
        {
            switch (*Start - 0xCC)
            {
                case 0xCC - 0xCC: return *(byte*)(Start + 1);
                case 0xCD - 0xCC: return AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1);
                case 0xCE - 0xCC: return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1);
                case 0xCF - 0xCC: return (long)AutoCSer.Extensions.Memory.Common.GetULongBigEndian(Start + 1);
                case 0xD0 - 0xCC: return *(sbyte*)(Start + 1);
                case 0xD1 - 0xCC: return AutoCSer.Extensions.Memory.Common.GetShortBigEndian(Start + 1);
                case 0xD2 - 0xCC: return AutoCSer.Extensions.Memory.Common.GetIntBigEndian(Start + 1);
                case 0xD3 - 0xCC: return AutoCSer.Extensions.Memory.Common.GetLongBigEndian(Start + 1);
            }
            if ((*Start & 0x80) == 0) return *Start;
            if ((*Start & 0xE0) == 0xE0) return *(sbyte*)Start;
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>无符号整数</returns>
        public static implicit operator ulong(PointerNode node) { return node.getULong(); }
        /// <summary>
        /// 无符号整数
        /// </summary>
        private ulong getULong()
        {
            switch (*Start - 0xCC)
            {
                case 0xCC - 0xCC: return *(byte*)(Start + 1);
                case 0xCD - 0xCC: return AutoCSer.Extensions.Memory.Common.GetUShortBigEndian(Start + 1);
                case 0xCE - 0xCC: return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(Start + 1);
                case 0xCF - 0xCC: return AutoCSer.Extensions.Memory.Common.GetULongBigEndian(Start + 1);
                case 0xD0 - 0xCC: return (ulong)(long)*(sbyte*)(Start + 1);
                case 0xD1 - 0xCC: return (ulong)(long)AutoCSer.Extensions.Memory.Common.GetShortBigEndian(Start + 1);
                case 0xD2 - 0xCC: return (ulong)(long)AutoCSer.Extensions.Memory.Common.GetIntBigEndian(Start + 1);
                case 0xD3 - 0xCC: return (ulong)AutoCSer.Extensions.Memory.Common.GetLongBigEndian(Start + 1);
            }
            if ((*Start & 0x80) == 0) return *Start;
            if ((*Start & 0xE0) == 0xE0) return (ulong)(long)*(sbyte*)Start;
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>逻辑值</returns>
        public static implicit operator bool(PointerNode node) { return node.getBool(); }
        /// <summary>
        /// 逻辑值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool getBool()
        {
            switch (*Start - 0xC2)
            {
                case 0xC2 - 0xC2: return false;
                case 0xC3 - 0xC2: return true;
            }
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>浮点数</returns>
        public static implicit operator float(PointerNode node) { return node.getFloat(); }
        /// <summary>
        /// 浮点数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private float getFloat()
        {
            if (*Start == 0xCA) return *(float*)(Start + 1);
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="node">数据节点</param>
        /// <returns>双精度浮点数</returns>
        public static implicit operator double(PointerNode node) { return node.getDouble(); }
        /// <summary>
        /// 双精度浮点数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private double getDouble()
        {
            switch (*Start - 0xCA)
            {
                case 0xCA - 0xCA: return *(float*)(Start + 1);
                case 0xCB - 0xCA: return *(double*)(Start + 1);
            }
            throw new InvalidCastException();
        }
        /// <summary>
        /// 获取数据字节数组
        /// </summary>
        public byte[] GetData()
        {
            byte[] data = AutoCSer.Common.GetUninitializedArray<byte>((int)(End - Start));
            AutoCSer.Common.CopyTo(Start, data, 0, data.Length);
            return data;
        }

        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <param name="start">节点数据起始位置</param>
        /// <param name="end">数据结束位置</param>
        /// <param name="isFull">默认为 true 表示完整匹配结束位置</param>
        /// <returns>失败则指针为 null</returns>
        public static PointerNode Create(byte* start, byte* end, bool isFull = true)
        {
            PointerNode node = new PointerNode(start, end);
            byte* nodeEnd = node.ignore(start);
            if (nodeEnd <= end)
            {
                if (nodeEnd == end || !isFull) return new PointerNode(start, nodeEnd);
            }
            return default(PointerNode);
        }
    }
}
