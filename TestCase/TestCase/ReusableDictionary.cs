using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCSer.TestCase
{
    class ReusableDictionary
    {
        private readonly int value;
        private ReusableDictionary(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// 可重用字典测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            int random = AutoCSer.Random.Default.Next();
            int[] data = randomData();

            AutoCSer.ReusableDictionary<int, int> dictionary = new ReusableDictionary<int, int>();
            foreach (int value in data)
            {
                if(!dictionary.Set(value, value + random))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.ReusableHashCodeKeyDictionary<int> hashCodeKeyDictionary = new ReusableHashCodeKeyDictionary<int>();
            foreach (int value in data)
            {
                if (!hashCodeKeyDictionary.Set(value, value + random))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.ReusableHashSet<int> hashSet = new ReusableHashSet<int>();
            foreach (int value in data)
            {
                if (!hashSet.Add(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.ReusableHashCodeKeyHashSet hashCodeKeyHashSet = new ReusableHashCodeKeyHashSet();
            foreach (int value in data)
            {
                if (!hashCodeKeyHashSet.Add(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.RemoveMarkHashSet<int> removeMarkHashSet = new RemoveMarkHashSet<int>();
            foreach (int value in data)
            {
                if (!removeMarkHashSet.Add(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.RemoveMarkHashSet hashCodeKeyRemoveMarkHashSet = new RemoveMarkHashSet();
            foreach (int value in data)
            {
                if (!hashCodeKeyRemoveMarkHashSet.Add(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotDictionary<int, ReusableDictionary> snapshotDictionary = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotDictionary<int, ReusableDictionary>();
            foreach (int value in data)
            {
                if (!snapshotDictionary.Set(value, new ReusableDictionary(value + random)))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.FragmentSnapshotDictionary256<int, int> fragmentSnapshotDictionary = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.FragmentSnapshotDictionary256<int, int>();
            foreach (int value in data)
            {
                if (!fragmentSnapshotDictionary.TryAdd(value, value + random))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotHashSet<int> snapshotHashSet = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotHashSet<int>();
            foreach (int value in data)
            {
                if (!snapshotHashSet.Add(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.FragmentSnapshotHashSet256<int> fragmentSnapshotHashSet = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.FragmentSnapshotHashSet256<int>();
            foreach (int value in data)
            {
                if (!fragmentSnapshotHashSet.Add(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }


            data.RandomSort();
            int removeValue;
            foreach (int value in data)
            {
                if (!dictionary.Remove(value, out removeValue) || removeValue != value + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (dictionary.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            foreach (int value in data)
            {
                if (!hashCodeKeyDictionary.Remove(value, out removeValue) || removeValue != value + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (hashCodeKeyDictionary.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            foreach (int value in data)
            {
                if (!hashSet.Remove(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (hashSet.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            foreach (int value in data)
            {
                if (!hashCodeKeyHashSet.Remove(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (hashCodeKeyHashSet.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            foreach (int value in data)
            {
                if (!removeMarkHashSet.Remove(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (removeMarkHashSet.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            foreach (int value in data)
            {
                if (!hashCodeKeyRemoveMarkHashSet.Remove(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (hashCodeKeyRemoveMarkHashSet.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            ReusableDictionary removeSnapshotValue;
            foreach (int value in data)
            {
                if (!snapshotDictionary.Remove(value, out removeSnapshotValue) || removeSnapshotValue.value != value + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (snapshotDictionary.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            foreach (int value in data)
            {
                if (!fragmentSnapshotDictionary.Remove(value, out removeValue) || removeValue != value + random)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (fragmentSnapshotDictionary.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            foreach (int value in data)
            {
                if (!snapshotHashSet.Remove(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (snapshotHashSet.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            foreach (int value in data)
            {
                if (!fragmentSnapshotHashSet.Remove(value))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (fragmentSnapshotHashSet.Count != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;

        }
        private static int[] randomData()
        {
            HashSet<int> data = new HashSet<int>();
            for (Random random = Random.Default; data.Count != (1 << 10) - 1; data.Add(random.Next())) ;
            return data.ToArray();
        }
    }
}
