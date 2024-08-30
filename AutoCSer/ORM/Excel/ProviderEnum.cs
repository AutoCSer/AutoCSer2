using System;

namespace AutoCSer.ORM.Excel
{
    /// <summary>
    /// Excel 接口类型
    /// </summary>
    public enum ProviderEnum : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Ace12,
        ///// <summary>
        ///// 
        ///// </summary>
        //Ace16,
        /// <summary>
        /// 只能操作Excel2007之前的.xls文件
        /// </summary>
        Jet4,
    }
}
