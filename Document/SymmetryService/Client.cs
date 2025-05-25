using System;

namespace AutoCSer.Document.SymmetryService
{
    /// <summary>
    /// Test client
    /// </summary>
    internal static class Client
    {
        /// <summary>
        /// Test client
        /// </summary>
        private static readonly AutoCSer.Net.CommandClient<ISymmetryService> commandClient = new AutoCSer.Net.CommandClientConfig<ISymmetryService>
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
        }.CreateSymmetryClient();

        /// <summary>
        /// Client test
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            var client = await commandClient.GetSocketEvent();
            if (client != null)
            {
                Console.WriteLine($"2 + 3 = {await client.InterfaceController.AddAsync(2, 3)}");
                Console.WriteLine($"1 + 2 = {client.InterfaceController.Add(1, 2)}");
                Console.WriteLine("Completed");
            }
        }
    }
}
