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
    /// Public parameters and pre-encapsulated apis
    /// 公共参数与预封装 API
    /// </summary>
    public static partial class Common
    {
        /// <summary>
        /// AutoCSer name prefix
        /// AutoCSer 名称前缀
        /// </summary>
        public const string NamePrefix = "AutoCSer";
        /// <summary>
        /// Public configuration parameters
        /// 公共配置参数
        /// </summary>
        public static readonly AutoCSer.Config Config;
        /// <summary>
        /// Default constructor
        /// 默认构造函数
        /// </summary>
        internal static readonly AutoCSer.Metadata.DefaultConstructor DefaultConstructor;
        /// <summary>
        /// Default system language and culture configuration
        /// 默认系统语言文化配置
        /// </summary>
        internal static readonly AutoCSer.Culture.Configuration Culture;
        /// <summary>
        /// Whether it is little-endian storage mode (serialization operations only support little-endian mode)
        /// 是否小端储存模式（序列化操作仅支持小端模式）
        /// </summary>
        internal static readonly bool IsLittleEndian;

        /// <summary>
        /// Current process information
        /// 当前进程信息
        /// </summary>
        public static readonly Process CurrentProcess = Process.GetCurrentProcess();
        /// <summary>
        /// Number of CPU logical processors (number of threads)
        /// CPU 逻辑处理器数量（线程数量）
        /// </summary>
        public static readonly int ProcessorCount = Math.Max(Environment.ProcessorCount, 1);
        /// <summary>
        /// Set the CPU affinity of the current process
        /// 设置当前进程的 CPU 亲缘性
        /// </summary>
        /// <param name="processorIndex">The logical processor number starts from 0
        /// 逻辑处理器编号，从 0 开始</param>
        /// <returns>Return false on failure</returns>
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
        /// Set the CPU affinity of the current process
        /// 设置当前进程的 CPU 亲缘性
        /// </summary>
        /// <param name="processorIndexs">A collection of logical processor numbers, starting from 0
        /// 逻辑处理器编号集合，从 0 开始</param>
        /// <returns>Return false on failure</returns>
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
        ///// <summary>
        ///// CPU cache block byte size, influence AutoCSer.Threading.BlockPool hard-coded fill the size
        ///// CPU 高速缓存块字节大小，影响 AutoCSer.Threading.BlockPool 硬编码填充大小
        ///// </summary>
        //public const int CpuCacheBlockSize = 64;
        ///// <summary>
        ///// Each CPU cache block holds the number of object references
        ///// 每个 CPU 高速缓存块容纳对象引用数量
        ///// </summary>
        //public static readonly unsafe int CpuCacheBlockObjectCount = CpuCacheBlockSize / sizeof(IntPtr);

        /// <summary>
        /// Encoding.Unicode.CodePage
        /// </summary>
        public static int UnicodeCodePage = Encoding.Unicode.CodePage;

        /// <summary>
        /// The program executes the main directory
        /// 程序执行主目录
        /// </summary>
        public static readonly DirectoryInfo ApplicationDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory);
        /// <summary>
        /// Whether it is a code generation environment
        /// 是否代码生成环境
        /// </summary>
        internal static bool IsCodeGenerator;

        /// <summary>
        /// Get the custom deserialization delegate data type
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
        /// Wait for the task to be completed
        /// 等待任务完成
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Wait(ValueTask task)
        {
            task.AsTask().wait();
        }
        /// <summary>
        /// Get IAsyncEnumerable
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
        /// Wait for the task to be completed
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
        /// Get the default value (used to eliminate IDE warnings)
        /// 获取默认值（用于消除 IDE 警告）
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
        /// Default empty object
        /// 默认空对象
        /// </summary>
        internal static readonly object EmptyObject = new object();
        /// <summary>
        /// The default empty function is used to eliminate the empty statement warning in the IDE
        /// 默认空函数，用于消除 IDE 空语句警告
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EmptyFunction() { }
        /// <summary>
        /// Default empty delegate
        /// 默认空委托
        /// </summary>
        public static readonly Action EmptyAction = EmptyFunction;
        /// <summary>
        /// Default empty method information
        /// 默认空方法信息
        /// </summary>
        internal static readonly MethodInfo NullMethodInfo = EmptyAction.Method;
        /// <summary>
        /// Default empty event
        /// 默认空事件
        /// </summary>
        internal static readonly System.Threading.AutoResetEvent NullAutoResetEvent = new System.Threading.AutoResetEvent(true);
        /// <summary>
        /// Default empty event
        /// 默认空事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
