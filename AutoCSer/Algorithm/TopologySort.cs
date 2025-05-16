using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 拓扑排序
    /// </summary>
    public static class TopologySort
    {
        /// <summary>
        /// 拓扑排序器
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct Sorter<T>
#if NetStandard21
 where T : notnull
#endif
        {
            /// <summary>
            /// 图
            /// </summary>
#if NetStandard21
            private Dictionary<T, ListArray<T>?> graph;
#else
            private Dictionary<T, ListArray<T>> graph;
#endif
            /// <summary>
            /// 排序结果
            /// </summary>
            private T[] values;
            /// <summary>
            /// 当前排序位置
            /// </summary>
            private int index;
            /// <summary>
            /// 是否反向排序
            /// </summary>
            private bool isDesc;
            /// <summary>
            /// 拓扑排序器
            /// </summary>
            /// <param name="graph">图</param>
            /// <param name="points">单点集合</param>
            /// <param name="isDesc">是否反向排序</param>
            public Sorter(Dictionary<T, ListArray<T>> graph, ref LeftArray<T> points, bool isDesc)
            {
#pragma warning disable CS8619
                this.graph = graph;
#pragma warning restore CS8619
                this.isDesc = isDesc;
                values = new T[graph.Count + points.Length];
                if (isDesc)
                {
                    index = points.Length;
                    points.CopyTo(values, 0);
                }
                else points.CopyTo(values, index = graph.Count);
            }
            /// <summary>
            /// 拓扑排序
            /// </summary>
            /// <returns>排序结果</returns>
            public T[] Sort()
            {
                var points = default(ListArray<T>);
                if (isDesc)
                {
                    foreach (T point in graph.getArray(value => value.Key))
                    {
                        if (graph.TryGetValue(point, out points))
                        {
                            graph[point] = null;
                            foreach (T nextPoint in points.notNull()) popDesc(nextPoint);
                            graph.Remove(point);
                            values[index++] = point;
                        }
                    }
                }
                else
                {
                    foreach (T point in graph.getArray(value => value.Key))
                    {
                        if (graph.TryGetValue(point, out points))
                        {
                            graph[point] = null;
                            foreach (T nextPoint in points.notNull()) pop(nextPoint);
                            graph.Remove(point);
                            values[--index] = point;
                        }
                    }
                }
                return values;
            }
            /// <summary>
            /// 排序子节点
            /// </summary>
            /// <param name="point">子节点</param>
            private void pop(T point)
            {
                var points = default(ListArray<T>);
                if (graph.TryGetValue(point, out points))
                {
                    if (points == null) throw new OverflowException(AutoCSer.Common.Culture.TopologySortLoopError);
                    graph[point] = null;
                    foreach (T nextPoint in points) pop(nextPoint);
                    graph.Remove(point);
                    values[--index] = point;
                }
            }
            /// <summary>
            /// 排序子节点
            /// </summary>
            /// <param name="point">子节点</param>
            private void popDesc(T point)
            {
                var points = default(ListArray<T>);
                if (graph.TryGetValue(point, out points))
                {
                    if (points == null) throw new OverflowException(AutoCSer.Common.Culture.TopologySortLoopError);
                    graph[point] = null;
                    foreach (T nextPoint in points) popDesc(nextPoint);
                    graph.Remove(point);
                    values[index++] = point;
                }
            }
        }
        /// <summary>
        /// 拓扑排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="edges">边集合</param>
        /// <param name="points">无边点集合</param>
        /// <param name="isDesc">是否反向排序</param>
        /// <returns>排序结果</returns>
        public static T[] Sort<T>(ICollection<KeyValue<T, T>> edges, HashSet<T> points, bool isDesc = false)
#if NetStandard21
 where T : notnull, IEquatable<T>
#else
 where T : IEquatable<T>
#endif
        {
            if (edges.Count == 0) return points.getArray();
            Dictionary<T, ListArray<T>> graph = DictionaryCreator<T>.Create<ListArray<T>>();
            if (points == null) points = HashSetCreator<T>.Create();
            var values = default(ListArray<T>);
            foreach (KeyValue<T, T> edge in edges)
            {
                if (!graph.TryGetValue(edge.Key, out values)) graph.Add(edge.Key, values = new ListArray<T>());
                values.Add(edge.Value);
                points.Add(edge.Value);
            }
            LeftArray<T> pointList = new LeftArray<T>(points.Count);
            foreach (T point in points)
            {
                if (!graph.ContainsKey(point)) pointList.UnsafeAdd(point);
            }
            return new Sorter<T>(graph, ref pointList, isDesc).Sort();
        }
    }
}
