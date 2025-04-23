using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !AOT
using AutoCSer.RandomObject.Metadata;
#endif

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 随机对象生成
    /// </summary>
    public static partial class Creator
    {
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly Config DefaultConfig = AutoCSer.Configuration.Common.Get<Config>()?.Value ?? new Config();

        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="isNullable"></param>
        /// <returns></returns>
#if NetStandard21
        public static T? Create<
#if AOT
            [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
        T>(Config config, bool isNullable)
#else
        public static T Create<T>(Config config, bool isNullable)
#endif
        {
            var customCreator = config.GetCustomCreator(typeof(T)).castType<Func<Config, bool, T>>();
            if (customCreator == null) return isNullable ? Creator<T>.Create(config) : Creator<T>.CreateNotNull(config);
            return customCreator(config, isNullable);
        }
        /// <summary>
        /// 创建随机成员对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="config"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void CreateBase<T>(ref T value, Config config)
        {
            Creator<T>.CreateMember(ref value, config);
        }
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T? CreateNullable<T>(Config config) where T : struct
        {
            if (CreateBool(config)) return Create<T>(config, false);
            return new T?();
        }
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        public static T?[]? CreateArray<T>(Config config)
#else
        public static T[] CreateArray<T>(Config config)
#endif
        {
            var customCreator = config.GetCustomCreator(typeof(T)).castType<Func<Config, bool, T>>();
            if (customCreator == null) return createArray<T>(config);
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return null;
                case 1: return EmptyArray<T>.Array;
            }
            int length = Math.Abs(AutoCSer.Random.Default.Next(config.MaxArraySize)) + 1;
            T[] value = new T[length];
            while (length != 0) value[--length] = customCreator(config, true);
            return value;
        }
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        private static T?[]? createArray<T>(Config config)
#else
        private static T[] createArray<T>(Config config)
#endif
        {
            var historyValue = config.TryGetValue(typeof(T[]));
            if (historyValue != null) return (T[])historyValue;
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return null;
                case 1: return EmptyArray<T>.Array;
            }
            int length = Math.Abs(AutoCSer.Random.Default.Next(config.MaxArraySize)) + 1;
#if NetStandard21
            T?[] value = new T[length];
#else
            T[] value = new T[length];
#endif
            config.SaveHistory(typeof(T[]), value);
            while (length != 0) value[--length] = Creator<T>.Create(config);
            return value;
        }
        /// <summary>
        /// 创建随机字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        public static T? CreateDictionary<T, KT, VT>(Config config) where T : IDictionary<KT, VT?>
#else
        public static T CreateDictionary<T, KT, VT>(Config config) where T : IDictionary<KT, VT>
#endif
        {
            var customCreator = config.GetCustomCreator(typeof(T)).castType<Func<Config, bool, T>>();
            if (customCreator == null)
            {
                var historyValue = typeof(T).IsValueType ? null : config.TryGetValue(typeof(T));
                if (historyValue != null) return (T)historyValue;
                switch (AutoCSer.Random.Default.NextByte())
                {
                    case 0: return default(T);
                    case 1: return AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
                }
                var dictionary = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
                if (dictionary != null)
                {
                    if (!typeof(T).IsValueType) config.SaveHistory(typeof(T), dictionary);
                    for (int length = Math.Abs(AutoCSer.Random.Default.Next(config.MaxArraySize)) + 1; length != 0; --length)
                    {
                        var key = Creator<KT>.Create(config);
                        if (key != null) dictionary[key] = Creator<VT>.Create(config);
                    }
                }
                return dictionary;
            }
            return customCreator(config, true);
        }
        /// <summary>
        /// 创建随机集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        public static T? CreateCollection<T, VT>(Config config) where T : ICollection<VT?>
#else
        public static T CreateCollection<T, VT>(Config config) where T : ICollection<VT>
