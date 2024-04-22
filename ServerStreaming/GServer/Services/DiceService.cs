using Grpc.Core;

namespace GServer.Services;

public class DiceService : DiceResultStreaming.DiceResultStreamingBase
{
   private static Random _random = new();
   public override async Task GetRandom(RandomRequest request, IServerStreamWriter<RandomReply> responseStream, ServerCallContext context)
   {
      for (var i = 0; i < request.Results; i++)
      {
         var result = _random.Next(1, request.CubeWalls);
         await responseStream.WriteAsync(new RandomReply() { Result = result });
         await Task.Delay(1000);
      }
      
   }
}