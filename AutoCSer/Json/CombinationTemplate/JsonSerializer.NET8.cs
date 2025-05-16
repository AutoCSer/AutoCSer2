using System;
using System.Numerics;
/*Vector2;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, Vector2 value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(Vector2 value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Vector2));
                if (write != null)
                {
                    *(ushort*)write = (byte)AutoCSer.Json.BinaryMixTypeEnum.Vector2;
                    *(Vector2*)(write + sizeof(ushort)) = value;
                }
            }
            else AutoCSer.Json.TypeSerializer<SerializeVector2>.Serialize(this, new Vector2Union { Vector2 = value }.SerializeValue);
        }
    }
}
