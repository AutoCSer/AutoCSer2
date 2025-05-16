using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Extensions;
using AutoCSer.Threading;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer
{
    /// <summary>
    /// 配置对象
    /// </summary>
    public abstract class ConfigObject
    {
        /// <summary>
        /// 日志队列访问锁
        /// </summary>
#if DEBUG && NetStandard21
        [AllowNull]
#endif
        protected readonly SemaphoreSlimLock onChangedLock;
        /// <summary>
        /// 配置数据是否只读
        /// </summary>
        public readonly bool IsReadOnly;
        /// <summary>
        /// 配置对象
        /// </summary>
        /// <param name="isReadOnly"></param>
        protected ConfigObject(bool isReadOnly)
        {
            IsReadOnly = isReadOnly;
            if (!isReadOnly) onChangedLock = new SemaphoreSlimLock(1);
        }

        /// <summary>
        /// 获取配置对象类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? GetConfigObjectType(Type type)
#else
        internal static Type GetConfigObjectType(Type type)
#endif
        {
            if (type.IsClass)
            {
                for (var baseType = type; baseType != typeof(object) && baseType != null; baseType = baseType.BaseType)
                {
                    if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(ConfigObject<>))
                    {
                        return baseType.GetGenericArguments()[0];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConfigObject Create<T>(object value) where T : class
        {
            return new ConfigObject<T>((T)value, true);
        }
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ConfigObject?> CreateTask<T>(object task)
#else
        internal static async Task<ConfigObject> CreateTask<T>(object task)
#endif
            where T : class
        {
#if NetStandard21
            var value = await (Task<T?>)task;
#else
            T value = await (Task<T>)task;
#endif
            return value != null ? new ConfigObject<T>(value, true) : null;
        }
        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ConfigObject?> GetTask<T>(object task)
#else
        internal static async Task<ConfigObject> GetTask<T>(object task)
#endif
            where T : class
        {
#if NetStandard21
            return (ConfigObject?)(object?)await (Task<T?>)task;
#else
            return (ConfigObject)(object)await (Task<T>)task;
#endif
        }
#if AOT
        /// <summary>
        /// 创建配置对象方法信息
        /// </summary>
        internal static readonly MethodInfo CreateMethod = typeof(ConfigObject).GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 创建配置对象方法信息
        /// </summary>
        internal static readonly MethodInfo CreateTaskMethod = typeof(ConfigObject).GetMethod(nameof(CreateTask), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 获取配置对象方法信息
        /// </summary>
        internal static readonly MethodInfo GetTaskMethod = typeof(ConfigObject).GetMethod(nameof(GetTask), BindingFlags.Static | BindingFlags.Public).notNull();
#endif
    }
    /// <summary>
    /// 配置对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigObject<T> : ConfigObject 
        where T : class
    {
        /// <summary>
        /// 配置数据
        /// </summary>
        public T Value { get; protected set; }
        /// <summary>
        /// 配置更新触发任务集合
        /// </summary>
#if NetStandard21
        protected readonly HashSet<ReferenceHashKey<Func<ConfigObject<T>, Task>>>? onChangeds;
#else
        protected readonly HashSet<ReferenceHashKey<Func<ConfigObject<T>, Task>>> onChangeds;
#endif
        /// <summary>
        /// 配置数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isReadOnly"></param>
        public ConfigObject(T value, bool isReadOnly = false) : base(isReadOnly)
        {
            this.Value = value;
            if (!isReadOnly) onChangeds = HashSetCreator<ReferenceHashKey<Func<ConfigObject<T>, Task>>>.Create();
        }
        /// <summary>
        /// 获取配置数据
        /// </summary>
        /// <returns></returns>
        public virtual Task<T> Get()
        {
            return Task.FromResult(Value);
        }
        /// <summary>
        /// 配置对象隐式转换，当 T 为接口类型时隐式转换会异常
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <returns>配置数据</returns>
#if NetStandard21
        public static implicit operator T?(ConfigObject<T>? config) { return config?.Value; }
#else
        public static implicit operator T(ConfigObject<T> config) { return config?.Value; }
#endif
        /// <summary>
        /// 清除配置更新触发任务
        /// </summary>
        public virtual async ValueTask ClearOnChanged()
        {
            if (onChangeds != null)
            {
                await onChangedLock.EnterAsync();
                onChangeds.Clear();
                onChangedLock.Exit();
            }
        }
        /// <summary>
        /// 添加配置更新触发任务
        /// </summary>
        /// <param name="onChanged">配置更新触发任务</param>
        /// <returns></returns>
#if NetStandard21
        public virtual async ValueTask<bool> SetOnChanged(Func<ConfigObject<T>, Task> onChanged)
#else
        public virtual async Task<bool> SetOnChanged(Func<ConfigObject<T>, Task> onChanged)
#endif
        {
            if (IsReadOnly) throw new InvalidOperationException();
            await onChangedLock.EnterAsync();
            try
            {
                return onChangeds.notNull().Add(onChanged);
            }
            finally
            {
                onChangedLock.Exit();
            }
        }
        /// <summary>
        /// 移除配置更新触发任务
        /// </summary>
        /// <param name="onChanged">配置更新触发任务</param>
        /// <returns></returns>
#if NetStandard21
        public virtual async ValueTask<bool> RemoveOnChanged(Func<ConfigObject<T>, Task> onChanged)
#else
        public virtual async Task<bool> RemoveOnChanged(Func<ConfigObject<T>, Task> onChanged)
#endif
        {
            if (IsReadOnly) throw new InvalidOperationException();
            await onChangedLock.EnterAsync();
            try
            {
                return onChangeds.notNull().Remove(onChanged);
            }
            finally
            {
                onChangedLock.Exit();
            }
        }
        /// <summary>
        /// 触发配置更新触发任务
        /// </summary>
        /// <returns></returns>
        protected async Task callOnChanged()
        {
            if (IsReadOnly) throw new InvalidOperationException();
            if (onChangeds.notNull().Count != 0)
            {
                ReferenceHashKey<Func<ConfigObject<T>, Task>>[] onChangedArray;
                await onChangedLock.EnterAsync();
                try
                {
                    onChangedArray = onChangeds.notNull().getArray();
                }
                finally
                {
                    onChangedLock.Exit();
                }

                foreach (ReferenceHashKey<Func<ConfigObject<T>, Task>> onChanged in onChangedArray)
                {
                    try
                    {
                        await onChanged.Value(this);
                    }
                    catch (Exception exception)
                    {
                        await AutoCSer.LogHelper.Exception(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                    }
                }
            }
        }
    }
}
