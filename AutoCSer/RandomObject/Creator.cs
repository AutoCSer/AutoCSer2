using AutoCSer.Extensions;
using AutoCSer.RandomObject.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        internal static T? Create<T>(Config config, bool isNullable)
#else
        internal static T Create<T>(Config config, bool isNullable)
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
        internal static void CreateMember<T>(ref T value, Config config)
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
        internal static T? CreateNullable<T>(Config config) where T : struct
        {
            if (createBool(config)) return Create<T>(config, false);
            return new T?();
        }
        /// <summary>
        /// 创建随机数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        internal static T?[]? CreateArray<T>(Config config)
#else
        internal static T[] CreateArray<T>(Config config)
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
        internal static T? CreateDictionary<T, KT, VT>(Config config) where T : IDictionary<KT, VT?>
#else
        internal static T CreateDictionary<T, KT, VT>(Config config) where T : IDictionary<KT, VT>
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
        internal static T? CreateCollection<T, VT>(Config config) where T : ICollection<VT?>
#else
        internal static T CreateCollection<T, VT>(Config config) where T : ICollection<VT>
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
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool createBool(Config config)
        {
            return AutoCSer.Random.Default.NextBit() != 0;
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte CreateByte(Config config)
        {
            return AutoCSer.Random.Default.NextByte();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static sbyte CreateSByte(Config config)
        {
            return (sbyte)AutoCSer.Random.Default.NextByte();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static short CreateShort(Config config)
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
        internal static ushort CreateUShort(Config config)
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
        internal static int CreateInt(Config config)
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
        internal static uint CreateUInt(Config config)
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
        internal static long CreateLong(Config config)
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
        internal static ulong CreateULong(Config config)
        {
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return ulong.MinValue;
                case 1: return ulong.MaxValue;
            }
            return AutoCSer.Random.Default.NextULong();
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
        private static decimal createDecimal(Config config)
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
        private static System.Guid createGuid(Config config)
        {
            return System.Guid.NewGuid();
        }
        /// <summary>
        /// 创建随机字符
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static char createChar(Config config)
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
        private static string? createString(Config config)
#else
        private static string createString(Config config)
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
        private static SubString createSubString(Config config)
        {
            return createString(config) ?? string.Empty;
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static float createFloat(Config config)
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
        private static double createDouble(Config config)
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
        internal static DateTime CreateDateTime(Config config)
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
        private static DateTimeOffset createDateTimeOffset(Config config)
        {
            return CreateDateTime(config);
        }
        /// <summary>
        /// 创建随机时间
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static TimeSpan CreateTimeSpan(Config config)
        {
            return new TimeSpan((long)(AutoCSer.Random.Default.NextULong() % (ulong)TimeSpan.TicksPerDay));
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
            createDelegates.Remove(type);
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
            createDelegates.Add(typeof(bool), (Func<Config, bool>)createBool);
            createDelegates.Add(typeof(byte), (Func<Config, byte>)CreateByte);
            createDelegates.Add(typeof(sbyte), (Func<Config, sbyte>)CreateSByte);
            createDelegates.Add(typeof(short), (Func<Config, short>)CreateShort);
            createDelegates.Add(typeof(ushort), (Func<Config, ushort>)CreateUShort);
            createDelegates.Add(typeof(int), (Func<Config, int>)CreateInt);
            createDelegates.Add(typeof(uint), (Func<Config, uint>)CreateUInt);
            createDelegates.Add(typeof(long), (Func<Config, long>)CreateLong);
            createDelegates.Add(typeof(ulong), (Func<Config, ulong>)CreateULong);
            createDelegates.Add(typeof(decimal), (Func<Config, decimal>)createDecimal);
            createDelegates.Add(typeof(Guid), (Func<Config, Guid>)createGuid);
            createDelegates.Add(typeof(char), (Func<Config, char>)createChar);
            createDelegates.Add(typeof(SubString), (Func<Config, SubString>)createSubString);
            createDelegates.Add(typeof(float), (Func<Config, float>)createFloat);
            createDelegates.Add(typeof(double), (Func<Config, double>)createDouble);
            createDelegates.Add(typeof(DateTime), (Func<Config, DateTime>)CreateDateTime);
            createDelegates.Add(typeof(DateTimeOffset), (Func<Config, DateTimeOffset>)createDateTimeOffset);
            createDelegates.Add(typeof(TimeSpan), (Func<Config, TimeSpan>)CreateTimeSpan);
#if NetStandard21
            createDelegates.Add(typeof(string), (Func<Config, string?>)createString);
#else
            createDelegates.Add(typeof(string), (Func<Config, string>)createString);
#endif

            AutoCSer.Memory.Common.AddClearCache(DefaultConfig.ClearHistory, AutoCSer.Common.Config.GetMemoryCacheClearSeconds());
            AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(Creator));
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
                if (type.GetArrayRank() == 1) return GenericType.Get(type.GetElementType().notNull()).CreateArrayDelegate;
#if NetStandard21
                return (Func<Config, T?>)createDefault;
#else
                return (Func<Config, T>)createDefault;
#endif
            }
            if (type.IsEnum) return EnumGenericType.Get(type).CreateEnumDelegate;
            if (type.isValueTypeNullable()) return StructGenericType.Get(type.GetGenericArguments()[0]).CreateNullableDelegate;
            foreach (Type interfaceType in type.getGenericInterface())
            {
                Type genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof(IDictionary<,>)) return DictionaryGenericType.Get(type, interfaceType).CreateDictionaryDelegate;
                if (genericType == typeof(ICollection<>)) return CollectionGenericType.Get(type, interfaceType).CreateCollectionDelegate;
            }
            if (!AutoCSer.Metadata.GenericType<T>.GetIsSerializeConstructor())
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
            MemberDynamicMethod dynamicMethod = new MemberDynamicMethod(type);
            foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                MemberInfo member = field.getPropertyMemberInfo() ?? field;
                if (member.GetCustomAttribute(typeof(IgnoreAttribute), false) == null)
                {
                    MemberAttribute attribute = member.GetCustomAttribute<MemberAttribute>(false) ?? MemberAttribute.Default;
                    dynamicMethod.Push(field, attribute);
                }
            }
            dynamicMethod.Base();
            memberCreator = (MemberCreator)dynamicMethod.Create(typeof(MemberCreator));
        }
    }
}