#if NetStandard21
        private static void emptyEventHandler(object? sender, EventArgs e) { }
#else
        private static void emptyEventHandler(object sender, EventArgs e) { }
#endif
        /// <summary>
        /// Default empty event
        /// 默认空事件
        /// </summary>
        public static readonly EventHandler EmptyEventHandler = emptyEventHandler;

        /// <summary>
        /// The completed task that returns true
        /// 返回 true 的已完成任务
        /// </summary>
        public static readonly Task<bool> TrueCompletedTask = Task.FromResult(true);
        /// <summary>
        /// Get completed tasks based on logical value
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
        /// Get the completed task
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T">Return value type</typeparam>
        /// <param name="value">Task return value
        /// 任务返回值</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<T[]> GetCompletedTask<T>(T[] value) where T : class
        {
            return value.Length != 0 ? Task.FromResult(value) : EmptyArrayCompletedTask<T>.EmptyArray;
        }
        /// <summary>
        /// Get the completed task
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T">Return value type</typeparam>
        /// <param name="value">Task return value
        /// 任务返回值</param>
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
        /// The task is completed by default
        /// 默认已完成任务
        /// </summary>
#if DotNet45
        public static Task CompletedTask { get { return TrueCompletedTask; } }
#else
        public static Task CompletedTask { get { return Task.CompletedTask; } }
#endif
        /// <summary>
        /// The task is completed by default
        /// 默认已完成任务
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
        /// Get the completed task
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T">Return value type</typeparam>
        /// <param name="value">Task return value
        /// 任务返回值</param>
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
        /// Get the completed task
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T">Return value type</typeparam>
        /// <param name="value">Task return value
        /// 任务返回值</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ValueTask<T> GetCompletedValueTask<T>(Task<T> value)
        {
            return new ValueTask<T>(value);
        }
