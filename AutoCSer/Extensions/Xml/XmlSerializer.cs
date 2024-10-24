﻿using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.Xml;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public unsafe sealed partial class XmlSerializer : TextSerializer<XmlSerializer, XmlSerializeConfig>
    {
        /// <summary>
        /// XML 自定义全局配置
        /// </summary>
        public static readonly CustomConfig CustomConfig = ((ConfigObject<CustomConfig>)AutoCSer.Configuration.Common.Get(typeof(CustomConfig)))?.Value ?? new CustomConfig();
        /// <summary>
        /// 默认序列化类型配置
        /// </summary>
        internal static readonly XmlSerializeAttribute ConfigurationAttribute = ((ConfigObject<XmlSerializeAttribute>)AutoCSer.Configuration.Common.Get(typeof(XmlSerializeAttribute)))?.Value;
        /// <summary>
        /// 默认序列化类型配置
        /// </summary>
        internal static readonly XmlSerializeAttribute AllMemberAttribute = ConfigurationAttribute ?? new XmlSerializeAttribute { IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly XmlSerializeConfig DefaultConfig = ((ConfigObject<XmlSerializeConfig>)AutoCSer.Configuration.Common.Get(typeof(XmlSerializeConfig)))?.Value ?? new XmlSerializeConfig();

        /// <summary>
        /// 字符状态位查询表格
        /// </summary>
        private readonly byte* bits = XmlDeserializer.DeserializeBits.Byte;
        /// <summary>
        /// 获取 XML 字符串输出缓冲区
        /// </summary>
        /// <param name="serializer"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CharStream GetCharStream(XmlSerializer serializer) { return serializer.CharStream; }
        /// <summary>
        /// 集合子节点名称
        /// </summary>
        private string itemName;
        /// <summary>
        /// 集合子节点名称
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal string GetItemName()
        {
            if (itemName == null) return Config.ItemName ?? XmlSerializeConfig.DefaultItemName;
            string value = itemName;
            itemName = null;
            return value;
        }
        /// <summary>
        /// 设置集合子节点名称
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="itemName"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetItemName(XmlSerializer xmlSerializer, string itemName)
        {
            xmlSerializer.itemName = itemName;
        }
        /// <summary>
        /// XML 序列化
        /// </summary>
        /// <param name="isThreadStatic">是否单线程模式</param>
        internal XmlSerializer(bool isThreadStatic = false) : base(isThreadStatic) { }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>Xml字符串</returns>
        private string serialize<T>(ref T value, XmlSerializeConfig config)
        {
            isProcessing = true;
            Config = config ?? DefaultConfig;
            CharStream.TrySetData(UnmanagedPool.Default);
            using (CharStream)
            {
                serialize(ref value);
                return (Warning & TextSerialize.WarningEnum.ResizeError) == 0 ? CharStream.ToString() : null;
            }
        }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="charStream">Xml输出缓冲区</param>
        /// <param name="config">配置参数</param>
        private void serialize<T>(ref T value, CharStream charStream, XmlSerializeConfig config)
        {
            isProcessing = true;
            Config = config ?? DefaultConfig;
            UnmanagedPoolPointer data;
            bool isUnmanaged, canResize;
            CharStream.Exchange(charStream, out data, out isUnmanaged, out canResize);
            try
            {
                serialize(ref value);
            }
            finally { CharStream.Exchange(charStream, ref data, isUnmanaged, canResize); }
        }
        /// <summary>
        /// 对象转换XML字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>Xml字符串</returns>
        private string serializeThreadStatic<T>(ref T value, XmlSerializeConfig config)
        {
            isProcessing = true;
            Config = config ?? DefaultConfig;
            CharStream.Clear();
            serialize(ref value);
            return (Warning & TextSerialize.WarningEnum.ResizeError) == 0 ? CharStream.ToString() : null;
        }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void serialize<T>(ref T value)
        {
            Warning = AutoCSer.TextSerialize.WarningEnum.None;
            forefather.Reserve = 0;
            CheckDepth = Config.CheckDepth;

            CharStream.WriteNotNull(Config.Header);
            fixed (char* nameFixed = Config.BootNodeName)
            {
                nameStart(nameFixed, Config.BootNodeName.Length);
                if (value != null) TypeSerializer<T>.Serialize(this, ref value);
                nameEnd(nameFixed, Config.BootNodeName.Length);
            }
            if (CharStream.IsResizeError) Warning |= AutoCSer.TextSerialize.WarningEnum.ResizeError;
        }
        /// <summary>
        /// 释放资源（线程静态实例模式）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void freeThreadStatic()
        {
            Config = DefaultConfig;
            free();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            freeThreadStatic();
            YieldPool.Default.Push(this);
        }
        /// <summary>
        /// 循环引用对象处理
        /// </summary>
        protected override void WriteLoopReference() { }
        /// <summary>
        /// 标签开始
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void nameStart(char* start, int length)
        {
            if (CharStream.PrepCharSize(length + (2 + 2)))
            {
                CharStream.Data.Pointer.Write('<');
                CharStream.Data.Pointer.SimpleWrite((byte*)start, length << 1);
                CharStream.Data.Pointer.Write('>');
            }
        }
        /// <summary>
        /// 标签结束
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void nameEnd(char* start, int length)
        {
            if (CharStream.PrepCharSize(length + (2 + 2)))
            {
                CharStream.Data.Pointer.Write('<' + ('/' << 16));
                CharStream.Data.Pointer.SimpleWrite((byte*)start, length << 1);
                CharStream.Data.Pointer.Write('>');
            }
        }
        /// <summary>
        /// 计算编码增加长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int encodeSpaceSize(char value)
        {
            if (((bits[(byte)value] & XmlDeserializer.EncodeSpaceBit) | (value & 0xff00)) == 0)
            {
                switch (value & 7)
                {
                    case '&' & 7:  //26 00100110
                        //case '>' & 7://3e 00111110
                        return 4 - ((value >> 4) & 1);
                    case '\n' & 7:
                    case '\r' & 7:
                    case ' ' & 7:
                        return 4;
                    case '\t' & 7:
                    case '<' & 7:
                        return 3;
                }
            }
            return 0;
        }
        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">字符</param>
        private void encodeSpace(ref byte* data, char value)
        {
#if DEBUG
            CharStream.Data.Pointer.DebugCheckEnd(data + sizeof(char));
#endif
            if (((bits[(byte)value] & XmlDeserializer.EncodeSpaceBit) | (value & 0xff00)) == 0)
            {
                switch ((byte)value)
                {
                    case (byte)'\t':
#if DEBUG
                        CharStream.Data.Pointer.DebugCheckEnd(data + sizeof(long));
#endif
                        *(long*)data = '&' + ('#' << 16) + ((long)'9' << 32) + ((long)';' << 48);
                        data += sizeof(long);
                        return;
                    case (byte)'\n':
#if DEBUG
                        CharStream.Data.Pointer.DebugCheckEnd(data + sizeof(long) + sizeof(char));
#endif
                        *(long*)data = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'0' << 48);
                        *(char*)(data + sizeof(long)) = ';';
                        data += sizeof(long) + sizeof(char);
                        return;
                    case (byte)'\r':
#if DEBUG
                        CharStream.Data.Pointer.DebugCheckEnd(data + sizeof(long) + sizeof(char));
#endif
                        *(long*)data = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'3' << 48);
                        *(char*)(data + sizeof(long)) = ';';
                        data += sizeof(long) + sizeof(char);
                        return;
                    case (byte)' ':
#if DEBUG
                        CharStream.Data.Pointer.DebugCheckEnd(data + sizeof(long) + sizeof(char));
#endif
                        *(long*)data = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'2' << 48);
                        *(char*)(data + sizeof(long)) = ';';
                        data += sizeof(long) + sizeof(char);
                        return;
                    case (byte)'&':
#if DEBUG
                        CharStream.Data.Pointer.DebugCheckEnd(data + sizeof(long) + sizeof(char));
#endif
                        *(long*)data = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'8' << 48);
                        *(char*)(data + sizeof(long)) = ';';
                        data += sizeof(long) + sizeof(char);
                        return;
                    case (byte)'<':
#if DEBUG
                        CharStream.Data.Pointer.DebugCheckEnd(data + sizeof(long));
#endif
                        *(long*)data = '&' + ('l' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        data += sizeof(long);
                        return;
                    case (byte)'>':
#if DEBUG
                        CharStream.Data.Pointer.DebugCheckEnd(data + sizeof(long));
#endif
                        *(long*)data = '&' + ('g' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        data += sizeof(long);
                        return;
                }
            }
            *(char*)data = value;
            data += sizeof(char);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void serialize(char* start, int length)
        {
            switch (length)
            {
                case 1:
                    PrimitiveSerialize(*start);
                    return;
                case 2:
                    PrimitiveSerialize(*start);
                    PrimitiveSerialize(*(start + 1));
                    return;
            }
            char* end = start + (length - 1);
            int addLength = 0;
            if (length > 8)
            {
                int isCData = *end == '>' && *(int*)(end - 2) == ']' + (']' << 16) ? 0 : 1;
                for (char* code = start + 2; code != end; ++code)
                {
                    if (((bits[*(byte*)code] & XmlDeserializer.EncodeBit) | *(((byte*)code) + 1)) == 0)
                    {
                        switch ((*(byte*)code >> 1) & 7)
                        {
                            case ('&' >> 1) & 7:
                                addLength += 4;
                                break;
                            case ('<' >> 1) & 7:
                                addLength += 3;
                                break;
                            case ('>' >> 1) & 7:
                                addLength += 3;
                                if (*(int*)(code - 2) == ']' + (']' << 16)) isCData = 0;
                                break;
                        }
                    }
                }
                if (isCData == 0)
                {
                    byte* code = (byte*)(start + 1);
                    if (((bits[*(byte*)code] & XmlDeserializer.EncodeBit) | *(((byte*)code) + 1)) == 0)
                    {
                        //& 26 00100110
                        //< 3c 00111100
                        //> 3e 00111110
                        addLength += 4 - ((*(byte*)code >> 4) & 1);
                    }
                }
                else
                {
                    if (addLength == 0) addLength += encodeSpaceSize(*start) + encodeSpaceSize(*(start + 1)) + encodeSpaceSize(*end);
                    if (addLength == 0) CharStream.Write(start, length);
                    else
                    {
                        byte* write = (byte*)CharStream.GetPrepCharSizeCurrent(length + 13);
                        if (write != null)
                        {
                            *(long*)write = '<' + ('!' << 16) + ((long)'[' << 32) + ((long)'C' << 48);
                            *(long*)(write + sizeof(long)) = 'D' + ('A' << 16) + ((long)'T' << 32) + ((long)'A' << 48);
                            *(char*)(write + sizeof(long) * 2) = '[';
                            AutoCSer.Common.Config.CopyTo(start, write + (sizeof(long) * 2 + sizeof(char)), length << 1);
                            *(long*)(write + (sizeof(long) * 2 + sizeof(char)) + (length << 1)) = ']' + (']' << 16) + ((long)'>' << 32);
                            CharStream.Data.Pointer.CurrentIndex += (length + 12) * sizeof(char);
                        }
                    }
                    return;
                }
            }
            else
            {
                for (char* code = start + 1; code != end; ++code)
                {
                    if (((bits[*(byte*)code] & XmlDeserializer.EncodeBit) | *(((byte*)code) + 1)) == 0)
                    {
                        //& 26 00100110
                        //< 3c 00111100
                        //> 3e 00111110
                        addLength += 4 - ((*(byte*)code >> 4) & 1);
                    }
                }
            }
            if ((addLength += encodeSpaceSize(*start) + encodeSpaceSize(*end)) == 0) CharStream.Write(start, length);
            else
            {
                byte* write = (byte*)CharStream.GetPrepCharSizeCurrent(length + addLength);
                if (write != null)
                {
                    encodeSpace(ref write, *start++);
                    do
                    {
                        if (((bits[*(byte*)start] & XmlDeserializer.EncodeBit) | *(((byte*)start) + 1)) == 0)
                        {
                            switch ((*(byte*)start >> 1) & 7)
                            {
                                case ('&' >> 1) & 7:
#if DEBUG
                                    CharStream.Data.Pointer.DebugCheckEnd(write + sizeof(long) + sizeof(char));
#endif
                                    *(long*)write = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'8' << 48);
                                    *(char*)(write + sizeof(long)) = ';';
                                    write += sizeof(long) + sizeof(char);
                                    break;
                                case ('<' >> 1) & 7:
#if DEBUG
                                    CharStream.Data.Pointer.DebugCheckEnd(write + sizeof(long));
#endif
                                    *(long*)write = '&' + ('l' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                                    write += sizeof(long);
                                    break;
                                case ('>' >> 1) & 7:
#if DEBUG
                                    CharStream.Data.Pointer.DebugCheckEnd(write + sizeof(long));
#endif
                                    *(long*)write = '&' + ('g' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                                    write += sizeof(long);
                                    break;
                            }
                        }
                        else
                        {
#if DEBUG
                            CharStream.Data.Pointer.DebugCheckEnd(write + sizeof(char));
#endif
                            *(char*)write = *start;
                            write += sizeof(char);
                        }
                    }
                    while (++start != end);
                    encodeSpace(ref write, *start);
                    CharStream.Data.Pointer.CurrentIndex += (length + addLength) * sizeof(char);
                }
            }
        }
        /// <summary>
        /// 输出空字符串
        /// </summary>
        private void emptyString()
        {
            //<![CDATA[]]>
            byte* write = CharStream.GetBeforeMove(24);
            if (write != null)
            {
                *(long*)write = '<' + ('!' << 16) + ((long)'[' << 32) + ((long)'C' << 48);
                *(long*)(write + sizeof(long)) = 'D' + ('A' << 16) + ((long)'T' << 32) + ((long)'A' << 48);
                *(long*)(write + sizeof(long) * 2) = '[' + (']' << 16) + ((long)']' << 32) + ((long)'>' << 48);
            }
        }
        /// <summary>
        /// 转换 XML 字符串
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Serialize<T>(XmlSerializer serializer, T value)
        {
            if (value != null) TypeSerializer<T>.Serialize(serializer, ref value);
        }
        /// <summary>
        /// 基类序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Base<T, BT>(XmlSerializer serializer, T value)
            where T : class, BT
        {
            if (value != null) TypeSerializer<BT>.Serialize(serializer, value);
        }
        /// <summary>
        /// 可空类型序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void nullable<T>(ref T? value) where T : struct
        {
            if (value.HasValue)
            {
                int streamIndex = CharStream.Data.Pointer.CurrentIndex;
                TypeSerializer<T>.Serialize(this, value.Value);
                if (streamIndex == CharStream.Data.Pointer.CurrentIndex) TypeSerializer<T>.WriteEmptyString(CharStream);
            }
        }
        /// <summary>
        /// 可空类型序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Nullable<T>(XmlSerializer serializer, T? value) where T : struct
        {
            serializer.nullable(ref value);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">数组对象</param>
        private void array<T>(T[] array)
        {
            string itemName = GetItemName();
            fixed (char* itemNameFixed = itemName)
            {
                int itemNameLength = itemName.Length;
                foreach (T value in array)
                {
                    nameStart(itemNameFixed, itemNameLength);
                    if (value != null) TypeSerializer<T>.Serialize(this, value);
                    nameEnd(itemNameFixed, itemNameLength);
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Array<T>(XmlSerializer serializer, T[] array)
        {
            if (array != null) serializer.array(array);
        }
        /// <summary>
        /// 集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        internal void collection<T>(ICollection<T> array)
        {
            string itemName = GetItemName();
            fixed (char* itemNameFixed = itemName)
            {
                int itemNameLength = itemName.Length;
                foreach (T value in array)
                {
                    nameStart(itemNameFixed, itemNameLength);
                    if (value != null) TypeSerializer<T>.Serialize(this, value);
                    nameEnd(itemNameFixed, itemNameLength);
                }
            }
        }
        /// <summary>
        /// 集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Collection<T, VT>(XmlSerializer serializer, T value)
            where T : ICollection<VT>
        {
            if (value != null) serializer.collection(value);
        }
        /// <summary>
        /// 自定义序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        internal static void NotSupport<T>(XmlSerializer serializer, T value)
        {
            int size = CustomConfig.NotSupport(serializer, value);
            if (size > 0) serializer.CharStream.Data.Pointer.CheckMoveSize(size << 1);
        }

        /// <summary>
        /// 逻辑值转换
        /// </summary>
        /// <param name="value">逻辑值</param>
        public void PrimitiveSerialize(bool value)
        {
            if (value)
            {
                byte* data = CharStream.GetBeforeMove(4 * sizeof(char));
                *(long*)data = 'T' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48);
            }
            else
            {
                byte* chars = (byte*)CharStream.GetBeforeMove(5 * sizeof(char));
                if (chars != null)
                {
                    *(long*)chars = 'F' + ('a' << 16) + ((long)'l' << 32) + ((long)'s' << 48);
                    *(char*)(chars + sizeof(long)) = 'e';
                }
            }
        }
        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="value">字符</param>
        public void PrimitiveSerialize(char value)
        {
            if (((bits[(byte)value] & XmlDeserializer.EncodeSpaceBit) | (value & 0xff00)) == 0)
            {
                switch ((byte)value)
                {
                    case (byte)'\t':
                        byte* data = CharStream.GetBeforeMove(4 * sizeof(char));
                        if (data != null) *(long*)data = '&' + ('#' << 16) + ((long)'9' << 32) + ((long)';' << 48);
                        return;
                    case (byte)'\n':
                        data = CharStream.GetBeforeMove(5 * sizeof(char));
                        if (data != null)
                        {
                            *(long*)data = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'0' << 48);
                            *(char*)(data + sizeof(long)) = ';';
                        }
                        return;
                    case (byte)'\r':
                        data = CharStream.GetBeforeMove(5 * sizeof(char));
                        if (data != null)
                        {
                            *(long*)data = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'3' << 48);
                            *(char*)(data + sizeof(long)) = ';';
                        }
                        return;
                    case (byte)' ':
                        data = CharStream.GetBeforeMove(5 * sizeof(char));
                        if (data != null)
                        {
                            *(long*)data = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'2' << 48);
                            *(char*)(data + sizeof(long)) = ';';
                        }
                        return;
                    case (byte)'&':
                        data = CharStream.GetBeforeMove(5 * sizeof(char));
                        if (data != null)
                        {
                            *(long*)data = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'8' << 48);
                            *(char*)(data + sizeof(long)) = ';';
                        }
                        return;
                    case (byte)'<':
                        data = CharStream.GetBeforeMove(4 * sizeof(char));
                        if (data != null) *(long*)data = '&' + ('l' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        return;
                    case (byte)'>':
                        data = CharStream.GetBeforeMove(4 * sizeof(char));
                        if (data != null) *(long*)data = '&' + ('g' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        return;
                }
            }
            CharStream.Write(value);
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        internal void SerializeDateTime(DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Utc:
                    char* utcFixed = CharStream.GetPrepCharSizeCurrent(AutoCSer.Date.ToStringSize + 1);
                    if (utcFixed != null)
                    {
                        int size = AutoCSer.Date.ToString(value, utcFixed);
                        *(char*)(utcFixed + size) = 'Z';
                        CharStream.Data.Pointer.MoveSize((size + 1) << 1);
                    }
                    return;
                case DateTimeKind.Local:
                    char* localFixed = CharStream.GetPrepCharSizeCurrent(AutoCSer.Date.ToStringSize + 6);
                    if (localFixed != null)
                    {
                        int size = AutoCSer.Date.ToString(value, localFixed);
                        *(long*)(localFixed + size) = Date.ZoneHourString;
                        *(int*)(localFixed + (size + 4)) = (int)Date.ZoneMinuteString;
                        CharStream.Data.Pointer.MoveSize((size + 6) << 1);
                    }
                    return;
                default:
                    char* timeFixed = CharStream.GetPrepCharSizeCurrent(AutoCSer.Date.ToStringSize);
                    if (timeFixed != null) CharStream.Data.Pointer.MoveSize(AutoCSer.Date.ToString(value, timeFixed) << 1);
                    return;
            }
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeTimeSpan(TimeSpan value)
        {
            char* chars = CharStream.GetPrepCharSizeCurrent(10 + 16);
            if (chars != null) CharStream.Data.Pointer.MoveSize(AutoCSer.Date.ToString(value, chars) << 1);
        }
        /// <summary>
        /// Guid 转换
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(Guid value)
        {
            byte* data = CharStream.GetBeforeMove(36 * sizeof(char));
            if (data != null) new GuidCreator { Value = value }.ToString((char*)data);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveSerializeNotEmpty(string value)
        {
            fixed (char* valueFixed = value) serialize(valueFixed, value.Length);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value"></param>
        public void PrimitiveSerialize(string value)
        {
            if (value != null)
            {
                if (value.Length == 0) emptyString();
                else primitiveSerializeNotEmpty(value);
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, string value)
        {
            serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value"></param>
        public void PrimitiveSerialize(SubString value)
        {
            if (value.String != null)
            {
                if (value.Length == 0) emptyString();
                else
                {
                    fixed (char* valueFixed = value.GetFixedBuffer()) serialize(valueFixed + value.Start, value.Length);
                }
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, SubString value)
        {
            serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// object 转换
        /// </summary>
        /// <param name="value"></param>
        internal void PrimitiveSerialize(object value)
        {
            if (value.GetType() == typeof(XmlNode)) PrimitiveSerialize((XmlNode)value);
            else if (Config.IsObject)
            {
                Type type = value.GetType();
                if (type != typeof(object)) AutoCSer.Extensions.Metadata.GenericType.Get(type).XmlSerializeObjectDelegate(this, value);
            }
        }
        /// <summary>
        /// object 转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, object value)
        {
            if (value != null) serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// object 对象转换XML字符串
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Object<T>(T value)
        {
            ++forefather.Reserve;
            TypeSerializer<T>.Serialize(this, value);
            --forefather.Reserve;
        }
        /// <summary>
        /// object 对象转换XML字符串
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Object<T>(XmlSerializer serializer, object value)
        {
            serializer.Object((T)value);
        }
        /// <summary>
        /// Node 转换
        /// </summary>
        /// <param name="value"></param>
        internal void PrimitiveSerialize(XmlNode value)
        {
            switch (value.Type)
            {
                case XmlNodeTypeEnum.String:
                    PrimitiveSerialize(value.String);
                    return;
                case XmlNodeTypeEnum.EncodeString:
                case XmlNodeTypeEnum.TempString:
                    CharStream.Write(value.String);
                    return;
                case XmlNodeTypeEnum.Node:
                    foreach (KeyValue<SubString, XmlNode> node in value.Nodes)
                    {
                        fixed (char* nameFixed = node.Key.GetFixedBuffer())
                        {
                            char* start = nameFixed + node.Key.Start;
                            nameStart(start, node.Key.Length);
                            PrimitiveSerialize(node.Value);
                            nameEnd(start, node.Key.Length);
                        }
                    }
                    return;
            }
        }
        /// <summary>
        /// Node 转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, XmlNode value)
        {
            serializer.PrimitiveSerialize(value);
        }

        /// <summary>
        /// 自定义序列化调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CustomSerialize<T>(T value)
        {
            if (value != null)
            {
                ++forefather.Reserve;
                TypeSerializer<T>.Serialize(this, ref value);
                --forefather.Reserve;
            }
        }

        /// <summary>
        /// 是否输出字符串
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsOutputSubString(XmlSerializer xmlSerializer, SubString value)
        {
            return value.Length != 0 || xmlSerializer.Config.IsOutputEmptyString;
        }
        /// <summary>
        /// 是否输出字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool isOutputString(string value)
        {
            if (value == null) return Config.IsOutputNull && Config.IsOutputEmptyString;
            return value.Length != 0 || Config.IsOutputEmptyString;
        }
        /// <summary>
        /// 是否输出字符串
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsOutputString(XmlSerializer xmlSerializer, string value)
        {
            return xmlSerializer.isOutputString(value);
        }
        /// <summary>
        /// 是否输出对象
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsOutput(XmlSerializer xmlSerializer, object value)
        {
            return value != null || xmlSerializer.Config.IsOutputNull;
        }
        /// <summary>
        /// 是否输出可空对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool IsOutputNullable<T>(XmlSerializer xmlSerializer, T? value) where T : struct
        {
            return value.HasValue || xmlSerializer.Config.IsOutputNull;
        }

        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Serialize<T>(T value, XmlSerializeConfig config = null)
        {
            AutoCSer.TextSerialize.WarningEnum warning;
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Serialize<T>(ref T value, XmlSerializeConfig config = null)
        {
            AutoCSer.TextSerialize.WarningEnum warning;
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Serialize<T>(T value, out AutoCSer.TextSerialize.WarningEnum warning, XmlSerializeConfig config = null)
        {
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        public static string Serialize<T>(ref T value, out AutoCSer.TextSerialize.WarningEnum warning, XmlSerializeConfig config = null)
        {
            XmlSerializer serializer = YieldPool.Default.Pop() ?? new XmlSerializer();
            try
            {
                string xml = serializer.serialize(ref value, config);
                warning = serializer.Warning;
                return xml;
            }
            finally { serializer.Free(); }
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="charStream">XML 输出缓冲区</param>
        /// <param name="config">配置参数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.TextSerialize.WarningEnum Serialize<T>(T value, CharStream charStream, XmlSerializeConfig config = null)
        {
            return Serialize(ref value, charStream, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="charStream">XML 输出缓冲区</param>
        /// <param name="config">配置参数</param>
        public static AutoCSer.TextSerialize.WarningEnum Serialize<T>(ref T value, CharStream charStream, XmlSerializeConfig config = null)
        {
            if (!charStream.IsResizeError)
            {
                XmlSerializer serializer = YieldPool.Default.Pop() ?? new XmlSerializer();
                try
                {
                    serializer.serialize(ref value, charStream, config);
                    return serializer.Warning;
                }
                finally { serializer.Free(); }
            }
            return TextSerialize.WarningEnum.ResizeError;
        }

        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string SerializeObject(object value, XmlSerializeConfig config = null)
        {
            return AutoCSer.Extensions.Metadata.GenericType.Get(value != null ? value.GetType() : typeof(object)).XmlSerializeObjectGenericDelegate(value, config).Key;
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        public static string SerializeObject(object value, out AutoCSer.TextSerialize.WarningEnum warning, XmlSerializeConfig config = null)
        {
            KeyValue<string, AutoCSer.TextSerialize.WarningEnum> xml = AutoCSer.Extensions.Metadata.GenericType.Get(value != null ? value.GetType() : typeof(object)).XmlSerializeObjectGenericDelegate(value, config);
            warning = xml.Value;
            return xml.Key;
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="objectValue">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串 + 警告提示状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static KeyValue<string, AutoCSer.TextSerialize.WarningEnum> Serialize<T>(object objectValue, XmlSerializeConfig config)
        {
            T value = (T)objectValue;
            AutoCSer.TextSerialize.WarningEnum warning;
            return new KeyValue<string, AutoCSer.TextSerialize.WarningEnum>(Serialize(ref value, out warning, config), warning);
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="charStream">XML 输出缓冲区</param>
        /// <param name="config">配置参数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.TextSerialize.WarningEnum SerializeObject(object value, CharStream charStream, XmlSerializeConfig config = null)
        {
            return AutoCSer.Extensions.Metadata.GenericType.Get(value != null ? value.GetType() : typeof(object)).XmlSerializeStreamObjectDelegate(value, charStream, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="objectValue">数据对象</param>
        /// <param name="charStream">XML 输出缓冲区</param>
        /// <param name="config">配置参数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.TextSerialize.WarningEnum Serialize<T>(object objectValue, CharStream charStream, XmlSerializeConfig config)
        {
            T value = (T)objectValue;
            return Serialize(ref value, charStream, config);
        }

        /// <summary>
        /// 对象转换 XML 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string ThreadStaticSerialize<T>(T value, XmlSerializeConfig config = null)
        {
            AutoCSer.TextSerialize.WarningEnum warning;
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string ThreadStaticSerialize<T>(ref T value, XmlSerializeConfig config = null)
        {
            AutoCSer.TextSerialize.WarningEnum warning;
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string ThreadStaticSerialize<T>(T value, out AutoCSer.TextSerialize.WarningEnum warning, XmlSerializeConfig config = null)
        {
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 字符串</returns>
        public static string ThreadStaticSerialize<T>(ref T value, out AutoCSer.TextSerialize.WarningEnum warning, XmlSerializeConfig config = null)
        {
            XmlSerializer serializer = ThreadStaticSerializer.Get().Serializer;
            try
            {
                string xml = serializer.serializeThreadStatic(ref value, config);
                warning = serializer.Warning;
                return xml;
            }
            finally { serializer.freeThreadStatic(); }
        }

        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        internal static readonly Dictionary<HashObject<Type>, AutoCSer.TextSerialize.DelegateReference> SerializeDelegates;

        static XmlSerializer()
        {
            SerializeDelegates = AutoCSer.DictionaryCreator.CreateHashObject<System.Type, AutoCSer.TextSerialize.DelegateReference>();
            SerializeDelegates.Add(typeof(bool), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, bool>)primitiveSerialize));
            SerializeDelegates.Add(typeof(bool?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, bool?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(byte), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, byte>)primitiveSerialize));
            SerializeDelegates.Add(typeof(byte?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, byte?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(sbyte), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, sbyte>)primitiveSerialize));
            SerializeDelegates.Add(typeof(sbyte?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, sbyte?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(short), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, short>)primitiveSerialize));
            SerializeDelegates.Add(typeof(short?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, short?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ushort), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, ushort>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ushort?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, ushort?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(int), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, int>)primitiveSerialize));
            SerializeDelegates.Add(typeof(int?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, int?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(uint), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, uint>)primitiveSerialize));
            SerializeDelegates.Add(typeof(uint?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, uint?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(long), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, long>)primitiveSerialize));
            SerializeDelegates.Add(typeof(long?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, long?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ulong), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, ulong>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ulong?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, ulong?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(float), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, float>)primitiveSerialize));
            SerializeDelegates.Add(typeof(float?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, float?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(double), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, double>)primitiveSerialize));
            SerializeDelegates.Add(typeof(double?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, double?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(decimal), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, decimal>)primitiveSerialize));
            SerializeDelegates.Add(typeof(decimal?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, decimal?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(char), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, char>)primitiveSerialize));
            SerializeDelegates.Add(typeof(char?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, char?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(DateTime), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, DateTime>)primitiveSerialize));
            SerializeDelegates.Add(typeof(DateTime?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, DateTime?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(TimeSpan), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, TimeSpan>)primitiveSerialize));
            SerializeDelegates.Add(typeof(TimeSpan?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, TimeSpan?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(Guid), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, Guid>)primitiveSerialize));
            SerializeDelegates.Add(typeof(Guid?), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, Guid?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(string), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, string>)primitiveSerialize));
            SerializeDelegates.Add(typeof(SubString), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, SubString>)primitiveSerialize));
            SerializeDelegates.Add(typeof(object), new AutoCSer.TextSerialize.DelegateReference { Delegate = new AutoCSer.TextSerialize.SerializeDelegate((Action<XmlSerializer, object>)primitiveSerialize), PushType = AutoCSer.TextSerialize.PushTypeEnum.DepthCount, IsUnknownMember = true, IsCompleted = true });
            SerializeDelegates.Add(typeof(XmlNode), new AutoCSer.TextSerialize.DelegateReference((Action<XmlSerializer, XmlNode>)primitiveSerialize));

            foreach (AutoCSer.TextSerialize.SerializeDelegate serializeDelegate in CustomConfig.PrimitiveSerializeDelegates)
            {
                Type type;
                AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
                if (serializeDelegate.Check(typeof(XmlSerializer), out type, out serializeDelegateReference))
                {
                    SerializeDelegates[type] = serializeDelegateReference;
                }
            }
        }
    }
}
