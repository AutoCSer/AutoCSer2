using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCSer.Example.Search
{
    class Program
    {
        static void Main(string[] args)
        {
            User[] users = new User[]
            {
                new User { Id = 1, Name = @"AutoCSer", Remark = @"现在的努力是为了将来能够吹牛B" }
                , new User { Id = 2, Name = @"张三", Remark = @"现在的努力是为了曾经吹过的牛B" }
                , new User { Id = 3, Name = @"李四", Remark = @"现在吹下的牛b是将来努力的动力" }
            };

            using (AutoCSer.Search.StaticSearcher<SearchKey> searcher = new AutoCSer.Search.StaticSearcher<SearchKey>())//没有词库
            {
                AutoCSer.Search.StaticSearcherQueueContext<SearchKey> queueContext = searcher.QueueContext;
                foreach (User user in users)
                {
                    queueContext.Add(new SearchKey { Type = SearchType.UserName, Id = user.Id }, user.Name);
                    queueContext.Add(new SearchKey { Type = SearchType.UserRemark, Id = user.Id }, user.Remark);
                }

                AutoCSer.Search.SearchResult<SearchKey> result = queueContext.Search(@"张三丰偷学AutoCSer以后不再吹牛B了");
                if (result.GetWordResults(out LeftArray<KeyValue<SubString, HashSet<SearchKey>>> wordResults))
                {
                    foreach (KeyValue<SubString, HashSet<SearchKey>> word in wordResults)
                    {
                        Console.WriteLine(word.Key.ToString() + " => " + string.Join(" , ", word.Value.Select(key => key.Id + "[" + key.Type + "]")));
                    }
                }
                if (result.CharResult.GetCharResults(out LeftArray<KeyValue<char, HashSet<SearchKey>>> charResults))
                {
                    foreach (KeyValue<char, HashSet<SearchKey>> word in charResults)
                    {
                        Console.WriteLine(word.Key.ToString() + " => " + string.Join(" , ", word.Value.Select(key => key.Id + "[" + key.Type + "]")));
                    }
                }
                if (result.CharResult.LessResultCount != 0)
                {
                    Console.WriteLine("LESS => " + string.Join(" , ", result.CharResult.LessWords.Select(word => word.ToString())));
                }
            }
            Console.WriteLine();

            using (AutoCSer.Search.StringTrieGraph trieGraph = new AutoCSer.Search.StringTrieGraph(new string[] { "吹牛", "牛B", "现在", "将来", "曾经", "以后", "努力", "张三丰" }, 1))
            using (AutoCSer.Search.StaticStringTrieGraph staticTrieGraph = trieGraph.CreateStaticGraph(false))
            using (AutoCSer.Search.StaticSearcher<SearchKey> searcher = new AutoCSer.Search.StaticSearcher<SearchKey>(staticTrieGraph))
            {
                AutoCSer.Search.StaticSearcherQueueContext<SearchKey> queueContext = searcher.QueueContext;
                foreach (User user in users)
                {
                    queueContext.Add(new SearchKey { Type = SearchType.UserName, Id = user.Id }, user.Name);
                    queueContext.Add(new SearchKey { Type = SearchType.UserRemark, Id = user.Id }, user.Remark);
                }

                AutoCSer.Search.SearchResult<SearchKey> result = queueContext.Search(@"张三丰偷学AutoCSer以后不再吹牛B了");
                if (result.GetWordResults(out LeftArray<KeyValue<SubString, HashSet<SearchKey>>> wordResults))
                {
                    foreach (KeyValue<SubString, HashSet<SearchKey>> word in wordResults)
                    {
                        Console.WriteLine(word.Key.ToString() + " => " + string.Join(" , ", word.Value.Select(key => key.Id + "[" + key.Type + "]")));
                    }
                }
                if (result.CharResult.GetCharResults(out LeftArray<KeyValue<char, HashSet<SearchKey>>> charResults))
                {
                    foreach (KeyValue<char, HashSet<SearchKey>> word in charResults)
                    {
                        Console.WriteLine(word.Key.ToString() + " => " + string.Join(" , ", word.Value.Select(key => key.Id + "[" + key.Type + "]")));
                    }
                }
                if (result.CharResult.LessResultCount != 0)
                {
                    Console.WriteLine("LESS => " + string.Join(" , ", result.CharResult.LessWords.Select(word => word.ToString())));
                }
            }
            Console.WriteLine();

            Console.WriteLine("Over");
            Console.ReadKey();
        }
    }
}
