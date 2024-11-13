using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeCache<GenericType>
    {
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallOutputDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackDelegate { get; }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal abstract Delegate CreateMethodCallbackDelegate { get; }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal abstract Delegate CreateMethodParameterCallbackDelegate { get; }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal abstract Delegate CreateMethodKeepCallbackDelegate { get; }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal abstract Delegate CreateMethodParameterKeepCallbackDelegate { get; }
        /// <summary>
        /// 添加待加载修复方法节点
        /// </summary>
        internal abstract Action<StreamPersistenceMemoryDatabaseServiceBase, DirectoryInfo, RepairNodeMethodDirectory> AppendRepairNodeMethodLoader { get; }

        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceCallOutputNodeCreateDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceKeepCallbackNodeCreateDelegate { get; }


        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static GenericType? lastGenericType;
#else
        protected static GenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(Type type)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallOutputDelegate { get { return (Func<ClientNode, int, Task<ResponseResult<T>>>)StreamPersistenceMemoryDatabaseClient.CallOutput<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackDelegate { get { return (Func<ClientNode, int, Task<KeepCallbackResponse<T>>>)StreamPersistenceMemoryDatabaseClient.KeepCallback<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputDelegate { get { return (Func<ClientNode, int, Task<ResponseResult<T>>>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeCallOutput<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackDelegate { get { return (Func<ClientNode, int, Task<KeepCallbackResponse<T>>>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeKeepCallback<T>; } }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal override Delegate CreateMethodCallbackDelegate { get { return (MethodCallback<T>.CreateDelegate)MethodCallback<T>.Create; } }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal override Delegate CreateMethodParameterCallbackDelegate { get { return (Func<CallInputOutputMethodParameter, MethodCallback<T>>)MethodCallback<T>.Create; } }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal override Delegate CreateMethodKeepCallbackDelegate { get { return (MethodKeepCallback<T>.CreateDelegate)MethodKeepCallback<T>.Create; } }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal override Delegate CreateMethodParameterKeepCallbackDelegate { get { return (Func<InputKeepCallbackMethodParameter, MethodKeepCallback<T>>)MethodKeepCallback<T>.Create; } }
        /// <summary>
        /// 添加待加载修复方法节点
        /// </summary>
        internal override Action<StreamPersistenceMemoryDatabaseServiceBase, DirectoryInfo, RepairNodeMethodDirectory> AppendRepairNodeMethodLoader { get { return StreamPersistenceMemoryDatabaseServiceBase.AppendRepairNodeMethodLoader<T>; } }

        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceCallOutputNodeCreateDelegate { get { return (Func<LocalClientNode, int, LocalServiceQueueNode<ResponseResult<T>>>)LocalServiceCallOutputNode<T>.Create; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceKeepCallbackNodeCreateDelegate { get { return (Func<LocalClientNode, int, LocalServiceQueueNode<KeepCallbackResponse<T>>>)LocalServiceKeepCallbackNode<T>.Create; } }
    }
}