#endif
        {
            var customCreator = config.GetCustomCreator(typeof(T)).castType<Func<Config, bool, T>>();
            if (customCreator == null)
            {
                var historyValue = typeof(T).IsValueType ? null : config.TryGetValue(typeof(T));
                if (historyValue != null) return (T)historyValue;
                switch (AutoCSer.Random.Default.NextByte())
                {
                    case 0: return default(T);
                    case 1: return AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
                }
                var collection = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
                if (collection != null)
                {
                    if (!typeof(T).IsValueType) config.SaveHistory(typeof(T), collection);
                    for (int length = Math.Abs(AutoCSer.Random.Default.Next(config.MaxArraySize)) + 1; length != 0; --length)
                    {
                        collection.Add(Creator<VT>.Create(config));
                    }
                }
                return collection;
            }
            return customCreator(config, true);
        }
        /// <summary>
        /// 创建随机键值对
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static KeyValue<KT?, VT?> CreateKeyValue<KT, VT>(Config config)
        {
            return new KeyValue<KT?, VT?>(Create<KT>(config, true), Create<VT>(config, true));
        }
#else
        public static KeyValue<KT, VT> CreateKeyValue<KT, VT>(Config config)
        {
            return new KeyValue<KT, VT>(Create<KT>(config, true), Create<VT>(config, true));
        }
#endif
        /// <summary>
        /// 创建随机键值对
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static KeyValuePair<KT?, VT?> CreateKeyValuePair<KT, VT>(Config config)
        {
            return new KeyValuePair<KT?, VT?>(Create<KT>(config, true), Create<VT>(config, true));
        }
#else
        public static KeyValuePair<KT, VT> CreateKeyValuePair<KT, VT>(Config config)
        {
            return new KeyValuePair<KT, VT>(Create<KT>(config, true), Create<VT>(config, true));
        }
