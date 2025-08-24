using AutoCSer.Memory;
using AutoCSer.TextSerialize;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 文本序列化
    /// </summary>
    /// <typeparam name="T">文本序列化类型</typeparam>
    /// <typeparam name="CT">序列化配置参数类型</typeparam>
    public unsafe abstract class TextSerializer<T, CT> : AutoCSer.Threading.Link<T>, IDisposable
        where T : TextSerializer<T, CT>
        where CT : SerializeConfig
    {
        /// <summary>
        /// 字符串输出缓冲区
        /// </summary>
        public readonly CharStream CharStream;
        /// <summary>
        /// 配置参数
        /// </summary>
        internal CT Config;
        /// <summary>
        /// 祖先节点集合
        /// </summary>
        protected LeftArray<object> forefather;
        /// <summary>
        /// 节点层级计数
        /// </summary>
        internal int CheckDepth;
        /// <summary>
        /// 是否二进制混杂模式
        /// </summary>
        internal bool IsBinaryMix;
        /// <summary>
        /// 警告提示状态
        /// </summary>
        public WarningEnum Warning { get; internal set; }
        /// <summary>
        /// 是否正在处理序列化操作
        /// </summary>
        protected bool isProcessing;
        /// <summary>
        /// 文本序列化
        /// </summary>
        /// <param name="config">Configuration parameters</param>
        /// <param name="isThreadStatic">是否单线程模式</param>
        protected TextSerializer(CT config, bool isThreadStatic = false)
        {
            Config = config;
            CharStream = isThreadStatic ? new CharStream(AutoCSer.Common.Config.SerializeUnmanagedPool) : new CharStream(default(AutoCSer.Memory.UnmanagedPoolPointer));
            forefather.SetEmpty();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            if (isProcessing) throw new InvalidOperationException(AutoCSer.Common.Culture.NotAllowDisposeSerializer);
            CharStream.Dispose();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void free()
        {
            CharStream.Clear();
            forefather.ClearLength();
            isProcessing = false;
        }
        /// <summary>
        /// 获取序列化循环引用检查类型
        /// </summary>
        /// <param name="pushType"></param>
        /// <returns></returns>
        internal AutoCSer.TextSerialize.PushTypeEnum Check(AutoCSer.TextSerialize.PushTypeEnum pushType)
        {
            if (--CheckDepth > 0)
            {
                if (Config.CheckLoop)
                {
                    switch (pushType)
                    {
                        case AutoCSer.TextSerialize.PushTypeEnum.DepthCount: return AutoCSer.TextSerialize.PushTypeEnum.DepthCount;
                        case AutoCSer.TextSerialize.PushTypeEnum.UnknownNode:
                            if (forefather.Reserve != 0) return AutoCSer.TextSerialize.PushTypeEnum.UnknownNode;
                            return AutoCSer.TextSerialize.PushTypeEnum.DepthCount;
                        case AutoCSer.TextSerialize.PushTypeEnum.UnknownDepthCount: ++forefather.Reserve; return AutoCSer.TextSerialize.PushTypeEnum.UnknownDepthCount;
                        case AutoCSer.TextSerialize.PushTypeEnum.Push: return AutoCSer.TextSerialize.PushTypeEnum.Push;
                    }
                }
                return AutoCSer.TextSerialize.PushTypeEnum.DepthCount;
            }
            ++CheckDepth;
            this.Warning |= AutoCSer.TextSerialize.WarningEnum.DepthOutOfRange;
            return AutoCSer.TextSerialize.PushTypeEnum.DepthOutOfRange;
        }
        /// <summary>
        /// 上级节点为值类型未知节点时添加循环对象检查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>0 表示循环引用</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int PushUnknownNode(object value)
        {
            if (Push(value))
            {
                int UnknownCount = forefather.Reserve;
                forefather.Reserve = 0;
                return UnknownCount;
            }
            return 0;
        }
        /// <summary>
        /// 添加循环引用检查对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool Push(object value)
        {
            if (forefather.Length != 0)
            {
                foreach (object arrayValue in forefather)
                {
                    if (object.ReferenceEquals(arrayValue, value))
                    {
                        WriteLoopReference();
                        Warning |= AutoCSer.TextSerialize.WarningEnum.LoopReference;
                        return false;
                    }
                }
            }
            forefather.Add(value);
            return true;
        }
        /// <summary>
        /// 循环引用对象处理
        /// </summary>
        protected abstract void WriteLoopReference();
        /// <summary>
        /// 上级节点为值类型未知节点时添加循环对象检查
        /// </summary>
        /// <param name="unknownCount"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PopUnknownNode(int unknownCount)
        {
            forefather.Reserve = unknownCount;
            Pop();
        }
        /// <summary>
        /// 当前节点为值类型未知节点
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PopUnknownDepthCount()
        {
            ++CheckDepth;
            --forefather.Reserve;
        }
        /// <summary>
        /// 弹出循环引用检查对象
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Pop()
        {
            ++CheckDepth;
            forefather.PopOnly();
        }
        /// <summary>
        /// Get and set the custom serialization member bitmap
        /// 获取并设置自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">Serialization member bitmap
        /// 序列化成员位图</param>
        /// <returns>Original serialization member bitmap
        /// 原序列化成员位图</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public AutoCSer.Metadata.MemberMap? SetCustomMemberMap(AutoCSer.Metadata.MemberMap? memberMap)
#else
        public AutoCSer.Metadata.MemberMap SetCustomMemberMap(AutoCSer.Metadata.MemberMap memberMap)
#endif
        {
            return Config.SetCustomMemberMap(memberMap);
        }
    }
}
