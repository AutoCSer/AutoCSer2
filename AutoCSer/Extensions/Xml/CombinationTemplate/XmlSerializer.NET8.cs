using System;
using System.Numerics;
/*Vector2;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void XmlSerialize(Vector2 value)
        {
            AutoCSer.Xml.TypeSerializer<AutoCSer.Extensions.SerializeVector2>.Serialize(this, new AutoCSer.Extensions.Vector2Union { Vector2 = value }.SerializeValue);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, Vector2 value)
        {
            serializer.XmlSerialize(value);
        }
    }
}
