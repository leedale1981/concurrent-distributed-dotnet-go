using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

// List of distributed host addresses and ports
ReadOnlyCollection<string> getTemperatureUri = new List<string>()
{
    "localhost:8000",
    "localhost:8000",
    "localhost:8000",
    "localhost:8001",
    "localhost:8001",
    "localhost:8001",
    "localhost:8002",
    "localhost:8002",
    "localhost:8002",
    "localhost:8003",
    "localhost:8003",
    "localhost:8003",
    "localhost:8004",
    "localhost:8004",
    "localhost:8004",
}.AsReadOnly();

// Creates a task for each URI in the list
var getTemperatureTasks = getTemperatureUri.Select(GetWeatherFromSocketApi);

// Starts all tasks and returns the first task that completes
Task<WeatherModel> firstWeatherTask = await Task.WhenAny(getTemperatureTasks.ToArray());

// Gets the value from the first completed task
WeatherModel firstWeather = await firstWeatherTask;

// Outputs the temperature result for the first completed task
Console.WriteLine($"First temperature received: {firstWeather.Temperature}");

// Task that encapsulates the call to the server.
static async Task<WeatherModel> GetWeatherFromSocketApi(string uri)
{
    IPHostEntry localhost = await Dns.GetHostEntryAsync(uri.Split(':')[0]);
    IPAddress ipAddress = localhost.AddressList[1];
    IPEndPoint ipEndPoint = new(ipAddress, int.Parse(uri.Split(':')[1]));
    using Socket client = new(
        ipEndPoint.AddressFamily, 
        SocketType.Stream, 
        ProtocolType.Tcp);
    await client.ConnectAsync(ipEndPoint);

    var buffer = new byte[1_024];
    var received = await client.ReceiveAsync(buffer, SocketFlags.None);

    if (received > 0)
    {
        var model = new WeatherModel(buffer[0]);
        return model;
    }

    return new WeatherModel(0);
}

public record WeatherModel(int Temperature);