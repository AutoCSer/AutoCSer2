﻿using System;

namespace AutoCSer
{
    /// <summary>
    /// Binary data serialization member configuration
    /// 二进制数据序列化成员配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class BinarySerializeMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        ///// <summary>
        ///// 全局版本编号（添加字段）
        ///// </summary>
        //public uint GlobalVersion;
        ///// <summary>
        ///// 全局版本编号（删除字段），大于添加字段全局版本编号时有效 ，静态字段不能用 public 修饰
        ///// </summary>
        //public uint RemoveGlobalVersion;
        ///// <summary>
        ///// 字段是否已经被删除
        ///// </summary>
        //internal bool IsRemove
        //{
        //    get { return RemoveGlobalVersion > GlobalVersion; }
        //}
        ///// <summary>
        ///// 默认为 true 表示字段删除前用 public 修饰
        ///// </summary>
        //public bool IsRemovePublic = true;
#if !AOT
        /// <summary>
        /// Are there any members that adopt JSON serialization
        /// 是否存在部分成员采用 JSON 序列化
        /// </summary>
        public bool IsJsonMember;
        /// <summary>
        /// Are there any members that adopt JSON serialization
        /// 是否存在部分成员采用 JSON 序列化
        /// </summary>
        internal virtual bool GetIsJsonMember
        {
            get { return IsJsonMember; }
        }
#endif

        /// <summary>
        /// Default empty configuration
        /// 默认空配置
        /// </summary>
        internal static readonly BinarySerializeMemberAttribute Null = new BinarySerializeMemberAttribute();
    }
}
