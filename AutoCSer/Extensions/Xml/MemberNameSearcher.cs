using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 成员名称搜索数据
    /// </summary>
    internal sealed unsafe class MemberNameSearcher
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        private readonly Type type;
        /// <summary>
        /// 默认顺序成员名称数据
        /// </summary>
        internal AutoCSer.Memory.Pointer Names;
        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        internal AutoCSer.Memory.Pointer Searcher;
        /// <summary>
        /// 空成员名称搜索数据
        /// </summary>
        private MemberNameSearcher()
        {
            type = typeof(void);
            Names = AutoCSer.Memory.Unmanaged.NullByte8;
            Searcher = default(AutoCSer.Memory.Pointer);
        }
        /// <summary>
        /// 成员名称搜索数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="names"></param>
        private MemberNameSearcher(Type type, string[] names)
        {
            this.type = type;
            int maxNameLength = 0, nameLength = 0;
            foreach (string name in names)
            {
                if (name.Length > maxNameLength) maxNameLength = name.Length;
                nameLength += name.Length;
            }

            if (maxNameLength > (short.MaxValue >> 1) - 2 || nameLength == 0) Names = AutoCSer.Memory.Unmanaged.NullByte8;
            else
            {
                Names = AutoCSer.Memory.Unmanaged.GetStaticPointer((nameLength + names.Length * 3 + 1) << 1, false);
                byte* write = Names.Byte;
                foreach (string name in names)
                {
                    if (name.Length != 0)
                    {
                        *(short*)write = (short)((name.Length + 2) * sizeof(char));
                        *(char*)(write + sizeof(short)) = '<';
                        AutoCSer.Common.CopyTo(name, write += sizeof(short) + sizeof(char));
                        * (char*)(write += name.Length << 1) = '>';
                        write += sizeof(char);
                    }
                }
                *(short*)write = 0;
            }
            Searcher = AutoCSer.StateSearcher.CharBuilder.Create(names, true);
        }

        /// <summary>
        /// 空成员名称搜索数据
        /// </summary>
        internal static readonly MemberNameSearcher Null = new MemberNameSearcher();
        /// <summary>
        /// 成员名称查找数据缓存
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, MemberNameSearcher> cache = DictionaryCreator.CreateHashObject<System.Type, MemberNameSearcher>();
        /// <summary>
        /// 最后一次访问的搜索数据
        /// </summary>
        private static MemberNameSearcher lastSearcher;
        /// <summary>
        /// 获取成员名称查找数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="names"></param>
        /// <returns>成员名称查找数据</returns>
        internal static MemberNameSearcher Get(Type type, string[] names)
        {
            if (!type.IsGenericType) return new MemberNameSearcher(type, names);

            type = type.GetGenericTypeDefinition();
            var value = lastSearcher;
            if (value.type == type) return value;

            Monitor.Enter(cache);
            try
            {
                if (!cache.TryGetValue(type, out value)) cache.Add(type, value = new MemberNameSearcher(type, names));
            }
            finally { Monitor.Exit(cache); }
            lastSearcher = value;
            return value;
        }

        static MemberNameSearcher()
        {
            lastSearcher = Null = new MemberNameSearcher();
        }
    }
}
