﻿using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 超时计数
    /// </summary>
    internal abstract unsafe class TimeoutCount : SecondTimerNode, IDisposable
    {
        /// <summary>
        /// 计时与索引位置
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = sizeof(long))]
        internal struct SecondIndex
        {
            /// <summary>
            /// 目标对象
            /// </summary>
            [FieldOffset(0)]
            public long Value;
            /// <summary>
            /// 计数索引位置
            /// </summary>
            [FieldOffset(0)]
            public int Index;
            /// <summary>
            /// 计时秒基数
            /// </summary>
            [FieldOffset(sizeof(int))]
            public uint Second;
            /// <summary>
            /// 计算下一个位置
            /// </summary>
            /// <param name="Size"></param>
            /// <returns></returns>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal long Next(int Size)
            {
                ++Second;
                //if (++Index == Size) Index = 0;
                ++Index;
                Index &= (Index ^ Size).logicalInversion() - 1;
                return Value;
            }
        }
        /// <summary>
        /// 计时与索引位置
        /// </summary>
        private SecondIndex secondIndex;
        /// <summary>
        /// 计数集合
        /// </summary>
        private AutoCSer.Memory.Pointer Counts;
        /// <summary>
        /// 最大超时秒数
        /// </summary>
        private ushort maxSeconds;
        /// <summary>
        /// 超时计数
        /// </summary>
        /// <param name="maxSeconds">最大超时秒数，必须大于 0</param>
        internal TimeoutCount(ushort maxSeconds)
        {
            if (maxSeconds != 0)
            {
                this.maxSeconds = maxSeconds;
                Counts = AutoCSer.Memory.Unmanaged.GetPointer8(((int)maxSeconds + 2) << 2, true);
                Counts.CurrentIndex = Counts.ByteSize >> 2;
                AutoCSer.Threading.SecondTimer.SecondNodeLink.PushNotNull(this);
            }
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            AutoCSer.Threading.SecondTimer.SecondNodeLink.PopNotNull(this);
            AutoCSer.Memory.Unmanaged.Free(ref Counts);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        ~TimeoutCount()
        {
            Dispose();
        }
        /// <summary>
        /// 增加超时计数
        /// </summary>
        /// <param name="seconds">超时秒数</param>
        /// <returns>超时秒计数</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint TryIncrement(ushort seconds)
        {
            return seconds == 0 || Counts.ByteSize == 0 ? 0 : Increment(Math.Min(maxSeconds, seconds));
        }
        /// <summary>
        /// 增加超时计数
        /// </summary>
        /// <param name="seconds">超时秒数，必须大于 0</param>
        /// <returns>超时秒计数</returns>
        internal uint Increment(ushort seconds)
        {
            SecondIndex secondIndex = new SecondIndex { Value = System.Threading.Interlocked.Read(ref this.secondIndex.Value) };

            int index = secondIndex.Index + seconds;
            uint timeoutSeconds = secondIndex.Second + seconds;
            if (index >= Counts.CurrentIndex) index -= Counts.CurrentIndex;
            if (timeoutSeconds != 0)
            {
                System.Threading.Interlocked.Increment(ref Counts.Int[index]);
                return timeoutSeconds;
            }
            //屏蔽 0
            //if (++index == Counts.CurrentIndex) index = 0;
            ++index;
            index &= (index ^ Counts.CurrentIndex).logicalInversion() - 1;
            System.Threading.Interlocked.Increment(ref Counts.Int[index]);
            return 1;
        }
        /// <summary>
        /// 减少超时计数
        /// </summary>
        /// <param name="seconds">超时秒计数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TryDecrement(uint seconds)
        {
            if (seconds != 0 && Counts.ByteSize != 0) Decrement(seconds);
        }
        /// <summary>
        /// 减少超时计数
        /// </summary>
        /// <param name="seconds">超时秒计数</param>
        internal void Decrement(uint seconds)
        {
            SecondIndex secondIndex = new SecondIndex { Value = System.Threading.Interlocked.Read(ref this.secondIndex.Value) };

            int index = (int)(seconds - secondIndex.Second);
            if (index > 0)
            {
                if (index <= Counts.CurrentIndex)
                {
                    index += secondIndex.Index;
                    if (index >= Counts.CurrentIndex) index -= Counts.CurrentIndex;
                    System.Threading.Interlocked.Decrement(ref Counts.Int[index]);
                }
            }
            else if ((uint)index == 0 && System.Threading.Interlocked.Decrement(ref Counts.Int[secondIndex.Index]) < 0)
            {
                System.Threading.Interlocked.Increment(ref Counts.Int[secondIndex.Index]);
            }
        }
        /// <summary>
        /// 超时检测
        /// </summary>
        protected internal override void OnTimer()
        {
            do
            {
                long value = System.Threading.Interlocked.Read(ref this.secondIndex.Value);
                SecondIndex secondIndex = new SecondIndex { Value = value };
                if (System.Threading.Interlocked.CompareExchange(ref this.secondIndex.Value, secondIndex.Next(Counts.CurrentIndex), value) == value)
                {
                    secondIndex.Value = value;
                    int count = System.Threading.Interlocked.Exchange(ref Counts.Int[secondIndex.Index], 0);
                    if (count > 0)
                    {
                        if (secondIndex.Second != 0) OnTimeout(secondIndex.Second);
                        else AutoCSer.LogHelper.ErrorIgnoreException("非法超时秒计数 0，请检查 Decrement 调用是否有 0 传参", LogLevelEnum.Fatal | LogLevelEnum.Error | LogLevelEnum.AutoCSer);
                    }
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// 超时事件（不允许阻塞）
        /// </summary>
        /// <param name="seconds">超时秒计数</param>
        internal abstract void OnTimeout(uint seconds);
    }
}
