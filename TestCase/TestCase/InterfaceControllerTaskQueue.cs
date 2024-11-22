using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    public interface IInterfaceControllerTaskQueue
    {
        AutoCSer.Threading.InterfaceControllerTaskQueueNode Call();
        AutoCSer.Threading.InterfaceControllerTaskQueueNode CallParameter(int value);
        AutoCSer.Threading.InterfaceControllerTaskQueueNode CallParameters(int left, int right);
        AutoCSer.Threading.InterfaceControllerTaskQueueNode<int> Return();
        AutoCSer.Threading.InterfaceControllerTaskQueueNode<int> ReturnParameter(int value);
        AutoCSer.Threading.InterfaceControllerTaskQueueNode<int> ReturnParameters(int left, int right);
    }
    public class InterfaceControllerTaskQueue
    {
        private int value;
        public void Call()
        {
            value = 1;
        }
        public void CallParameter(int value)
        {
            this.value = value;
        }
        public void CallParameters(int left, int right)
        {
            this.value = left + right;
        }
        public int Return()
        {
            return value;
        }
        public int ReturnParameter(int value)
        {
            return this.value = value;
        }
        public int ReturnParameters(int left, int right)
        {
            return this.value = left + right;
        }

        /// <summary>
        /// 反向命令服务测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static async Task<bool> TestCase()
        {
            using (AutoCSer.Threading.InterfaceControllerTaskQueue queue = new AutoCSer.Threading.InterfaceControllerTaskQueue())
            {
                InterfaceControllerTaskQueue controller = new InterfaceControllerTaskQueue();
                IInterfaceControllerTaskQueue caller = queue.CreateController<IInterfaceControllerTaskQueue, InterfaceControllerTaskQueue>(controller);

                CommandClientReturnValue returnType = await caller.Call();
                if (!returnType.IsSuccess || controller.value != 1)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                returnType = await caller.CallParameter(2);
                if (!returnType.IsSuccess || controller.value != 2)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                returnType = await caller.CallParameters(1, 2);
                if (!returnType.IsSuccess || controller.value != 3)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                CommandClientReturnValue<int> returnValue = await caller.Return();
                if (!returnValue.IsSuccess || returnValue.Value != controller.value)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                returnValue = await caller.ReturnParameter(4);
                if (!returnValue.IsSuccess || controller.value != 4 || returnValue.Value != controller.value)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                returnValue = await caller.ReturnParameters(2, 3);
                if (!returnValue.IsSuccess || controller.value != 5 || returnValue.Value != controller.value)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