#endif

        /// <summary>
        /// Return true for the completed awaiter
        /// 返回 true 的已完成 awaiter
        /// </summary>
        public static readonly CompletedTaskCastAwaiter<bool> TrueCompletedAwaiter = new CompletedTaskCastAwaiter<bool>(true);
        /// <summary>
        /// Get the completed awaiter based on the logical value
        /// 根据逻辑值获取已完成 awaiter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CompletedTaskCastAwaiter<bool> GetCompletedAwaiter(bool value)
        {
            return value ? TrueCompletedAwaiter : CompletedTaskCastAwaiter<bool>.Default;
        }

        /// <summary>
        /// Get an array, allowing uninitialized entries(Reference type members are not allowed in data types)
        /// 获取数组，允许存在未初始化数组项（数据类型不允许存在引用类型成员）
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
        /// Copy the array and allow the existence of uninitialized array items(Reference type members are not allowed in data types)
        /// 复制数组，允许存在未初始化数组项（数据类型不允许存在引用类型成员）
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
        /// Copy the array and allow the existence of uninitialized array items(Reference type members are not allowed in data types)
        /// 复制数组，允许存在未初始化数组项（数据类型不允许存在引用类型成员）
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
        /// Copy the array and allow the existence of uninitialized array items(Reference type members are not allowed in data types)
        /// 复制数组，允许存在未初始化数组项（数据类型不允许存在引用类型成员）
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
        /// Copy the array
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
        /// Copy the array
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
        /// Copy the array
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
        /// Fill in integers
        /// 填充整数
        /// </summary>
        /// <param name="src">The starting address cannot be null
        /// 起始地址，不能为null</param>
        /// <param name="count">ulong integer quantity
        /// ulong 整数数量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Clear(ulong* src, int count)
        {
            if (count > 0) AutoCSer.Memory.Common.Clear(src, count);
        }
        /// <summary>
        /// All the data are set to 0
        /// 数据全部设置为 0
        /// </summary>
        /// <param name="src">The starting address cannot be null
        /// 起始地址，不能为null</param>
        /// <param name="size">Number of bytes
        /// 字节数量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Clear(void* src, int size)
        {
            if (size > 0) AutoCSer.Memory.Common.Clear((byte*)src, size);
        }
        /// <summary>
        /// Fill in the data
        /// 填充数据
        /// </summary>
        /// <param name="src">The starting address cannot be null
        /// 起始地址，不能为null</param>
        /// <param name="count">ulong integer quantity
        /// ulong 整数数量</param>
        /// <param name="value">Filled integer values
        /// 填充的整数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Fill(void* src, int count, ulong value)
        {
            if (count > 0) AutoCSer.Memory.Common.Fill((ulong*)src, count, value);
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="source">The starting address of the original data cannot be null
        /// 原数据起始地址，不能为null</param>
        /// <param name="destination">The starting address of the target data cannot be null
        /// 目标数据起始地址，不能为null</param>
        /// <param name="size">Byte length
        /// 字节长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(void* source, void* destination, int size)
        {
            if (size > 0) AutoCSer.Memory.Common.Copy(source, destination, size);
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="source">The starting address of the original data cannot be null
        /// 原数据起始地址，不能为null</param>
        /// <param name="destination">The starting address of the target data cannot be null
        /// 目标数据起始地址，不能为null</param>
        /// <param name="size">Byte length
        /// 字节长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(void* source, void* destination, long size)
        {
            if (size > 0) AutoCSer.Memory.Common.Copy(source, destination, size);
        }
        /// <summary>
        /// Memory data is converted into a byte array
        /// 内存数据转换成字节数组
        /// </summary>
        /// <param name="source">The starting address cannot be null
        /// 起始地址，不能为 null</param>
        /// <param name="size">The byte length must be greater than 0
        /// 字节长度，必须大于 0</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static unsafe byte[] GetArray(void* source, int size)
        {
            byte[] array = GetUninitializedArray<byte>(size);
            CopyTo(source, array);
            return array;
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="source">The starting address of the original data
        /// 原数据起始地址</param>
        /// <param name="destination">Target data
        /// 目标数据</param>
        /// <param name="destinationIndex">The starting position of the target data
        /// 目标数据起始位置</param>
        /// <param name="size">Byte length
        /// 字节长度</param>
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
        /// Copy data
        /// </summary>
        /// <param name="source">Original data
        /// 原数据</param>
        /// <param name="sourceIndex">The starting position of the original data
        /// 原数据起始位置</param>
        /// <param name="destination">The starting address of the target data
        /// 目标数据起始地址</param>
        /// <param name="size">Byte length
        /// 字节长度</param>
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
        /// Copy data
        /// </summary>
        /// <param name="source">The length must be greater than 0
        /// 长度必须大于 0</param>
        /// <param name="destination">The starting address of the target data cannot be null
        /// 目标数据起始地址，不能为null</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(string source, void* destination)
        {
#if DEBUG
            if (source.Length > int.MaxValue >> 1) throw new Exception(source.Length.toString() + " > int.MaxValue / 2");
#endif
            fixed (char* sourceFixed = source) CopyTo(sourceFixed, destination, source.Length << 1);
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="source">The length must be greater than 0
        /// 长度必须大于 0</param>
        /// <param name="destination">The starting address of the target data cannot be null
        /// 目标数据起始地址，不能为null</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static unsafe void CopyTo(ref SubString source, void* destination)
        {
#if DEBUG
            if (source.Length > int.MaxValue >> 1) throw new Exception(source.Length.toString() + " > int.MaxValue / 2");
#endif
            fixed (char* sourceFixed = source.GetFixedBuffer()) CopyTo(sourceFixed + source.Start, destination, source.Length << 1);
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="source">The length must be greater than 0
        /// 长度必须大于 0</param>
        /// <param name="index">The starting position of the string
        /// 字符串起始位置</param>
        /// <param name="destination">The starting address of the target data cannot be null
        /// 目标数据起始地址，不能为null</param>
        /// <param name="size">The number of copied characters
        /// 复制字符数量</param>
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
        /// Fill the entire array with data
        /// 用数据填充整个数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">Data to be filled
        /// 待填充数据</param>
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
        /// Fill the array with data to specify the position
        /// 用数据填充数组指定位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">Data to be filled
        /// 待填充数据</param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">The number of filled data
        /// 填充数据数量</param>
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
        /// Comparison of byte arrays
        /// 字节数组比较
        /// </summary>
        /// <param name="left">null is not allowed
        /// 不允许为 null</param>
        /// <param name="right">null is not allowed
        /// 不允许为 null</param>
        /// <param name="size">The number of bytes for comparison
        /// 比较字节数</param>
        /// <returns>Is it equal
        /// 是否相等</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static unsafe bool SequenceEqual(void* left, void* right, int size)
        {
            return size <= 0 || AutoCSer.Memory.Common.SequenceEqual(left, right, size);
        }
        /// <summary>
        /// Comparison of byte arrays
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
        /// Comparison of byte arrays
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
        /// Determine whether the file exists
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
        /// Determine whether the file exists
        /// 判断文件是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isRefresh">Is refresh file information
        /// 是否刷新文件信息</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<bool> FileExists(FileInfo file, bool isRefresh = false)
        {
            if (isRefresh) file.Refresh();
            return AutoCSer.Common.GetCompletedTask(file.Exists);
        }
        /// <summary>
        /// Delete the file
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
        /// Delete the file if it exists
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
        /// Refresh the file status data
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
        /// Move the file
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
        /// Move the file
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
        /// Create a file stream
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
        /// Move the file stream location
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
        /// Set the file stream length
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
        /// Delete the file if it exists
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
        /// Get the name of the backup file
        /// 获取备份文件名称
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
        /// Read all bytes of the file and return the file data
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
        /// Read the file and return the text content of the file
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
        /// Write the text to the file. If the file does not exist, create it
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
        /// Write data to a file. If the file does not exist, create a file
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
        /// Copy the file
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
        /// Copy the file
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
        /// Set file attributes
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
        /// Determine whether the directory exists
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
        /// Determine whether the directory exists
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
        /// Move the directory
        /// 移动目录
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
        /// Move the directory
        /// 移动目录
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
        /// Get file collection
        /// 获取文件集合
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<FileInfo[]> DirectoryGetFiles(DirectoryInfo directory)
        {
            return AutoCSer.Common.GetCompletedTask(directory.GetFiles());
        }
        /// <summary>
        /// Get file collection
        /// 获取文件集合
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
        /// Get file collection
        /// 获取文件集合
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
        /// Get file name collection
        /// 获取文件名称集合
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
        /// Get file name collection
        /// 获取文件名称集合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<string[]> DirectoryGetFiles(string path)
        {
            return AutoCSer.Common.GetCompletedTask(Directory.GetFiles(path));
        }
        /// <summary>
        /// Get directory collection
        /// 获取目录集合
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<DirectoryInfo[]> GetDirectories(DirectoryInfo directory)
        {
            return AutoCSer.Common.GetCompletedTask(directory.GetDirectories());
        }
        /// <summary>
        /// Get directory collection
        /// 获取目录集合
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
        /// Get directory collection
        /// 获取目录集合
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
        /// Get directory name collection
        /// 获取目录名称集合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<string[]> GetDirectories(string path)
        {
            return AutoCSer.Common.GetCompletedTask(Directory.GetDirectories(path));
        }
        /// <summary>
        /// Create a directory
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
        /// Create a directory
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
        /// Delete the directory
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive">Whether to delete the files in subdirectories and folders
        /// 是否删除子目录与文件夹中的文件</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task TryDeleteDirectory(string path, bool recursive = true)
        {
            if (System.IO.Directory.Exists(path)) System.IO.Directory.Delete(path, recursive);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Delete the directory
        /// 删除目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="recursive">Whether to delete the files in subdirectories and folders
        /// 是否删除子目录与文件夹中的文件</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task TryDeleteDirectory(DirectoryInfo directory, bool recursive = true)
        {
            if (directory.Exists) directory.Delete(recursive);
            return AutoCSer.Common.CompletedTask;
        }

        /// <summary>
        /// Apply for string space
        /// 申请字符串空间
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private static string allocateString(int size)
        {
            return new string((char)0, size);
        }
        /// <summary>
        /// Apply for string space
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
