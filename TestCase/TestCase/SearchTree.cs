using AutoCSer.Extensions;
using System;

namespace AutoCSer.TestCase
{
    class SearchTree : AutoCSer.SearchTree.Node<SearchTree, int>
    {
        private readonly int value;
        private SearchTree(int key, int value) : base(key)
        {
            this.value = value;
        }
        /// <summary>
        /// 二叉搜索树测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            int random = AutoCSer.Random.Default.Next();
            data.RandomSort();

            AutoCSer.SearchTree.Dictionary<int, int> dictionary = new AutoCSer.SearchTree.Dictionary<int, int>();
            int nextValue = 0;
            foreach (int value in data)
            {
                dictionary.Set(value, value + random);
                //if(!dictionary.Check(++nextValue))
                //{
                //    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                //}
            }
            nextValue = 0;
            foreach (KeyValue<int, int> value in dictionary.KeyValues)
            {
                if (value.Key != nextValue || value.Value != nextValue + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                ++nextValue;
            }

            AutoCSer.SearchTree.Set<int> searchTreeSet = new AutoCSer.SearchTree.Set<int>();
            nextValue = 0;
            foreach (int value in data)
            {
                searchTreeSet.Add(value + random);
                //if (!searchTreeSet.Check(++nextValue))
                //{
                //    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                //}
            }
            nextValue = 0;
            foreach (int value in searchTreeSet.Values)
            {
                if (value != nextValue + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                ++nextValue;
            }

            AutoCSer.SearchTree.NodeDictionary<int, SearchTree> nodeDictionary = new AutoCSer.SearchTree.NodeDictionary<int, SearchTree>();
            nextValue = 0;
            foreach (int value in data)
            {
                nodeDictionary.Set(new SearchTree(value, value + random));
                //if (!nodeDictionary.Check(++nextValue))
                //{
                //    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                //}
            }
            nextValue = 0;
            foreach (SearchTree value in nodeDictionary.Values)
            {
                if (value.Key != nextValue || value.value != nextValue + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                ++nextValue;
            }

            data.RandomSort();
            int removeValue;
            foreach (int value in data)
            {
                if (!dictionary.Remove(value, out removeValue) || removeValue != value + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
            }
            if (dictionary.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
            }
            foreach (int value in data)
            {
                if (!searchTreeSet.Remove(value + random))
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
            }
            if (searchTreeSet.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
            }
            SearchTree removeNodeValue;
            foreach (int value in data)
            {
                if (!nodeDictionary.Remove(value, out removeNodeValue) || removeNodeValue.value != value + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
            }
            if (nodeDictionary.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
            }

            return true;
        }

        /// <summary>
        /// 测试数据
        /// </summary>
        private static readonly int[] data;
        static SearchTree()
        {
            data = AutoCSer.Common.GetUninitializedArray<int>(1 << 10);
            for (int index = data.Length; index != 0;)
            {
                --index;
                data[index] = index;
            }
        }
    }
}
