using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// JSON 节点
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct JsonNode
    {
        /// <summary>
        /// 64位整数值
        /// </summary>
        internal long Int64;
        /// <summary>
        /// 字符串
        /// </summary>
#if NetStandard21
        public string? String
#else
        public string String
#endif
        {
            get
            {
                switch (Type)
                {
                    case JsonNodeTypeEnum.String:
                    case JsonNodeTypeEnum.NumberString:
                    case JsonNodeTypeEnum.ErrorQuoteString:
                        return SubString;
                    case JsonNodeTypeEnum.QuoteString:
                        checkQuoteString();
                        return SubString;
                    case JsonNodeTypeEnum.NaN:
                        return "NaN";
                    case JsonNodeTypeEnum.PositiveInfinity:
                        return "Infinity";
                    case JsonNodeTypeEnum.NegativeInfinity:
                        return "-Infinity";
                    case JsonNodeTypeEnum.DateTimeTick:
                        return new DateTime(Int64, DateTimeKind.Local).toString();
                    case JsonNodeTypeEnum.Bool:
                        return (int)Int64 == 0 ? "false" : "true";
                    case JsonNodeTypeEnum.Array:
                        int count = (int)Int64;
                        if (count == 0) return "[]";
                        int isNext = 0;
                        var jsonDeserializer = default(JsonDeserializer);
                        try
                        {
                            using (AutoCSer.Memory.CharStream charStream = new AutoCSer.Memory.CharStream(AutoCSer.Common.Config.SerializeUnmanagedPool))
                            {
                                charStream.Data.Pointer.Write('[');
                                foreach (JsonNode node in ListArray)
                                {
                                    if (isNext == 0) isNext = 1;
                                    else charStream.Write(',');
                                    node.toString(charStream, ref jsonDeserializer);
                                    if (--count == 0) break;
                                }
                                charStream.Write(']');
                                return charStream.ToString();
                            }
                        }
                        finally
                        {
                            if (jsonDeserializer != null) AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Push(jsonDeserializer);
                        }
                    case JsonNodeTypeEnum.Dictionary:
                        return "[object Object]";
                }
                return null;
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为字符串
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>字符串</returns>
#if NetStandard21
        public static implicit operator string?(JsonNode value) { return value.String; }
#else
        public static implicit operator string(JsonNode value) { return value.String; }
#endif
        /// <summary>
        /// 检测未解析字符串
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void checkQuoteString()
        {
            Type = JsonDeserializer.DeserializeQuoteString(ref SubString, (int)(Int64 >> 32), (char)Int64, (int)Int64 >> 16) ? JsonNodeTypeEnum.String : JsonNodeTypeEnum.ErrorQuoteString;
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="jsonDeserializer"></param>
#if NetStandard21
        private void toString(CharStream charStream, ref JsonDeserializer? jsonDeserializer)
#else
        private void toString(CharStream charStream, ref JsonDeserializer jsonDeserializer)
#endif
        {
            switch (Type)
            {
                case JsonNodeTypeEnum.String:
                case JsonNodeTypeEnum.NumberString:
                case JsonNodeTypeEnum.ErrorQuoteString:
                    charStream.Write(ref SubString);
                    return;
                case JsonNodeTypeEnum.QuoteString:
                    if (jsonDeserializer == null) jsonDeserializer = AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Pop() ?? new JsonDeserializer();
                    if (!jsonDeserializer.DeserializeQuoteString(ref SubString, charStream, (int)(Int64 >> 32), (char)Int64)) Type = JsonNodeTypeEnum.ErrorQuoteString;
                    return;
                case JsonNodeTypeEnum.NaN:
                    charStream.WriteJsonNaN();
                    return;
                case JsonNodeTypeEnum.PositiveInfinity:
                    charStream.WritePositiveInfinity();
                    return;
                case JsonNodeTypeEnum.NegativeInfinity:
                    charStream.WriteNegativeInfinity();
                    return;
                case JsonNodeTypeEnum.DateTimeTick:
                    new DateTime(Int64, DateTimeKind.Local).toString(charStream);
                    return;
                case JsonNodeTypeEnum.Bool:
                    charStream.WriteJsonBool((int)Int64 != 0);
                    return;
                case JsonNodeTypeEnum.Array:
                    int count = (int)Int64;
                    if (count == 0) charStream.WriteJsonArray();
                    int isNext = 0;
                    charStream.Write('[');
                    foreach (JsonNode node in ListArray)
                    {
                        if (isNext == 0) isNext = 1;
                        else charStream.Write(',');
                        node.toString(charStream, ref jsonDeserializer);
                        if (--count == 0)
                        {
                            charStream.Write(']');
                            return;
                        }
                    }
                    return;
                case JsonNodeTypeEnum.Dictionary:
                    charStream.WriteJsonObjectString();
                    return;
            }
        }
        /// <summary>
        /// 逻辑值
        /// </summary>
        public bool Bool
        {
            get
            {
                switch (Type)
                {
                    case JsonNodeTypeEnum.String:
                        return SubString.Length != 0;
                    case JsonNodeTypeEnum.QuoteString:
                    case JsonNodeTypeEnum.ErrorQuoteString:
                        return true;
                    case JsonNodeTypeEnum.Array:
                    case JsonNodeTypeEnum.Dictionary:
                        return true;
                    case JsonNodeTypeEnum.NumberString:
                        if ((int)Int64 == 0)
                        {
                            if (SubString.Length == 1) return SubString[0] != '0';
                            double value;
                            return JsonSerializer.CustomConfig.Deserialize(ref SubString, out value) && value != 0 && !double.IsNaN(value);
                        }
                        return true;
                    case JsonNodeTypeEnum.DateTimeTick:
                        return Int64 != 0;
                    case JsonNodeTypeEnum.Bool:
                        return (int)Int64 != 0;
                }
                return false;
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为逻辑值
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>逻辑值</returns>
        public static implicit operator bool(JsonNode value) { return value.Bool; }
        /// <summary>
        /// 数值
        /// </summary>
        public double Number
        {
            get
            {
                switch (Type)
                {
                    case JsonNodeTypeEnum.NumberString:
                        if (SubString.Length == 1)
                        {
                            int value = SubString[0] - '0';
                            if ((uint)value < 10) return value;
                        }
                        double doubleValue;
                        if (JsonSerializer.CustomConfig.Deserialize(ref SubString, out doubleValue)) return doubleValue;
                        break;
                    case JsonNodeTypeEnum.NaN:
                        return double.NaN;
                    case JsonNodeTypeEnum.PositiveInfinity:
                        return double.PositiveInfinity;
                    case JsonNodeTypeEnum.NegativeInfinity:
                        return double.NegativeInfinity;
                }
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为数值
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>数值</returns>
        public static implicit operator double(JsonNode value) { return value.Number; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                switch (Type)
                {
                    case JsonNodeTypeEnum.String: return DateTime.Parse(SubString.ToString().notNull());
                    case JsonNodeTypeEnum.DateTimeTick: return new DateTime(Int64, DateTimeKind.Local);
                }
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为时间
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>字符串</returns>
        public static implicit operator DateTime(JsonNode value) { return value.DateTime; }
        /// <summary>
        /// 时间
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                switch (Type)
                {
                    case JsonNodeTypeEnum.String: return TimeSpan.Parse(SubString.ToString().notNull());
                }
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为时间
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>字符串</returns>
        public static implicit operator TimeSpan(JsonNode value) { return value.TimeSpan; }
        /// <summary>
        /// 字典
        /// </summary>
        internal KeyValue<JsonNode, JsonNode>[] DictionaryArray;
        /// <summary>
        /// 字典
        /// </summary>
        internal LeftArray<KeyValue<JsonNode, JsonNode>> Dictionary
        {
            get
            {
                return new LeftArray<KeyValue<JsonNode, JsonNode>>((int)Int64, DictionaryArray);
            }
        }
        /// <summary>
        /// 字典数据集合
        /// </summary>
        public IEnumerable<KeyValue<JsonNode, JsonNode>> Values
        {
            get
            {
                if (Type == JsonNodeTypeEnum.Array) return Dictionary;
                if (Type == JsonNodeTypeEnum.Null) return EmptyArray<KeyValue<JsonNode, JsonNode>>.Array;
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        internal JsonNode[] ListArray;
        /// <summary>
        /// 列表
        /// </summary>
        internal LeftArray<JsonNode> LeftArray
        {
            get
            {
                return new LeftArray<JsonNode>((int)Int64, ListArray);
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        public LeftArray<JsonNode> Array
        {
            get
            {
                if (Type == JsonNodeTypeEnum.Array) return LeftArray;
                if (Type == JsonNodeTypeEnum.Null) return new LeftArray<JsonNode>(0);
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 字符串
        /// </summary>
        internal SubString SubString;
        /// <summary>
        /// 类型
        /// </summary>
        public JsonNodeTypeEnum Type { get; internal set; }
        /// <summary>
        /// 是否空节点
        /// </summary>
        public bool IsNull
        {
            get { return Type == JsonNodeTypeEnum.Null; }
        }
        /// <summary>
        /// 字典 / 列表节点数量
        /// </summary>
        public int Count
        {
            get
            {
                switch (Type)
                {
                    case JsonNodeTypeEnum.Dictionary:
                    case JsonNodeTypeEnum.Array: return (int)Int64;
                    case JsonNodeTypeEnum.Null: return 0;
                }
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 获取列表节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public JsonNode this[int index]
        {
            get
            {
                switch (Type)
                {
                    case JsonNodeTypeEnum.String:
                    case JsonNodeTypeEnum.NumberString:
                    case JsonNodeTypeEnum.ErrorQuoteString:
                        goto CHARNODE;
                    case JsonNodeTypeEnum.QuoteString:
                        checkQuoteString();
                    CHARNODE:
                        if ((uint)index < SubString.Length) return new JsonNode { Type = JsonNodeTypeEnum.String, SubString = SubString.Slice(index, 1) };
                        break;
                    case JsonNodeTypeEnum.Array:
                        if ((uint)index < (uint)Int64) return ListArray[index];
                        break;
                    case JsonNodeTypeEnum.Dictionary:
                        int count = (int)Int64;
                        if (count != 0)
                        {
                            string key = index.toString();
                            foreach (KeyValue<JsonNode, JsonNode> value in DictionaryArray)
                            {
                                if (value.Key.String == key) return value.Value;
                            }
                        }
                        break;
                }
                return default(JsonNode);
            }
        }
        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns></returns>
        public JsonNode this[string key]
        {
            get
            {
                switch (Type)
                {
                    case JsonNodeTypeEnum.Dictionary:
                        int count = (int)Int64;
                        if (count != 0)
                        {
                            foreach (KeyValue<JsonNode, JsonNode> value in DictionaryArray)
                            {
                                if (value.Key.String == key) return value.Value;
                            }
                        }
                        break;
                }
                return default(JsonNode);
            }
        }

        /// <summary>
        /// 设置数字字符串
        /// </summary>
        /// <param name="quote"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNumberString(char quote)
        {
            Int64 = quote;
            Type = JsonNodeTypeEnum.NumberString;
        }
        /// <summary>
        /// 未解析字符串
        /// </summary>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <param name="isTempString"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetQuoteString(int escapeIndex, char quote, bool isTempString)
        {
            Type = JsonNodeTypeEnum.QuoteString;
            Int64 = ((long)escapeIndex << 32) + quote;
            if (isTempString) Int64 += 0x10000;
        }
        /// <summary>
        /// 设置列表
        /// </summary>
        /// <param name="list"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetList(ref LeftArray<JsonNode> list)
        {
            ListArray = list.Array;
            Int64 = list.Length;
            Type = JsonNodeTypeEnum.Array;
        }
        /// <summary>
        /// 设置列表
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetList()
        {
            ListArray = EmptyArray<JsonNode>.Array;
            Int64 = 0;
            Type = JsonNodeTypeEnum.Array;
        }
        /// <summary>
        /// 设置字典
        /// </summary>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetDictionary(ref LeftArray<KeyValue<JsonNode, JsonNode>> dictionary)
        {
            DictionaryArray = dictionary.Array;
            Int64 = dictionary.Length;
            Type = JsonNodeTypeEnum.Dictionary;
        }
        /// <summary>
        /// 设置字典
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetDictionary()
        {
            DictionaryArray = EmptyArray<KeyValue<JsonNode, JsonNode>>.Array;
            Int64 = 0;
            Type = JsonNodeTypeEnum.Dictionary;
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String ?? string.Empty;
        }
    }
}
