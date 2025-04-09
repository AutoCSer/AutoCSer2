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

            try
            {
                AutoCSer.ReusableDictionary<int, int> dictionary = new ReusableDictionary<int, int>();
                foreach (int value in data)
                {
                    if (!dictionary.Set(value, value + random))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.ReusableDictionary<int, int> dictionarySort = new ReusableDictionary<int, int>(0, ReusableDictionaryGroupTypeEnum.HashIndexSort);
                foreach (int value in data)
                {
                    if (!dictionarySort.Set(value, value + random))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.ReusableDictionary<int, int> dictionaryRoll = new ReusableDictionary<int, int>(0, ReusableDictionaryGroupTypeEnum.Roll);
                foreach (int value in data)
                {
                    if (!dictionaryRoll.Set(value, value + random, true))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.ReusableHashCodeKeyDictionary<int> hashCodeKeyDictionary = new ReusableHashCodeKeyDictionary<int>();
                foreach (int value in data)
                {
                    if (!hashCodeKeyDictionary.Set(value, value + random))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.ReusableHashCodeKeyDictionary<int> hashCodeKeyDictionarySort = new ReusableHashCodeKeyDictionary<int>(0, ReusableDictionaryGroupTypeEnum.HashIndexSort);
                foreach (int value in data)
                {
                    if (!hashCodeKeyDictionarySort.Set(value, value + random))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.ReusableHashCodeKeyDictionary<int> hashCodeKeyDictionaryRoll = new ReusableHashCodeKeyDictionary<int>(0, ReusableDictionaryGroupTypeEnum.Roll);
                foreach (int value in data)
                {
                    if (!hashCodeKeyDictionaryRoll.Set(value, value + random, true))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.ReusableHashSet<int> hashSet = new ReusableHashSet<int>();
                foreach (int value in data)
                {
                    if (!hashSet.Add(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.ReusableHashCodeKeyHashSet hashCodeKeyHashSet = new ReusableHashCodeKeyHashSet();
                foreach (int value in data)
                {
                    if (!hashCodeKeyHashSet.Add(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.RemoveMarkHashSet<int> removeMarkHashSet = new RemoveMarkHashSet<int>();
                foreach (int value in data)
                {
                    if (!removeMarkHashSet.Add(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.RemoveMarkHashSet hashCodeKeyRemoveMarkHashSet = new RemoveMarkHashSet();
                foreach (int value in data)
                {
                    if (!hashCodeKeyRemoveMarkHashSet.Add(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
#if !AOT
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotDictionary<int, ReusableDictionary> snapshotDictionary = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotDictionary<int, ReusableDictionary>();
                foreach (int value in data)
                {
                    if (!snapshotDictionary.Set(value, new ReusableDictionary(value + random)))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotDictionary<int, ReusableDictionary> snapshotDictionarySort = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotDictionary<int, ReusableDictionary>(0, ReusableDictionaryGroupTypeEnum.HashIndexSort);
                foreach (int value in data)
                {
                    if (!snapshotDictionarySort.Set(value, new ReusableDictionary(value + random)))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotDictionary<int, ReusableDictionary> snapshotDictionaryRoll = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotDictionary<int, ReusableDictionary>(0, ReusableDictionaryGroupTypeEnum.Roll);
                foreach (int value in data)
                {
                    if (!snapshotDictionaryRoll.Set(value, new ReusableDictionary(value + random), true))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.FragmentSnapshotDictionary256<int, int> fragmentSnapshotDictionary = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.FragmentSnapshotDictionary256<int, int>();
                foreach (int value in data)
                {
                    if (!fragmentSnapshotDictionary.TryAdd(value, value + random))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotHashSet<int> snapshotHashSet = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotHashSet<int>();
                foreach (int value in data)
                {
                    if (!snapshotHashSet.Add(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotHashSet<int> snapshotHashSetSort = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotHashSet<int>(0, ReusableDictionaryGroupTypeEnum.HashIndexSort);
                foreach (int value in data)
                {
                    if (!snapshotHashSetSort.Add(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotHashSet<int> snapshotHashSetRoll = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotHashSet<int>(0, ReusableDictionaryGroupTypeEnum.Roll);
                foreach (int value in data)
                {
                    if (!snapshotHashSetRoll.Add(value, true))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.FragmentSnapshotHashSet256<int> fragmentSnapshotHashSet = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.FragmentSnapshotHashSet256<int>();
                foreach (int value in data)
                {
                    if (!fragmentSnapshotHashSet.Add(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
#endif

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
                    if (!dictionarySort.Remove(value, out removeValue) || removeValue != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (dictionarySort.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!dictionaryRoll.Remove(value, out removeValue) || removeValue != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (dictionaryRoll.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!hashCodeKeyDictionary.Remove(value, out removeValue) || removeValue != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (hashCodeKeyDictionary.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!hashCodeKeyDictionarySort.Remove(value, out removeValue) || removeValue != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (hashCodeKeyDictionarySort.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!hashCodeKeyDictionaryRoll.Remove(value, out removeValue) || removeValue != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (hashCodeKeyDictionaryRoll.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!hashSet.Remove(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (hashSet.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!hashCodeKeyHashSet.Remove(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (hashCodeKeyHashSet.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!removeMarkHashSet.Remove(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (removeMarkHashSet.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!hashCodeKeyRemoveMarkHashSet.Remove(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (hashCodeKeyRemoveMarkHashSet.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
#if !AOT
                ReusableDictionary removeSnapshotValue;
                foreach (int value in data)
                {
                    if (!snapshotDictionary.Remove(value, out removeSnapshotValue) || removeSnapshotValue.value != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (snapshotDictionary.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!snapshotDictionarySort.Remove(value, out removeSnapshotValue) || removeSnapshotValue.value != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (snapshotDictionarySort.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!snapshotDictionaryRoll.Remove(value, out removeSnapshotValue) || removeSnapshotValue.value != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (snapshotDictionaryRoll.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!fragmentSnapshotDictionary.Remove(value, out removeValue) || removeValue != value + random)
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (fragmentSnapshotDictionary.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!snapshotHashSet.Remove(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (snapshotHashSet.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!snapshotHashSetSort.Remove(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (snapshotHashSetSort.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!snapshotHashSetRoll.Remove(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (snapshotHashSetRoll.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
                foreach (int value in data)
                {
                    if (!fragmentSnapshotHashSet.Remove(value))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                    }
                }
                if (fragmentSnapshotHashSet.Count != 0)
                {
                    return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
                }
#endif
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return AutoCSer.Breakpoint.ReturnFalse(new KeyValue<int, int[]>(random, data));
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
