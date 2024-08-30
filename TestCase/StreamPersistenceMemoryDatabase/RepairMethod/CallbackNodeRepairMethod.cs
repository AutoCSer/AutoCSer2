using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 修复方法测试
    /// </summary>
    public static class CallbackNodeRepairMethod
    {
        [ServerMethod(MethodIndex = (int)CallbackNodeMethodEnum.SetValueCallbackPersistence)]
        public static void SetValueCallbackPersistenceV1(CallbackNode node, int value, MethodCallback<int> callback)
        {
            Console.WriteLine($"{AutoCSer.Threading.SecondTimer.Now.toString()} : {nameof(CallbackNodeRepairMethod)}.{nameof(SetValueCallbackPersistenceV1)}({value})");
            node.Value = value;
            callback.Callback(value + 1);
        }
        [ServerMethod(MethodIndex = (int)CallbackNodeMethodEnum.BindNodeMethodTest, IsPersistence = false)]
        public static void BindNodeMethodTestV1(CallbackNode node, int value, MethodCallback<int> callback)
        {
            Console.WriteLine($"{AutoCSer.Threading.SecondTimer.Now.toString()} : {nameof(CallbackNodeRepairMethod)}.{nameof(BindNodeMethodTestV1)}({value})");
            node.Value = value;
            callback.Callback(value + 1);
        }
    }
}
