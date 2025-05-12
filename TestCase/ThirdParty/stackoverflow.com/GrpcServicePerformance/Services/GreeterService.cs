using System;

namespace GrpcServicePerformance.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<AddReply> Add(AddRequest request, Grpc.Core.ServerCallContext context)
        {
            return Task.FromResult(new AddReply
            {
                Result = request.Left + request.Right
            });
        }
    }
}
