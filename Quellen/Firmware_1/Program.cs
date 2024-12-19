using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace Firmware_1
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
            // Generator für Zufallszahlen anlegen
            var random = new Random();

            // Access Token für Gerät definieren
            var token = "s833bD55HhzFbCjwxvLq";

            // HTTP-Client erzeugen
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://192.168.0.102:8080");

            // Interval-Attribute abfragen
            var response = await client.GetAsync($"/api/v1/{token}/attributes?sharedKeys=interval");
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);

            // Inhalt der HTTP-Antwort auslesen
            var text = await response.Content.ReadAsStringAsync();
            Console.WriteLine(text);

            // Inhalt in ein JSON-Objekt parsen
            var body = JsonObject.Parse(text);

            // Interval-Wert aus dem JSON-Objekt auslesen
            var interval = body?["shared"]?["interval"]?.GetValue<int>();
            Console.WriteLine(interval);

            // Datentyp definieren
            var type = new MediaTypeHeaderValue("application/json");

            // Telemetriedaten in einer Endlosschleife senden
            while (true)
            {
                // Telemetrie-Datensatz erzeugen
                body = new JsonObject();
                body["temperature"] = (random.NextDouble() - 0.25) * 20;
                body["humidity"] = random.NextDouble() * 20;
                body["active"] = random.NextDouble() < 0.5;
                body["state"] = random.NextDouble() < 0.5 ? "alive" : "not alive";

                // Inhalt der HTTP-Nachricht festlegen
                var content = new StringContent(body.ToJsonString(), type);

                // HTTP-Anfrage verschicken und -Antwort erhalten
                response = await client.PostAsync($"/api/v1/{token}/telemetry", content);
                Console.WriteLine(response.StatusCode);

                // Programm entsprechend dem konfigurierten Intervall warten lassen
                Thread.Sleep(interval == null ? 1000 : (int)interval);
            }
        }
    }
}
