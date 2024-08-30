using AutoCSer.Net;
using AutoCSer.ORM;
using AutoCSer.TestCase.CommonModel.TableModel;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Client.Initialize();

            CommandClientReturnValue<int> count = await Client.AutoIdentityModelClient.CustomColumnQuery();
            if (!count.IsSuccess)
            {
                breakpoint();
                return;
            }
            count = await Client.FieldModelClient.CustomColumnQuery();
            if (!count.IsSuccess)
            {
                breakpoint();
                return;
            }
            count = await Client.PropertyModelClient.CustomColumnQuery();
            if (!count.IsSuccess)
            {
                breakpoint();
                return;
            }
            count = await Client.CustomColumnFieldModelClient.CustomColumnQuery();
            if (!count.IsSuccess)
            {
                breakpoint();
                return;
            }
            count = await Client.CustomColumnPropertyModelClient.CustomColumnQuery();
            if (!count.IsSuccess)
            {
                breakpoint();
                return;
            }
            do
            {
                if (!await autoIdentityModel()) { ConsoleWriteQueue.WriteLine("AutoIdentityModel ERROR", ConsoleColor.Red); break; }
                if (!await primaryKeyModel(Client.FieldModelClient, p => p.Key, (p, k) => p.Key = k)) { ConsoleWriteQueue.WriteLine("FieldModel ERROR", ConsoleColor.Red); break; }
                if (!await primaryKeyModel(Client.PropertyModelClient, p => p.Key, (p, k) => p.Key = k)) { ConsoleWriteQueue.WriteLine("PropertyModel ERROR", ConsoleColor.Red); break; }
                if (!await primaryKeyModel(Client.CustomColumnFieldModelClient, p => p.Key, (p, k) => p.Key = k)) { ConsoleWriteQueue.WriteLine("CustomColumnFieldModel ERROR", ConsoleColor.Red); break; }
                if (!await primaryKeyModel(Client.CustomColumnPropertyModelClient, p => p.Key, (p, k) => p.Key = k)) { ConsoleWriteQueue.WriteLine("CustomColumnPropertyModel ERROR", ConsoleColor.Red); break; }
                Console.Write('.');
            }
            while (true);
        }
        private static readonly RandomObjectConfig randomObjectConfig = new RandomObjectConfig
        {
            MinDateTime = DateTimeAttribute.MinSmallDateTime,
            MaxDateTime = DateTimeAttribute.MaxSmallDateTime.AddSeconds(-1),
            MinDecimal = -214748.36M,
            MaxDecimal = 214748.36M,
            MaxArraySize = 5,
            IsAscii = true,
            IsParseFloat = true
        };
        private static async Task<bool> autoIdentityModel()
        {
            AutoIdentityModel model = AutoCSer.RandomObject.Creator<AutoIdentityModel>.CreateNotNull(randomObjectConfig);
            CommandClientReturnValue<long> identity = await Client.AutoIdentityModelClient.Insert(model);
            if (!identity.IsSuccess) return breakpoint();
            CommandClientReturnValue<AutoIdentityModel> returnValue = await Client.AutoIdentityModelClient.Get(model.Identity = identity.Value);
            if (!returnValue.IsSuccess) return breakpoint();// || !AutoCSer.FieldEquals.Comparor<AutoIdentityModel>.Equals(model, returnValue.Value)
            AutoCSer.RandomObject.Creator.Create(ref model, randomObjectConfig);
            CommandClientReturnValue<bool> isUpdate = await Client.AutoIdentityModelClient.Update(model);
            if (!isUpdate.IsSuccess || !isUpdate.Value) return breakpoint();
            returnValue = await Client.AutoIdentityModelClient.Get(model.Identity);
            if (!returnValue.IsSuccess) return breakpoint();// || !AutoCSer.FieldEquals.Comparor<AutoIdentityModel>.Equals(model, returnValue.Value)
            CommandClientReturnValue<bool> isDelete = await Client.AutoIdentityModelClient.DeleteKey(model.Identity);
            if (!isDelete.IsSuccess || !isDelete.Value) return breakpoint();
            returnValue = await Client.AutoIdentityModelClient.Get(model.Identity);
            if (!returnValue.IsSuccess || returnValue.Value != null) return breakpoint();
            return true;
        }
        private static async Task<bool> primaryKeyModel<T, KT>(IPrimaryKeyClient<T, KT> client, Func<T, KT> getKey, Action<T, KT> setKey)
            where T : class
            where KT : IEquatable<KT>
        {
            T model = AutoCSer.RandomObject.Creator<T>.CreateNotNull(randomObjectConfig);
            CommandClientReturnValue<bool> isInsert = await client.Insert(model);
            if (!isInsert.IsSuccess || !isInsert.Value) return breakpoint();
            CommandClientReturnValue<T> returnValue = await client.Get(getKey(model));
            if (!returnValue.IsSuccess) return breakpoint();// || !AutoCSer.FieldEquals.Comparor<T>.Equals(model, returnValue.Value)
            AutoCSer.RandomObject.Creator.Create(ref model, randomObjectConfig);
            setKey(model, getKey(returnValue.Value));
            CommandClientReturnValue<bool> isUpdate = await client.Update(model);
            if (!isUpdate.IsSuccess || !isUpdate.Value) return breakpoint();
            returnValue = await client.Get(getKey(model));
            if (!returnValue.IsSuccess) return breakpoint();// || !AutoCSer.FieldEquals.Comparor<T>.Equals(model, returnValue.Value)
            CommandClientReturnValue<bool> isDelete = await client.DeleteKey(getKey(model));
            if (!isDelete.IsSuccess || !isDelete.Value) return breakpoint();
            returnValue = await client.Get(getKey(model));
            if (!returnValue.IsSuccess || returnValue.Value != null) return breakpoint();
            return true;
        }
        /// <summary>
        /// 测试错误断点
        /// </summary>
        /// <returns></returns>
        private static bool breakpoint()
        {
            return false;
        }
    }
}
