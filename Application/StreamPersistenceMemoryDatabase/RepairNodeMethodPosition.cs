﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 修复节点方法信息 与 文件流持久化位置
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct RepairNodeMethodPosition : AutoCSer.BinarySerialize.ICustomSerialize<RepairNodeMethodPosition>
    {
        /// <summary>
        /// 文件流持久化位置
        /// </summary>
        internal long Position;
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        internal RepairNodeMethod RepairNodeMethod;
        /// <summary>
        /// 文件流持久化位置
        /// </summary>
        /// <param name="position">文件流持久化位置</param>
        internal RepairNodeMethodPosition(long position)
        {
            Position = position;
            RepairNodeMethod = null;
        }
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        /// <param name="repairNodeMethod"></param>
        internal RepairNodeMethodPosition(RepairNodeMethod repairNodeMethod)
        {
            RepairNodeMethod = repairNodeMethod;
            Position = 0;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<RepairNodeMethodPosition>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            if (RepairNodeMethod == null)
            {
                serializer.Stream.Write(int.MinValue);
                serializer.Stream.Write(Position);
            }
            else RepairNodeMethod.Serialize(serializer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<RepairNodeMethodPosition>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            int state;
            if (deserializer.Read(out state))
            {
                if (state < 0) deserializer.Read(out Position);
                else
                {
                    RepairNodeMethod = new RepairNodeMethod();
                    RepairNodeMethod.Deserialize(deserializer, state);
                }
            }
        }
    }
}