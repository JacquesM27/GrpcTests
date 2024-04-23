using Grpc.Core;

namespace GcsServer.Services;

public class TemperatureService : ThermometerService.ThermometerServiceBase
{
    public override async Task<TemperatureSummary> StreamTemperatures(IAsyncStreamReader<TemperatureReading> requestStream, ServerCallContext context)
    {
        var totalReadings = 0;
        float totalTemperature = 0;

        await foreach (var reading in requestStream.ReadAllAsync())
        {
            totalReadings++;
            totalTemperature += reading.Temperature;
            Console.WriteLine($"Received temperature reading: Room ID {reading.RoomId}, Temperature: {reading.Temperature}");
        }

        var averageTemperature = totalTemperature / totalReadings;
        Console.WriteLine($"Reading temperature completed: total readings {totalReadings}, avg temperature: {averageTemperature}");

        return new TemperatureSummary()
        {
            TotalReadings = totalReadings,
            AverageTemperature = averageTemperature
        };
    }
}