using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端命令池
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal unsafe class CommandPool
    {
        /// <summary>
        /// 命令数组最小二进制长度
        /// </summary>
        private const int minArrayBits = 2;
        /// <summary>
        /// 命令数组最大二进制长度
        /// </summary>
        private const int maxArrayBits = 16;

        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly AutoCSer.Threading.CpuCachePad pad0;
        /// <summary>
        /// 客户端
        /// </summary>
        private readonly CommandClient client;
        /// <summary>
        /// 超时计数
        /// </summary>
#if NetStandard21
        internal readonly CommandPoolTimeoutCount? TimeoutCount;
#else
        internal readonly CommandPoolTimeoutCount TimeoutCount;
#endif
        /// <summary>
        /// 客户端命令池
        /// </summary>
        private CommandPoolLink[][] arrays;
        /// <summary>
        /// 第一个客户端命令池数组
        /// </summary>
        internal CommandPoolLink[] Array;
        /// <summary>
        /// 数组长度
        /// </summary>
        private int arraySizeAnd;
        /// <summary>
        /// 数组二进制长度
        /// </summary>
        private int bitSize;
        /// <summary>
        /// 当前数组数量
        /// </summary>
        private int arrayCount;
        /// <summary>
        /// 命令地址数量
        /// </summary>
        private int commandCount;
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly AutoCSer.Threading.CpuCachePad pad1;
        /// <summary>
        /// 客户端命令池数组
        /// </summary>
        private CommandPoolLink[] pushArray;
        /// <summary>
        /// 空闲命令位置
        /// </summary>
        private int freeIndex;
        /// <summary>
        /// 客户端命令池数组索引
        /// </summary>
        private int pushArrayIndex = int.MinValue;
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly AutoCSer.Threading.CpuCachePad pad2;
        /// <summary>
        /// 保持回调命令会话标识
        /// </summary>
        private CallbackIdentity keepCallbackIdentity = new CallbackIdentity(uint.MaxValue, uint.MaxValue);
        /// <summary>
        /// 保持回调命令
        /// </summary>
        private Command keepCallbackCommand;
        /// <summary>
        /// 客户端命令池数组
        /// </summary>
        private CommandPoolLink[] getArray;
        /// <summary>
        /// 客户端命令池数组索引
        /// </summary>
        private int getArrayIndex = int.MinValue;
        /// <summary>
        /// 空闲命令结束位置
        /// </summary>
        private int freeEndIndex;
        /// <summary>
        /// 是否输出过错误日志 活动会话数量过多
        /// </summary>
        private int isErrorLog;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        internal int IsDisposed;
        /// <summary>
        /// 空闲命令结束位置访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock freeEndIndexLock;
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly AutoCSer.Threading.CpuCachePad pad3;
        /// <summary>
        /// 客户端命令池
        /// </summary>
        /// <param name="client"></param>
        /// <param name="command"></param>
        internal CommandPool(CommandClient client, Command command)
        {
            this.client = client;
            keepCallbackCommand = command;
#if NetStandard21
            arrays = new CommandPoolLink[0][];
            Array = getArray = pushArray = EmptyArray<CommandPoolLink>.Array;
#endif
        }
        /// <summary>
        /// 客户端命令池
        /// </summary>
        /// <param name="client"></param>
        /// <param name="freeIndex"></param>
        internal CommandPool(CommandClient client, int freeIndex = KeepCallbackCommand.CommandPoolIndex)
        {
            this.client = client;
            bitSize = client.Config.CommandPoolBits;
            bitSize = bitSize <= maxArrayBits ? (bitSize >= minArrayBits ? bitSize : minArrayBits) : maxArrayBits;
            commandCount = 1 << bitSize;
            if ((uint)freeIndex >= commandCount) throw new IndexOutOfRangeException();
            Array = new CommandPoolLink[commandCount];
            arrays = new CommandPoolLink[4][];
            getArray = pushArray = EmptyArray<CommandPoolLink>.Array;
            keepCallbackCommand = CommandClientSocket.Null.CommandPool.keepCallbackCommand;
            this.freeIndex = freeIndex;
            arrays[0] = Array;
            arrayCount = 1;
            for (int index = freeIndex; index != commandCount; ++index) Array[index].Next = index + 1;
            freeEndIndex = arraySizeAnd = commandCount - 1;
            ushort maxTimeoutSeconds = client.Config.CommandMaxTimeoutSeconds;
            if (maxTimeoutSeconds != 0) TimeoutCount = new CommandPoolTimeoutCount(this, maxTimeoutSeconds);
        }
        /// <summary>
        /// 释放超时计数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void DisposeTimeout()
        {
            if (IsDisposed == 0)
            {
                IsDisposed = 1;
                TimeoutCount?.Dispose();
            }
        }
        /// <summary>
        /// 添加客户端命令
        /// </summary>
        /// <param name="command">客户端命令</param>
        /// <param name="identity">设置客户端命令</param>
        /// <returns>客户端命令索引位置</returns>
        internal int Push(Command command, out uint identity)
        {
            int index = freeIndex, arrayIndex = freeIndex >> bitSize;
            if (arrayIndex == 0) freeIndex = Array[index].Set(command, out identity);
            else
            {
                if (arrayIndex != pushArrayIndex) pushArray = arrays[pushArrayIndex = arrayIndex];
                freeIndex = pushArray[index & arraySizeAnd].Set(command, out identity);
            }
            return freeIndex == commandCount ? create(index) : index;
        }
        /// <summary>
        /// 新建客户端命令池
        /// </summary>
        /// <param name="currentIndex">当前空闲命令位置</param>
        /// <returns></returns>
        private int create(int currentIndex)
        {
            if (bitSize == maxArrayBits)
            {
                if (arrayCount == arrays.Length)
                {
                    if (arrayCount == 1 << (CallbackIdentity.CallbackIndexBits - maxArrayBits))
                    {
                        freeIndex = currentIndex;
                        if (isErrorLog == 0)
                        {
                            isErrorLog = 1;
                            client.Config.Log.ErrorIgnoreException("TCP 客户端活动会话数量过多", LogLevelEnum.Error | LogLevelEnum.AutoCSer);
                        }
                        return 0;
                    }
                    arrays = AutoCSer.Common.GetCopyArray(arrays, arrayCount << 1);
                }
                int index = 1 << maxArrayBits;
                CommandPoolLink[] array = new CommandPoolLink[index];
                do
                {
                    array[index - 1].Next = commandCount + index;
                }
                while (--index != 0);
                arrays[arrayCount++] = array;
                freeEndIndexLock.Enter();
                arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = commandCount;
                freeEndIndex = (commandCount += 1 << maxArrayBits) - 1;
                freeEndIndexLock.Exit();
            }
            else
            {
                CommandPoolLink[] array = new CommandPoolLink[1 << ++bitSize];
                for (int index = commandCount, endIndex = commandCount << 1; index != endIndex; ++index) array[index].Next = index + 1;
                freeEndIndexLock.Enter();
                AutoCSer.Common.CopyTo(Array, array);
                arrays[0] = Array = array;
                array[freeEndIndex].Next = commandCount;
                freeEndIndex = arraySizeAnd = (commandCount <<= 1) - 1;
                freeEndIndexLock.Exit();
            }
            freeIndex = (currentIndex < (1 << maxArrayBits) ? Array : pushArray)[currentIndex & arraySizeAnd].Next;
            return currentIndex;
        }
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="identity">客户端命令索引位置</param>
        /// <returns>客户端命令</returns>
#if NetStandard21
        internal Command? GetCommand(CallbackIdentity identity)
#else
        internal Command GetCommand(CallbackIdentity identity)
#endif
        {
            if (keepCallbackIdentity.CallbackEquals(identity)) return keepCallbackCommand;
            var command = default(Command);
            int index = identity.CallbackIndex;
            int arrayIndex = index >> bitSize;
            if (arrayIndex == 0)
            {
                freeEndIndexLock.Enter();
                switch (Array[index].Get(identity.Identity, commandCount, out command))
                {
                    case CommandPoolGetStateEnum.Command:
                        arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = index;
                        freeEndIndex = index;
                        freeEndIndexLock.Exit();
                        TimeoutCount?.TryDecrement(command.notNull().TimeoutSeconds);
                        return command;
                    case CommandPoolGetStateEnum.KeepCallback:
                        freeEndIndexLock.Exit();
                        keepCallbackCommand = command.notNull();
                        keepCallbackIdentity = identity;
                        return command;
                    default: freeEndIndexLock.Exit(); return null;
                }
            }
            if (arrayIndex != getArrayIndex) getArray = arrays[getArrayIndex = arrayIndex];
            int commandIndex = index & arraySizeAnd;
            freeEndIndexLock.Enter();
            switch (getArray[commandIndex].Get(identity.Identity, commandCount, out command))
            {
                case CommandPoolGetStateEnum.Command:
                    arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = index;
                    freeEndIndex = index;
                    freeEndIndexLock.Exit();
                    TimeoutCount?.TryDecrement(command.notNull().TimeoutSeconds);
                    return command;
                case CommandPoolGetStateEnum.KeepCallback:
                    freeEndIndexLock.Exit();
                    keepCallbackCommand = command.notNull();
                    keepCallbackIdentity = identity;
                    return command;
                default: freeEndIndexLock.Exit(); return null;
            }
        }
        /// <summary>
        /// 取消回调
        /// </summary>
        /// <param name="cancelKeepCallbackData"></param>
        internal void CancelCallback(ref CancelKeepCallbackData cancelKeepCallbackData)
        {
            var command = default(Command);
            int arrayIndex = cancelKeepCallbackData.Index >> bitSize;
            if (arrayIndex == 0)
            {
                freeEndIndexLock.Enter();
                command = Array[cancelKeepCallbackData.Index].CancelCallback(cancelKeepCallbackData.Identity, commandCount);
                freeEndIndexLock.Exit();
            }
            else
            {
                if (arrayIndex != getArrayIndex) getArray = arrays[getArrayIndex = arrayIndex];
                int commandIndex = cancelKeepCallbackData.Index & arraySizeAnd;
                freeEndIndexLock.Enter();
                command = getArray[commandIndex].CancelCallback(cancelKeepCallbackData.Identity, commandCount);
                freeEndIndexLock.Exit();
            }
            if (command != null)
            {
                if (cancelKeepCallbackData.ReturnType != CommandClientReturnTypeEnum.Unknown)
                {
                    command.CancelKeepCallback(cancelKeepCallbackData.ReturnType, cancelKeepCallbackData.ErrorMessage);
                }
                if (object.ReferenceEquals(command, keepCallbackCommand))
                {
                    keepCallbackIdentity.SetNull();
                    keepCallbackCommand = CommandClientSocket.Null.CommandPool.keepCallbackCommand;
                }
            }
        }
        /// <summary>
        /// 释放所有命令
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
#if NetStandard21
        internal Command? Free(Command? head, Command? end, int startIndex)
#else
        internal Command Free(Command head, Command end, int startIndex)
#endif
        {
            DisposeTimeout();
            bool isNext = false;
            freeEndIndexLock.EnterSleepFlag();
            try
            {
                foreach (CommandPoolLink[] array in arrays)
                {
                    if (isNext)
                    {
                        if (array == null) break;
                        for (startIndex = array.Length; startIndex != 0;)
                        {
                            var command = array[--startIndex].Free();
                            if (command != null)
                            {
                                if (head == null) head = end = command;
                                else
                                {
                                    end.notNull().LinkNext = command;
                                    end = command;
                                }
                            }
                        }
                    }
                    else
                    {
                        isNext = true;
                        do
                        {
                            var command = array[startIndex].Free();
                            if (command != null)
                            {
                                if (head == null) head = end = command;
                                else
                                {
                                    end.notNull().LinkNext = command;
                                    end = command;
                                }
                            }
                        }
                        while (++startIndex != array.Length);
                    }
                }
            }
            finally { freeEndIndexLock.ExitSleepFlag(); }
            return head;
        }
        /// <summary>
        /// 超时事件
        /// </summary>
        /// <param name="seconds">超时秒计数</param>
        internal void OnTimeout(uint seconds)
        {
            int startIndex = KeepCallbackCommand.CommandPoolIndex, index = 0;
            var head = default(Command);
            var end = default(Command);
            freeEndIndexLock.EnterSleepFlag();
            try
            {
                foreach (CommandPoolLink[] array in arrays)
                {
                    if (index != 0)
                    {
                        if (array == null) break;
                        for (startIndex = 0; startIndex != array.Length; ++startIndex, ++index)
                        {
                            var command = array[startIndex].CheckTimeout(seconds, commandCount);
                            if (command != null)
                            {
                                arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = index;
                                freeEndIndex = index;
                                if (head == null) head = end = command;
                                else
                                {
                                    end.notNull().LinkNext = command;
                                    end = command;
                                }
                            }
                        }
                    }
                    else
                    {
                        index = array.Length;
                        do
                        {
                            var command = array[startIndex].CheckTimeout(seconds, commandCount);
                            if (command != null)
                            {
                                arrays[freeEndIndex >> bitSize][freeEndIndex & arraySizeAnd].Next = startIndex;
                                freeEndIndex = startIndex;
                                if (head == null) head = end = command;
                                else
                                {
                                    end.notNull().LinkNext = command;
                                    end = command;
                                }
                            }
                        }
                        while (++startIndex != index);
                    }
                }
            }
            finally
            {
                freeEndIndexLock.ExitSleepFlag();
                client.OnTimeout(head);
            }
        }
    }
}
