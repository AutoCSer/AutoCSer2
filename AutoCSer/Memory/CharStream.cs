using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 内存字符流
    /// </summary>
    public sealed unsafe class CharStream : UnmanagedStreamBase
    {
        /// <summary>
        /// 数据
        /// </summary>
        public char* Char
        {
            get { return Data.Pointer.Char; }
        }
        /// <summary>
        /// 当前写入位置
        /// </summary>
        public char* CurrentChar
        {
            get { return (char*)Data.Pointer.Current; }
        }
        /// <summary>
        /// 当前数据长度
        /// </summary>
        public int Length
        {
            get { return Data.Pointer.CurrentIndex >> 1; }
        }
        /// <summary>
        /// 内存字符流
        /// </summary>
        /// <param name="unmanagedPool">非托管内存池</param>
        public CharStream(UnmanagedPool unmanagedPool) : base(unmanagedPool) { }
        /// <summary>
        /// 内存字符流
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isUnmanaged"></param>
        internal CharStream(ref UnmanagedPoolPointer data, bool isUnmanaged = false) : base(ref data, isUnmanaged) { }
        /// <summary>
        /// 内存字符流
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isUnmanaged"></param>
        internal CharStream(UnmanagedPoolPointer data, bool isUnmanaged = false) : base(ref data, isUnmanaged) { }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override unsafe string ToString()
        {
            return Data.Pointer.ToString();
        }
        /// <summary>
        /// 预增数据流字符长度
        /// </summary>
        /// <param name="size">增加字符长度</param>
        /// <returns>失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal char* GetPrepCharSizeCurrent(int size)
        {
            return PrepCharSize(size) ? (char*)Data.Pointer.Current : null;
        }
        /// <summary>
        /// 预增数据流字符长度
        /// </summary>
        /// <param name="size">增加字符长度</param>
        /// <returns>是否增加成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool PrepCharSize(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
#endif
            return PrepSize(size < int.MaxValue >> 1 ? size << 1 : (int.MinValue + 1));
        }
        /// <summary>
        /// 写字符串，适合零碎短小数据(不足8字节按8字节算)
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SimpleWrite(string value)
        {
            if (PrepSize((value.Length << 1) + 6)) Data.Pointer.SimpleWrite(value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(string value)
        {
            if (value != null) WriteNotNull(value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        internal void WriteNotNull(string value)
        {
            int length = value.Length;
            if (length != 0 && PrepSize(length * sizeof(char))) Data.Pointer.Write(value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteNotEmpty(string value)
        {
            if (PrepSize(value.Length * sizeof(char))) Data.Pointer.Write(value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="index">起始位置</param>
        /// <param name="size">长度必须大于0</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(string value, int index, int size)
        {
            if (PrepSize(size * sizeof(char))) Data.Pointer.Write(value, index, size);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(SubString value)
        {
            int length = value.Length;
            if (length != 0) Write(value.String.notNull(), value.Start, length);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(ref SubString value)
        {
            int length = value.Length;
            if (length != 0) Write(value.String.notNull(), value.Start, length);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        internal void Write(char* value, int size)
        {
            if (size != 0)
            {
                size <<= 1;
                byte* data = GetBeforeMove(size);
                if (data != null) AutoCSer.Common.CopyTo(value, data, size);
            }
        }

        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(byte value)
        {
            char* start = GetPrepCharSizeCurrent(3);
            if (start != null) Data.Pointer.MoveSize(AutoCSer.Extensions.NumberExtension.ToString(value, start) << 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(sbyte value)
        {
            char* start = GetPrepCharSizeCurrent(4);
            if (start != null) Data.Pointer.MoveSize(AutoCSer.Extensions.NumberExtension.ToString(value, start) << 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(ushort value)
        {
            char* start = GetPrepCharSizeCurrent(5);
            if (start != null) Data.Pointer.MoveSize(AutoCSer.Extensions.NumberExtension.ToString(value, start) << 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(short value)
        {
            char* start = GetPrepCharSizeCurrent(6);
            if (start != null) Data.Pointer.MoveSize(AutoCSer.Extensions.NumberExtension.ToString(value, start) << 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(uint value)
        {
            char* start = GetPrepCharSizeCurrent(10 + 3);
            if (start != null) Data.Pointer.MoveSize(AutoCSer.Extensions.NumberExtension.ToString(value, start) << 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(int value)
        {
            char* start = GetPrepCharSizeCurrent(12 + 3);
            if (start != null) Data.Pointer.MoveSize(AutoCSer.Extensions.NumberExtension.ToString(value, start) << 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(ulong value)
        {
            if (PrepCharSize(20 + 3)) UnsafeToString(value);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void UnsafeToString(ulong value)
        {
            Data.Pointer.MoveSize(AutoCSer.Extensions.NumberExtension.ToString(value, CurrentChar) << 1);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(long value)
        {
            if (PrepCharSize(22 + 3)) UnsafeToString(value);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void UnsafeToString(long value)
        {
            Data.Pointer.MoveSize(AutoCSer.Extensions.NumberExtension.ToString(value, CurrentChar) << 1);
        }


        /// <summary>
        /// 输出 null 值
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteJsonNull()
        {
            Write(JsonDeserializer.NullStringValue);
        }
        /// <summary>
        /// 输出 null 值
        /// </summary>
        /// <param name="charStream"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void WriteJsonNull(CharStream charStream)
        {
            charStream.WriteJsonNull();
        }
        /// <summary>
        /// 输出空对象
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteJsonObject()
        {
            Write('{' + ('}' << 16));
        }
        /// <summary>
        /// 输出空数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteJsonArray()
        {
            Write('[' + (']' << 16));
        }
        /// <summary>
        /// 写入空字符串
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteJsonEmptyString()
        {
            Write('"' + ('"' << 16));
        }
        /// <summary>
        /// 输出对象字符串 [object Object]
        /// </summary>
        public void WriteJsonObjectString()
        {
            if (PrepSize(sizeof(ulong) * 4))
            {
                Data.Pointer.WriteSize('[' + ('o' << 16) + ((long)'b' << 32) + ((long)'j' << 48)
                    , 'e' + ('c' << 16) + ((long)'t' << 32) + ((long)' ' << 48)
                    , 'O' + ('b' << 16) + ((long)'j' << 32) + ((long)'e' << 48)
                    , 'c' + ('t' << 16) + ((long)']' << 32)
                    , 15 * sizeof(char));
            }
        }
        /// <summary>
        /// 预申请数组长度并写入数组开始符号 [
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteJsonArrayStart(int size)
        {
            if (PrepCharSize(size)) Data.Pointer.Write('[');
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        public void WriteJsonBool(bool value)
        {
            if (value) Write('t' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48));
            else if(PrepSize(5 * sizeof(char)))
            {
                Data.Pointer.Write('f' + ('a' << 16) + ((long)'l' << 32) + ((long)'s' << 48));
                Data.Pointer.Write('e');
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteJsonHex(byte value)
        {
            if (value < 10) Write((char)(value + '0'));
            else writeJsonHex(value, 4 * sizeof(char));
        }
        /// <summary>
        /// 数字转换成十六进制字符串 0x..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool writeJsonHex(uint value, int size)
        {
            byte* chars = GetPrepSizeCurrent(size);
            if (chars != null)
            {
                uint high = value >> 4;
                *(int*)chars = '0' + ('x' << 16);
                if (high != 0)
                {
                    *(uint*)(chars + sizeof(char) * 2) = AutoCSer.Extensions.NumberExtension.ToHex(high) + (AutoCSer.Extensions.NumberExtension.ToHex(value & 15) << 16);
                    Data.Pointer.CurrentIndex += 4 * sizeof(char);
                }
                else
                {
                    *(char*)(chars + sizeof(char) * 2) = (char)AutoCSer.Extensions.NumberExtension.ToHex(value);
                    Data.Pointer.CurrentIndex += 3 * sizeof(char);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        internal void WriteJsonHex(sbyte value)
        {
            if (value >= 0) WriteJsonHex((byte)value);
            else
            {
                WriteNegative(5);
                WriteJsonHex((byte)-value);
            }
        }
        /// <summary>
        /// 数字转换成十六进制字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void writeJsonHex2(uint value)
        {
            Data.Pointer.Write(AutoCSer.Extensions.NumberExtension.ToHex(value >> 4) + (AutoCSer.Extensions.NumberExtension.ToHex(value & 15) << 16));
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        internal void WriteJsonHex(ushort value)
        {
            uint high = (uint)value >> 8;
            if (high == 0) WriteJsonHex((byte)value);
            else if (writeJsonHex(high, 6 * sizeof(char))) writeJsonHex2((byte)value);
        }
        /// <summary>
        /// 数字转换成十六进制字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void writeJsonHex4(uint value)
        {
            writeJsonHex2(value >> 8);
            writeJsonHex2(value & 0xff);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        internal void WriteJsonHex(uint value)
        {
            uint high = value >> 16;
            if (high == 0) WriteJsonHex((ushort)value);
            else
            {
                uint highLeft = high >> 8;
                if (highLeft != 0)
                {
                    if (writeJsonHex(highLeft, 10 * sizeof(char)))
                    {
                        writeJsonHex2(high & 0xff);
                        writeJsonHex4(value & 0xffff);
                    }
                }
                else
                {
                    if (writeJsonHex(high, 8 * sizeof(char))) writeJsonHex4(value & 0xffff);
                }
            }
        }
        /// <summary>
        /// 数字转换成十六进制字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void writeJsonHex8(uint value)
        {
            writeJsonHex4(value >> 16);
            writeJsonHex4(value & 0xffff);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonHex(ulong value)
        {
            uint high = (uint)(value >> 32);
            if (high == 0) WriteJsonHex((uint)value);
            else if (high >= 0x100)
            {
                WriteJsonHex(high);
                if (PrepSize(8 * sizeof(char))) writeJsonHex8((uint)value);
            }
            else if (writeJsonHex(high, 12 * sizeof(char))) writeJsonHex8((uint)value);
        }
        /// <summary>
        /// 预增数据流字符长度并写入负号
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteNegative(int size)
        {
            if (PrepCharSize(size)) Data.Pointer.Write('-');
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonString(long value)
        {
            if (PrepCharSize(24 + 2))
            {
                Data.Pointer.Write('"');
                UnsafeToString(value);
                Data.Pointer.Write('"');
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonString(ulong value)
        {
            if (PrepCharSize(22 + 2))
            {
                Data.Pointer.Write('"');
                UnsafeToString(value);
                Data.Pointer.Write('"');
            }
        }
        /// <summary>
        /// 输出 float 值
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJson(float value)
        {
            if (!float.IsNaN(value) && !float.IsInfinity(value))
            {
                int size = JsonSerializer.CustomConfig.Write(this, value);
                if (size > 0) Data.Pointer.CheckMoveSize(size << 1);
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出 float 值
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonInfinity(float value)
        {
            if (!float.IsNaN(value))
            {
                if (!float.IsInfinity(value))
                {
                    int size = JsonSerializer.CustomConfig.Write(this, value);
                    if (size > 0) Data.Pointer.CheckMoveSize(size << 1);
                }
                else if (float.IsPositiveInfinity(value)) WritePositiveInfinity();
                else WriteNegativeInfinity();
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出 double 值
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJson(double value)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value))
            {
                if (value <= 1.7976931348623150E+308)
                {
                    if (value >= -1.7976931348623150E+308)
                    {
                        int size = JsonSerializer.CustomConfig.Write(this, value);
                        if (size > 0) Data.Pointer.CheckMoveSize(size << 1);
                    }
                    else writeDoubleMinValue(value);
                }
                else writeDoubleMaxValue(value);
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出 double 值
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonInfinity(double value)
        {
            if (!double.IsNaN(value))
            {
                if (!double.IsInfinity(value))
                {
                    if (value <= 1.7976931348623150E+308)
                    {
                        if (value >= -1.7976931348623150E+308)
                        {
                            int size = JsonSerializer.CustomConfig.Write(this, value);
                            if (size > 0) Data.Pointer.CheckMoveSize(size << 1);
                        }
                        else writeDoubleMinValue(value);
                    }
                    else writeDoubleMaxValue(value);
                }
                else if (double.IsPositiveInfinity(value)) WritePositiveInfinity();
                else WriteNegativeInfinity();
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出 double 最大值
        /// </summary>
        /// <param name="value"></param>
        private void writeDoubleMaxValue(double value)
        {
            byte* write = (byte*)GetPrepCharSizeCurrent(24);
            if (write != null)
            {
                //1.79 7693 1348 6231 57E+ 308
                *(long*)write = '1' + ('.' << 16) + ((long)'7' << 32) + ((long)'9' << 48);
                *(long*)(write + sizeof(long)) = '7' + ('6' << 16) + ((long)'9' << 32) + ((long)'3' << 48);
                *(long*)(write + sizeof(long) * 2) = '1' + ('3' << 16) + ((long)'4' << 32) + ((long)'8' << 48);
                *(long*)(write + sizeof(long) * 3) = '6' + ('2' << 16) + ((long)'3' << 32) + ((long)'1' << 48);

                if (value >= 1.7976931348623157E+308) *(long*)(write + sizeof(long) * 4) = '5' + ('7' << 16) + ((long)'E' << 32) + ((long)'+' << 48);
                else if (value >= 1.7976931348623154E+308)
                {
                    if (value >= 1.7976931348623155E+308) *(long*)(write + sizeof(long) * 4) = '5' + ('6' << 16) + ((long)'E' << 32) + ((long)'+' << 48);
                    else *(long*)(write + sizeof(long) * 4) = '5' + ('4' << 16) + ((long)'E' << 32) + ((long)'+' << 48);
                }
                else *(long*)(write + sizeof(long) * 4) = '5' + ('2' << 16) + ((long)'E' << 32) + ((long)'+' << 48);
                *(long*)(write + sizeof(long) * 5) = '3' + ('0' << 16) + ((long)'8' << 32);
                Data.Pointer.MoveSize(23 * sizeof(char));
            }
        }
        /// <summary>
        /// 输出 double 最大值
        /// </summary>
        /// <param name="value"></param>
        private void writeDoubleMinValue(double value)
        {
            byte* write = (byte*)GetBeforeMove(24 * sizeof(char));
            if (write != null)
            {
                //-1.7 9769 3134 8623 157E +308
                *(long*)write = '-' + ('1' << 16) + ((long)'.' << 32) + ((long)'7' << 48);
                *(long*)(write + sizeof(long)) = '9' + ('7' << 16) + ((long)'6' << 32) + ((long)'9' << 48);
                *(long*)(write + sizeof(long) * 2) = '3' + ('1' << 16) + ((long)'3' << 32) + ((long)'4' << 48);
                *(long*)(write + sizeof(long) * 3) = '8' + ('6' << 16) + ((long)'2' << 32) + ((long)'3' << 48);

                if (value <= -1.7976931348623157E+308) *(long*)(write + sizeof(long) * 4) = '1' + ('5' << 16) + ((long)'7' << 32) + ((long)'E' << 48);
                else if (value <= -1.7976931348623154E+308)
                {
                    if (value <= -1.7976931348623155E+308) *(long*)(write + sizeof(long) * 4) = '1' + ('5' << 16) + ((long)'6' << 32) + ((long)'E' << 48);
                    else *(long*)(write + sizeof(long) * 4) = '1' + ('5' << 16) + ((long)'4' << 32) + ((long)'E' << 48);
                }
                else *(long*)(write + sizeof(long) * 4) = '1' + ('5' << 16) + ((long)'2' << 32) + ((long)'E' << 48);
                *(long*)(write + sizeof(long) * 5) = '+' + ('3' << 16) + ((long)'0' << 32) + ((long)'8' << 48);
            }
        }
        /// <summary>
        /// 输出非数字值
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteJsonNaN()
        {
            if (PrepSize(sizeof(ulong))) Data.Pointer.WriteSize('N' + ('a' << 16) + ((long)'N' << 32), 3 * sizeof(char));
        }
        /// <summary>
        /// 输出正无穷
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WritePositiveInfinity()
        {
            if (PrepSize(8 * sizeof(char))) writeInfinity();
        }
        /// <summary>
        /// 输出负无穷
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteNegativeInfinity()
        {
            if (PrepSize(9 * sizeof(char)))
            {
                Data.Pointer.Write('-');
                writeInfinity();
            }
        }
        /// <summary>
        /// 输出无穷
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void writeInfinity()
        {//Infinity
            Data.Pointer.Write('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48), 'n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48));
        }
        /// <summary>
        /// Guid转换成字符串（单引号）
        /// </summary>
        /// <param name="value">Guid</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteString(System.Guid value)
        {
            if (PrepSize(38 * sizeof(char))) Data.Pointer.WriteJson(ref value, '\'');
        }
        /// <summary>
        /// Guid转换成字符串（双引号）
        /// </summary>
        /// <param name="value">Guid</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteJson(ref System.Guid value)
        {
            if (PrepSize(38 * sizeof(char))) Data.Pointer.WriteJson(ref value, '"');
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        internal void WriteJson(char value)
        {
            if (((AutoCSer.JsonDeserializer.DeserializeBits.Byte[(byte)value] & AutoCSer.JsonDeserializer.EscapeBit) | (value >> 8)) == 0)
            {
                byte* data = GetBeforeMove(4 * sizeof(char));
                if (data != null)
                {
                    *(char*)data = '"';
                    switch ((value ^ (value >> 3)) & 7)
                    {
                        case ('\b' ^ ('\b' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('b' << 16); break;
                        case ('\t' ^ ('\t' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + (value == 0 ? ('0' << 16) : ('t' << 16)); break;
                        case ('\n' ^ ('\n' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('n' << 16); break;
                        case ('\f' ^ ('\f' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('f' << 16); break;
                        case ('\r' ^ ('\r' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('r' << 16); break;
                        case ('\\' ^ ('\\' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('\\' << 16); break;
                        case ('\"' ^ ('\"' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('"' << 16); break;
                    }
                    *(char*)(data + sizeof(char) * 3) = '"';
                }
            }
            else
            {
                byte* data = GetBeforeMove(3 * sizeof(char));
                if (data != null)
                {
                    *(char*)data = '"';
                    *(char*)(data + sizeof(char)) = value;
                    *(char*)(data + sizeof(char) * 2) = '"';
                }
            }
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="stringStart">起始位置</param>
        /// <param name="stringLength">字符串长度，必须大于0</param>
        internal void WriteJson(char* stringStart, int stringLength)
        {
            char* start = stringStart, end = stringStart + stringLength;
            byte* bits = AutoCSer.JsonDeserializer.DeserializeBits.Byte;
            int length = 0;
            do
            {
                if (((bits[*(byte*)start] & AutoCSer.JsonDeserializer.EscapeBit) | *(((byte*)start) + 1)) == 0) ++length;
            }
            while (++start != end);
            if (length == 0)
            {
                char* write = (char*)GetBeforeMove((stringLength + 2) * sizeof(char));
                if (write != null)
                {
                    *write = '"';
                    AutoCSer.Common.CopyTo(stringStart, ++write, stringLength << 1);
                    *(write + stringLength) = '"';
                }
            }
            else
            {
                length += stringLength + 2;
                char* write = (char*)GetBeforeMove(length * sizeof(char));
                if (write != null)
                {
                    *write++ = '"';
                    start = stringStart;
                    do
                    {
                        if (((bits[*(byte*)start] & AutoCSer.JsonDeserializer.EscapeBit) | *(((byte*)start) + 1)) == 0)
                        {
                            int code = *start;
                            switch ((code ^ (code >> 3)) & 7)
                            {
                                case ('\b' ^ ('\b' >> 3)) & 7: *(int*)write = '\\' + ('b' << 16); break;
                                case ('\t' ^ ('\t' >> 3)) & 7: *(int*)write = '\\' + (code == 0 ? ('0' << 16) : ('t' << 16)); break;
                                case ('\n' ^ ('\n' >> 3)) & 7: *(int*)write = '\\' + ('n' << 16); break;
                                case ('\f' ^ ('\f' >> 3)) & 7: *(int*)write = '\\' + ('f' << 16); break;
                                case ('\r' ^ ('\r' >> 3)) & 7: *(int*)write = '\\' + ('r' << 16); break;
                                case ('\\' ^ ('\\' >> 3)) & 7: *(int*)write = '\\' + ('\\' << 16); break;
                                case ('\"' ^ ('\"' >> 3)) & 7: *(int*)write = '\\' + ('"' << 16); break;
                            }
                            write += 2;
                        }
                        else *write++ = *start;
                    }
                    while (++start != end);
                    *write = '"';
                }
            }
        }
        /// <summary>
        /// 时间转字符串
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteJsonString(DateTime time)
        {
            switch (time.Kind)
            {
                case DateTimeKind.Utc:
                    char* utcFixed = GetPrepCharSizeCurrent(AutoCSer.Date.ToStringSize + 3);
                    if (utcFixed != null)
                    {
                        *utcFixed = '"';
                        int size = AutoCSer.Date.ToString(time, utcFixed + 1);
                        *(int*)(utcFixed + (size + 1)) = 'Z' + ('"' << 16);
                        Data.Pointer.MoveSize((size + 3) << 1);
                    }
                    return;
                case DateTimeKind.Local:
                    char* localFixed = GetPrepCharSizeCurrent(AutoCSer.Date.ToStringSize + 1 + 8);
                    if (localFixed != null)
                    {
                        *localFixed = '"';
                        int size = AutoCSer.Date.ToString(time, localFixed + 1);
                        *(long*)(localFixed + (size + 1)) = Date.ZoneHourString;
                        *(long*)(localFixed + (size + 5)) = Date.ZoneMinuteString;
                        Data.Pointer.MoveSize((size + 8) << 1);
                    }
                    return;
                default:
                    char* timeFixed = GetPrepCharSizeCurrent(AutoCSer.Date.ToStringSize + 2);
                    if (timeFixed != null)
                    {
                        *timeFixed = '"';
                        int size = AutoCSer.Date.ToString(time, timeFixed + 1);
                        *(timeFixed + (size + 1)) = '"';
                        Data.Pointer.MoveSize((size + 2) << 1);
                    }
                    return;
            }
        }
        /// <summary>
        /// 时间转字符串
        /// </summary>
        /// <param name="time">时间</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteJsonString(TimeSpan time)
        {
            writeString(time, '"');
        }
        /// <summary>
        /// 时间转字符串
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="quote"></param>
        private void writeString(TimeSpan time, char quote)
        {
            char* chars = GetPrepCharSizeCurrent(10 + 16 + 2);
            if (chars != null)
            {
                *chars = quote;
                int size = AutoCSer.Date.ToString(time, chars + 1);
                *(chars + (size + 1)) = quote;
                Data.Pointer.MoveSize((size + 2) << 1);
            }
        }
        /// <summary>
        /// 写入 new Date(
        /// </summary>
        internal void WriteJsonNewDate()
        {
            if (PrepSize((9 + 19 + 1) * sizeof(char)))
            {
                Data.Pointer.WriteSize('n' + ('e' << 16) + ((long)'w' << 32) + ((long)' ' << 48)
                    , 'D' + ('a' << 16) + ((long)'t' << 32) + ((long)'e' << 48)
                    , '(', 9 * sizeof(char));
            }
        }
        /// <summary>
        /// 时间转字符串 第三方格式开始 "/Date(
        /// </summary>
        internal void WriteJsonOtherDate()
        {
            if (PrepSize((7 + 19 + 4) * sizeof(char)))
            {
                Data.Pointer.WriteSize('"' + ('/' << 16) + ((long)'D' << 32) + ((long)'a' << 48)
                    , 't' + ('e' << 16) + ((long)'(' << 32)
                    , 7 * sizeof(char));
            }
        }
        /// <summary>
        /// 时间转字符串 第三方格式结束 )/"
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteJsonOtherDateEnd()
        {
            *(long*)Data.Pointer.Current = ')' + ('/' << 16) + ((long)'"' << 32);
            Data.Pointer.MoveSize(3 * sizeof(char));
        }
        /// <summary>
        /// 写入 JSON 名称
        /// </summary>
        /// <param name="value">字符串</param>
        internal void WriteJsonName(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                fixed (char* valueFixed = value) WriteJson(valueFixed, value.Length);
                Write(':');
            }
            else if (PrepSize(sizeof(ulong))) Data.Pointer.WriteSize('"' + ('"' << 16) + ((long)':' << 32), 3 * sizeof(char));
        }
        /// <summary>
        /// 写入 JSON Key
        /// </summary>
        /// <param name="size"></param>
        internal void WriteJsonKeyValueKey(int size)
        {
            if (PrepSize(size * sizeof(char)))
            {
                Data.Pointer.WriteSize('{' + ('"' << 16) + ((long)'K' << 32) + ((long)'e' << 48)
                    , 'y' + ('"' << 16) + ((long)':' << 32)
                    , 7 * sizeof(char));
            }
        }
        /// <summary>
        /// 写入 JSON Value
        /// </summary>
        internal void WriteJsonKeyValueValue()
        {
            if (PrepSize(3 * sizeof(ulong)))
            {
                Data.Pointer.WriteSize(',' + ('"' << 16) + ((long)'V' << 32) + ((long)'a' << 48)
                    , 'l' + ('u' << 16) + ((long)'e' << 32) + ((long)'"' << 48)
                    , ':', 9 * sizeof(char));
            }
        }

        /// <summary>
        /// 时间转字符串 yyyy/MM/dd HH:mm:ss.fffffff
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteSqlDateTime2String(DateTime time)
        {
            char* chars = GetPrepCharSizeCurrent(AutoCSer.Date.ToStringSize + 2);
            if (chars != null)
            {
                *chars = '\'';
                int size = AutoCSer.Date.ToString(time, chars + 1, ' ', '/');
                *(chars + (size + 1)) = '\'';
                Data.Pointer.MoveSize((size + 2) << 1);
            }
        }
        /// <summary>
        /// 时间转字符串 yyyy/MM/dd HH:mm:ss.fff
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteSqlDateTimeString(DateTime time)
        {
            char* chars = GetPrepCharSizeCurrent(AutoCSer.Date.ToStringSize - 4 + 2);
            if (chars != null)
            {
                *chars = '\'';
                int size = AutoCSer.Date.ToString3(time, chars + 1, ' ', '/');
                *(chars + (size + 1)) = '\'';
                Data.Pointer.MoveSize((size + 2) << 1);
            }
        }
        /// <summary>
        /// 时间转字符串 yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteSqlSmallDateTimeString(DateTime time)
        {
            char* chars = GetPrepCharSizeCurrent(19 + 2);
            if (chars != null)
            {
                *chars = '\'';
                AutoCSer.Date.ToSecondString(time, chars + 1, ' ', '/');
                *(chars + (19 + 1)) = '\'';
                Data.Pointer.MoveSize((19 + 2) << 1);
            }
        }
        /// <summary>
        /// 时间转字符串 yyyy/MM/dd
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteSqlDateString(DateTime time)
        {
            char* chars = GetPrepCharSizeCurrent(10 + 2);
            if (chars != null)
            {
                *chars = '\'';
                AutoCSer.Date.ToDateString(time, chars + 1, '/');
                *(chars + (10 + 1)) = '\'';
                Data.Pointer.MoveSize((10 + 2) << 1);
            }
        }
        /// <summary>
        /// 时间转字符串 HH:mm:ss.fffffff
        /// </summary>
        /// <param name="time">时间</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void WriteSqlString(TimeSpan time)
        {
            writeString(time, '\'');
        }

        /// <summary>
        /// 输出字符串，不处理转义符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quoteChar">默认为双引号</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(string value, char quoteChar = '"')
        {
            if (PrepCharSize(value.Length + 2)) Data.Pointer.Write(value, quoteChar);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(bool value)
        {
            Write(value ? '1' : '0');
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(byte value)
        {
            WriteJsonHex(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(sbyte value)
        {
            WriteJsonHex(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        public void WriteWebViewJson(short value)
        {
            if (value >= 0) WriteJsonHex((ushort)value);
            else
            {
                WriteNegative(7);
                WriteJsonHex((ushort)-value);
            }
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(ushort value)
        {
            WriteJsonHex(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(int value)
        {
            if (value >= 0) WriteJsonHex((uint)value);
            else
            {
                WriteNegative(11);
                WriteJsonHex((uint)-value);
            }
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(uint value)
        {
            WriteJsonHex(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        public void WriteWebViewJson(long value)
        {
            if (value >= 0) WriteWebViewJson((ulong)value);
            else if ((ulong)(value + JsonSerializer.MaxInteger) <= (ulong)(JsonSerializer.MaxInteger << 1))
            {
                WriteNegative(19);
                WriteJsonHex((ulong)-value);
            }
            else WriteJsonString(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(ulong value)
        {
            if (value <= JsonSerializer.MaxInteger) WriteJsonHex(value);
            else WriteJsonString(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(float value)
        {
            WriteJson(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(double value)
        {
            WriteJson(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(decimal value)
        {
            SimpleWrite(value.ToString());
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(char value)
        {
            WriteJson(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        public void WriteWebViewJson(DateTime value)
        {
            WriteJsonNewDate();
            WriteWebViewJson(((value.Kind == DateTimeKind.Utc ? value.Ticks + Date.LocalTimeTicks : value.Ticks) - AutoCSer.JsonDeserializer.JavaScriptLocalMinTimeTicks) / TimeSpan.TicksPerMillisecond);
            Data.Pointer.Write(')');
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WriteWebViewJson(Guid value)
        {
            WriteJson(ref value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        public void WriteWebViewJson(string value)
        {
            if (value.Length == 0) WriteJsonEmptyString();
            else
            {
                fixed (char* valueFixed = value) WriteJson(valueFixed, value.Length);
            }
        }
        /// <summary>
        /// WebView 写入 JSON 字符串
        /// </summary>
        /// <param name="value"></param>
        public void WriteWebViewJson(SubString value)
        {
            if (value.Length == 0) WriteJsonEmptyString();
            else
            {
                fixed (char* valueFixed = value.GetFixedBuffer()) WriteJson(valueFixed + value.Start, value.Length);
            }
        }
    }
}
