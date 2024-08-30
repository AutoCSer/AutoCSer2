using System;

namespace AutoCSer.ORM.MSSQL
{
    /// <summary>
    /// decimal 小数位数
    /// </summary>
    [CustomColumn(NameConcatType = CustomColumnNameConcatTypeEnum.Node)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DecimalDigits
    {
        /// <summary>
        /// decimal 有效位数
        /// </summary>
        public byte xprec;
        /// <summary>
        /// decimal 小数位数
        /// </summary>
        public byte xscale;

        /// <summary>
        /// 整数位数
        /// </summary>
        internal int Integer
        {
            get { return xprec - xscale; }
        }
    }
}
