// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;
using GServer;

Console.WriteLine("Press any key to start");
Console.ReadKey();

var channel = GrpcChannel.ForAddress("https://localhost:7160");
var client = new DiceResultStreaming.DiceResultStreamingClient(channel);
using var reply = client.GetRandom(new RandomRequest(){ CubeWalls = 6, Results = 100 });
while (await reply.ResponseStream.MoveNext(default))
{
    var roll = reply.ResponseStream.Current;
    Console.WriteLine("Dice roll result: " + roll.Result);
}