using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 业务模型定义
    /// </summary>
    public class BusinessModel : ModelGeneric<BusinessModelAssociated>
    {
        public int Other;
    }
}
