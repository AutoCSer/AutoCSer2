using System;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// Custom session object
    /// 自定义会话对象
    /// </summary>
    public sealed class CommandServerSessionObject
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
        internal bool CheckXor(string value)
        {
#if AOT
            return !string.IsNullOrEmpty(value);
#else
            return value == Xor().ToString();
#endif
        }
        internal bool CheckXor(string value, int index)
        {
#if AOT
            return !string.IsNullOrEmpty(value);
#else
            return value == (Xor() + index).ToString();
#endif
        }
        internal long Xor(int value, int refValue)
        {
            this.Value = value;
            this.Ref = refValue;
            return Xor();
        }
        internal void Set(int value)
        {
            this.Value = value;
        }
        //internal void Set(bool value)
        //{
        //    this.Value = value ? int.MaxValue : int.MinValue;
        //}

        internal bool Check(CommandServerSessionObject clientSessionObject)
        {
#if !AOT
            if (Value != clientSessionObject.Value)
            {
                //Console.WriteLine(Value);
                //Console.WriteLine(clientSessionObject.Value);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (Ref != clientSessionObject.Ref)
            {
                //Console.WriteLine(Ref);
                //Console.WriteLine(clientSessionObject.Ref);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (Out != clientSessionObject.Out)
            {
                //Console.WriteLine(Out);
                //Console.WriteLine(clientSessionObject.Out);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif
            return true;
        }

        internal Data.ORM.BusinessModel Model;
        internal bool Check(Data.ORM.ModelGeneric value)
        {
#if AOT
            return true;
#else
            if (value == null) return Model == null;
            return Model != null && BinarySerialize.ModelComparor(value, Model);
#endif
        }
    }
}