#endif
#if AOT
        /// <summary>
        /// 创建随机数组
        /// </summary>
        internal static readonly MethodInfo CreateArrayMethod = typeof(Creator).GetMethod(nameof(CreateArray), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        internal static readonly MethodInfo CreateNullableMethod = typeof(Creator).GetMethod(nameof(CreateNullable), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 创建随机字典
        /// </summary>
        internal static readonly MethodInfo CreateDictionaryMethod = typeof(Creator).GetMethod(nameof(CreateDictionary), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 创建随机集合
        /// </summary>
        internal static readonly MethodInfo CreateCollectionMethod = typeof(Creator).GetMethod(nameof(CreateCollection), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 创建随机键值对
        /// </summary>
        internal static readonly MethodInfo CreateKeyValueMethod = typeof(Creator).GetMethod(nameof(CreateKeyValue), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 创建随机键值对
        /// </summary>
        internal static readonly MethodInfo CreateKeyValuePairMethod = typeof(Creator).GetMethod(nameof(CreateKeyValuePair), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 代码生成调用激活 AOT 反射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T? CallCreate<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] T>(object? parameter = null)
        {
            return Creator.Create<T>(DefaultConfig, false);
        }
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal static void ReflectionMethodName<T>(object value) { }
#endif

        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool CreateBool(Config config)
        {
            return AutoCSer.Random.Default.NextBit() != 0;
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte CreateByte(Config config)
        {
            return AutoCSer.Random.Default.NextByte();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static sbyte CreateSByte(Config config)
        {
            return (sbyte)AutoCSer.Random.Default.NextByte();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static short CreateShort(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return short.MinValue;
                case 1: return short.MaxValue;
            }
            return (short)AutoCSer.Random.Default.NextUShort();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ushort CreateUShort(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return ushort.MinValue;
                case 1: return ushort.MaxValue;
            }
            return AutoCSer.Random.Default.NextUShort();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int CreateInt(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return int.MinValue;
                case 1: return int.MaxValue;
            }
            return AutoCSer.Random.Default.Next();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static uint CreateUInt(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return uint.MinValue;
                case 1: return uint.MaxValue;
            }
            return (uint)AutoCSer.Random.Default.Next();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static long CreateLong(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return long.MinValue;
                case 1: return long.MaxValue;
            }
            return (long)AutoCSer.Random.Default.NextULong();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ulong CreateULong(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return ulong.MinValue;
                case 1: return ulong.MaxValue;
            }
            return AutoCSer.Random.Default.NextULong();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Int128 CreateInt128(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return new Int128(((ulong)long.MaxValue) + 1, 0);
                case 1: return new Int128(long.MaxValue, ulong.MaxValue);
            }
            return new Int128(AutoCSer.Random.Default.NextULong(), AutoCSer.Random.Default.NextULong());
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static UInt128 CreateUInt128(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return new UInt128(0, 0);
                case 1: return new UInt128(ulong.MaxValue, ulong.MaxValue);
            }
            return new UInt128(AutoCSer.Random.Default.NextULong(), AutoCSer.Random.Default.NextULong());
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Half CreateHalf(Config config)
        {
#if NET8
            if (config.IsParseFloat)
            {
                switch (AutoCSer.Random.Default.NextByte())
                {
                    case 0: return Half.MinValue;
                    case 1: return Half.MaxValue;
                    case 2: return Half.Epsilon;
                }
                Half value = AutoCSer.Random.Default.NextHalf();
                if (Half.IsNaN(value) || Half.IsInfinity(value)) return Half.MinValue;
                return Half.Parse(value.ToString());
            }
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return Half.MinValue;
                case 1: return Half.MaxValue;
                case 2: return Half.Epsilon;
                case 3: return Half.NaN;
                case 4: return Half.PositiveInfinity;
                case 5: return Half.NegativeInfinity;
            }
            return AutoCSer.Random.Default.NextHalf();
#else
            ushort ushortValue = CreateUShort(config);
            unsafe { return *(Half*)&ushortValue; }
#endif
        }
        /// <summary>
        /// 创建随机数据
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Numerics.Complex CreateComplex(Config config)
        {
            return new System.Numerics.Complex(CreateDouble(config), CreateDouble(config));
        }
        /// <summary>
        /// 创建随机数据
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Numerics.Plane CreatePlane(Config config)
        {
            return new System.Numerics.Plane(CreateVector3(config), CreateFloat(config));
        }
        /// <summary>
        /// 创建随机数据
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Numerics.Quaternion CreateQuaternion(Config config)
        {
            return new System.Numerics.Quaternion(CreateVector3(config), CreateFloat(config));
        }
        /// <summary>
        /// 创建随机数据
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Numerics.Matrix3x2 CreateMatrix3x2(Config config)
        {
            return new System.Numerics.Matrix3x2(CreateFloat(config), CreateFloat(config), CreateFloat(config), CreateFloat(config), CreateFloat(config), CreateFloat(config));
        }
        /// <summary>
        /// 创建随机数据
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Numerics.Matrix4x4 CreateMatrix4x4(Config config)
        {
            return new System.Numerics.Matrix4x4(CreateFloat(config), CreateFloat(config), CreateFloat(config), CreateFloat(config)
                , CreateFloat(config), CreateFloat(config), CreateFloat(config), CreateFloat(config)
                , CreateFloat(config), CreateFloat(config), CreateFloat(config), CreateFloat(config)
                , CreateFloat(config), CreateFloat(config), CreateFloat(config), CreateFloat(config));
        }
        /// <summary>
        /// 创建随机数据
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Numerics.Vector2 CreateVector2(Config config)
        {
            return new System.Numerics.Vector2(CreateFloat(config), CreateFloat(config));
        }
        /// <summary>
        /// 创建随机数据
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Numerics.Vector3 CreateVector3(Config config)
        {
            return new System.Numerics.Vector3(CreateVector2(config), CreateFloat(config));
        }
        /// <summary>
        /// 创建随机数据
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Numerics.Vector4 CreateVector4(Config config)
        {
            return new System.Numerics.Vector4(CreateVector3(config), CreateFloat(config));
        }
        /// <summary>
        /// 随机数除数
        /// </summary>
        private static readonly decimal decimalDiv = 100;
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static decimal CreateDecimal(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return config.MinDecimal;
                case 1: return config.MaxDecimal;
            }
            decimal value = (decimal)(long)AutoCSer.Random.Default.NextULong() / decimalDiv;
            if (value >= config.MinDecimal && value <= config.MaxDecimal) return value;
            decimal mod = config.MaxDecimal - config.MinDecimal;
            value %= mod;
            if (value < 0) value += mod;
            return value + config.MinDecimal;
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static System.Guid CreateGuid(Config config)
        {
            return System.Guid.NewGuid();
        }
        /// <summary>
        /// 创建随机字符
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static char CreateChar(Config config)
        {
            if (config.IsAscii) return (char)((AutoCSer.Random.Default.NextByte() % (0x7f - ' ')) + ' ');
            if (config.IsNullChar) return (char)AutoCSer.Random.Default.NextUShort();
            char value = (char)AutoCSer.Random.Default.NextUShort();
            return value == 0 ? char.MaxValue : value;
        }
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        public static string? CreateString(Config config)
#else
        public static string CreateString(Config config)
#endif
        {
            var historyValue = config.TryGetValue(typeof(string));
            if (historyValue != null) return (string)historyValue;
            var charArray = createArray<char>(config);
            if (charArray == null) return null;
            string value = new string(charArray);
            config.SaveHistory(typeof(string), value);
            return value;
        }
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static SubString CreateSubString(Config config)
        {
            return CreateString(config) ?? string.Empty;
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static float CreateFloat(Config config)
        {
            if (config.IsParseFloat)
            {
                switch (AutoCSer.Random.Default.NextByte())
                {
                    case 0: return float.MinValue;
                    case 1: return float.MaxValue;
                    case 2: return float.Epsilon;
                }
                float value = AutoCSer.Random.Default.NextFloat();
                if (float.IsNaN(value) || float.IsInfinity(value)) return float.MinValue;
                return float.Parse(value.ToString());
            }
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return float.MinValue;
                case 1: return float.MaxValue;
                case 2: return float.Epsilon;
                case 3: return float.NaN;
                case 4: return float.PositiveInfinity;
                case 5: return float.NegativeInfinity;
            }
            return AutoCSer.Random.Default.NextFloat();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static double CreateDouble(Config config)
        {
            if (config.IsParseFloat)
            {
                switch (AutoCSer.Random.Default.NextByte())
                {
                    case 0: return double.MinValue;
                    case 1: return double.MaxValue;
                    case 2: return double.Epsilon;
                    case 3: return double.Epsilon;
                    case 10: return 1.7976931348623157E+308;
                    case 11: return 1.7976931348623156E+308;
                    case 12: return 1.7976931348623155E+308;
                    case 13: return 1.7976931348623154E+308;
                    case 14: return 1.7976931348623153E+308;
                    case 15: return 1.7976931348623152E+308;
                    case 16: return 1.7976931348623151E+308;
                    case 17: return 1.7976931348623150E+308;
                    case 20: return -1.7976931348623157E+308;
                    case 21: return -1.7976931348623156E+308;
                    case 22: return -1.7976931348623155E+308;
                    case 23: return -1.7976931348623154E+308;
                    case 24: return -1.7976931348623153E+308;
                    case 25: return -1.7976931348623152E+308;
                    case 26: return -1.7976931348623151E+308;
                    case 27: return -1.7976931348623150E+308;
                }
                double value = AutoCSer.Random.Default.NextDouble();
                if (double.IsNaN(value) || double.IsInfinity(value)) return double.MinValue;
                return double.Parse(value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return double.MinValue;
                case 1: return double.MaxValue;
                case 2: return double.Epsilon;
                case 3: return double.Epsilon;
                case 4: return double.NaN;
                case 5: return double.PositiveInfinity;
                case 6: return double.NegativeInfinity;
                case 10: return 1.7976931348623157E+308;
                case 11: return 1.7976931348623156E+308;
                case 12: return 1.7976931348623155E+308;
                case 13: return 1.7976931348623154E+308;
                case 14: return 1.7976931348623153E+308;
                case 15: return 1.7976931348623152E+308;
                case 16: return 1.7976931348623151E+308;
                case 17: return 1.7976931348623150E+308;
                case 20: return -1.7976931348623157E+308;
                case 21: return -1.7976931348623156E+308;
                case 22: return -1.7976931348623155E+308;
                case 23: return -1.7976931348623154E+308;
                case 24: return -1.7976931348623153E+308;
                case 25: return -1.7976931348623152E+308;
                case 26: return -1.7976931348623151E+308;
                case 27: return -1.7976931348623150E+308;
            }
            return AutoCSer.Random.Default.NextDouble();
        }
        /// <summary>
        /// 创建随机时间
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static DateTime CreateDateTime(Config config)
        {
            long startTicks = config.MinDateTime.Ticks, mod = config.MaxDateTime.Ticks - startTicks;
            if (config.IsSecondDateTime)
            {
                return new DateTime(((long)(AutoCSer.Random.Default.NextULong() % (ulong)mod) + startTicks) / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond);
            }
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return config.MinDateTime;
                case 1: return config.MaxDateTime;
            }
            return new DateTime((long)(AutoCSer.Random.Default.NextULong() % (ulong)mod) + startTicks);
        }
        /// <summary>
        /// 创建随机时间戳
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static DateTimeOffset CreateDateTimeOffset(Config config)
        {
            return CreateDateTime(config);
        }
        /// <summary>
        /// 创建随机时间
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TimeSpan CreateTimeSpan(Config config)
        {
            return new TimeSpan((long)(AutoCSer.Random.Default.NextULong() % (ulong)TimeSpan.TicksPerDay));
        }

        /// <summary>
        /// 获取随机对象生成成员
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        internal static IEnumerable<Member> GetRandomObjectFields(FieldInfo[] fields)
        {
            foreach (FieldInfo field in fields)
            {
                MemberInfo member = field.getPropertyMemberInfo() ?? field;
                if (member.GetCustomAttribute(typeof(IgnoreAttribute), false) == null) yield return new Member(field, member, member.GetCustomAttribute<MemberAttribute>(false) ?? MemberAttribute.Default);
            }
        }
        /// <summary>
        /// 基本类型随机数创建函数信息集合
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, Delegate> createDelegates;
        /// <summary>
        /// 获取基本类型随机数创建函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns>基本类型随机数创建函数信息</returns>
#if NetStandard21
        internal static Delegate? GetDelegate(Type type)
#else
        internal static Delegate GetDelegate(Type type)
#endif
        {
            var method = default(Delegate);
            if (!createDelegates.TryGetValue(type, out method)) return null;
            //createDelegates.Remove(type);
            return method;
        }

        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Create<T>(ref T value, Config? config = null)
#else
        public static void Create<T>(ref T value, Config config = null)
#endif
        {
            Creator<T>.Create(ref value, config);
        }

        static Creator()
        {
            createDelegates = DictionaryCreator.CreateHashObject<System.Type, Delegate>();
            createDelegates.Add(typeof(bool), (Func<Config, bool>)CreateBool);
            createDelegates.Add(typeof(byte), (Func<Config, byte>)CreateByte);
            createDelegates.Add(typeof(sbyte), (Func<Config, sbyte>)CreateSByte);
            createDelegates.Add(typeof(short), (Func<Config, short>)CreateShort);
            createDelegates.Add(typeof(ushort), (Func<Config, ushort>)CreateUShort);
            createDelegates.Add(typeof(int), (Func<Config, int>)CreateInt);
            createDelegates.Add(typeof(uint), (Func<Config, uint>)CreateUInt);
            createDelegates.Add(typeof(long), (Func<Config, long>)CreateLong);
            createDelegates.Add(typeof(ulong), (Func<Config, ulong>)CreateULong);
            createDelegates.Add(typeof(decimal), (Func<Config, decimal>)CreateDecimal);
            createDelegates.Add(typeof(Guid), (Func<Config, Guid>)CreateGuid);
            createDelegates.Add(typeof(char), (Func<Config, char>)CreateChar);
            createDelegates.Add(typeof(SubString), (Func<Config, SubString>)CreateSubString);
            createDelegates.Add(typeof(float), (Func<Config, float>)CreateFloat);
            createDelegates.Add(typeof(double), (Func<Config, double>)CreateDouble);
            createDelegates.Add(typeof(DateTime), (Func<Config, DateTime>)CreateDateTime);
            createDelegates.Add(typeof(DateTimeOffset), (Func<Config, DateTimeOffset>)CreateDateTimeOffset);
            createDelegates.Add(typeof(TimeSpan), (Func<Config, TimeSpan>)CreateTimeSpan);
#if NetStandard21
            createDelegates.Add(typeof(string), (Func<Config, string?>)CreateString);
#else
            createDelegates.Add(typeof(string), (Func<Config, string>)CreateString);
#endif
            createDelegates.Add(typeof(Half), (Func<Config, Half>)CreateHalf);
            createDelegates.Add(typeof(Int128), (Func<Config, Int128>)CreateInt128);
            createDelegates.Add(typeof(UInt128), (Func<Config, UInt128>)CreateUInt128);
            createDelegates.Add(typeof(System.Numerics.Complex), (Func<Config, System.Numerics.Complex>)CreateComplex);
            createDelegates.Add(typeof(System.Numerics.Plane), (Func<Config, System.Numerics.Plane>)CreatePlane);
            createDelegates.Add(typeof(System.Numerics.Quaternion), (Func<Config, System.Numerics.Quaternion>)CreateQuaternion);
            createDelegates.Add(typeof(System.Numerics.Matrix3x2), (Func<Config, System.Numerics.Matrix3x2>)CreateMatrix3x2);
            createDelegates.Add(typeof(System.Numerics.Matrix4x4), (Func<Config, System.Numerics.Matrix4x4>)CreateMatrix4x4);
            createDelegates.Add(typeof(System.Numerics.Vector2), (Func<Config, System.Numerics.Vector2>)CreateVector2);
            createDelegates.Add(typeof(System.Numerics.Vector3), (Func<Config, System.Numerics.Vector3>)CreateVector3);
            createDelegates.Add(typeof(System.Numerics.Vector4), (Func<Config, System.Numerics.Vector4>)CreateVector4);

            int clearSeconds = AutoCSer.Common.Config.GetMemoryCacheClearSeconds();
            if (clearSeconds > 0) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(DefaultConfig.ClearHistory, clearSeconds, Threading.SecondTimerKeepModeEnum.After, clearSeconds);
#if !AOT
            AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(Creator));
#endif
        }
    }
    /// <summary>
    /// 随机对象生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Creator<T>
    {
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
        internal delegate void MemberCreator(ref T value, Config config);
        /// <summary>
        /// 创建随机对象
        /// </summary>
        private static readonly MemberCreator memberCreator;
        /// <summary>
        /// 基本类型随机数创建函数
        /// </summary>
#if NetStandard21
        private static readonly Func<Config, T?>? defaultCreator;
#else
        private static readonly Func<Config, T> defaultCreator;
#endif

        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        public static T? Create(Config? config = null)
#else
        public static T Create(Config config = null)
#endif
        {
            if (config == null) config = Creator.DefaultConfig;
            if (defaultCreator != null) return defaultCreator(config);
            var value = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
            if (value != null) create(ref value, config);
            return value;
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        public static T CreateNotNull(Config? config = null)
#else
        public static T CreateNotNull(Config config = null)
#endif
        {
            if (config == null) config = Creator.DefaultConfig;
            if (defaultCreator != null)
            {
                do
                {
                    var value = defaultCreator(config);
                    if (value != null) return value;
                }
                while (true);
            }
            var newValue = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
            if (newValue != null)
            {
                create(ref newValue, config);
                return newValue;
            }
            throw new NullReferenceException();
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
#if NetStandard21
        private static void create(ref T value, Config config)
#else
        private static void create(ref T value, Config config = null)
#endif
        {
            try
            {
                memberCreator(ref value, config);
            }
            finally { config.ClearHistory(); }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
#if NetStandard21
        public static void Create(ref T value, Config? config = null)
#else
        public static void Create(ref T value, Config config = null)
#endif
        {
            create(ref value, config ?? Creator.DefaultConfig);
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CreateMember(ref T value, Config config)
        {
            memberCreator(ref value, config);
        }

        /// <summary>
        /// 不支持类型创建默认值
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        private static T? createDefault(Config config)
#else
        private static T createDefault(Config config)
#endif
        {
            var customCreator = config.GetCustomCreator(typeof(T)).castType<Func<Config, bool, T>>();
            return customCreator == null ? config.CreateNotSupport<T>() : customCreator(config, true);
        }
        /// <summary>
        /// 不支持类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
        private static void createDefault(ref T value, Config config) { }

        /// <summary>
        /// 获取随机数创建委托
        /// </summary>
        /// <returns>随机数创建委托</returns>
#if NetStandard21
        private static Delegate? getDelegate()
#else
        private static Delegate getDelegate()
#endif
        {
            Type type = typeof(T);
            var createDelegate = Creator.GetDelegate(type);
            if (createDelegate != null) return createDelegate;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
#if AOT
                    return Creator.CreateArrayMethod.MakeGenericMethod(type.GetElementType().notNull()).CreateDelegate(typeof(Func<Config, T?>));
#else
                    return GenericType.Get(type.GetElementType().notNull()).CreateArrayDelegate;
#endif
                }
#if NetStandard21
                return (Func<Config, T?>)createDefault;
#else
                return (Func<Config, T>)createDefault;
#endif
            }
            if (type.IsEnum)
            {
#if AOT
                switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return Creator.CreateEnumIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<Config, T?>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return Creator.CreateEnumUIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<Config, T?>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return Creator.CreateEnumByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<Config, T?>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return  Creator.CreateEnumULongMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<Config, T?>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return Creator.CreateEnumUShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<Config, T?>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return Creator.CreateEnumLongMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<Config, T?>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return Creator.CreateEnumShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<Config, T?>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return Creator.CreateEnumSByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<Config, T?>));
                }
#else
                return EnumGenericType.Get(type).CreateEnumDelegate;
#endif
            }
            if (type.isValueTypeNullable())
            {
#if AOT
                return Creator.CreateNullableMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(Func<Config, T?>));
#else
                return StructGenericType.Get(type.GetGenericArguments()[0]).CreateNullableDelegate;
#endif
            }
#if AOT
            if (type.IsValueType && type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(KeyValuePair<,>)) return Creator.CreateKeyValuePairMethod.MakeGenericMethod(type.GetGenericArguments()).CreateDelegate(typeof(Func<Config, T?>));
                if (genericType == typeof(KeyValue<,>)) return Creator.CreateKeyValueMethod.MakeGenericMethod(type.GetGenericArguments()).CreateDelegate(typeof(Func<Config, T?>));
            }
#endif
            var collectionType = default(Type);
            foreach (Type interfaceType in type.getGenericInterface())
            {
                Type genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof(IDictionary<,>))
                {
#if AOT
                    Type[] types = interfaceType.GetGenericArguments();
                    return Creator.CreateDictionaryMethod.MakeGenericMethod(type, types[0], types[1]).CreateDelegate(typeof(Func<Config, T?>));
#else
                    return DictionaryGenericType.Get(type, interfaceType).CreateDictionaryDelegate;
#endif
                }
                if (collectionType == null && genericType == typeof(ICollection<>)) collectionType = interfaceType;
            }
            if(collectionType != null)
            {
#if AOT
                return Creator.CreateCollectionMethod.MakeGenericMethod(type, collectionType.GetGenericArguments()[0]).CreateDelegate(typeof(Func<Config, T?>));
#else
                return CollectionGenericType.Get(type, collectionType).CreateCollectionDelegate;
#endif
            }
#if AOT
            if (!type.IsValueType && AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructorMethod.MakeGenericMethod(type).Invoke(null, null) == null)
#else
            if (!AutoCSer.Metadata.GenericType.Get(type).IsSerializeConstructor)
#endif
            {
#if NetStandard21
                return (Func<Config, T?>)createDefault;
#else
                return (Func<Config, T>)createDefault;
#endif
            }
            return null;
        }
        static Creator()
        {
            var createDelegate = getDelegate();
            if (createDelegate != null)
            {
#if NetStandard21
                defaultCreator = (Func<Config, T?>)createDelegate;
#else
                defaultCreator = (Func<Config, T>)createDelegate;
#endif
                memberCreator = createDefault;
                return;
            }
            Type type = typeof(T);
#if AOT
            var method = type.GetMethod(AutoCSer.CodeGenerator.RandomObjectAttribute.CreateRandomObjectMethodName, BindingFlags.Static | BindingFlags.NonPublic, new Type[] { type.MakeByRefType(), typeof(Config) });
            if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(void))
            {
                memberCreator = (MemberCreator)method.CreateDelegate(typeof(MemberCreator));
                return;
            }
            throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.RandomObjectAttribute.CreateRandomObjectMethodName);
#else
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
            MemberDynamicMethod dynamicMethod = new MemberDynamicMethod(type);
            foreach (Member field in Creator.GetRandomObjectFields(fields)) dynamicMethod.Push(field.Field, field.Attribute);
            dynamicMethod.Base();
            memberCreator = (MemberCreator)dynamicMethod.Create(typeof(MemberCreator));
#endif
        }
    }
}
