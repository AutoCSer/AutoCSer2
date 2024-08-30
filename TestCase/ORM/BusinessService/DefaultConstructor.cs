using AutoCSer.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 数据库表格模型构造函数映射到业务模型
    /// </summary>
    internal sealed class DefaultConstructor : AutoCSer.Metadata.DefaultConstructor
    {
        /// <summary>
        /// 默认构造函数集合
        /// </summary>
        private readonly Dictionary<HashType, KeyValue<Delegate, Type>> constructors;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        private DefaultConstructor()
        {
            BusinessPersistence.GetConstructors(constructors = DictionaryCreator.CreateHashType<KeyValue<Delegate, Type>>(), persistenceInitializeDelegates.Select(p => p.Method.DeclaringType));
        }

        /// <summary>
        /// 获取自定义创建对象的默认构造函数，用于反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>委托返回值的 T 类型对象不允许为 null</returns>
        public override Func<T> GetConstructor<T>()
        {
            if (constructors.TryGetValue(typeof(T), out KeyValue<Delegate, Type> constructor)) return (Func<T>)constructor.Key;
            return base.GetConstructor<T>();
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        internal static readonly DefaultConstructor Default = new DefaultConstructor();

        /// <summary>
        /// 持久化数据初始化任务集合
        /// </summary>
        private static readonly Func<Task>[] persistenceInitializeDelegates = new Func<Task>[]
        {
            Persistence.Initialize
        };
        /// <summary>
        /// 持久化数据初始化
        /// </summary>
        /// <returns></returns>
        internal static async Task PersistenceInitialize()
        {
            foreach (Func<Task> initialize in persistenceInitializeDelegates) await initialize();
        }
    }
}
