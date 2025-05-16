using System;
using System.Numerics;
/*Vector2;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref Vector2 value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="value"></param>
        public void JsonDeserialize(ref Vector2 value)
        {
            if (IsBinaryMix)
            {
                if (*(byte*)Current == (byte)AutoCSer.Json.BinaryMixTypeEnum.Vector2)
                {
                    value = *(Vector2*)((byte*)Current + sizeof(ushort));
                    Current += (sizeof(ushort) + sizeof(Vector2)) >> 1;
                }
                else State = AutoCSer.Json.DeserializeStateEnum.NotVector2;
                return;
            }
            SerializeVector2 serializeVector2 = default(SerializeVector2);
            AutoCSer.Json.TypeDeserializer<SerializeVector2>.DefaultDeserializer(this, ref serializeVector2);
            value = new Vector2Union { SerializeValue = serializeVector2 }.Vector2;
        }
    }
}
