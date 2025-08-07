using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// Remote type (for serialization operations)
    /// 远程类型（用于序列化操作）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.JsonSerialize]
#endif
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public partial struct RemoteType : IEquatable<RemoteType>
    {
        /// <summary>
        /// Assembly name
        /// 程序集名称
        /// </summary>
        public string AssemblyName;
        /// <summary>
        /// Type name
        /// </summary>
        public string Name;
        /// <summary>
        /// Remote type (for serialization operations)
        /// 远程类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        internal RemoteType(string assemblyName, string typeName)
        {
            AssemblyName = assemblyName;
            Name = typeName;
        }
        /// <summary>
        /// Set type information
        /// 设置类型信息
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(string assemblyName, string typeName)
        {
            AssemblyName = assemblyName;
            Name = typeName;
        }
        /// <summary>
        /// Remote type (for serialization operations)
        /// 远程类型
        /// </summary>
        /// <param name="type"></param>
        public RemoteType(Type type)
        {
            var name = default(string);
            if (typeNames.TryGetValue(type, out name))
            {
                AssemblyName = string.Empty;
                Name = name;
                return;
            }
            var assemblyName = type.Assembly.FullName;
            if (assemblyName == null) throw new ArgumentNullException($"{nameof(type)}.{nameof(type.Assembly)}.{nameof(type.Assembly.FullName)}");
            name = type.FullName;
            if (name == null) throw new ArgumentNullException($"{nameof(type)}.{nameof(type.FullName)}");
            Name = name;
            AssemblyName = assemblyName;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static implicit operator RemoteType(Type type) { return new RemoteType(type); }
        /// <summary>
        /// Try to get the type
        /// 尝试获取类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="checkType">The default is true, indicating that AutoCSer.Common.Config.CheckRemoteType needs to be called to check the validity of the remote type
        /// 默认为 true 表示需要调用 AutoCSer.Common.Config.CheckRemoteType 检查远程类型的合法性</param>
        /// <returns>Return false on failure</returns>
#if NetStandard21
        public bool TryGet([MaybeNullWhen(false)] out Type type, bool checkType = true)
#else
        public bool TryGet(out Type type, bool checkType = true)
#endif
        {
            bool isArray;
            int index = GetTypeIndex(out isArray);
            if (index >= 0)
            {
                type = FixedTypes[index];
                if (isArray) type = type.MakeArrayType();
                return true;
            }
            var assembly = AssemblyCache.Get(AssemblyName);
            if (assembly != null)
            {
                type = assembly.GetType(Name);
                if (type != null)
                {
                    return !checkType || AutoCSer.Common.Config.CheckRemoteType(type);
                }
            }
            else type = null;
            return false;
        }
        /// <summary>
        /// Get the index of a fixed-type array
        /// 获取固定类型数组索引
        /// </summary>
        /// <param name="isArray"></param>
        /// <returns></returns>
        internal int GetTypeIndex(out bool isArray)
        {
            isArray = false;
            if (string.IsNullOrEmpty(AssemblyName) && Name.Length == 1)
            {
                int index = Name[0];
                if (index >= arrayChar)
                {
                    index -= arrayChar;
                    isArray = true;
                }
                index -= 0x23;
                if ((uint)index < FixedTypes.Length) return index;
            }
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RemoteType other)
        {
            return Name == other.Name && AssemblyName == other.AssemblyName;
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<RemoteType>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((SubString)(Name ?? string.Empty)).GetHashCode() ^ ((SubString)(AssemblyName ?? string.Empty)).GetHashCode();
        }

        /// <summary>
        /// Fixed type maximum quantity
        /// 固定类型最大数量 46
        /// </summary>
        private const int arrayChar = (0x7f + 0x23) >> 1;
        /// <summary>
        /// Fixed type collection
        /// 固定类型集合
        /// </summary>
        internal static readonly Type[] FixedTypes = new Type[] 
        {
            typeof(bool), typeof(bool?), typeof(byte), typeof(byte?), typeof(sbyte), typeof(sbyte?), typeof(short), typeof(short?),
            typeof(ushort), typeof(ushort?), typeof(int), typeof(int?), typeof(uint), typeof(uint?), typeof(long), typeof(long?),
            typeof(ulong), typeof(ulong?), typeof(float), typeof(float?), typeof(double), typeof(double?), typeof(decimal), typeof(decimal?),
            typeof(char), typeof(char?), typeof(string), typeof(object), typeof(DateTime), typeof(TimeSpan), typeof(Guid), typeof(Type),
            typeof(System.Numerics.Vector2), typeof(System.Numerics.Vector3), typeof(System.Numerics.Vector4), typeof(System.Numerics.Complex), typeof(System.Numerics.Plane), typeof(System.Numerics.Quaternion), typeof(System.Numerics.Matrix3x2), typeof(System.Numerics.Matrix4x4),
            typeof(Half), typeof(Int128), typeof(UInt128)
        };
#if AOT
        /// <summary>
        /// Fixed type collection
        /// 固定类型集合
        /// </summary>
        private static readonly Type[] fixedArrayTypes = new Type[]
        {
            typeof(bool[]), typeof(bool?[]), typeof(byte[]), typeof(byte?[]), typeof(sbyte[]), typeof(sbyte?[]), typeof(short[]), typeof(short?[]),
            typeof(ushort[]), typeof(ushort?[]), typeof(int[]), typeof(int?[]), typeof(uint[]), typeof(uint?[]), typeof(long[]), typeof(long?[]),
            typeof(ulong[]), typeof(ulong?[]), typeof(float[]), typeof(float?[]), typeof(double[]), typeof(double?[]), typeof(decimal[]), typeof(decimal?[]),
            typeof(char[]), typeof(char?[]), typeof(string[]), typeof(object[]), typeof(DateTime[]), typeof(TimeSpan[]), typeof(Guid[]), typeof(Type[]),
            typeof(System.Numerics.Vector2[]), typeof(System.Numerics.Vector3[]), typeof(System.Numerics.Vector4[]), typeof(System.Numerics.Complex[]), typeof(System.Numerics.Plane[]), typeof(System.Numerics.Quaternion[]), typeof(System.Numerics.Matrix3x2[]), typeof(System.Numerics.Matrix4x4[]),
            typeof(Half[]), typeof(Int128[]), typeof(UInt128[])
        };
#endif
        /// <summary>
        /// Fixed type name collection
        /// 固定类型名称集合
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, string> typeNames;
        unsafe static RemoteType()
        {
            typeNames = AutoCSer.DictionaryCreator.CreateHashObject<System.Type, string>();
            char typeIndex = (char)0x23;
#if AOT
            int arrayIndex = 0;
#endif
            foreach(Type type in FixedTypes)
            {
                typeNames.Add(type, typeIndex.ToString());
#if AOT
                typeNames.Add(fixedArrayTypes[arrayIndex++], ((char)(typeIndex + arrayChar)).ToString());
#else
                typeNames.Add(type.MakeArrayType(), ((char)(typeIndex + arrayChar)).ToString());
#endif
                if (++typeIndex == arrayChar) break;
            }
        }
    }
}
