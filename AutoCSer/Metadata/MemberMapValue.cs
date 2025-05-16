using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图对象绑定
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if NetStandard21
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type?[] { null })]
#else
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type[] { null })]
#endif
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct MemberMapValue<T> : AutoCSer.BinarySerialize.ICustomSerialize<MemberMapValue<T>>, AutoCSer.Json.ICustomSerialize<MemberMapValue<T>>
    {
        /// <summary>
        /// 成员位图
        /// </summary>
#if NetStandard21
        public MemberMap<T>? MemberMap;
#else
        public MemberMap<T> MemberMap;
#endif
        /// <summary>
        /// 目标数据
        /// </summary>
#if NetStandard21
        public T? Value;
#else
        public T Value;
#endif

        /// <summary>
        /// 成员位图对象绑定自定义二进制序列化
        /// </summary>
        /// <param name="serializer"></param>
        public void Serialize(BinarySerializer serializer)
        {
            if (MemberMap == null || MemberMap.MemberMapData.IsDefault || Value == null) serializer.CustomSerialize(ref Value);
            else
            {
                BinarySerializeMemberMap serializeMemberMap = serializer.GetCustomMemberMap(MemberMap);
                try
                {
                    serializer.CustomSerialize(ref Value);
                }
                finally { serializer.SetCustomMemberMap(ref serializeMemberMap); }
            }
        }
        /// <summary>
        /// 成员位图对象绑定自定义二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<MemberMapValue<T>>.Deserialize(BinaryDeserializer deserializer)
        {
            if (MemberMap == null)
            {
                var oldMemberMapType = deserializer.SetCustomMemberMapType(typeof(T));
                try
                {
                    deserializer.CustomDeserialize(ref Value);
                }
                finally
                {
#if NetStandard21
                    MemberMap = (MemberMap<T>?)deserializer.GetCustomMemberMap(oldMemberMapType);
#else
                    MemberMap = (MemberMap<T>)deserializer.GetCustomMemberMap(oldMemberMapType);
#endif
                }
            }
            else
            {
                var oldMemberMap = deserializer.SetCustomMemberMap(MemberMap);
                try
                {
                    deserializer.CustomDeserialize(ref Value);
                }
                finally
                {
#if NetStandard21
                    MemberMap = (MemberMap<T>?)deserializer.SetCustomMemberMap(oldMemberMap);
#else
                    MemberMap = (MemberMap<T>)deserializer.SetCustomMemberMap(oldMemberMap);
#endif
                }
            }
        }
        /// <summary>
        /// 成员位图对象绑定自定义 JSON 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.Json.ICustomSerialize<MemberMapValue<T>>.Serialize(JsonSerializer serializer)
        {
            if (MemberMap == null || MemberMap.MemberMapData.IsDefault || Value == null) serializer.CustomSerialize(Value);
            else
            {
                var memberMap = serializer.SetCustomMemberMap(MemberMap);
                try
                {
                    serializer.CustomSerialize(Value);
                }
                finally { serializer.SetCustomMemberMap(memberMap); }
            }
        }
        /// <summary>
        /// 成员位图对象绑定自定义 JSON 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.Json.ICustomSerialize<MemberMapValue<T>>.Deserialize(JsonDeserializer deserializer)
        {
            if (MemberMap == null)
            {
                MemberMap = new MemberMap<T>();
                MemberMap.MemberMapData.Empty();
            }
            deserializer.MemberMap = MemberMap;
            deserializer.CustomDeserialize(ref Value);
        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator MemberMapValue<T>(T value)
        {
            return new MemberMapValue<T> { Value = value };
        }
    }
    /// <summary>
    /// 成员位图对象绑定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="VT"></typeparam>
#if NetStandard21
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type?[] { null })]
#else
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type[] { null })]
#endif
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct MemberMapValue<T, VT> : AutoCSer.BinarySerialize.ICustomSerialize<MemberMapValue<T, VT>>, AutoCSer.Json.ICustomSerialize<MemberMapValue<T, VT>>
        where VT : class, T
    {
        /// <summary>
        /// 成员位图
        /// </summary>
#if NetStandard21
        public MemberMap<T>? MemberMap;
#else
        public MemberMap<T> MemberMap;
#endif
        /// <summary>
        /// 目标数据
        /// </summary>
#if NetStandard21
        public VT? Value;
#else
        public VT Value;
#endif

        /// <summary>
        /// 成员位图对象绑定自定义二进制序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<MemberMapValue<T, VT>>.Serialize(BinarySerializer serializer)
        {
            if (MemberMap == null || MemberMap.MemberMapData.IsDefault || Value == null) serializer.CustomSerialize<T>(Value);
            else
            {

                BinarySerializeMemberMap serializeMemberMap = serializer.GetCustomMemberMap(MemberMap);
                try
                {
                    serializer.CustomSerialize<T>(Value);
                }
                finally { serializer.SetCustomMemberMap(ref serializeMemberMap); }
            }
        }
        /// <summary>
        /// 成员位图对象绑定自定义二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<MemberMapValue<T, VT>>.Deserialize(BinaryDeserializer deserializer)
        {
            if (Value == null) Value = DefaultConstructor<VT>.Constructor();
#if NetStandard21
            T? value = (T?)Value;
#else
            var value = (T)Value;
#endif
            if (MemberMap == null)
            {
                var oldMemberMapType = deserializer.SetCustomMemberMapType(typeof(T));
                try
                {
                    deserializer.CustomDeserialize(ref value);
                }
                finally
                {
#if NetStandard21
                    MemberMap = (MemberMap<T>?)deserializer.GetCustomMemberMap(oldMemberMapType);
#else
                    MemberMap = (MemberMap<T>)deserializer.GetCustomMemberMap(oldMemberMapType);
#endif
                }
            }
            else
            {
                var oldMemberMap = deserializer.SetCustomMemberMap(MemberMap);
                try
                {
                    deserializer.CustomDeserialize(ref value);
                }
                finally
                {
#if NetStandard21
                    MemberMap = (MemberMap<T>?)deserializer.SetCustomMemberMap(oldMemberMap);
#else
                    MemberMap = (MemberMap<T>)deserializer.SetCustomMemberMap(oldMemberMap);
#endif
                }
            }
        }
        /// <summary>
        /// 成员位图对象绑定自定义 JSON 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.Json.ICustomSerialize<MemberMapValue<T, VT>>.Serialize(JsonSerializer serializer)
        {
            if (MemberMap == null || MemberMap.MemberMapData.IsDefault || Value == null) serializer.CustomSerialize<T>(Value);
            else
            {
                var memberMap = serializer.SetCustomMemberMap(MemberMap);
                try
                {
                    serializer.CustomSerialize<T>(Value);
                }
                finally { serializer.SetCustomMemberMap(memberMap); }
            }
        }
        /// <summary>
        /// 成员位图对象绑定自定义 JSON 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.Json.ICustomSerialize<MemberMapValue<T, VT>>.Deserialize(JsonDeserializer deserializer)
        {
            if (MemberMap == null)
            {
                MemberMap = new MemberMap<T>();
                MemberMap.MemberMapData.Empty();
            }
            deserializer.MemberMap = MemberMap;
            if (Value == null) Value = DefaultConstructor<VT>.Constructor();
#if NetStandard21
            T? value = (T?)Value;
#else
            var value = (T)Value;
#endif
            deserializer.CustomDeserialize(ref value);
        }
    }
}
