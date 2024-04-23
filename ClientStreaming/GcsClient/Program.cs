// See https://aka.ms/new-console-template for more information

using GcsServer;
using Grpc.Core;
using Grpc.Net.Client;

Console.WriteLine("Press any key to start");
Console.ReadKey();
Console.WriteLine();

var channel = GrpcChannel.ForAddress("https://localhost:7224");
var client = new ThermometerService.ThermometerServiceClient(channel);

try
{
    var call = client.StreamTemperatures();
    var random = new Random();
    for (int i = 0; i < 10; i++)
    {
        await call.RequestStream.WriteAsync(new TemperatureReading()
        {
            RoomId = random.Next(1, 5),
            Temperature = (float)random.NextDouble() * 20 + 10
        });
        await Task.Delay(2000);
    }

    await call.RequestStream.CompleteAsync();

    var response = await call.ResponseAsync;
    Console.WriteLine(
        $"Received temperature summary: Total readings: {response.TotalReadings}, Average temperature: {response.AverageTemperature:F2}°C");
}
catch (RpcException exception)
{
    Console.WriteLine($"RPC failed: {exception}");
}
await channel.ShutdownAsync();