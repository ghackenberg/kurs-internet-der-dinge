using MQTTnet;
using MQTTnet.Client;
using System.Text.Json.Nodes;

namespace Firmware_2
{
    internal class Program
    {
        // Hauptroutine
        static void Main(string[] args)
        {
            Run().Wait();
        }
        // Asynchrone Laufroutine
        static async Task Run()
        {
            var random = new Random();
            var token = "s833bD55HhzFbCjwxvLq";
            var host = "localhost";
            var port = 1883;

            var factory = new MqttFactory();

            using (var client = factory.CreateMqttClient())
            {
                Console.WriteLine("Verbindung mit MQTT Broker wird aufgebaut");

                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(host, port)
                    .WithCredentials(token)
                    .Build();

                await client.ConnectAsync(options);

                Console.WriteLine("Mit MQTT Broker verbunden");

                for (var i = 0; i < 10; i++)
                {
                    var topic = "v1/devices/me/telemetry";

                    var payload = new JsonObject();
                    payload["temperature"] = (random.NextDouble() - 0.25) * 20;
                    payload["humidity"] = random.NextDouble() * 20;
                    payload["active"] = random.NextDouble() < 0.5;
                    payload["state"] = random.NextDouble() < 0.5 ? "alive" : "not alive";

                    await client.PublishStringAsync(topic, payload.ToJsonString());

                    Thread.Sleep(1000);
                }

                await client.DisconnectAsync();

                Console.WriteLine("Verbindung zu MQTT Broker wieder geschlossen");
            }
        }
    }
}
