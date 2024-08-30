using System;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 自定义会话对象
    /// </summary>
    internal sealed class CommandServerSessionObject
    {
        internal int Value;
        internal int Ref;
        internal long Out;
        internal long Xor(int value, ref int refValue, out long outValue)
        {
            this.Value = value;
            return Xor(ref refValue, out outValue);
        }
        internal long Xor(int value, ref int refValue)
        {
            this.Value = value;
            return Xor(ref refValue);
        }
        internal long Xor(int value, out long outValue)
        {
            this.Value = value;
            return Xor(out outValue);
        }
        internal long Xor(int value)
        {
            this.Value = value;
            return Xor();
        }
        internal long Xor(ref int refValue, out long outValue)
        {
            this.Ref = (refValue ^= 1);
            return Xor(out outValue);
        }
        internal long Xor(ref int refValue)
        {
            this.Ref = (refValue ^= 1);
            return Xor();
        }
        internal long Xor(out long outValue)
        {
            this.Out = outValue = (long)AutoCSer.Random.Default.NextULong();
            return Xor();
        }
        internal long Xor()
        {
            return Value ^ Ref ^ Out;
        }
        internal long Xor(int value, int refValue)
        {
            this.Value = value;
            this.Ref = refValue;
            return Xor();
        }

        internal bool Check(CommandServerSessionObject clientSessionObject)
        {
            if (Value != clientSessionObject.Value)
            {
                return false;
            }
            if (Ref != clientSessionObject.Ref)
            {
                return false;
            }
            if (Out != clientSessionObject.Out)
            {
                return false;
            }
            return true;
        }

        internal Data.ORM.BusinessModel Model;
        internal bool Check(Data.ORM.ModelGeneric value)
        {
            if (value == null) return Model == null;
            return Model != null && BinarySerialize.ModelComparor(value, Model);
        }
    }
}
