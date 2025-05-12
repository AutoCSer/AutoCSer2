using Grpc.Core;

namespace GrpcServicePerformance.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<AddReply> Add(AddRequest request, ServerCallContext context)
        {
            return Task.FromResult(new AddReply
            {
                Result = request.Left + request.Right
            });
        }
    }
}
