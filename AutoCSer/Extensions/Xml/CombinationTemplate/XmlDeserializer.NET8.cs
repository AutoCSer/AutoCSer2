using System;
using System.Numerics;
/*Vector2;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">data</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref Vector2 value)
        {
            serializer.XmlDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value">data</param>
        public void XmlDeserialize(ref Vector2 value)
        {
            AutoCSer.Extensions.SerializeVector2 serializeVector2 = default(AutoCSer.Extensions.SerializeVector2);
            AutoCSer.Xml.TypeDeserializer<AutoCSer.Extensions.SerializeVector2>.DefaultDeserializer(this, ref serializeVector2);
            value = new AutoCSer.Extensions.Vector2Union { SerializeValue = serializeVector2 }.Vector2;
        }
    }
}
