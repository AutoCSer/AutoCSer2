using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoCSer.Json
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
        internal StateSearcher Searcher;
        /// <summary>
        /// 空成员名称搜索数据
        /// </summary>
        private MemberNameSearcher()
        {
            type = typeof(void);
            Names = AutoCSer.Memory.Unmanaged.NullByte8;
            Searcher = new StateSearcher(new Pointer());
        }
        /// <summary>
        /// 成员名称搜索数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="names"></param>
#if AOT
        private MemberNameSearcher(Type type, LeftArray<string> names)
#else
        private MemberNameSearcher(Type type, string[] names)
#endif
        {
            this.type = type;
            int maxNameLength = 0, nameLength = 0;
            foreach (string name in names)
            {
                if (name.Length > maxNameLength) maxNameLength = name.Length;
                nameLength += name.Length;
            }

            if (maxNameLength > (short.MaxValue >> 1) - 4 || nameLength == 0) Names = AutoCSer.Memory.Unmanaged.NullByte8;
            else
            {
                Names = AutoCSer.Memory.Unmanaged.GetStaticPointer((nameLength + names.Length * 5 + 1) << 1, false);
                byte* write = Names.Byte;
                foreach (string name in names)
                {
                    if (name.Length != 0)
                    {
                        if (write == Names.Byte)
                        {
                            *(short*)write = (short)((name.Length + 3) * sizeof(char));
                            *(char*)(write + sizeof(short)) = '"';
                            write += sizeof(short) + sizeof(char);
                        }
                        else
                        {
                            *(short*)write = (short)((name.Length + 4) * sizeof(char));
                            *(int*)(write + sizeof(short)) = ',' + ('"' << 16);
                            write += sizeof(short) + sizeof(int);
                        }
                        AutoCSer.Common.CopyTo(name, write);
                        *(int*)(write += name.Length << 1) = '"' + (':' << 16);
                        write += sizeof(int);
                    }
                }
                *(short*)write = 0;
            }
            Searcher = new StateSearcher(AutoCSer.StateSearcher.CharBuilder.Create(names, true));
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
#if AOT
        internal static MemberNameSearcher Get(Type type, LeftArray<string> names)
#else
        internal static MemberNameSearcher Get(Type type, string[] names)
#endif
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
