using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public static partial class Common
    {
        /// <summary>
        /// 名称前缀
        /// </summary>
        public const string NamePrefix = "AutoCSer";
        /// <summary>
        /// 公共配置参数
        /// </summary>
        public static readonly AutoCSer.Config Config;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        internal static readonly AutoCSer.Metadata.DefaultConstructor DefaultConstructor;
        /// <summary>
        /// 默认系统语言文化配置
        /// </summary>
        internal static readonly AutoCSer.Culture.Configuration Culture;
        /// <summary>
        /// 是否小端储存模式（序列化操作仅支持小端模式）
        /// </summary>
        internal static readonly bool IsLittleEndian;

        /// <summary>
        /// 当前进程信息
        /// </summary>
        public static readonly Process CurrentProcess = Process.GetCurrentProcess();
        /// <summary>
        /// CPU 逻辑处理器数量（线程数量）
        /// </summary>
        public static readonly int ProcessorCount = Math.Max(Environment.ProcessorCount, 1);
        /// <summary>
        /// 设置当前进程的 CPU 亲缘性
        /// </summary>
        /// <param name="processorIndex">逻辑处理器编号，从 0 开始</param>
        /// <returns>是否设置成功</returns>
#if NET8
        [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
        [SupportedOSPlatform(SupportedOSPlatformName.Linux)]
#endif
        public static bool SetCurrentProcessAffinity(byte processorIndex)
        {
            if (processorIndex < ProcessorCount)
            {
                CurrentProcess.ProcessorAffinity = (IntPtr)(1UL << processorIndex);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置当前进程的 CPU 亲缘性
        /// </summary>
        /// <param name="processorIndexs">逻辑处理器编号集合，从 0 开始</param>
        /// <returns>是否设置成功</returns>
#if NET8
        [SupportedOSPlatform(SupportedOSPlatformName.Windows)]
        [SupportedOSPlatform(SupportedOSPlatformName.Linux)]
#endif
        public static bool SetCurrentProcessAffinity(params byte[] processorIndexs)
        {
            ulong mark = 0;
            foreach(byte processorIndex in processorIndexs)
            {
                if (processorIndex < ProcessorCount) mark |= 1UL << processorIndex;
                else return false;
            }
            if (mark != 0)
            {
                CurrentProcess.ProcessorAffinity = (IntPtr)mark;
                return true;
            }
            return false;
        }
        /// <summary>
        /// CPU 高速缓存块字节大小，影响 AutoCSer.Threading.BlockPool 硬编码填充大小
        /// </summary>
        public const int CpuCacheBlockSize = 64;
        /// <summary>
        /// 每个 CPU 高速缓存块容纳对象引用数量
        /// </summary>
        public static readonly unsafe int CpuCacheBlockObjectCount = CpuCacheBlockSize / sizeof(IntPtr);

        /// <summary>
        /// Encoding.Unicode.CodePage
        /// </summary>
        public static int UnicodeCodePage = Encoding.Unicode.CodePage;

        /// <summary>
        /// 程序执行主目录
        /// </summary>
        public static readonly DirectoryInfo ApplicationDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory);
        /// <summary>
        /// 是否代码生成环境
        /// </summary>
        internal static bool IsCodeGenerator;

        /// <summary>
        /// 获取自定义反序列化委托数据类型
        /// </summary>
        /// <param name="deserializerType"></param>
        /// <param name="deserializeDelegate"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? CheckDeserializeType(Type deserializerType, Delegate deserializeDelegate)
#else
        internal static Type CheckDeserializeType(Type deserializerType, Delegate deserializeDelegate)
#endif
        {
            MethodInfo methodInfo = deserializeDelegate.Method;
            if (methodInfo.IsStatic)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length == 2)
                {
                    if (parameters[0].ParameterType == deserializerType)
                    {
                        Type parameterType = parameters[1].ParameterType;
                        if (parameterType.IsByRef && !parameters[1].IsOut) return parameterType.GetElementType();
                        AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型反序列化函数 {methodInfo.DeclaringType?.fullName()}.{methodInfo.Name} 第二个参数类型必须为 ref", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
                    }
                    else AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型反序列化函数 {methodInfo.DeclaringType?.fullName()}.{methodInfo.Name} 第一个参数类型必须为 {deserializerType.fullName()}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
                }
                else AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型反序列化函数 {methodInfo.DeclaringType?.fullName()}.{methodInfo.Name} 参数数量 {parameters.Length.toString()} 不匹配", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            }
            else AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型反序列化函数 {methodInfo.DeclaringType?.fullName()}.{methodInfo.Name} 必须为非静态函数", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            return null;
        }
#if NetStandard21
        /// <summary>
        /// 等待任务完成
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Wait(ValueTask task)
        {
            task.AsTask().wait();
        }
        /// <summary>
        /// 获取 IAsyncEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeratorTask"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<T> GetAsyncEnumerable<T>(IEnumeratorCommand<T> enumeratorTask)
        {
            await using (enumeratorTask)
            {
                while (await enumeratorTask.MoveNext()) yield return enumeratorTask.Current;
            }
        }
#else
        /// <summary>
        /// 等待任务完成
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Wait(Task task)
        {
            task.Wait();
        }
#endif

        /// <summary>
        /// 获取默认值，消除警告
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T? GetDefault<T>()
#else
        internal static T GetDefault<T>()
#endif
        {
            return default(T);
        }
        /// <summary>
        /// 空对象
        /// </summary>
        internal static readonly object EmptyObject = new object();
        /// <summary>
        /// 空函数，用于消除空语句警告
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EmptyFunction() { }
        /// <summary>
        /// 空委托
        /// </summary>
        public static readonly Action EmptyAction = EmptyFunction;
        /// <summary>
        /// 空方法信息
        /// </summary>
        internal static readonly MethodInfo NullMethodInfo = EmptyAction.Method;
        /// <summary>
        /// 空事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
#if NetStandard21
        private static void emptyEventHandler(object? sender, EventArgs e) { }
#else
        private static void emptyEventHandler(object sender, EventArgs e) { }
#endif
        /// <summary>
        /// 空事件
        /// </summary>
        public static readonly EventHandler EmptyEventHandler = emptyEventHandler;

        /// <summary>
        /// 已完成任务返回 true
        /// </summary>
        public static readonly Task<bool> TrueCompletedTask = Task.FromResult(true);
        /// <summary>
        /// 根据逻辑值获取已完成任务
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<bool> GetCompletedTask(bool value)
        {
            return value ? TrueCompletedTask : CompletedTask<bool>.Default;
        }
        /// <summary>
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="value">任务返回值</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<T[]> GetCompletedTask<T>(T[] value) where T : class
        {
            return value.Length != 0 ? Task.FromResult(value) : EmptyArrayCompletedTask<T>.EmptyArray;
        }
        /// <summary>
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="value">任务返回值</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<T?> GetCompletedTask<T>(T? value) where T : class
#else
        public static Task<T> GetCompletedTask<T>(T value) where T : class
#endif
        {
#if NetStandard21
            return value != null ? Task.FromResult((T?)value) : CompletedTask<T>.Default;
#else
            return value != null ? Task.FromResult(value) : CompletedTask<T>.Default;
#endif
        }
        /// <summary>
        /// 已完成任务
        /// </summary>
#if DotNet45
        public static Task CompletedTask { get { return TrueCompletedTask; } }
#else
        public static Task CompletedTask { get { return Task.CompletedTask; } }
#endif
        /// <summary>
        /// 已完成任务
        /// </summary>
#if NetStandard21
#if NET8
        public static ValueTask CompletedValueTask { get { return ValueTask.CompletedTask; } }
#else
        public static ValueTask CompletedValueTask { get { return new ValueTask(CompletedTask); } }
#endif
#else
        public static ValueTask CompletedValueTask { get { return CompletedTask; } }
#endif
#if NetStandard21
        /// <summary>
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="value">任务返回值</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ValueTask<T> GetCompletedValueTask<T>(T value)
        {
#if NET8
            return ValueTask.FromResult(value);
#else
            return new ValueTask<T>(Task.FromResult(value));
#endif
        }
        /// <summary>
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="value">任务返回值</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ValueTask<T> GetCompletedValueTask<T>(Task<T> value)
        {
            return new ValueTask<T>(value);
        }
#endif

        /// <summary>
        /// 已完成 Awaiter 返回 true
        /// </summary>
        public static readonly CompletedTaskCastAwaiter<bool> TrueCompletedAwaiter = new CompletedTaskCastAwaiter<bool>(true);
        /// <summary>
        /// 根据逻辑值获取已完成 Awaiter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CompletedTaskCastAwaiter<bool> GetCompletedAwaiter(bool value)
        {
            return value ? TrueCompletedAwaiter : CompletedTaskCastAwaiter<bool>.Default;
        }

        /// <summary>
        /// 获取数组，允许存在未初始化数组项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="capacity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T[] GetUninitializedArray<T>(int capacity)
        {
#if NET8
            return GC.AllocateUninitializedArray<T>(capacity, false);
#else
            return new T[capacity];
#endif
        }
        /// <summary>
        /// 复制数组，允许存在未初始化数组项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <returns></returns>
#if NetStandard21
        public static T[]? GetUninitializedArray<T>(T[]? sourceArray)
#else
        public static T[] GetUninitializedArray<T>(T[] sourceArray)
#endif
        {
            if (sourceArray != null && sourceArray.Length != 0)
            {
                T[] newArray = GetUninitializedArray<T>(sourceArray.Length);
                CopyTo(sourceArray, newArray);
                return newArray;
            }
            return sourceArray;
        }
        /// <summary>
        /// 复制数组，允许存在未初始化数组项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T[] GetUninitializedArray<T>(T[] sourceArray, int capacity)
        {
            T[] newArray = GetUninitializedArray<T>(capacity);
            CopyTo(sourceArray, newArray);
            return newArray;
        }
        /// <summary>
        /// 复制数组，允许存在未初始化数组项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="capacity"></param>
        /// <param name="copyCount"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T[] GetUninitializedArray<T>(T[] sourceArray, int capacity, int copyCount)
        {
            T[] newArray = GetUninitializedArray<T>(capacity);
            CopyTo(sourceArray, newArray, 0, copyCount);
            return newArray;
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T[] GetCopyArray<T>(T[] sourceArray, int capacity)
        {
            T[] newArray = new T[capacity];
            CopyTo(sourceArray, newArray);
            return newArray;
        }
        /// <summary>
        /// 数组复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="destinationArray"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<T>(T[] sourceArray, T[] destinationArray)
        {
            sourceArray.CopyTo(destinationArray, 0);
        }
        /// <summary>
        /// 数组复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="destinationArray"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="length"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<T>(T[] sourceArray, T[] destinationArray, int destinationIndex, int length)
        {
            Array.Copy(sourceArray, 0, destinationArray, destinationIndex, length);
        }

        /// <summary>
        /// 填充整数
        /// </summary>
        /// <param name="src">串起始地址,不能为null</param>
        /// <param name="count">ulong 整数数量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Clear(ulong* src, int count)
        {
            if (count > 0) AutoCSer.Memory.Common.Clear(src, count);
        }
        /// <summary>
        /// 数据全部设置为 0
        /// </summary>
        /// <param name="src">串起始地址，不能为 null</param>
        /// <param name="size">字节数量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Clear(void* src, int size)
        {
            if (size > 0) AutoCSer.Memory.Common.Clear((byte*)src, size);
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="src">串起始地址,不能为null</param>
        /// <param name="count">ulong 整数数量</param>
        /// <param name="value">填充整数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Fill(void* src, int count, ulong value)
        {
            if (count > 0) AutoCSer.Memory.Common.Fill((ulong*)src, count, value);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="size">字节长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(void* source, void* destination, int size)
        {
            if (size > 0) AutoCSer.Memory.Common.Copy(source, destination, size);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="size">字节长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(void* source, void* destination, long size)
        {
            if (size > 0) AutoCSer.Memory.Common.Copy(source, destination, size);
        }
        /// <summary>
        /// 内存数据转换成字节数组
        /// </summary>
        /// <param name="source">串起始地址，不能为 null</param>
        /// <param name="size">字节长度,必须大于0</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static unsafe byte[] GetArray(void* source, int size)
        {
            byte[] array = GetUninitializedArray<byte>(size);
            CopyTo(source, array);
            return array;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原串起始地址</param>
        /// <param name="destination">目标数据</param>
        /// <param name="destinationIndex">目标数据起始位置</param>
        /// <param name="size">字节长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyTo(void* source, byte[] destination, int destinationIndex, int size)
        {
#if DEBUG
            if (destinationIndex < 0) throw new Exception(destinationIndex.toString() + " < 0");
            if (destinationIndex + size > destination.Length) throw new Exception(destinationIndex.toString() + " + " + size.toString() + " > " + destination.Length.toString());
#endif
            fixed (byte* destinationFixed = destination) CopyTo(source, destinationFixed + destinationIndex, size);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">原数据</param>
        /// <param name="sourceIndex">原数据起始位置</param>
        /// <param name="destination">目标串起始地址</param>
        /// <param name="size">字节长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(byte[] source, int sourceIndex, void* destination, int size)
        {
#if DEBUG
            if (sourceIndex < 0) throw new Exception(sourceIndex.toString() + " < 0");
            if (sourceIndex + size > source.Length) throw new Exception(sourceIndex.toString() + " + " + size.toString() + " > " + source.Length.toString());
#endif
            fixed (byte* sourceFixed = source) CopyTo(sourceFixed + sourceIndex, destination, size);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">字符串，长度必须大于0</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(string source, void* destination)
        {
#if DEBUG
            if (source.Length > int.MaxValue >> 1) throw new Exception(source.Length.toString() + " > int.MaxValue / 2");
#endif
            fixed (char* sourceFixed = source) CopyTo(sourceFixed, destination, source.Length << 1);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">字符串，长度必须大于0</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(ref SubString source, void* destination)
        {
#if DEBUG
            if (source.Length > int.MaxValue >> 1) throw new Exception(source.Length.toString() + " > int.MaxValue / 2");
#endif
            fixed (char* sourceFixed = source.GetFixedBuffer()) CopyTo(sourceFixed + source.Start, destination, source.Length << 1);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="source">字符串，长度必须大于0</param>
        /// <param name="index">字符串起始位置</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="size">字符长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(string source, int index, void* destination, int size)
        {
#if DEBUG
            source.DebugCheckSize(index, size);
            if (size > int.MaxValue >> 1) throw new Exception(size.toString() + " > int.MaxValue / 2");
#endif
            fixed (char* sourceFixed = source) CopyTo(sourceFixed + index, destination, size << 1);
        }
        /// <summary>
        /// 用数据填充整个数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">填充数据</param>
#if NetStandard21
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static void Fill<T>(T[] array, T value)
        {
#if NetStandard21
            Array.Fill(array, value);
#else
            for (int startIndex = array.Length; startIndex != 0; array[--startIndex] = value) ;
#endif
        }
        /// <summary>
        /// 用数据填充数组指定位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">填充数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">填充数据数量</param>
#if NetStandard21
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static void Fill<T>(T[] array, T value, int startIndex, int count)
        {
#if NetStandard21
            Array.Fill(array, value, startIndex, count);
#else
            if (startIndex < 0) throw new IndexOutOfRangeException(startIndex.toString() + " < 0");
            if (count < 0) throw new IndexOutOfRangeException(count.toString() + " < 0");
            int endIndex = startIndex + count;
            if (endIndex > array.Length) throw new IndexOutOfRangeException(startIndex.toString() + " + " + count.toString() + " > " + array.Length.toString());
            while (startIndex != endIndex) array[startIndex++] = value;
#endif
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="size">比较字节数</param>
        /// <returns>是否相等</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static unsafe bool SequenceEqual(void* left, void* right, int size)
        {
            return size <= 0 || AutoCSer.Memory.Common.SequenceEqual(left, right, size);
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if NetStandard21
        public static unsafe bool SequenceEqual(byte[]? left, byte[]? right)
#else
        public static unsafe bool SequenceEqual(byte[] left, byte[] right)
#endif
        {
            if (left != null)
            {
                if (left.Length == right?.Length)
                {
                    if (left.Length == 0 || object.ReferenceEquals(left, right)) return true;
                    fixed (byte* leftFixed = left, rightFixed = right)
                    {
                        return AutoCSer.Memory.Common.SequenceEqual(leftFixed, rightFixed, right.Length);
                    }
                }
                return false;
            }
            return right == null;
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static unsafe bool SequenceEqual(ref SubArray<byte> left, ref SubArray<byte> right)
        {
            if (left.Length == right.Length)
            {
                if (right.Length == 0) return true;
                if (object.ReferenceEquals(left.Array, right.Array) && left.Start == right.Start) return true;
                fixed (byte* leftFixed = left.GetFixedBuffer(), rightFixed = right.GetFixedBuffer())
                {
                    return AutoCSer.Memory.Common.SequenceEqual(leftFixed + left.Start, rightFixed + right.Start, right.Length);
                }
            }
            return false;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<bool> FileExists(string path)
        {
            return AutoCSer.Common.GetCompletedTask(File.Exists(path));
        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isRefresh">是否刷新文件信息</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<bool> FileExists(FileInfo file, bool isRefresh = false)
        {
            if (isRefresh) file.Refresh();
            return AutoCSer.Common.GetCompletedTask(file.Exists);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task DeleteFile(FileInfo file)
        {
            file.Delete();
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 如果文件存在则删除文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Task<bool> TryDeleteFile(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
                return AutoCSer.Common.TrueCompletedTask;
            }
            return CompletedTask<bool>.Default;
        }
        /// <summary>
        /// 刷新文件状态数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task RefreshFileInfo(FileInfo file)
        {
            file.Refresh();
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task FileMove(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFileName"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task FileMove(FileInfo sourceFile, string destFileName)
        {
            sourceFile.MoveTo(destFileName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 创建文件流
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <param name="share"></param>
        /// <param name="bufferSize"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Task<FileStream> CreateFileStream(string path, FileMode mode, FileAccess access, FileShare share = FileShare.None, int bufferSize = 4 << 10, FileOptions options = FileOptions.None)
        {
            var fileStream = new FileStream(path, mode, access, share, bufferSize, options);
            try
            {
                Task<FileStream> task = Task.FromResult(fileStream);
                fileStream = null;
                return task;
            }
            finally { fileStream?.Dispose(); }
        }
        /// <summary>
        /// 移动文件流位置
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="seekIndex"></param>
        /// <param name="seekOrigin"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task Seek(FileStream fileStream, long seekIndex, SeekOrigin seekOrigin)
        {
            fileStream.Seek(seekIndex, seekOrigin);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 设置文件流长度
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task SetLength(FileStream fileStream, long length)
        {
            fileStream.SetLength(length);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 如果文件存在则删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task TryDeleteFile(string path)
        {
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 获取备份文件名称（不允许返回重复文件名称，否则相关调用可能陷入死循环）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMoveBakFileName(string fileName)
        {
            string bakName = AutoCSer.IO.File.BakPrefix + AutoCSer.Threading.SecondTimer.Now.ToString("yyyyMMdd-HHmmss") + "_" + ((uint)Random.Default.Next()).toHex() + "_";
            int index = fileName.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1;
            return index != 0 ? fileName.Insert(index, bakName) : (bakName + fileName);
        }
        /// <summary>
        /// 读取文件所有字节并返回文件数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<byte[]> ReadFileAllBytes(string fileName)
        {
#if NetStandard21
            return File.ReadAllBytesAsync(fileName);
#else
            return Task.FromResult(File.ReadAllBytes(fileName));
#endif
        }
        /// <summary>
        /// 读取文件并返回文件文本内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<string> ReadFileAllText(string fileName, Encoding encoding)
        {
#if NetStandard21
            return File.ReadAllTextAsync(fileName, encoding);
#else
            return Task.FromResult(File.ReadAllText(fileName, encoding));
#endif
        }
        /// <summary>
        /// 将文本写入文件，文件不存在则创建文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task WriteFileAllText(string fileName, string text, Encoding encoding)
        {
#if NetStandard21
            return File.WriteAllTextAsync(fileName, text, encoding);
#else
            File.WriteAllText(fileName, text, encoding);
            return AutoCSer.Common.CompletedTask;
#endif
        }
        /// <summary>
        /// 将数据写入文件，文件不存在则创建文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task WriteAllBytes(string fileName, byte[] data)
        {
#if NetStandard21
            return File.WriteAllBytesAsync(fileName, data);
#else
            File.WriteAllBytes(fileName, data);
            return AutoCSer.Common.CompletedTask;
#endif
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="destFileName"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task FileCopyTo(FileInfo file, string destFileName, bool overwrite = true)
        {
            file.CopyTo(destFileName, overwrite);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task FileCopyTo(string sourceFileName, string destFileName, bool overwrite = true)
        {
            System.IO.File.Copy(sourceFileName, destFileName, overwrite);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 设置文件属性
        /// </summary>
        /// <param name="file"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task SetFileAttributes(FileInfo file, FileAttributes attributes)
        {
            file.Attributes = attributes;
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<bool> DirectoryExists(string path)
        {
            return AutoCSer.Common.GetCompletedTask(Directory.Exists(path));
        }
        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<bool> DirectoryExists(DirectoryInfo directory)
        {
            return AutoCSer.Common.GetCompletedTask(directory.Exists);
        }
        /// <summary>
        ///  移动目录
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectoryName"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task DirectoryMove(DirectoryInfo sourceDirectory, string destDirectoryName)
        {
            return DirectoryMove(sourceDirectory.FullName, destDirectoryName);
        }
        /// <summary>
        ///  移动目录
        /// </summary>
        /// <param name="sourceDirectoryName"></param>
        /// <param name="destDirectoryName"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task DirectoryMove(string sourceDirectoryName, string destDirectoryName)
        {
            Directory.Move(sourceDirectoryName, destDirectoryName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 搜索文件
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<FileInfo[]> DirectoryGetFiles(DirectoryInfo directory)
        {
            return AutoCSer.Common.GetCompletedTask(directory.GetFiles());
        }
        /// <summary>
        /// 搜索文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<FileInfo[]> DirectoryGetFiles(DirectoryInfo directory, string searchPattern)
        {
            return AutoCSer.Common.GetCompletedTask(directory.GetFiles(searchPattern));
        }
        /// <summary>
        /// 搜索文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<FileInfo[]> DirectoryGetFiles(DirectoryInfo directory, string searchPattern, SearchOption searchOption)
        {
            return AutoCSer.Common.GetCompletedTask(directory.GetFiles(searchPattern, searchOption));
        }
        /// <summary>
        /// 搜索文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<string[]> DirectoryGetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return AutoCSer.Common.GetCompletedTask(Directory.GetFiles(path, searchPattern, searchOption));
        }
        /// <summary>
        /// 搜索文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<string[]> DirectoryGetFiles(string path)
        {
            return AutoCSer.Common.GetCompletedTask(Directory.GetFiles(path));
        }
        /// <summary>
        /// 搜索目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<DirectoryInfo[]> GetDirectories(DirectoryInfo directory)
        {
            return AutoCSer.Common.GetCompletedTask(directory.GetDirectories());
        }
        /// <summary>
        /// 搜索目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<DirectoryInfo[]> GetDirectories(DirectoryInfo directory, string searchPattern)
        {
            return AutoCSer.Common.GetCompletedTask(directory.GetDirectories(searchPattern));
        }
        /// <summary>
        /// 搜索目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<DirectoryInfo[]> GetDirectories(DirectoryInfo directory, string searchPattern, SearchOption searchOption)
        {
            return AutoCSer.Common.GetCompletedTask(directory.GetDirectories(searchPattern, searchOption));
        }
        /// <summary>
        /// 搜索目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<string[]> GetDirectories(string path)
        {
            return AutoCSer.Common.GetCompletedTask(Directory.GetDirectories(path));
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Task<bool> TryCreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return AutoCSer.Common.TrueCompletedTask;
            }
            return CompletedTask<bool>.Default;
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static Task<bool> TryCreateDirectory(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
                return AutoCSer.Common.TrueCompletedTask;
            }
            return CompletedTask<bool>.Default;
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive">是否删除子目录与文件夹中的文件</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task TryDeleteDirectory(string path, bool recursive = true)
        {
            if (System.IO.Directory.Exists(path)) System.IO.Directory.Delete(path, recursive);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="recursive">是否删除子目录与文件夹中的文件</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task TryDeleteDirectory(DirectoryInfo directory, bool recursive = true)
        {
            if (directory.Exists) directory.Delete(recursive);
            return AutoCSer.Common.CompletedTask;
        }

        /// <summary>
        /// 申请字符串空间
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private static string allocateString(int size)
        {
            return new string((char)0, size);
        }
        /// <summary>
        /// 申请字符串空间
        /// </summary>
        public static readonly Func<int, string> AllocateString;

        unsafe static Common()
        {
            int isLittleEndian = 1;
            IsLittleEndian = *(byte*)&isLittleEndian == 1;

            var method = typeof(string).GetMethod("FastAllocateString", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(int) }, null);
            if (method != null) AllocateString = (Func<int, string>)AutoCSer.Reflection.Common.CreateDelegate(typeof(Func<int, string>), method);
            else AllocateString = allocateString;

            var config = AutoCSer.Configuration.Common.Get<AutoCSer.Config>()?.Value;
            Config = config ?? new AutoCSer.Config();
            Culture = AutoCSer.Configuration.Common.Get<AutoCSer.Culture.Configuration>()?.Value ?? AutoCSer.Culture.Configuration.GetDefault();
            DefaultConstructor = AutoCSer.Configuration.Common.Get<AutoCSer.Metadata.DefaultConstructor>()?.Value ?? new AutoCSer.Metadata.DefaultConstructor();
        }
    }
}
